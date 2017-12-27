using Licenta.ORM;
using System.Collections.Generic;
using System.Configuration;

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
    }
}
