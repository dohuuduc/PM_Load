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
    public partial class frmExporthosocongty : Form
    {
        private string strdatabasename;
        public string strDatabaseName {
            get { return strdatabasename; }
            set { strdatabasename = value; }
        }

       

        public frmExporthosocongty()
        {
            InitializeComponent();
            PleaseWait objPleaseWait = null;
            objPleaseWait = new PleaseWait();
            objPleaseWait.Show();
            objPleaseWait.Update();

            BindDmTinh_new();
            BindDmNhomNganh();
            CheckAutoTinh();
            radioButton3_CheckedChanged(null, null);
            checkBox2_CheckedChanged(null, null);

            objPleaseWait.Close();

        }
        public void BindDmTinh_new()
        {
            DataTable table = StorePhone.SQLDatabase.ExcDataTable("select tinh from hosocongty group by tinh order by tinh asc");

            checkedListBox1.DataSource = table;
            checkedListBox1.ValueMember = "tinh";
            checkedListBox1.DisplayMember = "tinh";
        }

        public void BindDmNhomNganh() {
            DataTable table = StorePhone.SQLDatabase.ExcDataTable("select id,name from dm_hsct where capid=1 ");

            checkedListBox2.DataSource = table;
            checkedListBox2.ValueMember = "id";
            checkedListBox2.DisplayMember = "name";

            

        }

        private string getstringListNhom() {
            try {
                StringBuilder str = new StringBuilder();
                foreach (DataRowView item in checkedListBox2.CheckedItems) {
                    DataTable tb = StorePhone.SQLDatabase.ExcDataTable(string.Format("[spFindDanhMuc] '{0}'", item[checkedListBox2.ValueMember].ToString()));
                    foreach (DataRow item2 in tb.Rows) {
                        str.Append(string.Format("{0},", item2["id"]));    
                    }
                }
                if (str.Length > 1)
                    return str.ToString().Substring(0, str.Length - 1);
                return "";
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message,"getstringListNhom");
                return "";
            }
        }
        private void btn_hosocongty_excel_Click(object sender, EventArgs e)
        {
            frmDialogColumn frm = new frmDialogColumn();
            frm.FromParent = "hosocongty.vn";

            if (frm.ShowDialog() == DialogResult.OK) {
                xuatfile(frm.strColumn);
            }
           // xuatfile("");  
        }
        private void xuatfile(string strcolumn) {
            PleaseWait objPleaseWait = null;
            StringBuilder myDK = new StringBuilder("");
            string str = "", strTieuDe="";
            try {
                if (radioButton3.Checked && checkedListBox1.CheckedItems.Count == 0) {
                    MessageBox.Show("Vui lòng chọn 1 vài tỉnh thành để xuất báo cáo", "Thông Báo");
                    return;
                }
                if (radioButton4.Checked && checkedListBox2.CheckedItems.Count == 0) {
                    MessageBox.Show("Vui lòng chọn nhóm ngành nghề để xuất báo cáo", "Thông Báo");
                    return;
                }
                //Bổ sung vào cuối một chuỗi
                foreach (var item in checkedListBox1.CheckedItems) {
                    if (((System.Data.DataRowView)(item)).Row.ItemArray[0].ToString() != "")
                        myDK.Append(string.Format("N''{0}'',", ((System.Data.DataRowView)(item)).Row.ItemArray[0]));
                }
                if (myDK.Length != 0)
                    str = myDK.ToString().Substring(0, myDK.ToString().Length - 1);

                string strsql = "";

                if (radioButton3.Checked)
                {
                    strTieuDe = ((DataRowView)checkedListBox1.CheckedItems[0]).Row.ItemArray[1].ToString();
                    strsql = string.Format(" select ms_thue,	giam_doc ,	dien_thoai,	dia_chi,	xa ,	huyen, 	tinh , 	email,	ten_cong_ty, " +
                                                   " b.maid as ma_nghanh ,b.name as ten_nganh,	a.nganh_nghe_chinh as ma_nganh_chinh, " +
                                                   " ten_nganh_chinh = (select name from dm_hsct c 	where c.maid=a.nganh_nghe_chinh),  " +
                                                   "  website_cty,	ten_quoc_te, ten_giao_dich, ngan_hang,so_tai_khoan,ngay_dong_cua, gp_kinh_doanh, 	fax,	ngay_cap ,  ngay_hoat_dong into tempExport" +
                                                   " from hosocongty a left join dm_hsct b on a.danhmucid= b.id " +
                                                   " where tinh in({0}) " +
                                                   " order by ten_nganh desc ", str);
                }
                else
                {
                    strTieuDe = ((DataRowView)checkedListBox2.CheckedItems[0]).Row.ItemArray[1].ToString();
                    strsql = string.Format(" select ms_thue,	giam_doc ,	dien_thoai,	dia_chi,	xa ,	huyen, 	tinh , 	email,	ten_cong_ty, " +
                                               " b.maid as ma_nghanh ,b.name as ten_nganh,	a.nganh_nghe_chinh as ma_nganh_chinh, " +
                                               " ten_nganh_chinh = (select name from dm_hsct c 	where c.maid=a.nganh_nghe_chinh),  " +
                                               "  website_cty,	ten_quoc_te, ten_giao_dich, ngan_hang,so_tai_khoan,ngay_dong_cua,gp_kinh_doanh, 	fax,	ngay_cap ,  ngay_hoat_dong into tempExport" +
                                               " from hosocongty a left join dm_hsct b on a.danhmucid= b.id " +
                                               " where a.danhmucid in({0}) " +
                                               " order by ten_nganh desc ", getstringListNhom());
                }

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = rad_hosocongty_excel.Checked ? "Excel|*.xls" : "text|*.txt";
                saveFileDialog1.Title = "Xuất File";
                saveFileDialog1.FileName = string.Format("{0}", Helpers.convertToUnSign3(strTieuDe));
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName == "") {

                    MessageBox.Show("Vui lòng nhập tên file", "Thông Báo");
                    return;
                }
                objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();
                string command = string.Format("exec [spExportHosocongty] N'{0}','{1}','{2}','{3}'", strsql, strcolumn, saveFileDialog1.FileName,strdatabasename);
                if (StorePhone.SQLDatabase.ExcNonQuery(command)) {
                    objPleaseWait.Close();
                    MessageBox.Show("Đã xuất thành công file.", "Thông Báo");
                }
                if (objPleaseWait != null)
                    objPleaseWait.Close();
                MessageBox.Show("Xuất file thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) {
                if (objPleaseWait != null)
                    objPleaseWait.Close();
                MessageBox.Show(ex.Message, "btn_hosocongty_excel_Click");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PleaseWait objPleaseWait = null;
            try
            {
                if ((radioButton1.Checked || radioButton3.Checked) && (MessageBox.Show("Bạn có chắc chắn chuẩn hoá số liệu của hồ sơ công ty không?", "Thông Báo",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                {
                    //TODO: Stuff
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();

                    int ischuanhoa;
                    ischuanhoa = radioButton1.Checked ? 0 : 1;
                    StorePhone.SQLDatabase.ExcDataTable(string.Format("[spDonDepDuLieu_hsct] {0}", ischuanhoa));
                    objPleaseWait.Close();

                    MessageBox.Show("Đã chuẩn hoá số liệu hồ sơ công ty", "Thông Báo");
                }
                if (radioButton2.Checked && (MessageBox.Show("Bạn có chắc chắn xoá toàn bộ dữ liệu hồ sơ công ty không?", "Thông Báo",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                {
                    //TODO: Stuff
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();

                    StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[hosocongty]', RESEED, 0);  delete from hosocongty");

                    objPleaseWait.Close();


                    MessageBox.Show("Đã xoá toàn bộ dữ của hồ sơ công ty", "Thông Báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button1_Click");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckAutoTinh();
        }

        public void CheckAutoTinh() {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, checkBox1.Checked);
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e) {
            if (radioButton3.Checked) {
                checkedListBox1.Enabled = true;
                checkBox1.Enabled = true;
                checkedListBox2.Enabled = false;
                checkBox2.Enabled = false;
            }
            else {
                checkedListBox1.Enabled = false;
                checkBox1.Enabled = false;
                checkedListBox2.Enabled = true;
                checkBox2.Enabled = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            for (int i = 0; i < checkedListBox2.Items.Count; i++) {
                checkedListBox2.SetItemChecked(i, checkBox2.Checked);
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void frmExporthosocongty_Load(object sender, EventArgs e)
        {

        }
    }
}
