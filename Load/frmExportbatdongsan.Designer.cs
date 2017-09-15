namespace Load
{
    partial class frmExportbatdongsan
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rad_hosocongty_excel = new System.Windows.Forms.RadioButton();
            this.rad_hosocongty_filetxt = new System.Windows.Forms.RadioButton();
            this.btn_hosocongty_excel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.cmb_bds_nhom = new System.Windows.Forms.ComboBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmb_bds_nhom);
            this.groupBox2.Controls.Add(this.rad_hosocongty_excel);
            this.groupBox2.Controls.Add(this.rad_hosocongty_filetxt);
            this.groupBox2.Controls.Add(this.btn_hosocongty_excel);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(495, 68);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            // 
            // rad_hosocongty_excel
            // 
            this.rad_hosocongty_excel.AutoSize = true;
            this.rad_hosocongty_excel.Location = new System.Drawing.Point(349, 46);
            this.rad_hosocongty_excel.Name = "rad_hosocongty_excel";
            this.rad_hosocongty_excel.Size = new System.Drawing.Size(51, 17);
            this.rad_hosocongty_excel.TabIndex = 20;
            this.rad_hosocongty_excel.Text = "Excel";
            this.rad_hosocongty_excel.UseVisualStyleBackColor = true;
            // 
            // rad_hosocongty_filetxt
            // 
            this.rad_hosocongty_filetxt.AutoSize = true;
            this.rad_hosocongty_filetxt.Checked = true;
            this.rad_hosocongty_filetxt.Location = new System.Drawing.Point(286, 46);
            this.rad_hosocongty_filetxt.Name = "rad_hosocongty_filetxt";
            this.rad_hosocongty_filetxt.Size = new System.Drawing.Size(46, 17);
            this.rad_hosocongty_filetxt.TabIndex = 20;
            this.rad_hosocongty_filetxt.TabStop = true;
            this.rad_hosocongty_filetxt.Text = "TXT";
            this.rad_hosocongty_filetxt.UseVisualStyleBackColor = true;
            // 
            // btn_hosocongty_excel
            // 
            this.btn_hosocongty_excel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_hosocongty_excel.Location = new System.Drawing.Point(414, 19);
            this.btn_hosocongty_excel.Name = "btn_hosocongty_excel";
            this.btn_hosocongty_excel.Size = new System.Drawing.Size(75, 23);
            this.btn_hosocongty_excel.TabIndex = 18;
            this.btn_hosocongty_excel.Text = "Xuất File";
            this.btn_hosocongty_excel.UseVisualStyleBackColor = true;
            this.btn_hosocongty_excel.Click += new System.EventHandler(this.btn_hosocongty_excel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton5);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(9, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(489, 65);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Xử Lý";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(190, 42);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(120, 17);
            this.radioButton5.TabIndex = 23;
            this.radioButton5.Text = "Chuân Hoá (Tất Cả)";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 32);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(58, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Xóa All";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(408, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "Xử Lý";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(190, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(166, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Chuẩn Hoá ( Chỉ Dữ Liệu Mới)";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // cmb_bds_nhom
            // 
            this.cmb_bds_nhom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_bds_nhom.FormattingEnabled = true;
            this.cmb_bds_nhom.Location = new System.Drawing.Point(6, 19);
            this.cmb_bds_nhom.Name = "cmb_bds_nhom";
            this.cmb_bds_nhom.Size = new System.Drawing.Size(394, 21);
            this.cmb_bds_nhom.TabIndex = 55;
            // 
            // frmExportbatdongsan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 148);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExportbatdongsan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "http://batdongsan.com.vn/";
            this.Load += new System.EventHandler(this.frmExportbatdongsan_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rad_hosocongty_excel;
        private System.Windows.Forms.RadioButton rad_hosocongty_filetxt;
        private System.Windows.Forms.Button btn_hosocongty_excel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ComboBox cmb_bds_nhom;
    }
}