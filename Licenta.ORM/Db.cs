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

        public List<T> GetList<T>(string query) where T : new()
        {
            var items = new List<T>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                var reader = command.ExecuteReader();

                var columns = GetColumns(reader);

                while (reader.Read())
                {
                    var item = Create<T>(columns, reader);

                    items.Add(item);
                }
            }

            return items;
        }

        public void Save<T>(T item)
        {
            var type = item.GetType();
            PropertyInfo[] properties = type.GetProperties();

            string queryTemplate = "INSERT INTO [dbo].[{0}] ({1}) VALUES ({2})";
            string tableName = type.Name;
            string columnNames = string.Join(",", properties.Select(p => string.Format("[{0}]", p.Name)));
            string parameterNames = string.Join(",", properties.Select(p => string.Format("@{0}", p.Name)));

            string query = string.Format(queryTemplate, tableName, columnNames, parameterNames);

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
        
        private List<string> GetColumns(SqlDataReader reader)
        {
            var columns = new List<string>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                columns.Add(reader.GetName(i));
            }

            return columns;
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
