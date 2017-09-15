using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StorePhone;

namespace Load
{
    public partial class frmDM_hosocongty : Form
    {

        public frmDM_hosocongty()
        {
            InitializeComponent();
        }

        private void frmDM_hosocongty_Load(object sender, EventArgs e)
        {
            BindDMNhom();
            BindDMCap();
            BindDanhmuc();
        }

        #region Bind Danh Mục
        private void BindDMNhom()
        {
            try
            {
                DataTable table = SQLDatabase.ExcDataTable("select * from [dbo].dm_hsct where PATH='' order by id");

                DataTable table_nhom = new DataTable();
                table_nhom.Columns.Add("id", typeof(int));
                table_nhom.Columns.Add("name", typeof(string));

                table_nhom.Rows.Add(-1, "---Chọn Nhóm Gian Hàng---");

                foreach (DataRow item in table.Rows)
                    table_nhom.Rows.Add(item["id"], item["name"]);

                cmb_nhomhs.DataSource = table_nhom;
                cmb_nhomhs.ValueMember = "id";
                cmb_nhomhs.DisplayMember = "name";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BindDMNhom");
            }
        }
        private void BindDanhmuc()
        {
            try
            {
                DataTable tb = new DataTable();
                tb = Utilities_hosocongty.Formatdm_hsct(SQLDatabase.ExcDataTable("[spLoaddm_hsct] null"));
                dataGridView1.DataSource = tb;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BindDanhmuc", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            var rowsCount = dataGridView1.SelectedRows.Count;
            txt_id.Text = "";
            txt_name.Text = "";
            txtMaid.Text = "";
            txt_path.Text = "";
            txt_orderid.Text = "";
            cmd_cap.SelectedValue = 1;
            if (rowsCount == 0 || rowsCount > 1) return;
            var row = dataGridView1.SelectedRows[0];
            if (row == null) return;
            txt_id.Text = row.Cells["id"].Value.ToString();
            txt_name.Text = row.Cells["name"].Value.ToString();
            txtMaid.Text = row.Cells["maid"].Value.ToString();
            txt_path.Text = row.Cells["path"].Value.ToString();
            txt_orderid.Text = row.Cells["orderid"].Value.ToString();
            txtMaid.Text = row.Cells["maid"].Value.ToString();
            cmb_nhomhs.SelectedValue = row.Cells["parentId"].Value.ToString() == "" ? "-1" : row.Cells["parentId"].Value.ToString();
            cmd_cap.SelectedValue = row.Cells["capid"].Value.ToString();
        }

        private void BindDMCap()
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("id", typeof(int));
                table.Columns.Add("name", typeof(string));

                table.Rows.Add("1", "Cấp 1");
                table.Rows.Add("2", "Cấp 2");
                table.Rows.Add("3", "Cấp 3");
                table.Rows.Add("4", "Cấp 4");
                table.Rows.Add("5", "Cấp 5");

                cmd_cap.DataSource = table;
                cmd_cap.ValueMember = "id";
                cmd_cap.DisplayMember = "name";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BindingTelNumberToGridView", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            string strsql = "";
            try {
                strsql = string.Format("select max(orderid) as vitri from dm_hsct");
                DataTable table = SQLDatabase.ExcDataTable(strsql);

                txt_name.Text = "";
                txt_id.Text = "";
                txt_orderid.Text = (ConvertType.ToInt(table.Rows[0]["vitri"]) + 1).ToString();
                txt_path.Text = "";
                txtMaid.Text = "";
                txtMaid.Focus();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "button1_Click");
            }
            
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            int vistrisua=0;
            bool isnew = false;
            if (!radioButton1.Checked)
            {
                if (cmb_nhomhs.SelectedValue.ToString() == "-1")
                {
                    MessageBox.Show("Vui lòng chọn nhóm hồ sơ", "Thông Báo");
                    return;
                }
                if (txtMaid.Text == "") {
                    MessageBox.Show("Vui lòng Mã Ngành", "Thông Báo");
                    txtMaid.Focus();
                    return;
                }
            }

            dm_hsct dm = new dm_hsct();
            dm.id = ConvertType.ToInt(txt_id.Text);
            dm.name = txt_name.Text;
            dm.path = txt_path.Text;
            dm.maid = txtMaid.Text;
            if(cmb_nhomhs.SelectedValue.ToString() !="-1")
                dm.parenid = ConvertType.ToInt(cmb_nhomhs.SelectedValue);

            dm.orderid = ConvertType.ToInt(txt_orderid.Text);
            dm.capid = ConvertType.ToInt(cmd_cap.SelectedValue);
            

            if (dm.id == 0) {
                isnew = true;
                if (ConvertType.ToInt(SQLDatabase.ExcScalar(string.Format("select count(*)  as soluong from dm_hsct  where maid='{0}'", dm.maid))) == 0) {
                    SQLDatabase.Adddm_hsct(dm);
                }
                else {
                    MessageBox.Show("Vui lòng chọn mã khác, Mã đã tồn tại", "Thông Báo");
                    txtMaid.Focus();
                    txtMaid.SelectAll();
                }
            }
            else {
                isnew = false;
                vistrisua = dataGridView1.SelectedRows[0].Index;
                string str = string.Format("select count(*) as soluong from dm_hsct where id<>'{0}' and maid='{1}'", dm.id, dm.maid);
                if (ConvertType.ToInt(str) > 0) {

                    MessageBox.Show("Vui lòng chọn mã khác, Mã đã tồn tại", "Thông Báo");
                    txtMaid.Focus();
                    txtMaid.SelectAll();
                }
                else {
                    SQLDatabase.Updatedm_hsct(dm);
                }

            }
                BindDMNhom();
                BindDanhmuc();
                dataGridView1.ClearSelection();
                if (isnew) {
                    int nRowIndex = dataGridView1.Rows.Count - 1;
                    if (dataGridView1.Rows.Count - 1 >= nRowIndex) {
                        dataGridView1.FirstDisplayedScrollingRowIndex = nRowIndex;
                        dataGridView1.Rows[nRowIndex].Selected = true;
                        dataGridView1.Rows[nRowIndex].Cells[0].Selected = true;
                    }
                }
                else {
                    dataGridView1.Rows[vistrisua].Selected = true; 
                }
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //cmb_nhomhs.Enabled = !radioButton1.Checked;
            txt_path.ReadOnly = radioButton1.Checked;

            if (radioButton1.Checked)
            {
                txt_path.Text = "";
            }
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e) {
            foreach (DataGridViewRow row in this.dataGridView1.Rows) {
                if ((row.Cells["path"].Value == null || row.Cells["path"].Value.ToString() == "") &&
                    (row.Cells["capid"].Value.ToString() =="1"))
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
                }


                if ((row.Cells["path"].Value == null || row.Cells["path"].Value.ToString() == "") &&
                    (row.Cells["capid"].Value.ToString() == "2")) {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
                }
            }  
        }

        private void button3_Click(object sender, EventArgs e) {
            try {
                string id = dataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
                if (MessageBox.Show("Bạn có muốn xóa thông tin đang chọn không?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                              MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes) {
                    //TODO: Stuff

                    int kq = ConvertType.ToInt(SQLDatabase.ExcScalar(string.Format("[spKiemTraXoa_vatgia] '{0}'", id)));

                    if (kq == 1) {
                        MessageBox.Show("Đang có mặt ngành còn tội tại trong nhóm ngành, Không thể xóa được \n Vui lòng xóa mặt hàng trong nhóm hàng trước", "Thông Báo");
                        return;
                    }
                    else if (kq == 2) {
                        MessageBox.Show("Ngành có tồn tại ở bảng hồ sơ công ty, Không thể xóa được", "Thông Báo");
                        return;
                    }

                    if (SQLDatabase.DelVatGia(new dm_vatgia() { id = ConvertType.ToInt(id) })) {
                        BindDanhmuc();
                        BindDMNhom();
                        MessageBox.Show(string.Format("Xóa thành công!"), "Thông báo");
                    }
                    else {
                        MessageBox.Show(string.Format("Xóa không thành công!"), "Thông báo");
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "button3_Click");
            }
        }
    }
}
