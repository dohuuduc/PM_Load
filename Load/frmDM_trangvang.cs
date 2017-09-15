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
    public partial class frmDM_trangvang : Form
    {
        private DataTable _table;
        public frmDM_trangvang()
        {
            InitializeComponent();
            _table = CreateTable();
            BindDmNhom();
            gv_trangvang_goc.DataSource = _table;
        }

        private void frmDM_trangvang_Load(object sender, EventArgs e)
        {
            
           
           
        }
        public DataTable CreateTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("stt", typeof(int));
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("path", typeof(string));
            table.Columns.Add("parentid", typeof(int));

            

            return table;
        }
        public void BindDmNhom()
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("select * from dm_trangvang where parentid is null"));
            DataTable tb_new = table.Clone();
            tb_new.Rows.Add(-1, "---Chọn---");
            tb_new.Clear();
            foreach (DataRow item in table.Rows)
                tb_new.ImportRow(item);
           
            comboBox1.DataSource = tb_new;
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "name";
            comboBox1.SelectedValue = -1;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PleaseWait objPleaseWait = new PleaseWait();
            try
            {
                _table.Clear();
                int id = ConvertType.ToInt(comboBox1.SelectedValue);
                txtid.Text = id.ToString();
                if (id == 0) return;

                objPleaseWait.Show();
                objPleaseWait.Update();

                string strPath = SQLDatabase.ExcDataTable(string.Format("select * from dm_trangvang where id={0}", id)).Rows[0]["path"].ToString();
                int max = Utilities_trangvang.getPageMax(strPath, null);
                int i = 0;
                Dictionary<string, string> dm = new Dictionary<string, string>();
                do
                {
                    string strPath1 = "";
                    i = i + 1;
                    strPath1 = string.Format(strPath + "?page={0}", i);
                    Utilities_trangvang.getDanhMuc(strPath1, ref dm);
                    
                } while (i <= max);

                DataTable tb = SQLDatabase.ExcDataTable(string.Format("select * from dm_trangvang where parentid='{0}'", id));
                foreach (DataRow item in tb.Rows)
                {
                    _table.Rows.Add(_table.Rows.Count+1,item["id"], item["name"], item["path"],id);
                }
                foreach (KeyValuePair<string, string> entry in dm)
                {
                    if (_table != null) { 
                        var k = (from r in _table.Rows.OfType<DataRow>()  
                                 where r["path"].ToString() == entry.Key select r).FirstOrDefault();
                        if (k == null) {
                            _table.Rows.Add(_table.Rows.Count+1,null, entry.Value, entry.Key, id);
                        }
                    }   
                }
                objPleaseWait.Close();
            }
            catch (Exception ex)
            {
                objPleaseWait.Close();
                MessageBox.Show(ex.Message, "cmd_trangvang_SelectedIndexChanged");
            }
        }
        private void gv_trangvang_goc_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            label4.Text = string.Format("Mới: {0}     -   Cũ: {1}  /  Tổng:  {2}", _table.Select("id is null").Count(), _table.Select("id is not null").Count(), gv_trangvang_goc.Rows.Count);
        }

        private void button2_Click(object sender, EventArgs e) {
            PleaseWait objPleaseWait = new PleaseWait();
            try {
                
                objPleaseWait.Show();
                objPleaseWait.Update();

                foreach (DataRow row in _table.Rows) {
                    dm_trangvang dm = new dm_trangvang();
                    dm.name = row["name"].ToString();
                    dm.path = row["path"].ToString();
                    dm.paren_id = ConvertType.ToInt( row["parentid"].ToString());

                    if (ConvertType.ToInt(SQLDatabase.ExcScalar(string.Format("select count(*) from dm_trangvang where path='{0}'", dm.path)))>0) {
                        SQLDatabase.UpdatedmTrangVang(dm);
                    }
                    else {
                        SQLDatabase.AdddmTrangVang(dm);
                        row["id"] = dm.id;
                    }

                }
                objPleaseWait.Close();
            }
            catch (Exception ex) {
                if(objPleaseWait!=null)
                    objPleaseWait.Close();

                MessageBox.Show(ex.Message,"button2_Click");
            }
        }

        private void gv_trangvang_goc_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
            if (gv_trangvang_goc.Rows[e.RowIndex].Cells["id_trangvang_goc"].Value.ToString()=="") 
            {
                    gv_trangvang_goc.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Beige;
                    
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            try {
                string id = gv_trangvang_goc.SelectedRows[0].Cells["id"].Value.ToString();
                if (MessageBox.Show("Bạn có muốn xóa thông tin đang chọn không?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                              MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes) {
                    //TODO: Stuff

                                  int kq = ConvertType.ToInt(SQLDatabase.ExcScalar(string.Format("[spKiemTraXoa_trangvang] '{0}'", id)));

                    if (kq == 1) {
                        MessageBox.Show("Đang có mặt ngành còn tội tại trong nhóm ngành, Không thể xóa được \n Vui lòng xóa mặt hàng trong nhóm hàng trước", "Thông Báo");
                        return;
                    }
                    else if (kq == 2) {
                        MessageBox.Show("Ngành có tồn tại ở bảng hồ sơ công ty, Không thể xóa được", "Thông Báo");
                        return;
                    }

                    if (SQLDatabase.DelVatGia(new dm_vatgia() { id = ConvertType.ToInt(id) })) {
                        comboBox1_SelectedIndexChanged(null,null);
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
