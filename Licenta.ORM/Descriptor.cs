using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Licenta.ORM
{
    public class Descriptor
    {
        public List<string> GetPropertyNames<T>()
        {
            return typeof(T).GetProperties().Select(p => p.Name).ToList();
        }

        public Dictionary<string, object> GetPropertyNamesAndValues<T>(T item)
        {
            var dictionary = new Dictionary<string, object>();

            foreach(var property in typeof(T).GetProperties())
            {
                object value = property.GetValue(item);
                dictionary.Add(property.Name, value);
            }

            return dictionary;
        }

        public T Create<T>(List<string> columns, IDataReader reader) where T : new()
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

        public List<string> GetEncryptedPropertyNames<T>()
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
