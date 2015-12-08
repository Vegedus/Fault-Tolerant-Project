using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTolerance
{
    class mikkelSort
    {
        static void Main(string[] args)
        {
            MikkelSort("Hello there. I am going to sort this string, but there are some problems");

            MikkelSort("the quick brown fox jumps over the lazy dog");

            while (true) { }

        }

        static string MikkelSort(string input)
        {
            string output = "";
            int l = input.Length;
            char[] check = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
            char[] cin = input.ToCharArray(0, l);

            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < l; j++)
                    {
                        if (cin[j] == check[i])
                        {
                            output += cin[j];
                        }
                    }
            }

            /*Console.WriteLine(output);*/

            return output;
        }
    }
}