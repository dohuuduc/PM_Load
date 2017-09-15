namespace Load
{
    partial class frmDM_Muaban_timviec
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
            this.gv_muaban_timviec = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cmd_muaban_tv_danhmuc = new System.Windows.Forms.ComboBox();
            this.label38 = new System.Windows.Forms.Label();
            this.txtid = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gv_muaban_timviec)).BeginInit();
            this.SuspendLayout();
            // 
            // gv_muaban_timviec
            // 
            this.gv_muaban_timviec.AllowUserToAddRows = false;
            this.gv_muaban_timviec.AllowUserToDeleteRows = false;
            this.gv_muaban_timviec.AllowUserToResizeColumns = false;
            this.gv_muaban_timviec.AllowUserToResizeRows = false;
            this.gv_muaban_timviec.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.gv_muaban_timviec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gv_muaban_timviec.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.stt,
            this.name,
            this.Path});
            this.gv_muaban_timviec.Location = new System.Drawing.Point(12, 48);
            this.gv_muaban_timviec.MultiSelect = false;
            this.gv_muaban_timviec.Name = "gv_muaban_timviec";
            this.gv_muaban_timviec.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gv_muaban_timviec.RowHeadersVisible = false;
            this.gv_muaban_timviec.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gv_muaban_timviec.Size = new System.Drawing.Size(542, 290);
            this.gv_muaban_timviec.TabIndex = 10;
            this.gv_muaban_timviec.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.gv_muaban_timviec_DataBindingComplete);
            this.gv_muaban_timviec.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint);
            this.gv_muaban_timviec.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.gv_muaban_timviec_RowStateChanged);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.Visible = false;
            this.id.Width = 30;
            // 
            // stt
            // 
            this.stt.DataPropertyName = "stt";
            this.stt.HeaderText = "STT";
            this.stt.Name = "stt";
            this.stt.Width = 31;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "Tên Danh Mục";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 160;
            // 
            // Path
            // 
            this.Path.DataPropertyName = "path";
            this.Path.HeaderText = "Link";
            this.Path.Name = "Path";
            this.Path.ReadOnly = true;
            this.Path.Width = 340;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(479, 344);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 30;
            this.button2.Text = "Thêm";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(7, 349);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "......";
            // 
            // cmd_muaban_tv_danhmuc
            // 
            this.cmd_muaban_tv_danhmuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmd_muaban_tv_danhmuc.FormattingEnabled = true;
            this.cmd_muaban_tv_danhmuc.Location = new System.Drawing.Point(335, 10);
            this.cmd_muaban_tv_danhmuc.Name = "cmd_muaban_tv_danhmuc";
            this.cmd_muaban_tv_danhmuc.Size = new System.Drawing.Size(219, 21);
            this.cmd_muaban_tv_danhmuc.TabIndex = 33;
            this.cmd_muaban_tv_danhmuc.SelectedIndexChanged += new System.EventHandler(this.cmd_muaban_tv_danhmuc_SelectedIndexChanged);
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(264, 13);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(53, 13);
            this.label38.TabIndex = 152;
            this.label38.Text = "Mua Bán:";
            // 
            // txtid
            // 
            this.txtid.Location = new System.Drawing.Point(38, 7);
            this.txtid.Name = "txtid";
            this.txtid.Size = new System.Drawing.Size(69, 20);
            this.txtid.TabIndex = 153;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 154;
            this.label1.Text = "ID:";
            // 
            // frmDM_Muaban_timviec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 379);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtid);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.cmd_muaban_tv_danhmuc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.gv_muaban_timviec);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDM_Muaban_timviec";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "muaban.net";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDM_Muaban_timviec_FormClosing);
            this.Load += new System.EventHandler(this.frmDM_Muaban_timviec_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gv_muaban_timviec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gv_muaban_timviec;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn stt;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Path;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmd_muaban_tv_danhmuc;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox txtid;
        private System.Windows.Forms.Label label1;
    }
}