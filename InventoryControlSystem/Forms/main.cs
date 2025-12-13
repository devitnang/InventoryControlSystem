using InventoryControlSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace InventoryControlSystem.Forms
{
    public partial class main : Form
    {
        // UserControls
        UC_Home home;
        UC_Category category;
        UC_Product product;

        // Runtime content panel to host UserControls (designer's mainPanel is the sidebar)
        private Panel contentPanel;

        // -------------------------
        // MAIN CONSTRUCTORS
        // -------------------------

        public main()
        {
            InitializeComponent();
            InitializeUserControls();
        }

        public main(User user)   // Called from Login form
        {
            InitializeComponent();
            InitializeUserControls();

            // Optional: show username or role
            if (lblUserName != null)
            {
                lblUserName.Text = user.Username;
            }
        }

        // -------------------------
        // Initialize all UserControls
        // -------------------------
        private void InitializeUserControls()
        {
            // Ensure designer mainPanel exists (it is used as the left menu/sidebar).
            // Create a separate contentPanel at runtime to host the UserControls so we don't
            // overwrite the sidebar buttons that are already placed in designer's mainPanel.
            if (contentPanel == null)
            {
                contentPanel = new Panel
                {
                    Name = "contentPanel",
                    BackColor = SystemColors.ControlLight,
                    AutoScroll = true,
                    // initial position: to the right of the sidebar (mainPanel)
                    Location = new Point(mainPanel.Right + 12, mainPanel.Top),
                    // initial size: fill remaining client area (will be anchored)
                    Size = new Size(Math.Max(100, ClientSize.Width - mainPanel.Right - 24), Math.Max(100, ClientSize.Height - mainPanel.Top - 12)),
                    Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
                };

                // Add to the form controls. Ensure sidebar (mainPanel) remains visible above it.
                Controls.Add(contentPanel);
                Controls.SetChildIndex(mainPanel, 0);
            }

            // create instances once
            home = new UC_Home();
            category = new UC_Category();
            product = new UC_Product();

            // Ensure controls are not visible until added to panel
            home.Visible = false;
            category.Visible = false;
            product.Visible = false;

            LoadUserControl(home);
        }

        // -------------------------
        // Load UC into contentPanel
        // -------------------------
        private void LoadUserControl(UserControl uc)
        {
            if (uc == null || contentPanel == null)
            {
                return;
            }

            // Optimize layout changes
            contentPanel.SuspendLayout();

            // Clear existing controls and add selected control
            contentPanel.Controls.Clear();

            // Configure the user control for full fill and visibility
            uc.Dock = DockStyle.Fill;
            uc.Visible = true;

            contentPanel.Controls.Add(uc);

            // Ensure the control is on top and the panel is refreshed
            uc.BringToFront();
            contentPanel.ResumeLayout();
            contentPanel.Refresh();
        }

        // -------------------------
        // MENU BUTTON EVENTS
        // -------------------------
        private void btnHome_Click(object sender, EventArgs e)
        {
            LoadUserControl(home);
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            LoadUserControl(category);
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            LoadUserControl(product);
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to sign out?", 
                "Confirm Sign Out", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            MessageBox.Show(
                "You have been signed out.", 
                "Signed Out", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);

            this.Hide();

            using (var loginForm = new LoginForm())
            {
                loginForm.ShowDialog();
            }
        }
    }
}
