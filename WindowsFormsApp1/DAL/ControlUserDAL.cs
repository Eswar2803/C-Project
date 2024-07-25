using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp1.DAL
{
    internal class ControlUserDAL
    {
        string connectionString = "YourConnectionStringHere"; // Replace with your actual database connection string

        public List<string> GetUserPermissions(int userID)
        {
            List<string> permissions = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT MenuName FROM Permissions WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string menuName = reader["MenuName"].ToString();
                        permissions.Add(menuName);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Handle exceptions as needed
                    Console.WriteLine("Error fetching permissions: " + ex.Message);
                }
            }

            return permissions;
        }

        public void SetUserPermissions(int userID, List<string> permissions)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // First, delete existing permissions for the user
                    string deleteQuery = "DELETE FROM Permissions WHERE UserID = @UserID";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@UserID", userID);
                    deleteCommand.ExecuteNonQuery();

                    // Then, insert new permissions
                    string insertQuery = "INSERT INTO Permissions (UserID, MenuName) VALUES (@UserID, @MenuName)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                    foreach (string menuName in permissions)
                    {
                        insertCommand.Parameters.Clear();
                        insertCommand.Parameters.AddWithValue("@UserID", userID);
                        insertCommand.Parameters.AddWithValue("@MenuName", menuName);
                        insertCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions as needed
                    Console.WriteLine("Error setting permissions: " + ex.Message);
                }
            }
        }
    }
}
