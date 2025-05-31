using System;
using System.Configuration; // Required for ConfigurationManager
using System.Data;
using System.Data.SqlClient; // Required for SQL Server specific classes
using System.Security.Cryptography; // For password hashing (SHA256)
using System.Text; // For encoding text

using System.Windows.Forms;

namespace librarymgtsystem
{
    public partial class login : Form
    {
        private string connectionString;

        public login()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["LibraryDbConnection"].ConnectionString;
            pass.UseSystemPasswordChar = true; // Hide password input
        }

        // Helper method to hash passwords (for both login and signup)
        // IMPORTANT: For production, use stronger hashing like BCrypt or Argon2 (via NuGet packages).
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // "x2" formats as hexadecimal
                }
                return builder.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e) // Login Button
        {
            string username = uname.Text.Trim();
            string password = pass.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hashedPassword = HashPassword(password);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    try
                    {
                        con.Open();
                        int count = (int)cmd.ExecuteScalar();

                        if (count == 1)
                        {
                            MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide(); // Hide login form
                            Mainwindow mainForm = new Mainwindow(); // Create main window instance
                            mainForm.Show(); // Show main window
                            // Optionally, this.Close(); // Close login form permanently
                        }
                        else
                        {
                            MessageBox.Show("Invalid Username or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Database error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine($"SQL Error during login: {ex.Message}\n{ex.StackTrace}"); // Log for debugging
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine($"General Error during login: {ex.Message}\n{ex.StackTrace}"); // Log for debugging
                    }
                }
            }
        }

        private void signup_Click(object sender, EventArgs e) // Sign Up Button
        {
            signup registrationForm = new signup();
            registrationForm.ShowDialog(); // Show as modal dialog
            pass.Clear(); // Clear password for security after returning from signup
        }

        // Keep these empty unless you add specific real-time logic
        private void uname_TextChanged(object sender, EventArgs e) { }
        private void pass_TextChanged(object sender, EventArgs e) { }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}