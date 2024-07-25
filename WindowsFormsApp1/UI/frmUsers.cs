using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.BLL;
using WindowsFormsApp1.DAL;

namespace WindowsFormsApp1.UI
{
    public partial class frmUsers : Form
    {
        private Dictionary<Control, Control> controlFocusMap;

        public frmUsers()
        {
            InitializeComponent();

            // Initialize the dictionary
            controlFocusMap = new Dictionary<Control, Control>
            {
                { txtFirstName, txtLastName },
                { txtLastName, txtEmail },
                { txtEmail, txtUsername },
                { txtUsername, txtPassword },
                { txtPassword, txtContact },
                { txtContact, txtAddress },
                { txtAddress, cmbGender },
                { cmbGender, cmbUserType },
                { cmbUserType, btnAdd }
            };

            // Assign KeyDown event handler to all relevant controls
            foreach (var control in controlFocusMap.Keys)
            {
                control.KeyDown += new KeyEventHandler(Controls_KeyDown);
            }
        }

        userBLL u = new userBLL();
        userDAL dal = new userDAL();

        private void label1_Click(object sender, EventArgs e) { }

        private void panel1_Paint(object sender, PaintEventArgs e) { }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }

        private void label1_Click_1(object sender, EventArgs e) { }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Check if we are in update mode
            if (isUpdateMode)
            {
                MessageBox.Show("Please finish Updating before Adding a New User.");
                return;
            }

            // Check if required fields are not empty
            if (txtFirstName.Text == "" || txtLastName.Text == "" || txtEmail.Text == "" ||
                txtUsername.Text == "" || txtPassword.Text == "" || txtContact.Text == "" ||
                txtAddress.Text == "" || cmbGender.Text == "" || cmbUserType.Text == "")
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            u.first_name = txtFirstName.Text;
            u.last_name = txtLastName.Text;
            u.email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.contact = txtContact.Text;
            u.address = txtAddress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUserType.Text;
            u.added_date = DateTime.Now;

            // Getting username of the logged-in user
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = dal.GetIDFromUsername(loggedUser);
            u.added_by = usr.id;

            // Inserting Data into the database
            bool success = dal.Insert(u);
            if (success == true)
            {
                MessageBox.Show("New User Successfully created");
                // Clear function Calling
                Clear();
            }
            else
            {
                MessageBox.Show("Failed to create user");
            }
            // Refreshing DataGrid View
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }

        private void Clear()
        {
            txtUserID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            cmbGender.Text = "";
            cmbUserType.Text = "";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        // Boolean flag to indicate if we are in update mode
        private bool isUpdateMode = false;

        private void dgvUsers_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Get the index of the particular row
            int rowIndex = e.RowIndex;
            txtUserID.Text = dgvUsers.Rows[rowIndex].Cells[0].Value.ToString();
            txtFirstName.Text = dgvUsers.Rows[rowIndex].Cells[1].Value.ToString();
            txtLastName.Text = dgvUsers.Rows[rowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dgvUsers.Rows[rowIndex].Cells[3].Value.ToString();
            txtUsername.Text = dgvUsers.Rows[rowIndex].Cells[4].Value.ToString();
            txtPassword.Text = dgvUsers.Rows[rowIndex].Cells[5].Value.ToString();
            txtContact.Text = dgvUsers.Rows[rowIndex].Cells[6].Value.ToString();
            txtAddress.Text = dgvUsers.Rows[rowIndex].Cells[7].Value.ToString();
            cmbGender.Text = dgvUsers.Rows[rowIndex].Cells[8].Value.ToString();
            cmbUserType.Text = dgvUsers.Rows[rowIndex].Cells[9].Value.ToString();

            // Set update mode flag to true
            isUpdateMode = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Check if we are not in update mode
            if (!isUpdateMode)
            {
                MessageBox.Show("Please select a user to update.");
                return;
            }

            u.id = Convert.ToInt32(txtUserID.Text);
            u.first_name = txtFirstName.Text;
            u.last_name = txtLastName.Text;
            u.email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.contact = txtContact.Text;
            u.address = txtAddress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUserType.Text;
            u.added_date = DateTime.Now;
            u.added_by = 1;

            // Updating Data into the database
            bool success = dal.Update(u);
            if (success == true)
            {
                MessageBox.Show("User successfully updated");
                Clear();
            }
            else
            {
                MessageBox.Show("Failed to Update");
            }
            // Refresh Data Grid Successfully
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Delete data from the database
            u.id = Convert.ToInt32(txtUserID.Text);

            bool success = dal.Delete(u);
            if (success == true)
            {
                MessageBox.Show("User Deleted Successfully");
                Clear();
            }
            else
            {
                MessageBox.Show("Failed to delete user");
            }
            // Refresh Data Grid Successfully
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }

        private void txtboxSearch_TextChanged(object sender, EventArgs e)
        {
            // Get the value from the textbox
            string keywords = txtboxSearch.Text;
            if (keywords != null)
            {
                DataTable dt = dal.Search(keywords);
                dgvUsers.DataSource = dt;
            }
            else
            {
                DataTable dt = dal.Select();
                dgvUsers.DataSource = dt;
            }
        }

        private void Controls_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the beep sound
                if (controlFocusMap.TryGetValue(sender as Control, out Control nextControl))
                {
                    nextControl.Focus();
                }
            }
        }

        private void frmUsers_Click(object sender, EventArgs e)
        {

        }
        private void frmUsers_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; // Cancel the form closing
            this.Hide(); // Hide the form instead
        }
        private void txtboxSearch_TextChanged_1(object sender, EventArgs e)
        {
            // Get the Keywords
            string keywords = txtboxSearch.Text;

            // Filter the categories based on keywords
            if (!string.IsNullOrEmpty(keywords))
            {
                // Use Search Method To Display Categories
                DataTable dt = dal.Search(keywords);
                dgvUsers.DataSource = dt;
            }
            else
            {
                // Use Select Method to Display All Categories
                DataTable dt = dal.Select();
                dgvUsers.DataSource = dt;
            }
        }      
        private void pictureBoxMinimize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBoxMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }
        private void pictureBoxClose(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
