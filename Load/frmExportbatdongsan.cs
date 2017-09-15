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
    public partial class frmExportbatdongsan : Form
    {
        public frmExportbatdongsan()
        {
            InitializeComponent();
            BindDmNhomNganh();
        }

        private string strdatabasename;
        public string strDatabaseName
        {
            get { return strdatabasename; }
            set { strdatabasename = value; }
        }

        private void frmExportbatdongsan_Load(object sender, EventArgs e)
        {

        }

        public void BindDmNhomNganh()
        {
            DataTable table = StorePhone.SQLDatabase.ExcDataTable("select id,name from dm_batdongsan where parentid is null");

            cmb_bds_nhom.DataSource = table;
            cmb_bds_nhom.ValueMember = "id";
            cmb_bds_nhom.DisplayMember = "name";
        }

        

        private void btn_hosocongty_excel_Click(object sender, EventArgs e)
        {
            try
            {
                frmDialogColumn frm = new frmDialogColumn();
                frm.FromParent = "batdongsan.com.vn";
                if (cmb_bds_nhom.Text.ToLower().Equals("nhà đất bán") ||
                    cmb_bds_nhom.Text.ToLower().Equals("nhà đất cho thuê"))
                {
                    frm.strbatdongsanmau = "canban";
                }
                else if (cmb_bds_nhom.Text.ToLower().Equals("nhà đất cần mua") ||
                   cmb_bds_nhom.Text.ToLower().Equals("nhà đất cần thuê"))
                {
                    frm.strbatdongsanmau = "canmua";
                }
                else {
                    frm.strbatdongsanmau = "moigioi";
                }

                    if (frm.ShowDialog() == DialogResult.OK)
                {
                    exportbatdongsan(frm.strColumn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_hosocongty_excel_Click");
            }
        }

        private void exportbatdongsan(string strcot)
        {

            StringBuilder myDK = new StringBuilder("");
            PleaseWait objPleaseWait = null;
            try
            {
                string strsql = string.Format(" select " + strcot + "	from {0}.dbo.batdongsan a left join {0}.dbo.dm_batdongsan b on a.danhmucid= b.id " +
                                         "  where b.parentid in({1}) order by a.danhmucid ", strdatabasename, cmb_bds_nhom.SelectedValue);

               

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = rad_hosocongty_excel.Checked ? "Excel|*.xls" : "text|*.txt";
                saveFileDialog1.Title = rad_hosocongty_excel.Checked ? "Xuất file Excel" : "Xuất file text";
                saveFileDialog1.FileName = string.Format("{0}", Helpers.convertToUnSign3(cmb_bds_nhom.Text));
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
                    //error : https://stackoverflow.com/questions/5131491/enable-xp-cmdshell-sql-server
                    string command = string.Format("exec [spExportBatdongsan] '{0}','{1}'", strsql, saveFileDialog1.FileName);
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
                if ((radioButton1.Checked || radioButton5.Checked) && (MessageBox.Show("Bạn có chắc chắn gôm dữ liệu trùng của bất động sản không?", "Thông Báo",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                {
                    //TODO: Stuff
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();

                    int ischuanhoa;
                    ischuanhoa = radioButton1.Checked ? 0 : 1;

                    StorePhone.SQLDatabase.ExcDataTable(string.Format("[spDonDepDuLieu_batdongsan] {0}", ischuanhoa));
                    objPleaseWait.Close();

                    MessageBox.Show("Đã gôm dữ liệu trùng của bất động sản", "Thông Báo");
                }
                if (radioButton2.Checked && (MessageBox.Show("Bạn có chắc chắn xoá toàn bộ dữ liệu bất động sản không?", "Thông Báo",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                {
                    //TODO: Stuff
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();

                    StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[batdongsan]', RESEED, 0);  delete from batdongsan");

                    objPleaseWait.Close();

                    MessageBox.Show("Đã xoá toàn bộ dữ của bất động sản", "Thông Báo");

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
