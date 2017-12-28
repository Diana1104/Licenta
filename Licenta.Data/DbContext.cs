using Licenta.ORM;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Licenta.Data
{
    public class DbContext
    {
        private Db db;

        public DbContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Licenta"].ConnectionString;
            db = new Db(connectionString);
        }

        public List<Person> GetPersons()
        {
            return db.GetList<Person>("SELECT FirstName, LastName, DateOfBirth, CardNo FROM Person");
        }

        public List<InventoryItem> GetInventory()
        {
            return db.GetList<InventoryItem>("SELECT Name, Price, Count FROM Inventory");
        }

        public void Save<T>(T item)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Licenta"].ConnectionString;
            
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
    }
}
