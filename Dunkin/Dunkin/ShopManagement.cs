using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Dunkin
{
    public partial class ShopManagement : Form
    {
        //String cnn = @"Data Source=dbdunkindev.clsow6k2ei1p.ap-southeast-1.rds.amazonaws.com;DATABASE=dbDunkin;User ID=admin;Password=Dunkinpass";
        String cnn = @"Data Source=LAPTOP-IFV6NHU7;DATABASE=dbDunkin;User ID=sa;Password=p@ssw0rd";
        SqlDataReader dr;
        public ShopManagement()
        {

            InitializeComponent();

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void ManageShop_Load(object sender, EventArgs e)
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
        private void SaveDataToDatabase()
        {
            dataGridView1.EndEdit();

            List<DataGridViewRow> rowsToInsert = new List<DataGridViewRow>();
            List<DataGridViewRow> rowsToUpdate = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
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

                    // Insert new rows
                    foreach (var row in rowsToInsert)
                    {
                        string shopName = row.Cells["SHOP_NAME"].Value.ToString().ToUpper();


                        if (string.IsNullOrWhiteSpace(shopName))
                        {
                            continue;
                        }

                        string queryInsert = "INSERT INTO tblShop (SHOP_NAME) values (@shopName)";
                        using (SqlCommand cmd = new SqlCommand(queryInsert, con))
                        {
                            cmd.Parameters.AddWithValue("@shopName", shopName);

                            cmd.ExecuteNonQuery();
                        }

                        row.Tag = "existing";  // Mark as "existing" after insertion
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving/updating data: {ex.Message}");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            SaveDataToDatabase();
            UpdateDataToDatabase();
            MessageBox.Show("Data saved/updated successfully.");
            loadData();
        }

        public void loadData()
        {
            try
            {
                SqlConnection con = new SqlConnection(cnn);
                con.Open();
                dr = new SqlCommand("SELECT ID, SHOP_NAME FROM tblShop", con).ExecuteReader();

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


        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            // Mark the newly added row as "new"
            DataGridViewRow newRow = e.Row;
            newRow.Tag = "new";  // Mark this row as "new" since it was just added by the user
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddRow();
        }

        // Assuming 'dataGridView1' is your DataGridView control
        private void AddRow()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            // Create a new row
            DataRow newRow = dt.NewRow();

            // Set values for the new row
            newRow["SHOP_NAME"] = "";


            // Add the new row to the DataTable
            dt.Rows.Add(newRow);

            // Mark the new row as "new"
            DataGridViewRow newDataGridViewRow = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
            newDataGridViewRow.Tag = "new";  // Mark it as new
        }


        private void UpdateDataToDatabase()
        {
            // Ensure that any ongoing edits are committed before saving
            dataGridView1.EndEdit();

            // Use a list to collect the rows to be updated
            List<DataGridViewRow> rowsToUpdate = new List<DataGridViewRow>();

            // Collect rows that are marked as "existing" and have modified values into the list for processing
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the row is marked as "existing" and any cell in the row is modified (not equal to the original value)
                if (row.Tag?.ToString() == "existing")
                {
                    // Assuming that the original values are stored in the "Tag" or a similar property
                    rowsToUpdate.Add(row);
                }
            }

            try
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open();

                    // Prepare the SQL query for updating records
                    string query = "UPDATE tblShop SET SHOP_NAME = @shopName WHERE ID = @id";

                    // Iterate over all rows to update
                    foreach (var row in rowsToUpdate)
                    {
                        // Retrieve values from the row
                        string shopName = row.Cells["SHOP_NAME"].Value.ToString().ToUpper();
                        int id = Convert.ToInt32(row.Cells["id"].Value);

                        // Check if any field is empty or invalid
                        if (string.IsNullOrWhiteSpace(shopName))
                        {
                            continue; // Skip rows with missing data
                        }

                        // Update data into the database
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@shopName", shopName);
                            cmd.Parameters.AddWithValue("@id", id);

                            cmd.ExecuteNonQuery();  // Execute the query to update data
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the update process
                MessageBox.Show($"Error updating data: {ex.Message}");
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void DeleteRecordFromDatabase(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open();
                    string query = "DELETE FROM tblShop WHERE ID = @id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show($"No record found for ID {id}. It may have already been deleted.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting record with ID {id}: {ex.Message}");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddRow();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            // Check if any rows are selected
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Ask the user to confirm the deletion of all selected rows
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete all selected records?", "Confirm Deletion", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    // Collect all selected rows to delete
                    List<DataGridViewRow> rowsToDelete = new List<DataGridViewRow>();

                    // Loop through the selected rows and add them to the delete list
                    foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
                    {
                        // Avoid issues with deleted rows being processed
                        if (!selectedRow.IsNewRow)
                        {
                            rowsToDelete.Add(selectedRow);
                        }
                    }

                    // Delete records from the database
                    foreach (DataGridViewRow row in rowsToDelete)
                    {
                        int id = Convert.ToInt32(row.Cells["id"].Value);
                        DeleteRecordFromDatabase(id);  // Call the method to delete from the database
                    }

                    // Now remove the rows from the DataGridView
                    foreach (DataGridViewRow row in rowsToDelete)
                    {
                        dataGridView1.Rows.Remove(row);
                    }

                    MessageBox.Show("Selected records deleted successfully.");
                }
            }
            else
            {
                MessageBox.Show("Please select rows to delete.");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            SaveDataToDatabase();
            UpdateDataToDatabase();
            MessageBox.Show("Data saved/updated successfully.");
            loadData();
            Cursor = Cursors.Default;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = dataGridView1.SelectedRows.Count > 0;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
