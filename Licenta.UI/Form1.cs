using Licenta.Data;
using System.Windows.Forms;

namespace Licenta.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var context = new DbContext();

            this.dataGridView1.DataSource = context.GetPersons();
            this.dataGridView2.DataSource = context.GetInventory();
        }        
    }
}
