namespace Load {
    partial class frmSaveAs {
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.id_chon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_id_chon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ma_chon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cap_id_chon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name_chon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.path_chon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderid_chon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scannerby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parentid_chon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_chon,
            this.id,
            this.group_id_chon,
            this.ma_chon,
            this.cap_id_chon,
            this.name_chon,
            this.path_chon,
            this.orderid_chon,
            this.scannerby,
            this.parentid_chon,
            this.createdate});
            this.dataGridView1.Location = new System.Drawing.Point(3, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(644, 386);
            this.dataGridView1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(572, 404);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Chọn";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // id_chon
            // 
            this.id_chon.DataPropertyName = "id_chon";
            this.id_chon.HeaderText = "ID";
            this.id_chon.Name = "id_chon";
            this.id_chon.ReadOnly = true;
            this.id_chon.Width = 50;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // group_id_chon
            // 
            this.group_id_chon.DataPropertyName = "group_id_chon";
            this.group_id_chon.HeaderText = "group_id_chon";
            this.group_id_chon.Name = "group_id_chon";
            this.group_id_chon.ReadOnly = true;
            this.group_id_chon.Visible = false;
            // 
            // ma_chon
            // 
            this.ma_chon.DataPropertyName = "ma_chon";
            this.ma_chon.HeaderText = "Mã";
            this.ma_chon.Name = "ma_chon";
            this.ma_chon.ReadOnly = true;
            this.ma_chon.Width = 70;
            // 
            // cap_id_chon
            // 
            this.cap_id_chon.DataPropertyName = "cap_id_chon";
            this.cap_id_chon.HeaderText = "cap_id_chon";
            this.cap_id_chon.Name = "cap_id_chon";
            this.cap_id_chon.ReadOnly = true;
            this.cap_id_chon.Visible = false;
            // 
            // name_chon
            // 
            this.name_chon.DataPropertyName = "name_chon";
            this.name_chon.HeaderText = "Tên";
            this.name_chon.Name = "name_chon";
            this.name_chon.ReadOnly = true;
            this.name_chon.Width = 200;
            // 
            // path_chon
            // 
            this.path_chon.DataPropertyName = "path_chon";
            this.path_chon.HeaderText = "path_chon";
            this.path_chon.Name = "path_chon";
            this.path_chon.ReadOnly = true;
            this.path_chon.Width = 300;
            // 
            // orderid_chon
            // 
            this.orderid_chon.DataPropertyName = "orderid_chon";
            this.orderid_chon.HeaderText = "orderid_chon";
            this.orderid_chon.Name = "orderid_chon";
            this.orderid_chon.ReadOnly = true;
            this.orderid_chon.Visible = false;
            // 
            // scannerby
            // 
            this.scannerby.DataPropertyName = "scannerby";
            this.scannerby.HeaderText = "scannerby";
            this.scannerby.Name = "scannerby";
            this.scannerby.ReadOnly = true;
            this.scannerby.Visible = false;
            // 
            // parentid_chon
            // 
            this.parentid_chon.DataPropertyName = "parentid_chon";
            this.parentid_chon.HeaderText = "parentid_chon";
            this.parentid_chon.Name = "parentid_chon";
            this.parentid_chon.ReadOnly = true;
            this.parentid_chon.Visible = false;
            // 
            // createdate
            // 
            this.createdate.DataPropertyName = "createdate";
            this.createdate.HeaderText = "createdate";
            this.createdate.Name = "createdate";
            this.createdate.ReadOnly = true;
            this.createdate.Visible = false;
            // 
            // frmSaveAs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 430);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSaveAs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Save As";
            this.Load += new System.EventHandler(this.frmSaveAs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_chon;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn group_id_chon;
        private System.Windows.Forms.DataGridViewTextBoxColumn ma_chon;
        private System.Windows.Forms.DataGridViewTextBoxColumn cap_id_chon;
        private System.Windows.Forms.DataGridViewTextBoxColumn name_chon;
        private System.Windows.Forms.DataGridViewTextBoxColumn path_chon;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderid_chon;
        private System.Windows.Forms.DataGridViewTextBoxColumn scannerby;
        private System.Windows.Forms.DataGridViewTextBoxColumn parentid_chon;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdate;
    }
}