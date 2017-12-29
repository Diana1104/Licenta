using Licenta.Data;
using Licenta.ORM;
using System;
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

            var aesConfiguration = new AesConfiguration()
            {
                BlockSize = 256,
                CipherMode = System.Security.Cryptography.CipherMode.CBC,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7,
                Key = Convert.FromBase64String("H+dCfDa4cugB2BPAA9S2lOe7cSnXJgKPqfiDehwak2w="),
                Iv = Convert.FromBase64String("dnyiUgfcGV9YBBafw4U3Cxqz4l6RlMI4s0pqVlWMuj8=")
            };

            db = new Db(connectionString, aesConfiguration);

            this.personsDataGridView.DataSource = db.GetAll<Person>();
            this.productsDataGridView.DataSource = db.GetAll<Product>();
        }

        private void SaveNewPerson(object sender, System.EventArgs e)
        {
            var editPersonForm = new EditPerson();
            var result = editPersonForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                db.Save(editPersonForm.Person);
            }

            this.personsDataGridView.DataSource = db.GetAll<Person>();
        }

        private void DeleteSelectedPersons(object sender, System.EventArgs e)
        {
            foreach(DataGridViewRow item in personsDataGridView.SelectedRows)
            {
                var person = (Person)item.DataBoundItem;
                db.Delete(person);    
            }

            this.personsDataGridView.DataSource = db.GetAll<Person>();
        }
    }
}
