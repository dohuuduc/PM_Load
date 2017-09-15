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
    public partial class frmExporttimviec : Form
    {
        private string strdatabasename;
        public string strDatabaseName
        {
            get { return strdatabasename; }
            set { strdatabasename = value; }
        }

        public frmExporttimviec()
        {
            InitializeComponent();
            BindDmNhomNganh();
            checkBox2_CheckedChanged(null, null);
        }
        public void BindDmNhomNganh()
        {
            DataTable table = StorePhone.SQLDatabase.ExcDataTable("select id,name from dm_muaban where parentId is null ");

            checkedListBox2.DataSource = table;
            checkedListBox2.ValueMember = "id";
            checkedListBox2.DisplayMember = "name";
        }
        private void frmExporttimviec_Load(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, checkBox2.Checked);
            }
        }

        private void btn_hosocongty_excel_Click(object sender, EventArgs e)
        {
            try
            {
                frmDialogColumn frm = new frmDialogColumn();
                frm.FromParent = "muaban.net";
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    exportvatgia(frm.strColumn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_hosocongty_excel_Click");
            }
        }
        private string getstringListNhom()
        {
            try
            {
                StringBuilder str = new StringBuilder();
                foreach (DataRowView item in checkedListBox2.CheckedItems)
                {
                    str.Append(string.Format("{0},", item[checkedListBox2.ValueMember].ToString()));
                }
                if (str.Length > 1)
                    return str.ToString().Substring(0, str.Length - 1);
                return "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "getstringListNhom");
                return "";
            }
        }

        private void exportvatgia(string strcot)
        {

            StringBuilder myDK = new StringBuilder("");
            PleaseWait objPleaseWait = null;
            try
            {
                if (checkedListBox2.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn đầu mục cần để xuất báo cáo", "Thông Báo");
                    return;
                }
                string strTieuDe = ((DataRowView)checkedListBox2.CheckedItems[0]).Row.ItemArray[1].ToString();

                string strsql = string.Format(  " Select " + strcot + "	from {0}.dbo.muaban a left join {0}.dbo.dm_muaban b on a.danhmucid= b.id " +
                                                " where a.danhmucid in({1}) order by a.danhmucid ", strdatabasename, getstringListNhom());

                

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                //saveFileDialog1.Filter = rad_hosocongty_excel.Checked ? "Excel|*.xls" : "text|*.txt";
                saveFileDialog1.Filter = rad_hosocongty_excel.Checked ? "Excel 97-2003 WorkBook|*.xls|Excel WorkBook|*.xlsx|All Excel Files|*.xls;*.xlsx" : "text|*.txt";
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
                    string command = string.Format("exec [spExportMuaban] '{0}','{1}'", strsql, saveFileDialog1.FileName);
                    if (StorePhone.SQLDatabase.ExcNonQuery(command))
                        MessageBox.Show("Đã xuất thành công file.", "Thông Báo");
                    objPleaseWait.Close();
                }
            }
            catch (Exception ex)
            {
                if (objPleaseWait != null)
                    objPleaseWait.Close();
                MessageBox.Show(ex.Message, "exportvatgia");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PleaseWait objPleaseWait = null;
            try
            {
                if ((radioButton1.Checked || radioButton5.Checked) && (MessageBox.Show("Bạn có chắc chắn chuẩn hoá số liệu của tìm việc (muaban.net)  không?", "Thông Báo",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                {
                    //TODO: Stuff
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();

                    int ischuanhos;
                    ischuanhos = radioButton1.Checked ? 0 : 1;
                    StorePhone.SQLDatabase.ExcDataTable(string.Format("[spDonDepDuLieu_muaban] {0}", ischuanhos));
                    objPleaseWait.Close();

                    MessageBox.Show("Đã gôm dữ liệu trùng của muaban.net", "Thông Báo");
                }
                if (radioButton2.Checked && (MessageBox.Show("Bạn có chắc chắn chuẩn hoá số liệu tìm việc muaban.net không?", "Thông Báo",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                {
                    //TODO: Stuff
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();
                    StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[muaban]', RESEED, 0);  delete from muaban");
                    objPleaseWait.Close();

                    MessageBox.Show("Đã xoá toàn bộ dữ của tìm việc muaban.net", "Thông Báo");
                }
            }
            catch (Exception ex)
            {
                if (objPleaseWait != null)
                    objPleaseWait.Close();
                MessageBox.Show(ex.Message, "button1_Click");
            }
        }
    }
}
