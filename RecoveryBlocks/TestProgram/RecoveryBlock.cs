using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RecoveryBlocks
{
    class RecoveryBlock
    {
        //TODO: How make generic function?
        private ObservableCollection<Delegate> algorithms = new ObservableCollection<Delegate>();
        private Delegate acceptanceTest;

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

        public Return Run<Return, DataObject, Parameter>(Parameter parameters)
        {
            int i = 0;
            foreach (Delegate algorithm in algorithms)
            {
                Return result;

                Checkpoint<DataObject> objectCP = null;
                if (algorithm.Target != null)
                    objectCP = new Checkpoint<DataObject>((DataObject)algorithm.Target);

                Checkpoint<Parameter> parameterCP = new Checkpoint<Parameter>(parameters);

                try
                {
                    result = (Return)algorithm.DynamicInvoke(parameters);
                    //result = algorithm(default(DataClass));
                    if ((bool)acceptanceTest.DynamicInvoke())
                    {
                        Console.WriteLine("Successfully run algorithm(s) using the recovery block framework. " + i +
                            " algorithms failed, algorithm " + algorithm.Method.Name + " being succesful.");
                        return result;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (objectCP != null)
                    objectCP.Load();
                parameters = parameterCP.Load();

                i++;
            }
            throw new Exception("None of the existing algorithms completed correctly, consider improving them or providing more.");
        }

        public void SpeedTest()
        {

        }
    }
    //Func can has max 16 parameters, so should we
}
