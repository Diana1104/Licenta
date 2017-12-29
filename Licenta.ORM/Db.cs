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
                    var item = Reflection.Create<T>(columns, reader);

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
                var properties = Reflection.GetPropertyNamesAndValues<T>(item);
                foreach (var property in properties)
                {
                    command.Parameters.AddWithValue(property.Key, property.Value);
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

                var properties = Reflection.GetPropertyNamesAndValues<T>(item);
                foreach (var property in properties)
                {
                    command.Parameters.AddWithValue(property.Key, property.Value);
                }

                command.ExecuteNonQuery();
            }
        }
    }
}
