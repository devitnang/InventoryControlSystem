using InventoryControlSystem.Models;
using MongoDB.Driver;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace InventoryControlSystem.Forms
{
    public partial class RegisterForm : Form
    {
        private IMongoCollection<User> userCollection;

        public RegisterForm()
        {
            InitializeComponent();

            // Use the parameterless helper that returns the users collection
            userCollection = MongoDBHelper.GetUserCollection("users");
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // go back to login form
            var loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // Hide password characters
            txtPassword.UseSystemPasswordChar = true;
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // show/hide password characters
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text; // do not Trim password

            // If you have ComboBoxes for role/status in designer, they should be named cboRole, cboStatus.
            // Use their selected values when available; otherwise use defaults.
            string role = "User";
            if (this.Controls.ContainsKey("cboRole"))
            {
                var cbo = this.Controls["cboRole"] as ComboBox;
                if (cbo != null && cbo.SelectedItem != null)
                    role = cbo.SelectedItem.ToString();
            }

            string status = "Active";
            if (this.Controls.ContainsKey("cboStatus"))
            {
                var cbo = this.Controls["cboStatus"] as ComboBox;
                if (cbo != null && cbo.SelectedItem != null)
                    status = cbo.SelectedItem.ToString();
            }

            // Validation
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if username already exists (case-insensitive)
            var existing = userCollection.Find(u => u.Username.ToLower() == username.ToLower()).FirstOrDefault();
            if (existing != null)
            {
                MessageBox.Show("Username already exists. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hash password
            string hashedPassword = HashPassword(password);

            // Create user object (match your User.cs fields)
            var newUser = new User
            {
                FullName = fullName,
                Username = username,
                PasswordHash = hashedPassword,
                Role = role,
                Status = status,
                IsActive = string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase),
                CreatedAt = DateTime.UtcNow
            };

            // Save last registered username
            UserStorage.SaveLastUser(username);

            // Insert into MongoDB
            try
            {
                userCollection.InsertOne(newUser);
                MessageBox.Show("User registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //// ---- AUTO LOGIN ----
            //main mainForm = new main(newUser); // Pass the new user to main form
            //mainForm.ShowDialog();

            //// Close register form
            //this.Hide();
            //this.Close();

            // AUTO LOGIN TO MAIN FORM AND CLOSE REGISTER FORM
            var mainForm = new main(newUser); // Pass the new user to main form
            mainForm.ShowDialog();
            this.Hide();
            this.Close();

        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (var b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private void ClearFields()
        {
            txtFullName.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            if (this.Controls.ContainsKey("cboRole"))
            {
                var cbo = this.Controls["cboRole"] as ComboBox;
                if (cbo != null) cbo.SelectedIndex = -1;
            }
            if (this.Controls.ContainsKey("cboStatus"))
            {
                var cbo = this.Controls["cboStatus"] as ComboBox;
                if (cbo != null) cbo.SelectedIndex = -1;
            }
            txtFullName.Focus();
        }
    }
}
