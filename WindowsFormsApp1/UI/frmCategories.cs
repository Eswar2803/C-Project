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
    public partial class frmCategories : Form
    {
       
        public frmCategories()
        {
            InitializeComponent();
        }

        private bool isUpdateMode = false;
        private void pictureBoxclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        categoriesBLL c = new categoriesBLL();
        categoriesDAL dal = new categoriesDAL();
        userDAL udal = new userDAL();
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Check if we are in update mode
            if (isUpdateMode)
            {
                MessageBox.Show("Please finish updating before adding a new category.");
                return;
            }

            // Check if required fields are not empty
            if (string.IsNullOrEmpty(txtTitle.Text) || string.IsNullOrEmpty(txtDescription.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;

            // Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            // Passing the ID of logged in user in added by field
            c.added_by = usr.id;

            // Creating Boolean Method to insert data into database
            bool success = dal.Insert(c);

            // If the category is inserted successfully then the value of the success will be true else it will be false
            if (success == true)
            {
                // New Category Inserted Successfully
                MessageBox.Show("New Category Inserted Successfully.");
                Clear();
                // Refresh Data Grid View
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                // Failed to Insert New Category
                MessageBox.Show("Failed to Insert New Category.");
            }
        }

        public void Clear()
        {
            txtCategoryID.Text = "";
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtboxSearch.Text = "";
            isUpdateMode = false; // Reset update mode
        }

        private void dgvCategories_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Finding the Row Index of the Row Clicked on Data Grid View
            int RowIndex = e.RowIndex;
            txtCategoryID.Text = dgvCategories.Rows[RowIndex].Cells[0].Value.ToString();
            txtTitle.Text = dgvCategories.Rows[RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dgvCategories.Rows[RowIndex].Cells[2].Value.ToString();

            // Set update mode flag to true
            isUpdateMode = true;
        }

        private void frmCategories_Load(object sender, EventArgs e)
        {
            // Here write the code to display all the categories in Data Grid View
            DataTable dt = dal.Select();
            dgvCategories.DataSource = dt;

            // Attach KeyDown event to all relevant TextBoxes
            txtTitle.KeyDown += new KeyEventHandler(txtFields_KeyDown);
            txtDescription.KeyDown += new KeyEventHandler(txtFields_KeyDown);
            txtboxSearch.KeyDown += new KeyEventHandler(txtFields_KeyDown);
        }

        private void txtFields_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevents the "ding" sound
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Check if we are not in update mode
            if (!isUpdateMode)
            {
                MessageBox.Show("Please select a category to update.");
                return;
            }

            // Get the values from the Category form
            c.id = int.Parse(txtCategoryID.Text);
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;
            // Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            // Passing the ID of logged in user in added by field
            c.added_by = usr.id;

            // Creating Boolean variable to update categories and check
            bool success = dal.Update(c);
            // If the category is updated successfully then the value of success will be true else it will be false
            if (success == true)
            {
                // Category updated successfully
                MessageBox.Show("Category Updated Successfully");
                Clear();
                // Refresh Data Grid View
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                // Failed to Update Category
                MessageBox.Show("Failed to Update Category");
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if we are not in update mode
            if (!isUpdateMode)
            {
                MessageBox.Show("Please select a category to delete.");
                return;
            }

            // Get the ID of the Category Which we want to Delete
            c.id = int.Parse(txtCategoryID.Text);

            // Creating Boolean Variable to Delete The Category
            bool success = dal.Delete(c);

            // If the Category is Deleted Successfully then the value of success will be true else it will be false
            if (success == true)
            {
                // Category Deleted Successfully
                MessageBox.Show("Category Deleted Successfully");
                Clear();
                // Refreshing Data Grid View
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                // Failed to Delete Category
                MessageBox.Show("Failed to Delete Category");
            }
        }
        private void txtboxSearch_TextChanged(object sender, EventArgs e)
        {
            // Get the Keywords
            string keywords = txtboxSearch.Text;

            // Filter the categories based on keywords
            if (!string.IsNullOrEmpty(keywords))
            {
                // Use Search Method To Display Categories
                DataTable dt = dal.Search(keywords);
                dgvCategories.DataSource = dt;
            }
            else
            {
                // Use Select Method to Display All Categories
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Clear();
        }
        private void pictureBoxMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBoxClose_Click_1(object sender, EventArgs e)
        {
            this.Close();   
        }
    }
}
