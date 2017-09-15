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
    public partial class frmExportvatgia : Form
    {
        private string strdatabasename;
        public string strDatabaseName
        {
            get { return strdatabasename; }
            set { strdatabasename = value; }
        }
        public frmExportvatgia()
        {
            InitializeComponent();

        }
        private string fromParent;
        public string FromParent
        {
            get { return fromParent; }
            set { fromParent = value; }
        }
        public void BindDmNhomNganh()
        {
            DataTable table = StorePhone.SQLDatabase.ExcDataTable("select id,name from dm_vatgia where parentId is null ");

            checkedListBox2.DataSource = table;
            checkedListBox2.ValueMember = "id";
            checkedListBox2.DisplayMember = "name";
        }

        public void BindDmthitruongsi()
        {
            DataTable table = StorePhone.SQLDatabase.ExcDataTable("select id,name from dm_thitruongsi where parentId is null ");

            checkedListBox2.DataSource = table;
            checkedListBox2.ValueMember = "id";
            checkedListBox2.DisplayMember = "name";
        }

        private string getstringListNhom()
        {
            try
            {
                StringBuilder str = new StringBuilder();
                foreach (DataRowView item in checkedListBox2.CheckedItems)
                {
                    DataTable tb=null;
                    switch (FromParent)
                    {
                        case "vatgia.com":
                            tb = StorePhone.SQLDatabase.ExcDataTable(string.Format("[spFindDmvatgia] '{0}'", item[checkedListBox2.ValueMember].ToString()));
                            break;
                        case "thitruongsi.com":
                            tb = StorePhone.SQLDatabase.ExcDataTable(string.Format("[spFindDmthitruongsi] '{0}'", item[checkedListBox2.ValueMember].ToString()));
                            break;
                    }
                    foreach (DataRow item2 in tb.Rows)
                    {
                        str.Append(string.Format("{0},", item2["id"]));
                    }
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
                    MessageBox.Show("Vui lòng chọn nhóm sản phẩm để xuất báo cáo", "Thông Báo");
                    return;
                }
                string strTieuDe = ((DataRowView)checkedListBox2.CheckedItems[0]).Row.ItemArray[1].ToString();

                string strsql = string.Format(" select " + strcot + "	from {0}.dbo.vatgia a left join {0}.dbo.dm_vatgia b on a.danhmucid= b.id " +
                                         "  where a.danhmucid in({1}) order by a.danhmucid ", strdatabasename, getstringListNhom());
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
               // saveFileDialog1.Filter = rad_hosocongty_excel.Checked ? "Excel|*.xls" : "text|*.txt";
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
                    string command = string.Format("exec [spExportVatgia] '{0}','{1}'", strsql, saveFileDialog1.FileName);
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

        private void exportthitruongsi(string strcot)
        {

            StringBuilder myDK = new StringBuilder("");
            PleaseWait objPleaseWait = null;
            try
            {
                if (checkedListBox2.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn nhóm sản phẩm để xuất báo cáo", "Thông Báo");
                    return;
                }
                string strTieuDe = ((DataRowView)checkedListBox2.CheckedItems[0]).Row.ItemArray[1].ToString();

                string strsql = string.Format(" select " + strcot + "	from {0}.dbo.thitruongsi a left join {0}.dbo.dm_thitruongsi b on a.danhmucid= b.id " +
                                         "  where a.danhmucid in({1}) order by a.danhmucid ", strdatabasename, getstringListNhom());

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                //saveFileDialog1.Filter = rad_hosocongty_excel.Checked ? "Excel |*.xls " : "text|*.txt";
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
                    string command = string.Format("exec [spExportthitruongsi] '{0}','{1}'", strsql, saveFileDialog1.FileName);
                    if (StorePhone.SQLDatabase.ExcNonQuery(command))
                        MessageBox.Show("Đã xuất thành công file.", "Thông Báo");
                    objPleaseWait.Close();
                }
            }
            catch (Exception ex)
            {
                if (objPleaseWait != null)
                    objPleaseWait.Close();
                MessageBox.Show(ex.Message, "exportthitruongsi");
            }
        }

        private void btn_hosocongty_excel_Click(object sender, EventArgs e)
        {
            try
            {

                frmDialogColumn frm = new frmDialogColumn();
                frm.FromParent = FromParent;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    switch (FromParent)
                    {
                        case "vatgia.com":
                            exportvatgia(frm.strColumn);
                            break;
                        case "thitruongsi.com":
                            exportthitruongsi(frm.strColumn);
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_hosocongty_excel_Click");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PleaseWait objPleaseWait = null;
            try
            {
                switch (FromParent)
                {
                    case "vatgia.com":
                        #region vatgia.com
                        if ((radioButton1.Checked || radioButton5.Checked) && (MessageBox.Show("Bạn có chắc chắn gôm dữ liệu trùng của vật giá không?", "Thông Báo",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                        {
                            //TODO: Stuff
                            objPleaseWait = new PleaseWait();
                            objPleaseWait.Show();
                            objPleaseWait.Update();

                            int ischuanhoa;
                            ischuanhoa = radioButton1.Checked ? 0 : 1;

                            StorePhone.SQLDatabase.ExcDataTable(string.Format("[spDonDepDuLieu_vatgia] {0}", ischuanhoa));
                            objPleaseWait.Close();

                            MessageBox.Show("Đã gôm dữ liệu trùng của vật giá", "Thông Báo");
                        }
                        if (radioButton2.Checked && (MessageBox.Show("Bạn có chắc chắn xoá toàn bộ dữ liệu vật giá không?", "Thông Báo",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                        {
                            //TODO: Stuff
                            objPleaseWait = new PleaseWait();
                            objPleaseWait.Show();
                            objPleaseWait.Update();

                            StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[vatgia]', RESEED, 0);  delete from vatgia");

                            objPleaseWait.Close();

                            MessageBox.Show("Đã xoá toàn bộ dữ của vật giá", "Thông Báo");

                        }
                        #endregion
                        break;
                    case "thitruongsi.com":
                        #region thitruongsi.com
                        if ((radioButton1.Checked || radioButton5.Checked) && (MessageBox.Show("Bạn có chắc chắn gôm dữ liệu trùng của thị trường sỉ không?", "Thông Báo",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                        {
                            //TODO: Stuff
                            objPleaseWait = new PleaseWait();
                            objPleaseWait.Show();
                            objPleaseWait.Update();

                            int ischuanhoa;
                            ischuanhoa = radioButton1.Checked ? 0 : 1;

                            StorePhone.SQLDatabase.ExcDataTable(string.Format("[spDonDepDuLieu_thitruongsi] {0}", ischuanhoa));
                            objPleaseWait.Close();

                            MessageBox.Show("Đã gôm dữ liệu trùng của thị trường sĩ", "Thông Báo");
                        }
                        if (radioButton2.Checked && (MessageBox.Show("Bạn có chắc chắn xoá toàn bộ dữ liệu thị trường sĩ không?", "Thông Báo",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                        {
                            //TODO: Stuff
                            objPleaseWait = new PleaseWait();
                            objPleaseWait.Show();
                            objPleaseWait.Update();

                            StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[thitruongsi]', RESEED, 0);  delete from thitruongsi");

                            objPleaseWait.Close();

                            MessageBox.Show("Đã xoá toàn bộ dữ của thị trường sĩ", "Thông Báo");

                        }
                        #endregion
                        break;
                }

            }
            catch (Exception ex)
            {
                if (objPleaseWait != null)
                    objPleaseWait.Close();

                MessageBox.Show(ex.Message, "button1_Click");
            }
        }

        private void frmExportvatgia_Load(object sender, EventArgs e)
        {
            this.Text = FromParent;
            switch (FromParent)
            {
                case "vatgia.com":
                    BindDmNhomNganh();
                    checkBox2_CheckedChanged(null, null);
                    break;
                case "thitruongsi.com":
                    BindDmthitruongsi();
                    checkBox2_CheckedChanged(null, null);
                    break;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, checkBox2.Checked);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
