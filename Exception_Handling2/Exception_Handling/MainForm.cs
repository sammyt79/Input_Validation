// Input Validation
// Samuel Tollefson
// POS/409
// May 4, 2015
// Jon Jensen

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Exception_Handling
{
    public partial class MainForm : Form
    {
        const string idPattern = @"^[a-z]{3}\d{3}$";

        const string phonePattern = @"^\(?[2-9]\d{2}\)?-?[2-9]\d{2}-?\d{4}$";

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowIndex = 0;

            try
            {
                // Check for null data.
                if (txtID.Text == "" | txtFName.Text == "" | txtLName.Text == "" | txtPhone.Text == "")
                {
                    throw new ArgumentNullException();
                }

                // Check for invalid data.
                if (lblInvalidFName.Text != "" || lblInvalidID.Text != "" || lblInvalidLName.Text != "" || lblInvalidPhone.Text != "")
                {
                    throw new InvalidInputException();
                }

                // Check current list for duplicate ID.
                foreach (DataGridViewRow row in dgvPeople.Rows)
                {
                    if (txtID.Text == dgvPeople.Rows[rowIndex].Cells[0].Value.ToString())
                    {
                        throw new IDAlreadyExistsException();
                    }
                    rowIndex++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // Used for following ID check
            char[] b = new char[6];
            List<string> idCollectionString = Person.searchList().Split(',').ToList();

            // Check text file for duplicate ID.
            foreach (string s in idCollectionString)
            {
                try
                {
                    using (StringReader sr = new StringReader(s))
                    {
                        sr.Read(b, 0, 6);
                        string str = new string(b);

                        if (txtID.Text == str)
                        {
                            throw new IDAlreadyExistsException();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            // Set the variables.
            new Person(txtID.Text.Replace(" ", String.Empty), txtFName.Text, txtLName.Text, txtPhone.Text.Replace(" ", String.Empty));

            // Display the new person.
            dgvPeople.Rows.Add(Person.PersonID, Person.FName, Person.LName, Person.PhoneNumber);

            Person.addPersonToList();

            Person.writeToXML();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearTextBoxes()
        {
            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                        (control as TextBox).Clear();
                    else
                        func(control.Controls);
            };

            func(Controls);
        }

        private void txtID_Leave(object sender, EventArgs e)
        {
            bool valid = false;
            string text = txtID.Text.Replace(" ", String.Empty);
            if (text.Length == 0) valid = true;
            if (Regex.IsMatch(text, idPattern, RegexOptions.IgnoreCase)) valid = true;
            if (!valid)
            {
                lblInvalidID.Text = "Invalid input";
                lblInvalidID.ForeColor = Color.Red;
            } 
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {
            bool valid = false;
            string text = txtID.Text.Replace(" ", String.Empty);
            if (text.Length == 0) valid = true;
            if (Regex.IsMatch(text, idPattern, RegexOptions.IgnoreCase)) valid = true;
            if (valid) lblInvalidID.Text = "";
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            bool valid = false;
            string text = txtPhone.Text.Replace(" ", String.Empty);
            if (text.Length == 0) valid = true;
            if (Regex.IsMatch(text, phonePattern)) valid = true;
            if (valid) lblInvalidPhone.Text = "";
            else
            {
                lblInvalidPhone.Text = "Invalid input";
                lblInvalidPhone.ForeColor = Color.Red;
            } 
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            bool valid = false;
            string text = txtPhone.Text.Replace(" ", String.Empty);
            if (text.Length == 0) valid = true;
            if (Regex.IsMatch(text, phonePattern)) valid = true;
            if (valid) lblInvalidPhone.Text = "";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Person.addListToFile();
        }
    }
}
