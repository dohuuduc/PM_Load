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

namespace Load {
    public partial class frmDM_scanner2 : Form {
        public frmDM_scanner2() {
            InitializeComponent();
        }

        private void frmDM_scanner_Load(object sender, EventArgs e) {
            BindDmscanner();
           
        }
        public void BindDmscanner() {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("select id, name, lienket, domain, orderid from [dbo].[dm_scanner]"));
            dataGridView1.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e) {
            bool isnew = false;
            int vistrisua = 0;
            try {
                if (textBox1.Text == "") {
                    MessageBox.Show("Vui lòng nhập tên.", "Thông Báo");
                    return;
                }
                if (textBox5.Text == "") {
                    MessageBox.Show("Vui lòng nhập Domain.", "Thông Báo");
                    return;
                }
                dm_scanner dm = new dm_scanner();
                dm.id = ConvertType.ToInt(textBox3.Text);
                dm.name = textBox1.Text;
                dm.lienket = textBox4.Text;
                dm.domain = textBox5.Text;
                dm.orderid = ConvertType.ToInt(textBox2.Text);
                if (dm.id == 0) {
                    isnew = true;
                    if (ConvertType.ToInt(SQLDatabase.ExcScalar(string.Format("select count(*)  as soluong from dm_scanner  where name='{0}'", dm.name))) == 0) {
                        SQLDatabase.Adddm_scanner(dm);
                        
                        dm_scanner_ct ct = new dm_scanner_ct();
                        ct.dosau = 0;
                        ct.name = dm.name;
                        ct.path = dm.lienket;
                        ct.statur = false;
                        ct.parentid = dm.id;
                        

                        SQLDatabase.Add_dm_scanner_ct(ct);
                    }
                    else {
                        MessageBox.Show("Vui lòng chọn tên khác, Tên đã tồn tại", "Thông Báo");
                        textBox1.Focus();
                        textBox1.SelectAll();
                    }
                }
                else {
                    vistrisua = dataGridView1.SelectedRows[0].Index;
                    string str = string.Format("select count(*) as soluong from dm_scanner where id<>'{0}' and name='{1}'", dm.id, dm.name);
                    DataTable tb = SQLDatabase.ExcDataTable(str);
                    if (ConvertType.ToInt(tb.Rows[0][0]) > 0) {
                        MessageBox.Show("Vui lòng chọn tên khác, Tên đã tồn tại", "Thông Báo");
                        textBox1.Focus();
                        textBox1.SelectAll();
                    }
                    else {
                        SQLDatabase.Updatedm_scanner(dm);
                        string str1 = string.Format("select count(*) from dm_scanner where parentid='{0}' and path='{1}'", dm.id, dm.lienket);
                        DataTable tb1 = SQLDatabase.ExcDataTable(str1);
                        if (ConvertType.ToInt(tb.Rows[0][0]) == 0) {
                            dm_scanner_ct ct = new dm_scanner_ct();
                            //ct.domain = dm.domain;
                            ct.dosau = 0;
                            ct.name = dm.name;
                            ct.path = dm.lienket;
                            ct.statur = true;
                            ct.parentid = dm.id;
                            SQLDatabase.Add_dm_scanner_ct(ct);
                        }
                    }
                }
                BindDmscanner();
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
            catch (Exception ex) {
                MessageBox.Show(ex.Message,"button1_Click");
            }
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e) {
            var rowsCount = dataGridView1.SelectedRows.Count;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
           
            if (rowsCount == 0 || rowsCount > 1)
                return;
            var row = dataGridView1.SelectedRows[0];
            if (row == null)
                return;
            textBox3.Text = row.Cells["id"].Value.ToString();
            textBox1.Text = row.Cells["name"].Value.ToString();
            textBox4.Text = row.Cells["lienket"].Value.ToString();
            textBox2.Text = row.Cells["orderid"].Value.ToString();
            textBox5.Text = row.Cells["domain"].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e) {
            string strsql = "";
            try {
                strsql = string.Format("select max(orderid) as vitri from dm_scanner");
                DataTable table = SQLDatabase.ExcDataTable(strsql);

                textBox1.Text = "";
                textBox3.Text = "";
                textBox2.Text = (ConvertType.ToInt(table.Rows[0]["vitri"]) + 1).ToString();
                textBox4.Text = "";
                textBox1.Focus();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            try {
                string id = dataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
                if (MessageBox.Show("Bạn có muốn xóa thông tin đang chọn không?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                              MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes) {
                                  int kq = ConvertType.ToInt(SQLDatabase.ExcScalar(string.Format("[spDel_scanner] '{0}'", id)));
                        BindDmscanner();
                        MessageBox.Show(string.Format("Xóa thành công!"), "Thông báo");
                    }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "button3_Click");
            }
        
        }

        

        private void textBox4_Leave(object sender, EventArgs e) {
            try {
                if (textBox4.Text == "")
                    return;
                textBox5.Text = Helpers.getDomain(textBox4.Text);
            }
            catch (Exception) {

                throw;
            }
        }

        private void frmDM_scanner2_FormClosed(object sender, FormClosedEventArgs e) {
            this.DialogResult = DialogResult.OK;
        }
    }
}
