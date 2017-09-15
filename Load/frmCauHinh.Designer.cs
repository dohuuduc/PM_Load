namespace Load {
    partial class frmCauHinh {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chk_scanner = new System.Windows.Forms.CheckBox();
            this.chk_vatgia = new System.Windows.Forms.CheckBox();
            this.chk_TrangVang = new System.Windows.Forms.CheckBox();
            this.chk_hosocongty = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_hosocongty = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.file_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type_desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.physical_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.max_size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state_desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(637, 102);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(55, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Xoá Hết";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chk_scanner);
            this.groupBox1.Controls.Add(this.chk_vatgia);
            this.groupBox1.Controls.Add(this.chk_TrangVang);
            this.groupBox1.Controls.Add(this.chk_hosocongty);
            this.groupBox1.Location = new System.Drawing.Point(21, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(610, 67);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Xoá Dữ Liệu";
            // 
            // chk_scanner
            // 
            this.chk_scanner.AutoSize = true;
            this.chk_scanner.Location = new System.Drawing.Point(268, 42);
            this.chk_scanner.Name = "chk_scanner";
            this.chk_scanner.Size = new System.Drawing.Size(88, 17);
            this.chk_scanner.TabIndex = 3;
            this.chk_scanner.Text = "Phone_Email";
            this.chk_scanner.UseVisualStyleBackColor = true;
            // 
            // chk_vatgia
            // 
            this.chk_vatgia.AutoSize = true;
            this.chk_vatgia.Location = new System.Drawing.Point(268, 19);
            this.chk_vatgia.Name = "chk_vatgia";
            this.chk_vatgia.Size = new System.Drawing.Size(78, 17);
            this.chk_vatgia.TabIndex = 2;
            this.chk_vatgia.Text = "vatgia.com";
            this.chk_vatgia.UseVisualStyleBackColor = true;
            // 
            // chk_TrangVang
            // 
            this.chk_TrangVang.AutoSize = true;
            this.chk_TrangVang.Location = new System.Drawing.Point(20, 42);
            this.chk_TrangVang.Name = "chk_TrangVang";
            this.chk_TrangVang.Size = new System.Drawing.Size(97, 17);
            this.chk_TrangVang.TabIndex = 1;
            this.chk_TrangVang.Text = "trangvang.com";
            this.chk_TrangVang.UseVisualStyleBackColor = true;
            // 
            // chk_hosocongty
            // 
            this.chk_hosocongty.AutoSize = true;
            this.chk_hosocongty.Location = new System.Drawing.Point(20, 19);
            this.chk_hosocongty.Name = "chk_hosocongty";
            this.chk_hosocongty.Size = new System.Drawing.Size(96, 17);
            this.chk_hosocongty.TabIndex = 0;
            this.chk_hosocongty.Text = "hosocongty.vn";
            this.chk_hosocongty.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(637, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(55, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Lưu";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Kết Nối Dữ Liệu";
            // 
            // txt_hosocongty
            // 
            this.txt_hosocongty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_hosocongty.ForeColor = System.Drawing.Color.Red;
            this.txt_hosocongty.Location = new System.Drawing.Point(106, 13);
            this.txt_hosocongty.Multiline = true;
            this.txt_hosocongty.Name = "txt_hosocongty";
            this.txt_hosocongty.Size = new System.Drawing.Size(525, 42);
            this.txt_hosocongty.TabIndex = 11;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(21, 149);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(610, 143);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Quản Lý Log & Database";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.file_id,
            this.name,
            this.type_desc,
            this.physical_name,
            this.size,
            this.max_size,
            this.state_desc});
            this.dataGridView1.Location = new System.Drawing.Point(20, 22);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(584, 115);
            this.dataGridView1.TabIndex = 0;
            // 
            // file_id
            // 
            this.file_id.DataPropertyName = "file_id";
            this.file_id.HeaderText = "id";
            this.file_id.Name = "file_id";
            this.file_id.Width = 30;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "Database";
            this.name.Name = "name";
            this.name.Width = 70;
            // 
            // type_desc
            // 
            this.type_desc.DataPropertyName = "type_desc";
            this.type_desc.HeaderText = "Types";
            this.type_desc.Name = "type_desc";
            this.type_desc.Width = 75;
            // 
            // physical_name
            // 
            this.physical_name.DataPropertyName = "physical_name";
            this.physical_name.HeaderText = "Path";
            this.physical_name.Name = "physical_name";
            this.physical_name.Width = 150;
            // 
            // size
            // 
            this.size.DataPropertyName = "size";
            this.size.HeaderText = "Size";
            this.size.Name = "size";
            this.size.Width = 65;
            // 
            // max_size
            // 
            this.max_size.DataPropertyName = "max_size";
            this.max_size.HeaderText = "Size Max";
            this.max_size.Name = "max_size";
            this.max_size.Width = 80;
            // 
            // state_desc
            // 
            this.state_desc.DataPropertyName = "state_desc";
            this.state_desc.HeaderText = "State";
            this.state_desc.Name = "state_desc";
            this.state_desc.Width = 80;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(637, 203);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(55, 25);
            this.button3.TabIndex = 15;
            this.button3.Text = "Xoá Log";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // frmCauHinh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 304);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_hosocongty);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCauHinh";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kết Nối Database";
            this.Load += new System.EventHandler(this.frmCauHinh_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chk_scanner;
        private System.Windows.Forms.CheckBox chk_vatgia;
        private System.Windows.Forms.CheckBox chk_TrangVang;
        private System.Windows.Forms.CheckBox chk_hosocongty;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_hosocongty;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridViewTextBoxColumn file_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn type_desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn physical_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn size;
        private System.Windows.Forms.DataGridViewTextBoxColumn max_size;
        private System.Windows.Forms.DataGridViewTextBoxColumn state_desc;


    }
}