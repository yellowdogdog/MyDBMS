namespace MyDBMS
{
    partial class Form1
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
            this.btnSQL = new System.Windows.Forms.Button();
            this.lbxTable = new System.Windows.Forms.ListBox();
            this.btnChoose = new System.Windows.Forms.Button();
            this.btnDropTable = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.dgvTableDetial = new System.Windows.Forms.DataGridView();
            this.dgvValue = new System.Windows.Forms.DataGridView();
            this.btnSaveTable = new System.Windows.Forms.Button();
            this.btnSaveValue = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableDetial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValue)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSQL
            // 
            this.btnSQL.Location = new System.Drawing.Point(12, 12);
            this.btnSQL.Name = "btnSQL";
            this.btnSQL.Size = new System.Drawing.Size(75, 23);
            this.btnSQL.TabIndex = 0;
            this.btnSQL.Text = "执行sql";
            this.btnSQL.UseVisualStyleBackColor = true;
            this.btnSQL.Click += new System.EventHandler(this.btnSQL_Click);
            // 
            // lbxTable
            // 
            this.lbxTable.FormattingEnabled = true;
            this.lbxTable.ItemHeight = 12;
            this.lbxTable.Location = new System.Drawing.Point(13, 54);
            this.lbxTable.Name = "lbxTable";
            this.lbxTable.Size = new System.Drawing.Size(239, 436);
            this.lbxTable.TabIndex = 1;
            // 
            // btnChoose
            // 
            this.btnChoose.Location = new System.Drawing.Point(13, 501);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(75, 23);
            this.btnChoose.TabIndex = 2;
            this.btnChoose.Text = "查看表格";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // btnDropTable
            // 
            this.btnDropTable.Location = new System.Drawing.Point(95, 501);
            this.btnDropTable.Name = "btnDropTable";
            this.btnDropTable.Size = new System.Drawing.Size(75, 23);
            this.btnDropTable.TabIndex = 3;
            this.btnDropTable.Text = "删除表格";
            this.btnDropTable.UseVisualStyleBackColor = true;
            this.btnDropTable.Click += new System.EventHandler(this.btnDropTable_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(177, 501);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // dgvTableDetial
            // 
            this.dgvTableDetial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTableDetial.Location = new System.Drawing.Point(272, 54);
            this.dgvTableDetial.Name = "dgvTableDetial";
            this.dgvTableDetial.RowTemplate.Height = 23;
            this.dgvTableDetial.Size = new System.Drawing.Size(464, 194);
            this.dgvTableDetial.TabIndex = 5;
            // 
            // dgvValue
            // 
            this.dgvValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvValue.Location = new System.Drawing.Point(272, 254);
            this.dgvValue.Name = "dgvValue";
            this.dgvValue.RowTemplate.Height = 23;
            this.dgvValue.Size = new System.Drawing.Size(464, 236);
            this.dgvValue.TabIndex = 6;
            // 
            // btnSaveTable
            // 
            this.btnSaveTable.Location = new System.Drawing.Point(741, 225);
            this.btnSaveTable.Name = "btnSaveTable";
            this.btnSaveTable.Size = new System.Drawing.Size(75, 23);
            this.btnSaveTable.TabIndex = 7;
            this.btnSaveTable.Text = "保存表格";
            this.btnSaveTable.UseVisualStyleBackColor = true;
            this.btnSaveTable.Click += new System.EventHandler(this.btnSaveTable_Click);
            // 
            // btnSaveValue
            // 
            this.btnSaveValue.Location = new System.Drawing.Point(741, 466);
            this.btnSaveValue.Name = "btnSaveValue";
            this.btnSaveValue.Size = new System.Drawing.Size(75, 23);
            this.btnSaveValue.TabIndex = 8;
            this.btnSaveValue.Text = "保存内容";
            this.btnSaveValue.UseVisualStyleBackColor = true;
            this.btnSaveValue.Click += new System.EventHandler(this.btnSaveValue_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 571);
            this.Controls.Add(this.btnSaveValue);
            this.Controls.Add(this.btnSaveTable);
            this.Controls.Add(this.dgvValue);
            this.Controls.Add(this.dgvTableDetial);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnDropTable);
            this.Controls.Add(this.btnChoose);
            this.Controls.Add(this.lbxTable);
            this.Controls.Add(this.btnSQL);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableDetial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSQL;
        private System.Windows.Forms.ListBox lbxTable;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.Button btnDropTable;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridView dgvTableDetial;
        private System.Windows.Forms.DataGridView dgvValue;
        private System.Windows.Forms.Button btnSaveTable;
        private System.Windows.Forms.Button btnSaveValue;
    }
}

