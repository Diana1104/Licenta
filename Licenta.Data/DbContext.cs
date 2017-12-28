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
            return db.GetList<Person>();
        }

        public List<Product> GetProducts()
        {
            return db.GetList<Product>();
        }

        public void Save<T>(T item)
        {
            db.Save(item);
        }
    }
}
