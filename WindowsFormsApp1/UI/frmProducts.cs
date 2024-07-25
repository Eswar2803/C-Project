using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp1.BLL;
using WindowsFormsApp1.DAL;

namespace WindowsFormsApp1.UI
{
    public partial class frmProducts : Form
    {
        private bool isUpdateMode = false;
        private Dictionary<Control, Control> controlFocusMap;

        public frmProducts()
        {
            InitializeComponent();
            // Initialize the dictionary with the focus order
            controlFocusMap = new Dictionary<Control, Control>
            {
                { txtName, cmbCategory },
                { cmbCategory, txtDescription },
                { txtDescription, txtRate },
                { txtRate, btnAdd }
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
                e.SuppressKeyPress = true; // Prevent the beep sound
                if (controlFocusMap.TryGetValue(sender as Control, out Control nextControl))
                {
                    nextControl.Focus();
                }
            }
        }

        ProductsBLL p = new ProductsBLL();
        ProductsDAL pdal = new ProductsDAL();
        categoriesDAL cdal = new categoriesDAL();
        userDAL udal = new userDAL();

        private void frmProducts_Load(object sender, EventArgs e)
        {
            // Load categories into ComboBox
            DataTable categoriesDT = cdal.Select();
            cmbCategory.DataSource = categoriesDT;
            cmbCategory.DisplayMember = "title";
            cmbCategory.ValueMember = "title";

            // Load products into DataGridView
            DataTable dt = pdal.Select();
            dgvProducts.DataSource = dt;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Check if we are in update mode
            if (isUpdateMode)
            {
                MessageBox.Show("Please finish updating before adding a new product.");
                return;
            }

            // Validate input fields
            if (!ValidateInput())
                return;

            // Set product details
            p.name = txtName.Text;
            p.category = cmbCategory.Text;
            p.description = txtDescription.Text;
            p.rate = decimal.Parse(txtRate.Text);
            p.qty = 0; // Default quantity
            p.added_date = DateTime.Now;

            // Get ID of logged in user
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            p.added_by = usr.id;

            // Insert product into database
            bool success = pdal.Insert(p);

            if (success)
            {
                MessageBox.Show("Product Added Successfully");
                Clear();
                RefreshGridView();
            }
            else
            {
                MessageBox.Show("Failed to Add Product");
            }
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtName.Text) ||
                string.IsNullOrEmpty(cmbCategory.Text) ||
                string.IsNullOrEmpty(txtDescription.Text) ||
                string.IsNullOrEmpty(txtRate.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return false;
            }

            decimal rate;
            if (!decimal.TryParse(txtRate.Text, out rate))
            {
                MessageBox.Show("Please enter a valid rate.");
                return false;
            }

            return true;
        }
        private void dgvProducts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0 && rowIndex < dgvProducts.Rows.Count)
            {
                txtProductID.Text = dgvProducts.Rows[rowIndex].Cells[0].Value.ToString();
                txtName.Text = dgvProducts.Rows[rowIndex].Cells[1].Value.ToString();
                cmbCategory.Text = dgvProducts.Rows[rowIndex].Cells[2].Value.ToString();
                txtDescription.Text = dgvProducts.Rows[rowIndex].Cells[3].Value.ToString();
                txtRate.Text = dgvProducts.Rows[rowIndex].Cells[4].Value.ToString();

                // Enable update mode
                isUpdateMode = true;
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Check if we are not in update mode
            if (!isUpdateMode)
            {
                MessageBox.Show("Please select a product to update.");
                return;
            }

            // Validate input fields
            if (!ValidateInput())
                return;

            // Set product details
            p.id = int.Parse(txtProductID.Text);
            p.name = txtName.Text;
            p.category = cmbCategory.Text;
            p.description = txtDescription.Text;
            p.rate = decimal.Parse(txtRate.Text);
            p.added_date = DateTime.Now;

            // Get ID of logged in user
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            p.added_by = usr.id;

            // Update product in database
            bool success = pdal.Update(p);

            if (success)
            {
                MessageBox.Show("Product Updated Successfully");
                Clear();
                RefreshGridView();
                isUpdateMode = false; // Reset update mode
                btnAdd.Enabled = true; // Re-enable add button
            }
            else
            {
                MessageBox.Show("Failed to Update Product");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!isUpdateMode)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            // Set product ID
            p.id = int.Parse(txtProductID.Text);

            // Delete product from database
            bool success = pdal.Delete(p);

            if (success)
            {
                MessageBox.Show("Product Deleted Successfully");
                Clear();
                RefreshGridView();
                isUpdateMode = false; // Reset update mode
                btnAdd.Enabled = true; // Re-enable add button
            }
            else
            {
                MessageBox.Show("Failed to Delete Product");
            }
        }

        private void Clear()
        {
            txtProductID.Text = "";
            txtName.Text = "";
            cmbCategory.SelectedIndex = -1;
            txtDescription.Text = "";
            txtRate.Text = "";
            isUpdateMode = false; // Reset update mode
            btnAdd.Enabled = true; // Re-enable add button
        }

        private void RefreshGridView()
        {
            DataTable dt = pdal.Select();
            dgvProducts.DataSource = dt;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtboxSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the Keywordss from Form
            string keywords = txtboxSearch.Text;

            if (keywords != null)
            {
                //Search the products
                DataTable dt = pdal.Search(keywords);
                dgvProducts.DataSource = dt;
            }
            else
            {
                //Display All the products
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Clear();
        }
    }
    }

