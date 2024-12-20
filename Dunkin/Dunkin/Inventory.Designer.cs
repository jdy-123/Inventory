namespace Dunkin
{
    partial class Inventory
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbLock = new System.Windows.Forms.CheckBox();
            this.btnValidate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtProdName = new System.Windows.Forms.TextBox();
            this.cbLowStock = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.TOTAL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NET_PRICE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INBOUND = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndingInventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIssued = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdjustment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPO5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPO4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPO3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPO2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInventoryBeginning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Linen;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProductCode,
            this.colProductName,
            this.colCategory,
            this.colInventoryBeginning,
            this.colPO,
            this.colPO2,
            this.colPO3,
            this.colPO4,
            this.colPO5,
            this.colAdjustment,
            this.colIssued,
            this.colEndingInventory,
            this.colRemarks,
            this.colId,
            this.INBOUND,
            this.NET_PRICE,
            this.TOTAL});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.GridColor = System.Drawing.Color.White;
            this.dataGridView1.Location = new System.Drawing.Point(214, 64);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowHeadersWidth = 25;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.Size = new System.Drawing.Size(9999, 9999);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown_1);
            // 
            // btnAdd
            // 
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Dunkin", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnAdd.Location = new System.Drawing.Point(31, 209);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(121, 28);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "ADD ITEM";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Dunkin", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.DarkOrange;
            this.button1.Location = new System.Drawing.Point(449, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 29);
            this.button1.TabIndex = 10;
            this.button1.Text = "REFRESH";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbLock
            // 
            this.cbLock.AutoSize = true;
            this.cbLock.Font = new System.Drawing.Font("Dunkin", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLock.ForeColor = System.Drawing.Color.DarkOrange;
            this.cbLock.Location = new System.Drawing.Point(214, 6);
            this.cbLock.Name = "cbLock";
            this.cbLock.Size = new System.Drawing.Size(137, 26);
            this.cbLock.TabIndex = 27;
            this.cbLock.Text = "Enable Grid";
            this.cbLock.UseVisualStyleBackColor = true;
            this.cbLock.CheckedChanged += new System.EventHandler(this.cbLock_CheckedChanged_1);
            // 
            // btnValidate
            // 
            this.btnValidate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnValidate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidate.Font = new System.Drawing.Font("Dunkin", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidate.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnValidate.Location = new System.Drawing.Point(31, 295);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(121, 28);
            this.btnValidate.TabIndex = 28;
            this.btnValidate.Text = "VALIDATE";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtProdName);
            this.groupBox1.Font = new System.Drawing.Font("Dunkin", 9F);
            this.groupBox1.ForeColor = System.Drawing.Color.DarkOrange;
            this.groupBox1.Location = new System.Drawing.Point(9, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 51);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Product Name";
            // 
            // txtProdName
            // 
            this.txtProdName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtProdName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtProdName.ForeColor = System.Drawing.Color.DarkOrange;
            this.txtProdName.Location = new System.Drawing.Point(3, 23);
            this.txtProdName.Name = "txtProdName";
            this.txtProdName.Size = new System.Drawing.Size(179, 19);
            this.txtProdName.TabIndex = 0;
            this.txtProdName.TextChanged += new System.EventHandler(this.txtProdName_TextChanged);
            // 
            // cbLowStock
            // 
            this.cbLowStock.AutoSize = true;
            this.cbLowStock.Font = new System.Drawing.Font("Dunkin", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLowStock.ForeColor = System.Drawing.Color.DarkOrange;
            this.cbLowStock.Location = new System.Drawing.Point(214, 30);
            this.cbLowStock.Name = "cbLowStock";
            this.cbLowStock.Size = new System.Drawing.Size(229, 26);
            this.cbLowStock.TabIndex = 30;
            this.cbLowStock.Text = "Filter Low on Stocks";
            this.cbLowStock.UseVisualStyleBackColor = true;
            this.cbLowStock.CheckedChanged += new System.EventHandler(this.cbLowStock_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbCategory);
            this.groupBox2.Font = new System.Drawing.Font("Dunkin", 9F);
            this.groupBox2.ForeColor = System.Drawing.Color.DarkOrange;
            this.groupBox2.Location = new System.Drawing.Point(10, 138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 51);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Category";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCategory.Font = new System.Drawing.Font("Dunkin", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCategory.ForeColor = System.Drawing.Color.DarkOrange;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "ALL",
            "DONUT",
            "BEVERAGES",
            "POPPING TEA",
            "SAVORY",
            "KITCHEN & DINING",
            "CLEANING SUNDIES",
            "OFFICE SUPPLIES"});
            this.cmbCategory.Location = new System.Drawing.Point(6, 21);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(175, 24);
            this.cmbCategory.TabIndex = 31;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmbMonth);
            this.groupBox3.Font = new System.Drawing.Font("Dunkin", 9F);
            this.groupBox3.ForeColor = System.Drawing.Color.DarkOrange;
            this.groupBox3.Location = new System.Drawing.Point(9, 24);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(187, 51);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Month";
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbMonth.Font = new System.Drawing.Font("Dunkin", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMonth.ForeColor = System.Drawing.Color.DarkOrange;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(6, 21);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(175, 24);
            this.cmbMonth.TabIndex = 31;
            this.cmbMonth.SelectedIndexChanged += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
            // 
            // btnPrint
            // 
            this.btnPrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Dunkin", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnPrint.Location = new System.Drawing.Point(31, 209);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(121, 28);
            this.btnPrint.TabIndex = 33;
            this.btnPrint.Text = "PRINT";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PeachPuff;
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnValidate);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(-1, -8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(205, 9999);
            this.panel1.TabIndex = 34;
            // 
            // btnDelete
            // 
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Dunkin", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnDelete.Location = new System.Drawing.Point(31, 252);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(121, 28);
            this.btnDelete.TabIndex = 34;
            this.btnDelete.Text = "DELETE";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.button2_Click);
            // 
            // TOTAL
            // 
            this.TOTAL.DataPropertyName = "TOTAL";
            this.TOTAL.HeaderText = "TOTAL";
            this.TOTAL.Name = "TOTAL";
            this.TOTAL.Visible = false;
            this.TOTAL.Width = 80;
            // 
            // NET_PRICE
            // 
            this.NET_PRICE.DataPropertyName = "NET_PRICE";
            this.NET_PRICE.HeaderText = "NET_PRICE";
            this.NET_PRICE.Name = "NET_PRICE";
            this.NET_PRICE.Visible = false;
            this.NET_PRICE.Width = 116;
            // 
            // INBOUND
            // 
            this.INBOUND.DataPropertyName = "INBOUND";
            this.INBOUND.HeaderText = "INBOUND";
            this.INBOUND.Name = "INBOUND";
            this.INBOUND.Visible = false;
            this.INBOUND.Width = 102;
            // 
            // colId
            // 
            this.colId.DataPropertyName = "ID";
            this.colId.HeaderText = "ID";
            this.colId.Name = "colId";
            this.colId.Visible = false;
            this.colId.Width = 47;
            // 
            // colRemarks
            // 
            this.colRemarks.DataPropertyName = "REMARKS";
            this.colRemarks.HeaderText = "REMARKS";
            this.colRemarks.Name = "colRemarks";
            this.colRemarks.Width = 107;
            // 
            // colEndingInventory
            // 
            this.colEndingInventory.DataPropertyName = "Ending_Inventory";
            this.colEndingInventory.HeaderText = "ENDING_INVENTORY";
            this.colEndingInventory.Name = "colEndingInventory";
            this.colEndingInventory.ReadOnly = true;
            this.colEndingInventory.Width = 184;
            // 
            // colIssued
            // 
            this.colIssued.DataPropertyName = "ISSUED";
            this.colIssued.HeaderText = "ISSUED";
            this.colIssued.Name = "colIssued";
            this.colIssued.ReadOnly = true;
            this.colIssued.Width = 88;
            // 
            // colAdjustment
            // 
            this.colAdjustment.DataPropertyName = "ADJUSTMENT";
            this.colAdjustment.HeaderText = "ADJUSTMENT";
            this.colAdjustment.Name = "colAdjustment";
            this.colAdjustment.ReadOnly = true;
            this.colAdjustment.Width = 134;
            // 
            // colPO5
            // 
            this.colPO5.DataPropertyName = "PO5";
            this.colPO5.HeaderText = "INBOUND_5";
            this.colPO5.Name = "colPO5";
            this.colPO5.ReadOnly = true;
            this.colPO5.Width = 118;
            // 
            // colPO4
            // 
            this.colPO4.DataPropertyName = "PO4";
            this.colPO4.HeaderText = "INBOUND_4";
            this.colPO4.Name = "colPO4";
            this.colPO4.ReadOnly = true;
            this.colPO4.Width = 118;
            // 
            // colPO3
            // 
            this.colPO3.DataPropertyName = "PO3";
            this.colPO3.HeaderText = "INBOUND_3";
            this.colPO3.Name = "colPO3";
            this.colPO3.ReadOnly = true;
            this.colPO3.Width = 118;
            // 
            // colPO2
            // 
            this.colPO2.DataPropertyName = "PO2";
            this.colPO2.HeaderText = "INBOUND_2";
            this.colPO2.Name = "colPO2";
            this.colPO2.ReadOnly = true;
            this.colPO2.Width = 118;
            // 
            // colPO
            // 
            this.colPO.DataPropertyName = "PO";
            this.colPO.HeaderText = "INBOUND_1";
            this.colPO.Name = "colPO";
            this.colPO.ReadOnly = true;
            this.colPO.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPO.Width = 118;
            // 
            // colInventoryBeginning
            // 
            this.colInventoryBeginning.DataPropertyName = "INVENTORY_BEGINNING";
            this.colInventoryBeginning.HeaderText = "INVENTORY_BEGINNING";
            this.colInventoryBeginning.Name = "colInventoryBeginning";
            this.colInventoryBeginning.ReadOnly = true;
            this.colInventoryBeginning.Width = 209;
            // 
            // colCategory
            // 
            this.colCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colCategory.DataPropertyName = "CATEGORY";
            this.colCategory.HeaderText = "CATEGORY";
            this.colCategory.Name = "colCategory";
            this.colCategory.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCategory.Visible = false;
            this.colCategory.Width = 116;
            // 
            // colProductName
            // 
            this.colProductName.DataPropertyName = "PRODUCT_NAME";
            this.colProductName.HeaderText = "PRODUCT_NAME";
            this.colProductName.Name = "colProductName";
            this.colProductName.Width = 159;
            // 
            // colProductCode
            // 
            this.colProductCode.DataPropertyName = "PRODUCT_CODE";
            this.colProductCode.HeaderText = "PRODUCT_CODE";
            this.colProductCode.Name = "colProductCode";
            this.colProductCode.ReadOnly = true;
            this.colProductCode.Width = 160;
            // 
            // Inventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Linen;
            this.ClientSize = new System.Drawing.Size(1165, 576);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbLowStock);
            this.Controls.Add(this.cbLock);
            this.Controls.Add(this.dataGridView1);
            this.DoubleBuffered = true;
            this.Name = "Inventory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inventory";
            this.Load += new System.EventHandler(this.Stock_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Stock_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbLock;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtProdName;
        private System.Windows.Forms.CheckBox cbLowStock;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInventoryBeginning;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPO2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPO3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPO4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPO5;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdjustment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIssued;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndingInventory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn INBOUND;
        private System.Windows.Forms.DataGridViewTextBoxColumn NET_PRICE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOTAL;
    }
}