using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAL;

namespace WindowsFormsApp1.UI
{
    public partial class frmTransactions : Form
    {
        public frmTransactions()
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

        transactionDAL tdal = new transactionDAL();
        private void frmTransactions_Load(object sender, EventArgs e)

        {
            //Dispay all the transactions
            DataTable dt = tdal.DisplayAllTransactions();
            dgvTransactions.DataSource = dt;
        }
        
        private void cmbTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get the Value from Combobox
            string type = cmbTransactionType.Text;

            DataTable dt = tdal.DisplayTransactionByType(type);
            dgvTransactions.DataSource = dt;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            //Dispay all the transactions
            DataTable dt = tdal.DisplayAllTransactions();
            dgvTransactions.DataSource = dt;
        }

        private void btnexprttopdf_Click(object sender, EventArgs e)
        {
            if (dgvTransactions.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "Output.pdf";
                bool fileError = false;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Create PDF Table
                        PdfPTable pdfTable = new PdfPTable(dgvTransactions.Columns.Count);
                        pdfTable.DefaultCell.Padding = 3;
                        pdfTable.WidthPercentage = 100;
                        pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                        // Add Headers from DataGridView to PDF Table
                        foreach (DataGridViewColumn column in dgvTransactions.Columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                            pdfTable.AddCell(cell);
                        }

                        // Add Rows from DataGridView to PDF Table
                        foreach (DataGridViewRow row in dgvTransactions.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                pdfTable.AddCell(cell.Value?.ToString()); // Use ?.ToString() to handle null values
                            }
                        }

                        // Create PDF Document
                        using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                        {
                            Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                            PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();

                            // Add PDF Table to Document
                            pdfDoc.Add(pdfTable);

                            // Close Document and FileStream
                            pdfDoc.Close();
                            stream.Close();

                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                    }
                    catch (IOException ex)
                    {
                        fileError = true;
                        MessageBox.Show("Error writing file to disk." + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }

                if (fileError)
                {
                    MessageBox.Show("File operation error occurred. Export failed.", "Error");
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MAXIMIZE = 0xF030;

            // Check if the message is a system command message
            if (m.Msg == WM_SYSCOMMAND)
            {
                // Check if the system command is for maximizing
                if ((int)m.WParam == SC_MAXIMIZE)
                {
                    // Do nothing to ignore the maximize command
                    return;
                }
            }

            // Call the base class method for other messages
            base.WndProc(ref m);
        }


    }
}
