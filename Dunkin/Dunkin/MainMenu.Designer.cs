namespace Dunkin
{
    partial class MainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.datetime = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.transaction = new System.Windows.Forms.ToolStripDropDownButton();
            this.stockIssuanceToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryRecordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.report = new System.Windows.Forms.ToolStripDropDownButton();
            this.stockIssuanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stockIssuanceToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.issuedProductReportToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryRecordReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endingInventoryReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maintenance = new System.Windows.Forms.ToolStripDropDownButton();
            this.manageUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageShopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.transaction,
            this.report,
            this.maintenance,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1658, 27);
            this.toolStrip1.TabIndex = 39;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // transaction
            // 
            this.transaction.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.transaction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stockIssuanceToolStripMenuItem1,
            this.inventoryRecordsToolStripMenuItem});
            this.transaction.Image = ((System.Drawing.Image)(resources.GetObject("transaction.Image")));
            this.transaction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.transaction.Name = "transaction";
            this.transaction.Size = new System.Drawing.Size(97, 24);
            this.transaction.Text = "Transaction";
            // 
            // stockIssuanceToolStripMenuItem1
            // 
            this.stockIssuanceToolStripMenuItem1.Name = "stockIssuanceToolStripMenuItem1";
            this.stockIssuanceToolStripMenuItem1.Size = new System.Drawing.Size(196, 24);
            this.stockIssuanceToolStripMenuItem1.Text = "Stock Issuance";
            this.stockIssuanceToolStripMenuItem1.Click += new System.EventHandler(this.stockIssuanceToolStripMenuItem1_Click);
            // 
            // inventoryRecordsToolStripMenuItem
            // 
            this.inventoryRecordsToolStripMenuItem.Name = "inventoryRecordsToolStripMenuItem";
            this.inventoryRecordsToolStripMenuItem.Size = new System.Drawing.Size(196, 24);
            this.inventoryRecordsToolStripMenuItem.Text = "Inventory Records";
            this.inventoryRecordsToolStripMenuItem.Click += new System.EventHandler(this.inventoryRecordsToolStripMenuItem_Click);
            // 
            // report
            // 
            this.report.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stockIssuanceToolStripMenuItem,
            this.inventoryToolStripMenuItem});
            this.report.Name = "report";
            this.report.Size = new System.Drawing.Size(73, 24);
            this.report.Text = "Reports";
            this.report.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // stockIssuanceToolStripMenuItem
            // 
            this.stockIssuanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stockIssuanceToolStripMenuItem2,
            this.issuedProductReportToolStripMenuItem1});
            this.stockIssuanceToolStripMenuItem.Name = "stockIssuanceToolStripMenuItem";
            this.stockIssuanceToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.stockIssuanceToolStripMenuItem.Text = "Issuance";
            this.stockIssuanceToolStripMenuItem.Click += new System.EventHandler(this.stockIssuanceToolStripMenuItem_Click);
            // 
            // stockIssuanceToolStripMenuItem2
            // 
            this.stockIssuanceToolStripMenuItem2.Name = "stockIssuanceToolStripMenuItem2";
            this.stockIssuanceToolStripMenuItem2.Size = new System.Drawing.Size(222, 24);
            this.stockIssuanceToolStripMenuItem2.Text = "Stock Issuance Report";
            this.stockIssuanceToolStripMenuItem2.Click += new System.EventHandler(this.stockIssuanceToolStripMenuItem2_Click);
            // 
            // issuedProductReportToolStripMenuItem1
            // 
            this.issuedProductReportToolStripMenuItem1.Name = "issuedProductReportToolStripMenuItem1";
            this.issuedProductReportToolStripMenuItem1.Size = new System.Drawing.Size(222, 24);
            this.issuedProductReportToolStripMenuItem1.Text = "Issued Item Report";
            this.issuedProductReportToolStripMenuItem1.Click += new System.EventHandler(this.issuedProductReportToolStripMenuItem1_Click);
            // 
            // inventoryToolStripMenuItem
            // 
            this.inventoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inventoryRecordReportToolStripMenuItem,
            this.endingInventoryReportToolStripMenuItem});
            this.inventoryToolStripMenuItem.Name = "inventoryToolStripMenuItem";
            this.inventoryToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.inventoryToolStripMenuItem.Text = "Inventory";
            this.inventoryToolStripMenuItem.Click += new System.EventHandler(this.inventoryToolStripMenuItem_Click);
            // 
            // inventoryRecordReportToolStripMenuItem
            // 
            this.inventoryRecordReportToolStripMenuItem.Name = "inventoryRecordReportToolStripMenuItem";
            this.inventoryRecordReportToolStripMenuItem.Size = new System.Drawing.Size(239, 24);
            this.inventoryRecordReportToolStripMenuItem.Text = "Inventory Record Report";
            this.inventoryRecordReportToolStripMenuItem.Click += new System.EventHandler(this.inventoryRecordReportToolStripMenuItem_Click);
            // 
            // endingInventoryReportToolStripMenuItem
            // 
            this.endingInventoryReportToolStripMenuItem.Name = "endingInventoryReportToolStripMenuItem";
            this.endingInventoryReportToolStripMenuItem.Size = new System.Drawing.Size(239, 24);
            this.endingInventoryReportToolStripMenuItem.Text = "Ending Inventory Report";
            this.endingInventoryReportToolStripMenuItem.Click += new System.EventHandler(this.endingInventoryReportToolStripMenuItem_Click);
            // 
            // maintenance
            // 
            this.maintenance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.maintenance.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageUserToolStripMenuItem,
            this.manageCategoryToolStripMenuItem,
            this.manageShopToolStripMenuItem});
            this.maintenance.Image = ((System.Drawing.Image)(resources.GetObject("maintenance.Image")));
            this.maintenance.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.maintenance.Name = "maintenance";
            this.maintenance.Size = new System.Drawing.Size(107, 24);
            this.maintenance.Text = "Maintenance";
            // 
            // manageUserToolStripMenuItem
            // 
            this.manageUserToolStripMenuItem.Name = "manageUserToolStripMenuItem";
            this.manageUserToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
            this.manageUserToolStripMenuItem.Text = "User Management";
            this.manageUserToolStripMenuItem.Click += new System.EventHandler(this.manageUserToolStripMenuItem_Click);
            // 
            // manageCategoryToolStripMenuItem
            // 
            this.manageCategoryToolStripMenuItem.Name = "manageCategoryToolStripMenuItem";
            this.manageCategoryToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
            this.manageCategoryToolStripMenuItem.Text = "Product Management";
            this.manageCategoryToolStripMenuItem.Click += new System.EventHandler(this.manageCategoryToolStripMenuItem_Click);
            // 
            // manageShopToolStripMenuItem
            // 
            this.manageShopToolStripMenuItem.Name = "manageShopToolStripMenuItem";
            this.manageShopToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
            this.manageShopToolStripMenuItem.Text = "Shop Management";
            this.manageShopToolStripMenuItem.Click += new System.EventHandler(this.manageShopToolStripMenuItem_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(60, 24);
            this.toolStripButton4.Text = "Logout";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Dunkin.Properties.Resources.Untitled;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1658, 1008);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Form";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer datetime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton maintenance;
        private System.Windows.Forms.ToolStripMenuItem manageUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton report;
        private System.Windows.Forms.ToolStripMenuItem stockIssuanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageShopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageCategoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripDropDownButton transaction;
        private System.Windows.Forms.ToolStripMenuItem stockIssuanceToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem inventoryRecordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stockIssuanceToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem issuedProductReportToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem inventoryRecordReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem endingInventoryReportToolStripMenuItem;
    }
}