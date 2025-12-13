namespace InventoryControlSystem.Forms
{
    partial class main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            this.mainPanel = new System.Windows.Forms.Panel();
            this.lblUserName = new System.Windows.Forms.Label();
            this.btnSignOut = new Guna.UI2.WinForms.Guna2Button();
            this.btnProduct = new Guna.UI2.WinForms.Guna2Button();
            this.btnCategory = new Guna.UI2.WinForms.Guna2Button();
            this.btnHome = new Guna.UI2.WinForms.Guna2Button();
            this.guna2CirclePictureBox2 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.mainPanel.AutoScroll = true;
            this.mainPanel.AutoSize = true;
            this.mainPanel.BackColor = System.Drawing.Color.Thistle;
            this.mainPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainPanel.Controls.Add(this.btnSignOut);
            this.mainPanel.Controls.Add(this.btnProduct);
            this.mainPanel.Controls.Add(this.btnCategory);
            this.mainPanel.Controls.Add(this.lblUserName);
            this.mainPanel.Controls.Add(this.btnHome);
            this.mainPanel.Controls.Add(this.guna2CirclePictureBox2);
            this.mainPanel.Location = new System.Drawing.Point(12, 12);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(293, 646);
            this.mainPanel.TabIndex = 9;
            // 
            // lblUserName
            // 
            this.lblUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(76, 181);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(155, 38);
            this.lblUserName.TabIndex = 10;
            this.lblUserName.Text = "Name User";
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSignOut
            // 
            this.btnSignOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSignOut.Animated = true;
            this.btnSignOut.AutoRoundedCorners = true;
            this.btnSignOut.BackColor = System.Drawing.Color.Transparent;
            this.btnSignOut.BorderColor = System.Drawing.Color.BlueViolet;
            this.btnSignOut.BorderThickness = 3;
            this.btnSignOut.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSignOut.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSignOut.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSignOut.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSignOut.FillColor = System.Drawing.Color.DarkOrchid;
            this.btnSignOut.Font = new System.Drawing.Font("Segoe UI", 16.2F);
            this.btnSignOut.ForeColor = System.Drawing.Color.White;
            this.btnSignOut.HoverState.BorderColor = System.Drawing.Color.DarkOrchid;
            this.btnSignOut.HoverState.CustomBorderColor = System.Drawing.Color.DarkOrchid;
            this.btnSignOut.HoverState.FillColor = System.Drawing.Color.White;
            this.btnSignOut.HoverState.ForeColor = System.Drawing.Color.DarkOrchid;
            this.btnSignOut.HoverState.Image = global::InventoryControlSystem.Properties.Resources.Logout_Rounded;
            this.btnSignOut.Image = global::InventoryControlSystem.Properties.Resources.Logout_Rounded1;
            this.btnSignOut.ImageAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnSignOut.ImageSize = new System.Drawing.Size(35, 35);
            this.btnSignOut.Location = new System.Drawing.Point(32, 594);
            this.btnSignOut.Name = "btnSignOut";
            this.btnSignOut.Size = new System.Drawing.Size(217, 49);
            this.btnSignOut.TabIndex = 13;
            this.btnSignOut.Text = "Sign Out";
            this.btnSignOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnSignOut.UseTransparentBackground = true;
            this.btnSignOut.Click += new System.EventHandler(this.btnSignOut_Click);
            // 
            // btnProduct
            // 
            this.btnProduct.Animated = true;
            this.btnProduct.AutoRoundedCorners = true;
            this.btnProduct.BackColor = System.Drawing.Color.Transparent;
            this.btnProduct.BorderColor = System.Drawing.Color.BlueViolet;
            this.btnProduct.BorderThickness = 3;
            this.btnProduct.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnProduct.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnProduct.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnProduct.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnProduct.FillColor = System.Drawing.Color.DarkOrchid;
            this.btnProduct.Font = new System.Drawing.Font("Segoe UI", 16.2F);
            this.btnProduct.ForeColor = System.Drawing.Color.White;
            this.btnProduct.HoverState.BorderColor = System.Drawing.Color.DarkOrchid;
            this.btnProduct.HoverState.CustomBorderColor = System.Drawing.Color.DarkOrchid;
            this.btnProduct.HoverState.FillColor = System.Drawing.Color.White;
            this.btnProduct.HoverState.ForeColor = System.Drawing.Color.DarkOrchid;
            this.btnProduct.HoverState.Image = global::InventoryControlSystem.Properties.Resources.Product;
            this.btnProduct.Image = global::InventoryControlSystem.Properties.Resources.Product1;
            this.btnProduct.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnProduct.ImageSize = new System.Drawing.Size(35, 35);
            this.btnProduct.Location = new System.Drawing.Point(32, 367);
            this.btnProduct.Name = "btnProduct";
            this.btnProduct.Size = new System.Drawing.Size(217, 49);
            this.btnProduct.TabIndex = 12;
            this.btnProduct.Text = "Product";
            this.btnProduct.UseTransparentBackground = true;
            this.btnProduct.Click += new System.EventHandler(this.btnProduct_Click);
            // 
            // btnCategory
            // 
            this.btnCategory.Animated = true;
            this.btnCategory.AutoRoundedCorners = true;
            this.btnCategory.BackColor = System.Drawing.Color.Transparent;
            this.btnCategory.BorderColor = System.Drawing.Color.BlueViolet;
            this.btnCategory.BorderThickness = 3;
            this.btnCategory.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCategory.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCategory.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCategory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCategory.FillColor = System.Drawing.Color.DarkOrchid;
            this.btnCategory.Font = new System.Drawing.Font("Segoe UI", 16.2F);
            this.btnCategory.ForeColor = System.Drawing.Color.White;
            this.btnCategory.HoverState.BorderColor = System.Drawing.Color.DarkOrchid;
            this.btnCategory.HoverState.CustomBorderColor = System.Drawing.Color.DarkOrchid;
            this.btnCategory.HoverState.FillColor = System.Drawing.Color.White;
            this.btnCategory.HoverState.ForeColor = System.Drawing.Color.DarkOrchid;
            this.btnCategory.HoverState.Image = global::InventoryControlSystem.Properties.Resources.Category;
            this.btnCategory.Image = global::InventoryControlSystem.Properties.Resources.Category1;
            this.btnCategory.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnCategory.ImageSize = new System.Drawing.Size(35, 35);
            this.btnCategory.Location = new System.Drawing.Point(32, 312);
            this.btnCategory.Name = "btnCategory";
            this.btnCategory.Size = new System.Drawing.Size(217, 49);
            this.btnCategory.TabIndex = 11;
            this.btnCategory.Text = "Category";
            this.btnCategory.UseTransparentBackground = true;
            this.btnCategory.Click += new System.EventHandler(this.btnCategory_Click);
            // 
            // btnHome
            // 
            this.btnHome.Animated = true;
            this.btnHome.AutoRoundedCorners = true;
            this.btnHome.BackColor = System.Drawing.Color.Transparent;
            this.btnHome.BorderColor = System.Drawing.Color.BlueViolet;
            this.btnHome.BorderThickness = 3;
            this.btnHome.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHome.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHome.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHome.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHome.FillColor = System.Drawing.Color.DarkOrchid;
            this.btnHome.Font = new System.Drawing.Font("Segoe UI", 16.2F);
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.HoverState.BorderColor = System.Drawing.Color.DarkOrchid;
            this.btnHome.HoverState.CustomBorderColor = System.Drawing.Color.DarkOrchid;
            this.btnHome.HoverState.FillColor = System.Drawing.Color.White;
            this.btnHome.HoverState.ForeColor = System.Drawing.Color.DarkOrchid;
            this.btnHome.HoverState.Image = global::InventoryControlSystem.Properties.Resources.Home;
            this.btnHome.Image = global::InventoryControlSystem.Properties.Resources.Home1;
            this.btnHome.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnHome.ImageSize = new System.Drawing.Size(35, 35);
            this.btnHome.Location = new System.Drawing.Point(32, 257);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(217, 49);
            this.btnHome.TabIndex = 6;
            this.btnHome.Text = "Home";
            this.btnHome.UseTransparentBackground = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // guna2CirclePictureBox2
            // 
            this.guna2CirclePictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.guna2CirclePictureBox2.ErrorImage = ((System.Drawing.Image)(resources.GetObject("guna2CirclePictureBox2.ErrorImage")));
            this.guna2CirclePictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("guna2CirclePictureBox2.Image")));
            this.guna2CirclePictureBox2.ImageRotate = 0F;
            this.guna2CirclePictureBox2.InitialImage = ((System.Drawing.Image)(resources.GetObject("guna2CirclePictureBox2.InitialImage")));
            this.guna2CirclePictureBox2.Location = new System.Drawing.Point(66, 13);
            this.guna2CirclePictureBox2.Name = "guna2CirclePictureBox2";
            this.guna2CirclePictureBox2.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox2.Size = new System.Drawing.Size(165, 165);
            this.guna2CirclePictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2CirclePictureBox2.TabIndex = 8;
            this.guna2CirclePictureBox2.TabStop = false;
            this.guna2CirclePictureBox2.UseTransparentBackground = true;
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 670);
            this.Controls.Add(this.mainPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "main";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button btnHome;
        private Guna.UI2.WinForms.Guna2CirclePictureBox guna2CirclePictureBox2;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label lblUserName;
        private Guna.UI2.WinForms.Guna2Button btnCategory;
        private Guna.UI2.WinForms.Guna2Button btnProduct;
        private Guna.UI2.WinForms.Guna2Button btnSignOut;
    }
}