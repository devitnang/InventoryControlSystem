using InventoryControlSystem.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryControlSystem.Forms
{
    public partial class UC_Product : UserControl
    {
        private IMongoCollection<Category> _categoryCollection;
        private IMongoCollection<Product> _productCollection;

        private ObjectId editingId = ObjectId.Empty; // detect update mode
        private string selectedImageBase64 = null;

        public static event EventHandler ProductsChanged;

        public UC_Product()
        {
            InitializeComponent();

            _categoryCollection = MongoDBHelper.GetCollection<Category>("categories");
            _productCollection = MongoDBHelper.GetCollection<Product>("products");

            // Load initial data
            LoadCategoryDropdown();
            LoadProductList();

            // Subscribe to category changes so the combo updates immediately
            UC_Category.CategoriesChanged += OnCategoriesChanged;

            // Subscribe to product change notifications so this instance refreshes
            UC_Product.ProductsChanged += OnProductsChanged;

            // Initial pretty styling for datagrid
            StyleDataGridView(dgvProduct);
            //StyleDataGridView

        }

        // event handler that ensures UI-thread reload
        private void OnCategoriesChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new Action(() => LoadCategoryDropdown()));
            }
            else
            {
                LoadCategoryDropdown();
            }
        }

        // Handler that reloads on UI thread
        private void OnProductsChanged(object sender, EventArgs e)
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired)
                BeginInvoke(new Action(() => LoadProductList()));
            else
                LoadProductList();
        }

        // ensure LoadCategoryDropdown safely binds results
        private async void LoadCategoryDropdown()
        {
            try
            {
                var categories = await _categoryCollection.Find(_ => true).ToListAsync();
                cbCategory.DataSource = null;
                cbCategory.DisplayMember = "Name";
                cbCategory.ValueMember = "Id"; // or "Id.ToString()" depending on your SelectedValue handling
                cbCategory.DataSource = categories;
                cbCategory.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load categories: " + ex.Message, "Error");
            }
        }

        private async void btnAddProduct_Click(object sender, EventArgs e)
        {
            string name = txtProductName.Text.Trim();
            string purchaseStr = txtPurchasePrice.Text.Trim();
            string saleStr = txtSalePrice.Text.Trim();
            string stockStr = txtStock.Text.Trim();

            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(purchaseStr) ||
                string.IsNullOrEmpty(saleStr))
            {
                MessageBox.Show("Please fill all fields!", "Warning");
                return;
            }

            if (!decimal.TryParse(purchaseStr, out decimal purchasePrice) ||
                !decimal.TryParse(saleStr, out decimal salePrice) ||
                !int.TryParse(stockStr, out int stock))
            {
                MessageBox.Show("Invalid number format!", "Error");
                return;
            }

            Category selectedCategory = cbCategory.SelectedItem as Category;
            if (selectedCategory == null)
            {
                MessageBox.Show("Please select a category.", "Warning");
                return;
            }

            try
            {
                // UPDATE MODE
                if (editingId != ObjectId.Empty)
                {
                    var update = Builders<Product>.Update
                        .Set(p => p.Name, name)
                        .Set(p => p.CategoryId, selectedCategory.Id.ToString())
                        .Set(p => p.PurchasePrice, purchasePrice)
                        .Set(p => p.SalePrice, salePrice)
                        .Set(p => p.Stock, stock)
                        .Set(p => p.ImageBase64, selectedImageBase64);

                    await _productCollection.UpdateOneAsync(
                        p => p.Id == editingId, update
                    );

                    MessageBox.Show("Product updated successfully!");
                    btnAddProduct.Text = "Add Product";
                    editingId = ObjectId.Empty;
                }
                else // ADD NEW PRODUCT
                {
                    var newProduct = new Product
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = name,
                        CategoryId = selectedCategory.Id.ToString(),
                        PurchasePrice = purchasePrice,
                        SalePrice = salePrice,
                        Stock = stock,
                        ImageBase64 = selectedImageBase64,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _productCollection.InsertOneAsync(newProduct);
                    MessageBox.Show("Product added successfully!");
                    ClearInputs();
                    LoadProductList();

                    // Notify listeners
                    ProductsChanged?.Invoke(this, EventArgs.Empty);
                }

                ClearInputs();
                LoadProductList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving product: " + ex.Message, "Error");
            }
        }

        // View model for grid to show friendly category name and keep image base64 for conversion
        private class ProductGridItem
        {
            public ObjectId Id { get; set; }
            public string Name { get; set; }
            public string CategoryName { get; set; }
            public decimal PurchasePrice { get; set; }
            public decimal SalePrice { get; set; }
            public int Stock { get; set; }
            public string ImageBase64 { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        private async void LoadProductList()
        {
            try
            {
                var products = await _productCollection.Find(_ => true).ToListAsync();
                var categories = await _categoryCollection.Find(_ => true).ToListAsync();
                var catMap = categories.ToDictionary(c => c.Id.ToString(), c => c.Name);

                var gridItems = products.Select(p => new ProductGridItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryName = (p.CategoryId != null && catMap.ContainsKey(p.CategoryId)) ? catMap[p.CategoryId] : "(unknown)",
                    PurchasePrice = p.PurchasePrice,
                    SalePrice = p.SalePrice,
                    Stock = p.Stock,
                    ImageBase64 = p.ImageBase64,
                    CreatedAt = p.CreatedAt
                }).ToList();

                // Bind the friendly view model
                dgvProduct.DataSource = gridItems;

                // Remove or adjust autogenerated columns
                if (dgvProduct.Columns.Contains("ImageBase64"))
                    dgvProduct.Columns["ImageBase64"].Visible = false;
                if (dgvProduct.Columns.Contains("Id"))
                    dgvProduct.Columns["Id"].Visible = false;

                // Add/refresh image column and icon action columns
                AddOrUpdateImageColumn(gridItems);
                SetupGridIconButtons();

                // Final column formatting
                if (dgvProduct.Columns.Contains("PurchasePrice"))
                    dgvProduct.Columns["PurchasePrice"].DefaultCellStyle.Format = "N2";
                if (dgvProduct.Columns.Contains("SalePrice"))
                    dgvProduct.Columns["SalePrice"].DefaultCellStyle.Format = "N2";
                if (dgvProduct.Columns.Contains("CreatedAt"))
                    dgvProduct.Columns["CreatedAt"].DefaultCellStyle.Format = "g";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load products: " + ex.Message, "Error");
            }
        }

        private void AddOrUpdateImageColumn(List<ProductGridItem> products)
        {
            // Remove existing image column to avoid duplicates
            if (dgvProduct.Columns.Contains("Image"))
                dgvProduct.Columns.Remove("Image");

            // Add image column at index 0
            var imageCol = new DataGridViewImageColumn
            {
                Name = "Image",
                HeaderText = "Image",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 80
            };
            dgvProduct.Columns.Insert(0, imageCol);

            // Populate image cells from Base64
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                var row = dgvProduct.Rows[i];
                var prod = row.DataBoundItem as ProductGridItem;
                if (prod != null && !string.IsNullOrEmpty(prod.ImageBase64))
                {
                    row.Cells["Image"].Value = Base64ToImage(prod.ImageBase64);
                }
                else
                {
                    row.Cells["Image"].Value = null;
                }
            }
        }

        private void SetupGridIconButtons()
        {
            try
            {
                // Remove old icon columns to avoid duplicates
                if (dgvProduct.Columns.Contains("Edit"))
                    dgvProduct.Columns.Remove("Edit");
                if (dgvProduct.Columns.Contains("Delete"))
                    dgvProduct.Columns.Remove("Delete");

                // ---- Add Edit Image Column ----
                var editCol = new DataGridViewImageColumn
                {
                    Name = "Edit",
                    HeaderText = "",
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                    Width = 36
                };
                // Set default icon from resources (same icon for all rows)
                editCol.Image = Properties.Resources.edit_icon;
                dgvProduct.Columns.Add(editCol);

                // ---- Add Delete Image Column ----
                var deleteCol = new DataGridViewImageColumn
                {
                    Name = "Delete",
                    HeaderText = "",
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                    Width = 36
                };
                deleteCol.Image = Properties.Resources.remove_icon;
                dgvProduct.Columns.Add(deleteCol);

                // Ensure event handler is attached only once
                dgvProduct.CellClick -= dgvProduct_CellContentClickAsync;
                dgvProduct.CellClick += dgvProduct_CellContentClickAsync;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to setup grid buttons: " + ex.Message, "Error");
            }
        }

        private async void dgvProduct_CellContentClickAsync(object sender, DataGridViewCellEventArgs e)
        {
            // DataGridView Button Click (edit/delete)
            try
            {
                if (e.RowIndex < 0) return; // header
                if (e.ColumnIndex < 0) return;

                var row = dgvProduct.Rows[e.RowIndex];
                var gridItem = row.DataBoundItem as ProductGridItem;
                if (gridItem == null) return;

                var column = dgvProduct.Columns[e.ColumnIndex];
                if (column == null) return;

                if (column.Name == "Edit")
                {
                    // Load data into input fields from DB using Id to get full Product (including ImageBase64)
                    var prod = await _productCollection.Find(p => p.Id == gridItem.Id).FirstOrDefaultAsync();
                    if (prod == null) return;

                    txtProductName.Text = prod.Name;
                    txtPurchasePrice.Text = prod.PurchasePrice.ToString();
                    txtSalePrice.Text = prod.SalePrice.ToString();
                    txtStock.Text = prod.Stock.ToString();

                    if (!string.IsNullOrEmpty(prod.CategoryId) &&
                        ObjectId.TryParse(prod.CategoryId, out ObjectId catObjId))
                    {
                        try { cbCategory.SelectedValue = catObjId; } catch { }
                    }

                    if (!string.IsNullOrEmpty(prod.ImageBase64))
                    {
                        picProduct.Image = Base64ToImage(prod.ImageBase64);
                        selectedImageBase64 = prod.ImageBase64;
                    }
                    else
                    {
                        picProduct.Image = null;
                        selectedImageBase64 = null;
                    }

                    editingId = prod.Id;
                    btnAddProduct.Text = "Update Product";
                }
                else if (column.Name == "Delete")
                {
                    var confirmResult = MessageBox.Show("Are you sure to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        await _productCollection.DeleteOneAsync(p => p.Id == gridItem.Id);
                        MessageBox.Show("Product deleted successfully!");
                        LoadProductList();

                        // Notify listeners
                        ProductsChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowTemplate.Height = 60;
            dgv.AllowUserToAddRows = false;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.BorderStyle = BorderStyle.None;

            // ---- Header Style ----
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 35);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(6);
            dgv.ColumnHeadersHeight = 45;

            // ---- Row Style ----
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(230, 230, 230);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);

            // ---- Make grid smooth ----
            // DataGridView.DoubleBuffered is a protected property. Set it via reflection.
            try
            {
                typeof(DataGridView).InvokeMember(
                    "DoubleBuffered",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty,
                    null,
                    dgv,
                    new object[] { true });
            }
            catch
            {
                // if reflection fails for any reason, silently continue — styling is non-critical
            }

        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Images|*.jpg;*.jpeg;*.png";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        Image img = Image.FromStream(fs);
                        picProduct.Image = new Bitmap(img); // detach stream
                    }

                    // convert selected file to base64 using Image stored in picture box to ensure consistent format
                    if (picProduct.Image != null)
                        selectedImageBase64 = ImageToBase64(picProduct.Image);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load image: " + ex.Message, "Error");
                }
            }
        }

        private string ImageToBase64(Image img)
        {
            if (img == null) return null;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private Image Base64ToImage(string base64)
        {
            if (string.IsNullOrEmpty(base64)) return null;
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                using (var ms = new MemoryStream(bytes))
                {
                    using (var img = Image.FromStream(ms))
                    {
                        // Return a copy so the returned Image is not tied to the disposed stream
                        return new Bitmap(img);
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private void ClearInputs()
        {
            txtProductName.Clear();
            txtPurchasePrice.Clear();
            txtSalePrice.Clear();
            txtStock.Clear();
            picProduct.Image = null;
            selectedImageBase64 = null;
            btnAddProduct.Text = "Add Product";
            editingId = ObjectId.Empty;
            // reset category selection if available
            if (cbCategory.Items.Count > 0)
                cbCategory.SelectedIndex = -1;
        }

        private void UC_Product_Load(object sender, EventArgs e)
        {

        }

        private void lblProduct_Click(object sender, EventArgs e)
        {

        }
    }
}
