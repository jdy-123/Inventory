using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dunkin
{
    public partial class StockIssuance : Form
    {
        //String cnn = @"Data Source=dbdunkindev.clsow6k2ei1p.ap-southeast-1.rds.amazonaws.com;DATABASE=dbDunkin;User ID=admin;Password=Dunkinpass";
        String cnn = @"Data Source=LAPTOP-IFV6NHU7;DATABASE=dbDunkin;User ID=sa;Password=p@ssw0rd";
        SqlDataReader dr;
        private bool isDragging = false;
        private Point lastCursor;
        private bool isDataChanged = false;
        private string name;
        bool isReports = false;
        public StockIssuance(Boolean isReport, String name)
        {
            InitializeComponent();
            isReports = isReport;
            this.name = name;
            checkIfReport();
            // Enable KeyPreview to capture key presses before they reach the controls
            this.KeyPreview = true;

            // Subscribe to the KeyDown event
            this.KeyDown += Reports_KeyDown;

        }

        private void checkIfReport()
        {
            if (isReports)
            {
                btnAdd.Hide();
                btnEdit.Hide();
                btnSave.Hide();
                dataGridView2.ReadOnly = true;
                this.Text = "Issuance Report";
            }
            else
            {
                btnPrint.Hide();
            }
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblMinimize_Click(object sender, EventArgs e)
        {
            // Minimize the form
            this.WindowState = FormWindowState.Minimized;
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            // If the left mouse button is pressed, set dragging to true and capture the cursor position
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastCursor = e.Location;
            }
        }


        private void PasteDataIntoSelectedCells()
        {
            // Get the data from the clipboard
            string clipboardData = Clipboard.GetText();

            // Split clipboard data by new lines (rows) and tabs (columns)
            string[] rows = clipboardData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (DataGridViewCell selectedCell in dataGridView2.SelectedCells)
            {
                int rowIndex = selectedCell.RowIndex;
                int colIndex = selectedCell.ColumnIndex;

                if (rowIndex < rows.Length)
                {
                    string[] cells = rows[rowIndex].Split('\t');
                    if (colIndex < cells.Length)
                    {
                        // Here we assign the cell value from clipboard
                        dataGridView2.Rows[rowIndex].Cells[colIndex].Value = cells[colIndex];
                    }
                }
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // Calculate the distance the mouse has moved
                Point offset = new Point(e.X - lastCursor.X, e.Y - lastCursor.Y);

                // Move the form by the distance the mouse has moved
                this.Location = new Point(this.Location.X + offset.X, this.Location.Y + offset.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            // Release the dragging state when the mouse button is released
            isDragging = false;
        }
        private void EnableDoubleBuffering()
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                                              System.Reflection.BindingFlags.NonPublic |
                                              System.Reflection.BindingFlags.Instance |
                                              System.Reflection.BindingFlags.SetProperty,
                                              null, dataGridView2, new object[] { true });
        }
        private void Reports_Load(object sender, EventArgs e)
        {
            EnableDoubleBuffering();

            setSizeGrid();
            // Optionally, adjust the location to position it with a margin
            cmbShop.Text = "ALL";
            SetupProductNameComboBox();
            cmbShop.Sorted = true;
            loadShop();
            dtDateFrom.Value = DateTime.Now;
            dtDateTo.Value = DateTime.Now;
            btnSave.Enabled = false;
            // TODO: This line of code loads data into the 'dbDunkinDataSet1.tblReport' table. You can move, or remove it, as needed.
            loadData();
            MakeColumnsReadOnly();
            dataGridView2.CellValueChanged += dataGridView2_CellValueChanged;
            this.cmbShop.SelectedIndexChanged += new System.EventHandler(this.cmbShop_SelectedIndexChanged);

        }
        private void SetupProductNameComboBox()
        {
            // Step 1: Check if PRODUCT_NAME column is already in the DataGridView
            if (dataGridView2.Columns.Contains("PRODUCT_NAME"))
            {
                dataGridView2.Columns.Remove("PRODUCT_NAME"); // Remove if it already exists
            }

            // Step 2: Create and add ComboBox column for PRODUCT_NAME
            DataGridViewComboBoxColumn productNameColumn = new DataGridViewComboBoxColumn
            {
                Name = "PRODUCT_NAME",
                HeaderText = "PRODUCT_NAME",
                DataPropertyName = "PRODUCT_NAME"
            };

            // Step 3: Populate ComboBox with valid product names from tblStock
            List<string> productNames = GetProductNames();
            productNameColumn.Items.AddRange(productNames.ToArray());  // Populate with valid names

            // Step 4: Insert ComboBox column at the desired index (index 2 is the third column)
            if (dataGridView2.Columns.Count >= 1) // Ensure there are enough columns
            {
                dataGridView2.Columns.Insert(1, productNameColumn); // Insert in the 3rd position (0-based index)
            }
            else
            {
                dataGridView2.Columns.Add(productNameColumn); // Add at the end if fewer columns exist
            }
        }

        private List<string> GetProductNames()
        {
            List<string> productNames = new List<string>();
            using (var connection = new SqlConnection(cnn))
            {
                connection.Open();
                string query = "SELECT DISTINCT(PRODUCT_NAME) FROM tblStock UNION SELECT DISTINCT(PRODUCT_NAME) FROM tblReport ORDER BY PRODUCT_NAME;";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productNames.Add(reader.GetString(0).Trim());  // Get valid product names and remove extra spaces
                    }
                }
            }
            return productNames;
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Check if we are editing the PRODUCT_NAME column
            if (dataGridView2.CurrentCell.ColumnIndex == dataGridView2.Columns["PRODUCT_NAME"].Index)
            {
                // Get the ComboBox control for the current cell
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    // Populate ComboBox with valid product names
                    List<string> productNames = GetProductNames();
                    comboBox.Items.Clear();
                    comboBox.Items.Add("");
                    comboBox.Items.AddRange(productNames.ToArray());

                    // Get the current value of the PRODUCT_NAME cell
                    string currentValue = dataGridView2.CurrentCell.Value?.ToString().Trim();

                    // If the value exists in the list, select it
                    if (!string.IsNullOrEmpty(currentValue) && productNames.Contains(currentValue))
                    {
                        comboBox.SelectedItem = currentValue; // Automatically select the item
                    }
                    else
                    {
                        comboBox.SelectedIndex = -1; // No selection
                    }
                }
            }
        }
        private void dataGridView2_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView2.IsCurrentCellDirty)
            {
                // Commit the edit immediately
                dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void loadShop()
        {
            using (SqlConnection con = new SqlConnection(cnn))
            {
                con.Open(); // Ensure the connection is open
                dr = new SqlCommand("SELECT * FROM tblShop", con).ExecuteReader();
                while (dr.Read())
                {
                    cmbShop.Items.Add(dr.GetString(1));
                }
                con.Close();
                cmbShop.SelectedIndex = 0;
            }
        }

        private void setSizeGrid()
        {
            this.WindowState = FormWindowState.Maximized; // Maximized
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);  // Position it at the top-left corner
            this.Size = Screen.PrimaryScreen.Bounds.Size; // Full screen size
                                                          // Get the screen size
            Rectangle screenSize = Screen.PrimaryScreen.Bounds;

            // Set the DataGridView size with some margin (e.g., 50 pixels)

            dataGridView2.Size = new Size(screenSize.Width - 250, screenSize.Height - 120);
        }
        // Method to enable or disable the Validate button
        private void ToggleValidateButton()
        {
            // Enable the button if there are changes, otherwise disable it
            btnSave.Enabled = isDataChanged;
        }
        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Set the flag to true when data changes
            isDataChanged = true;
            ToggleValidateButton();
            // Ensure the row index is valid (ignore the header row)
            if (e.RowIndex >= 0)
            {
                // Get the row that was changed
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                // Get column indices for QUANTITY, GROSS_AMOUNT, TOTAL_COST_IN_GROSS, PRODUCT_CODE, DATE, and SHOP
                int colQuantityIndex = dataGridView2.Columns["QUANTITY"].Index;
                int colGrossAmountIndex = dataGridView2.Columns["GROSS_AMOUNT"].Index;
                int colTotalCostIndex = dataGridView2.Columns["TOTAL_COST_IN_GROSS"].Index;
                int colProductCodeIndex = dataGridView2.Columns["PRODUCT_CODE"].Index;
                int colProductNameIndex = dataGridView2.Columns["PRODUCT_NAME"].Index;
                int colDateIndex = dataGridView2.Columns["DATE"].Index;
                int colShopIndex = dataGridView2.Columns["SHOP"].Index;
                int colIdIndex = dataGridView2.Columns["ID"].Index;
                int colAdjustmentIndex = dataGridView2.Columns["ADJUSTMENT"].Index;


                // Check if the column changed is either QUANTITY or GROSS_AMOUNT
                if (e.ColumnIndex == colQuantityIndex || e.ColumnIndex == colAdjustmentIndex)
                {
                    // Get values for QUANTITY and GROSS_AMOUNT
                    var quantityValue = row.Cells[colQuantityIndex].Value;
                    var grossAmountValue = row.Cells[colGrossAmountIndex].Value;
                    var adjusmentValue = row.Cells[colAdjustmentIndex].Value;

                    // Check if QUANTITY is empty or null
                    if (quantityValue == null || string.IsNullOrEmpty(quantityValue.ToString()))
                    {
                        row.Cells[colQuantityIndex].Value = 0;  // Set to 0 if empty
                    }
                    // Check if QUANTITY is empty or null
                    if (grossAmountValue == null || string.IsNullOrEmpty(grossAmountValue.ToString()))
                    {
                        row.Cells[colGrossAmountIndex].Value = 0;  // Set to 0 if empty
                    }
                    // Check if QUANTITY is empty or null
                    if (adjusmentValue == null || string.IsNullOrEmpty(adjusmentValue.ToString()))
                    {
                        adjusmentValue = 0;  // Set to 0 if empty
                    }

                    // Check if the values are valid numbers and not null
                    if (quantityValue != null && grossAmountValue != null &&
                        double.TryParse(quantityValue.ToString(), out double quantity) &&
                        double.TryParse(grossAmountValue.ToString(), out double grossAmount))
                    {
                        // Calculate TOTAL_COST_IN_GROSS
                        double totalCostInGross = (quantity + Convert.ToDouble(adjusmentValue)) * grossAmount;

                        // Set the value of TOTAL_COST_IN_GROSS
                        row.Cells[colTotalCostIndex].Value = totalCostInGross;
                    }
                    else
                    {
                        // If invalid, set TOTAL_COST_IN_GROSS to null or 0
                        row.Cells[colTotalCostIndex].Value = DBNull.Value;
                    }
                }

                // Check if the changed column is PRODUCT_CODE (for fetching PRODUCT_NAME and GROSS_PRICE)
                if (e.ColumnIndex == colProductCodeIndex)
                {
                    // Check if the PRODUCT_CODE is not null or empty
                    if (row.Cells[colProductCodeIndex].Value != null && !string.IsNullOrEmpty(row.Cells[colProductCodeIndex].Value.ToString()))
                    {
                        string productCode = row.Cells[colProductCodeIndex].Value.ToString();
                        var adjusmentValue = row.Cells[colAdjustmentIndex].Value;
                        // Fetch the PRODUCT_NAME and GROSS_PRICE from the database based on PRODUCT_CODE
                        (string productName, double grossPrice, string unit) = GetProductDetailsFromDatabase(productCode);

                        // Update the PRODUCT_NAME and GROSS_AMOUNT cells in the DataGridView
                        row.Cells[colProductNameIndex].Value = productName;  // Update PRODUCT_NAME
                        row.Cells[colGrossAmountIndex].Value = grossPrice;   // Update GROSS_AMOUNT (if needed)
                        // Check if QUANTITY is empty or null
                        // Check if QUANTITY is empty or null
                        if (adjusmentValue == null || string.IsNullOrEmpty(adjusmentValue.ToString()))
                        {
                            adjusmentValue = 0;  // Set to 0 if empty
                        }
                        // Also update TOTAL_COST_IN_GROSS based on new GROSS_AMOUNT if QUANTITY is present
                        if (row.Cells[colQuantityIndex].Value != null && double.TryParse(row.Cells[colQuantityIndex].Value.ToString(), out double quantity))
                        {
                            double totalCostInGross = (quantity + Convert.ToDouble(adjusmentValue)) * grossPrice;
                            row.Cells[colTotalCostIndex].Value = totalCostInGross;  // Update TOTAL_COST_IN_GROSS
                        }
                    }
                    else
                    {

                        row.Cells[colGrossAmountIndex].Value = "0";
                        row.Cells[colTotalCostIndex].Value = "0";
                    }
                }

                if (e.ColumnIndex == colProductNameIndex)
                {
                    // Check if the PRODUCT_CODE is not null or empty
                    if (row.Cells[colProductNameIndex].Value != null && !string.IsNullOrEmpty(row.Cells[colProductNameIndex].Value.ToString()))
                    {
                        string productName = row.Cells[colProductNameIndex].Value.ToString();
                        var adjusmentValue = row.Cells[colAdjustmentIndex].Value;

                        // Fetch the PRODUCT_NAME and GROSS_PRICE from the database based on PRODUCT_CODE
                        (string productCode, double grossPrice) = GetProductNameDetailsFromDatabase(productName);
                        // Check if QUANTITY is empty or null
                        if (adjusmentValue == null || string.IsNullOrEmpty(adjusmentValue.ToString()))
                        {
                            adjusmentValue = 0;  // Set to 0 if empty
                        }
                        // Update the PRODUCT_NAME and GROSS_AMOUNT cells in the DataGridView
                        row.Cells[colProductCodeIndex].Value = productCode;  // Update PRODUCT_NAME
                        row.Cells[colGrossAmountIndex].Value = grossPrice;   // Update GROSS_AMOUNT (if needed)

                        // Also update TOTAL_COST_IN_GROSS based on new GROSS_AMOUNT if QUANTITY is present
                        if (row.Cells[colQuantityIndex].Value != null && double.TryParse(row.Cells[colQuantityIndex].Value.ToString(), out double quantity))
                        {
                            double totalCostInGross = (quantity + Convert.ToDouble(adjusmentValue)) * grossPrice;
                            row.Cells[colTotalCostIndex].Value = totalCostInGross;  // Update TOTAL_COST_IN_GROSS
                        }
                    }
                    else
                    {
                        // If PRODUCT_CODE is empty, clear PRODUCT_NAME and GROSS_AMOUNT
                        row.Cells[colProductCodeIndex].Value = "";
                        row.Cells[colGrossAmountIndex].Value = "0";
                        row.Cells[colTotalCostIndex].Value = "0";
                    }
                }

                // ** NEW CODE: Check for duplicate PRODUCT_CODE, DATE, and SHOP **
                string productCodeChanged = row.Cells[colProductCodeIndex].Value?.ToString();
                string dateChanged = row.Cells[colDateIndex].Value?.ToString();
                string shopChanged = row.Cells[colShopIndex].Value?.ToString();

                foreach (DataGridViewRow existingRow in dataGridView2.Rows)
                {
                    // Skip the current row itself during the check (do not compare to itself)
                    if (existingRow.Index != row.Index)
                    {
                        // Check if the PRODUCT_CODE, DATE, and SHOP match
                        string existingProductCode = existingRow.Cells[colProductCodeIndex].Value?.ToString();
                        string existingDate = existingRow.Cells[colDateIndex].Value?.ToString();
                        string existingShop = existingRow.Cells[colShopIndex].Value?.ToString();


                    }
                }

                // Check if the changed column is QUANTITY
                if (e.ColumnIndex == colQuantityIndex)
                {
                    var quantityValue = row.Cells[colQuantityIndex].Value;
                    string productCode = row.Cells[colProductCodeIndex].Value?.ToString();
                    string id = row.Cells[colIdIndex].Value?.ToString();

                    // Proceed only if the PRODUCT_CODE and QUANTITY are valid
                    if (!string.IsNullOrEmpty(productCode) && quantityValue != null)
                    {
                        if (double.TryParse(quantityValue.ToString(), out double quantity))
                        {
                            // Fetch ENDING_INVENTORY for the given product code
                            double endingInventory = GetendingInventoryFromDatabase(productCode);
                            decimal oldQuantity;
                            decimal adjustment;
                            using (SqlConnection con = new SqlConnection(cnn))
                            {
                                con.Open();
                                var date = row.Cells[colDateIndex].Value;
                                oldQuantity = GetOldQuantityFromDatabase(con, id);
                                adjustment = GetAdjustmentFromDatabase(con, productCode);
                            }

                            // Check if QUANTITY - ENDING_INVENTORY is negative
                            if (endingInventory + Convert.ToDouble(oldQuantity) + Convert.ToDouble(adjustment) < quantity)
                            {
                                var date = row.Cells[colDateIndex].Value;
                                // Show error message if the condition is met
                                MessageBox.Show($"Error: Insufficient stock for Product code '{productCode}'.", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                row.Cells[colQuantityIndex].Style.BackColor = Color.Red;
                                row.Cells[colQuantityIndex].ToolTipText = "Insufficient stock";
                                // Reset the quantity value to prevent invalid input
                                return;
                            }
                            else
                            {
                                Color originalColor = row.Cells[colGrossAmountIndex].Style.BackColor;
                                row.Cells[colQuantityIndex].Style.BackColor = originalColor;
                                row.Cells[colQuantityIndex].ToolTipText = "";
                            }

                        }
                    }
                }
            }
        }
        private void cmbShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected shop
            string selectedShop = cmbShop.Text;

            // Get the selected date range (from and to)
            string selectedDateFrom = dtDateFrom.Value.ToString("yyyy-MM-dd");
            string selectedDateTo = dtDateTo.Value.ToString("yyyy-MM-dd");
            // Apply the filter to the DataTable based on the selected shop and date range
            FilterDataByShopAndDate(selectedShop, selectedDateFrom, selectedDateTo);
        }

        private void FilterDataByShopAndDate(string selectedShop, string selectedDateFrom, string selectedDateTo)
        {
            loadData();
            // Assuming that the DataTable used as the DataSource is named dt
            DataTable dt = (DataTable)dataGridView2.DataSource;

            // Apply a filter based on the selected shop and date range
            string filter = "";

            if (dt != null)
            {
                // Check if a "From" and "To" date are provided
                if (selectedShop == "ALL")
                {
                    // No filter for shop, but apply date range filter
                    filter = $"DATE >= '{selectedDateFrom}' AND DATE <= '{selectedDateTo}'";  // Filter rows by date range
                }
                else
                {
                    // Filter rows by selected shop and date range
                    filter = $"SHOP = '{selectedShop}' AND DATE >= '{selectedDateFrom}' AND DATE <= '{selectedDateTo}'";  // Apply both shop and date range filter
                }

                // Apply the combined filter
                dt.DefaultView.RowFilter = filter;
            }
            // Mark all loaded rows as "existing"
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Tag != "new")
                {
                    row.Tag = "existing";  // Mark rows loaded from DB as existing
                }
            }
        }

        private double GetendingInventoryFromDatabase(string productCode)
        {
            double endingInventory = 0;

            string query = "SELECT ENDING_INVENTORY FROM tblInventory WHERE PRODUCT_CODE = @productCode AND DATE = @date";

            try
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open(); // Ensure the connection is open

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@productCode", productCode);
                        // Set date parameter to the same format "MMM-yyyy" (e.g., "Nov-2024")
                        cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MMM-yyyy"));

                        // Execute the query and fetch the result
                        var result = cmd.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            endingInventory = Convert.ToDouble(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching ending inventory: " + ex.Message);
            }

            return endingInventory;
        }
        private (string ProductName, double GrossPrice) GetProductNameDetailsFromDatabase(string productName)
        {
            string productCode = string.Empty;
            double grossPrice = 0;
            string query = "SELECT PRODUCT_CODE, GROSS_PRICE FROM tblStock WHERE PRODUCT_NAME = @productName";

            try
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open(); // Ensure the connection is open

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@productName", productName);

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            productCode = reader["PRODUCT_CODE"].ToString();
                            grossPrice = reader["GROSS_PRICE"] == "" ? 0 : Convert.ToDouble(reader["GROSS_PRICE"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching product details: " + ex.Message);
            }

            return (productCode, grossPrice);
        }
        private (string ProductName, double GrossPrice, string unit) GetProductDetailsFromDatabase(string productCode)
        {
            string productName = string.Empty;
            string unit = string.Empty;
            double grossPrice = 0;
            string query = "SELECT PRODUCT_NAME, GROSS_PRICE, UNIT_OF_MEASUREMENT FROM tblStock WHERE PRODUCT_CODE = @productCode";

            try
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open(); // Ensure the connection is open

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@productCode", productCode);

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            productName = reader["PRODUCT_NAME"].ToString();
                            grossPrice = reader["GROSS_PRICE"] == "" ? 0 : Convert.ToDouble(reader["GROSS_PRICE"]);
                            unit = reader["UNIT_OF_MEASUREMENT"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching product details: " + ex.Message);
            }

            return (productName, grossPrice, unit);
        }
        private void MakeColumnsReadOnly()
        {
            // Check if the columns exist
            if (dataGridView2.Columns.Contains("GROSS_AMOUNT"))
            {
                dataGridView2.Columns["GROSS_AMOUNT"].ReadOnly = true;
            }

            if (dataGridView2.Columns.Contains("TOTAL_COST_IN_GROSS"))
            {
                dataGridView2.Columns["TOTAL_COST_IN_GROSS"].ReadOnly = true;
            }

            if (dataGridView2.Columns.Contains("ID"))
            {
                dataGridView2.Columns["ID"].Visible = false;
            }
        }


        public void loadData()
        {
            try
            {
                SqlConnection con = new SqlConnection(cnn);
                con.Open();
                if ("ALL".Equals(cmbShop.Text))
                {
                    dr = new SqlCommand("SELECT a.PRODUCT_CODE, a.PRODUCT_NAME, a.DATE, a.QUANTITY, b.UNIT_OF_MEASUREMENT, a.ADJUSTMENT, a.GROSS_AMOUNT, a.TOTAL_COST_IN_GROSS, a.SHOP, a.ID FROM tblReport a " +
                        " LEFT JOIN tblStock b on (a.PRODUCT_CODE = b.PRODUCT_CODE)" +
                        " WHERE CAST(a.DATE AS DATE) BETWEEN '" + dtDateFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateTo.Value.ToString("yyyy-MM-dd") + "'  ORDER BY a.ID", con).ExecuteReader();
                }
                else
                {
                    dr = new SqlCommand("SELECT a.PRODUCT_CODE, a.PRODUCT_NAME, a.DATE, a.QUANTITY, b.UNIT_OF_MEASUREMENT, a.ADJUSTMENT, a.GROSS_AMOUNT, a.TOTAL_COST_IN_GROSS, a.SHOP, a.ID FROM tblReport a " +
                        " LEFT JOIN tblStock b on (a.PRODUCT_CODE = b.PRODUCT_CODE) " +
                        "WHERE a.SHOP = '" + cmbShop.Text + "' AND CAST(a.DATE AS DATE) BETWEEN '" + dtDateFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateTo.Value.ToString("yyyy-MM-dd") + "'  ORDER BY a.ID", con).ExecuteReader();
                }

                DataTable dt = new DataTable();
                // Fill DataTable with data from the database
                dt.Load(dr);

                // Bind the DataTable to the DataGridView
                dataGridView2.DataSource = dt;

                // Mark all loaded rows as "existing"
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    row.Tag = "existing";  // Mark rows loaded from DB as existing
                                           // Set the row to read-only if the Tag is 'existing'
                    if (row.Tag.ToString() == "existing")
                    {
                        row.ReadOnly = true;  // Make the entire row readonly
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.ReadOnly = true;  // Optionally, set each individual cell to readonly
                        }
                    }
                    // Check if the Quantity value is valid (numeric)
                    if (row.Cells["QUANTITY"].Value != null)
                    {
                        string quantityValue = row.Cells["QUANTITY"].Value.ToString();
                        if (!int.TryParse(quantityValue, out int quantity) || quantity < 0)
                        {
                            // If the value is not a valid number or is negative, color the cell red
                            row.Cells["QUANTITY"].Style.BackColor = Color.Red;
                            row.Cells["QUANTITY"].Style.ForeColor = Color.White; // Optional: change text color to white for better visibility
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }


        private void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Add functionality to handle clicking column headers for sorting if necessary.
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Set the flag to true when a row is added
            isDataChanged = true;
            ToggleValidateButton();
            AddRow();
        }

        private void dataGridView2_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            // Mark the newly added row as "new"
            DataGridViewRow newRow = e.Row;
            newRow.Tag = "new";  // Mark this row as "new" since it was just added by the user
        }

        // Assuming 'dataGridView2' is your DataGridView control
        private void AddRow()
        {
            DataTable dt = (DataTable)dataGridView2.DataSource;

            // Create a new row
            DataRow newRow = dt.NewRow();

            // Set values for the new row
            newRow["PRODUCT_CODE"] = "";
            newRow["PRODUCT_NAME"] = "";
            newRow["DATE"] = DateTime.Now.ToString("yyyy-MM-dd");
            newRow["QUANTITY"] = "0";
            newRow["ADJUSTMENT"] = "0";
            newRow["GROSS_AMOUNT"] = "0";
            newRow["TOTAL_COST_IN_GROSS"] = "0";
            newRow["SHOP"] = cmbShop.Text.Equals("ALL") ? "" : cmbShop.Text;


            // Add the new row to the DataTable
            dt.Rows.Add(newRow);

            // Mark the new row as "new"
            DataGridViewRow newDataGridViewRow = dataGridView2.Rows[dataGridView2.Rows.Count - 1];
            newDataGridViewRow.Tag = "new";  // Mark it as new
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            SaveDataToDatabase();
            Cursor = Cursors.Default;


        }


        private void SaveDataToDatabase()
        {
            dataGridView2.EndEdit();

            List<DataGridViewRow> rowsToInsert = new List<DataGridViewRow>();
            List<DataGridViewRow> rowsToUpdate = new List<DataGridViewRow>();


            // Check for red color in the grid before processing
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                string productCode = row.Cells["PRODUCT_CODE"].Value?.ToString();
                string shop = row.Cells["SHOP"].Value?.ToString().ToUpper();
                if (shop == "")
                {
                    MessageBox.Show($"Error: Empty value is not allowed for column name SHOP'. Please correct the data before saving/updating.");
                    row.Cells["SHOP"].Style.BackColor = Color.Red;
                    return;
                }
                Color originalColor = row.Cells["PRODUCT_CODE"].Style.BackColor;
                row.Cells["SHOP"].Style.BackColor = originalColor;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    // Check if the cell's background color is red (you can adjust the color based on your needs)
                    if (cell.Style.BackColor == Color.Red)
                    {
                        // Retrieve column name (header text)
                        string columnName = dataGridView2.Columns[cell.ColumnIndex].HeaderText;

                        MessageBox.Show($"Error: Error detected in Product Code '{productCode}' at column '{columnName}'. Please correct the data before saving/updating.");
                        return; // Exit the method if red color is found
                    }
                }
            }
            // Categorize the rows into new or existing
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Tag?.ToString() == "new")
                {
                    rowsToInsert.Add(row);  // New rows
                }
                else if (row.Tag?.ToString() == "existing")
                {
                    rowsToUpdate.Add(row);  // Modified existing rows
                }
            }

            try
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open();

                    // Insert new rows into tblReport first
                    foreach (var row in rowsToInsert)
                    {
                        string productCode = row.Cells["PRODUCT_CODE"].Value?.ToString();
                        string productName = row.Cells["PRODUCT_NAME"].Value?.ToString();
                        string date = row.Cells["DATE"].Value?.ToString();
                        string quantity = row.Cells["QUANTITY"].Value?.ToString() ?? "0";  // Default to "0" if null
                        string adjustment = row.Cells["ADJUSTMENT"].Value?.ToString() ?? "0";  // Default to "0" if null
                        string grossAmount = row.Cells["GROSS_AMOUNT"].Value?.ToString() ?? "0";  // Default to "0" if null
                        string totalGross = row.Cells["TOTAL_COST_IN_GROSS"].Value?.ToString() ?? "0";
                        string shop = row.Cells["SHOP"].Value?.ToString().ToUpper();

                        decimal parsedQuantity = 0;
                        decimal parsedAdjustment = 0;
                        decimal totalGrossValue = 0;

                        double dbEndingInventory = GetendingInventoryFromDatabase(productCode);

                        if (adjustment == "")
                        {
                            adjustment = "0";
                        }
                        // Handle the parsing of numeric fields safely
                        if (!decimal.TryParse(quantity, out parsedQuantity))
                        {
                            MessageBox.Show($"Invalid quantity value: {quantity}. Must be a valid number.");
                            return;
                        }
                        // Handle the parsing of numeric fields safely
                        if (!decimal.TryParse(adjustment, out parsedAdjustment))
                        {
                            MessageBox.Show($"Invalid adjustment value: {adjustment}. Must be a valid number.");
                            return;
                        }
                        if (!decimal.TryParse(totalGross, out totalGrossValue))
                        {
                            totalGrossValue = 0;  // Default to 0 if invalid
                        }


                        if (dbEndingInventory >= double.Parse(quantity))
                        {
                            if (productCode != "")
                            {
                                // Insert into tblReport
                                string queryInsert = "INSERT INTO tblReport (PRODUCT_CODE, PRODUCT_NAME, DATE, QUANTITY, GROSS_AMOUNT, TOTAL_COST_IN_GROSS, SHOP) " +
                                                     "VALUES (@productCode, @productName, @date, @quantity, @grossAmount, @totalCostInGross, @shop)";

                                using (SqlCommand cmd = new SqlCommand(queryInsert, con))
                                {
                                    cmd.Parameters.AddWithValue("@productCode", productCode);
                                    cmd.Parameters.AddWithValue("@productName", productName);
                                    cmd.Parameters.AddWithValue("@date", date);
                                    cmd.Parameters.AddWithValue("@quantity", parsedQuantity);  // Insert the new quantity
                                    cmd.Parameters.AddWithValue("@adjustment", adjustment);
                                    cmd.Parameters.AddWithValue("@grossAmount", grossAmount);
                                    cmd.Parameters.Add("@totalCostInGross", SqlDbType.Decimal).Value = totalGrossValue;
                                    cmd.Parameters.AddWithValue("@shop", shop);

                                    cmd.ExecuteNonQuery();  // Execute the insert query
                                }
                            }
                            // Adjust inventory after insertion
                            AdjustendingInventory(con, productCode, parsedQuantity, false, 0, parsedAdjustment, false, 0);
                            updateIssued();
                            updateTotal();
                            Color originalColor = row.Cells["QUANTITY"].Style.BackColor;
                            row.Cells["QUANTITY"].Style.BackColor = originalColor;
                            // Mark as "existing" after insertion
                            row.Tag = "existing";
                        }
                        else
                        {
                            DialogResult = MessageBox.Show("Error: Invalid quantity, the available stock left for product " + productName + " is only " + dbEndingInventory, " Error");
                            row.Cells["QUANTITY"].Style.BackColor = Color.Red;
                            return;
                        }
                    }

                    // Now handle the update of existing rows (rowsToUpdate)
                    foreach (var row in rowsToUpdate)
                    {
                        string productCode = row.Cells["PRODUCT_CODE"].Value?.ToString();
                        string productName = row.Cells["PRODUCT_NAME"].Value?.ToString();
                        string date = row.Cells["DATE"].Value?.ToString();
                        string quantity = row.Cells["QUANTITY"].Value?.ToString() ?? "0";  // Default to "0" if null
                        string adjustment = row.Cells["ADJUSTMENT"].Value?.ToString() ?? "0";
                        string grossAmount = row.Cells["GROSS_AMOUNT"].Value?.ToString() ?? "0";  // Default to "0" if null
                        string totalGross = row.Cells["TOTAL_COST_IN_GROSS"].Value?.ToString() ?? "0";
                        string shop = row.Cells["SHOP"].Value?.ToString().ToUpper();
                        string id = row.Cells["ID"].Value?.ToString();

                        decimal parsedQuantity = 0;
                        decimal parsedAdjustment = 0;
                        if (adjustment == "")
                        {
                            adjustment = "0";
                        }
                        // Handle the parsing of numeric fields safely
                        if (!decimal.TryParse(quantity, out parsedQuantity))
                        {
                            MessageBox.Show($"Invalid quantity value: {quantity}. Must be a valid number.");
                            return;
                        }
                        // Handle the parsing of numeric fields safely
                        if (!decimal.TryParse(adjustment, out parsedAdjustment))
                        {
                            MessageBox.Show($"Invalid adjustment value: {adjustment}. Must be a valid number.");
                            return;
                        }

                        // Retrieve the existing values from the database
                        decimal oldQuantity = GetOldQuantityFromDatabase(con, id);
                        decimal oldAdjustment = GetOldAdjustmentFromDatabase(con, id);
                        string oldShop = GetOldShopFromDatabase(con, id);

                        // Check if the values have changed
                        if (parsedQuantity != oldQuantity || shop != oldShop || parsedAdjustment != oldAdjustment)
                        {

                            // Update the existing row's data in tblReport
                            string queryUpdate = "UPDATE tblReport SET " +
                                                 "PRODUCT_CODE = @productCode, " +
                                                 "PRODUCT_NAME = @productName, " +
                                                 "DATE = @date, " +
                                                 "QUANTITY = @quantity, " +  // Update quantity
                                                 "ADJUSTMENT = @adjustment, " +
                                                 "GROSS_AMOUNT = @grossAmount, " +
                                                 "TOTAL_COST_IN_GROSS = @totalCostInGross, " +
                                                 "SHOP = @shop " +
                                                 "WHERE ID = @id";

                            using (SqlCommand cmd = new SqlCommand(queryUpdate, con))
                            {
                                cmd.Parameters.AddWithValue("@productCode", productCode);
                                cmd.Parameters.AddWithValue("@productName", productName);
                                cmd.Parameters.AddWithValue("@date", date);
                                cmd.Parameters.AddWithValue("@quantity", parsedQuantity);  // Update with new quantity
                                cmd.Parameters.AddWithValue("@adjustment", parsedAdjustment);
                                cmd.Parameters.AddWithValue("@grossAmount", grossAmount);
                                cmd.Parameters.Add("@totalCostInGross", SqlDbType.Decimal).Value = totalGross;
                                cmd.Parameters.AddWithValue("@shop", shop);
                                cmd.Parameters.AddWithValue("@id", id);

                                cmd.ExecuteNonQuery();  // Execute the update query
                            }
                            decimal originalAdjustment = GetOriginalAdjustment(con, productCode);
                            // Adjust inventory based on the change in quantity
                            decimal quantityDifference = parsedQuantity - oldQuantity;
                            decimal adjustmentDifference = parsedAdjustment - oldAdjustment;
                            bool isAdd = quantityDifference > 0;
                            bool isAddAdjustment = adjustmentDifference > 0;
                            AdjustendingInventory(con, productCode, quantityDifference, isAdd, oldQuantity, oldAdjustment, isAddAdjustment, adjustmentDifference);
                        }

                        // Mark as "existing" after update
                        row.Tag = "existing";
                    }
                    updateIssued();
                    updateTotal();

                }
                Cursor.Equals(Cursors.Default);
                MessageBox.Show("Data saved/updated successfully.");
                loadData();
                isDataChanged = false;
                btnSave.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving/updating data: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void updateTotal()
        {
            // Update the ISSUED column using a CTE and LEFT JOIN, with ISNUMERIC and CASE for safe conversion to INT
            string updateQuery = @"
            UPDATE tblInventory 
            SET TOTAL = LTRIM(STR(ROUND(CAST(ISNULL(ISSUED, '0') AS FLOAT) * CAST(ISNULL(NET_PRICE, '0') AS FLOAT), 2), 18, 2))
            WHERE DATE = @date";
            using (SqlConnection con = new SqlConnection(cnn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                {

                    // Set date parameter to the same format "MMM-yyyy" (e.g., "Nov-2024")
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MMM-yyyy"));

                    // Execute the update query
                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
        }

        private void updateIssued()
        {
            // Update the ISSUED column using a CTE and LEFT JOIN, with ISNUMERIC and CASE for safe conversion to INT
            string updateQuery = @"
                        WITH IssuedSum AS ( SELECT PRODUCT_CODE, SUM(CASE WHEN ISNUMERIC(QUANTITY) = 1 THEN CAST(QUANTITY AS INT) ELSE 0 END) AS TotalIssued
                            FROM tblReport 
                             WHERE LEFT(DATENAME(MONTH, CAST(DATE AS DATE)), 3) + '-' + LEFT(DATE, 4) = @date
                            GROUP BY PRODUCT_CODE)
                        UPDATE t
                        SET t.ISSUED = ISNULL(i.TotalIssued, 0)
                        FROM tblInventory t
                        JOIN IssuedSum i ON t.PRODUCT_CODE = i.PRODUCT_CODE
                        WHERE DATE = @date";
            using (SqlConnection con = new SqlConnection(cnn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                {

                    // Set date parameter to the same format "MMM-yyyy" (e.g., "Nov-2024")
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MMM-yyyy"));

                    // Execute the update query
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Helper method to get the existing total gross for a product from tblReport
        private string GetOldShopFromDatabase(SqlConnection con, string id)
        {
            string query = "SELECT SHOP FROM tblReport WHERE ID = @id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                var result = cmd.ExecuteScalar();
                return result.ToString();
            }
        }




        // Helper method to adjust inventory based on quantity change
        private void AdjustendingInventory(SqlConnection con, string productCode, decimal quantityChange, bool isAdd, decimal oldQuantity
            , decimal adjustment, bool isAddAdjustment, decimal adjustmentChange)
        {
            // Determine the direction of the change (subtract if isAdd is false, add if isAdd is true)
            decimal inventoryChange = isAdd ? quantityChange : quantityChange;
            decimal oldAdjustments = Convert.ToDecimal(adjustment);
            // Retrieve the current ENDING_INVENTORY
            decimal originalInventory = GetOriginalendingInventory(con, productCode);
            decimal originalAdjustment = GetOriginalAdjustment(con, productCode);

            // Calculate the new ending inventory

            decimal newAdjustment = originalAdjustment + adjustmentChange;
            decimal newInventory = isAdd ? (originalInventory + oldQuantity) - (inventoryChange + oldQuantity) : (originalInventory - inventoryChange);
            newInventory = (newInventory + originalAdjustment) - newAdjustment;

            // Update ENDING_INVENTORY in tblInventory
            string query = "UPDATE tblInventory SET ENDING_INVENTORY = @newInventory, ADJUSTMENT = @adjustment WHERE PRODUCT_CODE = @productCode and DATE = @date";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@newInventory", newInventory);
                cmd.Parameters.AddWithValue("@adjustment", newAdjustment);
                cmd.Parameters.AddWithValue("@productCode", productCode);
                cmd.Parameters.AddWithValue("@date", Convert.ToString(DateTime.Now.ToString("MMM-yyyy")));
                cmd.ExecuteNonQuery();
            }
        }
        private decimal GetOldAdjustmentFromDatabase(SqlConnection con, string id)
        {
            string query = "SELECT ADJUSTMENT FROM tblReport WHERE ID = @id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        // Helper method to get the current quantity of a product from tblReport
        private decimal GetOldQuantityFromDatabase(SqlConnection con, string id)
        {
            string query = "SELECT QUANTITY FROM tblReport WHERE ID = @id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        private decimal GetAdjustmentFromDatabase(SqlConnection con, string productCode)
        {
            string query = "SELECT ADJUSTMENT FROM tblInventory WHERE PRODUCT_CODE = @productCode and DATE = @date";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@productCode", productCode);
                cmd.Parameters.AddWithValue("@date", Convert.ToString(DateTime.Now.ToString("MMM-yyyy")));
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        // Helper method to get the original ENDING_INVENTORY value for a product from tblInventory
        private decimal GetOriginalendingInventory(SqlConnection con, string productCode)
        {
            string query = "SELECT ENDING_INVENTORY FROM tblInventory WHERE PRODUCT_CODE = @productCode AND DATE = @date";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@productCode", productCode);
                cmd.Parameters.AddWithValue("@date", Convert.ToString(DateTime.Now.ToString("MMM-yyyy")));
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }
        private decimal GetOriginalAdjustment(SqlConnection con, string productCode)
        {
            string query = "SELECT ADJUSTMENT FROM tblInventory WHERE PRODUCT_CODE = @productCode AND DATE = @date";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@productCode", productCode);
                cmd.Parameters.AddWithValue("@date", Convert.ToString(DateTime.Now.ToString("MMM-yyyy")));
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            // Retrieve the edited value and convert it to uppercase
            var editedValue = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = editedValue.ToUpper();
            // Ensure that we are not processing the header row
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                // Get the column index for QUANTITY, GROSS_AMOUNT, and TOTAL_COST_IN_GROSS
                int colQuantityIndex = dataGridView2.Columns["QUANTITY"].Index;
                int colGrossAmountIndex = dataGridView2.Columns["GROSS_AMOUNT"].Index;
                int colTotalCostIndex = dataGridView2.Columns["TOTAL_COST_IN_GROSS"].Index;
                int colAdjusmentIndex = dataGridView2.Columns["ADJUSTMENT"].Index;

                // Make sure the columns exist and we're not trying to access an invalid column
                if (colQuantityIndex == -1 || colGrossAmountIndex == -1 || colTotalCostIndex == -1)
                {
                    MessageBox.Show("Required columns are missing in the DataGridView.");
                    return;
                }

                // Get the values for QUANTITY and GROSS_AMOUNT
                var quantityValue = row.Cells[colQuantityIndex].Value;
                var grossAmountValue = row.Cells[colGrossAmountIndex].Value;
                var adjustmentValue = row.Cells[colAdjusmentIndex].Value;
                // Check if QUANTITY is empty or null
                if (adjustmentValue == null || string.IsNullOrEmpty(adjustmentValue.ToString()))
                {
                    adjustmentValue = 0;  // Set to 0 if empty
                }
                // Check if both values are valid numbers
                if (quantityValue != null && grossAmountValue != null &&
                    double.TryParse(quantityValue.ToString(), out double quantity) &&
                    double.TryParse(grossAmountValue.ToString(), out double grossAmount))
                {
                    // Calculate the TOTAL_COST_IN_GROSS
                    double totalCostInGross = (quantity + Convert.ToDouble(adjustmentValue)) * grossAmount;

                    // Update the TOTAL_COST_IN_GROSS cell with the calculated value
                    row.Cells[colTotalCostIndex].Value = totalCostInGross;
                }
                else
                {
                    // If the values are invalid, clear the TOTAL_COST_IN_GROSS cell
                    row.Cells[colTotalCostIndex].Value = DBNull.Value;
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        private void DeleteRecordFromDatabase(String productCode)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open();
                    string query = "DELETE FROM tblReport WHERE ID = @id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", productCode);
                        int rowsAffected = cmd.ExecuteNonQuery();


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting record with ID {productCode}: {ex.Message}");
            }
        }
        private void cmbShop_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void dtDateFrom_ValueChanged_1(object sender, EventArgs e)
        {
            // Get the selected shop
            string selectedShop = cmbShop.SelectedItem.ToString();
            // Get the selected shop
            string selectedDateFrom = dtDateFrom.Value.ToString("yyyy-MM-dd");
            string selectedDateTo = dtDateTo.Value.ToString("yyyy-MM-dd");
            FilterDataByShopAndDate(selectedShop, selectedDateFrom, selectedDateTo);
        }

        private void dtDateTo_ValueChanged(object sender, EventArgs e)
        {
            // Get the selected shop
            string selectedShop = cmbShop.SelectedItem.ToString();
            // Get the selected shop
            string selectedDateFrom = dtDateFrom.Value.ToString("yyyy-MM-dd");
            string selectedDateTo = dtDateTo.Value.ToString("yyyy-MM-dd");
            FilterDataByShopAndDate(selectedShop, selectedDateFrom, selectedDateTo);
        }



        public class ReportViewerForm : Form
        {
            public ReportViewerForm(DataTable dt, String name, string counter)
            {
                // Initialize CrystalReportViewer
                CrystalReportViewer crystalReportViewer = new CrystalReportViewer();
                crystalReportViewer.Dock = DockStyle.Fill;

                // Create a new report document
                ReportDocument reportDocument = new ReportDocument();

                // Build the dynamic path to the report file using the relative path
                string reportPath = Path.Combine(Application.StartupPath, "prntStockIssuance.rpt");

                // Ensure the report file exists at the expected location
                if (File.Exists(reportPath))
                {
                    // Load the Crystal Report with the dynamic path
                    reportDocument.Load(reportPath);

                    // Set the data source for the report
                    reportDocument.SetDataSource(dt);

                    // Set the report source for the Crystal Report Viewer
                    crystalReportViewer.ReportSource = reportDocument;
                    // Pass the value of the label to the report parameter "test"
                    //string labelValue = "Sample Report"; // Assume lblTest is your Label control
                    reportDocument.SetParameterValue("processedBy", name); // Set the parameter value
                    reportDocument.SetParameterValue("controlNum", counter); // Set the parameter value

                    // Add the viewer to the form
                    this.Controls.Add(crystalReportViewer);
                }
                else
                {
                    MessageBox.Show("The report file could not be found at: " + reportPath);
                }
            }
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            int counter = 0;
            using (SqlConnection con = new SqlConnection(cnn))
            {
                con.Open();
                String query = "SELECT ID FROM IncrementingIDs";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            counter = dr.GetInt32(0);
                        }
                    }

                }
            }
            counter++;
            string controlNumber = AddZeros(counter);
            // Create a DataTable to hold the selected rows data
            DataTable dt = new DataTable();
            dt.Columns.Add("PRODUCT_CODE");
            dt.Columns.Add("PRODUCT_NAME");
            dt.Columns.Add("DATE");
            dt.Columns.Add("QUANTITY");
            dt.Columns.Add("ADJUSTMENT");
            dt.Columns.Add("GROSS_AMOUNT");
            dt.Columns.Add("TOTAL_COST_IN_GROSS");
            dt.Columns.Add("SHOP");
            dt.Columns.Add("UNIT_OF_MEASUREMENT");

            // Get the current sorting column and sort order from the DataGridView
            DataGridViewColumn sortedColumn = dataGridView2.SortedColumn;
            System.Windows.Forms.SortOrder sortOrder = dataGridView2.SortOrder;

            // Sort the selected rows based on the current sorting in the grid
            var sortedRows = dataGridView2.SelectedRows.Cast<DataGridViewRow>();

            // Apply the sorting logic based on the column and order
            if (sortedColumn != null)
            {
                // Sorting based on the sorted column
                if (sortOrder == System.Windows.Forms.SortOrder.Ascending)
                {
                    sortedRows = sortedRows.OrderBy(row => row.Cells[sortedColumn.Name].Value.ToString()).ToList();
                }
                else if (sortOrder == System.Windows.Forms.SortOrder.Descending)
                {
                    sortedRows = sortedRows.OrderByDescending(row => row.Cells[sortedColumn.Name].Value.ToString()).ToList();
                }
            }
            else
            {
                sortedRows = dataGridView2.SelectedRows.Cast<DataGridViewRow>()
                      .OrderBy(row => row.Cells["ID"].Value) // Sorting based on the "ID" column
                      .ToList();
            }
            // Loop through sorted rows in the DataGridView and add to the DataTable
            foreach (DataGridViewRow row in sortedRows)
            {
                string adjustment = row.Cells["ADJUSTMENT"].Value.ToString() == "0" ? "" : row.Cells["ADJUSTMENT"].Value.ToString();
                DataRow dr = dt.NewRow();
                dr["PRODUCT_CODE"] = row.Cells["PRODUCT_CODE"].Value;
                dr["PRODUCT_NAME"] = row.Cells["PRODUCT_CODE"].Value + "-" + row.Cells["PRODUCT_NAME"].Value;
                dr["DATE"] = row.Cells["DATE"].Value;
                dr["QUANTITY"] = row.Cells["QUANTITY"].Value;
                dr["ADJUSTMENT"] = adjustment;
                dr["GROSS_AMOUNT"] = row.Cells["GROSS_AMOUNT"].Value;
                dr["TOTAL_COST_IN_GROSS"] = row.Cells["TOTAL_COST_IN_GROSS"].Value;
                dr["SHOP"] = row.Cells["SHOP"].Value;
                dr["UNIT_OF_MEASUREMENT"] = row.Cells["UNIT_OF_MEASUREMENT"].Value;
                dt.Rows.Add(dr);
            }

            // Check if any rows were selected
            if (dt.Rows.Count > 0)
            {
                // Open the ReportViewerForm and pass the sorted data to it
                ReportViewerForm reportViewer = new ReportViewerForm(dt, name, controlNumber);
                reportViewer.Show();
                String query = "";
                using (SqlConnection con = new SqlConnection(cnn))
                {

                    con.Open();
                    if (counter == 1)
                    {
                        query = "INSERT into IncrementingIDs (ID)  values (1)";
                    }
                    else
                    {
                        query = "UPDATE IncrementingIDs SET ID = '" + counter + "'";
                    }
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.ExecuteReader();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select rows to print.");
            }
        }

        static string AddZeros(int number)
        {
            int digitCount = number.ToString().Length;
            int zerosToAdd = Math.Max(0, 5 - digitCount);
            return number.ToString("D" + (digitCount + zerosToAdd));
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            // Ask for password when the checkbox is checked
            string password = PromptForPassword();

            if (password == "password") // Replace "your_password" with the actual password
            {
                // Loop through all rows in the DataGridView
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    // Set specific columns to be editable
                    //row.Cells["PRODUCT_CODE"].ReadOnly = false;
                    row.Cells["QUANTITY"].ReadOnly = false;
                    row.Cells["SHOP"].ReadOnly = false;
                    row.Cells["ADJUSTMENT"].ReadOnly = false;

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

        private void dataGridView2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }

        private void cmbShop_SelectedIndexChanged_2(object sender, EventArgs e)
        {

        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {


        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx",
                Title = "Select an Excel File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string connectionString = GetConnectionString(filePath);
                LoadExcelData(connectionString);
            }
        }
        private string GetConnectionString(string filePath)
        {
            string ext = System.IO.Path.GetExtension(filePath);
            if (ext == ".xls")
            {
                return $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={filePath};Extended Properties=\"Excel 8.0;HDR=Yes;\"";
            }
            else if (ext == ".xlsx")
            {
                return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties=\"Excel 12.0;HDR=Yes;\"";
            }
            else
            {
                throw new Exception("Invalid file type.");
            }
        }

        private void LoadExcelData(string connectionString)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sheetName = "PETRON MALVAR$"; // Replace with the exact sheet name

                    // Load PRODUCT_CODE and SHOP from Excel
                    OleDbDataAdapter adapter = new OleDbDataAdapter($"SELECT PRODUCT_CODE,DATE,QUANTITY, SHOP FROM [{sheetName}]", conn);
                    DataTable excelData = new DataTable();
                    adapter.Fill(excelData);

                    // Create a new DataTable with all required columns
                    DataTable gridData = new DataTable();
                    gridData.Columns.Add("PRODUCT_CODE");
                    gridData.Columns.Add("PRODUCT_NAME");
                    gridData.Columns.Add("DATE");
                    gridData.Columns.Add("QUANTITY");
                    gridData.Columns.Add("ADJUSTMENT");
                    gridData.Columns.Add("GROSS_AMOUNT");
                    gridData.Columns.Add("TOTAL_COST_IN_GROSS");
                    gridData.Columns.Add("SHOP");
                    gridData.Columns.Add("ID");
                    // Get today's date in 'MMM-yyyy' format
                    string today = DateTime.Now.ToString("MMM-yyyy");

                    // Populate PRODUCT_CODE and SHOP, leave other columns empty
                    foreach (DataRow row in excelData.Rows)
                    {
                        DataRow newRow = gridData.NewRow();
                        newRow["PRODUCT_CODE"] = row["PRODUCT_CODE"];
                        newRow["DATE"] = today; // Add today's date in the desired format
                        newRow["QUANTITY"] = row["QUANTITY"];
                        newRow["SHOP"] = row["SHOP"];
                        gridData.Rows.Add(newRow);
                    }

                    // Bind the DataTable to the grid
                    dataGridView2.DataSource = gridData;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void picInventory_Click(object sender, EventArgs e)
        {

        }

        private void Reports_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Ctrl and S are pressed together
            if (e.Control && e.KeyCode == Keys.S)
            {
                // Only trigger the button click if it hasn't already been handled
                if (!e.Handled)
                {
                    // Trigger the click event of btnSave
                    btnSave.PerformClick();

                    // Mark the event as handled
                    e.Handled = true;
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = dataGridView2.SelectedRows.Count > 0;
        }
    }
}
