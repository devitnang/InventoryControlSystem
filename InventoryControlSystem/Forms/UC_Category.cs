using InventoryControlSystem.Models;
using MongoDB.Bson;
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
    public partial class UC_Category : UserControl
    {
        private IMongoCollection<Category> _categoryCollection;

        // Event raised when categories change so other controls can refresh immediately
        public static event EventHandler CategoriesChanged;

        // detect update mode
        private ObjectId editingId = ObjectId.Empty;

        public UC_Category()
        {
            InitializeComponent();

            _categoryCollection = MongoDBHelper.GetCollection<Category>("categories");
            LoadCategoryList();
        }

        private async void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a category name!", "Warning");
                return;
            }

            try
            {
                if (editingId != ObjectId.Empty)
                {
                    // Update existing category
                    var update = Builders<Category>.Update
                        .Set(c => c.Name, name);

                    await _categoryCollection.UpdateOneAsync(c => c.Id == editingId, update);

                    MessageBox.Show("Category updated successfully!");
                    editingId = ObjectId.Empty;
                    btnAddCategory.Text = "Add Category";
                }
                else
                {
                    var newCategory = new Category
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = name,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _categoryCollection.InsertOneAsync(newCategory);

                    MessageBox.Show("Category added successfully!");
                }

                txtCategoryName.Clear();
                LoadCategoryList();

                // Notify listeners (e.g. UC_Product) that categories changed
                CategoriesChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving category: " + ex.Message, "Error");
            }
        }

        private async void LoadCategoryList()
        {
            try
            {
                var list = await _categoryCollection.Find(_ => true).ToListAsync();

                dgvCategory.DataSource = null; // reset
                dgvCategory.AutoGenerateColumns = false;
                dgvCategory.Columns.Clear();

                // ----- DESIGN FIRST -----
                DesignCategoryGrid();

                // ----- ADD NORMAL TEXT COLUMNS -----
                dgvCategory.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Name",
                    HeaderText = "Category Name",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 200
                });

                dgvCategory.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "CreatedAt",
                    HeaderText = "Created Date",
                    Width = 150,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" }
                });

                // ----- ADD ICON BUTTONS -----
                SetupGridButtons();

                // Bind data
                dgvCategory.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load categories: " + ex.Message, "Error");
            }
        }

        private void DesignCategoryGrid()
        {
            dgvCategory.BorderStyle = BorderStyle.None;
            dgvCategory.BackgroundColor = Color.White;
            dgvCategory.EnableHeadersVisualStyles = false;
            dgvCategory.RowHeadersVisible = false;

            // ----- HEADER STYLE -----
            dgvCategory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvCategory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCategory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvCategory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvCategory.ColumnHeadersHeight = 40;

            // ----- ROW STYLE -----
            dgvCategory.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvCategory.DefaultCellStyle.BackColor = Color.White;
            dgvCategory.DefaultCellStyle.ForeColor = Color.Black;
            dgvCategory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dgvCategory.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvCategory.RowTemplate.Height = 40;

            dgvCategory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            dgvCategory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }


        private void SetupGridButtons()
        {
            try
            {
                // Remove old columns if exist
                if (dgvCategory.Columns.Contains("Edit")) dgvCategory.Columns.Remove("Edit");
                if (dgvCategory.Columns.Contains("Delete")) dgvCategory.Columns.Remove("Delete");

                // ----- EDIT BUTTON -----
                var editCol = new DataGridViewImageColumn
                {
                    Name = "Edit",
                    HeaderText = "",
                    Width = 40,
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };

                try { editCol.Image = Properties.Resources.edit_icon; }
                catch { editCol.Image = null; }

                // ----- DELETE BUTTON -----
                var deleteCol = new DataGridViewImageColumn
                {
                    Name = "Delete",
                    HeaderText = "",
                    Width = 40,
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };

                try { deleteCol.Image = Properties.Resources.remove_icon; }
                catch { deleteCol.Image = null; }

                // Add them LAST → so they appear on the RIGHT
                dgvCategory.Columns.Add(editCol);
                dgvCategory.Columns.Add(deleteCol);

                // Attach event only once
                dgvCategory.CellClick -= dgvCategory_CellClickAsync;
                dgvCategory.CellClick += dgvCategory_CellClickAsync;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to setup grid buttons: " + ex.Message, "Error");
            }
        }

        private async void dgvCategory_CellClickAsync(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                var row = dgvCategory.Rows[e.RowIndex];
                var category = row.DataBoundItem as Category;
                if (category == null) return;

                var column = dgvCategory.Columns[e.ColumnIndex];
                if (column == null) return;

                if (column.Name == "Edit")
                {
                    // Populate inputs for editing
                    txtCategoryName.Text = category.Name;
                    editingId = category.Id;
                    btnAddCategory.Text = "Update Category";
                }
                else if (column.Name == "Delete")
                {
                    var confirm = MessageBox.Show("Are you sure you want to delete this category?", "Confirm Delete", MessageBoxButtons.YesNo);
                    if (confirm == DialogResult.Yes)
                    {
                        await _categoryCollection.DeleteOneAsync(c => c.Id == category.Id);
                        MessageBox.Show("Category deleted successfully!");

                        // refresh list and notify listeners
                        LoadCategoryList();
                        CategoriesChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error handling grid action: " + ex.Message, "Error");
            }
        }
    }
}
