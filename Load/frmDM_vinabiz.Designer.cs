namespace Load
{
    partial class frm_dmVinabiz
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
      this.button3 = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.txt_id = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.button2 = new System.Windows.Forms.Button();
      this.button1 = new System.Windows.Forms.Button();
      this.txt_path = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txt_orderid = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.txt_name = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.parentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.orderid = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.label6 = new System.Windows.Forms.Label();
      this.cmbLoai = new System.Windows.Forms.ComboBox();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.GridviewLoaiDinhNghia = new System.Windows.Forms.DataGridView();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.lamTươiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.thêmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.id_loaidinhnghia = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.stt = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.name_loaidinhnghia = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.orderid_loaidinhnghia = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.del = new System.Windows.Forms.DataGridViewImageColumn();
      this.isAct = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.GridviewLoaiDinhNghia)).BeginInit();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(624, 8);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(75, 23);
      this.button3.TabIndex = 31;
      this.button3.Text = "Xóa";
      this.button3.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 40);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(95, 13);
      this.label1.TabIndex = 38;
      this.label1.Text = "Nhóm Tỉnh/Huyện";
      // 
      // comboBox1
      // 
      this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new System.Drawing.Point(107, 36);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(484, 21);
      this.comboBox1.TabIndex = 26;
      this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // txt_id
      // 
      this.txt_id.Location = new System.Drawing.Point(107, 8);
      this.txt_id.Name = "txt_id";
      this.txt_id.ReadOnly = true;
      this.txt_id.Size = new System.Drawing.Size(27, 20);
      this.txt_id.TabIndex = 28;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 13);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(18, 13);
      this.label5.TabIndex = 37;
      this.label5.Text = "ID";
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(624, 89);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 32;
      this.button2.Text = "Lưu";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(543, 89);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 30;
      this.button1.Text = "Thêm";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // txt_path
      // 
      this.txt_path.BackColor = System.Drawing.Color.White;
      this.txt_path.Location = new System.Drawing.Point(107, 63);
      this.txt_path.Name = "txt_path";
      this.txt_path.Size = new System.Drawing.Size(592, 20);
      this.txt_path.TabIndex = 27;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 66);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(27, 13);
      this.label3.TabIndex = 34;
      this.label3.Text = "Link";
      // 
      // txt_orderid
      // 
      this.txt_orderid.Location = new System.Drawing.Point(644, 36);
      this.txt_orderid.Name = "txt_orderid";
      this.txt_orderid.Size = new System.Drawing.Size(55, 20);
      this.txt_orderid.TabIndex = 29;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(611, 40);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(33, 13);
      this.label4.TabIndex = 35;
      this.label4.Text = "Vị Trí";
      // 
      // txt_name
      // 
      this.txt_name.Location = new System.Drawing.Point(224, 10);
      this.txt_name.Name = "txt_name";
      this.txt_name.Size = new System.Drawing.Size(367, 20);
      this.txt_name.TabIndex = 25;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(145, 13);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(73, 13);
      this.label2.TabIndex = 36;
      this.label2.Text = "Tên Khu Vực:";
      // 
      // dataGridView1
      // 
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToDeleteRows = false;
      this.dataGridView1.AllowUserToResizeColumns = false;
      this.dataGridView1.AllowUserToResizeRows = false;
      this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.name,
            this.Path,
            this.parentId,
            this.orderid});
      this.dataGridView1.Location = new System.Drawing.Point(6, 118);
      this.dataGridView1.MultiSelect = false;
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
      this.dataGridView1.RowHeadersVisible = false;
      this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dataGridView1.Size = new System.Drawing.Size(693, 488);
      this.dataGridView1.TabIndex = 33;
      this.dataGridView1.DataSourceChanged += new System.EventHandler(this.dataGridView1_DataSourceChanged);
      this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
      this.dataGridView1.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridView1_RowStateChanged);
      // 
      // id
      // 
      this.id.DataPropertyName = "id";
      this.id.HeaderText = "ID";
      this.id.Name = "id";
      this.id.Width = 30;
      // 
      // name
      // 
      this.name.DataPropertyName = "name";
      this.name.HeaderText = "Tên ";
      this.name.Name = "name";
      this.name.ReadOnly = true;
      this.name.Width = 200;
      // 
      // Path
      // 
      this.Path.DataPropertyName = "path";
      this.Path.HeaderText = "Link";
      this.Path.Name = "Path";
      this.Path.ReadOnly = true;
      this.Path.Width = 400;
      // 
      // parentId
      // 
      this.parentId.DataPropertyName = "parentId";
      this.parentId.HeaderText = "parentId";
      this.parentId.Name = "parentId";
      this.parentId.Visible = false;
      // 
      // orderid
      // 
      this.orderid.DataPropertyName = "orderid";
      this.orderid.HeaderText = "Vị Trí";
      this.orderid.Name = "orderid";
      this.orderid.Width = 60;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl1.Location = new System.Drawing.Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(710, 614);
      this.tabControl1.TabIndex = 39;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.label6);
      this.tabPage1.Controls.Add(this.cmbLoai);
      this.tabPage1.Controls.Add(this.label5);
      this.tabPage1.Controls.Add(this.button3);
      this.tabPage1.Controls.Add(this.dataGridView1);
      this.tabPage1.Controls.Add(this.label1);
      this.tabPage1.Controls.Add(this.label2);
      this.tabPage1.Controls.Add(this.comboBox1);
      this.tabPage1.Controls.Add(this.txt_name);
      this.tabPage1.Controls.Add(this.txt_id);
      this.tabPage1.Controls.Add(this.label4);
      this.tabPage1.Controls.Add(this.txt_orderid);
      this.tabPage1.Controls.Add(this.button2);
      this.tabPage1.Controls.Add(this.label3);
      this.tabPage1.Controls.Add(this.button1);
      this.tabPage1.Controls.Add(this.txt_path);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(702, 588);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Danh Mục";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(8, 94);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(58, 13);
      this.label6.TabIndex = 40;
      this.label6.Text = "Nhóm Loại";
      this.label6.Click += new System.EventHandler(this.label6_Click);
      // 
      // cmbLoai
      // 
      this.cmbLoai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbLoai.FormattingEnabled = true;
      this.cmbLoai.Location = new System.Drawing.Point(107, 91);
      this.cmbLoai.Name = "cmbLoai";
      this.cmbLoai.Size = new System.Drawing.Size(430, 21);
      this.cmbLoai.TabIndex = 39;
      this.cmbLoai.SelectedIndexChanged += new System.EventHandler(this.cmbLoai_SelectedIndexChanged);
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.groupBox1);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(702, 588);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Định Nghĩ Danh Mục Loại";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.GridviewLoaiDinhNghia);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(3, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(696, 582);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Định Nghĩa Loại Vinabiz";
      // 
      // GridviewLoaiDinhNghia
      // 
      this.GridviewLoaiDinhNghia.AllowUserToAddRows = false;
      this.GridviewLoaiDinhNghia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.GridviewLoaiDinhNghia.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_loaidinhnghia,
            this.stt,
            this.name_loaidinhnghia,
            this.orderid_loaidinhnghia,
            this.del,
            this.isAct});
      this.GridviewLoaiDinhNghia.ContextMenuStrip = this.contextMenuStrip1;
      this.GridviewLoaiDinhNghia.Dock = System.Windows.Forms.DockStyle.Fill;
      this.GridviewLoaiDinhNghia.Location = new System.Drawing.Point(3, 16);
      this.GridviewLoaiDinhNghia.MultiSelect = false;
      this.GridviewLoaiDinhNghia.Name = "GridviewLoaiDinhNghia";
      this.GridviewLoaiDinhNghia.RowHeadersVisible = false;
      this.GridviewLoaiDinhNghia.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.GridviewLoaiDinhNghia.Size = new System.Drawing.Size(690, 563);
      this.GridviewLoaiDinhNghia.TabIndex = 0;
      this.GridviewLoaiDinhNghia.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridviewLoaiDinhNghia_CellClick);
      this.GridviewLoaiDinhNghia.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.GridviewLoaiDinhNghia_CellPainting);
      this.GridviewLoaiDinhNghia.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridviewLoaiDinhNghia_CellValueChanged);
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lamTươiToolStripMenuItem,
            this.thêmToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(125, 48);
      // 
      // lamTươiToolStripMenuItem
      // 
      this.lamTươiToolStripMenuItem.Name = "lamTươiToolStripMenuItem";
      this.lamTươiToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
      this.lamTươiToolStripMenuItem.Text = "Làm Tươi";
      this.lamTươiToolStripMenuItem.Click += new System.EventHandler(this.lamTươiToolStripMenuItem_Click);
      // 
      // thêmToolStripMenuItem
      // 
      this.thêmToolStripMenuItem.Name = "thêmToolStripMenuItem";
      this.thêmToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
      this.thêmToolStripMenuItem.Text = "Thêm";
      this.thêmToolStripMenuItem.Click += new System.EventHandler(this.thêmToolStripMenuItem_Click);
      // 
      // id_loaidinhnghia
      // 
      this.id_loaidinhnghia.DataPropertyName = "id";
      this.id_loaidinhnghia.HeaderText = "id";
      this.id_loaidinhnghia.Name = "id_loaidinhnghia";
      this.id_loaidinhnghia.Visible = false;
      // 
      // stt
      // 
      this.stt.DataPropertyName = "stt";
      this.stt.HeaderText = "STT";
      this.stt.Name = "stt";
      this.stt.Width = 50;
      // 
      // name_loaidinhnghia
      // 
      this.name_loaidinhnghia.DataPropertyName = "name";
      this.name_loaidinhnghia.HeaderText = "Tên";
      this.name_loaidinhnghia.Name = "name_loaidinhnghia";
      this.name_loaidinhnghia.Width = 370;
      // 
      // orderid_loaidinhnghia
      // 
      this.orderid_loaidinhnghia.DataPropertyName = "orderid";
      this.orderid_loaidinhnghia.HeaderText = "Vị Trí";
      this.orderid_loaidinhnghia.Name = "orderid_loaidinhnghia";
      this.orderid_loaidinhnghia.Width = 120;
      // 
      // del
      // 
      this.del.HeaderText = "Xóa";
      this.del.Name = "del";
      this.del.Width = 50;
      // 
      // isAct
      // 
      this.isAct.DataPropertyName = "isAct";
      this.isAct.HeaderText = "Act";
      this.isAct.Name = "isAct";
      this.isAct.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.isAct.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      this.isAct.Width = 50;
      // 
      // frm_dmVinabiz
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(710, 614);
      this.Controls.Add(this.tabControl1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frm_dmVinabiz";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "vinabiz.org";
      this.Load += new System.EventHandler(this.frm_dmVinabiz_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.GridviewLoaiDinhNghia)).EndInit();
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txt_path;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_orderid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Path;
        private System.Windows.Forms.DataGridViewTextBoxColumn parentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderid;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.DataGridView GridviewLoaiDinhNghia;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem lamTươiToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem thêmToolStripMenuItem;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.ComboBox cmbLoai;
    private System.Windows.Forms.DataGridViewTextBoxColumn id_loaidinhnghia;
    private System.Windows.Forms.DataGridViewTextBoxColumn stt;
    private System.Windows.Forms.DataGridViewTextBoxColumn name_loaidinhnghia;
    private System.Windows.Forms.DataGridViewTextBoxColumn orderid_loaidinhnghia;
    private System.Windows.Forms.DataGridViewImageColumn del;
    private System.Windows.Forms.DataGridViewCheckBoxColumn isAct;
  }
}