using Licenta.ORM;
using System.Collections.Generic;

namespace Licenta.Data
{
    public class DbContext
    {
        private Db db;

        public DbContext()
        {
            db = new Db("server=.\\SQLEXPRESS;database=Licenta;integrated security=true");
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
