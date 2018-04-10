using StorePhone;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Load
{
    public partial class frmExportVinabiz : Form
    {
        public frmExportVinabiz()
        {
            InitializeComponent();
            
        }
        private static LogWriter writer;
        private Thread theardProcess;
        public bool hasProcess = true; /*khai bao bien stop*/

        private string strdatabasename;
        public string strDatabaseName
        {
            get { return strdatabasename; }
            set { strdatabasename = value; }
        }

        private DataTable _table_vinabiz;
        private DataTable _table_tinh;


        private void frmExportVinabiz_Load(object sender, EventArgs e)
        {
            
            BindDmNhom();
            _table_vinabiz = CreateTable_vinabiz();
            gw_vinabiz_chon.DataSource = _table_vinabiz;
            gw_vinabiz_chon.Sort(gw_vinabiz_chon.Columns["id_chon"], ListSortDirection.Ascending);

            BindDmTinh_new();

            /*tách ra 2 trường hop; do thuat toan tim gan dung u tien du lieu tim dau tien; quy tac muc cha tim cuoi cung*/
            List<dm_hsct> tbHsct = SQLDatabase.Loaddm_hsct("select * from dm_hsct where path<>''");
            foreach (var item in SQLDatabase.Loaddm_hsct("select * from dm_hsct where path=''"))
                tbHsct.Add(item);
            Utilities_vinabiz._listHsct = tbHsct;
            Utilities_vinabiz._listquetcan = SQLDatabase.Loaddm_vinabiz_map("select * from dm_vinabiz_map");

            dateTimePicker1.Value = new DateTime(1990,  1,  1);

    }

        public DataTable CreateTable_vinabiz()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id_chon", typeof(int));
            table.Columns.Add("maid_chon", typeof(string));
            table.Columns.Add("name_chon", typeof(string));
            table.Columns.Add("tongsl_chon", typeof(int));
            return table;
        }

        public DataTable CreateTable_tinh() {
            DataTable table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("ten", typeof(string));
            return table;
        }
        public void BindDmTinh_new()
        {
            
            int nKXD = ConvertType.ToInt(SQLDatabase.ExcDataTable("select dbo.fnGetSoLuongVinabizByTinhThanh(0)").Rows[0][0]);
            DataTable table = StorePhone.SQLDatabase.ExcDataTable("select id,ten +'_'+ REPLACE(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,tongsl),1), '.00',''),',','.') as ten from dm_Tinh order by ten asc");
            table.Rows.Add(0,string.Format("[Không Xác Định]_{0}",nKXD));

            checkedListBox1.DataSource = table;
            checkedListBox1.ValueMember = "id";
            checkedListBox1.DisplayMember = "ten";
            
            /*
            _table_tinh = CreateTable_tinh();
            checkedListBox1.DataSource = _table_tinh;
            checkedListBox1.ValueMember = "id";
            checkedListBox1.DisplayMember = "ten";

            int nKXD = ConvertType.ToInt(SQLDatabase.ExcDataTable("select dbo.fnGetSoLuongVinabizByTinhThanh(0)").Rows[0][0]);
            DataTable table = StorePhone.SQLDatabase.ExcDataTable("select id,ten +'_'+ REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,tongsl),1), '.00','') as ten from dm_Tinh order by ten asc");
            _table_tinh.Rows.Add(0, string.Format("[Không Xác Định]_{0}", nKXD));
            foreach (DataRow item in table.Rows)
            {
                _table_tinh.Rows.Add(item["id"], item["ten"]);
            }
            */
        }

        

        private void btn_hosocongty_excel_Click(object sender, EventArgs e)
        {
            frmDialogColumn frm = new frmDialogColumn();
           
            frm.FromParent = "vinabiz.org";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                xuatfile(frm.strColumn);
            }
        }

        
        public void CheckAutoTinh()
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, checkBox1.Checked);
            }
        }
        private string getstringListTinh()
        {
            try
            {
                StringBuilder str = new StringBuilder();
                foreach (DataRowView item in checkedListBox1.CheckedItems)
                {
                   str.Append(string.Format("{0},", item["id"]));
                }
                if (str.Length > 1)
                    return str.ToString().Substring(0, str.Length - 1);
                return "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "getstringListTinh");
                return "";
            }
        }

      

        private void xuatfile(string strcolumn)
        {
            PleaseWait objPleaseWait = null;
            StringBuilder myDK = new StringBuilder("");
            try
            {
                if (radioButton3.Checked && checkedListBox1.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn 1 vài tỉnh thành để xuất báo cáo", "Thông Báo");
                    return;
                }
                if (radioButton4.Checked && _table_vinabiz.Rows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn nhóm ngành nghề để xuất báo cáo", "Thông Báo");
                    return;
                }
                string strsql = "",strDK="", strTieuDe="";


                if (radioButton3.Checked)
                {
                    strTieuDe = ((DataRowView)checkedListBox1.CheckedItems[0]).Row.ItemArray[1].ToString();
                    strsql = string.Format(" select  " + strcolumn + "	from {0}.dbo.vinabiz a left join {0}.dbo.dm_vinabiz b on a.danhmucid= b.id " +
                                      "  where a.ttlh_tinhid in({1}) and (CONVERT(DATETIME,ttdk_ngaycap,103) between ''{2}'' and ''{3}'') order by a.danhmucid ", strdatabasename, getstringListTinh(),dateTimePicker1.Text,dateTimePicker2.Text);
                }
                else
                {
                    strTieuDe = string.Format( "{0}_{1}_{2}", _table_vinabiz.Rows[0]["maid_chon"].ToString(),
                                                             Regex.Replace(_table_vinabiz.Rows[0]["name_chon"].ToString(), @"\d", "").Replace("-","").Trim(),
                                                          ConvertType.ToInt( _table_vinabiz.Rows[0]["tongsl_chon"]).ToString("N0").Replace(',','.'));

                    getstringListNhom(ref strDK,radCha.Checked);
                    strsql = string.Format(" select  " + strcolumn + "	from {0}.dbo.vinabiz a left join {0}.dbo.dm_vinabiz b on a.danhmucid= b.id " +
                                      "  where  ({1}) and (CONVERT(DATETIME,ttdk_ngaycap,103) between ''{2}''and ''{3}'') order by a.danhmucid ", strdatabasename, strDK,dateTimePicker1.Text,dateTimePicker2.Text);
                }

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = rad_hosocongty_excel.Checked ? "Excel 97-2003 WorkBook|*.xls" : "text|*.txt";
                saveFileDialog1.Title = rad_hosocongty_excel.Checked ? "Xuất file Excel" : "Xuất file text";
                saveFileDialog1.FileName = string.Format("{0}", Helpers.convertToUnSign3(strTieuDe));
                //saveFileDialog1.ShowDialog();
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
                    string command = string.Format("exec [spExportVinabiz] 'SET DATEFORMAT DMY {0}','{1}'", strsql, saveFileDialog1.FileName);
                    if (StorePhone.SQLDatabase.ExcNonQuery(command))
                    {
                        objPleaseWait.Close();
                        MessageBox.Show("Đã xuất thành công file.", "Thông Báo");
                    }
                    if (objPleaseWait != null)
                        objPleaseWait.Close();
                    //MessageBox.Show("Xuất file thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (objPleaseWait != null)
                    objPleaseWait.Close();
                MessageBox.Show(ex.Message, "btn_hosocongty_excel_Click");
            }
        }

        private void xuatfile(string strcolumn,string namefile,int id,bool isTinh)
        {
            PleaseWait objPleaseWait = null;
            StringBuilder myDK = new StringBuilder("");
            try
            {
                string strsql = "", strDK = "", strTieuDe = "";

                if (isTinh)
                {
                    strTieuDe = ((DataRowView)checkedListBox1.CheckedItems[0]).Row.ItemArray[1].ToString();
                    strsql = string.Format(" select  " + strcolumn + "	from {0}.dbo.vinabiz a left join {0}.dbo.dm_vinabiz b on a.danhmucid= b.id " +
                                      "  where a.ttlh_tinhid in({1}) order by a.danhmucid ", strdatabasename, id);
                }
                else
                {
                    

                    getstringListNhom(ref strDK,id);
                    strsql = string.Format(" select  " + strcolumn + "	from {0}.dbo.vinabiz a left join {0}.dbo.dm_vinabiz b on a.danhmucid= b.id " +
                                      "  where  ({1}) order by a.danhmucid ", strdatabasename, strDK);
                }

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = rad_hosocongty_excel.Checked ? "Excel 97-2003 WorkBook|*.xls" : "text|*.txt";
                saveFileDialog1.Title = rad_hosocongty_excel.Checked ? "Xuất file Excel" : "Xuất file text";
                saveFileDialog1.FileName = namefile;
                //saveFileDialog1.ShowDialog();
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
                    string command = string.Format("exec [spExportVinabiz] '{0}','{1}'", strsql, saveFileDialog1.FileName);
                    if (StorePhone.SQLDatabase.ExcNonQuery(command))
                    {
                        objPleaseWait.Close();
                        MessageBox.Show("Đã xuất thành công file.", "Thông Báo");
                    }
                    if (objPleaseWait != null)
                        objPleaseWait.Close();
                    //MessageBox.Show("Xuất file thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (objPleaseWait != null)
                    objPleaseWait.Close();
                MessageBox.Show(ex.Message, "btn_hosocongty_excel_Click");
            }
        }


        private void getstringListNhom(ref string strDk,bool isCha)
        {
            if (radioButton8.Checked)/*danh sách ngành nghề*/
            {
                List<string> ds = new List<string>();
                for (int i = 0; i < _table_vinabiz.Rows.Count; i++)
                {
                    int id = ConvertType.ToInt(_table_vinabiz.Rows[i]["id_chon"]);
                    DataTable table = SQLDatabase.ExcDataTable(string.Format("[spFindDanhMuc] '{0}'", id));
                    if (table.Rows.Count != 0 && isCha)
                        foreach (DataRow item in table.Rows)
                        {
                            if (ds.Count(p => p.Equals(item["id"].ToString().PadLeft(3,'0'))) == 0)
                            {
                                ds.Add(item["id"].ToString().PadLeft(3, '0'));

                                strDk += string.Format("ds_nganhnghekinhdoanhId LIKE ''%{0}%'' or ", item["id"].ToString().PadLeft(3, '0'));
                            }
                        }
                    else
                    {

                        strDk += string.Format("ds_nganhnghekinhdoanhId LIKE ''%{0}%'' or ", id.ToString().PadLeft(3, '0'));
                    }
                }

                strDk = strDk.Substring(0, strDk.Length - 3);
            }
            if (radioButton7.Checked) {/*ngành nghề chinh*/
                List<string> ds = new List<string>();
                strDk += "danhmucid in (";
                for (int i = 0; i < _table_vinabiz.Rows.Count; i++)
                {
                    int id = ConvertType.ToInt(_table_vinabiz.Rows[i]["id_chon"]);
                    DataTable table = SQLDatabase.ExcDataTable(string.Format("[spFindDanhMuc] '{0}'", id));
                    if (table.Rows.Count != 0 && isCha)
                    {
                       
                        foreach (DataRow item in table.Rows)
                        {
                            if (ds.Count(p => p.Equals(item["id"])) == 0)
                            {
                                ds.Add(item["id"].ToString());

                                strDk += string.Format("{0}, ", item["id"].ToString());
                            }
                        }
                    }
                    else
                    {

                        strDk += string.Format("{0}, ", id.ToString());
                    }
                }

                strDk = strDk.Substring(0, strDk.Length - 2) + ")";
            }

        }
        private void getstringListNhom(ref string strDk, int id)
        {
            if (radioButton8.Checked)/*danh sách ngành nghề*/
            {
                List<string> ds = new List<string>();
                    DataTable table = SQLDatabase.ExcDataTable(string.Format("[spFindDanhMuc] '{0}'", id));
                    if (table.Rows.Count != 0 )
                        foreach (DataRow item in table.Rows)
                        {
                            if (ds.Count(p => p.Equals(item["id"].ToString().PadLeft(3, '0'))) == 0)
                            {
                                ds.Add(item["id"].ToString().PadLeft(3, '0'));

                                strDk += string.Format("ds_nganhnghekinhdoanhId LIKE ''%{0}%'' or ", item["id"].ToString().PadLeft(3, '0'));
                            }
                        }
                    else
                    {

                        strDk += string.Format("ds_nganhnghekinhdoanhId LIKE ''%{0}%'' or ", id.ToString().PadLeft(3, '0'));
                    }

                strDk = strDk.Substring(0, strDk.Length - 3);
            }
            if (radioButton7.Checked)
            {/*ngành nghề chinh*/
                List<string> ds = new List<string>();
                strDk += "danhmucid in (";
                   
                    DataTable table = SQLDatabase.ExcDataTable(string.Format("[spFindDanhMuc] '{0}'", id));
                    if (table.Rows.Count != 0)
                    {

                        foreach (DataRow item in table.Rows)
                        {
                            if (ds.Count(p => p.Equals(item["id"])) == 0)
                            {
                                ds.Add(item["id"].ToString());

                                strDk += string.Format("{0}, ", item["id"].ToString());
                            }
                        }
                    }
                    else
                    {

                        strDk += string.Format("{0}, ", id.ToString());
                    }

                strDk = strDk.Substring(0, strDk.Length - 2) + ")";
            }

        }


        private void getstringListNhom(int id, ref string strDk, bool isCha)
        {
            if (radioButton8.Checked)/*danh sách ngành nghề*/
            {
                List<string> ds = new List<string>();
                
                    
                    DataTable table = SQLDatabase.ExcDataTable(string.Format("[spFindDanhMuc] '{0}'", id));
                    if (table.Rows.Count != 0 && isCha)
                        foreach (DataRow item in table.Rows)
                        {
                            if (ds.Count(p => p.Equals(item["id"].ToString().PadLeft(3, '0'))) == 0)
                            {
                                ds.Add(item["id"].ToString().PadLeft(3, '0'));

                                strDk += string.Format("ds_nganhnghekinhdoanhId LIKE ''%{0}%'' or ", item["id"].ToString().PadLeft(3, '0'));
                            }
                        }
                    else
                    {

                        strDk += string.Format("ds_nganhnghekinhdoanhId LIKE ''%{0}%'' or ", id.ToString().PadLeft(3, '0'));
                    }
                

                strDk = strDk.Substring(0, strDk.Length - 3);
            }
            if (radioButton7.Checked)
            {/*ngành nghề chinh*/
                List<string> ds = new List<string>();
                strDk += "danhmucid in (";
                
                    DataTable table = SQLDatabase.ExcDataTable(string.Format("[spFindDanhMuc] '{0}'", id));
                    if (table.Rows.Count != 0 && isCha)
                    {

                        foreach (DataRow item in table.Rows)
                        {
                            if (ds.Count(p => p.Equals(item["id"])) == 0)
                            {
                                ds.Add(item["id"].ToString());

                                strDk += string.Format("{0}, ", item["id"].ToString());
                            }
                        }
                    }
                    else
                    {

                        strDk += string.Format("{0}, ", id.ToString());
                    }
                

                strDk = strDk.Substring(0, strDk.Length - 2) + ")";
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckAutoTinh();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                if ((radioButton5.Checked) && (MessageBox.Show("Bạn có chắc chắn chuẩn hoá số liệu của vinabiz không?", "Thông Báo",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                {
                    PleaseWait objPleaseWait = null;
                    //TODO: Stuff
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();

                  
                    StorePhone.SQLDatabase.ExcDataTable(string.Format("[spDonDepDuLieu_vinabiz]"));
                    objPleaseWait.Close();

                    MessageBox.Show("Đã chuẩn hoá số liệu vinabiz", "Thông Báo");
                    frmExportVinabiz_Load(null, null);
                }
                if (radioButton2.Checked && (MessageBox.Show("Bạn có chắc chắn xoá toàn bộ dữ liệu vinabiz không?", "Thông Báo",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                {
                    PleaseWait objPleaseWait = null;
                    //TODO: Stuff
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();

                    StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[vinabiz]', RESEED, 0);  delete from vinabiz ;  ");
                    StorePhone.SQLDatabase.ExcDataTable("update dm_hsct set tongsl=0;");
                    StorePhone.SQLDatabase.ExcDataTable("update dm_Tinh set tongsl =0;");
                    frmExportVinabiz_Load(null, null);
                   

                    objPleaseWait.Close();
                    MessageBox.Show("Đã xoá toàn bộ dữ của vinabiz", "Thông Báo");
                }
                if (radioButton6.Checked || radioButton1.Checked) {
                    chuanhoa();
                }
                if (radioButton11.Checked) {
                    try
                    {
                        frm_chuanhoanPhone frm = new frm_chuanhoanPhone();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            //BindingTelNumberToGridView();
                            //SoLuongKhachHang();
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button1_Click");
            }
        }
        private void chuanhoa() {
            ParameterizedThreadStart par;
            ArrayList arr;
            try
            {
                if (button1.Text == "Xử Lý")
                {
                    button1.Text = "Stop";
                    hasProcess = true;
                    lbl_message.Visible = true;
                    groupBox1.Enabled = true;
                    groupBox2.Enabled = true;
                    groupBox5.Enabled = true;

                   
                }
                else
                {
                    button1.Text = "Xử Lý";
                    hasProcess = false;
                    lbl_message.Visible = false;
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox5.Enabled = false;
                }

                /*Cấu hình controll*/
                Control.CheckForIllegalCrossThreadCalls = false;
                if(radioButton1.Checked)
                    par = new ParameterizedThreadStart(ProcessChuanhoaNganhNgheChinh);
                else
                    par = new ParameterizedThreadStart(ProcessChuanhoaNganhNghePhu);
                theardProcess = new Thread(par);

                arr = new ArrayList();
                arr.Add(lbl_message);
                arr.Add(lbl_phantram);
                arr.Add(pr);
               

                theardProcess.IsBackground = true;
                theardProcess.Start(arr);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }
        public void BindDmNhom()
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("select id, REPLACE(CONVERT(varchar(20), (CAST(tongsl AS money)), 1), '.00', '')+'_'+ name as name,capid,parentid from dm_hsct where capid=1 or capid=2"));
            DataTable tb_new = table.Clone();
            tb_new.Clear();
            tb_new.Rows.Add(-1, "---Chọn Nhóm Gian Hàng---");
            tb_new.Rows.Add(0,string.Format("{0}_[Chưa rõ thuộc ngành nghề nào]",ConvertType.ToInt(SQLDatabase.ExcDataTable("select COUNT(*) FROM vinabiz where danhmucid"))));

            foreach (DataRow item in table.Select("capid=1"))
            {
                
                tb_new.ImportRow(item);
                /*
                foreach (DataRow item2 in table.Select(string.Format("capid='{0}' and parentid='{1}'", 2, item["id"])))
                {
                    tb_new.ImportRow(item2);
                }
                */
            }
            cmb_vnBizNhom.DataSource = tb_new;
            cmb_vnBizNhom.ValueMember = "id";
            cmb_vnBizNhom.DisplayMember = "name";
            cmb_vnBizNhom.SelectedValue = -1;
            cmbNhom_SelectedIndexChanged(null, null);

        }

        private void cmbNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDMHosocongty_new(cmb_vnBizNhom.SelectedValue);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "cmbNhom_SelectedIndexChanged");
            }
        }
        public void BindDMHosocongty_new(object nhom)
        {
            if (nhom == null || nhom.Equals(-1)) return;
            if (nhom.ToString() == "System.Data.DataRowView") return;


            DataTable table = SQLDatabase.ExcDataTable(string.Format("[spFindDanhMuc] '{0}'", nhom));
            DataTable tb_new = table.Clone();
            tb_new.Clear();

            DataRow[] tb_cap1 = table.Select(string.Format("capid=2 and parentId={0}", nhom));
            foreach (DataRow item in tb_cap1)
            {
                tb_new.ImportRow(item);
                DataRow[] tb_cap_duoi = table.Select(string.Format("parentId={0}", item["id"].ToString()));
                foreach (DataRow item2 in tb_cap_duoi)
                {
                    tb_new.ImportRow(item2);
                }
            }

            //if (tb_cap1.Count() == 0)
            //    tb_new = table.Copy();
            DataRow[] tb_cap3_4_5 = table.Select(string.Format("capid in(3,4,5) and parentId={0}", nhom));
            foreach (DataRow item in tb_cap3_4_5)
            {
                tb_new.ImportRow(item);
            }
            if (nhom.Equals(0))
            {
                tb_new.Rows.Add(nhom, nhom, nhom, "Chưa rõ thuộc ngành nghề nào");
                tb_new.Rows[0]["alevel"] = 0;
                tb_new.Rows[0]["tongsl"] = ConvertType.ToInt(SQLDatabase.ExcDataTable("select COUNT(*) from vinabiz where danhmucid=0").Rows[0][0]);
            }
            gw_vinabiz_goc.DataSource = tb_new;
        }
        private void ProcessChuanhoaNganhNghePhu(object arrControl)
        {
            try
            {
                //----- Add control process from
                ArrayList arr1 = (ArrayList)arrControl;
                Label lbl_message = (Label)arr1[0];
                Label lbl_phantram = (Label)arr1[1];
                ProgressBar pr = (ProgressBar)arr1[2];
               
                lbl_message.Text = "Đang lọc số liệu có danh sách ngành nghề là rỗng để xử lý.....";
                lbl_message.Font = new Font(lbl_message.Font, FontStyle.Bold);
                lbl_message.Update();
                DataTable _tbAll = SQLDatabase.ExcDataTable("select  id,ds_nganhnghekinhdoanh from vinabiz where ds_nganhnghekinhdoanhId is null");
               
                lbl_message.Text = string.Empty;
                lbl_message.Font = new Font(lbl_message.Font, FontStyle.Regular);
                lbl_message.Update();
                int total = _tbAll.Rows.Count;
                pr.Value = 0;
                pr.Maximum = total;
                lbl_phantram.Text = "0% Hoàn thành...";
                lbl_phantram.Update();
                vinabiz model = new vinabiz();
               
                int i = 0;
                int tongcong = _tbAll.Rows.Count;
                foreach (DataRow item in _tbAll.Rows)
                {
                    string strdmnganhnghe = item["ds_nganhnghekinhdoanh"].ToString();
                    string strlistNganhNghe = "";

                    string[] words = strdmnganhnghe.Split('|');
                    model.id = ConvertType.ToInt(item["id"]);
                    foreach (var word in words)
                    {
                        dm_hsct dm = Utilities_vinabiz.getIDKiemTraGanDung(word.Trim());
                        if (dm == null)
                            strlistNganhNghe += "000 | ";
                        else
                            strlistNganhNghe += dm.id.ToString().PadLeft(3,'0') + " | ";
                    }
                    pr.PerformStep();
                    pr.Update();
                    model.ds_nganhnghekinhdoanhid = strlistNganhNghe;

                    lbl_message.Text =string.Format( "Đang xử lý: {0}/{1}    id: {2} -> {3}",i.ToString("#,##0"), tongcong.ToString("#,##0"), item["id"],item["ds_nganhnghekinhdoanh"].ToString());
                    lbl_message.Update();

                    i++;
                    lbl_phantram.Text = Math.Round((i / (double)total) * 100, 0) + "% Hoàn thành...";
                    lbl_phantram.Update();

                    SQLDatabase.UpdateChuanHoaDanhSachNganhNghe(model);
                    if (!hasProcess) break;
                }
                /*===================================================================*/
                lbl_message.Text = hasProcess ? "Hoàn thành chuẩn hoá số liệu." : "Tạm dừng do người dùng!!! ";


                button1.Text = "Xử Lý-Start";
                lbl_message.Visible = true;
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox5.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ProcessChuanhoa");
            }
        }

        private void ProcessChuanhoaNganhNgheChinh(object arrControl)
        {
            try
            {
                //----- Add control process from
                ArrayList arr1 = (ArrayList)arrControl;
                Label lbl_message = (Label)arr1[0];
                Label lbl_phantram = (Label)arr1[1];
                ProgressBar pr = (ProgressBar)arr1[2];

                lbl_message.Text = "Đang lọc số liệu có ngành nghề chính là rỗng để xử lý.....";
                lbl_message.Font = new Font(lbl_message.Font, FontStyle.Bold);
                lbl_message.Update();
                DataTable _tbAll = SQLDatabase.ExcDataTable("select id,nganhnghechinh2 from vinabiz where danhmucid =0 and danhmucbyVnbiz=''and nganhnghechinh2<>''");

                lbl_message.Text = string.Empty;
                lbl_message.Font = new Font(lbl_message.Font, FontStyle.Regular);
                lbl_message.Update();
                int total = _tbAll.Rows.Count;
                pr.Value = 0;
                pr.Maximum = total;
                lbl_phantram.Text = "0% Hoàn thành...";
                lbl_phantram.Update();
                vinabiz model = new vinabiz();

                int i = 0;
                int tongcong = _tbAll.Rows.Count;
                foreach (DataRow item in _tbAll.Rows)
                {
                    string strdmnganhnghe = item["nganhnghechinh2"].ToString();
                    dm_hsct dm = Utilities_vinabiz.getIDKiemTraGanDung(strdmnganhnghe.Trim());
                    if (dm != null)
                    {
                        pr.PerformStep();
                        pr.Update();
                        model.id = Convert.ToInt32(item["id"]);
                        model.danhmucid = dm.id;
                        model.danhmucbyVnbiz = dm.name;

                        lbl_message.Text = string.Format("Đang xử lý: {0}/{1}    id: {2} -> {3}", i.ToString("#,##0"), tongcong.ToString("#,##0"), item["id"], item["nganhnghechinh2"].ToString());
                        lbl_message.Update();

                        i++;
                        lbl_phantram.Text = Math.Round((i / (double)total) * 100, 0) + "% Hoàn thành...";
                        lbl_phantram.Update();

                        SQLDatabase.UpdateChuanHoaNganhNgheChinh(model);
                    }
                    else {
                        /*lưu log lai truong hop khong map dc;de doi ky thuat kiem tra*/
                        MessageBox.Show(string.Format("Không map được Ngành Nghề '{0}'", strdmnganhnghe), "Thông Báo");
                        writer = LogWriter.Instance;
                        writer.WriteToLog(string.Format("Không map được Ngành Nghề '{0}'   ->{1}", strdmnganhnghe, "ProcessChuanhoaNganhNgheChinh"));
                    }
                    if (!hasProcess) break;
                }
                /*===================================================================*/
                if (hasProcess)
                {
                    lbl_message.Text = "Đang thống kê lại số lượng ngành nghề .....";
                    lbl_message.Font = new Font(lbl_message.Font, FontStyle.Bold);
                    lbl_message.Update();

                    StorePhone.SQLDatabase.ExcNonQuery("update dm_hsct set tongsl = dbo.fnGetSoLuongVinabizByNganhNghe(id)");

                    lbl_message.Text = string.Empty;
                    lbl_message.Font = new Font(lbl_message.Font, FontStyle.Regular);
                    lbl_message.Update();

                }

                lbl_message.Text = hasProcess ? "Hoàn thành chuẩn hoá số liệu." : "Tạm dừng do người dùng!!! ";


                button1.Text = "Xử Lý-Start";
                lbl_message.Visible = true;
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox5.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ProcessChuanhoa");
            }
        }

        private void btn_vinabiz_Them_Click(object sender, EventArgs e)
        {
            try
            {
                int id = 0, tongsl = 0;
                string name = "", ma = ""; 
                foreach (DataGridViewRow row in gw_vinabiz_goc.SelectedRows)
                {
                    DataRow[] result = _table_vinabiz.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_goc"].Value)));

                    int khuvuc = radCha.Checked ? 0 :  1;
                    if (result.Count() == 0 && row.Cells["alevel"].Value.ToString().Equals(khuvuc.ToString()))
                    {
                        id = ConvertType.ToInt(row.Cells["id_goc"].Value);
                        ma = row.Cells["maid_goc"].Value.ToString();
                        name = row.Cells["name_goc"].Value.ToString();
                        tongsl = ConvertType.ToInt(row.Cells["tongsl_goc"].Value.ToString()); 
                        _table_vinabiz.Rows.Add(id,ma, name,tongsl);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

        private void gw_vinabiz_goc_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.gw_vinabiz_goc.Rows)
            {
                if (ConvertType.ToInt(row.Cells["alevel"].Value) == 0 )
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
                }
            }
        }

        private void btn_vinabiz_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                _table_vinabiz.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button4_Click");
            }
        }

        private void radCha_CheckedChanged(object sender, EventArgs e)
        {
            btn_vinabiz_Xoa_Click(null, null);
        }

        private void btn_vinabiz_Bo_Click(object sender, EventArgs e)
        {
            try
            {
                if (gw_vinabiz_chon.SelectedRows.Count == 0) return;
                foreach (DataGridViewRow row in gw_vinabiz_chon.SelectedRows)
                {
                    _table_vinabiz.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_chon"].Value)))[0].Delete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button3_Click");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strtiteude = string.Format("Bạn có chắc chắn thống kê lại số liệu danh mục {0} của vinabiz không, theo ?" , radioButton9.Checked? "Tỉnh Thành": "Ngành Nghề");
            if (MessageBox.Show(strtiteude, "Thông Báo",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                PleaseWait objPleaseWait = null;
                //TODO: Stuff
                objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();
                if (radioButton9.Checked)
                    StorePhone.SQLDatabase.ExcDataTable("update dm_Tinh set tongsl = dbo.fnGetSoLuongVinabizByTinhThanh(id)");
                else
                    StorePhone.SQLDatabase.ExcDataTable("update dm_hsct set tongsl = dbo.fnGetSoLuongVinabizByNganhNghe(id)");
                objPleaseWait.Close();
                MessageBox.Show(string.Format("Thống kê Số Lượng theo {0} của vinabiz hoàn tất",radioButton9.Checked?"Tỉnh Thành":"Ngành Nghề"), "Thông Báo");
                    frmExportVinabiz_Load(null, null);
            }
        }

        private void checkedListBox1_DoubleClick(object sender, EventArgs e)
        {
            int id= ConvertType.ToInt(((System.Data.DataRowView)checkedListBox1.SelectedItem).Row["id"]);
            string name = ((System.Data.DataRowView)checkedListBox1.SelectedItem).Row["ten"].ToString();
            frmDialogColumn frm = new frmDialogColumn();
            frm.FromParent = "vinabiz.org";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                xuatfile(frm.strColumn, Helpers.convertToUnSign3(name), id, true);
            }
        }

        private void gw_vinabiz_goc_DoubleClick(object sender, EventArgs e)
        {
            if (gw_vinabiz_goc.SelectedRows.Count > 0)
            {
                DataGridViewRow row = gw_vinabiz_goc.SelectedRows[0];
                int id = ConvertType.ToInt(row.Cells["id_goc"].Value);
                string ma = row.Cells["maid_goc"].Value.ToString();
                string name = row.Cells["name_goc"].Value.ToString();
                string tongsl = row.Cells["tongsl_goc"].Value.ToString();
               


                string strTieuDe = string.Format("{0}_{1}_{2}", ma,
                                         Regex.Replace(Helpers.convertToUnSign3(name), @"\d", "").Replace("-", "").Trim(),
                                      ConvertType.ToInt(tongsl).ToString("N0").Replace(',', '.'));

                frmDialogColumn frm = new frmDialogColumn();
                frm.FromParent = "vinabiz.org";
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    xuatfile(frm.strColumn, strTieuDe, id, false);
                }
            }
        }
    }
}
