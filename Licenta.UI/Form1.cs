﻿using Licenta.Data;
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

            var aesConfiguration = new AesConfiguration()
            {
                Keysize = 256,
                DerivationIterations = 1000,
                BlockSize = 256,
                CipherMode = System.Security.Cryptography.CipherMode.CBC,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7,
                Password = "123"
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
