using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Math;
using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace Dunkin
{
    public partial class EndingInventoryReport : Form
    {
        //String cnn = @"Data Source=dbdunkindev.clsow6k2ei1p.ap-southeast-1.rds.amazonaws.com;DATABASE=dbDunkin;User ID=admin;Password=Dunkinpass";
        String cnn = @"Data Source=LAPTOP-IFV6NHU7;DATABASE=dbDunkin;User ID=sa;Password=p@ssw0rd";
        SqlDataReader dr;

        public EndingInventoryReport()
        {
            InitializeComponent();

        }
        private void EnableDoubleBuffering()
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                                              System.Reflection.BindingFlags.NonPublic |
                                              System.Reflection.BindingFlags.Instance |
                                              System.Reflection.BindingFlags.SetProperty,
                                              null, dataGridView1, new object[] { true });
        }
        private void EndingInventoryReport_Load(object sender, EventArgs e)
        {
            EnableDoubleBuffering();
            dtDateFrom.Value = DateTime.Now;
            dtDateTo.Value = DateTime.Now;
            cmbCategory.SelectedIndex = 0;
            loadData();
        }
        public void loadData()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open();

                    string selectQuery = "WITH LatestInventoryAudit AS ( " +
                    "SELECT PRODUCT_CODE, COLUMN_UPDATED, CAST(NEW_VALUE AS INT) AS NEW_VALUE, " +
                    "UPDATE_DATE, ROW_NUMBER() OVER (PARTITION BY PRODUCT_CODE, COLUMN_UPDATED " +
                    "ORDER BY CASE WHEN UPDATE_DATE = @dateTo THEN 1 " +
                    "WHEN UPDATE_DATE = @dateFrom THEN 2 ELSE 3 END) AS RN " +
                    "FROM tblInventory_Audit WHERE CAST(UPDATE_DATE AS DATE) BETWEEN @dateFrom AND @dateTo ) " +

                    "SELECT a.PRODUCT_CODE, a.PRODUCT_NAME, a.INVENTORY_BEGINNING, " +
                    "ISNULL(c.PO, 0) + ISNULL(c.PO2, 0) + ISNULL(c.PO3, 0) + ISNULL(c.PO4, 0) + ISNULL(c.PO5, 0) AS INBOUND, " +
                    "ISNULL(r.TotalQuantity, 0) + ISNULL(r.TotalAdjustment, 0) AS ISSUED, " +
                    "ISNULL(c.ENDING_INVENTORY, 0) AS ENDING_INVENTORY, a.CATEGORY " +

                    "FROM tblInventory a " +

                    "LEFT JOIN ( " +
                    "SELECT PRODUCT_CODE, " +
                    "SUM(CAST(QUANTITY AS INT)) AS TotalQuantity, " +
                    "SUM(CAST(ADJUSTMENT AS INT)) AS TotalAdjustment " +
                    "FROM tblReport WHERE CAST(DATE AS DATE) BETWEEN @dateFrom AND @dateTo " +
                    "GROUP BY PRODUCT_CODE ) r ON a.PRODUCT_CODE = r.PRODUCT_CODE " +

                    "LEFT JOIN ( " +
                    "SELECT PRODUCT_CODE, " +
                    "MAX(CASE WHEN COLUMN_UPDATED = 'PO' AND RN = 1 THEN NEW_VALUE END) AS PO, " +
                    "MAX(CASE WHEN COLUMN_UPDATED = 'PO2' AND RN = 1 THEN NEW_VALUE END) AS PO2, " +
                    "MAX(CASE WHEN COLUMN_UPDATED = 'PO3' AND RN = 1 THEN NEW_VALUE END) AS PO3, " +
                    "MAX(CASE WHEN COLUMN_UPDATED = 'PO4' AND RN = 1 THEN NEW_VALUE END) AS PO4, " +
                    "MAX(CASE WHEN COLUMN_UPDATED = 'PO5' AND RN = 1 THEN NEW_VALUE END) AS PO5, " +
                    "MAX(CASE WHEN COLUMN_UPDATED = 'ENDING_INVENTORY' AND RN = 1 THEN NEW_VALUE END) AS ENDING_INVENTORY " +
                    "FROM LatestInventoryAudit GROUP BY PRODUCT_CODE ) c ON c.PRODUCT_CODE = a.PRODUCT_CODE " +

                    "ORDER BY CAST(a.PRODUCT_CODE AS INT) ASC";

                    using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@dateFrom", dtDateFrom.Value.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@dateTo", dtDateTo.Value.ToString("yyyy-MM-dd"));
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {

                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            // Bind the DataTable to the DataGridView
                            dataGridView1.DataSource = dt;


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

        private void txtProdName_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtProdName.Text.Trim().ToLower(); // Get the input text and trim it
                                                                   // Filter the rows based on the product name column
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

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering data: " + ex.Message);
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDataGrid(txtProdName.Text);
        }

        private void dtDateFrom_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void dtDateTo_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
