using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Licenta.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var persons = new List<Person>();
            
            var connectionString = "server=.\\SQLEXPRESS;database=Licenta;integrated security=true";
            var query = "SELECT FirstName, LastName, CardNo FROM Person";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                var reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    var person = new Person();

                    person.FirstName = reader["FirstName"].ToString();
                    person.LastName = reader["LastName"].ToString();
                    person.CardNo = reader["CardNo"].ToString();
                    
                    persons.Add(person);
                }
            }

            this.dataGridView1.DataSource = persons;
        }
    }
}
