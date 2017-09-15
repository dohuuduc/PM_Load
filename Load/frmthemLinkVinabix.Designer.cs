namespace Load
{
    partial class frmthemLinkVinabix
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtvnbiz_ten = new System.Windows.Forms.TextBox();
            this.txt_vnbiz_link = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_vnbiz_trang = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.txt_vnbiz_den = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên Ngành";
            // 
            // txtvnbiz_ten
            // 
            this.txtvnbiz_ten.Location = new System.Drawing.Point(151, 18);
            this.txtvnbiz_ten.Name = "txtvnbiz_ten";
            this.txtvnbiz_ten.Size = new System.Drawing.Size(430, 20);
            this.txtvnbiz_ten.TabIndex = 1;
            // 
            // txt_vnbiz_link
            // 
            this.txt_vnbiz_link.Location = new System.Drawing.Point(151, 44);
            this.txt_vnbiz_link.Name = "txt_vnbiz_link";
            this.txt_vnbiz_link.Size = new System.Drawing.Size(430, 20);
            this.txt_vnbiz_link.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Liên kết";
            // 
            // txt_vnbiz_trang
            // 
            this.txt_vnbiz_trang.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_vnbiz_trang.ForeColor = System.Drawing.Color.Red;
            this.txt_vnbiz_trang.Location = new System.Drawing.Point(151, 126);
            this.txt_vnbiz_trang.Name = "txt_vnbiz_trang";
            this.txt_vnbiz_trang.Size = new System.Drawing.Size(84, 26);
            this.txt_vnbiz_trang.TabIndex = 5;
            this.txt_vnbiz_trang.Text = "0";
            this.txt_vnbiz_trang.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_vnbiz_trang.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_vnbiz_trang_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Trang";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(506, 129);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Lưu";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "nhà đất bán",
            "nhà đất cho thuê",
            "nhà đất cần mua",
            "nhà đất cần thuê",
            "danh bạ"});
            this.comboBox1.Location = new System.Drawing.Point(151, 70);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(430, 21);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Nhóm Danh Mục:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Ngành Nghề:";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "nhà đất bán",
            "nhà đất cho thuê",
            "nhà đất cần mua",
            "nhà đất cần thuê",
            "danh bạ"});
            this.comboBox2.Location = new System.Drawing.Point(151, 97);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(430, 21);
            this.comboBox2.TabIndex = 11;
            // 
            // txt_vnbiz_den
            // 
            this.txt_vnbiz_den.Enabled = false;
            this.txt_vnbiz_den.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_vnbiz_den.ForeColor = System.Drawing.Color.Red;
            this.txt_vnbiz_den.Location = new System.Drawing.Point(282, 126);
            this.txt_vnbiz_den.Name = "txt_vnbiz_den";
            this.txt_vnbiz_den.Size = new System.Drawing.Size(84, 26);
            this.txt_vnbiz_den.TabIndex = 13;
            this.txt_vnbiz_den.Text = "10";
            this.txt_vnbiz_den.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(372, 130);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(58, 17);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "Tất Cả";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(241, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Đến";
            // 
            // frmthemLinkVinabix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 168);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.txt_vnbiz_den);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txt_vnbiz_trang);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_vnbiz_link);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtvnbiz_ten);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmthemLinkVinabix";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "vinabiz.org";
            this.Load += new System.EventHandler(this.frmthemLinkVinabix_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtvnbiz_ten;
        private System.Windows.Forms.TextBox txt_vnbiz_link;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_vnbiz_trang;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox txt_vnbiz_den;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label6;
    }
}