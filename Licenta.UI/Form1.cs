using Licenta.Data;
using Licenta.ORM;
using System.Configuration;
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
                db.Delete(person);    
            }

            this.dataGridView1.DataSource = db.GetAll<Person>();
        }
    }
}
