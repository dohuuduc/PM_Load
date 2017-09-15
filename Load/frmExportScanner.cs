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
    public partial class frmExportScanner : Form {

        private DataTable _tableDauSo;
        private string strdatabasename;
        public string strDatabaseName {
            get { return strdatabasename; }
            set { strdatabasename = value; }
        }

        public frmExportScanner() {
            InitializeComponent();
            BindDmNhaMang();
            CheckAutoEmail();
        }

        private void frmExportScanner_Load(object sender, EventArgs e) {
            _tableDauSo = GetTable();
            checkedListBox2.DataSource = _tableDauSo;
            checkedListBox2.ValueMember = "id";
            checkedListBox2.DisplayMember = "dauso";
        }
      

        public void BindDmNhaMang() {
            DataTable table = StorePhone.SQLDatabase.ExcDataTable("select * from dau_so where parentid is null order by nhamang");

            checkedListBox1.DataSource = table;
            checkedListBox1.ValueMember = "id";
            checkedListBox1.DisplayMember = "nhamang";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            CheckAutoNhaMang();
        }

        public void CheckAutoNhaMang() {
            for (int i = 0; i < checkedListBox1.Items.Count; i++) {
                checkedListBox1.SetItemChecked(i, checkBox2.Checked);
            }
        }


        private DataTable GetTable() {
            DataTable table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("dauso", typeof(string));
            return table;
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e) {
            this.BeginInvoke((MethodInvoker) (
                () => Console.WriteLine(checkedListBox1.SelectedItems.Count)));
        }

        private void checkedListBox1_MouseUp(object sender, MouseEventArgs e) {
            try {
                string dayso = "";
                if (checkedListBox1.CheckedItems.Count == 0) {
                    _tableDauSo.Clear();
                    return;
                }
                foreach (object itemChecked in checkedListBox1.CheckedItems) {
                    DataRowView castedItem = itemChecked as DataRowView;
                    int id_nhamang = ConvertType.ToInt(castedItem["id"]);
                    dayso += string.Format("{0},", id_nhamang);
                }
                if (dayso.Length > 0)
                    dayso= dayso.Substring(0, dayso.Length - 1);

                string strDk = string.Format("parentId in({0})", dayso);
                ///*====================================================*/
                _tableDauSo.Clear();
                DataTable tb = SQLDatabase.ExcDataTable(string.Format("select id,dauso from dau_so where parentId in({0})", dayso));
                foreach (DataRow item in tb.Rows) {
                    _tableDauSo.Rows.Add(item["id"], item["dauso"]);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "checkedListBox1_MouseUp");
            }
        }
        public void CheckAutoDauSo() {
            for (int i = 0; i < checkedListBox2.Items.Count; i++) {
                checkedListBox2.SetItemChecked(i, checkBox2.Checked);
            }
        }
        public void CheckAutoEmail() {
            for (int i = 0; i < checkedListBox4.Items.Count; i++) {
                checkedListBox4.SetItemChecked(i, checkBox3.Checked);
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            CheckAutoDauSo();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) {
            CheckAutoEmail();
        }

        private void button2_Click(object sender, EventArgs e) {
            PleaseWait objPleaseWait = null;
            try {
                string strDk = "";
                string strSQL = string.Format(" select  a.id,a.email,b.name,b.path  from [{0}].[dbo].[scanner_email]  a inner join  [{0}].[dbo].[dm_scanner_ct] b on a.dm_scanner_ct_id= b.id where ", strdatabasename);

                if (checkedListBox4.CheckedItems.Count == 0) {
                    MessageBox.Show("Vui lòng chọn đuôi email xuất file", "Thông Báo");
                    return;
                }

                string strTieuDe = checkedListBox4.CheckedItems[0].ToString();

                foreach (object itemChecked in checkedListBox4.CheckedItems) {
                    if (itemChecked.ToString() != "Khác") {
                        strDk += string.Format(" {0} email like ''%{1}''", strDk == "" ? "" : " or ", itemChecked);
                    }
                    else {
                        strDk += string.Format(" {0} ( email not like ''%@yahoo.com'' and email not like ''%@yahoo.com.vn'' and email not like ''%@google.com'')", strDk == "" ? "" : " or ");
                    }
                }
                strDk += " group by a.id,a.email,b.name,b.path ";
                strSQL = strSQL + strDk;
               
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                //saveFileDialog1.Filter = !radtext.Checked ? "Excel|*.xls" : "text|*.txt";
                saveFileDialog1.Filter = !radtext.Checked ? "Excel 97-2003 WorkBook|*.xls|Excel WorkBook|*.xlsx|All Excel Files|*.xls;*.xlsx" : "text|*.txt";
                saveFileDialog1.Title = "Xuất File";
                saveFileDialog1.FileName = string.Format("{0}", Helpers.convertToUnSign3(strTieuDe));
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog1.FileName == "")
                    {

                        MessageBox.Show("Vui lòng nhập tên file", "Thông Báo");
                        return;
                    }
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();
                    string command = string.Format("exec [spExportScanner] '{0}','{1}'", strSQL, saveFileDialog1.FileName);
                    if (StorePhone.SQLDatabase.ExcNonQuery(command))
                    {
                        objPleaseWait.Close();
                        MessageBox.Show("Đã xuất thành công file.", "Thông Báo");
                    }
                    if (objPleaseWait != null)
                        objPleaseWait.Close();
                }

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message,"button2_Click");
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            PleaseWait objPleaseWait;
            try {
                string strDk = "";
                string strSQL = string.Format(" select  a.id,nhamang= (select nhamang from [{0}].[dbo].[dau_so] where id = b.parentId),phone,c.name,c.path as LienKet " +
                                " from [{0}].[dbo].[scanner_phone]  a , [{0}].[dbo].[dau_so] b , [vatgia].[dbo].[dm_scanner_ct] c" +
                                " where ((left(a.phone,3) = ltrim(rtrim(b.dauso)) and len(b.dauso)=3) or " +
                                " (left(a.phone,4) = ltrim(rtrim(b.dauso)) and len(b.dauso)=4)) and c.id=a.dm_scanner_ct_id", strdatabasename);

                if (checkedListBox2.CheckedItems.Count == 0) {
                    MessageBox.Show("Vui lòng chọn đầu số xuất file", "Thông Báo");
                    return;
                }

                foreach (object itemChecked in checkedListBox2.CheckedItems) {
                    DataRowView castedItem = itemChecked as DataRowView;
                    strDk += string.Format("phone like ''{0}%'' or ", castedItem["dauso"].ToString());
                }
                if (strDk.Length > 0)
                    strDk = strDk.Substring(0, strDk.Length - 4);


                strSQL = strSQL + " and (" + strDk +")";

                string strTieuDe = ((DataRowView)checkedListBox1.CheckedItems[0]).Row.ItemArray[1].ToString();

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = !radtext.Checked ? "Excel 97-2003 WorkBook|*.xls|Excel WorkBook|*.xlsx|All Excel Files|*.xls;*.xlsx" : "text|*.txt";

                saveFileDialog1.Title = "Xuất File";
                saveFileDialog1.FileName = string.Format("{0}", Helpers.convertToUnSign3(strTieuDe));
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog1.FileName == "")
                    {

                        MessageBox.Show("Vui lòng nhập tên file", "Thông Báo");
                        return;
                    }
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();
                    string command = string.Format("exec [spExportScanner] '{0}','{1}'", strSQL, saveFileDialog1.FileName);
                    if (StorePhone.SQLDatabase.ExcNonQuery(command))
                    {
                        objPleaseWait.Close();
                        MessageBox.Show("Đã xuất thành công file.", "Thông Báo");
                    }
                    if (objPleaseWait != null)
                        objPleaseWait.Close();
                }

            }catch(Exception  ex){
                MessageBox.Show(ex.Message, "button3_Click");
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            PleaseWait objPleaseWait = null;
            try {
                if (radioButton4.Checked && (MessageBox.Show("Bạn có chắc chắn muốn chuẩn hoá số liệu Scanner Phone và Scanner Email không?", "Thông Báo",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)) {
                    //TODO: Stuff
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();

                    StorePhone.SQLDatabase.ExcDataTable("[spDonDepDuLieu_scanner]");
                    objPleaseWait.Close();

                    MessageBox.Show("Đã chuẩn hoá số liệu Scanner Phone và Scanner Email ", "Thông Báo");
                    return;
                }
                if (!radioButton4.Checked && (MessageBox.Show("Bạn có chắc chắn xoá toàn bộ dữ liệu hồ sơ công ty không?", "Thông Báo",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)) {
                    //TODO: Stuff
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();

                    if(chkEmail.Checked)
                        StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[scanner_email]', RESEED, 0);  delete from scanner_email");
                    if (ckhPhone.Checked)
                        StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[scanner_phone]', RESEED, 0);  delete from scanner_phone");

                    objPleaseWait.Close();
                    MessageBox.Show("Đã xoá xong dữ liệu", "Thông Báo");
                    return;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "button1_Click");
            }
        }
    }
}
