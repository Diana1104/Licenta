using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Licenta.ORM
{
    public class Db
    {
        private string connectionString;

        public Db(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<T> GetAll<T>() where T : new()
        {
            var tableName = typeof(T).Name;
            var columns = Reflection.GetPropertyNames<T>();
            var query = Sql.GetSelectStatement(tableName, columns);

            var items = new List<T>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var item = new T();

                    foreach (var column in columns)
                    {
                        SetValue(column, item, reader[column]);
                    }

                    items.Add(item);
                }
            }

            return items;
        }
        
        public void Save<T>(T item)
        {
            var tableName = typeof(T).Name;
            var columns = Reflection.GetPropertyNames<T>();
            var query = Sql.GetInsertStatement(tableName, columns);

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                foreach (var column in columns)
                {                    
                    command.Parameters.AddWithValue(column, GetValue(column, item));
                }

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        
        public void Delete<T>(T item)
        {
            var tableName = typeof(T).Name;
            var columns = Reflection.GetPropertyNames<T>();
            var query = Sql.GetDeleteStatement(tableName, columns);
            
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                foreach (var column in columns)
                {
                    command.Parameters.AddWithValue(column, GetValue(column, item));
                }

                command.ExecuteNonQuery();
            }
        }
        
        private object GetValue<T>(string propertyName, T item)
        {
            var value = Reflection.GetValue(propertyName, item);
            return Reflection.IsEncrypted<T>(propertyName) ? Encrypt(value) : value;
        }

        private void SetValue<T>(string propertyName, T item, object value)
        {
            var normalizedValue = Reflection.IsEncrypted<T>(propertyName) ? Decrypt(value as byte[]) : value;
            Reflection.SetValue(propertyName, item, normalizedValue);
        }

        private byte[] Encrypt(object input)
        {
            var aes = new Aes();
            var bytes = Serialization.Serialize(input);
            return aes.Encrypt(bytes, "123");
        }

        private object Decrypt(byte[] input)
        {
            var aes = new Aes();
            var bytes = aes.Decrypt(input, "123");
            return Serialization.Deserialize(bytes);
        }
    }
}
