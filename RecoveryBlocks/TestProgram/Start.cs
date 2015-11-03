using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Xml;
using System.Diagnostics;

namespace TestProgram
{
    public static class Start
    {
        public const string checkpointFileName = "checkpoint.xml";

        static void Main()
        {
            PrimeSearcher primes = new PrimeSearcher();
            Checkpoint<PrimeSearcher> checkpoint = new Checkpoint<PrimeSearcher>();
            try
            {
                primes = checkpoint.Load();
            }
            catch (Exception){}
            for (long i = 0; i < 100; i++)
            {
                Stopwatch timer = Stopwatch.StartNew();
                primes.RunIterations(10);
                if(timer.ElapsedMilliseconds > 1000)
                {
                    checkpoint.Save(primes);
                    timer.Restart();
                }               
            }
            checkpoint.Save(primes);


            primes.PrintResults();
            DataKeeper j = new DataKeeper();
            Checkpoint<DataKeeper> checkpoint2 = new Checkpoint<DataKeeper>();

            checkpoint2.Save(j);
                j.a = 123;
            j = (DataKeeper)checkpoint2.Load();

            Console.WriteLine(j.a);
        }
    }

    public class DataKeeper
    {
        public int a = 0;
        private int b = 1;
        protected int c = 2;
        public Dataer outerObj = new Dataer();
    }

    public class Dataer
    {
        public List<String> list = new List<string>();
        public int[] arr = {2,5,3};
        public Random obj = new Random();

        public Dataer()
        {
            list.Add("entry1");
            list.Add("entry2");
        }
        //public FileStream fileStream = new FileStream("checkpoint2.xml", FileMode.Open);
    }
}
