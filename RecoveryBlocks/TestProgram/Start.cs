using System;
using System.Collections.Generic;
using System.Diagnostics;
using Print = System.Diagnostics.Debug;

namespace RecoveryBlocks
{
    public static class Start
    {
        public const string checkpointFileName = "checkpoint.xml";

        static void Main()
        {
            //testCheckpoint();
            testRecoveryBlockPrime();
        }

        public static void testRecoveryBlockPrime()
        {
            long maxSearch = 100;
            PrimeSearcher primeSearch = new PrimeSearcher();
            Func<PrimeSearcher, long, List<long>> algoFail1 = PrimeSearcher.SearchStatic;
            Func<long, List<long>> algoFail2 = primeSearch.SearchFail;
            Func<long, List<long>> algo1 = primeSearch.Search;
            Func<long, List<long>> algo2 = PrimeSearcher.SearchStatic;

            List<long> primes = algo1(maxSearch);
            Func<bool> checkPrimes = primeSearch.CheckPrimes;
            Console.WriteLine("\n\n\nWithout recovery blocks: ");
            primeSearch.PrintResults();

            primeSearch = new PrimeSearcher();
            RecoveryBlock reco = new RecoveryBlock(checkPrimes);

            //reco.AddAlgorithm(algoFail1);
           // reco.AddAlgorithm(algoFail2);
            reco.AddAlgorithm(algo1);
            reco.AddAlgorithm(algo2);
            reco.AddAlgorithm((Func<long, List<long>>)primeSearch.SearchUnoptimized); //Just a different way to add the algorithm.

            List<long> primes2 = reco.Run<List<long>, PrimeSearcher, long>(maxSearch);
            primeSearch.PrintResults();
            Console.WriteLine("Recovery block run result:\n"+ string.Join(",", primes2.ToArray())+ "\n\n\n");
        }


        public static void testCheckpoint()
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