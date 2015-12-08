using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace RecoveryBlocks
{
    class Test
    {
        private delegate string SortDelegate(string s);

        public static void TestRecoveryBlockSort()
        {
            String[] testStrings = {
                "the quick brown fox jumps over the lazy dog",
                "Hello there. I am going to sort this string, but there are some problems.",
                "#Testing «ταБЬℓσ»: 1<2 & 4+1>3, now 20% off! E-mail @ (me)"
            };
            ObservableCollection<Delegate> sorters = new ObservableCollection<Delegate>();

            //Three different ways of building the delegates
            sorters.Add((Func<String, String>)Sort.MikkelSort);

            SortDelegate madsSort = Sort.MadsSort;
            sorters.Add(madsSort);

            MethodInfo sortMethod = typeof(Sort).GetMethod("MarcSort");
            Delegate marcSort = Delegate.CreateDelegate(typeof(SortDelegate), sortMethod);
            sorters.Add(marcSort);

            //Print stuff without recovery blocks
            Console.WriteLine("Unsorted:");
            foreach (string unsortedString in testStrings)
            {
                Console.WriteLine(unsortedString);
            }
            foreach (Delegate sorter in sorters)
            {
                Console.WriteLine(sorter.Method.Name + ":");
                foreach (string unsortedString in testStrings)
                {
                    Console.WriteLine(sorter.DynamicInvoke(unsortedString));
                }
            }

            //Recovery block testing
            RecoveryBlock sortReco = new RecoveryBlock(sorters, null);
            sortReco.acceptanceTest = (Func<string, string, bool>)String.Equals;
            sortReco.acceptanceParameters = new List<Object>();
            sortReco.useResultForTest = true;

            char[] charArr = testStrings[0].ToCharArray();
            Array.Sort<char>(charArr);

            sortReco.acceptanceParameters.Add(new String(charArr));

            Console.WriteLine(sortReco.Run<string, string>(testStrings[0]));

            sortReco.acceptanceParameters.Clear();
            sortReco.acceptanceParameters.Add("am are but going Hello I problems. some sort string, there there. this to");
            Console.WriteLine(sortReco.Run<string, string>(testStrings[1]));

            sortReco.acceptanceParameters.Clear();
            sortReco.acceptanceParameters.Add("          !#%&()+,-0112234:<>@ETaeeffgiilmmnnoostw«»αστБЬℓ");
            Console.WriteLine(sortReco.Run<string, string>(testStrings[2]));

        }

        public const string checkpointFileName = "checkpoint.xml";

        public static void TestRecoveryBlockPrime()
        {
            long maxSearch = 100;
            PrimeSearcher primeSearch = new PrimeSearcher();

            Console.WriteLine("\n\n\nWithout recovery blocks: ");
            primeSearch.Search(maxSearch);
            primeSearch.PrintResults();
            Console.WriteLine("");

            primeSearch = new PrimeSearcher();
            Func<PrimeSearcher, long, List<long>> algoFail1 = PrimeSearcher.SearchStatic;
            Func<long, List<long>> algoFail2 = primeSearch.SearchFail;
            Func<long, List<long>> algo1 = primeSearch.Search;
            Func<long, List<long>> algo2 = PrimeSearcher.SearchStatic;
            Func<bool> checkPrimes = primeSearch.CheckPrimes;

            RecoveryBlock reco = new RecoveryBlock(checkPrimes);

            reco.AddAlgorithm(algoFail1);
            reco.AddAlgorithm(algoFail2);
            reco.AddAlgorithm(algo1);
            reco.AddAlgorithm(algo2);
            reco.AddAlgorithm((Func<long, List<long>>)primeSearch.SearchUnoptimized); //Just a different way to add the algorithm.

            List<long> primes2 = reco.Run<List<long>, PrimeSearcher, long>(maxSearch);
            Console.WriteLine("Recovery block run result:\n" + string.Join(",", primes2.ToArray()) + "\n\n");
        }

        public static void TestCheckpoint()
        {
            PrimeSearcher primes = new PrimeSearcher();
            Checkpoint<PrimeSearcher> checkpoint = new Checkpoint<PrimeSearcher>();
            try
            {
                primes = checkpoint.Load();
            }
            catch (Exception) { }
            for (long i = 0; i < 100; i++)
            {
                Stopwatch timer = Stopwatch.StartNew();
                primes.Search(10);
                if (timer.ElapsedMilliseconds > 1000)
                {
                    checkpoint.Save(primes);
                    timer.Restart();
                }
            }
            checkpoint.Save(primes);

            primes.PrintResults();
            TestCheckpoint jk = new TestCheckpoint();
            Checkpoint<TestCheckpoint> checkpoint2 = new Checkpoint<TestCheckpoint>();
        }
    }

    public class TestCheckpoint
    {
        public int a = 0;
        private int b = 1;
        protected int c = 2;
        public TestNestedOBject outerObj = new TestNestedOBject();


    }

    public class TestNestedOBject
    {
        public List<String> list = new List<string>();
        public int[] arr = { 2, 5, 3 };
        public Random obj = new Random();

        public TestNestedOBject()
        {
            list.Add("entry1");
            list.Add("entry2");
        }
        //public FileStream fileStream = new FileStream("checkpoint2.xml", FileMode.Open);
    }
}
