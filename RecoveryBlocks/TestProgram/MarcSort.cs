using System;

namespace MarcSort
{
	class Program
	{
		static void Main (string[] args)
		{
			//Console.WriteLine ("ALPHABETICAL SORTING OF STRINGS");
			MarcSort ("Mikkel, Mads, Fault-Tolerant, Systems, Recovery, Blocks");
		}
			
		static string MarcSort (string str)
		{
			// PRINTS UNSORTED ARRAY
//			Console.WriteLine(str);

			// FROM STRING TO STRING ARRAY
			str = str.Replace (" ", "");
			string[] strArr = str.Split (',');

			// ALPHABETICAL SORTING
			string temp = string.Empty;
			try {
				for (int i = 1; i < strArr.Length; i++) {
					for (int j = 0; j < strArr.Length - i; j++) {
						if (strArr [j].CompareTo (strArr [j + 1]) > 0) {
							temp = strArr [j];
							strArr [j] = strArr [j + 1];
							strArr [j + 1] = temp;
						}
					}
				}
			} 
			catch (Exception except) {
				Console.Write (except.Message);
			}

			// FROM STRING ARRAY TO STRING
			str = String.Join (", ", strArr);

			// PRINTS SORTED ARRAY
//			Console.WriteLine (str);

			return str;
		}
	}
}