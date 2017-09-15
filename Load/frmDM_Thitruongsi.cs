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
    public partial class frmDM_Thitruongsi : Form
    {
        public frmDM_Thitruongsi()
        {
            InitializeComponent();
            BindDM_NhomVatGia();
            BindDMSanPham();
        }

        private void BindDM_NhomVatGia()
        {
            try
            {
                DataTable table = SQLDatabase.ExcDataTable("select * from [dbo].[dm_thitruongsi] where [parentId] is null order by id");

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
                MessageBox.Show(ex.Message, "BindDM_NhomVatGia", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindDMSanPham()
        {
            try
            {
                DataTable table = SQLDatabase.ExcDataTable(string.Format("[spLoaddm_thitruongsi] null"));
                DataTable tb_new = table.Clone();
                tb_new.Rows.Add(-1, "---Chọn Nhóm Gian Hàng---");
                tb_new.Clear();
                foreach (DataRow item in table.Select("alevel=0"))
                {
                    tb_new.ImportRow(item);
                    foreach (DataRow item2 in table.Select(string.Format("parentid='{0}'", item["id"])))
                    {
                        tb_new.ImportRow(item2);
                    }
                }
                dataGridView1.DataSource = tb_new;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BindDMSanPham", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strsql = "";
            if (radioButton1.Checked)
                strsql = "select max(orderid) as vitri from dm_thitruongsi";
            else
            {
                if (cmb_nhomhs.SelectedValue.ToString() != "-1")
                    strsql = string.Format("select isnull(max(orderid),0) as vitri from dm_thitruongsi where parentId={0}", cmb_nhomhs.SelectedValue);
                else
                    strsql = "select 0 as vitri";
            }
            DataTable table = SQLDatabase.ExcDataTable(strsql);

            txt_name.Text = "";
            txt_id.Text = "";
            txt_orderid.Text = (ConvertType.ToInt(table.Rows[0]["vitri"]) + 1).ToString();
            txt_path.Text = "";
            //comboBox1.SelectedValue = "-1";
            txt_name.Focus();
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

                    int kq = ConvertType.ToInt(SQLDatabase.ExcScalar(string.Format("[spKiemTraXoa_thitruongsi] '{0}'", id)));

                    if (kq == 1)
                    {
                        MessageBox.Show("Đang có mặt hàng còn tội tại trong nhóm hàng, Không thể xóa được \n Vui lòng xóa mặt hàng trong nhóm hàng trước", "Thông Báo");
                        return;
                    }
                    else if (kq == 2)
                    {
                        MessageBox.Show("Mặt hàng có tồn tại ở bảng vật giá, Không thể xóa được", "Thông Báo");
                        return;
                    }

                    if (SQLDatabase.Deldm_thitruongsi(new dm_thitruongsi() { id = ConvertType.ToInt(id) }))
                    {
                        BindDM_NhomVatGia();
                        BindDMSanPham();
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
            bool isnew = true;
            int vistrisua = 0;
            if (!radioButton1.Checked)
            {
                if (cmb_nhomhs.SelectedValue == null || cmb_nhomhs.SelectedValue.ToString() == "-1")
                {
                    MessageBox.Show("Vui lòng chọn nhóm gian hàng", "Thông Báo");
                    return;
                }
            }

            dm_thitruongsi dm = new dm_thitruongsi();
            dm.id = ConvertType.ToInt(txt_id.Text);
            dm.name = txt_name.Text.Trim();
            dm.path = txt_path.Text;
            dm.paren_id = radioButton1.Checked ? (int?)null : ConvertType.ToInt(cmb_nhomhs.SelectedValue);
            dm.orderid = ConvertType.ToInt(txt_orderid.Text);
            if (dm.id == 0)
            {
                isnew = true;
                SQLDatabase.AdddmThiTruongSi(dm);
            }
            else
            {
                isnew = false;
                vistrisua = dataGridView1.SelectedRows[0].Index;
                SQLDatabase.Updatedm_thitruongsi(dm);
            }

            if (radioButton1.Checked)
                BindDM_NhomVatGia();

            BindDMSanPham();

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

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            try
            {
                var rowsCount = dataGridView1.SelectedRows.Count;
                if (rowsCount == 0)
                    return;

                var row = dataGridView1.SelectedRows[0];
                if (row == null) return;

                txt_id.Text = row.Cells["id"].Value.ToString();
                txt_name.Text = row.Cells["name"].Value.ToString();
                txt_path.Text = row.Cells["path"].Value.ToString();
                txt_orderid.Text = row.Cells["orderid"].Value.ToString();
                cmb_nhomhs.SelectedValue = row.Cells["parentId"].Value.ToString() == "" ? "-1" : row.Cells["parentId"].Value.ToString();
                if (row.Cells["path"].Value.ToString() == "")
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton2.Checked = true;
                    radioButton1.Checked = false;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "dataGridView1_RowStateChanged");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            cmb_nhomhs.Enabled = !radioButton1.Checked;
            txt_path.ReadOnly = radioButton1.Checked;

            if (radioButton1.Checked)
            {
                BindDM_NhomVatGia();
                txt_path.Text = "";
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (row.Cells["alevel"].Value.ToString() == "0")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 8);
                }
            }
        }
    }
}
