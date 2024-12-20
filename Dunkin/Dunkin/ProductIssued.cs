using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dunkin
{
    public partial class ProductIssued : Form
    {
        //String cnn = @"Data Source=dbdunkindev.clsow6k2ei1p.ap-southeast-1.rds.amazonaws.com;DATABASE=dbDunkin;User ID=admin;Password=Dunkinpass";
        String cnn = @"Data Source=LAPTOP-IFV6NHU7;DATABASE=dbDunkin;User ID=sa;Password=p@ssw0rd";
        SqlDataReader dr;
        private bool isDragging = false;
        private Point lastCursor;
        private bool isDataChanged = false;
        private string name;
        bool isReports = false;
        public ProductIssued(String name)
        {
            InitializeComponent();
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
            cmbShop.Sorted = true;
            loadShop();
            dtDateFrom.Value = DateTime.Now;
            dtDateTo.Value = DateTime.Now;
            // TODO: This line of code loads data into the 'dbDunkinDataSet1.tblReport' table. You can move, or remove it, as needed.
            loadData();
            this.cmbShop.SelectedIndexChanged += new System.EventHandler(this.cmbShop_SelectedIndexChanged);

        }
        public void loadData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    con.Open();
                    string query;

                    if ("ALL".Equals(cmbShop.Text))
                    {
                        query = "SELECT PRODUCT_CODE, PRODUCT_NAME, SUM(CAST(QUANTITY AS INT)) + SUM(CAST(ISNULL(ADJUSTMENT, 0) AS INT)) AS QUANTITY, " +
                                "ROUND(SUM(CAST(TOTAL_COST_IN_GROSS AS DECIMAL(18, 2))), 2) AS TOTAL_COST_IN_GROSS, SHOP " +
                                "FROM tblReport WHERE CAST(DATE AS DATE) BETWEEN @dateFrom AND @dateTo " +
                                "GROUP BY SHOP, PRODUCT_CODE, PRODUCT_NAME ORDER BY SHOP, PRODUCT_CODE;";
                    }
                    else
                    {
                        query = "SELECT PRODUCT_CODE, PRODUCT_NAME, SUM(CAST(QUANTITY AS INT)) + SUM(CAST(ISNULL(ADJUSTMENT, 0) AS INT)) AS QUANTITY, " +
                                "ROUND(SUM(CAST(TOTAL_COST_IN_GROSS AS DECIMAL(18, 2))), 2) AS TOTAL_COST_IN_GROSS, SHOP " +
                                "FROM tblReport WHERE SHOP = @shop AND CAST(DATE AS DATE) BETWEEN @dateFrom AND @dateTo " +
                                "GROUP BY SHOP, PRODUCT_CODE, PRODUCT_NAME ORDER BY SHOP, PRODUCT_CODE;";
                    }


                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@dateFrom", dtDateFrom.Value.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@dateTo", dtDateTo.Value.ToString("yyyy-MM-dd"));

                        if (!"ALL".Equals(cmbShop.Text))
                        {
                            cmd.Parameters.AddWithValue("@shop", cmbShop.Text);
                        }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            dataGridView2.DataSource = dt;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
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


        private void cmbShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }



        public class ReportViewerForm : Form
        {
            public ReportViewerForm(DataTable dt, String dateFrom, String dateTo)
            {
                // Initialize CrystalReportViewer
                CrystalReportViewer crystalReportViewer = new CrystalReportViewer();
                crystalReportViewer.Dock = DockStyle.Fill;

                // Create a new report document
                ReportDocument reportDocument = new ReportDocument();

                // Build the dynamic path to the report file using the relative path
                string reportPath = Path.Combine(Application.StartupPath, "printProductIssued.rpt");

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
                    reportDocument.SetParameterValue("date", dateFrom + " - " + dateTo); // Set the parameter value

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
                      .OrderBy(row => row.Cells["PRODUCT_NAME"].Value) // Sorting based on the "ID" column
                      .ToList();
            }
            // Loop through sorted rows in the DataGridView and add to the DataTable
            foreach (DataGridViewRow row in sortedRows)
            {

                DataRow dr = dt.NewRow();
                dr["PRODUCT_CODE"] = row.Cells["PRODUCT_CODE"].Value;
                dr["PRODUCT_NAME"] = row.Cells["PRODUCT_NAME"].Value;
                dr["QUANTITY"] = row.Cells["QUANTITY"].Value;
                dr["TOTAL_COST_IN_GROSS"] = row.Cells["TOTAL_COST_IN_GROSS"].Value;
                dr["SHOP"] = row.Cells["SHOP"].Value;
                dt.Rows.Add(dr);
            }

            // Check if any rows were selected
            if (dt.Rows.Count > 0)
            {
                // Open the ReportViewerForm and pass the sorted data to it
                ReportViewerForm reportViewer = new ReportViewerForm(dt, dtDateFrom.Value.ToString("MMM. dd, yyyy"), dtDateTo.Value.ToString("MMM. dd, yyyy"));
                reportViewer.Show();
            }
            else
            {
                MessageBox.Show("Please select rows to print.");
            }
        }

        private void dtDateTo_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void dtDateFrom_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
