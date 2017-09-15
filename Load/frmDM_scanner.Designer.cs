namespace Load {
    partial class frmDM_scanner {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_tao = new System.Windows.Forms.Button();
            this.dv_cu = new System.Windows.Forms.DataGridView();
            this.id_cu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dosau_cu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name_cu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.path_cu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parentid_cu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderid_cu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statur_cu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dv_moi = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dosau = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parentid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbl_cu = new System.Windows.Forms.Label();
            this.lbl_moi = new System.Windows.Forms.Label();
            this.btn_start = new System.Windows.Forms.Button();
            this.txt_lanlap = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txt_timeout = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtThoiGianCho = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDoSau = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_scanner_bo = new System.Windows.Forms.Button();
            this.btn_chon_scanner = new System.Windows.Forms.Button();
            this.btn_scanner_xoa_cu = new System.Windows.Forms.Button();
            this.lbl_message = new System.Windows.Forms.Label();
            this.btn_scanner_xoa_moi = new System.Windows.Forms.Button();
            this.btn_scanner_dosau_giam = new System.Windows.Forms.Button();
            this.btn_scanner_dosau_tang = new System.Windows.Forms.Button();
            this.btn_scanner_sleep_giam = new System.Windows.Forms.Button();
            this.btn_scanner_sleep_tang = new System.Windows.Forms.Button();
            this.btn_scanner_timeout_giam = new System.Windows.Forms.Button();
            this.btn_scanner_timeout_tang = new System.Windows.Forms.Button();
            this.btn_scanner_soluong_giam = new System.Windows.Forms.Button();
            this.btn_scanner_soluong_tang = new System.Windows.Forms.Button();
            this.btn_scanner_lanlap_giam = new System.Windows.Forms.Button();
            this.btn_scanner_lanlap_tang = new System.Windows.Forms.Button();
            this.lbl_scanner_khoa = new System.Windows.Forms.Label();
            this.btn_scanner_giaihan_tang = new System.Windows.Forms.Button();
            this.btn_scanner_giaihan_giam = new System.Windows.Forms.Button();
            this.txtGioiHan = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_handoi = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dv_cu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dv_moi)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(75, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(665, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Website";
            // 
            // btn_tao
            // 
            this.btn_tao.Location = new System.Drawing.Point(746, 10);
            this.btn_tao.Name = "btn_tao";
            this.btn_tao.Size = new System.Drawing.Size(81, 23);
            this.btn_tao.TabIndex = 2;
            this.btn_tao.Text = "Tạo";
            this.btn_tao.UseVisualStyleBackColor = true;
            this.btn_tao.Click += new System.EventHandler(this.button1_Click);
            // 
            // dv_cu
            // 
            this.dv_cu.AllowUserToAddRows = false;
            this.dv_cu.AllowUserToDeleteRows = false;
            this.dv_cu.AllowUserToOrderColumns = true;
            this.dv_cu.AllowUserToResizeColumns = false;
            this.dv_cu.AllowUserToResizeRows = false;
            this.dv_cu.BackgroundColor = System.Drawing.Color.White;
            this.dv_cu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dv_cu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_cu,
            this.dosau_cu,
            this.name_cu,
            this.path_cu,
            this.parentid_cu,
            this.orderid_cu,
            this.statur_cu});
            this.dv_cu.Location = new System.Drawing.Point(11, 52);
            this.dv_cu.Name = "dv_cu";
            this.dv_cu.RowHeadersVisible = false;
            this.dv_cu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dv_cu.Size = new System.Drawing.Size(907, 228);
            this.dv_cu.TabIndex = 3;
            this.dv_cu.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dv_cu_DataBindingComplete);
            // 
            // id_cu
            // 
            this.id_cu.DataPropertyName = "id";
            this.id_cu.HeaderText = "ID";
            this.id_cu.Name = "id_cu";
            this.id_cu.Width = 50;
            // 
            // dosau_cu
            // 
            this.dosau_cu.DataPropertyName = "dosau";
            this.dosau_cu.HeaderText = "Độ Sâu";
            this.dosau_cu.Name = "dosau_cu";
            this.dosau_cu.Width = 80;
            // 
            // name_cu
            // 
            this.name_cu.DataPropertyName = "name";
            this.name_cu.HeaderText = "Link";
            this.name_cu.Name = "name_cu";
            this.name_cu.Width = 300;
            // 
            // path_cu
            // 
            this.path_cu.DataPropertyName = "path";
            this.path_cu.HeaderText = "Đường Dẫn";
            this.path_cu.Name = "path_cu";
            this.path_cu.Width = 500;
            // 
            // parentid_cu
            // 
            this.parentid_cu.DataPropertyName = "parentid";
            this.parentid_cu.HeaderText = "parentid";
            this.parentid_cu.Name = "parentid_cu";
            this.parentid_cu.Visible = false;
            // 
            // orderid_cu
            // 
            this.orderid_cu.DataPropertyName = "orderid";
            this.orderid_cu.HeaderText = "orderid";
            this.orderid_cu.Name = "orderid_cu";
            this.orderid_cu.Visible = false;
            // 
            // statur_cu
            // 
            this.statur_cu.DataPropertyName = "statur";
            this.statur_cu.HeaderText = "statur";
            this.statur_cu.Name = "statur_cu";
            this.statur_cu.Visible = false;
            // 
            // dv_moi
            // 
            this.dv_moi.AllowUserToAddRows = false;
            this.dv_moi.AllowUserToDeleteRows = false;
            this.dv_moi.AllowUserToOrderColumns = true;
            this.dv_moi.AllowUserToResizeColumns = false;
            this.dv_moi.AllowUserToResizeRows = false;
            this.dv_moi.BackgroundColor = System.Drawing.Color.White;
            this.dv_moi.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.dosau,
            this.name,
            this.path,
            this.parentid,
            this.orderid,
            this.statur});
            this.dv_moi.Location = new System.Drawing.Point(11, 311);
            this.dv_moi.Name = "dv_moi";
            this.dv_moi.RowHeadersVisible = false;
            this.dv_moi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dv_moi.Size = new System.Drawing.Size(907, 259);
            this.dv_moi.TabIndex = 4;
            this.dv_moi.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dv_moi_DataBindingComplete);
            this.dv_moi.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dv_moi_DataError);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 50;
            // 
            // dosau
            // 
            this.dosau.DataPropertyName = "dosau";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dosau.DefaultCellStyle = dataGridViewCellStyle1;
            this.dosau.HeaderText = "Độ Sâu";
            this.dosau.Name = "dosau";
            this.dosau.ReadOnly = true;
            this.dosau.Width = 80;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "Link";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 300;
            // 
            // path
            // 
            this.path.DataPropertyName = "path";
            this.path.HeaderText = "Đường Dẫn";
            this.path.Name = "path";
            this.path.ReadOnly = true;
            this.path.Width = 480;
            // 
            // parentid
            // 
            this.parentid.DataPropertyName = "parentid";
            this.parentid.HeaderText = "parentid";
            this.parentid.Name = "parentid";
            this.parentid.ReadOnly = true;
            this.parentid.Visible = false;
            // 
            // orderid
            // 
            this.orderid.DataPropertyName = "orderid";
            this.orderid.HeaderText = "orderid";
            this.orderid.Name = "orderid";
            this.orderid.Visible = false;
            // 
            // statur
            // 
            this.statur.DataPropertyName = "statur";
            this.statur.HeaderText = "statur";
            this.statur.Name = "statur";
            this.statur.ReadOnly = true;
            this.statur.Visible = false;
            // 
            // lbl_cu
            // 
            this.lbl_cu.AutoSize = true;
            this.lbl_cu.Location = new System.Drawing.Point(12, 36);
            this.lbl_cu.Name = "lbl_cu";
            this.lbl_cu.Size = new System.Drawing.Size(125, 13);
            this.lbl_cu.TabIndex = 5;
            this.lbl_cu.Text = "I.Danh Sách Liên Kết Cũ";
            // 
            // lbl_moi
            // 
            this.lbl_moi.AutoSize = true;
            this.lbl_moi.Location = new System.Drawing.Point(16, 291);
            this.lbl_moi.Name = "lbl_moi";
            this.lbl_moi.Size = new System.Drawing.Size(132, 13);
            this.lbl_moi.TabIndex = 6;
            this.lbl_moi.Text = "II.Danh Sách Liên Kết Mới";
            // 
            // btn_start
            // 
            this.btn_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_start.ForeColor = System.Drawing.Color.Red;
            this.btn_start.Location = new System.Drawing.Point(837, 10);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(81, 23);
            this.btn_start.TabIndex = 7;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.button2_Click);
            // 
            // txt_lanlap
            // 
            this.txt_lanlap.BackColor = System.Drawing.Color.White;
            this.txt_lanlap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_lanlap.ForeColor = System.Drawing.Color.Red;
            this.txt_lanlap.Location = new System.Drawing.Point(305, 578);
            this.txt_lanlap.Name = "txt_lanlap";
            this.txt_lanlap.ReadOnly = true;
            this.txt_lanlap.Size = new System.Drawing.Size(55, 20);
            this.txt_lanlap.TabIndex = 85;
            this.txt_lanlap.Text = "30";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(240, 581);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(49, 13);
            this.label17.TabIndex = 84;
            this.label17.Text = "Lần Lập:";
            // 
            // txt_timeout
            // 
            this.txt_timeout.BackColor = System.Drawing.Color.White;
            this.txt_timeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_timeout.ForeColor = System.Drawing.Color.Red;
            this.txt_timeout.Location = new System.Drawing.Point(85, 600);
            this.txt_timeout.Name = "txt_timeout";
            this.txt_timeout.ReadOnly = true;
            this.txt_timeout.Size = new System.Drawing.Size(46, 20);
            this.txt_timeout.TabIndex = 78;
            this.txt_timeout.Text = "20000";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 604);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 13);
            this.label11.TabIndex = 76;
            this.label11.Text = "Timeout (ms):";
            // 
            // txtThoiGianCho
            // 
            this.txtThoiGianCho.BackColor = System.Drawing.Color.White;
            this.txtThoiGianCho.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThoiGianCho.ForeColor = System.Drawing.Color.Red;
            this.txtThoiGianCho.Location = new System.Drawing.Point(85, 577);
            this.txtThoiGianCho.Name = "txtThoiGianCho";
            this.txtThoiGianCho.ReadOnly = true;
            this.txtThoiGianCho.Size = new System.Drawing.Size(46, 20);
            this.txtThoiGianCho.TabIndex = 77;
            this.txtThoiGianCho.Text = "10";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 581);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 13);
            this.label12.TabIndex = 75;
            this.label12.Text = "Sleep: (s)";
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.BackColor = System.Drawing.Color.White;
            this.txtSoLuong.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSoLuong.ForeColor = System.Drawing.Color.Red;
            this.txtSoLuong.Location = new System.Drawing.Point(305, 600);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.ReadOnly = true;
            this.txtSoLuong.Size = new System.Drawing.Size(55, 20);
            this.txtSoLuong.TabIndex = 72;
            this.txtSoLuong.Text = "5";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(243, 602);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 71;
            this.label9.Text = "Số Luồng:";
            // 
            // txtDoSau
            // 
            this.txtDoSau.BackColor = System.Drawing.Color.White;
            this.txtDoSau.Enabled = false;
            this.txtDoSau.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDoSau.ForeColor = System.Drawing.Color.Red;
            this.txtDoSau.Location = new System.Drawing.Point(553, 578);
            this.txtDoSau.Name = "txtDoSau";
            this.txtDoSau.ReadOnly = true;
            this.txtDoSau.Size = new System.Drawing.Size(63, 20);
            this.txtDoSau.TabIndex = 68;
            this.txtDoSau.Text = "6";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(501, 581);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 67;
            this.label8.Text = "Độ Sâu:";
            // 
            // btn_scanner_bo
            // 
            this.btn_scanner_bo.Location = new System.Drawing.Point(837, 282);
            this.btn_scanner_bo.Name = "btn_scanner_bo";
            this.btn_scanner_bo.Size = new System.Drawing.Size(81, 23);
            this.btn_scanner_bo.TabIndex = 88;
            this.btn_scanner_bo.Text = "Cũ >> Mới";
            this.btn_scanner_bo.UseVisualStyleBackColor = true;
            this.btn_scanner_bo.Click += new System.EventHandler(this.btn_scanner_bo_Click);
            // 
            // btn_chon_scanner
            // 
            this.btn_chon_scanner.Location = new System.Drawing.Point(829, 578);
            this.btn_chon_scanner.Name = "btn_chon_scanner";
            this.btn_chon_scanner.Size = new System.Drawing.Size(81, 23);
            this.btn_chon_scanner.TabIndex = 87;
            this.btn_chon_scanner.Text = "Mới >> Cũ";
            this.btn_chon_scanner.UseVisualStyleBackColor = true;
            this.btn_chon_scanner.Click += new System.EventHandler(this.btn_chon_scanner_Click);
            // 
            // btn_scanner_xoa_cu
            // 
            this.btn_scanner_xoa_cu.Location = new System.Drawing.Point(746, 282);
            this.btn_scanner_xoa_cu.Name = "btn_scanner_xoa_cu";
            this.btn_scanner_xoa_cu.Size = new System.Drawing.Size(81, 23);
            this.btn_scanner_xoa_cu.TabIndex = 86;
            this.btn_scanner_xoa_cu.Text = "Xóa Cũ";
            this.btn_scanner_xoa_cu.UseVisualStyleBackColor = true;
            this.btn_scanner_xoa_cu.Click += new System.EventHandler(this.btn_scanner_xoa_Click);
            // 
            // lbl_message
            // 
            this.lbl_message.AutoSize = true;
            this.lbl_message.ForeColor = System.Drawing.Color.Blue;
            this.lbl_message.Location = new System.Drawing.Point(11, 645);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(57, 13);
            this.lbl_message.TabIndex = 89;
            this.lbl_message.Text = "Liên kết:...";
            // 
            // btn_scanner_xoa_moi
            // 
            this.btn_scanner_xoa_moi.Location = new System.Drawing.Point(742, 578);
            this.btn_scanner_xoa_moi.Name = "btn_scanner_xoa_moi";
            this.btn_scanner_xoa_moi.Size = new System.Drawing.Size(81, 23);
            this.btn_scanner_xoa_moi.TabIndex = 90;
            this.btn_scanner_xoa_moi.Text = "Xóa Mới";
            this.btn_scanner_xoa_moi.UseVisualStyleBackColor = true;
            this.btn_scanner_xoa_moi.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btn_scanner_dosau_giam
            // 
            this.btn_scanner_dosau_giam.Location = new System.Drawing.Point(622, 576);
            this.btn_scanner_dosau_giam.Name = "btn_scanner_dosau_giam";
            this.btn_scanner_dosau_giam.Size = new System.Drawing.Size(31, 23);
            this.btn_scanner_dosau_giam.TabIndex = 91;
            this.btn_scanner_dosau_giam.Text = "<<";
            this.btn_scanner_dosau_giam.UseVisualStyleBackColor = true;
            this.btn_scanner_dosau_giam.Click += new System.EventHandler(this.btn_scanner_dosau_giam_Click);
            // 
            // btn_scanner_dosau_tang
            // 
            this.btn_scanner_dosau_tang.Location = new System.Drawing.Point(654, 576);
            this.btn_scanner_dosau_tang.Name = "btn_scanner_dosau_tang";
            this.btn_scanner_dosau_tang.Size = new System.Drawing.Size(31, 23);
            this.btn_scanner_dosau_tang.TabIndex = 92;
            this.btn_scanner_dosau_tang.Text = ">>";
            this.btn_scanner_dosau_tang.UseVisualStyleBackColor = true;
            this.btn_scanner_dosau_tang.Click += new System.EventHandler(this.btn_scanner_dosau_tang_Click);
            // 
            // btn_scanner_sleep_giam
            // 
            this.btn_scanner_sleep_giam.Location = new System.Drawing.Point(134, 575);
            this.btn_scanner_sleep_giam.Name = "btn_scanner_sleep_giam";
            this.btn_scanner_sleep_giam.Size = new System.Drawing.Size(31, 23);
            this.btn_scanner_sleep_giam.TabIndex = 94;
            this.btn_scanner_sleep_giam.Text = "<<";
            this.btn_scanner_sleep_giam.UseVisualStyleBackColor = true;
            this.btn_scanner_sleep_giam.Click += new System.EventHandler(this.btn_scanner_sleep_giam_Click);
            // 
            // btn_scanner_sleep_tang
            // 
            this.btn_scanner_sleep_tang.Location = new System.Drawing.Point(166, 575);
            this.btn_scanner_sleep_tang.Name = "btn_scanner_sleep_tang";
            this.btn_scanner_sleep_tang.Size = new System.Drawing.Size(31, 23);
            this.btn_scanner_sleep_tang.TabIndex = 93;
            this.btn_scanner_sleep_tang.Text = ">>";
            this.btn_scanner_sleep_tang.UseVisualStyleBackColor = true;
            this.btn_scanner_sleep_tang.Click += new System.EventHandler(this.btn_scanner_sleep_tang_Click);
            // 
            // btn_scanner_timeout_giam
            // 
            this.btn_scanner_timeout_giam.Location = new System.Drawing.Point(134, 599);
            this.btn_scanner_timeout_giam.Name = "btn_scanner_timeout_giam";
            this.btn_scanner_timeout_giam.Size = new System.Drawing.Size(31, 23);
            this.btn_scanner_timeout_giam.TabIndex = 96;
            this.btn_scanner_timeout_giam.Text = "<<";
            this.btn_scanner_timeout_giam.UseVisualStyleBackColor = true;
            this.btn_scanner_timeout_giam.Click += new System.EventHandler(this.btn_scanner_timeout_giam_Click);
            // 
            // btn_scanner_timeout_tang
            // 
            this.btn_scanner_timeout_tang.Location = new System.Drawing.Point(166, 599);
            this.btn_scanner_timeout_tang.Name = "btn_scanner_timeout_tang";
            this.btn_scanner_timeout_tang.Size = new System.Drawing.Size(31, 23);
            this.btn_scanner_timeout_tang.TabIndex = 95;
            this.btn_scanner_timeout_tang.Text = ">>";
            this.btn_scanner_timeout_tang.UseVisualStyleBackColor = true;
            this.btn_scanner_timeout_tang.Click += new System.EventHandler(this.btn_scanner_timeout_tang_Click);
            // 
            // btn_scanner_soluong_giam
            // 
            this.btn_scanner_soluong_giam.Location = new System.Drawing.Point(364, 600);
            this.btn_scanner_soluong_giam.Name = "btn_scanner_soluong_giam";
            this.btn_scanner_soluong_giam.Size = new System.Drawing.Size(31, 23);
            this.btn_scanner_soluong_giam.TabIndex = 100;
            this.btn_scanner_soluong_giam.Text = "<<";
            this.btn_scanner_soluong_giam.UseVisualStyleBackColor = true;
            this.btn_scanner_soluong_giam.Click += new System.EventHandler(this.btn_scanner_soluong_giam_Click);
            // 
            // btn_scanner_soluong_tang
            // 
            this.btn_scanner_soluong_tang.Location = new System.Drawing.Point(396, 600);
            this.btn_scanner_soluong_tang.Name = "btn_scanner_soluong_tang";
            this.btn_scanner_soluong_tang.Size = new System.Drawing.Size(31, 23);
            this.btn_scanner_soluong_tang.TabIndex = 99;
            this.btn_scanner_soluong_tang.Text = ">>";
            this.btn_scanner_soluong_tang.UseVisualStyleBackColor = true;
            this.btn_scanner_soluong_tang.Click += new System.EventHandler(this.btn_scanner_soluong_tang_Click);
            // 
            // btn_scanner_lanlap_giam
            // 
            this.btn_scanner_lanlap_giam.Location = new System.Drawing.Point(364, 576);
            this.btn_scanner_lanlap_giam.Name = "btn_scanner_lanlap_giam";
            this.btn_scanner_lanlap_giam.Size = new System.Drawing.Size(31, 23);
            this.btn_scanner_lanlap_giam.TabIndex = 98;
            this.btn_scanner_lanlap_giam.Text = "<<";
            this.btn_scanner_lanlap_giam.UseVisualStyleBackColor = true;
            this.btn_scanner_lanlap_giam.Click += new System.EventHandler(this.btn_scanner_lanlap_giam_Click);
            // 
            // btn_scanner_lanlap_tang
            // 
            this.btn_scanner_lanlap_tang.Location = new System.Drawing.Point(396, 576);
            this.btn_scanner_lanlap_tang.Name = "btn_scanner_lanlap_tang";
            this.btn_scanner_lanlap_tang.Size = new System.Drawing.Size(31, 23);
            this.btn_scanner_lanlap_tang.TabIndex = 97;
            this.btn_scanner_lanlap_tang.Text = ">>";
            this.btn_scanner_lanlap_tang.UseVisualStyleBackColor = true;
            this.btn_scanner_lanlap_tang.Click += new System.EventHandler(this.btn_scanner_lanlap_tang_Click);
            // 
            // lbl_scanner_khoa
            // 
            this.lbl_scanner_khoa.AutoSize = true;
            this.lbl_scanner_khoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_scanner_khoa.ForeColor = System.Drawing.Color.Red;
            this.lbl_scanner_khoa.Location = new System.Drawing.Point(124, 630);
            this.lbl_scanner_khoa.Name = "lbl_scanner_khoa";
            this.lbl_scanner_khoa.Size = new System.Drawing.Size(71, 13);
            this.lbl_scanner_khoa.TabIndex = 101;
            this.lbl_scanner_khoa.Text = "Đứt Kết Nối...";
            this.lbl_scanner_khoa.Visible = false;
            // 
            // btn_scanner_giaihan_tang
            // 
            this.btn_scanner_giaihan_tang.Location = new System.Drawing.Point(654, 599);
            this.btn_scanner_giaihan_tang.Name = "btn_scanner_giaihan_tang";
            this.btn_scanner_giaihan_tang.Size = new System.Drawing.Size(31, 23);
            this.btn_scanner_giaihan_tang.TabIndex = 105;
            this.btn_scanner_giaihan_tang.Text = ">>";
            this.btn_scanner_giaihan_tang.UseVisualStyleBackColor = true;
            this.btn_scanner_giaihan_tang.Click += new System.EventHandler(this.btn_scanner_giaihan_tang_Click);
            // 
            // btn_scanner_giaihan_giam
            // 
            this.btn_scanner_giaihan_giam.Location = new System.Drawing.Point(622, 599);
            this.btn_scanner_giaihan_giam.Name = "btn_scanner_giaihan_giam";
            this.btn_scanner_giaihan_giam.Size = new System.Drawing.Size(31, 23);
            this.btn_scanner_giaihan_giam.TabIndex = 104;
            this.btn_scanner_giaihan_giam.Text = "<<";
            this.btn_scanner_giaihan_giam.UseVisualStyleBackColor = true;
            this.btn_scanner_giaihan_giam.Click += new System.EventHandler(this.btn_scanner_giaihan_giam_Click);
            // 
            // txtGioiHan
            // 
            this.txtGioiHan.BackColor = System.Drawing.Color.White;
            this.txtGioiHan.Enabled = false;
            this.txtGioiHan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGioiHan.ForeColor = System.Drawing.Color.Red;
            this.txtGioiHan.Location = new System.Drawing.Point(553, 601);
            this.txtGioiHan.Name = "txtGioiHan";
            this.txtGioiHan.ReadOnly = true;
            this.txtGioiHan.Size = new System.Drawing.Size(63, 20);
            this.txtGioiHan.TabIndex = 103;
            this.txtGioiHan.Text = "2000000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(454, 604);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 102;
            this.label2.Text = "Giới Hạn Liên Kết:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(114, 630);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 13);
            this.label3.TabIndex = 106;
            this.label3.Text = "/";
            // 
            // lbl_handoi
            // 
            this.lbl_handoi.AutoSize = true;
            this.lbl_handoi.ForeColor = System.Drawing.Color.Purple;
            this.lbl_handoi.Location = new System.Drawing.Point(10, 629);
            this.lbl_handoi.Name = "lbl_handoi";
            this.lbl_handoi.Size = new System.Drawing.Size(55, 13);
            this.lbl_handoi.TabIndex = 107;
            this.lbl_handoi.Text = "Hàng Đợi:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(433, 579);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(64, 17);
            this.checkBox1.TabIndex = 108;
            this.checkBox1.Text = "Loop All";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // frmDM_scanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 669);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.lbl_handoi);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_scanner_giaihan_tang);
            this.Controls.Add(this.btn_scanner_giaihan_giam);
            this.Controls.Add(this.txtGioiHan);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_scanner_khoa);
            this.Controls.Add(this.btn_scanner_soluong_giam);
            this.Controls.Add(this.btn_scanner_soluong_tang);
            this.Controls.Add(this.btn_scanner_lanlap_giam);
            this.Controls.Add(this.btn_scanner_lanlap_tang);
            this.Controls.Add(this.btn_scanner_timeout_giam);
            this.Controls.Add(this.btn_scanner_timeout_tang);
            this.Controls.Add(this.btn_scanner_sleep_giam);
            this.Controls.Add(this.btn_scanner_sleep_tang);
            this.Controls.Add(this.btn_scanner_dosau_tang);
            this.Controls.Add(this.btn_scanner_dosau_giam);
            this.Controls.Add(this.btn_scanner_xoa_moi);
            this.Controls.Add(this.lbl_message);
            this.Controls.Add(this.btn_scanner_bo);
            this.Controls.Add(this.btn_scanner_xoa_cu);
            this.Controls.Add(this.btn_chon_scanner);
            this.Controls.Add(this.txt_lanlap);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txt_timeout);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtThoiGianCho);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtSoLuong);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtDoSau);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.lbl_moi);
            this.Controls.Add(this.lbl_cu);
            this.Controls.Add(this.dv_moi);
            this.Controls.Add(this.dv_cu);
            this.Controls.Add(this.btn_tao);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDM_scanner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Phone_Email";
            this.Load += new System.EventHandler(this.frmDM_scanner_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dv_cu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dv_moi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_tao;
        private System.Windows.Forms.DataGridView dv_cu;
        private System.Windows.Forms.DataGridView dv_moi;
        private System.Windows.Forms.Label lbl_cu;
        private System.Windows.Forms.Label lbl_moi;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.TextBox txt_lanlap;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txt_timeout;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtThoiGianCho;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDoSau;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_scanner_bo;
        private System.Windows.Forms.Button btn_chon_scanner;
        private System.Windows.Forms.Button btn_scanner_xoa_cu;
        private System.Windows.Forms.Label lbl_message;
        private System.Windows.Forms.Button btn_scanner_xoa_moi;
        private System.Windows.Forms.Button btn_scanner_dosau_giam;
        private System.Windows.Forms.Button btn_scanner_dosau_tang;
        private System.Windows.Forms.Button btn_scanner_sleep_giam;
        private System.Windows.Forms.Button btn_scanner_sleep_tang;
        private System.Windows.Forms.Button btn_scanner_timeout_giam;
        private System.Windows.Forms.Button btn_scanner_timeout_tang;
        private System.Windows.Forms.Button btn_scanner_soluong_giam;
        private System.Windows.Forms.Button btn_scanner_soluong_tang;
        private System.Windows.Forms.Button btn_scanner_lanlap_giam;
        private System.Windows.Forms.Button btn_scanner_lanlap_tang;
        private System.Windows.Forms.Label lbl_scanner_khoa;
        private System.Windows.Forms.Button btn_scanner_giaihan_tang;
        private System.Windows.Forms.Button btn_scanner_giaihan_giam;
        private System.Windows.Forms.TextBox txtGioiHan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_handoi;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_cu;
        private System.Windows.Forms.DataGridViewTextBoxColumn dosau_cu;
        private System.Windows.Forms.DataGridViewTextBoxColumn name_cu;
        private System.Windows.Forms.DataGridViewTextBoxColumn path_cu;
        private System.Windows.Forms.DataGridViewTextBoxColumn parentid_cu;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderid_cu;
        private System.Windows.Forms.DataGridViewTextBoxColumn statur_cu;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn dosau;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn path;
        private System.Windows.Forms.DataGridViewTextBoxColumn parentid;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderid;
        private System.Windows.Forms.DataGridViewTextBoxColumn statur;
    }
}