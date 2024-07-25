using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsFormsApp1.BLL;
using WindowsFormsApp1.DAL;

namespace WindowsFormsApp1.UI
{
    public partial class frmDeal_Cust : Form
    {
        private Dictionary<Control, Control> controlFocusMap;

        public frmDeal_Cust()
        {
            InitializeComponent();
            // Initialize the dictionary with the focus order
            controlFocusMap = new Dictionary<Control, Control>
            {
                { cmbDeaCust, txtName },
                { txtName, txtEmail },
                { txtEmail, txtContact },
                { txtContact, txtAddress },
                { txtAddress, btnAdd }             
            };

            // Assign KeyDown event handler to all relevant controls
            foreach (var control in controlFocusMap.Keys)
            {
                control.KeyDown += new KeyEventHandler(Controls_KeyDown);
            }
        }
        private void Controls_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevents the "ding" sound

                Control nextControl;
                if (controlFocusMap.TryGetValue((Control)sender, out nextControl))
                {
                    nextControl.Focus();
                }
            }
        }

        private bool isUpdateMode = false; // Flag to track update mode

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void frmdeal_cust_Load(object sender, EventArgs e)
        {
            // Load the data into DataGridView
            DataTable dt = dcDal.Select();
            dgvDeaCust.DataSource = dt;
            // Attach RowHeaderMouseClick event to DataGridView
            dgvDeaCust.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgvDeaCust_RowHeaderMouseClick);
        }

        DeaCustBLL dc = new DeaCustBLL();
        DeaCustDAL dcDal = new DeaCustDAL();
        userDAL uDal = new userDAL();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Check if we are in update mode
            if (isUpdateMode)
            {
                MessageBox.Show("Please finish updating before adding a new dealer or customer.");
                return;
            }

            // Check if required fields are not empty
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtContact.Text) || string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            //Get the Values from Form
            dc.type = cmbDeaCust.Text;
            dc.name = txtName.Text;
            dc.email = txtEmail.Text;
            dc.contact = txtContact.Text;
            dc.address = txtAddress.Text;
            dc.added_date = DateTime.Now;
            //Getting the ID to Logged in user and passing its value in dealer or customer module
            string loggedUsr = frmLogin.loggedIn;
            userBLL usr = uDal.GetIDFromUsername(loggedUsr);
            dc.added_by = usr.id;

            //Creating boolean variable to check whether the dealer or customer is added or not
            bool success = dcDal.Insert(dc);

            if (success == true)
            {
                //Dealer or Customer inserted successfully 
                MessageBox.Show("Dealer or Customer Added Successfully");
                Clear();
                //Refresh Data Grid View
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                //failed to insert dealer or customer
                MessageBox.Show("Failed to Insert Dealer or Customer");
            }
        }

        public void Clear()
        {
            txtDeaCustID.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            txtboxSearch.Text = "";
            isUpdateMode = false; // Reset update mode
        }

        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Ensure that the clicked row index is valid
            if (e.RowIndex >= 0)
            {
                // Finding the Row Index of the Row Clicked on Data Grid View
                int RowIndex = e.RowIndex;

                // Populate the textboxes with the data from the selected row
                txtDeaCustID.Text = dgvDeaCust.Rows[RowIndex].Cells["deal_cust_id"].Value.ToString();
                txtName.Text = dgvDeaCust.Rows[RowIndex].Cells["name"].Value.ToString();
                txtEmail.Text = dgvDeaCust.Rows[RowIndex].Cells["email"].Value.ToString();
                txtContact.Text = dgvDeaCust.Rows[RowIndex].Cells["contact"].Value.ToString();
                txtAddress.Text = dgvDeaCust.Rows[RowIndex].Cells["address"].Value.ToString();
                cmbDeaCust.Text = dgvDeaCust.Rows[RowIndex].Cells["type"].Value.ToString();

                // Set update mode flag to true
                isUpdateMode = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Check if we are not in update mode
            if (!isUpdateMode)
            {
                MessageBox.Show("Please select a dealer or customer to update.");
                return;
            }

            // Get the values from the form
            dc.deal_cust_id = int.Parse(txtDeaCustID.Text);
            dc.type = cmbDeaCust.Text;
            dc.name = txtName.Text;
            dc.email = txtEmail.Text;
            dc.contact = txtContact.Text;
            dc.address = txtAddress.Text;
            dc.added_date = DateTime.Now;
            //Getting the ID to Logged in user and passing its value in dealer or customer module
            string loggedUsr = frmLogin.loggedIn;
            userBLL usr = uDal.GetIDFromUsername(loggedUsr);
            dc.added_by = usr.id;

            //Creating boolean variable to check whether the dealer or customer is updated or not
            bool success = dcDal.Update(dc);

            if (success == true)
            {
                //Dealer or Customer update Successfully
                MessageBox.Show("Dealer or Customer updated Successfully");
                Clear();
                //Refresh the Data Grid View
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                //Failed to update Dealer or Customer
                MessageBox.Show("Failed to Update Dealer or Customer");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if we are not in update mode
            if (!isUpdateMode)
            {
                MessageBox.Show("Please select a dealer or customer to delete.");
                return;
            }

            //Get the id of the user to be deleted from form
            dc.deal_cust_id = int.Parse(txtDeaCustID.Text);

            //Create boolean variable to check whether the dealer or customer is deleted or not
            bool success = dcDal.Delete(dc);

            if (success == true)
            {
                //Dealer or Customer Deleted Successfully
                MessageBox.Show("Dealer or Customer Deleted Successfully");
                Clear();
                //Refresh the Data Grid View
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                //Dealer or Customer Failed to Delete
                MessageBox.Show("Failed to Delete Dealer or Customer");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Clear();
        }

        private void txtboxSearch_TextChanged_1(object sender, EventArgs e)
        {
            // Get the Keywords
            string keywords = txtboxSearch.Text;

            // Filter the dealers and customers based on keywords
            if (!string.IsNullOrEmpty(keywords))
            {
                // Use Search Method To Display Dealers and Customers
                DataTable dt = dcDal.Search(keywords);
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                // Use Select Method to Display All Dealers and Customers
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
        }
    }
}
