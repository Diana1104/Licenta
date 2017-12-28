using Licenta.ORM;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

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

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("FirstName", person.FirstName);
                command.Parameters.AddWithValue("LastName", person.LastName);
                command.Parameters.AddWithValue("CardNo", person.CardNo);
                command.Parameters.AddWithValue("DateOfBirth", person.DateOfBirth);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
