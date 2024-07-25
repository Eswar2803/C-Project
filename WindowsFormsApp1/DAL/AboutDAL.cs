using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.BLL;

namespace WindowsFormsApp1.DAL
{
    internal class frmaboutDAL
    {

        //Static String Method for Database Connection String
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region Insert into database
        public bool InsertCompanyDetails(frmaboutBLL company)
        {
            bool isSuccess = false;
            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                string query = "INSERT INTO tbl_about (Name, Address, Phone, Mail, Website, Managing_Director) VALUES (@Name, @Address, @Phone, @Mail, @Website, @Managing_Director)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Name", company.Name);
                cmd.Parameters.AddWithValue("@Address", company.Address);
                cmd.Parameters.AddWithValue("@Phone", company.Phone);
                cmd.Parameters.AddWithValue("@Mail", company.Mail);
                cmd.Parameters.AddWithValue("@Website", company.Website);
                cmd.Parameters.AddWithValue("@Managing_Director", company.ManagingDirector);

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isSuccess = rows > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return isSuccess;
        }
        #endregion
        #region Select the data from database
        public frmaboutBLL GetCompanyDetails()
        {
            frmaboutBLL company = new frmaboutBLL();
            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                string query = "SELECT TOP 1 * FROM tbl_about ORDER BY id DESC";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        company.ID = Convert.ToInt32(reader["id"]);
                        company.Name = reader["Name"].ToString();
                        company.Address = reader["address"].ToString();
                        company.Phone = reader["phone"].ToString();
                        company.Mail = reader["mail"].ToString();
                        company.Website = reader["website"].ToString();
                        company.ManagingDirector = reader["managing_director"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return company;
        }
        #endregion
       #region Update data into database
        public bool UpdateCompanyDetails(frmaboutBLL company)
        {
            bool isSuccess = false;
            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                string query = "UPDATE tbl_about SET Name = @Name, Address = @Address, Phone = @Phone, Mail = @Mail, Website = @Website, Managing_Director = @Managing_Director WHERE id = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ID", company.ID);
                cmd.Parameters.AddWithValue("@Name", company.Name);
                cmd.Parameters.AddWithValue("@Address", company.Address);
                cmd.Parameters.AddWithValue("@Phone", company.Phone);
                cmd.Parameters.AddWithValue("@Mail", company.Mail);
                cmd.Parameters.AddWithValue("@Website", company.Website);
                cmd.Parameters.AddWithValue("@Managing_Director", company.ManagingDirector);

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isSuccess = rows > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating company details: " + ex.Message);
                }
            }
            return isSuccess;
        }
        #endregion

    }
}
