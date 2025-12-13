using InventoryControlSystem.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace InventoryControlSystem.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // close this form
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text; // don't trim password (some systems allow spaces)

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // get collection
                var col = MongoDBHelper.GetCollection<User>("users");

                // find user case-insensitively (safer)
                var filter = Builders<User>.Filter.Eq(u => u.Username, username)
                             | Builders<User>.Filter.Eq(u => u.Username, username.ToLower())
                             | Builders<User>.Filter.Eq(u => u.Username, username.ToUpper());

                // simpler: try exact first, then fallback to case-insensitive using regex
                var user = col.Find(u => u.Username == username).FirstOrDefault();
                if (user == null)
                {
                    // fallback: case-insensitive regex search
                    var regexFilter = Builders<User>.Filter.Regex(u => u.Username, new MongoDB.Bson.BsonRegularExpression("^" + RegexEscape(username) + "$", "i"));
                    user = col.Find(regexFilter).FirstOrDefault();
                }

                if (user == null)
                {
                    MessageBox.Show("User not found. Please verify username.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Debug info: tell which password field exists (remove in production)
                bool hasHash = !string.IsNullOrEmpty(user.PasswordHash);
                bool hasPlain = typeof(User).GetProperty("Password") != null && // property exists on model
                                user.GetType().GetProperty("Password") != null &&
                                user.GetType().GetProperty("Password").GetValue(user) != null;

                // Prefer hashed check if present
                if (hasHash)
                {
                    string enteredHash = ComputeSha256Hash(password);
                    if (!string.Equals(enteredHash, user.PasswordHash, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Incorrect password (hashed check failed).", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (user.GetType().GetProperty("Password") != null) // check plaintext property if available in model/object
                {
                    var plainObj = user.GetType().GetProperty("Password").GetValue(user) as string;
                    if (plainObj == null || !string.Equals(plainObj, password))
                    {
                        MessageBox.Show("Incorrect password (plaintext check failed).", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    // If your C# model doesn't expose 'Password' but DB has 'password' field,
                    // try fetching via BsonDocument fallback
                    var colRaw = MongoDBHelper.GetCollection<BsonDocument>("users");
                    var doc = colRaw.Find(Builders<BsonDocument>.Filter.Eq("username", user.Username)).FirstOrDefault();
                    if (doc != null)
                    {
                        if (doc.Contains("passwordHash"))
                        {
                            string enteredHash = ComputeSha256Hash(password);
                            if (!enteredHash.Equals(doc["passwordHash"].AsString, StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show("Incorrect password (passwordHash in DB mismatch).", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else if (doc.Contains("password"))
                        {
                            if (!doc["password"].AsString.Equals(password))
                            {
                                MessageBox.Show("Incorrect password (plaintext password in DB mismatch).", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("No password field found for this user in DB. Check your DB document.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("User found but unable to read password fields from DB.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // check active/status (be flexible)
                bool active = user.IsActive;
                if (!active && !string.Equals(user.Status, "Active", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Account is inactive/disabled.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Success
                this.Hide();
                var mainForm = new main(user);
                mainForm.FormClosed += (s2, args2) => this.Close();
                mainForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string RegexEscape(string input)
        {
            return System.Text.RegularExpressions.Regex.Escape(input);
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                var sb = new StringBuilder();
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // Hide password characters
            txtPassword.UseSystemPasswordChar = true;
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim().Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                txtPassword.Focus();
            }

            
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
                e.SuppressKeyPress = true; // stops beep sound
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
                e.SuppressKeyPress = true; // stops beep sound
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            //Show password
            if (chkShowPassword.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Open registration form, When go to register form, close login form
            var registerForm = new RegisterForm();
            //registerForm.FormClosed += (s2, args2) => this.Close();
            registerForm.Show();
            this.Hide();
        }

        public void SetUsername(string username)
        {
            txtUsername.Text = username;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            string LastUser = UserStorage.LoadLastUsername();   
            if (!string.IsNullOrEmpty(LastUser))
            {
                txtUsername.Text = LastUser;
            }
        }
    }
}
