namespace BatchTransfer
{
    partial class Index
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SourceCityName = new System.Windows.Forms.Label();
            this.SourceDeptTable = new System.Windows.Forms.DataGridView();
            this.SourceDeptCheckbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SourceCityList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.GoToCityList = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.GoToCityId = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.GoToDeptId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SourceDeptTable)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.SourceCityName);
            this.groupBox1.Controls.Add(this.SourceDeptTable);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.SourceCityList);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(12, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(908, 553);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "需要转移的资产";
            // 
            // SourceCityName
            // 
            this.SourceCityName.AutoSize = true;
            this.SourceCityName.Location = new System.Drawing.Point(354, 42);
            this.SourceCityName.Name = "SourceCityName";
            this.SourceCityName.Size = new System.Drawing.Size(85, 21);
            this.SourceCityName.TabIndex = 5;
            this.SourceCityName.Text = "城市Id:";
            // 
            // SourceDeptTable
            // 
            this.SourceDeptTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SourceDeptTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SourceDeptCheckbox});
            this.SourceDeptTable.Location = new System.Drawing.Point(20, 119);
            this.SourceDeptTable.Name = "SourceDeptTable";
            this.SourceDeptTable.RowTemplate.Height = 23;
            this.SourceDeptTable.Size = new System.Drawing.Size(870, 415);
            this.SourceDeptTable.TabIndex = 4;
            // 
            // SourceDeptCheckbox
            // 
            this.SourceDeptCheckbox.HeaderText = "选择";
            this.SourceDeptCheckbox.Name = "SourceDeptCheckbox";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "部门列表：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "城市：";
            // 
            // SourceCityList
            // 
            this.SourceCityList.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SourceCityList.FormattingEnabled = true;
            this.SourceCityList.Location = new System.Drawing.Point(95, 41);
            this.SourceCityList.Name = "SourceCityList";
            this.SourceCityList.Size = new System.Drawing.Size(229, 22);
            this.SourceCityList.TabIndex = 1;
            this.SourceCityList.SelectedIndexChanged += new System.EventHandler(this.SourceCityList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(28, 610);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "接收资产的城市：";
            // 
            // GoToCityList
            // 
            this.GoToCityList.FormattingEnabled = true;
            this.GoToCityList.Location = new System.Drawing.Point(202, 610);
            this.GoToCityList.Name = "GoToCityList";
            this.GoToCityList.Size = new System.Drawing.Size(218, 20);
            this.GoToCityList.TabIndex = 3;
            this.GoToCityList.SelectedIndexChanged += new System.EventHandler(this.GoToCityList_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(790, 668);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 36);
            this.button1.TabIndex = 4;
            this.button1.Text = "确认转移";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GoToCityId
            // 
            this.GoToCityId.AutoSize = true;
            this.GoToCityId.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GoToCityId.Location = new System.Drawing.Point(455, 609);
            this.GoToCityId.Name = "GoToCityId";
            this.GoToCityId.Size = new System.Drawing.Size(85, 21);
            this.GoToCityId.TabIndex = 5;
            this.GoToCityId.Text = "城市Id:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(626, 609);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 21);
            this.label6.TabIndex = 6;
            this.label6.Text = "接收部门Id:";
            // 
            // GoToDeptId
            // 
            this.GoToDeptId.Location = new System.Drawing.Point(759, 609);
            this.GoToDeptId.Name = "GoToDeptId";
            this.GoToDeptId.Size = new System.Drawing.Size(143, 21);
            this.GoToDeptId.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(135, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(535, 21);
            this.label5.TabIndex = 10;
            this.label5.Text = "只需要选择要转移的最上级部门，子部门系统自动转移。";
            // 
            // Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 745);
            this.Controls.Add(this.GoToDeptId);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.GoToCityId);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.GoToCityList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Name = "Index";
            this.Text = "批量转移资产";
            this.Load += new System.EventHandler(this.Index_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SourceDeptTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox SourceCityList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView SourceDeptTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label SourceCityName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox GoToCityList;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SourceDeptCheckbox;
        private System.Windows.Forms.Label GoToCityId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox GoToDeptId;
        private System.Windows.Forms.Label label5;
    }
}

