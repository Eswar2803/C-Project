using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System;
using WindowsFormsApp1.BLL;

internal class loginDAL
{
    static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

    public bool loginCheck(loginBLL l)
    {
        bool isSuccess = false;

        // Step 1: Connect Database
        using (SqlConnection conn = new SqlConnection(myconnstrng))
        {
            try
            {
                string sql = "SELECT * FROM tbl_users WHERE username=@username AND password=@password AND user_type=@user_type";

                // Step 2: Create SqlCommand object
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", l.Username);
                cmd.Parameters.AddWithValue("@password", l.Password);
                cmd.Parameters.AddWithValue("@user_type", l.User_type);

                // Step 3: Open connection and execute query
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd); // Pass cmd to the SqlDataAdapter constructor

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Step 4: Check if any rows were returned
                if (dt.Rows.Count > 0)
                {
                    // Login successful
                    isSuccess = true;
                }
                else
                {
                    // Login failed
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        return isSuccess;
    }
}
