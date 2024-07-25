using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.BLL;

namespace WindowsFormsApp1.DAL
{
    internal class userDAL
    {
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        #region Select Data from Database
        //----------------------------- Selecting data from database--------------------------
        public DataTable Select()
        {
            // Step1: Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                // Step 2: Writing SQL Query
                string sql = "SELECT * FROM tbl_users";
                // Creating cmd using sql and conn
                SqlCommand cmd = new SqlCommand(sql, conn);
                // Creating SqlDataAdapter using cmd
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Connection open
                conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                // Handle exception (optional)
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        #endregion

        #region Insert Data into Database
        //----------------------- Inserting data into Database-------------------------------
        public bool Insert(userBLL u)
        {
            // Creating a default return type and setting its value to false 
            bool isSuccess = false;
            // Step 1: Connect Database
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {
              String sql = "INSERT INTO tbl_users (first_name, last_name, email,username,password,contact, address,gender,user_type,added_date,added_by) VALUES (@first_name, @last_name, @email,@username,@password,@contact,@address,@gender,@user_type,@added_date,@added_by)";
              SqlCommand command = new SqlCommand(sql, conn);

                // Create Parameters to add data
                SqlCommand cmd = new SqlCommand(sql, conn);

                // Adding parameters
                cmd.Parameters.AddWithValue("@first_name", u.first_name);
                cmd.Parameters.AddWithValue("@last_name", u.last_name);
                cmd.Parameters.AddWithValue("@email", u.email);
                cmd.Parameters.AddWithValue("@username", u.username);
                cmd.Parameters.AddWithValue("@password", u.password);
                cmd.Parameters.AddWithValue("@contact", u.contact);
                cmd.Parameters.AddWithValue("@address", u.address);
                cmd.Parameters.AddWithValue("@gender", u.gender);
                cmd.Parameters.AddWithValue("@user_type", u.user_type);
                cmd.Parameters.AddWithValue("@added_date", u.added_date);
                cmd.Parameters.AddWithValue("@added_by", u.added_by);
                
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                // If the query runs successfully then the value of rows will be greater than zero else it will be zero
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                conn.Close();
            }
            return isSuccess;
        }
        #endregion

        #region Update Data in Database
        public bool Update(userBLL u)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                string sql = @"UPDATE tbl_users 
                       SET first_name = @first_name, 
                           last_name = @last_name, 
                           email = @email, 
                           username = @username, 
                           password = @password, 
                           contact = @contact, 
                           address = @address, 
                           gender = @gender, 
                           user_type = @user_type, 
                           added_date = @added_date, 
                           added_by = @added_by 
                       WHERE id = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                // Adding parameters
                cmd.Parameters.AddWithValue("@first_name", u.first_name);
                cmd.Parameters.AddWithValue("@last_name", u.last_name);
                cmd.Parameters.AddWithValue("@email", u.email);
                cmd.Parameters.AddWithValue("@username", u.username);
                cmd.Parameters.AddWithValue("@password", u.password);
                cmd.Parameters.AddWithValue("@contact", u.contact);
                cmd.Parameters.AddWithValue("@address", u.address);
                cmd.Parameters.AddWithValue("@gender", u.gender);
                cmd.Parameters.AddWithValue("@user_type", u.user_type);
                cmd.Parameters.AddWithValue("@added_date", u.added_date);
                cmd.Parameters.AddWithValue("@added_by", u.added_by);
                cmd.Parameters.AddWithValue("@id", u.id);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                // If the query runs successfully, rows will be greater than 0
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }
        #endregion
      
        #region Delete Data from Database
        public bool Delete(userBLL u)
        {
            bool isSuccess = false;
            //Sql Connection
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {
                string sql = "DELETE FROM tbl_users WHERE id = @id";

                // SqlCommand to execute the query
                SqlCommand cmd = new SqlCommand(sql, conn);

                // Adding parameters
                cmd.Parameters.AddWithValue("@id", u.id); 

                // Open the connection
                conn.Open();

                // Execute the query
                int rows = cmd.ExecuteNonQuery();

                // Check if the query executed successfully
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            { 
            }

            finally 
            {
            }
                return isSuccess;
        }
        #endregion

        #region Search User On Database using keywords
        public DataTable Search(string keywords)
        {
            // Step1: Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                // Step 2: Writing SQL Query
                string sql = "SELECT * FROM tbl_users WHERE id LIKE '%" + keywords + "%' OR first_name LIKE '%" + keywords + "%' OR last_name LIKE '%" + keywords + "%' OR username LIKE '%" + keywords + "%'";
                // Creating cmd using sql and conn
                SqlCommand cmd = new SqlCommand(sql, conn);
                // Creating SqlDataAdapter using cmd
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Connection open
                conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                // Handle exception (optional)
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        #endregion

        #region Getting User ID from Username
        public userBLL GetIDFromUsername(string username)
        {
            userBLL u = new userBLL();
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT id FROM tbl_users WHERE username='" + username + "'";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                conn.Open();

                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    u.id = int.Parse(dt.Rows[0]["id"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return u;
        }
        #endregion
    }
}
