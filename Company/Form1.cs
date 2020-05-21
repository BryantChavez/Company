using Company.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Company
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Connection cn = new Connection();
            cn.IfDbExists();
            birthDateValue.Format = DateTimePickerFormat.Custom;
            birthDateValue.CustomFormat = "MM/dd/yyyy";
            txtId.Text=String.Format("{0:00000000}", 1);

        }

        //Method that will clean all fields.
        public void CleanFields()
        {
            txtFirstName.Text = "";
            txtId.Text = String.Format("{0:00000000}", 1);
            txtLastName.Text = "";
            birthDateValue.Value = DateTime.Today;
        }

        //Method that will add a user to the DB
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Employee em = new Employee();
            if (txtId.Text != "" && txtLastName.Text != "" && txtId.Text.Length ==8)
            {
                em.Id = txtId.Text;
                em.FirstName = txtFirstName.Text;
                em.LastName = txtLastName.Text;
                em.BirthDate = birthDateValue.Value.Date.ToString("MM/dd/yyyy");
                Boolean idExists = em.IfIdExists(Convert.ToInt32(em.Id));
                if (idExists == false)
                {
                    Boolean result = em.Add(em);
                    if (result == true)
                    {
                        CleanFields();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please, fill the form correctly.");
            }
        }

        //Method that will validate that id only receives numbers
        private void txtId_TextChanged(object sender, EventArgs e)
       {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtId.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers for Id.");
                txtId.Text = txtId.Text.Remove(txtId.Text.Length - 1);
            }
        }

        //Method that validates that birth date is not a future date.
        private void birthDateValue_ValueChanged(object sender, EventArgs e)
        {
            var CurrentDate = DateTime.Today;
            var DateSelected = birthDateValue.Value;
            if (DateSelected > CurrentDate)
            {
                MessageBox.Show("Select a valid Birth Date.");
                birthDateValue.Value = DateTime.Today;
            }
        }

        //Button that will call the method ConvertToTxt
        private void btnToTxt_Click(object sender, EventArgs e)
        {
            Employee em = new Employee();
            Boolean r = em.ConvertToTxt();
            if (r == false)
            {
                MessageBox.Show("There was an error converting the file to .txt");
            }
            else
            {
                MessageBox.Show("Database converted to .txt succesfully!");
            }
        }

        //Button that will call the method ConvertToXml
        private void btnToXML_Click(object sender, EventArgs e)
        {
            Employee em = new Employee();
            Boolean result = em.ConvertToXml();
            if (result == false)
            {
                MessageBox.Show("There was an error converting the file to .XML");
            }
            else
            {
                MessageBox.Show("Database converted to .XML succesfully!");
            }
        }
    }
}
