using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Reflection;

namespace OrganizationProfile
{
    public partial class frmRegistration : Form
    {
        //names of the variables.
        private string _FullName;
        private int _Age;
        private long _ContactNo;
        private long _StudentNo;

        public frmRegistration()
        {
            InitializeComponent();
        }

        private void frmRegistration_Load(object sender, EventArgs e)
        {
            string[] ListOfProgram = new string[] {
                "BS Information Technology",
                "BS Computer Science",
                "BS Information System",
                "BS in Accountancy",
                "BS in Hospitality Management",
                "BS in Tourism Management"
            };
            cbPrograms.Items.Clear();
            for (int i = 0; i < 6; i++)
            {
                cbPrograms.Items.Add(ListOfProgram[i].ToString());
            }
            cbGender.Items.Clear();
            cbGender.Items.AddRange(new string[] { "Male", "Female" });

            if (cbPrograms.Items.Count > 0) cbPrograms.SelectedIndex = 0;
            if (cbGender.Items.Count > 0) cbGender.SelectedIndex = 0;
        }
        /////return methods 
        public long StudentNumber(string studNum)
        {
            if (string.IsNullOrWhiteSpace(studNum))
            {
                throw new ArgumentNullException("Student number is required.");
            }
            else if (!Regex.IsMatch(studNum, @"^[0-9]+$"))
            {
                throw new FormatException("Student number must contain only digits.");
            }
            else if (studNum.Length != 10)
            {
                throw new IndexOutOfRangeException("Student number must be exactly 10 digits.");
            }
            else
            {
                try
                {
                    _StudentNo = long.Parse(studNum);
                }
                catch (OverflowException)
                {
                    throw new OverflowException("Student number is too large.");
                }
                return _StudentNo;
            }
        }

        public long ContactNo(string Contact)
        {
            if (string.IsNullOrWhiteSpace(Contact))
            {
                throw new ArgumentNullException("Contact number is required.");
            }
            else if (!Regex.IsMatch(Contact, @"^[0-9]+$"))
            {
                throw new FormatException("Contact number must be numeric.");
            }
            else if (Contact.Length < 10 || Contact.Length > 11)
            {
                throw new IndexOutOfRangeException("Contact number must be 10 or 11 digits.");
            }
            else
            {
                try
                {
                    _ContactNo = long.Parse(Contact);
                }
                catch (OverflowException)
                {
                    throw new OverflowException("Contact number is too long.");
                }
                return _ContactNo;
            }
        }

        public string FullName(string LastName, string FirstName, string MiddleInitial)
        {
            if (string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(MiddleInitial))
            {
                throw new ArgumentNullException("All name fields are required.");
            }
            else if (!Regex.IsMatch(LastName, @"^[a-zA-Z]+$") || !Regex.IsMatch(FirstName, @"^[a-zA-Z]+$") || !Regex.IsMatch(MiddleInitial, @"^[a-zA-Z]+$"))
            {
                throw new FormatException("Name fields must contain only letters.");
            }
            else
            {
                _FullName = LastName + ", " + FirstName + " " + MiddleInitial + ".";
                return _FullName;
            }
        }

        public int Age(string age)
        {
            if (string.IsNullOrWhiteSpace(age))
            {
                throw new ArgumentNullException("Age is required.");
            }
            else if (!Regex.IsMatch(age, @"^[0-9]{1,3}$"))
            {
                throw new FormatException("Age must be a number between 1 and 3 digits.");
            }
            else
            {
                try
                {
                    _Age = Int32.Parse(age);
                }
                catch (OverflowException)
                {
                    throw new OverflowException("Age value is too large.");
                }
                return _Age;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                StudentInformationClass.SetFullName = FullName(txtLastName.Text, txtFirstName.Text, txtMiddleInitial.Text);
                StudentInformationClass.SetStudentNo = StudentNumber(txtStudentNo.Text);
                StudentInformationClass.SetProgram = cbPrograms.Text;
                StudentInformationClass.SetGender = cbGender.Text;
                StudentInformationClass.SetContactNo = ContactNo(txtContactNo.Text);
                StudentInformationClass.SetAge = Age(txtAge.Text);
                StudentInformationClass.SetBirthday = datePickerBirthday.Value.ToString("yyyy- MM-dd");

                frmConfirmation frm = new frmConfirmation();
                frm.ShowDialog();
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "Argument Null Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (OverflowException ex)
            {
                MessageBox.Show(ex.Message, "Overflow Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Index Out of Range Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Always go back to first field
                txtLastName.Focus();
            }
        }
    }
}