using Licenta.Data;
using System;
using System.Windows.Forms;

namespace Licenta.UI
{
    public partial class EditPerson : Form
    {
        public EditPerson()
        {
            InitializeComponent();
        }

        public Person Person
        {
            get
            {
                return new Person
                {
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    CardNo = CardNoTextBox.Text,
                    DateOfBirth = DateOfBirthDateTimePicker.Value
                };
            }
        }

        private void SavePerson(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cancel(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
