using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                new UI.Form1.Person { FirstName="Jon", LastName="Snow",CardNo="1234" }
            };

            this.dataGridView1.DataSource = persons;
        }

        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string CardNo { get; set; }
        }
    }
}
