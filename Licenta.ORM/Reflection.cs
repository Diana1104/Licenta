using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Licenta.ORM
{
    public class Reflection
    {
        public static List<string> GetPropertyNames<T>()
        {
            return typeof(T).GetProperties().Select(p => p.Name).ToList();
        }

        public static void SetValue<T>(string propertyName, T item, object value)
        {
            typeof(T).GetProperty(propertyName).SetValue(item, value);
        }
                
        public static object GetValue<T>(string propertyName, T item)
        {
            return typeof(T).GetProperty(propertyName).GetValue(item);
        }

        public static bool IsEncrypted<T>(string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);
            var attributes = property.GetCustomAttributes(true);
            return attributes.Any(a => a is EncryptedAttribute);            
        }
    }
}
