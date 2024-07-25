using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using WindowsFormsApp1.BLL;
using WindowsFormsApp1.DAL;
using System.IO;


namespace WindowsFormsApp1.UI
{
    public partial class frmPurchaseAndSales : Form
    {
        public string TransactionType { get; set; }
        
        public frmPurchaseAndSales()
        {
            InitializeComponent();
            
        }        
        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        DeaCustDAL dcDAL = new DeaCustDAL();
        ProductsDAL pDAL = new ProductsDAL();
        userDAL uDAL = new userDAL();
        transactionDAL tDAL = new transactionDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();

        DataTable transactionDT = new DataTable();
        private void frmPurchaseAndSales_Load(object sender, EventArgs e)
        {
            //Get the transactionType value from frmUserDashboard
            string type = FrmAdminDashboard.transactionType;
            //Set the value on lblTop
            lblTop.Text = type;

            //Specify Columns for our TransactionDataTable
            transactionDT.Columns.Add("Product Name");
            transactionDT.Columns.Add("Rate");
            transactionDT.Columns.Add("Quantity");
            transactionDT.Columns.Add("Total");
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keyword fro the text box
            string keyword = txtSearch.Text;

            if (keyword == "")
            {
                //Clear all the textboxes
                txtName.Text = "";
                txtEmail.Text = "";
                txtContact.Text = "";
                txtAddress.Text = "";
                return;
            }

            //Write the code to get the details and set the value on text boxes
            DeaCustBLL dc = dcDAL.SearchDealerCustomerForTransaction(keyword);

            //Now transfer or set the value from DeCustBLL to textboxes
            txtName.Text = dc.name;
            txtEmail.Text = dc.email;
            txtContact.Text = dc.contact;
            txtAddress.Text = dc.address;
        }
        private void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keyword from productsearch textbox
            string keyword = txtProductSearch.Text;

            //Check if we have value to txtSearchProduct or not
            if (keyword == "")
            {
                txtProductName.Text = "";
                txtInventory.Text = "";
                txtRate.Text = "";
                txtQuantity.Text = "";
                return;
            }

            //Search the product and display on respective textboxes
            ProductsBLL p = pDAL.GetProductsForTransaction(keyword);

            //Set the values on textboxes based on p object
            txtProductName.Text = p.name;
            txtInventory.Text = p.qty.ToString();
            txtRate.Text = p.rate.ToString();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Select the product first. Try Again.");
                return;
            }

            if (!decimal.TryParse(txtRate.Text, out decimal rate))
            {
                MessageBox.Show("Please enter a valid decimal number for Rate.");
                return;
            }

            if (!decimal.TryParse(txtQuantity.Text, out decimal qty))
            {
                MessageBox.Show("Please enter a valid decimal number for Quantity.");
                return;
            }

            if (!decimal.TryParse(txtSubTotal.Text, out decimal subTotal))
            {
                subTotal = 0; // Initialize subtotal to 0 if the text box is empty or invalid
            }

            // Calculate total
            decimal total = rate * qty;

            // Update subtotal
            subTotal += total;

            // Add product to the DataGridView
            transactionDT.Rows.Add(txtProductName.Text, rate, qty, total);
            dgvAddedProducts.DataSource = transactionDT;

            // Display the updated subtotal
            txtSubTotal.Text = subTotal.ToString();

            // Clear the textboxes
            txtProductSearch.Text = "";
            txtProductName.Text = "";
            txtInventory.Text = "0.00";
            txtRate.Text = "0.00";
            txtQuantity.Text = "0.00";
        }
        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            //Get the value fro discount textbox
            string value = txtDiscount.Text;

            if (value == "")
            {
                //Display Error Message
                MessageBox.Show("Please Add Discount First");
                txtGrandTotal.Clear();
            }
            else
            {
                //Get the discount in decimal value
                decimal subTotal = decimal.Parse(txtSubTotal.Text);
                decimal discount = decimal.Parse(txtDiscount.Text);

                //Calculate the grandtotal based on discount
                decimal grandTotal = ((100 - discount) / 100) * subTotal;

                //Display the GrandTotla in TextBox
                txtGrandTotal.Text = grandTotal.ToString();
            }
        }

        private decimal originalGrandTotal = 0; // To store the original grand total
        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            // Check if the grandTotal has value or not if it has no value then calculate the discount first
            if (string.IsNullOrEmpty(txtGrandTotal.Text))
            {
                // Display the error message to calculate discount
                MessageBox.Show("Calculate the discount and set the Grand Total first.");
            }
            else
            {
                // Parse the current grand total once if not already done
                if (originalGrandTotal == 0)
                {
                    originalGrandTotal = decimal.Parse(txtGrandTotal.Text);
                }

                // Check if the VAT textbox is not empty and contains a valid number
                if (decimal.TryParse(txtVat.Text, out decimal vat))
                {
                    // Calculate VAT
                    decimal grandTotalWithVAT = ((100 + vat) / 100) * originalGrandTotal;

                    // Displaying new grand total with VAT
                    txtGrandTotal.Text = grandTotalWithVAT.ToString();
                }
                else
                {
                    // If VAT textbox is cleared, reset the grand total to the original value
                    txtGrandTotal.Text = originalGrandTotal.ToString();

                    // Display the error message if VAT value is not valid
                    MessageBox.Show("Please enter a valid VAT percentage.");
                }
            }
        }
        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            //Check if the grand total textbox is not empty and contains a valid number
            if (decimal.TryParse(txtGrandTotal.Text, out decimal grandTotal))
            {
                //Check if the Paid Amount textbox is not empty and contains a valid number
                if (decimal.TryParse(txtPaidAmount.Text, out decimal paidAmount))
                {
                    decimal returnAmount = paidAmount - grandTotal;

                    //Display the return amount as well
                    txtReturnAmount.Text = returnAmount.ToString();
                }
                else
                {
                    //Clear the return amount textbox if the paid amount is not valid
                    txtReturnAmount.Text = "0.00";
                }
            }
            else
            {
                //Clear the return amount textbox if the grand total is not valid
                txtReturnAmount.Text = "0.00";
            }
        }

        private void lblSave_Click(object sender, EventArgs e)
        {
            // Assuming transactionBLL, dcDAL, uDAL, tDAL, pDAL, tdDAL, etc. are properly instantiated

            transactionBLL transaction = new transactionBLL
            {
                type = lblTop.Text,
                deal_cust_id = dcDAL.GetDeaCustIDFromName(txtName.Text).deal_cust_id,
                grandTotal = Math.Round(decimal.Parse(txtGrandTotal.Text), 2),
                transaction_date = DateTime.Now,
                tax = ParseDecimal(txtVat.Text),
                discount = ParseDecimal(txtDiscount.Text),
                added_by = uDAL.GetIDFromUsername(frmLogin.loggedIn).id,
                transactionDetails = transactionDT
            };

            int transactionID = -1;
            bool success = false;

            using (TransactionScope scope = new TransactionScope())
            {
                bool w = tDAL.Insert_Transaction(transaction, out transactionID);

                foreach (DataRow row in transactionDT.Rows)
                {
                    var transactionDetail = new transactionDetailBLL
                    {
                        product_id = pDAL.GetProductIDFromName(row[0].ToString()).id,
                        rate = decimal.Parse(row[1].ToString()),
                        qty = decimal.Parse(row[2].ToString()),
                        total = Math.Round(decimal.Parse(row[3].ToString()), 2),
                        deal_cust_id = dcDAL.GetDeaCustIDFromName(txtName.Text).deal_cust_id,
                        added_date = DateTime.Now,
                        added_by = uDAL.GetIDFromUsername(frmLogin.loggedIn).id
                    };

                    bool x = lblTop.Text == "Purchase"
                        ? pDAL.IncreaseProduct(transactionDetail.product_id, transactionDetail.qty)
                        : pDAL.DecreaseProduct(transactionDetail.product_id, transactionDetail.qty);

                    bool y = tdDAL.InsertTransactionDetail(transactionDetail);
                    success = w && x && y;
                }

                if (success)
                {
                    scope.Complete();
                    MessageBox.Show("Transaction Completed Successfully");

                    // Clear input fields
                    ClearInputFields();

                    // Show the Crystal Report
                    ShowReport(transactionID);
                }
                else
                {
                    MessageBox.Show("Transaction Failed");
                }
            }
        }

        private decimal ParseDecimal(string input)
        {
            return decimal.TryParse(input, out decimal result) ? result : 0;
        }

        private void ClearInputFields()
        {
            dgvAddedProducts.DataSource = null;
            dgvAddedProducts.Rows.Clear();

            txtSearch.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            txtProductSearch.Text = "";
            txtProductName.Text = "";
            txtInventory.Text = "0";
            txtRate.Text = "0";
            txtQuantity.Text = "0";
            txtSubTotal.Text = "0";
            txtDiscount.Text = "0";
            txtVat.Text = "0";
            txtGrandTotal.Text = "0";
            txtPaidAmount.Text = "0";
            txtReturnAmount.Text = "0";
        }

        private void ShowReport(int id)
        {
            ReportDocument reportDocument = new ReportDocument();
            string reportPath = "G:\\Billing Software\\WindowsFormsApp1\\TransReport.rpt";

            if (File.Exists(reportPath))
            {
                reportDocument.Load(reportPath);
                reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "Billing");

                // Update with the correct field name
                reportDocument.RecordSelectionFormula = "{tbl_transaction_detail.id} = " + id;

                frmReportPrint reportForm = new frmReportPrint();
                reportForm.crystalReportViewer1.ReportSource = reportDocument;
                reportForm.Show();
            }
            else
            {
                MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvAddedProducts_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Check if the clicked area is valid (not the header row itself)
            if (e.RowIndex >= 0)
            {
                int RowIndex = e.RowIndex;

                // Populate textboxes with row data
                txtProductName.Text = dgvAddedProducts.Rows[RowIndex].Cells[0].Value.ToString();
                txtRate.Text = dgvAddedProducts.Rows[RowIndex].Cells[1].Value.ToString();
                txtQuantity.Text = dgvAddedProducts.Rows[RowIndex].Cells[2].Value.ToString();
                txtGrandTotal.Text = dgvAddedProducts.Rows[RowIndex].Cells[3].Value.ToString(); // Assuming you have a Total column

                // Remove the row from the DataGridView
                dgvAddedProducts.Rows.RemoveAt(RowIndex);

                // Clear selection in the DataGridView
                dgvAddedProducts.ClearSelection();

                // Optionally clear other textboxes
                txtSubTotal.Clear();
                txtGrandTotal.Clear();
            }
        }

        private void btnreport_Click(object sender, EventArgs e)
        {
            frmReportPrint report = new frmReportPrint();
            report.Show();
        }
    }
}
    

