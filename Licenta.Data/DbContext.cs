using Licenta.ORM;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

        public void Save(Person person)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Licenta"].ConnectionString;

            string query = @"
                 INSERT INTO [dbo].[Person]
                       ([FirstName]
                       ,[LastName]
                       ,[CardNo]
                       ,[DateOfBirth])
                 VALUES
                       (@FirstName
                       ,@LastName
                       ,@CardNo
                       ,@DateOfBirth)";

            var t = person.GetType();
            PropertyInfo[] properties = t.GetProperties();
            
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                foreach (PropertyInfo property in properties)
                {
                    object value = property.GetValue(person);
                    command.Parameters.AddWithValue(property.Name, value);
                }

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
