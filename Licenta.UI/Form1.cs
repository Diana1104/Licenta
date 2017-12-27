using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace Licenta.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var connectionString = "server=.\\SQLEXPRESS;database=Licenta;integrated security=true";
            
            this.dataGridView1.DataSource = GetList<Person>(connectionString, "SELECT FirstName, LastName, DateOfBirth, CardNo FROM Person");
            this.dataGridView2.DataSource = GetList<InventoryItem>(connectionString, "SELECT Name, Price, Count FROM Inventory");
        }

        private List<T> GetList<T>(string connectionString, string query) where T:new()
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

        private List<string> GetColumns(SqlDataReader reader)
        {
            var columns = new List<string>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                columns.Add(reader.GetName(i));
            }

            return columns;
        }

        private T Create<T>(List<string> columns, SqlDataReader reader) where T: new() 
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
