using System.IO;
using System.Xml.Serialization;

namespace Licenta.ORM
{
    class XmlSerialization
    {
        public static byte[] Serialize<T>(T item)
        {
            using (var memoryStream = new MemoryStream())
            {
                var xmlSerializer = new XmlSerializer(item.GetType());
                xmlSerializer.Serialize(memoryStream, item);
                return memoryStream.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] input)
        {
            using (var memoryStream = new MemoryStream(input))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(memoryStream);
            }
        }
    }
}
