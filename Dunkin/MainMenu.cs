using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace Dunkin
{
    public partial class MainMenu : Form
    {
        String position;
        String name;
        //String cnn = @"Data Source=dbdunkindev.clsow6k2ei1p.ap-southeast-1.rds.amazonaws.com;DATABASE=dbDunkin;User ID=admin;Password=Dunkinpass";
        String cnn = @"Data Source=LAPTOP-IFV6NHU7;DATABASE=dbDunkin;User ID=sa;Password=p@ssw0rd";
        private bool isDragging = false;
        private Point lastCursor;
        public MainMenu(String position, String name)
        {
            InitializeComponent();
            this.position = position;
            this.name = name;
            timer1.Interval = 300000; // 5 minutes in milliseconds

        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void lblMinimize_Click(object sender, EventArgs e)
        {
            // Minimize the form
            this.WindowState = FormWindowState.Minimized;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized; // Maximized
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);  // Position it at the top-left corner
            this.Size = Screen.PrimaryScreen.Bounds.Size; // Full screen size
            datetime.Start();
            if (!"ADMINISTRATOR".Equals(position.ToUpper()))
            {
                ToolStripItem transactionRemove = toolStrip1.Items["transaction"];
                ToolStripItem maintenanceRemove = toolStrip1.Items["maintenance"];
                toolStrip1.Items.Remove(transactionRemove);
                toolStrip1.Items.Remove(maintenanceRemove);
            }
            CheckStockLevels();
        }
        private void CheckStockLevels()
        {


            using (SqlConnection con = new SqlConnection(cnn))
            {
                con.Open(); // Ensure the connection is open
                string query = @"
                        SELECT COUNT(t.PRODUCT_CODE)
                        FROM tblInventory t
                        INNER JOIN tblStock s 
                            ON t.PRODUCT_CODE = s.PRODUCT_CODE 
                            AND (CAST(t.ENDING_INVENTORY AS INT) <= CAST(s.CRITICAL_QUANTITY AS INT))
                        WHERE t.DATE = @date";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@date", Convert.ToString(DateTime.Now.ToString("MMM-yyyy")));
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            if (dr.GetInt32(0) != 0)
                            {
                                DialogResult result = MessageBox.Show("Warning: Some item are low on stock, please check your inventory.", "Warning");
                            }

                        }
                    }
                }
            }
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


        private void btnManageDeac_Click(object sender, EventArgs e)
        {
            UserManagement user = new UserManagement();
            user.ShowDialog();
        }


        private void btnManageActive_Click(object sender, EventArgs e)
        {
            UserManagement user = new UserManagement();
            user.ShowDialog();

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            CheckStockLevels();
        }

        private void manageUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserManagement user = new UserManagement();
            user.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void stockIssuanceToolStripMenuItem_Click(object sender, EventArgs e)
        {



        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void manageShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShopManagement shop = Application.OpenForms.OfType<ShopManagement>().FirstOrDefault();
            if (shop == null)
            {
                shop = new ShopManagement();
                shop.Show();
            }
            else
            {
                shop.BringToFront();
            }
        }

        private void manageCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductManagement productPrice = Application.OpenForms.OfType<ProductManagement>().FirstOrDefault();
            if (productPrice == null)
            {
                productPrice = new ProductManagement();
                productPrice.Show();
            }
            else
            {
                productPrice.BringToFront();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Logout Confirmation",
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question
      );
            if (result == DialogResult.Yes)
            {
                // Create a list of forms to close
                List<Form> formsToClose = new List<Form>();

                // Collect all open forms except the current one
                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm != this)
                    {
                        formsToClose.Add(openForm);
                    }
                }

                // Close collected forms
                foreach (Form form in formsToClose)
                {
                    form.Close();

                }

                Application.Restart();
            }
        }

        private void stockIssuanceToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            StockIssuance report = new StockIssuance(false, name);
            report.Show();

        }

        private void inventoryRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Stock stock = Application.OpenForms.OfType<Stock>().FirstOrDefault();
             if (stock == null)
             {
                 stock = new Stock();
                 stock.Show();
             }
             else
             {
                 stock.BringToFront();
             } */
            Inventory stock = new Inventory(false);
            stock.Show();
        }

        private void issuedProductReportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void stockIssuanceToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            StockIssuance report = new StockIssuance(true, name);
            report.Show();
        }

        private void issuedProductReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ProductIssued productIssued = new ProductIssued(name);
            productIssued.Show();
        }

        private void inventoryRecordReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inventory stock = new Inventory(true);
            stock.Show();
        }

        private void endingInventoryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EndingInventoryReport endingInventoryReport = new EndingInventoryReport();
            endingInventoryReport.Show();
        }
    }
}
