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

        public static List<string> GetEncryptedPropertyNames<T>()
        {
            List<string> encryptedProperties = new List<string>();
             
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] customAttributes = property.GetCustomAttributes(true);
                foreach (object atribute in customAttributes)
                {
                    var encryptedAttribute = atribute as EncryptedAttribute;
                    if (encryptedAttribute != null)
                    {
                        encryptedProperties.Add(property.Name);
                    }
                }
            }

            return encryptedProperties;
        }
    }
}
