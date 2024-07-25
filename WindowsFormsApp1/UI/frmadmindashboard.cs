using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.BLL;
using WindowsFormsApp1.DAL;
using WindowsFormsApp1.UI;

namespace WindowsFormsApp1
{
    public partial class FrmAdminDashboard : Form
    {
        private string userType;
        private Dictionary<Type, Form> mdiChildForms = new Dictionary<Type, Form>();
        
        public FrmAdminDashboard()
        {
            InitializeComponent();
            IsMdiContainer = true; // Set this form as MDI container
            

            #region Placeholder Text
            // Initialize ToolTip (assuming toolTip1 is the name in designer)
            toolTip1.ShowAlways = true; // Show the tooltip even if the form is not active
            toolTip1.InitialDelay = 0; // Show the tooltip immediately

            // Attach MouseEnter and MouseLeave events to Menu items
            usersToolStripMenuItem.MouseEnter += Menu_MouseEnter;
            usersToolStripMenuItem.MouseLeave += Menu_MouseLeave;
            categoryToolStripMenuItem.MouseEnter += Menu_MouseEnter;
            categoryToolStripMenuItem.MouseLeave += Menu_MouseLeave;
            productsToolStripMenuItem.MouseEnter += Menu_MouseEnter;
            productsToolStripMenuItem.MouseLeave += Menu_MouseLeave;
            dealerToolStripMenuItem.MouseEnter += Menu_MouseEnter;
            dealerToolStripMenuItem.MouseLeave += Menu_MouseLeave;
            PurchaseToolStripMenuItem.MouseEnter += Menu_MouseEnter;
            PurchaseToolStripMenuItem.MouseLeave += Menu_MouseLeave;
            SalesToolStripMenuItem.MouseEnter += Menu_MouseEnter;
            SalesToolStripMenuItem.MouseLeave += Menu_MouseLeave;
            inventoryToolStripMenuItem1.MouseEnter += Menu_MouseEnter;
            inventoryToolStripMenuItem1.MouseLeave += Menu_MouseLeave;
            transactionsToolStripMenuItem1.MouseEnter += Menu_MouseEnter;
            transactionsToolStripMenuItem1.MouseLeave +=Menu_MouseLeave;
            aboutToolStripMenuItem.MouseEnter += Menu_MouseEnter;
            aboutToolStripMenuItem.MouseLeave += Menu_MouseLeave;
            logOutInToolStripMenuItem.MouseEnter += Menu_MouseEnter;
            logOutInToolStripMenuItem.MouseLeave += Menu_MouseLeave;


            // Set tooltips for each menu item
            usersToolStripMenuItem.Tag = "Manage Users";
            categoryToolStripMenuItem.Tag = "Manage Categories";
            productsToolStripMenuItem.Tag = "Manage Products";
            dealerToolStripMenuItem.Tag = "Manage Dealers and Customers";
            PurchaseToolStripMenuItem.Tag = "Manage Purchase";
            SalesToolStripMenuItem.Tag = "Manage Sales";
            inventoryToolStripMenuItem1.Tag = "Manage Inventory";
            transactionsToolStripMenuItem1.Tag = "Manage Transactions";
            aboutToolStripMenuItem.Tag = "Manage About";
            logOutInToolStripMenuItem.Tag = "Manage LogOut/In";
            #endregion

        }
        public FrmAdminDashboard(string userType) : this()
        {
            this.userType = userType;
            ManageMenuVisibility();
        }

        private void ManageMenuVisibility()
        {
            if (userType == "Admin")
            {
                // Admin can see all menu items
                usersToolStripMenuItem.Visible = true;
                categoryToolStripMenuItem.Visible = true;
                productsToolStripMenuItem.Visible = true;
                PurchaseToolStripMenuItem.Visible=true;
                inventoryToolStripMenuItem1.Visible = true;
                transactionsToolStripMenuItem1.Visible = true;
                aboutToolStripMenuItem.Visible = true;
                logOutInToolStripMenuItem.Visible = true;
            }
            else if (userType == "User")
            {
                // User can see selected menu items
                usersToolStripMenuItem.Visible = false;
                categoryToolStripMenuItem.Visible = true;
                productsToolStripMenuItem.Visible = true;
                PurchaseToolStripMenuItem.Visible = true;
                inventoryToolStripMenuItem1.Visible = true;
                transactionsToolStripMenuItem1.Visible = true;
                aboutToolStripMenuItem.Visible = true;
                logOutInToolStripMenuItem.Visible = true;
            }
        }
        private void ShowForm<T>() where T : Form, new()
        {

            // Check if form is already open
            if (!mdiChildForms.ContainsKey(typeof(T)) || mdiChildForms[typeof(T)]?.IsDisposed == true)
            {
                mdiChildForms[typeof(T)] = new T
                {
                    MdiParent = this,
                    StartPosition = FormStartPosition.CenterScreen // Or another appropriate StartPosition
                };

                mdiChildForms[typeof(T)].Show();
            }
            else
            {
                mdiChildForms[typeof(T)].Activate(); // Activate the form if it's already open
            }
        }
        private void FormAdminDashboard_Load(object sender, EventArgs e)
        {
            // Load event code here if needed
            lblLoggedInUser.Text = frmLogin.loggedIn;
            // Start the timer when the form loads
            timer2.Start();

        }      
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show users form and hide homepage labels
            ShowForm<frmUsers>();
           
        }
        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show categories form and hide homepage labels
            ShowForm<frmCategories>();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show products form and hide homepage labels
            ShowForm<frmProducts>();
        }
        private void dealerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show Dealers and Customers form and hide homepage labels
            ShowForm<frmDeal_Cust>();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            // Update the label with the current date and time
            lblDateTime.Text = DateTime.Now.ToString("F");

        }
        private void Menu_MouseEnter(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                string tooltipText = menuItem.Tag?.ToString();
                if (!string.IsNullOrEmpty(tooltipText))
                {
                    toolTip1.SetToolTip(menuStrip1, tooltipText); // Show the tooltip with the item text
                }
            }
        }
        private void Menu_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(menuStrip1, string.Empty); // Hide the tooltip
        }

        private void inventoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Show Iventory form and hide homepage labels
            ShowForm<frmInventory>();
        }

        private void transactionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Show Transactions form and hide homepage labels
            ShowForm<frmTransactions>();
        }

        //Set a public static method to specify whether the form is purchase or sales
        public static string transactionType;
        private void PurchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //set value on transactionType static method
            transactionType = "Purchase";
            frmPurchaseAndSales purchase = new frmPurchaseAndSales();
            purchase.Show();
        }

        private void SalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set the value to transacionType method to sales
            transactionType = "Sales";
            frmPurchaseAndSales sales = new frmPurchaseAndSales();
            sales.Show();
        }
        frmaboutDAL dal = new frmaboutDAL();
       
        private void CompanyDetailostoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowForm<frmAbout>();
        }

        private void logOutInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Display a confirmation dialog
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Check the result of the dialog
            if (result == DialogResult.Yes)
            {
                // Hide the current form
                this.Hide();

                // Show the login form
                frmLogin loginForm = new frmLogin();
                loginForm.ShowDialog();

                // Close the admin dashboard after login form is closed
                this.Close();
            }
            // If the result is No, Do Nothing
        }

       /* private void FrmAdminDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Display a confirmation dialog
            DialogResult result = MessageBox.Show(
                "Are you sure you want to close the application?",
                "Confirm Close",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Check the result of the dialog
            if (result == DialogResult.No)
            {
                // Cancel the form closing if the user clicks "No"
                e.Cancel = true;
            }
        }*/
    }
}
