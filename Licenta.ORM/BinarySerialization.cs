using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Licenta.ORM
{
    public class BinarySerialization
    {
        static BinaryFormatter binaryFormatter = new BinaryFormatter();

        public static byte[] Serialize<T>(T item)
        {
            using (var memoryStream = new MemoryStream())
            {                
                binaryFormatter.Serialize(memoryStream, item);
                return memoryStream.ToArray();
            }
        }

        public static object Deserialize(byte[] input)
        {
            using (var memoryStream = new MemoryStream(input))
            {
                return binaryFormatter.Deserialize(memoryStream);
            }
        }
    }
}
