using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace RecoveryBlocks
{
    public class SerializerTest
    {
        const string checkpointFileName = "checkpoint.xml";

        SerializerTest

        static void Main()
        {
            XmlSerializer checkpointer = new XmlSerializer(typeof(DataKeeper));
            DataKeeper j = new DataKeeper();

            using (TextWriter fileWriter = new StreamWriter(checkpointFileName))
            {
                checkpointer.Serialize(fileWriter, j);
                j.i = 123;

                using (FileStream fileStream = new FileStream(checkpointFileName, FileMode.Open))
                {
                    j = (DataKeeper)checkpointer.Deserialize(fileStream);
                }
            }

            Console.WriteLine(j.i);
        }
    }

    public class DataKeeper
    {
        private int a = 0;
        public int b = 1;
        protected int c = 2;
        
        public void scramble()
        {
        }
         
    }
}
