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
    public partial class frmAbout : Form
    {

        frmaboutDAL dal = new frmaboutDAL();
        private bool isNewEntry = true; // Assume initially it's a new entry
        private frmaboutBLL currentCompanyDetails;


        public frmAbout()
        {
            InitializeComponent();
           
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            // Load initial company details
            LoadCompanyDetails();
        }
        private void LoadCompanyDetails()
        {
            currentCompanyDetails = dal.GetCompanyDetails();

            if (currentCompanyDetails.ID > 0)
            {
                // Company details exist, populate text boxes
                txtName.Text = currentCompanyDetails.Name;
                txtAddress.Text = currentCompanyDetails.Address;
                txtPhone.Text = currentCompanyDetails.Phone;
                txtEmail.Text = currentCompanyDetails.Mail;
                txtWebsite.Text = currentCompanyDetails.Website;
                txtMangDire.Text = currentCompanyDetails.ManagingDirector;

                isNewEntry = false; // It's an update operation
            }
            else
            {
                // No existing details found, initialize new company object
                currentCompanyDetails = new frmaboutBLL();
                isNewEntry = true; // It's a new entry
            }
        }


        private void lblSave_Click(object sender, EventArgs e)
        {
            // Update company details object
            currentCompanyDetails.Name = txtName.Text;
            currentCompanyDetails.Address = txtAddress.Text;
            currentCompanyDetails.Phone = txtPhone.Text;
            currentCompanyDetails.Mail = txtEmail.Text;
            currentCompanyDetails.Website = txtWebsite.Text;
            currentCompanyDetails.ManagingDirector = txtMangDire.Text;

            bool isSuccess = false;

            if (isNewEntry)
            {
                // Insert new company details
                isSuccess = dal.InsertCompanyDetails(currentCompanyDetails);
            }
            else
            {
                // Update existing company details
                isSuccess = dal.UpdateCompanyDetails(currentCompanyDetails);
            }

            if (isSuccess)
            {
                MessageBox.Show("Company details saved successfully.");
                // Optionally refresh the form or update UI as needed
            }
            else
            {
                MessageBox.Show("Failed to save company details.");
            }
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBoxMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private int companyID; // Declare a field to store the current company ID

        private void lblEdit_Click(object sender, EventArgs e)
        {
            // Load company details into text boxes
            LoadCompanyDetails();
        }


        private void ClearForm()
        {
            // Clear all form fields and disable editing
            txtName.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtWebsite.Text = "";
            txtMangDire.Text = "";

            txtName.Enabled = false;
            txtAddress.Enabled = false;
            txtPhone.Enabled = false;
            txtEmail.Enabled = false;
            txtWebsite.Enabled = false;
            txtMangDire.Enabled = false;

            lblSave.Visible = false; // Hide Save button after delete
        }

    }
}

