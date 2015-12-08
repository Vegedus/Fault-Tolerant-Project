using System;
using System.Collections.Generic;
using System.Diagnostics;
using Print = System.Diagnostics.Debug;

namespace RecoveryBlocks
{
    public class PrimeSearcher
    {
        public List<long> foundPrimes = new List<long>();
        public long primeCandidate = 3;

        public List<long> Search(long endNumber)
        {
            endNumber += primeCandidate;
            for (; primeCandidate < endNumber; primeCandidate += 2)
            {
                bool isPrime = true;
                for (int i = 2; i <= Math.Sqrt(primeCandidate); i++)
                {
                    if (primeCandidate % i == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                    foundPrimes.Add(primeCandidate);
            }
            return foundPrimes;
        }

        public List<long> SearchUnoptimized(long endNumber)
        {
            endNumber += primeCandidate;
            for (; primeCandidate < endNumber; primeCandidate++)
            {
                for (int i = 2; i < primeCandidate; i++)
                {
                    double divided = (double)primeCandidate / (double)i;
                    if (i == primeCandidate - 1 && divided == Math.Floor(divided)) foundPrimes.Add(primeCandidate);
                }
            }
            return foundPrimes;
        }

        public static List<long> SearchStatic(long endNumber)
        {
            PrimeSearcher primer = new PrimeSearcher();
            return primer.Search(endNumber);
        }

        public static List<long> SearchStatic(PrimeSearcher primer, long endAmount)
        {
            return primer.Search(endAmount);
        }

        public List<long> SearchFail(long endNumber)
        {
            endNumber += primeCandidate;
            for (; primeCandidate < endNumber; primeCandidate++)
            {
                for (int i = 2; i < primeCandidate; i++)
                {
                    double divided = (double)primeCandidate / (double)i;
                    if (i == primeCandidate - 1 && divided == Math.Floor(divided)) foundPrimes.Add(primeCandidate);
                    if(primeCandidate>80)
                        throw new Exception("Exception, higher prime than I'm comfortable with.");
                }
            }
            return foundPrimes;
        }

        public void PrintResults()
        {
            Console.WriteLine(string.Join(",", foundPrimes.ToArray()));
        }

        public bool CheckPrimes()
        {
            if (foundPrimes.Count == 0)
                return false;
            foreach (long prime in foundPrimes)
            {
                if (prime % 2 == 0 || prime % Math.Sqrt(prime) == 0)
                    return false;
                Random randomGenerator = new Random();
                for (int i = 0; i < 10; i++)
                {
                    int ceiling = (int)Math.Sqrt(prime);
                    if (ceiling < 4)
                        break;
                    int divider = randomGenerator.Next(4, ceiling);
                    if (divider % 2 == 0)
                        divider--;
                    if (prime % divider == 0)
                        return false;
                }
            }
            return true;
        }
    }
}