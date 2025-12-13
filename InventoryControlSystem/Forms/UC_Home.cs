using InventoryControlSystem.Models;
using MongoDB.Driver;
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
    public partial class UC_Home : UserControl
    {
        private readonly IMongoCollection<Product> _productCol;
        private readonly IMongoCollection<Category> _categoryCol;


        // Dashboard count labels
        private Label lblProducts, lblCategories;

        // Layout UI elements
        private Panel panelHeader;
        private FlowLayoutPanel flowCards;

        public UC_Home()
        {
            InitializeComponent();
            BuildUI();

            // Mongo collections
            _productCol = MongoDBHelper.GetCollection<Product>("products");
            _categoryCol = MongoDBHelper.GetCollection<Category>("categories");

            LoadDashboard();
        }

        private async void LoadDashboard()
        {
            try
            {
                lblProducts.Text = (await _productCol.CountDocumentsAsync(_ => true)).ToString();
                lblCategories.Text = (await _categoryCol.CountDocumentsAsync(_ => true)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dashboard loading failed: " + ex.Message);
            }
        }

        //private Panel panelHeader;
        //private Label lblWelcome;
        //private FlowLayoutPanel flowCards;

        //private Label lblProducts, lblCategories;

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // HEADER PANEL
            Panel header = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(33, 51, 99) // same as your screenshot
            };

            Label title = new Label()
            {
                Text = "📊 Inventory Dashboard",
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 18, FontStyle.Bold),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };

            header.Controls.Add(title);
            this.Controls.Add(header);

            // MAIN PANEL - CONTAINER
            FlowLayoutPanel mainContainer = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(40),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };

            this.Controls.Add(mainContainer);

            // ADD CARDS
            mainContainer.Controls.Add(CreateCard("Products", out lblProducts, Properties.Resources.box_icon));
            mainContainer.Controls.Add(CreateCard("Categories", out lblCategories, Properties.Resources.category_icon));
        }

        // CARD BUILDER
        private Panel CreateCard(string title, out Label lblValue, Image icon)
        {
            Panel card = new Panel()
            {
                Width = 260,
                Height = 150,
                BackColor = Color.White,
                Margin = new Padding(40),
            };

            // ROUNDED BORDER
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (SolidBrush shadow = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
                {
                    e.Graphics.FillRoundedRectangle(shadow, 4, 4, card.Width - 8, card.Height - 8, 20);
                }

                using (SolidBrush bg = new SolidBrush(Color.White))
                {
                    e.Graphics.FillRoundedRectangle(bg, 0, 0, card.Width - 8, card.Height - 8, 20);
                }
            };

            // ICON
            PictureBox pb = new PictureBox()
            {
                Image = icon,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 60,
                Height = 60,
                Location = new Point(20, 20)
            };

            // TITLE
            Label lblTitle = new Label()
            {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true,
                Location = new Point(95, 25)
            };

            // VALUE LABEL
            lblValue = new Label()
            {
                Text = "0",
                Font = new Font("Segoe UI Semibold", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 33, 33),
                AutoSize = true,
                Location = new Point(95, 65)
            };

            card.Controls.Add(pb);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }


        private void UC_Home_Load(object sender, EventArgs e)
        {

        }
    }
    public static class GraphicsExtension
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, int x, int y, int width, int height, int radius)
        {
            using (var path = RoundedRect(new Rectangle(x, y, width, height), radius))
            {
                g.FillPath(brush, path);
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.X + bounds.Width - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.X + bounds.Width - d, bounds.Y + bounds.Height - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
