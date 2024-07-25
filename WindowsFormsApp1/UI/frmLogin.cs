using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.BLL;
using WindowsFormsApp1.DAL;

namespace WindowsFormsApp1.UI
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();

            // Assign KeyDown event handlers
            txtUsername.KeyDown += new KeyEventHandler(TextBoxUsername_KeyDown);
            txtPassword.KeyDown += new KeyEventHandler(TextBoxPassword_KeyDown);
            cmbUserType.KeyDown += new KeyEventHandler(ComboBoxUserType_KeyDown);
        }

        loginBLL l = new loginBLL();
        loginDAL dal = new loginDAL();
        public static string loggedIn;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            l.Username = txtUsername.Text;
            l.Password = txtPassword.Text;
            l.User_type = cmbUserType.Text;

            // Checking the login credentials
            bool success = dal.loginCheck(l);
            if (success == true)
            {
                // Login Success
                MessageBox.Show("Login Successful");
                loggedIn = l.Username;

                // Open Admin Dashboard and pass user type
                FrmAdminDashboard adminDashboard = new FrmAdminDashboard(l.User_type);
                adminDashboard.Show();
                this.Hide();
            }
            else
            {
                // Login Failed
                MessageBox.Show("Login Failed. Please Try Again");
            }
        }

        private void TextBoxUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the beep sound
                txtPassword.Focus();
            }
        }

        private void TextBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                cmbUserType.Focus();
            }
        }

        private void ComboBoxUserType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnLogin.PerformClick(); // Simulate a click on the login button
            }
        }
    }
}

