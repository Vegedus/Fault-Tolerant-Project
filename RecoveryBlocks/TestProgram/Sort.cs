using System;

namespace RecoveryBlocks
{
    class Sort
    {
        public static string MarcSort(string str)
        {
            // PRINTS UNSORTED ARRAY
            // Console.WriteLine(str);

            // FROM STRING TO STRING ARRAY
            string[] strArr = str.Split(' ');

            // ALPHABETICAL SORTING
            string temp = string.Empty;
            try
            {
                for (int i = 1; i < strArr.Length; i++)
                {
                    for (int j = 0; j < strArr.Length - i; j++)
                    {
                        if (strArr[j].CompareTo(strArr[j + 1]) > 0)
                        {
                            temp = strArr[j];
                            strArr[j] = strArr[j + 1];
                            strArr[j + 1] = temp;
                        }
                    }
                }
            }
            catch (Exception except)
            {
                Console.Write(except.Message);
            }

            // FROM STRING ARRAY TO STRING
            str = String.Join(" ", strArr);

            // PRINTS SORTED ARRAY
            //  Console.WriteLine (str);

            return str;
        }

        public static string MikkelSort(string input)
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

        public static string MadsSort(string input)
        {
            return input;
        }

        public static Boolean CorrectString(String result, String expected)
        {
            //char[] array = result.ToCharArray();
            //Array.Sort<char>(array);
            return result == expected;
        }
    }
}