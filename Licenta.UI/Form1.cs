using Licenta.Data;
using Licenta.ORM;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Licenta.UI
{
    public partial class Form1 : Form
    {
        Db db;

        public Form1()
        {
            InitializeComponent();

            var connectionString = ConfigurationManager.ConnectionStrings["Licenta"].ConnectionString;
            db = new Db(connectionString);

            this.dataGridView1.DataSource = db.GetAll<Person>();
            this.dataGridView2.DataSource = db.GetAll<Product>();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var editPersonForm = new EditPerson();
            var result = editPersonForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                db.Save(editPersonForm.Person);
            }

            this.dataGridView1.DataSource = db.GetAll<Person>();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            foreach(DataGridViewRow item in dataGridView1.SelectedRows)
            {
                var person = (Person)item.DataBoundItem;
                Delete(person);    
            }

            this.dataGridView1.DataSource = db.GetAll<Person>();
        }

        private void Delete(Person person)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Licenta"].ConnectionString;

            var type = person.GetType();
            var properties = type.GetProperties();
            var tableName = type.Name;
            var columns = properties.Select(p => p.Name).ToList();
            var query = GetDeleteStatement(tableName, columns);

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                foreach (var property in properties)
                {
                    object value = property.GetValue(person);
                    command.Parameters.AddWithValue(property.Name, value);
                }

                command.ExecuteNonQuery();
            }
        }

        private string GetDeleteStatement(string tableName, List<string> columns)
        {
            string queryTemplate = "DELETE FROM [dbo].[{0}] WHERE {1}";
            string searchCondition = string.Join(" AND ", columns.Select(column => string.Format("[{0}] = @{0}", column)));
            return string.Format(queryTemplate, tableName, searchCondition);
        }
    }
}
