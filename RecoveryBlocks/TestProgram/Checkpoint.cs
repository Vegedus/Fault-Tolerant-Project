using System;
using System.IO;
using System.Xml.Serialization;

namespace RecoveryBlocks
{
    public class Checkpoint<Type>
    {
        XmlSerializer serializer;
        String path;
        Type savedObject;

        public Checkpoint()
        {
            path = Path.GetFullPath(typeof(Type).ToString())+".xml";
            serializer = new XmlSerializer(typeof(Type));
        }

        public Checkpoint(Type dataObject) : this() 
        {
            Save(dataObject);
        }
        //
        public Checkpoint(String path)
        {
            path = Path.GetFullPath(path);
            serializer = new XmlSerializer(typeof(Type));
        }

        public void Save(Type dataObject) {
            savedObject = dataObject;
            using (TextWriter fileWriter = new StreamWriter(path))
            {
                serializer.Serialize(fileWriter, dataObject);
            }
        }

        public Type Load()
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                savedObject = (Type)serializer.Deserialize(fileStream);
                return savedObject;
            }
        }

        public static T Load<T>(String path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                return (T)serializer.Deserialize(fileStream);
            }
        }

    }
}
