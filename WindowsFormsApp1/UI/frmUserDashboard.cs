using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.UI;

namespace WindowsFormsApp1
{
    public partial class frmUserDashboard : Form
    {
        private Dictionary<Type, Form> mdiChildForms = new Dictionary<Type, Form>();
        public frmUserDashboard()
        {
            InitializeComponent();
            IsMdiContainer = true; // Set this form as MDI container

        }

        //Set a public static method to specify whether the form is purchase or sales
        public static string transactionType;
        private void frmUserDashboard_Load(object sender, EventArgs e)
        {
            lblLoggedInUser.Text = frmLogin.loggedIn;
            // Start the timer when the form loads
            timer2.Start();
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

        private void timer2_Tick(object sender, EventArgs e)
        {
            // Update the label with the current date and time
            lblDateTime.Text = DateTime.Now.ToString("F");

        }

        private void dealersAndCustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm<frmDeal_Cust>();
        }

        private void purchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //set value on transactionType static method
            transactionType = "Purchase";
            frmPurchaseAndSales purchase = new frmPurchaseAndSales();
            purchase.Show();
        }

        private void salesFormsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set the value to transacionType method to sales
            transactionType = "Sales";
            frmPurchaseAndSales sales = new frmPurchaseAndSales();
            sales.Show();
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm<frmInventory>();
        }
    }
}
