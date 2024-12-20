using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.Windows.Forms;
using static Dunkin.Reports;
using System.IO;

namespace Dunkin
{
    public partial class Stock : Form
    {
        //String cnn = @"Data Source=dbdunkindev.clsow6k2ei1p.ap-southeast-1.rds.amazonaws.com;DATABASE=dbDunkin;User ID=admin;Password=Dunkinpass";
        String cnn = @"Data Source=LAPTOP-IFV6NHU7;DATABASE=dbDunkin;User ID=sa;Password=p@ssw0rd";
        SqlDataReader dr;
        private bool isDragging = false;
        private Point lastCursor;
        private bool isDataChanged = false;
        bool isReports = false;
        public Stock(bool isReport)
        {
            InitializeComponent();
            // Add event handler for checkbox change
            cbLock.CheckedChanged += new EventHandler(cbLock_CheckedChanged);
            txtProdName.TextChanged += new EventHandler(txtProdName_TextChanged);
            isReports = isReport;
            checkIfReport();
        }
        private void checkIfReport()
        {
            if (isReports)
            {
                cbLock.Hide();
                btnAdd.Hide();
                btnValidate.Hide();
            }
        }
        private void cbLock_CheckedChanged(object sender, EventArgs e)
        {
            // Check if the checkbox is checked
            if (cbLock.Checked)
            {
                // Ask for password when the checkbox is checked
                string password = PromptForPassword();

                if (password == "password") // Replace "your_password" with the actual password
                {
                    // Correct password, make INVENTORY_BEGINNING column editable
                    dataGridView1.Columns["colInventoryBeginning"].ReadOnly = false;
                    dataGridView1.Columns["colPO"].ReadOnly = false;
                    dataGridView1.Columns["colPO2"].ReadOnly = false;
                    dataGridView1.Columns["colPO3"].ReadOnly = false;
                    dataGridView1.Columns["colPO4"].ReadOnly = false;
                    dataGridView1.Columns["colPO5"].ReadOnly = false;
                    //  dataGridView1.Columns["colAdjustment"].ReadOnly = false;

                }
                else
                {
                    // Incorrect password, uncheck the checkbox and show a message
                    cbLock.Checked = false; // Uncheck the checkbox
                    MessageBox.Show("Incorrect password. The column INVENTORY BEGINNING remains locked.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // If the checkbox is unchecked, keep the column read-only
                dataGridView1.Columns["colInventoryBeginning"].ReadOnly = true;
                dataGridView1.Columns["colPO"].ReadOnly = true;
                dataGridView1.Columns["colPO2"].ReadOnly = true;
                dataGridView1.Columns["colPO3"].ReadOnly = true;
                dataGridView1.Columns["colPO4"].ReadOnly = true;
                dataGridView1.Columns["colPO5"].ReadOnly = true;
                // dataGridView1.Columns["colAdjustment"].ReadOnly = true;
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
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            // If the left mouse button is pressed, set dragging to true and capture the cursor position
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastCursor = e.Location;
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
        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void lblMinimize_Click(object sender, EventArgs e)
        {
            // Minimize the form
            this.WindowState = FormWindowState.Minimized;
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

            dataGridView1.Size = new Size(screenSize.Width - 300, screenSize.Height - 200);
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
        private void EnableDoubleBuffering()
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                                              System.Reflection.BindingFlags.NonPublic |
                                              System.Reflection.BindingFlags.Instance |
                                              System.Reflection.BindingFlags.SetProperty,
                                              null, dataGridView1, new object[] { true });
        }
        private void Stock_Load(object sender, EventArgs e)
        {

            EnableDoubleBuffering();
            btnValidate.Enabled = false;
            generateMonth();
            copyAndInsertPreviousMonthData();
            cmbMonth.Text = DateTime.Now.ToString("MMM-yyyy");

            setSizeGrid();
            // Enable both vertical and horizontal scrollbars
            dataGridView1.ScrollBars = ScrollBars.Both;
            // Freeze specific columns
            FreezeColumns();
            // Load data into DataGridView first
            loadData();




        }
        private void FreezeColumns()
        {
            // Freeze the 'PRODUCT_CODE' and 'PRODUCT_NAME' columns
            dataGridView1.Columns["colProductCode"].Frozen = true;
            dataGridView1.Columns["colProductName"].Frozen = true;
        }
        private void validate()
        {
            // Apply the calculation logic for all rows after loading data
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    // Get the values for Inventory Beginning and Issued
                    string category = row.Cells["colCategory"].Value.ToString();
                    string productCode = row.Cells["colProductCode"].Value.ToString();
                    string productName = row.Cells["colProductName"].Value.ToString();
                    decimal inventoryBeginning = GetDecimalValue(row.Cells["colInventoryBeginning"].Value);
                    decimal po = GetDecimalValue(row.Cells["colPO"].Value);
                    decimal po2 = GetDecimalValue(row.Cells["colPO2"].Value);
                    decimal po3 = GetDecimalValue(row.Cells["colPO3"].Value);
                    decimal po4 = GetDecimalValue(row.Cells["colPO4"].Value);
                    decimal po5 = GetDecimalValue(row.Cells["colPO5"].Value);
                    decimal adjustment = GetDecimalValue(row.Cells["colAdjustment"].Value);
                    string remarks = row.Cells["colRemarks"].Value.ToString();
                    decimal issued = GetDecimalValue(row.Cells["colIssued"].Value);
                    string id = row.Cells["colId"].Value.ToString();
                    // Calculate Ending Inventory
                    decimal endingInventory = (inventoryBeginning + po + po2 + po3 + po4 + po5) - (adjustment) - issued;

                    if (!String.IsNullOrEmpty(productCode) && !String.IsNullOrEmpty(productName))
                    {
                        // If Ending Inventory is valid (non-negative), reset the background color and update the value
                        row.Cells["colEndingInventory"].Style.BackColor = row.Cells["colProductCode"].Style.BackColor; // Reset to default color
                        row.Cells["colEndingInventory"].Value = endingInventory;
                        if (row.Tag?.ToString() == "existing")
                        {
                            // Now update the database with the new Ending Inventory value
                            UpdateDatabase(id, productCode, productName, inventoryBeginning, po, po2, po3, po4, po5, endingInventory, remarks, adjustment, category);
                        }
                        else if (row.Tag?.ToString() == "new")
                        {
                            InsertDatabase(id, productCode, productName, inventoryBeginning, po, po2, po3, po4, po5, endingInventory, remarks, adjustment, category);
                            insertStock(productCode, productName, category);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Cursor.Equals(Cursors.Default);
                    MessageBox.Show("Error processing row: " + ex.Message);
                }
            }
            Cursor.Equals(Cursors.Default);
            isDataChanged = false;
            MessageBox.Show("Data saved/updated successfully.");
        }
        private void InsertDatabase(string id, string productCode, string productName, decimal inventoryBeginning, decimal po,
          decimal po2, decimal po3, decimal po4, decimal po5, decimal endingInventory, string remarks, decimal adjustment, string category)
        {
            try
            {
                // Create a new connection to the database
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open();

                    // Prepare the UPDATE query to update the tblInventory for the specific product
                    string insertQuery = "INSERT INTO tblInventory (PRODUCT_CODE, PRODUCT_NAME, INVENTORY_BEGINNING, PO, PO2, PO3, PO4, PO5, " +
                        "ADJUSTMENT, ENDING_INVENTORY, REMARKS, ISSUED, CATEGORY, DATE) VALUES (@productCode, @productName, @inventoryBeginning, @po, @po2, @po3, @po4, " +
                        " @po5, @adjustment, @endingInventory, @remarks, '0', @category, @date)";

                    // Create the command with parameters
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@category", category);
                        cmd.Parameters.AddWithValue("@productCode", productCode);
                        cmd.Parameters.AddWithValue("@productName", productName);
                        cmd.Parameters.AddWithValue("@inventoryBeginning", inventoryBeginning);
                        cmd.Parameters.AddWithValue("@po", po);
                        cmd.Parameters.AddWithValue("@po2", po2);
                        cmd.Parameters.AddWithValue("@po3", po3);
                        cmd.Parameters.AddWithValue("@po4", po4);
                        cmd.Parameters.AddWithValue("@po5", po5);
                        cmd.Parameters.AddWithValue("@adjustment", adjustment);
                        cmd.Parameters.AddWithValue("@endingInventory", endingInventory);
                        cmd.Parameters.AddWithValue("@remarks", remarks);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@date", Convert.ToString(DateTime.Now.ToString("MMM-yyyy")));
                        // Execute the insert query
                        cmd.ExecuteNonQuery();
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting database: " + ex.Message);
            }
        }

        private void insertStock(string productCode, string productName, string category)
        {
            try
            {
                // Create a new connection to the database
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open();

                    string insertQueryToStock = "INSERT INTO tblStock (PRODUCT_CODE, PRODUCT_NAME, GROSS_PRICE, NET_PRICE, CRITICAL QUANTITY, CATEGORY) VALUES (@productCode, @productName, 0 , 0, 0, @category)";
                    // Create the command with parameters
                    using (SqlCommand cmd = new SqlCommand(insertQueryToStock, con))
                    {
                        cmd.Parameters.AddWithValue("@productCode", productCode);
                        cmd.Parameters.AddWithValue("@productName", productName);
                        cmd.Parameters.AddWithValue("@category", category);
                        // Execute the insert query
                        cmd.ExecuteNonQuery();
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting database: " + ex.Message);
            }
        }
        private void UpdateDatabase(string id, string productCode, string productName, decimal inventoryBeginning, decimal po,
            decimal po2, decimal po3, decimal po4, decimal po5, decimal endingInventory, string remarks, decimal adjustment, string category)
        {
            try
            {
                // Create a new connection to the database
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open();

                    // Prepare the UPDATE query to update the tblInventory for the specific product
                    string updateQuery = "UPDATE tblInventory SET ENDING_INVENTORY = @endingInventory, PRODUCT_CODE = @productCode" +
                        ", PRODUCT_NAME = @productName, INVENTORY_BEGINNING = @inventoryBeginning, PO = @po, PO2 = @po2, PO3 = @po3, PO4 = @po4" +
                        " , PO5 = @po5, REMARKS = @remarks, ADJUSTMENT = @adjustment, CATEGORY = @category WHERE ID = @id and DATE = @date";

                    // Create the command with parameters
                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@category", category);
                        cmd.Parameters.AddWithValue("@productCode", productCode);
                        cmd.Parameters.AddWithValue("@productName", productName);
                        cmd.Parameters.AddWithValue("@inventoryBeginning", inventoryBeginning);
                        cmd.Parameters.AddWithValue("@po", po);
                        cmd.Parameters.AddWithValue("@po2", po2);
                        cmd.Parameters.AddWithValue("@po3", po3);
                        cmd.Parameters.AddWithValue("@po4", po4);
                        cmd.Parameters.AddWithValue("@po5", po5);
                        cmd.Parameters.AddWithValue("@adjustment", adjustment);
                        cmd.Parameters.AddWithValue("@endingInventory", endingInventory);
                        cmd.Parameters.AddWithValue("@remarks", remarks);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@date", Convert.ToString(DateTime.Now.ToString("MMM-yyyy")));
                        // Execute the update query
                        cmd.ExecuteNonQuery();
                    }

                    // Prepare the UPDATE query to update the tblReport column for the specific product
                    string updateReport = "UPDATE tblReport SET PRODUCT_CODE = @productCode, PRODUCT_NAME = @productName WHERE PRODUCT_CODE = @productCode";

                    // Create the command with parameters
                    using (SqlCommand cmd = new SqlCommand(updateReport, con))
                    {
                        cmd.Parameters.AddWithValue("@productCode", productCode);
                        cmd.Parameters.AddWithValue("@productName", productName);

                        // Execute the update query
                        cmd.ExecuteNonQuery();
                    }


                    // Close the connection
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating database: " + ex.Message);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Set the flag to true when data changes
            isDataChanged = true;
            ToggleValidateButton();

            //Check if the changed cell is in either colInventoryBeginning or colIssued
            if (e.ColumnIndex == dataGridView1.Columns["colInventoryBeginning"].Index ||
                e.ColumnIndex == dataGridView1.Columns["colPO"].Index || e.ColumnIndex == dataGridView1.Columns["colPO2"].Index ||
                e.ColumnIndex == dataGridView1.Columns["colPO3"].Index || e.ColumnIndex == dataGridView1.Columns["colPO4"].Index ||
                e.ColumnIndex == dataGridView1.Columns["colPO5"].Index || e.ColumnIndex == dataGridView1.Columns["colAdjustment"].Index)
            {
                try
                {
                    // Retrieve the updated values from the grid (handle null values safely)
                    decimal inventoryBeginning = GetDecimalValue(dataGridView1.Rows[e.RowIndex].Cells["colInventoryBeginning"].Value);
                    decimal issued = GetDecimalValue(dataGridView1.Rows[e.RowIndex].Cells["colIssued"].Value);
                    decimal po = GetDecimalValue(dataGridView1.Rows[e.RowIndex].Cells["colPO"].Value);
                    decimal po2 = GetDecimalValue(dataGridView1.Rows[e.RowIndex].Cells["colPO2"].Value);
                    decimal po3 = GetDecimalValue(dataGridView1.Rows[e.RowIndex].Cells["colPO3"].Value);
                    decimal po4 = GetDecimalValue(dataGridView1.Rows[e.RowIndex].Cells["colPO4"].Value);
                    decimal po5 = GetDecimalValue(dataGridView1.Rows[e.RowIndex].Cells["colPO5"].Value);
                    decimal adjustment = GetDecimalValue(dataGridView1.Rows[e.RowIndex].Cells["colAdjustment"].Value);

                    // Calculate the new Ending Inventory
                    decimal endingInventory = (inventoryBeginning + po + po2 + po3 + po4 + po5) - (adjustment) - issued;


                    DataGridViewRow currentRow = dataGridView1.Rows[e.RowIndex];
                    currentRow.Cells["colEndingInventory"].Value = endingInventory;
                    /* Check if the error flag is set in the row's Tag property (nullable boolean)
                    bool? hasShownError = currentRow.Tag as bool?;

                    // If Ending Inventory is negative and the error has not been shown
                    if (endingInventory < 0 && hasShownError != true)
                    {
                        // Highlight the row in red
                        currentRow.Cells["colEndingInventory"].Style.BackColor = Color.Red;


                        // Show the error message only once
                        MessageBox.Show("Error: Ending inventory cannot be negative.", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Set a flag in the row's Tag property to indicate the error message has been shown
                        currentRow.Tag = true;

                    }
                    else if (endingInventory >= 0)
                    {
                        // If Ending Inventory is not negative, remove the red color
                        currentRow.Cells["colEndingInventory"].Style.BackColor = currentRow.Cells["colPO"].Style.BackColor;

                        // Update the Ending Inventory cell with the calculated value
                        currentRow.Cells["colEndingInventory"].Value = endingInventory;

                        // Reset the error flag if the inventory is valid
                        currentRow.Tag = null; // Remove the error flag
                    }
                    */
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating Ending Inventory: " + ex.Message);
                }
            }

        }

        // Helper method to safely get decimal values, handling nulls
        private decimal GetDecimalValue(object cellValue)
        {
            if (cellValue == null || cellValue == DBNull.Value || string.IsNullOrEmpty(cellValue.ToString()))
            {
                return 0; // Treat empty or null as zero
            }
            return Convert.ToDecimal(cellValue);
        }

        private void generateMonth()
        {
            using (SqlConnection con = new SqlConnection(cnn))
            {
                con.Open();
                // Now load the data from tblInventory into the ComboBox
                string selectQuery = "SELECT DISTINCT(DATE) FROM tblInventory";

                using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Clear existing items
                    cmbMonth.Items.Clear();

                    // Loop through the data and extract month
                    while (reader.Read())
                    {

                        cmbMonth.Items.Add(reader.GetString(0));

                    }
                }
            }
        }
        private void copyAndInsertPreviousMonthData()
        {
            if (!cmbMonth.Items.Contains(DateTime.Now.ToString("MMM-yyyy")))
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open();

                    // Get the previous month
                    string previousMonth = DateTime.Now.AddMonths(-1).ToString("MMM-yyyy");
                    string currentMonth = DateTime.Now.ToString("MMM-yyyy");
                    // Copy data from the previous month
                    string copyQuery = @"
                    INSERT INTO tblInventory (PRODUCT_CODE, PRODUCT_NAME, INVENTORY_BEGINNING, ENDING_INVENTORY, CATEGORY, DATE, PO,ISSUED,PO2,PO3,PO4,PO5,ADJUSTMENT)
                    SELECT 
                    PRODUCT_CODE, PRODUCT_NAME, ENDING_INVENTORY, ENDING_INVENTORY, CATEGORY, @currentMonth,0,0,0,0,0,0,0
                    FROM tblInventory
                    WHERE DATE = @previousMonth";

                    // Now insert the new data
                    using (SqlCommand cmd = new SqlCommand(copyQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@previousMonth", previousMonth);
                        cmd.Parameters.AddWithValue("@currentMonth", currentMonth);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            generateMonth();

        }

        public void loadData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open();



                    // Now load the data from tblInventory into the DataGridView
                    string selectQuery = "SELECT CATEGORY, PRODUCT_CODE, PRODUCT_NAME, INVENTORY_BEGINNING, PO, PO2, PO3, PO4, PO5, ADJUSTMENT, ISSUED, ENDING_INVENTORY, REMARKS, ID FROM tblInventory" +
                        " WHERE DATE = @date ORDER BY CAST(PRODUCT_CODE AS INT) ASC";
                    using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@date", cmbMonth.Text);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {

                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            // Bind the DataTable to the DataGridView
                            dataGridView1.DataSource = dt;

                            // Attach the CellValueChanged event handler
                            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
                            checkLowStockRed();


                        }
                    }

                    con.Close();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            Cursor.Equals(Cursors.Default);
        }

        private void checkLowStockRed()
        {
            if (cmbMonth.Text == DateTime.Now.ToString("MMM-yyyy"))
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Tag = "existing";  // Mark rows loaded from DB as existing
                    decimal productCode = GetDecimalValue(row.Cells["colProductCode"].Value);
                    decimal endingInventory = GetDecimalValue(row.Cells["colEndingInventory"].Value);

                    using (SqlConnection con = new SqlConnection(cnn))
                    {
                        con.Open();
                        // Now load the data from tblInventory into the DataGridView
                        string selectQuery = "SELECT CRITICAL_QUANTITY FROM tblStock WHERE PRODUCT_CODE = @productCode";
                        using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@productCode", productCode);
                            // Execute the reader
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    // Read the data from the reader
                                    if (endingInventory <= GetDecimalValue(dr.GetString(0)))
                                    {
                                        row.Cells["colEndingInventory"].Style.BackColor = Color.Red;
                                        row.Cells["colEndingInventory"].ToolTipText = "Low on stocks";
                                    }
                                }

                                con.Close();

                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Equals(Cursors.WaitCursor);
            checkLowStock();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Set the flag to true when a row is added
            isDataChanged = true;
            ToggleValidateButton();
            AddRow();
        }
        // Method to enable or disable the Validate button
        private void ToggleValidateButton()
        {
            // Enable the button if there are changes, otherwise disable it
            btnValidate.Enabled = isDataChanged;
        }
        private void AddRow()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            // Create a new row
            DataRow newRow = dt.NewRow();

            // Set values for the new row
            newRow["PRODUCT_CODE"] = "";
            newRow["PRODUCT_NAME"] = "";
            newRow["PO"] = "0";
            newRow["PO2"] = "0";
            newRow["PO3"] = "0";
            newRow["PO4"] = "0";
            newRow["PO5"] = "0";
            newRow["ADJUSTMENT"] = "0";
            newRow["INVENTORY_BEGINNING"] = "0";
            newRow["ISSUED"] = "0";
            newRow["ENDING_INVENTORY"] = "0";
            newRow["REMARKS"] = "";
            // Ask the user for the category input using the InputBox (simple pop-up)
            string category = PromptForCategory();

            // Check if the user provided a category (not empty)
            if (!string.IsNullOrEmpty(category))
            {
                newRow["CATEGORY"] = category; // Assign the category to the new row
            }

            // Add the new row to the DataTable
            dt.Rows.Add(newRow);

            // Mark the new row as "new"
            DataGridViewRow newDataGridViewRow = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
            newDataGridViewRow.Tag = "new";  // Mark it as new

        }

        private string PromptForCategory()
        {
            // Create a new form for the category prompt
            Form categoryForm = new Form();
            categoryForm.Text = "Enter Category";
            categoryForm.StartPosition = FormStartPosition.CenterParent;


            ComboBox category = new ComboBox();
            category.Items.Add("DONUT");
            category.Items.Add("BEVERAGES");
            category.Items.Add("POPPING TEA");
            category.Items.Add("SAVORY");
            category.Items.Add("KITCHEN & DINING");
            category.Items.Add("CLEANING SUNDIES");
            category.Items.Add("OFFICE SUPPLIES");
            category.Location = new Point(10, 10);
            category.Width = 230;

            // Create the OK button
            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(10, 40);

            // Add controls to the form
            categoryForm.Controls.Add(category);
            categoryForm.Controls.Add(okButton);

            // Set the form's properties and display it
            categoryForm.AcceptButton = okButton;
            categoryForm.ClientSize = new Size(250, 80);

            // Show the form as a dialog and return the entered password if OK is pressed
            if (categoryForm.ShowDialog() == DialogResult.OK)
            {
                return category.Text;
            }

            return string.Empty;
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            Cursor.Equals(Cursors.WaitCursor);
            validate();
        }

        private void cbLock_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void txtProdName_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtProdName.Text.Trim().ToLower(); // Get the input text and trim it
            if (string.IsNullOrEmpty(filterText))
            {
                if (cbLowStock.Checked)
                {
                    checkLowStock();
                    checkLowStockRed();
                }
                else
                {
                    // If there's no text, load all data (or you can choose to hide all rows)
                    FilterDataGrid(filterText);
                }

            }
            else
            {
                // Filter the rows based on the product name column
                FilterDataGrid(filterText);
            }

        }
        private void FilterDataGrid(string filterText)
        {
            try
            {

                // Check if there's text to filter
                if (string.IsNullOrEmpty(filterText))
                {
                    // If no filter text, show all rows
                    if (cmbCategory.Text == "ALL")
                    {
                        (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                    }
                    else
                    {
                        (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"CATEGORY LIKE '%{cmbCategory.Text}%'";
                    }
                }

                else
                {
                    // Apply the filter on ProductName column
                    if (cmbCategory.Text == "ALL")
                    {
                        (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"PRODUCT_NAME LIKE '%{filterText}%'";
                    }
                    else
                    {
                        (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"PRODUCT_NAME LIKE '%{filterText}%' and CATEGORY LIKE '%{cmbCategory.Text}%'";
                    }
                }
                checkLowStockRed();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering data: " + ex.Message);
            }
        }

        private void cbLowStock_CheckedChanged(object sender, EventArgs e)
        {
            cmbCategory.Text = "ALL";
            checkLowStock();
            checkLowStockRed();

        }



        private void checkLowStock()
        {
            txtProdName.Text = string.Empty;

            // Check if the checkbox is checked
            if (cbLowStock.Checked)
            {
                // Loop through the rows in reverse order to avoid collection modification issues
                for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
                {
                    var row = dataGridView1.Rows[i];
                    int endingInventory = Convert.ToInt32(row.Cells["colEndingInventory"].Value);
                    decimal productCode = GetDecimalValue(row.Cells["colProductCode"].Value);
                    using (SqlConnection con = new SqlConnection(cnn))
                    {
                        con.Open();
                        // Now load the data from tblInventory into the DataGridView
                        string selectQuery = "SELECT CRITICAL_QUANTITY FROM tblStock WHERE PRODUCT_CODE = @productCode";
                        using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@productCode", productCode);
                            // Execute the reader
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    // Read the data from the reader
                                    if (endingInventory > GetDecimalValue(dr.GetString(0)))
                                    {
                                        dataGridView1.Rows.RemoveAt(i);
                                    }
                                }
                                con.Close();
                            }
                        }
                    }

                }
            }
            else
            {
                loadData();
            }
        }
    

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDataGrid(txtProdName.Text);
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbLowStock.Enabled = false;
            if (cmbMonth.Text == DateTime.Now.ToString("MMM-yyyy"))
            {
                cbLowStock.Enabled = true;
            }
           
            cbLowStock.Checked = false;
            loadData();
        }

        private void dataGridView1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)  // Check if Ctrl+V is pressed
            {
                PasteDataIntoSelectedCells();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a row is selected in DataGridView
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a row from the DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the selected PRODUCT_CODE from DataGridView
                string selectedProductCode = dataGridView1.SelectedRows[0].Cells["colProductCode"].Value.ToString();
                // SQL query with WHERE condition for the selected PRODUCT_CODE
                string query = @"
                    SELECT 
                        tblInventory.PRODUCT_CODE, 
                        tblInventory.PRODUCT_NAME, 
                        tblInventory.INVENTORY_BEGINNING, 
                         CAST(tblInventory.PO AS INT) + CAST(tblInventory.PO2 AS INT) + CAST(tblInventory.PO3 AS INT) + CAST(tblInventory.PO4 AS INT) + CAST(tblInventory.PO5 AS INT) AS TOTAL_PO,
                        tblInventory.ISSUED, 
                        tblInventory.ENDING_INVENTORY, 
                        tblInventory.ADJUSTMENT, 
                        tblStock.NET_PRICE, 
                         CAST(tblInventory.ISSUED AS DECIMAL(18, 2)) * CAST(tblStock.NET_PRICE AS DECIMAL(18, 2)) AS TOTAL_COST
                    FROM tblInventory
                    INNER JOIN tblStock 
                    ON tblInventory.PRODUCT_CODE = tblStock.PRODUCT_CODE AND tblInventory.PRODUCT_CODE = @ProductCode AND tblStock.PRODUCT_CODE = @ProductCode
                    WHERE tblInventory.PRODUCT_CODE = @ProductCode";

                // Fetch data from the database
                DataTable dataTable = new DataTable();
                using (SqlConnection connection = new SqlConnection(cnn))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add the parameter for PRODUCT_CODE
                        command.Parameters.AddWithValue("@ProductCode", selectedProductCode);

                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }

                // Open the ReportViewerForm and pass the sorted data to it
                ReportViewerForm reportViewer = new ReportViewerForm(dataTable, selectedProductCode);
                reportViewer.ShowDialog();
                

               
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class ReportViewerForm : Form
        {
            public ReportViewerForm(DataTable dt, string productCode)
            {
                // Initialize CrystalReportViewer
                CrystalReportViewer crystalReportViewer = new CrystalReportViewer();
                crystalReportViewer.Dock = DockStyle.Fill;

                // Create a new report document
                ReportDocument reportDocument = new ReportDocument();

                // Build the dynamic path to the report file using the relative path
                string reportPath = Path.Combine(Application.StartupPath, "printStock.rpt");

                // Ensure the report file exists at the expected location
                if (File.Exists(reportPath))
                {
                    // Load the Crystal Report with the dynamic path
                    reportDocument.Load(reportPath);

                    // Set the data source for the report
                    reportDocument.SetDataSource(dt);

                    // Set the report source for the Crystal Report Viewer
                    crystalReportViewer.ReportSource = reportDocument;

                    // Pass the PRODUCT_CODE parameter to the report
                    reportDocument.SetParameterValue("ProductCodeParam", productCode);

                    // Add the viewer to the form
                    this.Controls.Add(crystalReportViewer);
                }
                else
                {
                    MessageBox.Show("The report file could not be found at: " + reportPath);
                }
            }
        }
    }
}
