using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace RecoveryBlocks
{
    class RecoveryBlock
    {
        //TODO: How make generic function?
        private ObservableCollection<Delegate> algorithms = new ObservableCollection<Delegate>();
        public Delegate acceptanceTest;
        public List<Object> acceptanceParameters = new List<Object>();
        public bool useResultForTest = false;

        public RecoveryBlock(Delegate acceptanceTest)
        {
            this.acceptanceTest = acceptanceTest;
        }

        public RecoveryBlock(ObservableCollection<Delegate> algorithms, Delegate acceptanceTest)
        {
            this.algorithms = algorithms;
            this.acceptanceTest = acceptanceTest;
        }

        public void AddAlgorithm(Delegate algorithm)
        {
            algorithms.Add(algorithm);
        }

        //DataObject is not relevant if we're using only static methods.
        public Return Run<Return, Parameter>(Parameter parameters)
        {
            return Run<Return, Object, Parameter>(parameters);
        }

        public Return Run<Return, DataObject, Parameter>(Parameter parameters)
        {
            int i = 0;
            Checkpoint<DataObject> objectCP = null;

            foreach (Delegate algorithm in algorithms)
            {
                Return result;

                Delegate algorithmLoaded = algorithm;

                if (algorithm.Target != null)
                {
                    if (objectCP != null)
                    {
                        DataObject loadedDataObject = (DataObject)objectCP.Load();
                        if (loadedDataObject.GetType() == algorithm.Target.GetType())
                        {
                            //The original object of instance methods/delegates cannot be modified so a new delegate is made in it's place
                            String algorithmName = algorithm.Method.Name;
                            algorithmLoaded = Delegate.CreateDelegate(algorithm.GetType(), loadedDataObject, algorithmName);
                            String testName = acceptanceTest.Method.Name;
                            acceptanceTest = Delegate.CreateDelegate(acceptanceTest.GetType(), loadedDataObject, testName);
                        }
                    }
                    objectCP = new Checkpoint<DataObject>((DataObject)algorithm.Target);

                }
                Checkpoint<Parameter> parameterCP = new Checkpoint<Parameter>(parameters);

                try
                {
                    //Run the algorithm
                    result = (Return)algorithmLoaded.DynamicInvoke(parameters);

                    List<Object> testParameters = new List<Object>(acceptanceParameters);
                    if (useResultForTest)
                        testParameters.Add(result);
                    if ((bool)acceptanceTest.Method.Invoke(acceptanceTest.Target, testParameters.ToArray()))
                    {
                        Console.WriteLine("\nSuccessfully run algorithm(s) using the recovery block framework. " + i +
                            " algorithms failed, algorithm " + algorithm.Method.Name + " being succesful.");
                        return result;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Algorithm failed with message: " + e.Message + ". Next algorithm running.");
                }
                parameters = parameterCP.Load();

                i++;
            }
            throw new Exception("None of the existing algorithms completed correctly, consider improving them or providing more.");
        }
    }
}
