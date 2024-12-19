using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Dunkin
{
    public partial class ProductManagement : Form
    {
        //String cnn = @"Data Source=dbdunkindev.clsow6k2ei1p.ap-southeast-1.rds.amazonaws.com;DATABASE=dbDunkin;User ID=admin;Password=Dunkinpass";
        String cnn = @"Data Source=LAPTOP-IFV6NHU7;DATABASE=dbDunkin;User ID=sa;Password=p@ssw0rd";
        SqlDataReader dr;
        public ProductManagement()
        {
            InitializeComponent();
            // Subscribe to the CellValueChanged event
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
        }

        private void ProductPrice_Load(object sender, EventArgs e)
        {
            loadData();
            EnableDoubleBuffering();
        }

        private void EnableDoubleBuffering()
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                                              System.Reflection.BindingFlags.NonPublic |
                                              System.Reflection.BindingFlags.Instance |
                                              System.Reflection.BindingFlags.SetProperty,
                                              null, dataGridView1, new object[] { true });
        }
        private void PasteDataIntoSelectedCells()
        {
            // Get the data from the clipboard
            string clipboardData = Clipboard.GetText();

            // Split clipboard data by new lines (rows) and tabs (columns)
            string[] rows = clipboardData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (DataGridViewCell selectedCell in dataGridView1.SelectedCells)
            {
                int rowIndex = selectedCell.RowIndex;
                int colIndex = selectedCell.ColumnIndex;

                if (rowIndex < rows.Length)
                {
                    string[] cells = rows[rowIndex].Split('\t');
                    if (colIndex < cells.Length)
                    {
                        // Here we assign the cell value from clipboard
                        dataGridView1.Rows[rowIndex].Cells[colIndex].Value = cells[colIndex];
                    }
                }
            }
        }
        public void loadData()
        {
            try
            {
                SqlConnection con = new SqlConnection(cnn);
                con.Open();
                dr = new SqlCommand("SELECT * FROM tblStock ORDER BY CAST(PRODUCT_CODE AS INT) ASC", con).ExecuteReader();

                DataTable dt = new DataTable();

                // Fill DataTable with data from the database
                dt.Load(dr);

                // Bind the DataTable to the DataGridView
                dataGridView1.DataSource = dt;



                // Mark all loaded rows as "existing"
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Tag = "existing";  // Mark rows loaded from DB as existing
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Ask for password when the checkbox is checked
            string password = PromptForPassword();

            if (password == "password") // Replace "your_password" with the actual password
            {
                // Loop through all rows in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Set specific columns to be editable
                    row.Cells["GROSS_PRICE"].ReadOnly = false;
                    row.Cells["CRITICAL_QUANTITY"].ReadOnly = false;
                    row.Cells["CATEGORY"].ReadOnly = false;
                    row.Cells["UNIT_OF_MEASUREMENT"].ReadOnly = false;

                    // Optionally, you can check if these columns exist to avoid runtime errors
                    // (in case the DataGridView has columns with different names)
                }
            }
            else
            {
                MessageBox.Show("Incorrect password. The columns remains locked.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private string PromptForPassword()
        {
            // Create a new form for the password prompt
            Form passwordForm = new Form();
            passwordForm.Text = "Enter Password";
            passwordForm.StartPosition = FormStartPosition.CenterParent;

            // Create the password text box
            TextBox passwordTextBox = new TextBox();
            passwordTextBox.UseSystemPasswordChar = true; // Hide password characters
            passwordTextBox.Location = new Point(10, 10);
            passwordTextBox.Width = 230;

            // Create the OK button
            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(10, 40);

            // Add controls to the form
            passwordForm.Controls.Add(passwordTextBox);
            passwordForm.Controls.Add(okButton);

            // Set the form's properties and display it
            passwordForm.AcceptButton = okButton;
            passwordForm.ClientSize = new Size(250, 80);

            // Show the form as a dialog and return the entered password if OK is pressed
            if (passwordForm.ShowDialog() == DialogResult.OK)
            {
                return passwordTextBox.Text;
            }

            return string.Empty;
        }

        private void txtProdName_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtProdName.Text.Trim().ToLower(); // Get the input text and trim it
            FilterDataGrid(filterText);
        }

        private void FilterDataGrid(string filterText)
        {
            try
            {

                // Check if there's text to filter
                if (string.IsNullOrEmpty(filterText))
                {
                    // If no filter text, show all rows
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;

                }
                else
                {
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"PRODUCT_NAME LIKE '%{filterText}%'";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering data: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open();
                    Cursor = Cursors.WaitCursor;
                    // Insert new rows
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string productCode = row.Cells["PRODUCT_CODE"].Value.ToString().ToUpper();
                        string productName = row.Cells["PRODUCT_NAME"].Value.ToString().ToUpper();
                        string grossPrice = row.Cells["GROSS_PRICE"].Value.ToString() ?? "0.00";
                        string netPrice = row.Cells["NET_PRICE"].Value.ToString() ?? "0.00";
                        string criticalQuantity = row.Cells["CRITICAL_QUANTITY"].Value.ToString() ?? "0";
                        string category = row.Cells["CATEGORY"].Value.ToString().ToUpper();
                        string measurement = row.Cells["UNIT_OF_MEASUREMENT"].Value.ToString();

                        if (netPrice == "" || netPrice == "0" || netPrice == null)
                        {
                            netPrice = "0.00";
                        }
                        if (grossPrice == "" || grossPrice == "0" || grossPrice == null)
                        {
                            grossPrice = "0.00";
                        }
                        if (criticalQuantity == "" || criticalQuantity == null)
                        {
                            criticalQuantity = "0";
                        }
                        string queryUpdate = "UPDATE tblStock set GROSS_PRICE = @grossPrice, NET_PRICE = @netPrice, CRITICAL_QUANTITY = @criticalQuantity," +
                            "CATEGORY = @category, UNIT_OF_MEASUREMENT = @measurement where PRODUCT_CODE = @productCode AND PRODUCT_NAME = @productName";
                        using (SqlCommand cmd = new SqlCommand(queryUpdate, con))
                        {
                            cmd.Parameters.AddWithValue("@productCode", productCode);
                            cmd.Parameters.AddWithValue("@productName", productName);
                            cmd.Parameters.AddWithValue("@grossPrice", grossPrice);
                            cmd.Parameters.AddWithValue("@netPrice", netPrice);
                            cmd.Parameters.AddWithValue("@criticalQuantity", criticalQuantity);
                            cmd.Parameters.AddWithValue("@category", category);
                            cmd.Parameters.AddWithValue("@measurement", measurement);

                            cmd.ExecuteNonQuery();
                        }
                        string queryUpdateInventory = "UPDATE tblInventory set CATEGORY = @category where PRODUCT_CODE = @productCode AND PRODUCT_NAME = @productName";
                        using (SqlCommand cmd = new SqlCommand(queryUpdateInventory, con))
                        {
                            cmd.Parameters.AddWithValue("@productCode", productCode);
                            cmd.Parameters.AddWithValue("@productName", productName);
                            cmd.Parameters.AddWithValue("@category", category);

                            cmd.ExecuteNonQuery();
                        }
                        string queryUpdateNetPrice = "UPDATE tblInventory set NET_PRICE = @netPrice WHERE PRODUCT_CODE = @productCode AND PRODUCT_NAME = @productName";
                        using (SqlCommand cmd = new SqlCommand(queryUpdateNetPrice, con))
                        {
                            cmd.Parameters.AddWithValue("@productCode", productCode);
                            cmd.Parameters.AddWithValue("@productName", productName);
                            cmd.Parameters.AddWithValue("@netPrice", netPrice);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Data saved/updated successfully.");
                    Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving/updating data: {ex.Message}");
            }
        }

        private void dataGridView1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)  // Check if Ctrl+V is pressed
            {
                PasteDataIntoSelectedCells();
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the event is triggered for the GROSS_PRICE column
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "GROSS_PRICE")
            {
                var row = dataGridView1.Rows[e.RowIndex];
                if (decimal.TryParse(row.Cells["GROSS_PRICE"].Value?.ToString(), out decimal grossPrice))
                {
                    // Calculate NET_PRICE, e.g., deducting a fixed percentage (e.g., 10%)
                    decimal netPrice = Math.Round(grossPrice / 1.12m, 2);

                    // Update the NET_PRICE cell
                    row.Cells["NET_PRICE"].Value = netPrice;
                }
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.OwningColumn.Name == "GROSS_PRICE" && e.Control is TextBox textBox)
            {
                // Subscribe to the KeyPress event to allow numeric input only (optional)
                textBox.KeyPress -= TextBox_KeyPress; // Ensure no duplicate handlers
                textBox.KeyPress += TextBox_KeyPress;
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers, one decimal point, and control keys
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox)?.Text.Contains('.') == true)
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = dataGridView1.SelectedRows.Count > 0;
        }
    }
}
