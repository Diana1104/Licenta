using Licenta.Data;
using Licenta.ORM;
using System.Windows.Forms;

namespace Licenta.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var connectionString = "server=.\\SQLEXPRESS;database=Licenta;integrated security=true";

            var db = new Db(connectionString);

            this.dataGridView1.DataSource = db.GetList<Person>("SELECT FirstName, LastName, DateOfBirth, CardNo FROM Person");
            this.dataGridView2.DataSource = db.GetList<InventoryItem>("SELECT Name, Price, Count FROM Inventory");
        }        
    }
}
