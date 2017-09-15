namespace Load
{
    partial class frmDM_trangvang
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
            this.txtid = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gv_trangvang_goc = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.stt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parentid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id_trangvang_goc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name_trangvang_goc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.path_trangvang_goc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gv_trangvang_goc)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // txtid
            // 
            this.txtid.Location = new System.Drawing.Point(44, 13);
            this.txtid.Name = "txtid";
            this.txtid.ReadOnly = true;
            this.txtid.Size = new System.Drawing.Size(51, 20);
            this.txtid.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(185, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(276, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Trang A - Z";
            // 
            // gv_trangvang_goc
            // 
            this.gv_trangvang_goc.AllowUserToAddRows = false;
            this.gv_trangvang_goc.AllowUserToDeleteRows = false;
            this.gv_trangvang_goc.AllowUserToOrderColumns = true;
            this.gv_trangvang_goc.AllowUserToResizeColumns = false;
            this.gv_trangvang_goc.AllowUserToResizeRows = false;
            this.gv_trangvang_goc.BackgroundColor = System.Drawing.Color.White;
            this.gv_trangvang_goc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gv_trangvang_goc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.stt,
            this.parentid,
            this.id_trangvang_goc,
            this.name_trangvang_goc,
            this.path_trangvang_goc});
            this.gv_trangvang_goc.Location = new System.Drawing.Point(16, 69);
            this.gv_trangvang_goc.Name = "gv_trangvang_goc";
            this.gv_trangvang_goc.RowHeadersVisible = false;
            this.gv_trangvang_goc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gv_trangvang_goc.Size = new System.Drawing.Size(445, 415);
            this.gv_trangvang_goc.TabIndex = 28;
            this.gv_trangvang_goc.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.gv_trangvang_goc_DataBindingComplete);
            this.gv_trangvang_goc.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.gv_trangvang_goc_RowPrePaint);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(386, 490);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 29;
            this.button1.Text = "Xóa";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(387, 36);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 29;
            this.button2.Text = "Thêm";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(17, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "......";
            // 
            // stt
            // 
            this.stt.DataPropertyName = "stt";
            this.stt.HeaderText = "STT";
            this.stt.Name = "stt";
            this.stt.Width = 40;
            // 
            // parentid
            // 
            this.parentid.DataPropertyName = "parentid";
            this.parentid.HeaderText = "parentid";
            this.parentid.Name = "parentid";
            this.parentid.Visible = false;
            // 
            // id_trangvang_goc
            // 
            this.id_trangvang_goc.DataPropertyName = "id";
            this.id_trangvang_goc.HeaderText = "ID";
            this.id_trangvang_goc.Name = "id_trangvang_goc";
            this.id_trangvang_goc.Width = 40;
            // 
            // name_trangvang_goc
            // 
            this.name_trangvang_goc.DataPropertyName = "name";
            this.name_trangvang_goc.HeaderText = "Ngành Nghề";
            this.name_trangvang_goc.Name = "name_trangvang_goc";
            this.name_trangvang_goc.ReadOnly = true;
            this.name_trangvang_goc.Width = 200;
            // 
            // path_trangvang_goc
            // 
            this.path_trangvang_goc.DataPropertyName = "path";
            this.path_trangvang_goc.HeaderText = "Link";
            this.path_trangvang_goc.Name = "path_trangvang_goc";
            this.path_trangvang_goc.Width = 150;
            // 
            // frmDM_trangvang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 520);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gv_trangvang_goc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.txtid);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDM_trangvang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "trangvangvietnam.com";
            this.Load += new System.EventHandler(this.frmDM_trangvang_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gv_trangvang_goc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtid;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView gv_trangvang_goc;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn stt;
        private System.Windows.Forms.DataGridViewTextBoxColumn parentid;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_trangvang_goc;
        private System.Windows.Forms.DataGridViewTextBoxColumn name_trangvang_goc;
        private System.Windows.Forms.DataGridViewTextBoxColumn path_trangvang_goc;
    }
}