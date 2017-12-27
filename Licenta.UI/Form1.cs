using System.Collections.Generic;
using System.Windows.Forms;

namespace Licenta.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var persons = new List<Person>
            {
                new Person { FirstName="Jon", LastName="Snow",CardNo="1234" }
            };

            this.dataGridView1.DataSource = persons;
        }
    }
}
