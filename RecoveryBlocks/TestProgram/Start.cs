using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace TestProgram
{
    public static class Start
    {
        public const string checkpointFileName = "checkpoint.xml";

        static void Main()
        {
            XmlSerializer checkpointer = new XmlSerializer(typeof(DataKeeper));
            DataKeeper j = new DataKeeper();

            var s = Path.GetFullPath(checkpointFileName);

            using (TextWriter fileWriter = new StreamWriter(checkpointFileName))
            {
                checkpointer.Serialize(fileWriter, j);
                j.a = 123;
            }
            using (FileStream fileStream = new FileStream(checkpointFileName, FileMode.Open))
            {
                j = (DataKeeper)checkpointer.Deserialize(fileStream);
            }

            Console.WriteLine(j.a);
        }
    }

    public class DataKeeper
    {
        public int a = 0;
        private int b = 1;
        protected int c = 2;
        public Random f = new Random();

        public DataKeeper()
        {
            d.Next();
        }
    }
}
