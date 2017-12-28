using Licenta.Data;
using System;
using System.Windows.Forms;

namespace Licenta.UI
{
    public partial class Form1 : Form
    {
        DbContext context;

        public Form1()
        {
            InitializeComponent();

            context = new DbContext();

            this.dataGridView1.DataSource = context.GetPersons();
            this.dataGridView2.DataSource = context.GetInventory();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var editPersonForm = new EditPerson();
            var result = editPersonForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                context.Save(editPersonForm.Person);
            }

            this.dataGridView1.DataSource = context.GetPersons();
        }
    }
}
