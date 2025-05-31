using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

using System.Windows.Forms;

namespace librarymgtsystem
{
    public partial class signup : Form
    {
        private string connectionString;

        public signup()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["LibraryDbConnection"].ConnectionString;
            txtpass.UseSystemPasswordChar = true; // Hide password input
        }

        // Helper method to hash passwords (copied from login.cs for consistency)
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void btnreg_Click(object sender, EventArgs e) // Register Button
        {
            string username = txtname.Text.Trim();
            string password = txtpass.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username and Password cannot be empty.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 6) // Enforce minimum password length
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hashedPassword = HashPassword(password);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    try
                    {
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registration successful! You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close(); // Close signup form after successful registration
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627 || ex.Number == 2601) 
                        {
                            MessageBox.Show("This username already exists. Please choose a different username.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show($"Database error during registration: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Console.WriteLine($"SQL Error during signup: {ex.Message}\n{ex.StackTrace}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine($"General Error during signup: {ex.Message}\n{ex.StackTrace}");
                    }
                }
            }
        }

        // Keep these empty unless you add specific real-time logic
        private void txtpass_TextChanged(object sender, EventArgs e) { }
        private void txtname_TextChanged(object sender, EventArgs e) { }
        private void txtid_TextChanged(object sender, EventArgs e) { } 
    }
}