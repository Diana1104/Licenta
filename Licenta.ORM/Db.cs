using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

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
            var columns = typeof(T).GetProperties().Select(p => p.Name).ToList();
            var query = GetSelectStatement(tableName, columns);

            var items = new List<T>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var item = Create<T>(columns, reader);

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
            var type = item.GetType();
            PropertyInfo[] properties = type.GetProperties();

            var tableName = type.Name;
            var columns = properties.Select(p => p.Name).ToList();
            var query = GetInsertStatement(tableName, columns);

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                foreach (PropertyInfo property in properties)
                {
                    object value = property.GetValue(item);
                    command.Parameters.AddWithValue(property.Name, value);
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

        private T Create<T>(List<string> columns, SqlDataReader reader) where T : new()
        {
            var person = new T();

            foreach (var column in columns)
            {
                PropertyInfo prop = person.GetType().GetProperty(column, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    prop.SetValue(person, reader[column], null);
                }
            }

            return person;
        }
    }
}
