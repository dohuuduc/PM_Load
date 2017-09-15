using StorePhone;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Load
{
    public partial class frmDM_batdongsan : Form
    {
        public frmDM_batdongsan()
        {
            InitializeComponent();
            BindDMNhom();
            BindDanhmuc();
        }

        private void BindDMNhom()
        {
            try
            {
                DataTable table = StorePhone.SQLDatabase.ExcDataTable("select * from [dbo].dm_batdongsan where path='' order by id");

                DataTable table_nhom = new DataTable();
                table_nhom.Columns.Add("id", typeof(int));
                table_nhom.Columns.Add("name", typeof(string));

                table_nhom.Rows.Add(-1, "---Chọn Nhóm---");

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

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (row.Cells["path"].Value == null || row.Cells["path"].Value.ToString() == "")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
                }


                if (row.Cells["path"].Value == null || row.Cells["path"].Value.ToString() == "")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
                }
            }
        }
        private void BindDanhmuc()
        {
            try
            {
                DataTable tb = new DataTable();
                tb = Utilities_batdongsan.Formatdm_batdongsan(StorePhone.SQLDatabase.ExcDataTable("[spLoaddm_batdongsan] null"));
                dataGridView1.DataSource = tb;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BindDanhmuc", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void frmDM_batdongsan_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strsql = "";
            try
            {
                strsql = string.Format("select max(orderid) as vitri from dm_batdongsan");
                DataTable table = StorePhone.SQLDatabase.ExcDataTable(strsql);

                txt_name.Text = "";
                txt_id.Text = "";
                txt_orderid.Text = (ConvertType.ToInt(table.Rows[0]["vitri"]) + 1).ToString();
                txt_path.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button1_Click");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string id = dataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
                if (MessageBox.Show("Bạn có muốn xóa thông tin đang chọn không?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                              MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    //TODO: Stuff

                    int kq = ConvertType.ToInt(StorePhone.SQLDatabase.ExcScalar(string.Format("[spKiemTraXoa_batdongsan] '{0}'", id)));

                    if (kq == 1)
                    {
                        MessageBox.Show("Đang có mặt ngành còn tội tại trong nhóm ngành, Không thể xóa được \n Vui lòng xóa mặt hàng trong nhóm hàng trước", "Thông Báo");
                        return;
                    }
                    else if (kq == 2)
                    {
                        MessageBox.Show("Ngành có tồn tại ở bảng hồ sơ công ty, Không thể xóa được", "Thông Báo");
                        return;
                    }

                    if (StorePhone.SQLDatabase.DelDM_batdongsan(new dm_batdongsan() { id = ConvertType.ToInt(id) }))
                    {
                        BindDanhmuc();
                        BindDMNhom();
                        MessageBox.Show(string.Format("Xóa thành công!"), "Thông báo");
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Xóa không thành công!"), "Thông báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button3_Click");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int vistrisua = 0;
            bool isnew = false;
            if (!radioButton1.Checked)
            {
                if (cmb_nhomhs.SelectedValue.ToString() == "-1")
                {
                    MessageBox.Show("Vui lòng chọn nhóm hồ sơ", "Thông Báo");
                    return;
                }
            }

            dm_batdongsan dm = new dm_batdongsan();
            dm.id = ConvertType.ToInt(txt_id.Text);
            dm.name = txt_name.Text;
            dm.path = txt_path.Text;
            if (cmb_nhomhs.SelectedValue.ToString() != "-1")
                dm.parenid = ConvertType.ToInt(cmb_nhomhs.SelectedValue);

            dm.orderid = ConvertType.ToInt(txt_orderid.Text);

            if (dm.id == 0)
            {
                isnew = true;

                if (!radioButton1.Checked && ConvertType.ToInt(SQLDatabase.ExcScalar(string.Format("select count(*)  as soluong from dm_batdongsan  where path='{0}'", dm.path))) > 0)
                {
                    MessageBox.Show("Vui lòng chọn link khác, Link đã tồn tại", "Thông Báo");
                    txt_path.Focus();
                    txt_path.SelectAll();
                }
                else
                    SQLDatabase.Adddm_batdongsang(dm);

            }
            else
            {
                isnew = false;
                vistrisua = dataGridView1.SelectedRows[0].Index;
                if (radioButton2.Checked)
                {
                    string str = string.Format("select count(*) as soluong from dm_batdongsan where id<>'{0}' and path='{1}'", dm.id, dm.path);
                    if (ConvertType.ToInt(SQLDatabase.ExcScalar(str)) > 0)
                    {
                        MessageBox.Show("Vui lòng chọn mã khác, Mã đã tồn tại", "Thông Báo");
                        txt_path.Focus();
                        txt_path.SelectAll();
                    }
                }
                else
                    SQLDatabase.Updatedm_batdongsan(dm);
            }

            BindDMNhom();
            BindDanhmuc();
            dataGridView1.ClearSelection();
            if (isnew)
            {
                int nRowIndex = dataGridView1.Rows.Count - 1;
                if (dataGridView1.Rows.Count - 1 >= nRowIndex)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = nRowIndex;
                    dataGridView1.Rows[nRowIndex].Selected = true;
                    dataGridView1.Rows[nRowIndex].Cells[0].Selected = true;
                }
            }
            else
            {
                dataGridView1.Rows[vistrisua].Selected = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            txt_path.ReadOnly = radioButton1.Checked;
            if (radioButton1.Checked)
            {
                txt_path.Text = "";
            }
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            var rowsCount = dataGridView1.SelectedRows.Count;
            txt_id.Text = "";
            txt_name.Text = "";
            txt_path.Text = "";
            txt_orderid.Text = "";
            if (rowsCount == 0 || rowsCount > 1) return;
            var row = dataGridView1.SelectedRows[0];
            if (row == null) return;
            txt_id.Text = row.Cells["id"].Value.ToString();
            txt_name.Text = row.Cells["name"].Value.ToString();
            txt_path.Text = row.Cells["path"].Value.ToString();
            txt_orderid.Text = row.Cells["orderid"].Value.ToString();
            cmb_nhomhs.SelectedValue = row.Cells["parentId"].Value.ToString() == "" ? "-1" : row.Cells["parentId"].Value.ToString();
        }
    }
}
