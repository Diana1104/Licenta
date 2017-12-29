using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Licenta.ORM
{
    public class Reflection
    {
        public static List<string> GetPropertyNames<T>()
        {
            return typeof(T).GetProperties().Select(p => p.Name).ToList();
        }
        
        public static T Create<T>(List<string> columns, IDataReader reader) where T : new()
        {
            var item = new T();

            foreach (var column in columns)
            {
                PropertyInfo prop = item.GetType().GetProperty(column, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    prop.SetValue(item, reader[column], null);
                }
            }

            return item;
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
