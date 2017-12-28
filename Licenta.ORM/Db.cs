using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Licenta.ORM
{
    public class Db
    {
        private string connectionString;
        private Descriptor descriptor;

        public Db(string connectionString)
        {
            this.connectionString = connectionString;
            this.descriptor = new Descriptor();
        }

        public List<T> GetAll<T>() where T : new()
        {
            var tableName = typeof(T).Name;
            var columns = descriptor.GetPropertyNames<T>();
            var query = GetSelectStatement(tableName, columns);

            var items = new List<T>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var item = descriptor.Create<T>(columns, reader);

                    items.Add(item);
                }
            }

            return items;
        }

        private string GetSelectStatement(string tableName, List<string> columns)
        {
            string queryTemplate = "SELECT {0} FROM [dbo].[{1}]";
            string columnNames = string.Join(",", columns.Select(column => string.Format("[{0}]", column)));
            return string.Format(queryTemplate, columnNames, tableName);
        }

        public void Save<T>(T item)
        {
            var tableName = typeof(T).Name;
            var columns = descriptor.GetPropertyNames<T>();
            var query = GetInsertStatement(tableName, columns);

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                var properties = descriptor.GetPropertyNamesAndValues<T>(item);
                foreach (var property in properties)
                {
                    command.Parameters.AddWithValue(property.Key, property.Value);
                }

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private string GetInsertStatement(string tableName, List<string> columns)
        {
            string queryTemplate = "INSERT INTO [dbo].[{0}] ({1}) VALUES ({2})";
            string columnNames = string.Join(",", columns.Select(column => string.Format("[{0}]", column)));
            string parameterNames = string.Join(",", columns.Select(column => string.Format("@{0}", column)));
            return string.Format(queryTemplate, tableName, columnNames, parameterNames);
        }
        
        public void Delete<T>(T item)
        {
            var tableName = typeof(T).Name;
            var columns = descriptor.GetPropertyNames<T>();
            var query = GetDeleteStatement(tableName, columns);
            
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                var properties = descriptor.GetPropertyNamesAndValues<T>(item);
                foreach (var property in properties)
                {
                    command.Parameters.AddWithValue(property.Key, property.Value);
                }

                command.ExecuteNonQuery();
            }
        }

        private string GetDeleteStatement(string tableName, List<string> columns)
        {
            string queryTemplate = "DELETE FROM [dbo].[{0}] WHERE {1}";
            string searchCondition = string.Join(" AND ", columns.Select(column => string.Format("[{0}] = @{0}", column)));
            return string.Format(queryTemplate, tableName, searchCondition);
        }
    }
}
