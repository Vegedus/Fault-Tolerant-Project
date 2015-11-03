using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProgram
{
    public class PrimeSearcher
    {
        public List<long> foundPrimes = new List<long>();
        public long index = 3;
        public Checkpoint<PrimeSearcher> checkpoint;

        public void RunIterations(long amount)
        {
            amount += index;
            for(; index < amount; index++)
            {
                for (int i = 2; i < index; i++)
                {
                    double divided = (double)index / (double)i;
                    if (divided == Math.Floor(divided)) break;
                    else if (i == index - 1) foundPrimes.Add(index);
                }
            }
        }

        public void RunToEnd()
        {
            RunIterations(long.MaxValue-index);
        }

        public void PrintResults()
        {
            Console.WriteLine(string.Join(",", foundPrimes.ToArray()));
        }
    }
}
