using Newtonsoft.Json;
using StorePhone;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Load
{

    public partial class Main : Form
    {
        private LogWriter writer;
        private Thread theardProcess;
        private DataTable _table_hsct;
        private DataTable _table_vatgia;
        private DataTable _table_trangvang;
        private DataTable _table_batdongsan;
        private DataTable _table_vinabiz;
        private DataTable _table_thitruongsi;

        private LinkQueues2 _queue;
        private Dictionary<int, MyClass> dmDangChay;

        private string _username;
        private int _quyenid;
        /*scanner*/
        private const int sleep_bat = 10;
        private const int timeout_bat = 500;
        private const int lanlap_bat = 1;
        private const int soluong_bat = 1;
        private const int dosau_bat = 1;
        private const int gioihan_bat = 50;
        private const int sotrang_nhay = 1;

        //scanec phone & email
        private string _domain;
        private string _comboxnhom;

        private string _strDatabase;

        /*******06/2017(Mua Ban/Tim viec)***********/
        public string _strPath = "https://muaban.net/";
        infoPathmuaban _modelTrang = new infoPathmuaban();

        /*******06/2017(bất động sản)*************/
        
        /***********06/2017(vinabiz)************/
        private List<dm_hsct> _listHsct = new List<dm_hsct>();
        private List<dm_Tinh> _listdm_Tinh = new List<dm_Tinh>();
        DataTable _danhmucVinabiz;

        /**********07/2017 (thitruongsi.com) ****************/
        WebBrowser wThiTruongSi = new WebBrowser();

        #region Hệ Thống
        public Main()
        {
            try
            {
                InitializeComponent();
                writer = LogWriter.Instance;





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Form1_Load");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                frmLogin frmlogin = new frmLogin();
                if (frmlogin.ShowDialog() == DialogResult.OK)
                {

                    _username = frmlogin.UserName;
                    _quyenid = frmlogin.RightID;

                    menuStrip1.Enabled = _quyenid == 1 ? true : false;
                    label4.Text = string.Format("Xin Chào : {0}- Đổi mật khẩu", _username);
                }
                else
                {
                    this.Close();
                }

                cmb_scannce_UserAgent.SelectedIndex = 0;
                BindDmNhom();
                BindDmNhomScane();
                BinddmVatgia();
                BinddmTrangVang();
                _strDatabase = StorePhone.SQLDatabase.ExcDataTable(string.Format("SELECT DB_NAME(0)AS [DatabaseName]; ")).Rows[0]["DatabaseName"].ToString();
                this.Text = string.Format("DATABASE- Kết nối : {0}", System.Configuration.ConfigurationSettings.AppSettings.Get("ConnectionString"));


                dmDangChay = new Dictionary<int, MyClass>();
                dmDangChay.Add(1, new MyClass("hosocongty.vn", false, false));
                dmDangChay.Add(2, new MyClass("trangvangvietnam.com", false, false));
                dmDangChay.Add(3, new MyClass("vatgia.com", false, false));
                dmDangChay.Add(4, new MyClass("phone_email", false, false));
                dmDangChay.Add(5, new MyClass("muaban.net", false, false));
                dmDangChay.Add(6, new MyClass("batdongsan.com.vn", false, false));
                dmDangChay.Add(7, new MyClass("vinabiz.org", false, false));
                dmDangChay.Add(8, new MyClass("thitruongsi.com", false, false));

                _table_hsct = CreateTable_hsct();
                gw_hosocongty.DataSource = null;
                gw_hosocongty.DataSource = _table_hsct;
                gw_hosocongty.Sort(gw_hosocongty.Columns["ma_chon"], ListSortDirection.Ascending);

                _table_vatgia = CreateTable_vatgia();
                gw_mathang_chon.DataSource = null;
                gw_mathang_chon.DataSource = _table_vatgia;
                gw_mathang_chon.Sort(gw_mathang_chon.Columns["id_vattu_chon"], ListSortDirection.Ascending);

                _table_trangvang = CreateTable_trangvang();
                gv_trangvang_chon.DataSource = null;
                gv_trangvang_chon.DataSource = _table_trangvang;
                gv_trangvang_chon.Sort(gv_trangvang_chon.Columns["id_trangvang_chon"], ListSortDirection.Ascending);


                /*-----------------------*/
                Utilities_muaban._Timeout = ConvertType.ToInt(txt_muaban_tv_timeout.Text);
                Utilities_muaban._Sleep = ConvertType.ToInt(txt_muaban_tv_sleep.Text);

                BindDanhMuc();
                BindmuabanMucLuongDen();
                BindmuabanMucLuongTu();
                BindmuabanHinhThuc();
                BindmuabanThoiGianDang();
                /*-------bất động sản----*/
                Binddmbds();

                _table_batdongsan = CreateTable_batdongsan();
                gw_bds_chon.DataSource = _table_batdongsan;
                gw_bds_chon.Sort(gw_bds_chon.Columns["id_bds_chon"], ListSortDirection.Ascending);
                /*---------vinabiz-----*/
                getdataVinabiz();
                _listHsct = SQLDatabase.Loaddm_hsct("select * from dm_hsct where path<>''");
                _listdm_Tinh = SQLDatabase.Loaddm_tinh("select * from dm_tinh");
                BinddmVinabiz();
                _table_vinabiz = CreateTable_vinabiz();
                gw_vinabiz_chon.DataSource = _table_vinabiz;
                gw_vinabiz_chon.Sort(gw_vinabiz_chon.Columns["id_vnbiz_chon"], ListSortDirection.Ascending);

                /*thi trường sỉ*/
                Binddmthitruongsi();

                _table_thitruongsi = CreateTable_vinabiz();
                gw_si_chon.DataSource = _table_thitruongsi;
                gw_si_chon.Sort(gw_si_chon.Columns["id_si_chon"], ListSortDirection.Ascending);



            }
            catch (Exception ex)
            {
                this.Close();
            }

        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (_table_hsct.Rows.Count == 0 && _table_trangvang.Rows.Count == 0 &&  _table_vatgia.Rows.Count == 0 &&   _table_batdongsan.Rows.Count ==0 &&  _table_vinabiz.Rows.Count ==0)
            if(MessageBox.Show("Bạn có chắc kết thúc chương trình không?", "Thông Báo",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                //TODO: Stuff
                e.Cancel = true;
                return;
            }

            Utilities_trangvang.hasProcess = false;
            Utilities_scanner.hasProcess = false;
            Utilities_vatgia.hasProcess = false;
            Utilities_hosocongty.hasProcess = false;
            Utilities_muaban.hasProcess = false;
            Utilities_batdongsan.hasProcess = false;
            Utilities_vinabiz.hasProcess = false;

            if (_table_hsct.Rows.Count != 0 || 
                _table_trangvang.Rows.Count != 0 || 
                _table_vatgia.Rows.Count != 0 ||
                _table_batdongsan.Rows.Count!=0 ||
                _table_vinabiz.Rows.Count!=0)
            {
                if ((MessageBox.Show("Lưu file trước khi đóng chương trình không?", "Thông Báo",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                {
                    SavaAs_hsct();
                    SavaAs_trangvang();
                    SavaAs_vatgia();
                    SavaAs_batdongsan();
                }
            }


        }
        private void label4_Click(object sender, EventArgs e)
        {
            frmChangPassword frm = new frmChangPassword();
            frm.UserName = _username;
            frm.ShowDialog();
        }
        private void cấuHìnhKếtNốiToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void câuHinhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCauHinh frm = new frmCauHinh();
            frm.ShowDialog();
        }
        #endregion

        #region Bind danh muc hồ sơ công ty

        public DataTable CreateTable_hsct()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id_chon", typeof(int));
            table.Columns.Add("path_chon", typeof(string));
            table.Columns.Add("cap_id_chon", typeof(int));
            table.Columns.Add("group_id_chon", typeof(string));
            table.Columns.Add("name_chon", typeof(string));
            table.Columns.Add("ma_chon", typeof(string));
            table.Columns.Add("createdate_chon", typeof(DateTime));
            return table;
        }
        public DataTable CreateTable_vatgia()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id_chon", typeof(int));
            table.Columns.Add("name_chon", typeof(string));
            table.Columns.Add("path_chon", typeof(string));
            table.Columns.Add("parentid_chon", typeof(string));
            table.Columns.Add("orderid_chon", typeof(string));
            return table;
        }
        public void BindDMHosocongty_new(object nhom)
        {
            if (nhom == null) return;
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
            gw_hosocongty_goc.DataSource = tb_new;
        }

        public void BindDmNhom()
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("select id,name,capid,parentid from dm_hsct where capid=1 or capid=2"));
            DataTable tb_new = table.Clone();
            tb_new.Clear();
            tb_new.Rows.Add(-1, "---Chọn Nhóm Gian Hàng---");

            foreach (DataRow item in table.Select("capid=1"))
            {
                tb_new.ImportRow(item);
                foreach (DataRow item2 in table.Select(string.Format("capid='{0}' and parentid='{1}'", 2, item["id"])))
                {
                    tb_new.ImportRow(item2);
                }
            }
            cmbNhom.DataSource = tb_new;
            cmbNhom.ValueMember = "id";
            cmbNhom.DisplayMember = "name";
            cmbNhom.SelectedValue = -1;
            cmbNhom_SelectedIndexChanged(null, null);

        }

        private void ProcessHosocongty(object arrControl)
        {
            try
            {
                //----- Add control process from

                ArrayList arr1 = (ArrayList)arrControl;
                Label lbl_thongbao_hosocongty = (Label)arr1[0];
                Label lbl_thongbao_hosocongty1 = (Label)arr1[1];
                Label lbl_hsct_khoa = (Label)arr1[2];

                //----- update display control
                lbl_thongbao_hosocongty.Update();
                lbl_thongbao_hosocongty1.Update();
                lbl_hsct_khoa.Update();
                /*================================================================*/
                int x = 0;
                while (gw_hosocongty.Rows.Count > 0)
                {
                    if (!Utilities_hosocongty.hasProcess) return;
                    int id = Convert.ToInt32(gw_hosocongty.Rows[x].Cells["id_chon"].Value);
                    string strPath = gw_hosocongty.Rows[x].Cells["path_chon"].Value.ToString().Replace(" ", "").Replace("\t", "");
                    string nganh = gw_hosocongty.Rows[x].Cells["name_chon"].Value.ToString();

                    lbl_thongbao_hosocongty1.Text = string.Format("Đang Xử Lý:{0}", nganh);
                    lbl_thongbao_hosocongty1.Update();


                    int solanlap = 0;
                    Utilities_hosocongty.IdDanhmuc = id;
                    Utilities_hosocongty.ChkCKiemTraTrung = chk_kiemtratrung_hsct.Checked;
                    Utilities_hosocongty.getwebBrowser(strPath, null, ref solanlap, lbl_thongbao_hosocongty, lbl_hsct_khoa);

                    if (Utilities_hosocongty.hasProcess)
                        if (_table_hsct.Select(string.Format("id_chon={0}", id)).Count() != 0)
                            _table_hsct.Select(string.Format("id_chon={0}", id))[0].Delete();
                }
                /*================================================================*/

                button1.Text = "Start";
                Utilities_hosocongty.hasProcess = false;
                Utilities_hosocongty._thanhcong = 0;
                Utilities_hosocongty._thatbai = 0;
                setTitleWindow(1, false);

                button2.Enabled = true;
                button4.Enabled = true;
                button3.Enabled = true;
                cmbNhom.Enabled = true;
                txt_trang_hosocongty.Enabled = true;
                chk_tatcatrang_hosocongty.Enabled = true;

                gw_hosocongty.Enabled = true;
                gw_hosocongty_goc.Enabled = true;

                pic_hsct_in.Enabled = true;
                pic_hsct_out.Enabled = true;

                btn_hosocongty_sotrang_giam.Enabled = true;
                btn_hosocongty_sotrang_tang.Enabled = true;
                txt_trang_hosocongty.Enabled = true;

                txt_hosocongty_sleep.Enabled = true;
                btn_hosocongty_sleep_giam.Enabled = true;
                btn_hosocongty_sleep_tang.Enabled = true;

                txt_hosocongty_lanlap.Enabled = true;
                btn_hosocongty_lanlap_giam.Enabled = true;
                btn_hosocongty_lanlap_tang.Enabled = true;

                txt_hosocongty_timeout.Enabled = true;
                btn_hosocongty_timerout_giam.Enabled = true;
                btn_hosocongty_timerout_tang.Enabled = true;


                chk_kiemtratrung_hsct.Enabled = true;
                if (chk_tatcatrang_hosocongty.Checked)
                {
                    txt_trang_hosocongty.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "ProcessHosocongty", "error1"));
                MessageBox.Show(ex.Message, "ProcessHosocongty");
            }
        }

        #endregion

        #region bind Vật Gía
        public void BinddmVatgia()
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("select ID,NAME from dm_vatgia where parentId is null "));
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(int));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add(-1, "---Chọn Nhóm Gian Hàng---");
            foreach (DataRow item in table.Rows)
                table_nhom.Rows.Add(item["id"], item["name"]);

            cmb_nhomhang.DataSource = table_nhom;
            cmb_nhomhang.ValueMember = "id";
            cmb_nhomhang.DisplayMember = "name";
            cmb_nhomhang.SelectedValue = -1;
        }
        public void BindVatGia(object nhom)
        {
            if (nhom == null)
                return;
            if (nhom.ToString() == "System.Data.DataRowView")
                return;
            DataTable table = SQLDatabase.ExcDataTable(string.Format("[spFindDmvatgia] '{0}'", nhom));
            IEnumerable<DataRow> query = from order in table.AsEnumerable()
                                         .Where(s => s.Field<string>("path").Length == 0)
                                         select order;

            gw_mathang_goc.DataSource = table;
        }
        #endregion

        #region hosocongty
        private void chk_hosocongty_lanlap_CheckedChanged(object sender, EventArgs e)
        {
            txt_hosocongty_lanlap.Enabled = !chk_hosocongty_lanlap.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ParameterizedThreadStart par;
            ArrayList arr;
            try
            {
                if (gw_hosocongty.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ngành nghề kinh doanh", "Thông báo");
                    return;
                }
                Utilities_hosocongty._Timeout = ConvertType.ToInt(txt_hosocongty_timeout.Text);
                Utilities_hosocongty._Sleep = ConvertType.ToInt(txt_hosocongty_sleep.Text);


                if (button1.Text == "Start")
                {
                    button1.Text = "Stop";

                    setTitleWindow(1, true);

                    Utilities_hosocongty.hasProcess = true;
                    button2.Enabled = false;
                    button4.Enabled = false;
                    button3.Enabled = false;
                    cmbNhom.Enabled = false;
                    chk_kiemtratrung_hsct.Enabled = false;
                    txt_trang_hosocongty.Enabled = false;
                    chk_tatcatrang_hosocongty.Enabled = false;

                    lbl_thongbao_hosocongty.Visible = true;
                    lbl_thongbao_hosocongty_1.Visible = true;

                    gw_hosocongty.Enabled = false;
                    gw_hosocongty_goc.Enabled = false;

                    pic_hsct_in.Enabled = false;
                    pic_hsct_out.Enabled = false;

                    btn_hosocongty_sotrang_giam.Enabled = false;
                    btn_hosocongty_sotrang_tang.Enabled = false;
                    txt_trang_hosocongty.Enabled = false;

                    txt_hosocongty_sleep.Enabled = false;
                    btn_hosocongty_sleep_giam.Enabled = false;
                    btn_hosocongty_sleep_tang.Enabled = false;

                    txt_hosocongty_lanlap.Enabled = false;
                    btn_hosocongty_lanlap_giam.Enabled = false;
                    btn_hosocongty_lanlap_tang.Enabled = false;

                    txt_hosocongty_timeout.Enabled = false;
                    btn_hosocongty_timerout_giam.Enabled = false;
                    btn_hosocongty_timerout_tang.Enabled = false;


                    /*kiem tra neu checkpath dc chon thi luu pathlimit vao gioi hang*/
                    if (!chk_tatcatrang_hosocongty.Checked)
                        Utilities_hosocongty._PathLimit = ConvertType.ToInt(txt_trang_hosocongty.Text);
                    else
                        Utilities_hosocongty._PathLimit = -1;

                    if (!chk_hosocongty_lanlap.Checked)
                        Utilities_hosocongty._lanquetlai = ConvertType.ToInt(txt_hosocongty_lanlap.Text);
                    else
                        Utilities_hosocongty._lanquetlai = -1;


                }
                else
                {
                    button1.Text = "Start";
                    setTitleWindow(1, false);

                    Utilities_hosocongty.hasProcess = false;
                    Utilities_hosocongty._thanhcong = 0;
                    Utilities_hosocongty._thatbai = 0;

                    button2.Enabled = true;
                    button4.Enabled = true;
                    button3.Enabled = true;
                    cmbNhom.Enabled = true;
                    txt_trang_hosocongty.Enabled = true;
                    chk_tatcatrang_hosocongty.Enabled = true;

                    gw_hosocongty.Enabled = true;
                    gw_hosocongty_goc.Enabled = true;

                    pic_hsct_in.Enabled = true;
                    pic_hsct_out.Enabled = true;

                    btn_hosocongty_sotrang_giam.Enabled = true;
                    btn_hosocongty_sotrang_tang.Enabled = true;
                    txt_trang_hosocongty.Enabled = true;

                    txt_hosocongty_sleep.Enabled = true;
                    btn_hosocongty_sleep_giam.Enabled = true;
                    btn_hosocongty_sleep_tang.Enabled = true;

                    txt_hosocongty_lanlap.Enabled = true;
                    btn_hosocongty_lanlap_giam.Enabled = true;
                    btn_hosocongty_lanlap_tang.Enabled = true;

                    txt_hosocongty_timeout.Enabled = true;
                    btn_hosocongty_timerout_giam.Enabled = true;
                    btn_hosocongty_timerout_tang.Enabled = true;


                    chk_kiemtratrung_hsct.Enabled = true;
                    if (chk_tatcatrang_hosocongty.Checked)
                    {
                        txt_trang_hosocongty.Enabled = false;
                    }
                }

                /*Cấu hình controll*/
                Control.CheckForIllegalCrossThreadCalls = false;

                par = new ParameterizedThreadStart(ProcessHosocongty);
                theardProcess = new Thread(par);

                arr = new ArrayList();
                arr.Add(lbl_thongbao_hosocongty);
                arr.Add(lbl_thongbao_hosocongty_1);
                arr.Add(lbl_hsct_khoa);

                //http://stackoverflow.com/questions/3542061/how-do-i-stop-a-thread-when-my-winform-application-closes
                theardProcess.IsBackground = true;
                theardProcess.Start(arr);
            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "button2_Click", "error1"));
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

        /// <summary>
        /// Hiễn thị thông tin các tab đang chạy trên title window
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isadd"></param>
        private void setTitleWindow(int key, bool isadd)
        {
            try
            {
                dmDangChay[key] = new MyClass(dmDangChay[key].name, isadd, true);

                string strghep = "";
                string title = "DATABASE-  ";
                foreach (MyClass item in dmDangChay.Values)
                {
                    if (item.show == true)
                        strghep += string.Format(" {0}-[{1}]  ", item.check ? "Chạy" : "Dừng", item.name);
                }
                this.Text = title + strghep;
            }
            catch { }
        }
        private void gw_hosocongty_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            label2.Text = string.Format("Danh Sách Ngành Nghề Quét: {0}", gw_hosocongty.Rows.Count);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txt_trang_hosocongty.Enabled = !chk_tatcatrang_hosocongty.Checked;
        }
        private int getIndexGridview_hsct(int id)
        {
            int i = 0;
            foreach (DataRow item in _table_hsct.Rows)
            {
                if (item["id_chon"].ToString() == id.ToString())
                {
                    return i;
                }
                i++;

            }
            return i;
        }
        private void danhMucToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmDM_hosocongty frm = new frmDM_hosocongty();
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
        private void cmbNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDMHosocongty_new(cmbNhom.SelectedValue);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void gw_hosocongty_goc_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.gw_hosocongty_goc.Rows)
            {
                if (row.Cells["path_goc"].Value == null || row.Cells["path_goc"].Value.ToString() == "")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
                }

            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int id = 0;
                string group_id = "";
                string name = "";
                string path = "";
                string cap_id = "";
                string ma_chon = "";
                DateTime createdate;
                foreach (DataGridViewRow row in gw_hosocongty_goc.SelectedRows)
                {
                    DataRow[] result = _table_hsct.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_goc"].Value)));
                    /*kiểm tra xem thông tin chọn đã có trong chọn chưa*/
                    if (result.Count() == 0 && row.Cells["path_goc"].Value.ToString().Length != 0)/*kiễm tra trong gw chọn chưa tồn tại*/
                    {
                        id = ConvertType.ToInt(row.Cells["id_goc"].Value);
                        group_id = row.Cells["parentId"].Value.ToString();
                        path = row.Cells["path_goc"].Value.ToString();
                        cap_id = row.Cells["capid"].Value.ToString();
                        name = row.Cells["name_goc"].Value.ToString();
                        ma_chon = row.Cells["maid"].Value.ToString();
                        createdate = ConvertType.ToDateTime(row.Cells["createdate"].Value);
                        _table_hsct.Rows.Add(id, path, cap_id, group_id, name, ma_chon, createdate);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                _table_hsct.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button4_Click");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (gw_hosocongty.SelectedRows.Count == 0) return;
                foreach (DataGridViewRow row in gw_hosocongty.SelectedRows)
                {
                    _table_hsct.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_chon"].Value)))[0].Delete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button3_Click");
            }
        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmExporthosocongty frm = new frmExporthosocongty();
                //frm.FromParent = "thitruongsi.com";
                frm.strDatabaseName = _strDatabase;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (SavaAs_hsct())
                MessageBox.Show("Lưu dữ liệu thành công", "Thông Báo");
        }

        private bool SavaAs_hsct()
        {
            PleaseWait objPleaseWait = null;

            if (_table_hsct.Rows.Count > 0)
            {
                objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();
                SaveAs sa = new SaveAs();

                foreach (DataRow item in _table_hsct.Rows)
                {
                    DataTable tb = SQLDatabase.ExcDataTable(string.Format("select count(*) from SaveAs where scannerby='{0}' and path_chon='{1}'", "hosocongty.vn", item["path_chon"].ToString()));
                    if (ConvertType.ToInt(tb.Rows[0][0]) == 0)
                        SQLDatabase.AddSaveAs(new SaveAs()
                        {
                            id_chon = item["id_chon"].ToString(),
                            name_chon = item["name_chon"].ToString(),
                            path_chon = item["path_chon"].ToString(),
                            ma_chon = item["ma_chon"].ToString(),
                            cap_id_chon = item["cap_id_chon"].ToString(),
                            scannerby = "hosocongty.vn",
                        });
                }
                if (objPleaseWait != null)
                    objPleaseWait.Close();
                return true;
            }
            else
                return true;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    List<SaveAs> save = new List<SaveAs>();
                    frmSaveAs frm = new frmSaveAs();
                    frm.fromparent = "hosocongty.vn";
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        save = frm.Saveas;
                        if (save.Count() > 0)
                        {
                            foreach (SaveAs item in save)
                            {
                                if (_table_hsct.Select(string.Format("path_chon = '{0}'", item.path_chon)).Count() == 0)
                                    _table_hsct.Rows.Add(ConvertType.ToInt(item.id_chon), item.path_chon, ConvertType.ToInt(item.cap_id_chon), item.group_id_chon, item.name_chon, item.ma_chon, DateTime.Now);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "danhMucToolStripMenuItem1_Click");
                }
                #region
                //openFileDialog1.Filter = "XML file (*.xml)|*.xml";
                //openFileDialog1.AddExtension = true;
                //openFileDialog1.RestoreDirectory = true;
                //openFileDialog1.Title = "Where do you want to save the file?";
                //openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                //if (openFileDialog1.ShowDialog() == DialogResult.OK) {

                //    XmlDocument xmlDoc = new XmlDocument();
                //    xmlDoc.Load(openFileDialog1.FileName);
                //    /*thông tin hệ thống*/
                //    XmlNodeList nodeListSys = xmlDoc.DocumentElement.SelectNodes("/Table/System");
                //    string KeyFile = nodeListSys[0].SelectSingleNode("KeyFile").InnerText;
                //    string cmdchon = nodeListSys[0].SelectSingleNode("IndexDM").InnerText;
                //    string SoTrang = nodeListSys[0].SelectSingleNode("SoTrang").InnerText;

                //    if (KeyFile != "hsct") {
                //        MessageBox.Show("File xml không đúng dịnh dạng data 'Hồ Sơ Công Ty'. \n Không đúng file xml data hồ sơ công ty", "Thông Báo");
                //        return;
                //    }
                //    cmbNhom.SelectedValue = ConvertType.ToInt(cmdchon);
                //    txt_trangvang_page.Text = SoTrang;
                //    if (txt_trangvang_page.Text == "-1")
                //        chk_tatcatrang_hosocongty.Checked = true;
                //    else
                //        chk_tatcatrang_hosocongty.Checked = false;


                //    _table_hsct.Clear();
                //    XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Table/AutoLoad");
                //    string pID = "", pName = "", pPath = "", pMa = "", pCap = "", pOrderid = "";
                //    foreach (XmlNode node in nodeList) {
                //        pID = node.SelectSingleNode("id").InnerText;
                //        pName = node.SelectSingleNode("name").InnerText;
                //        pPath = node.SelectSingleNode("path").InnerText;
                //        pMa = node.SelectSingleNode("ma").InnerText;
                //        pCap = node.SelectSingleNode("cap").InnerText;
                //        pOrderid = node.SelectSingleNode("OrderId").InnerText;

                //        _table_hsct.Rows.Add(pID, pPath, pCap, null, pName, pMa, null);

                //    }

                //}
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("File xml không đúng định dạng\n" + ex.Message, "button5_Click_1");

            }
        }
        private void gw_hosocongty_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //e.ThrowException = false;
            e.Cancel = true;
        }

        private void btn_hosocongty_sotrang_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_trang_hosocongty.Text) <= 1)
                    return;
                txt_trang_hosocongty.Text = (ConvertType.ToInt(txt_trang_hosocongty.Text) - sotrang_nhay).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_hosocongty_sotrang_giam_Click");
            }
        }

        private void btn_hosocongty_sotrang_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_trang_hosocongty.Text) >= 20)
                    return;
                txt_trang_hosocongty.Text = (ConvertType.ToInt(txt_trang_hosocongty.Text) + sotrang_nhay).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_hosocongty_sotrang_tang_Click");
            }
        }

        private void btn_hosocongty_sleep_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_hosocongty_sleep.Text) <= 0)
                    return;
                txt_hosocongty_sleep.Text = (ConvertType.ToInt(txt_hosocongty_sleep.Text) - sleep_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_hosocongty_sleep_giam_Click");
            }
        }

        private void btn_hosocongty_sleep_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_hosocongty_sleep.Text) >= 1000)
                    return;
                txt_hosocongty_sleep.Text = (ConvertType.ToInt(txt_hosocongty_sleep.Text) + sleep_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_sleep_tang_Click");
            }
        }

        private void btn_hosocongty_timerout_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_hosocongty_timeout.Text) <= 0)
                    return;
                txt_hosocongty_timeout.Text = (ConvertType.ToInt(txt_hosocongty_timeout.Text) - timeout_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_hosocongty_timerout_giam_Click");
            }
        }

        private void btn_hosocongty_timerout_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_hosocongty_timeout.Text) >= 50000)
                    return;
                txt_hosocongty_timeout.Text = (ConvertType.ToInt(txt_hosocongty_timeout.Text) + timeout_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_hosocongty_timerout_tang_Click");
            }
        }
        #endregion

        #region Vật giá

        private void cmb_nhomhang_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindVatGia(cmb_nhomhang.SelectedValue);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "cmb_nhomhang_SelectedIndexChanged");
            }
        }
        private void btn_chon_vatgia_Click(object sender, EventArgs e)
        {
            try
            {
                int id = 0;
                string parentId = "";
                string name = "";
                string path = "";
                string orderid = "";
                foreach (DataGridViewRow row in gw_mathang_goc.SelectedRows)
                {
                    DataRow[] result = _table_vatgia.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_vatgia_chon"].Value)));

                    if (result.Count() == 0 && row.Cells["path_vatgia_goc"].Value != null)
                    {
                        id = ConvertType.ToInt(row.Cells["id_vatgia_chon"].Value);
                        name = row.Cells["name_vatgia"].Value.ToString();
                        path = row.Cells["path_vatgia_goc"].Value == null ? "" : row.Cells["path_vatgia_goc"].Value.ToString();
                        parentId = row.Cells["parentId_vatgia"].Value == null ? "" : row.Cells["parentId_vatgia"].Value.ToString();
                        orderid = row.Cells["orderid_vatgia"].Value == null ? "" : row.Cells["orderid_vatgia"].Value.ToString();

                        _table_vatgia.Rows.Add(id, name, path, parentId, orderid);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }
        private void btn_vatgia_xoa_Click(object sender, EventArgs e)
        {
            try
            {
                _table_vatgia.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button4_Click");
            }
        }
        private void btn_vatgia_bo_Click(object sender, EventArgs e)
        {
            try
            {
                if (gw_mathang_chon.SelectedRows.Count == 0) return;
                foreach (DataGridViewRow row in gw_mathang_chon.SelectedRows)
                {
                    _table_vatgia.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_vattu_chon"].Value)))[0].Delete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button3_Click");
            }
        }
        private void ProcessVatGia(object arrControl)
        {
            try
            {
                //----- Add control process from
                ArrayList arr1 = (ArrayList)arrControl;
                Label lbl_vatgia_message1 = (Label)arr1[0];
                Label lbl_vatgia_message2 = (Label)arr1[1];
                Label lbl_vtagia_khoa = (Label)arr1[2];

                //----- update display control
                lbl_vatgia_message1.Update();
                lbl_vatgia_message2.Update();
                lbl_vtagia_khoa.Update();

                /*===============================================================*/
                int x = 0;
                while (gw_mathang_chon.Rows.Count > 0)
                {
                    if (!Utilities_vatgia.hasProcess) break;
                    int id = Convert.ToInt32(gw_mathang_chon.Rows[x].Cells["id_vattu_chon"].Value);
                    string strPath = gw_mathang_chon.Rows[x].Cells["path_vatgia_chon"].Value.ToString().Replace(" ", "").Replace("\t", "");
                    string strnganh = gw_mathang_chon.Rows[x].Cells["name_vatgia_chon"].Value.ToString();


                    Utilities_vatgia.IdDanhmuc = id;
                    int pagemax = Utilities_vatgia.getPageMax(strPath, lbl_vtagia_khoa);
                    //int pagemax = 100;
                    int i = 0;
                    do
                    {
                        if (!Utilities_vatgia.hasProcess) break;
                        {
                            /*kiễm tra trang*/
                            if (Utilities_vatgia._PathLimit != -1 && Utilities_vatgia._PathLimit == i) break;
                            i = i + 1;

                            lbl_vatgia_message1.Text = string.Format("Ngành:{0} \n Trang: {1}\\ Tổng Trang:{2}", strnganh, i, pagemax + 1);
                            lbl_vatgia_message1.Update();

                            int solanlap = 0;
                            Utilities_vatgia.getwebBrowser(strPath, ref solanlap, lbl_vatgia_message2, lbl_vtagia_khoa);
                            strPath = string.Format("http://www.vatgia.com/home/shop.php?view=list&iCat=433&page={0}", i);
                        }
                    } while (i <= pagemax);
                    /*neu dang stop thi khong dc phep xoa*/
                    if (Utilities_vatgia.hasProcess)
                    {
                        if (_table_vatgia.Select(string.Format("id_chon={0}", id)).Count() != 0)
                            _table_vatgia.Select(string.Format("id_chon={0}", id))[0].Delete();
                    }
                }
                /*===================================================================*/
                lbl_vatgia_message1.Text = Utilities_vatgia.hasProcess ? "Hoàn thành load số liệu." : "Tạm dừng do người dùng!!!";
                btn_start_vatgia.Text = "Start";
                Utilities_vatgia.hasProcess = false;

                btn_chon_vatgia.Enabled = true;
                btn_vatgia_xoa.Enabled = true;
                btn_vatgia_bo.Enabled = true;
                cmb_nhomhang.Enabled = true;
                chk_vatgia_tatca.Enabled = true;
                txt_vatgia_trang.Enabled = true;
                //lbl_vatgia_message1.Visible = false;
                //lbl_vatgia_message2.Visible = false;
                //lbl_vtagia_khoa.Visible = false;
                gw_mathang_chon.Enabled = true;
                gw_mathang_goc.Enabled = true;

                pic_vatgia_in.Enabled = true;
                pic_vatgia_out.Enabled = true;

                btn_vatgia_sotrang_giam.Enabled = true;
                btn_vatgia_sotrang_tang.Enabled = true;

                txt_vatgia_lanlap.Enabled = true;
                btn_vatgia_lanlap_giam.Enabled = true;
                btn_vatgia_lanlap_tang.Enabled = true;

                txt_vatgia_sleep.Enabled = true;
                btn_vatgia_sleep_tang.Enabled = true;
                btn_vatgia_sleep_giam.Enabled = true;


                txt_vatgia_timeout.Enabled = true;
                btn_vatgia_timeout_giam.Enabled = true;
                btn_vatgia_timeout_tang.Enabled = true;


                if (chk_vatgia_tatca.Checked)
                {
                    txt_vatgia_trang.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "ProcessVatGia", "error1"));
                MessageBox.Show(ex.Message, "ProcessVatGia");
            }
        }
        private int getIndexGridview_vatgia(int id)
        {
            int i = 0;
            foreach (DataRow item in _table_vatgia.Rows)
            {
                if (item["id_chon"].ToString() == id.ToString())
                {
                    return i;
                }
                i++;

            }
            return i;
        }
        private void btn_start_vatgia_Click(object sender, EventArgs e)
        {
            ParameterizedThreadStart par;
            ArrayList arr;
            try
            {
                if (gw_mathang_chon.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ngành nghề kinh doanh", "Thông báo");
                    return;
                }
                if (btn_start_vatgia.Text == "Start")
                {
                    btn_start_vatgia.Text = "Stop";
                    setTitleWindow(3, true);

                    Utilities_vatgia.hasProcess = true;
                    btn_chon_vatgia.Enabled = false;
                    btn_vatgia_xoa.Enabled = false;
                    btn_vatgia_bo.Enabled = false;
                    cmb_nhomhang.Enabled = false;
                    chk_vatgia_tatca.Enabled = false;
                    txt_vatgia_trang.Enabled = false;
                    lbl_vatgia_message1.Visible = true;
                    lbl_vatgia_message2.Visible = true;
                    gw_mathang_chon.Enabled = false;
                    gw_mathang_goc.Enabled = false;

                    pic_vatgia_in.Enabled = false;
                    pic_vatgia_out.Enabled = false;

                    btn_vatgia_sotrang_giam.Enabled = false;
                    btn_vatgia_sotrang_tang.Enabled = false;

                    txt_vatgia_lanlap.Enabled = false;
                    btn_vatgia_lanlap_giam.Enabled = false;
                    btn_vatgia_lanlap_tang.Enabled = false;

                    txt_vatgia_sleep.Enabled = false;
                    btn_vatgia_sleep_tang.Enabled = false;
                    btn_vatgia_sleep_giam.Enabled = false;


                    txt_vatgia_timeout.Enabled = false;
                    btn_vatgia_timeout_giam.Enabled = false;
                    btn_vatgia_timeout_tang.Enabled = false;

                    /*kiem tra neu checkpath dc chon thi luu pathlimit vao gioi hang*/
                    if (!chk_vatgia_tatca.Checked)
                        Utilities_vatgia._PathLimit = ConvertType.ToInt(txt_vatgia_trang.Text);
                    else
                        Utilities_vatgia._PathLimit = -1;

                    /*load danh sách đầu số*/

                    DataTable tb_dausp = SQLDatabase.ExcDataTable("select distinct dauso dauso,lenght " +
                                                  "  from dau_so where dauso is not null and dauso <> ''");

                    Dictionary<string, int> dauso = new Dictionary<string, int>();
                    foreach (DataRow item in tb_dausp.Rows)
                    {
                        dauso.Add(item["dauso"].ToString(), ConvertType.ToInt(item["lenght"].ToString()));
                    }
                    Utilities_vatgia._dau_so = dauso;
                    Utilities_scanner._regexs = SQLDatabase.LoadRegexs("select * from Regexs order by OrderID desc");

                }
                else
                {
                    btn_start_vatgia.Text = "Start";
                    Utilities_vatgia.hasProcess = false;
                    setTitleWindow(3, false);

                    btn_chon_vatgia.Enabled = true;
                    btn_vatgia_xoa.Enabled = true;
                    btn_vatgia_bo.Enabled = true;
                    cmb_nhomhang.Enabled = true;
                    chk_vatgia_tatca.Enabled = true;
                    txt_vatgia_trang.Enabled = true;
                    //lbl_vatgia_message1.Visible = false;
                    lbl_vatgia_message2.Visible = false;
                    lbl_vtagia_khoa.Visible = false;
                    gw_mathang_chon.Enabled = true;
                    gw_mathang_goc.Enabled = true;

                    pic_vatgia_in.Enabled = true;
                    pic_vatgia_out.Enabled = true;

                    btn_vatgia_sotrang_giam.Enabled = true;
                    btn_vatgia_sotrang_tang.Enabled = true;

                    txt_vatgia_lanlap.Enabled = true;
                    btn_vatgia_lanlap_giam.Enabled = true;
                    btn_vatgia_lanlap_tang.Enabled = true;

                    txt_vatgia_sleep.Enabled = true;
                    btn_vatgia_sleep_tang.Enabled = true;
                    btn_vatgia_sleep_giam.Enabled = true;


                    txt_vatgia_timeout.Enabled = true;
                    btn_vatgia_timeout_giam.Enabled = true;
                    btn_vatgia_timeout_tang.Enabled = true;


                    if (chk_vatgia_tatca.Checked)
                    {
                        txt_vatgia_trang.Enabled = false;
                    }
                }

                /*Cấu hình controll*/
                Control.CheckForIllegalCrossThreadCalls = false;

                par = new ParameterizedThreadStart(ProcessVatGia);
                theardProcess = new Thread(par);

                arr = new ArrayList();
                arr.Add(lbl_vatgia_message1);
                arr.Add(lbl_vatgia_message2);
                arr.Add(lbl_vtagia_khoa);

                //http://stackoverflow.com/questions/3542061/how-do-i-stop-a-thread-when-my-winform-application-closes
                theardProcess.IsBackground = true;
                theardProcess.Start(arr);
            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "button2_Click", "error1"));
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button5_Click");
            }
        }
        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                frmExportvatgia frm = new frmExportvatgia();
                frm.strDatabaseName = _strDatabase;
                frm.FromParent = "vatgia.com";
                if (frm.ShowDialog() == DialogResult.OK)
                {
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void chk_vatgia_tatca_CheckedChanged(object sender, EventArgs e)
        {
            txt_vatgia_trang.Enabled = !chk_vatgia_tatca.Checked;
        }
        private void gw_mathang_chon_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            label3.Text = string.Format("Danh Sách Mặt Hàng: {0}", gw_mathang_chon.Rows.Count);
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            if (SavaAs_vatgia())
                MessageBox.Show("Lưu dữ liệu thành công", "Thông Báo");
        }

        private bool SavaAs_vatgia()
        {
            PleaseWait objPleaseWait = null;

            if (_table_vatgia.Rows.Count > 0)
            {
                SaveAs sa = new SaveAs();
                objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();

                foreach (DataRow item in _table_vatgia.Rows)
                {
                    string path = item["path_chon"].ToString();
                    DataTable tb = SQLDatabase.ExcDataTable(string.Format("select count(*) from SaveAs where scannerby='{0}' and path_chon='{1}'", "vatgia.com", item["path_chon"].ToString()));
                    if (ConvertType.ToInt(tb.Rows[0][0]) == 0)
                    {
                        SQLDatabase.AddSaveAs(new SaveAs()
                        {
                            id_chon = item["id_chon"].ToString(),
                            name_chon = item["name_chon"].ToString(),
                            path_chon = item["path_chon"].ToString(),
                            parentid_chon = item["parentid_chon"].ToString(),
                            orderid_chon = item["orderid_chon"].ToString(),
                            scannerby = "vatgia.com",
                        });
                    }
                }
                if (objPleaseWait != null)
                    objPleaseWait.Close();

                return true;
            }
            else
            {
                return true;
            }
        }


        private void pic_vatgia_in_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    List<SaveAs> save = new List<SaveAs>();
                    frmSaveAs frm = new frmSaveAs();
                    frm.fromparent = "vatgia.com";
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        save = frm.Saveas;
                        if (save.Count() > 0)
                        {
                            foreach (SaveAs item in save)
                            {
                                if (_table_vatgia.Select(string.Format("path_chon = '{0}'", item.path_chon)).Count() == 0)
                                    _table_vatgia.Rows.Add(ConvertType.ToInt(item.id_chon),
                                                                item.name_chon,
                                                                item.path_chon,
                                                                item.parentid_chon,
                                                                item.orderid_chon);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "danhMucToolStripMenuItem1_Click");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("File xml không đúng định dạng\n" + ex.Message, "button5_Click_1");

            }
        }
        private void gw_mathang_chon_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void btn_vatgia_sotrang_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_vatgia_trang.Text) >= 20)
                    return;
                txt_vatgia_trang.Text = (ConvertType.ToInt(txt_vatgia_trang.Text) + sotrang_nhay).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_vatgia_sotrang_tang_Click");
            }
        }

        private void btn_vatgia_sotrang_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_vatgia_trang.Text) <= 1)
                    return;
                txt_vatgia_trang.Text = (ConvertType.ToInt(txt_vatgia_trang.Text) - sotrang_nhay).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_vatgia_sotrang_giam_Click");
            }
        }

        private void btn_vatgia_sleep_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_vatgia_sleep.Text) <= 0)
                    return;
                txt_vatgia_sleep.Text = (ConvertType.ToInt(txt_vatgia_sleep.Text) - sleep_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_vatgia_sleep_tang_Click");
            }
        }

        private void btn_vatgia_sleep_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_vatgia_sleep.Text) >= 1000)
                    return;
                txt_vatgia_sleep.Text = (ConvertType.ToInt(txt_vatgia_sleep.Text) + sleep_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_vatgia_sleep_giam_Click");
            }
        }

        private void btn_vatgia_lanlap_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_vatgia_lanlap.Text) >= 20)
                    return;
                txt_vatgia_lanlap.Text = (ConvertType.ToInt(txt_vatgia_lanlap.Text) + lanlap_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_vatgia_lanlap_tang_Click");
            }
        }

        private void btn_vatgia_lanlap_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_vatgia_lanlap.Text) <= 0)
                    return;
                txt_vatgia_lanlap.Text = (ConvertType.ToInt(txt_vatgia_lanlap.Text) - lanlap_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_vatgia_lanlap_giam_Click");
            }
        }

        private void btn_vatgia_timeout_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_vatgia_timeout.Text) <= 500)
                    return;
                txt_vatgia_timeout.Text = (ConvertType.ToInt(txt_vatgia_timeout.Text) - timeout_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_vatgia_timeout_giam_Click");
            }
        }

        private void btn_vatgia_timeout_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_vatgia_timeout.Text) >= 100000)
                    return;
                txt_vatgia_timeout.Text = (ConvertType.ToInt(txt_vatgia_timeout.Text) + timeout_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_vatgia_timeout_tang_Click");
            }
        }

        private void mụcLụcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmDM_Vatgia frm = new frmDM_Vatgia();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "danhMucToolStripMenuItem1_Click");
            }
        }

        #endregion

        #region Trang Vàng
        private void chk_trangvang_lanlap_all_CheckedChanged(object sender, EventArgs e)
        {
            txt_trangvang_lanlap.Enabled = !chk_trangvang_all.Checked;
        }
        public void BinddmTrangVang()
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("select ID,NAME from dm_trangvang where parentId is null "));
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(int));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add(-1, "---Chọn Nhóm Gian Hàng---");
            foreach (DataRow item in table.Rows)
                table_nhom.Rows.Add(item["id"], item["name"]);

            cmd_trangvang.DataSource = table_nhom;
            cmd_trangvang.ValueMember = "id";
            cmd_trangvang.DisplayMember = "name";
            cmd_trangvang.SelectedValue = -1;
        }

        public DataTable CreateTable_trangvang()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id_chon", typeof(int));
            table.Columns.Add("name_chon", typeof(string));
            table.Columns.Add("path_chon", typeof(string));
            table.Columns.Add("parentid_chon", typeof(string));
            table.Columns.Add("orderid_chon", typeof(string));
            return table;
        }

        public DataTable CreateTable_scanner()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id_chon", typeof(int));
            table.Columns.Add("dosau_chon", typeof(string));
            table.Columns.Add("name_chon", typeof(string));
            table.Columns.Add("path_chon", typeof(string));
            table.Columns.Add("parentid_chon", typeof(int));


            return table;
        }

        private void mucLucToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmDM_trangvang frm = new frmDM_trangvang();
                frm.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmd_trangvang_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindTrangVang(cmd_trangvang.SelectedValue);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "cmb_nhomhang_SelectedIndexChanged");
            }
        }

        public void BindTrangVang(object nhom)
        {
            if (nhom == null)
                return;
            if (nhom.ToString() == "System.Data.DataRowView")
                return;
            DataTable table = SQLDatabase.ExcDataTable(string.Format("[spFindDmtrangvang] '{0}'", nhom));
            IEnumerable<DataRow> query = from order in table.AsEnumerable()
                                         .Where(s => s.Field<string>("path").Length == 0)
                                         select order;

            gv_trangvang_goc.DataSource = table;
        }
        private void btn_next_Click(object sender, EventArgs e)
        {
            try
            {
                int id = 0;
                string parentId = "";
                string name = "";
                string path = "";
                string orderid = "";
                foreach (DataGridViewRow row in gv_trangvang_goc.SelectedRows)
                {
                    DataRow[] result = _table_trangvang.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_trangvang_goc"].Value)));

                    if (result.Count() == 0 && row.Cells["path_trangvang_goc"].Value != null)
                    {
                        id = ConvertType.ToInt(row.Cells["id_trangvang_goc"].Value);
                        name = row.Cells["name_trangvang_goc"].Value.ToString();
                        path = row.Cells["path_trangvang_goc"].Value == null ? "" : row.Cells["path_trangvang_goc"].Value.ToString();
                        parentId = row.Cells["parentId_trangvang_goc"].Value == null ? "" : row.Cells["parentId_trangvang_goc"].Value.ToString();
                        orderid = row.Cells["orderid_trangvang_goc"].Value == null ? "" : row.Cells["orderid_trangvang_goc"].Value.ToString();

                        _table_trangvang.Rows.Add(id, name, path, parentId, orderid);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }
        private void btn_xoa_Click(object sender, EventArgs e)
        {
            try
            {
                _table_trangvang.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button4_Click");
            }
        }
        private void btn_last_Click(object sender, EventArgs e)
        {
            try
            {
                if (gv_trangvang_chon.SelectedRows.Count == 0)
                    return;
                foreach (DataGridViewRow row in gv_trangvang_chon.SelectedRows)
                {
                    _table_trangvang.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_trangvang_chon"].Value)))[0].Delete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button3_Click");
            }
        }
        private void gv_trangvang_chon_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            lbl_trangvang_danhsach.Text = string.Format("Danh Sách Mặt Hàng: {0}", gv_trangvang_chon.Rows.Count);
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            ParameterizedThreadStart par;
            ArrayList arr;
            try
            {
                if (gv_trangvang_chon.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ngành nghề kinh doanh", "Thông báo");
                    return;
                }

                Utilities_trangvang._Timeout = ConvertType.ToInt(txt_trangvang_timeout.Text);
                Utilities_trangvang._Sleep = ConvertType.ToInt(txt_trangvang_sleep.Text);
                Utilities_trangvang._regexs = SQLDatabase.LoadRegexs("select * from Regexs");
                /*load danh sách đầu số*/

                DataTable tb_dausp = SQLDatabase.ExcDataTable("select distinct dauso dauso,lenght " +
                                                   "  from dau_so where dauso is not null and dauso <> ''");

                Dictionary<string, int> dauso = new Dictionary<string, int>();
                foreach (DataRow item in tb_dausp.Rows)
                {
                    dauso.Add(item["dauso"].ToString(), ConvertType.ToInt(item["lenght"]));
                }
                Utilities_trangvang._dau_so = dauso;

                if (btn_start.Text == "Start")
                {
                    btn_start.Text = "Stop";
                    setTitleWindow(2, true);

                    Utilities_trangvang.hasProcess = true;
                    btn_next.Enabled = false;
                    btn_xoa.Enabled = false;
                    btn_last.Enabled = false;
                    cmd_trangvang.Enabled = false;
                    chk_trangvang_all.Enabled = false;
                    chk_trangvang_lanlap_all.Enabled = false;
                    txt_trangvang_page.Enabled = false;

                    pic_trangvang_import.Enabled = false;
                    pic_trangvang_out.Enabled = false;


                    lbl_message_trangvang_1.Visible = true;
                    lbl_message_trangvang_2.Visible = true;
                    lbl_message_trangvang_khoa.Visible = true;
                    gv_trangvang_chon.Enabled = false;
                    gv_trangvang_goc.Enabled = false;

                    btn_trangvang_sotrang_tang.Enabled = false;
                    btn_trangvang_sotrang_giam.Enabled = false;

                    txt_trangvang_sleep.Enabled = false;
                    btn_trangvang_sleep_giam.Enabled = false;
                    btn_trangvang_sleep_tang.Enabled = false;

                    txt_trangvang_lanlap.Enabled = false;
                    btn_trangvang_lanlap_giam.Enabled = false;
                    btn_trangvang_lanlap_tang.Enabled = false;

                    txt_trangvang_timeout.Enabled = false;
                    btn_trangvang_timeout_giam.Enabled = false;
                    btn_trangvang_timeout_tang.Enabled = false;

                    /*kiem tra neu checkpath dc chon thi luu pathlimit vao gioi hang*/
                    if (!chk_trangvang_all.Checked)
                        Utilities_trangvang._PathLimit = ConvertType.ToInt(txt_trangvang_page.Text);
                    else
                        Utilities_trangvang._PathLimit = -1;

                    if (!chk_trangvang_lanlap_all.Checked)
                        Utilities_trangvang._lanquetlai = ConvertType.ToInt(txt_trangvang_lanlap.Text);
                    else
                        Utilities_trangvang._lanquetlai = -1;

                }
                else
                {
                    btn_start.Text = "Start";
                    Utilities_trangvang.hasProcess = false;
                    setTitleWindow(2, false);

                    btn_next.Enabled = true;
                    btn_xoa.Enabled = true;
                    btn_last.Enabled = true;
                    cmd_trangvang.Enabled = true;
                    chk_trangvang_all.Enabled = true;
                    chk_trangvang_lanlap_all.Enabled = true;
                    txt_trangvang_page.Enabled = true;
                    //lbl_message_trangvang_1.Visible = false;
                    // lbl_message_trangvang_2.Visible = false;
                    lbl_message_trangvang_khoa.Visible = false;
                    gv_trangvang_chon.Enabled = true;
                    gv_trangvang_goc.Enabled = true;

                    pic_trangvang_import.Enabled = true;
                    pic_trangvang_out.Enabled = true;

                    btn_trangvang_sotrang_tang.Enabled = true;
                    btn_trangvang_sotrang_giam.Enabled = true;

                    txt_trangvang_sleep.Enabled = true;
                    btn_trangvang_sleep_giam.Enabled = true;
                    btn_trangvang_sleep_tang.Enabled = true;

                    txt_trangvang_lanlap.Enabled = true;
                    btn_trangvang_lanlap_giam.Enabled = true;
                    btn_trangvang_lanlap_tang.Enabled = true;

                    txt_trangvang_timeout.Enabled = true;
                    btn_trangvang_timeout_giam.Enabled = true;
                    btn_trangvang_timeout_tang.Enabled = true;

                    if (chk_trangvang_all.Checked)
                    {
                        txt_trangvang_page.Enabled = false;
                    }
                }

                /*Cấu hình controll*/
                Control.CheckForIllegalCrossThreadCalls = false;

                par = new ParameterizedThreadStart(ProcessTrangVang);
                theardProcess = new Thread(par);

                arr = new ArrayList();
                arr.Add(lbl_message_trangvang_1);
                arr.Add(lbl_message_trangvang_2);
                arr.Add(lbl_message_trangvang_khoa);


                //http://stackoverflow.com/questions/3542061/how-do-i-stop-a-thread-when-my-winform-application-closes
                theardProcess.IsBackground = true;
                theardProcess.Start(arr);
            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "button2_Click", "error1"));
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

        private void ProcessTrangVang(object arrControl)
        {
            try
            {
                //----- Add control process from
                string domain = "http://trangvangvietnam.com";
                ArrayList arr1 = (ArrayList)arrControl;
                Label lbl_message_trangvang_1 = (Label)arr1[0];
                Label lbl_message_trangvang_2 = (Label)arr1[1];
                Label lbl_message_trangvang_khoa = (Label)arr1[2];

                //----- update display control
                lbl_message_trangvang_1.Update();
                lbl_message_trangvang_2.Update();
                lbl_message_trangvang_khoa.Update();

                /*===============================================================*/
                int x = 0;
                string strUrl = "";
                while (gv_trangvang_chon.Rows.Count > 0)
                {
                    if (!Utilities_trangvang.hasProcess) break;

                    int id = Convert.ToInt32(gv_trangvang_chon.Rows[x].Cells["id_trangvang_chon"].Value);
                    string strPath = gv_trangvang_chon.Rows[x].Cells["path_trangvang_chon"].Value.ToString();
                    string strnganh = gv_trangvang_chon.Rows[x].Cells["name_trangvang_chon"].Value.ToString();
                    strUrl = domain + strPath.Replace("..", "").Replace(" ", "").Replace("\t", "");

                    int pagemax = Utilities_trangvang.getPageMax(strUrl, lbl_message_trangvang_khoa);
                    //int pagemax = 100;
                    int i = 0;
                    do
                    {
                        if (!Utilities_trangvang.hasProcess)
                            break;
                        {
                            /*kiễm tra trang*/
                            if (Utilities_trangvang._PathLimit != -1 && Utilities_trangvang._PathLimit == i)
                                break;
                            i = i + 1;

                            string strUrl1 = "";
                            strUrl1 = string.Format(strUrl + "?page={0}", i);

                            lbl_message_trangvang_1.Text = string.Format("Đang Xử Lý: {0} \n Trang: {1}\\ Tổng Trang: {2}", strnganh, i, pagemax + 1);
                            lbl_message_trangvang_1.Update();

                            int solanlap = 0;
                            Utilities_trangvang.IdDanhmuc = id;
                            Utilities_trangvang.getwebBrowser(strUrl1, ref solanlap, lbl_message_trangvang_2, lbl_message_trangvang_khoa);

                        }
                    } while (i <= pagemax);
                    /*neu dang stop thi khong dc phep xoa*/
                    if (Utilities_trangvang.hasProcess)
                        if (_table_trangvang.Select(string.Format("id_chon={0}", id)).Count() != 0)
                            _table_trangvang.Select(string.Format("id_chon={0}", id))[0].Delete();
                }
                /*===================================================================*/
                lbl_message_trangvang_1.Text = Utilities_trangvang.hasProcess ? "Hoàn thành load số liệu." : "Tạm dừng do người dùng!!!";
                if (Utilities_trangvang.hasProcess)
                {
                    Utilities_trangvang._thanhcong = 0;
                    Utilities_trangvang._thatbai = 0;
                }

                btn_start.Text = "Start";
                Utilities_trangvang.hasProcess = false;
                setTitleWindow(2, false);

                btn_next.Enabled = true;
                btn_xoa.Enabled = true;
                btn_last.Enabled = true;
                cmd_trangvang.Enabled = true;
                chk_trangvang_all.Enabled = true;
                txt_trangvang_page.Enabled = true;
                //lbl_message_trangvang_1.Visible = false;
                // lbl_message_trangvang_2.Visible = false;
                lbl_message_trangvang_khoa.Visible = false;
                gv_trangvang_chon.Enabled = true;
                gv_trangvang_goc.Enabled = true;

                pic_trangvang_import.Enabled = true;
                pic_trangvang_out.Enabled = true;

                btn_trangvang_sotrang_tang.Enabled = true;
                btn_trangvang_sotrang_giam.Enabled = true;

                txt_trangvang_sleep.Enabled = true;
                btn_trangvang_sleep_giam.Enabled = true;
                btn_trangvang_sleep_tang.Enabled = true;

                txt_trangvang_lanlap.Enabled = true;
                btn_trangvang_lanlap_giam.Enabled = true;
                btn_trangvang_lanlap_tang.Enabled = true;

                txt_trangvang_timeout.Enabled = true;
                btn_trangvang_timeout_giam.Enabled = true;
                btn_trangvang_timeout_tang.Enabled = true;

                if (chk_trangvang_all.Checked)
                {
                    txt_trangvang_page.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "ProcessTrangVang", "error1"));
                MessageBox.Show(ex.Message, "ProcessTrangVang");
            }
        }
        private void xuâtFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmExporttrangvang frm = new frmExporttrangvang();
                frm.strDatabaseName = _strDatabase;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void pic_trangvang_out_Click(object sender, EventArgs e)
        {
            if (SavaAs_trangvang())
                MessageBox.Show("Lưu dữ liệu thành công", "Thông Báo");
        }

        private bool SavaAs_trangvang()
        {
            PleaseWait objPleaseWait = null;
            if (_table_trangvang.Rows.Count > 0)
            {
                objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();

                SaveAs sa = new SaveAs();
                foreach (DataRow item in _table_trangvang.Rows)
                {
                    string path = item["path_chon"].ToString();
                    DataTable tb = SQLDatabase.ExcDataTable(string.Format("select count(*) from SaveAs where scannerby='{0}' and path_chon='{1}'", "trangvang.com", item["path_chon"].ToString()));
                    if (ConvertType.ToInt(tb.Rows[0][0]) == 0)
                    {

                        SQLDatabase.AddSaveAs(new SaveAs()
                        {
                            id_chon = item["id_chon"].ToString(),
                            name_chon = item["name_chon"].ToString(),
                            path_chon = item["path_chon"].ToString(),
                            parentid_chon = item["parentid_chon"].ToString(),
                            orderid_chon = item["orderid_chon"].ToString(),
                            scannerby = "trangvang.com",
                        });
                    }
                }

                if (objPleaseWait != null)
                    objPleaseWait.Close();

                return true;
            }
            else
                return true;
        }


        private void pic_trangvang_import_Click(object sender, EventArgs e)
        {

            try
            {
                List<SaveAs> save = new List<SaveAs>();
                frmSaveAs frm = new frmSaveAs();
                frm.fromparent = "trangvang.com";
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    save = frm.Saveas;
                    if (save.Count() > 0)
                    {
                        foreach (SaveAs item in save)
                        {
                            if (_table_trangvang.Select(string.Format("path_chon = '{0}'", item.path_chon)).Count() == 0)
                                _table_trangvang.Rows.Add(ConvertType.ToInt(item.id_chon),
                                                            item.name_chon,
                                                            item.path_chon,
                                                            item.parentid_chon,
                                                            item.orderid_chon);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "danhMucToolStripMenuItem1_Click");
            }
        }

        private void chk_trangvang_all_CheckedChanged(object sender, EventArgs e)
        {
            txt_trangvang_page.Enabled = !chk_trangvang_all.Checked;
        }
        private void gv_trangvang_chon_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void btn_trangvang_sotrang_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_trangvang_page.Text) <= 1)
                    return;
                txt_trangvang_page.Text = (ConvertType.ToInt(txt_trangvang_page.Text) - sotrang_nhay).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_trangvang_sotrang_giam_Click");
            }
        }

        private void btn_trangvang_sotrang_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_trangvang_page.Text) >= 20)
                    return;
                txt_trangvang_page.Text = (ConvertType.ToInt(txt_trangvang_page.Text) + sotrang_nhay).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_trangvang_sotrang_tang_Click");
            }
        }

        private void btn_trangvang_sleep_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_trangvang_sleep.Text) <= 0)
                    return;
                txt_trangvang_sleep.Text = (ConvertType.ToInt(txt_trangvang_sleep.Text) - sleep_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_trangvang_sleep_giam_Click");
            }

        }

        private void btn_trangvang_sleep_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_trangvang_sleep.Text) >= 1000)
                    return;
                txt_trangvang_sleep.Text = (ConvertType.ToInt(txt_trangvang_sleep.Text) + sleep_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_trangvang_sleep_tang_Click");
            }
        }

        private void btn_trangvang_lanlap_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_trangvang_lanlap.Text) <= 0)
                    return;
                txt_trangvang_lanlap.Text = (ConvertType.ToInt(txt_trangvang_lanlap.Text) - lanlap_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_trangvang_lanlap_giam_Click");
            }
        }

        private void btn_trangvang_lanlap_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_trangvang_lanlap.Text) >= 20)
                    return;
                txt_trangvang_lanlap.Text = (ConvertType.ToInt(txt_trangvang_lanlap.Text) + lanlap_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_trangvang_lanlap_tang_Click");
            }
        }

        private void btn_trangvang_timeout_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_trangvang_timeout.Text) >= 50000)
                    return;
                txt_trangvang_timeout.Text = (ConvertType.ToInt(txt_trangvang_timeout.Text) + timeout_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_trangvang_timeout_tang_Click");
            }
        }

        private void btn_trangvang_timeout_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_trangvang_timeout.Text) <= 500)
                    return;
                txt_trangvang_timeout.Text = (ConvertType.ToInt(txt_trangvang_timeout.Text) - timeout_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_trangvang_timeout_giam_Click");
            }
        }

        #endregion

        #region Scanner

        public void BindDmNhomScane()
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format(" select id,name from [dbo].[dm_scanner]"));
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(int));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add(-1, "---Chọn Nhóm---");
            foreach (DataRow item in table.Rows)
                table_nhom.Rows.Add(item["id"], item["name"]);

            comboBox1.DataSource = table_nhom;
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "name";
            comboBox1.SelectedValue = -1;

        }

        private void danhMụcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmExportScanner frm = new frmExportScanner();
                frm.strDatabaseName = _strDatabase;
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

        private void btn_scanner_sleep_giam_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txtThoiGianCho.Text) <= 5)
                    return;
                txtThoiGianCho.Text = (ConvertType.ToInt(txtThoiGianCho.Text) - sleep_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_sleep_giam_Click");
            }
        }

        private void btn_scanner_sleep_tang_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txtThoiGianCho.Text) >= 100)
                    return;
                txtThoiGianCho.Text = (ConvertType.ToInt(txtThoiGianCho.Text) + sleep_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_sleep_tang_Click");
            }
        }

        private void btn_scanner_timeout_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_timeout.Text) <= 500)
                    return;
                txt_timeout.Text = (ConvertType.ToInt(txt_timeout.Text) - timeout_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_timeout_giam_Click");
            }
        }

        private void btn_scanner_timeout_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_timeout.Text) >= 100000)
                    return;
                txt_timeout.Text = (ConvertType.ToInt(txt_timeout.Text) + timeout_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_sleep_tang_Click");
            }
        }

        private void btn_scanner_lanlap_giam_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_lanlap.Text) <= 1)
                    return;
                txt_lanlap.Text = (ConvertType.ToInt(txt_lanlap.Text) - lanlap_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_lanlap_giam_Click");
            }
        }

        private void btn_scanner_lanlap_tang_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txt_lanlap.Text) >= 100)
                    return;
                txt_lanlap.Text = (ConvertType.ToInt(txt_lanlap.Text) + lanlap_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_lanlap_tang_Click");
            }
        }

        private void btn_scanner_soluong_giam_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txtSoLuong.Text) <= 1)
                    return;
                txtSoLuong.Text = (ConvertType.ToInt(txtSoLuong.Text) - soluong_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_soluong_giam_Click");
            }
        }

        private void btn_scanner_soluong_tang_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txtSoLuong.Text) >= 100)
                    return;
                txtSoLuong.Text = (ConvertType.ToInt(txtSoLuong.Text) + soluong_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_soluong_tang_Click");
            }
        }

        private void btn_scanner_dosau_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txtDoSau.Text) <= 0)
                    return;
                txtDoSau.Text = (ConvertType.ToInt(txtDoSau.Text) - dosau_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_dosau_giam_Click");
            }
        }

        private void btn_scanner_dosau_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txtDoSau.Text) >= 700)
                    return;
                txtDoSau.Text = (ConvertType.ToInt(txtDoSau.Text) + dosau_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_dosau_tang_Click");
            }
        }

        private void btn_scanner_giaihan_giam_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txtGioiHan.Text) <= 0)
                    return;
                txtGioiHan.Text = (ConvertType.ToInt(txtGioiHan.Text) - gioihan_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_giaihan_giam_Click");
            }
        }

        private void btn_scanner_giaihan_tang_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConvertType.ToInt(txtGioiHan.Text) >= 70000000)
                    return;
                txtGioiHan.Text = (ConvertType.ToInt(txtGioiHan.Text) + gioihan_bat).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btn_scanner_giaihan_tang_Click");
            }
        }
        private void btnXoaScanes_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    MessageBox.Show("Vui lòng chọn website", "Thông báo");
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn Xoá dữ liệu không?", "Thông Báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                 MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    SQLDatabase.ExcDataTable(string.Format("[spDel_scanner_ct] '{0}'", ConvertType.ToInt(comboBox1.SelectedValue)));
                    MessageBox.Show("Xoá thành công", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btnXoaScanes_Click");
            }
        }
        private int landautien = 0;
        private void ProcessScanner(object arrControl)
        {
            try
            {
                //----- Add control process from
                ArrayList arr1 = (ArrayList)arrControl;
                Label lbl_message = (Label)arr1[0];
                Label lbl_scanner_khoa = (Label)arr1[1];
                Label lbl_handoi = (Label)arr1[2];
                Label lbl_sl_phone = (Label)arr1[3];
                Label lbl_sl_email = (Label)arr1[4];
                Label lbl_scance_sl_Link = (Label)arr1[5];
                Label lblLienKetFind = (Label)arr1[6];


                bool dosaugioihang = false;
                /*===============================================================*/
                while (true)
                {
                    dm_scanner_ct link = null;
                    if (!Utilities_vatgia.hasProcess)
                        break;

                    try
                    {
                        if (_queue.CountQueue1() == 0)
                            break;
                        link = _queue.DequeueLinks1();

                        if (!Utilities_scanner.hasProcess ||
                        Utilities_scanner._gioihan_lienket <= Utilities_scanner._tonglink ||
                        Utilities_scanner._dosau <= ConvertType.ToInt(link.dosau))
                        {
                            if (Utilities_scanner._dosau <= ConvertType.ToInt(link.dosau))
                                dosaugioihang = true;
                            break;
                        }
                    }
                    catch { break; }
                    if (null != link)
                    {
                        int solanlap = 0;
                        Utilities_scanner.getwebBrowserFindLink(link, ref _queue, ref arrControl, ref solanlap);
                        Thread.Sleep(Utilities_scanner._sleep);
                    }
                    /*************************Thả các tiến trình con vào***********************************/
                    while (landautien == 0)
                    {
                        for (int i = 0; i < ConvertType.ToInt(txtSoLuong.Text); i++)
                        {
                            ParameterizedThreadStart par = new ParameterizedThreadStart(CacTieuTrinh);
                            Thread theardProcess = new Thread(par);
                            theardProcess.Start(arrControl);
                            Thread.Sleep(Utilities_scanner._sleep);
                        }
                        landautien = 1;
                    }

                }
                if (!Utilities_scanner.hasProcess)
                {
                    lbl_scanner_khoa.Invoke((MethodInvoker)(() => lbl_scanner_khoa.Text = string.Format("Tạm dừng do người dùng....")));
                    return;
                }
                else if (_queue.CountQueue1() == 0)
                {
                    lbl_scanner_khoa.Invoke((MethodInvoker)(() => lbl_scanner_khoa.Text = string.Format("Hoàn tất việc dò tìm....")));
                    return;
                }

                else if (dosaugioihang)
                {
                    lbl_scanner_khoa.Invoke((MethodInvoker)(() => lbl_scanner_khoa.Text = string.Format("{0} Đã hoàn thành việc dò tìm tới giới hạn liên kết {0}", Utilities_scanner._gioihan_lienket)));
                    return;
                }
                else
                {
                    lbl_scanner_khoa.Invoke((MethodInvoker)(() => lbl_scanner_khoa.Text = string.Format("Đã kết thúc tiến trình : {0}", theardProcess.Name)));
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ProcessScanner");
            }
        }

        private void CacTieuTrinh(object arrControl)
        {
            try
            {
                while (true)
                {
                    dm_scanner_ct link = null;
                    try
                    {
                        if (_queue.CountQueue1() == 0)
                        {
                            return;
                        }
                        link = _queue.DequeueLinks1();
                        if (!Utilities_scanner.hasProcess ||
                         Utilities_scanner._gioihan_lienket <= Utilities_scanner._tonglink ||
                         Utilities_scanner._dosau <= ConvertType.ToInt(link.dosau))
                            break;
                    }
                    catch { break; }
                    if (null != link)
                    {
                        int solanlap = 0;
                        Utilities_scanner.getwebBrowserFindLink(link, ref _queue, ref arrControl, ref solanlap);
                        Thread.Sleep(Utilities_scanner._sleep);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CacTieuTrinh");
            }
        }
        private void btn_tao_Click(object sender, EventArgs e)
        {
            try
            {
                frmDM_scanner2 frm = new frmDM_scanner2();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    BindDmNhomScane();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            ArrayList arr;
            int nSoLuong = 0;
            try
            {

                if (comboBox1.SelectedValue.ToString() == "-1")
                {
                    MessageBox.Show("Vui lòng nhập liên kết.", "Thông Báo");
                    comboBox1.Focus();
                    return;
                }
                if (_domain == "")
                {
                    MessageBox.Show("Vui lòng thông tin domain của liên kết.", "Thông Báo");
                    btn_tao.Focus();
                    return;
                }


                int _tonglink = 0;
                _comboxnhom = comboBox1.SelectedValue.ToString();

                if (chk_scannce_quetlaitrangindex.Checked)
                {
                    SQLDatabase.ExcNonQuery(string.Format("update dm_scanner_ct " +
                                                          " set statur= 0 " +
                                                          " from dm_scanner_ct a inner join dm_scanner b on a.parentid=b.id " +
                                                          " where parentid={0} ", _comboxnhom));
                }

                _queue = LinkQueues2.Instance;
                _queue.setIdLienKet(ConvertType.ToInt(_comboxnhom));
                _queue.InitBindLinsAll(ConvertType.ToInt(_comboxnhom), ref _tonglink);

                DataTable _tb1 = SQLDatabase.ExcDataTable(string.Format(" select COUNT(*)  as tongcong" +
                                                                        " from scanner_phone a inner join dm_scanner_ct b on a.dm_scanner_ct_id=b.id " +
                                                                        " where b.parentid = '{0}'", _comboxnhom
                                                            ));
                Utilities_scanner._slPhone = ConvertType.ToInt(_tb1.Rows[0]["tongcong"]);

                DataTable _tb2 = SQLDatabase.ExcDataTable(string.Format(" select COUNT(*)  as tongcong" +
                                                                        " from scanner_email a inner join dm_scanner_ct b on a.dm_scanner_ct_id=b.id " +
                                                                        " where b.parentid = '{0}'", _comboxnhom
                                                            ));
                Utilities_scanner._slEmail = ConvertType.ToInt(_tb2.Rows[0]["tongcong"]);


                Utilities_scanner._tonglink = _tonglink;
                lbl_scance_sl_Link.Text = _tonglink.ToString("#,#", CultureInfo.InvariantCulture);
                lbl_handoi.Text = _queue.CountQueue1().ToString("#,#", CultureInfo.InvariantCulture);
                Utilities_scanner._regexs = SQLDatabase.LoadRegexs("select * from Regexs order by OrderID desc");
                Utilities_scanner._UserAgent = cmb_scannce_UserAgent.SelectedItem.ToString();

                nSoLuong = ConvertType.ToInt(txtSoLuong.Text);
                if (button5.Text == "Start")
                {
                    button5.Text = "Stop";

                    if (_queue.CountQueue1() == 0)
                    {
                        lbl_message.Text = "Đang dò tìm liên kết ";
                        lbl_message.Update();
                    }
                    else
                    {
                        lbl_message.Text = lbl_message.Text + "Bắt đầu quay lại công việc dò tìm.... " + Environment.NewLine;
                        lbl_message.Update();
                    }

                    lbl_message.Text = lbl_message.Text + "Nạp dữ liệu chưa quét vào hàng đợi.... " + Environment.NewLine;
                    lbl_message.Update();

                    Utilities_scanner.hasProcess = true;
                    Utilities_scanner._doman = _domain;
                    Utilities_scanner._sleep = ConvertType.ToInt(txtThoiGianCho.Text);
                    Utilities_scanner._lanquetlai = ConvertType.ToInt(txt_lanlap.Text);
                    Utilities_scanner._timeout = ConvertType.ToInt(txt_timeout.Text);
                    Utilities_scanner._gioihan_lienket = ConvertType.ToInt(txtGioiHan.Text);
                    Utilities_scanner._sodienthoai = chk_scanner_Phone.Checked;
                    Utilities_scanner._emails = chk_scanner_Email.Checked;
                    Utilities_scanner._HienThiChiTietQuet = chkChiTietquet.Checked;
                    /*load danh sách đầu số*/

                    DataTable tb_dausp = SQLDatabase.ExcDataTable("select distinct dauso dauso,lenght " +
                                                  "  from dau_so where dauso is not null and dauso <> ''");

                    Dictionary<string, int> dauso = new Dictionary<string, int>();
                    foreach (DataRow item in tb_dausp.Rows)
                    {
                        dauso.Add(item["dauso"].ToString(), ConvertType.ToInt(item["lenght"].ToString()));
                    }
                    Utilities_scanner._dau_so = dauso;
                    comboBox1.Enabled = false;
                    txtDoSau.Enabled = false;
                    txtSoLuong.Enabled = false;
                    txtThoiGianCho.Enabled = false;
                    txt_timeout.Enabled = false;
                    txt_lanlap.Enabled = false;
                    btn_tao.Enabled = false;

                    btn_scanner_sleep_giam.Enabled = false;
                    btn_scanner_sleep_tang.Enabled = false;
                    btn_scanner_timeout_giam.Enabled = false;
                    btn_scanner_timeout_tang.Enabled = false;
                    btn_scanner_lanlap_tang.Enabled = false;
                    btn_scanner_lanlap_giam.Enabled = false;
                    btn_scanner_soluong_tang.Enabled = false;
                    btn_scanner_soluong_giam.Enabled = false;
                    btn_scanner_dosau_giam.Enabled = false;
                    btn_scanner_dosau_tang.Enabled = false;
                    btn_scanner_giaihan_giam.Enabled = false;
                    btn_scanner_giaihan_tang.Enabled = false;
                    chkChiTietquet.Enabled = false;
                    checkBox1.Enabled = false;
                    chk_scanner_Phone.Enabled = false;
                    chk_scanner_Email.Enabled = false;
                    btnXoaScanes.Enabled = false;
                    chk_scannce_quetlaitrangindex.Enabled = false;
                    cmb_scannce_UserAgent.Enabled = false;


                    /*kiem tra neu checkpath dc chon thi luu pathlimit vao gioi hang*/

                    Utilities_scanner._dosau = ConvertType.ToInt(txtDoSau.Text);
                    if (!checkBox1.Checked)
                        Utilities_scanner._lanquetlai = ConvertType.ToInt(txt_lanlap.Text);
                    else
                        Utilities_scanner._lanquetlai = -1;

                    lbl_message.Text = lbl_message.Text + "Đang quét.... " + Environment.NewLine;
                    lbl_message.Update();

                }
                else
                {
                    button5.Text = "Start";
                    Utilities_scanner.hasProcess = false;
                    comboBox1.Enabled = true;
                    txtDoSau.Enabled = true;
                    txtSoLuong.Enabled = true;

                    btn_tao.Enabled = true;
                    txtThoiGianCho.Enabled = true;
                    txt_timeout.Enabled = true;
                    txt_lanlap.Enabled = true;

                    btn_scanner_sleep_giam.Enabled = true;
                    btn_scanner_sleep_tang.Enabled = true;
                    btn_scanner_timeout_giam.Enabled = true;
                    btn_scanner_timeout_tang.Enabled = true;
                    btn_scanner_lanlap_tang.Enabled = true;
                    btn_scanner_lanlap_giam.Enabled = true;
                    btn_scanner_soluong_tang.Enabled = true;
                    btn_scanner_soluong_giam.Enabled = true;
                    btn_scanner_dosau_giam.Enabled = true;
                    btn_scanner_dosau_tang.Enabled = true;
                    btn_scanner_giaihan_giam.Enabled = true;
                    btn_scanner_giaihan_tang.Enabled = true;
                    chkChiTietquet.Enabled = true;
                    btnXoaScanes.Enabled = true;
                    checkBox1.Enabled = true;
                    chk_scanner_Phone.Enabled = true;
                    chk_scanner_Email.Enabled = true;
                    chk_scannce_quetlaitrangindex.Enabled = true;
                    cmb_scannce_UserAgent.Enabled = true;
                    txtDoSau.Enabled = false;

                    lbl_message.Text = lbl_message.Text + "Dừng lại.... " + Environment.NewLine;
                    lbl_message.Update();

                }
                /*Cấu hình controll*/
                Control.CheckForIllegalCrossThreadCalls = false;
                ParameterizedThreadStart par;

                par = new ParameterizedThreadStart(ProcessScanner);
                theardProcess = new Thread(par);

                arr = new ArrayList();
                arr.Add(lbl_message);
                arr.Add(lbl_scanner_khoa);
                arr.Add(lbl_handoi);
                arr.Add(lbl_sl_phone);
                arr.Add(lbl_sl_email);
                arr.Add(lbl_scance_sl_Link);
                arr.Add(lblLienKetFind);
                theardProcess.Start(arr);
            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "button5_Click_1", "error1"));
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            object id = ConvertType.ToInt(comboBox1.SelectedValue);
            if (id.ToString() == "0")
                return;

            DataTable tb = SQLDatabase.ExcDataTable(string.Format("select * from dm_scanner where id='{0}'", id));
            if (tb == null || tb.Rows.Count == 0)
                return;
            _domain = tb.Rows[0]["domain"].ToString();
        }
        #endregion

        #region Muaban

        public void BindDanhMuc()
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("select id,name from dm_muaban where parentId is null"));
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(string));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add("-1", "---Tất Cả---");
            foreach (DataRow item in table.Rows)
            {
                table_nhom.Rows.Add(item["id"], item["name"]);
            }

            cmd_muaban_tv_danhmuc.DataSource = table_nhom;
            cmd_muaban_tv_danhmuc.ValueMember = "id";
            cmd_muaban_tv_danhmuc.DisplayMember = "name";
            cmd_muaban_tv_danhmuc.SelectedValue = -1;
        }

        public void BinddmNganhNghe(int id)
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("select path,name from dm_muaban where parentId={0}", id));
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("path", typeof(string));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add(-1, "---Tất Cả---");
            foreach (DataRow item in table.Rows)
                table_nhom.Rows.Add(item["path"], item["name"]);

            cmb_muaban_tv_nganhnghe.DataSource = table_nhom;
            cmb_muaban_tv_nganhnghe.ValueMember = "path";
            cmb_muaban_tv_nganhnghe.DisplayMember = "name";
            cmb_muaban_tv_nganhnghe.SelectedValue = -1;
        }
        private void BindmuabanMucLuongTu()
        {
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(int));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add(0, "--");
            table_nhom.Rows.Add(1000, "1 Triệu");
            table_nhom.Rows.Add(3000, "3 Triệu");
            table_nhom.Rows.Add(5000, "5 Triệu");
            table_nhom.Rows.Add(10000, "10 Triệu");
            table_nhom.Rows.Add(20000, "20 Triệu");
            table_nhom.Rows.Add(25000, "15 Triệu");
            table_nhom.Rows.Add(50000, "50 Triệu");



            cmb_muaban_tv_tu.DataSource = table_nhom;
            cmb_muaban_tv_tu.ValueMember = "id";
            cmb_muaban_tv_tu.DisplayMember = "name";
            cmb_muaban_tv_tu.SelectedValue = 0;
        }

        private void BindmuabanMucLuongDen()
        {
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(int));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add(0, "--");
            table_nhom.Rows.Add(1000, "1 Triệu");
            table_nhom.Rows.Add(3000, "3 Triệu");
            table_nhom.Rows.Add(5000, "5 Triệu");
            table_nhom.Rows.Add(10000, "10 Triệu");
            table_nhom.Rows.Add(20000, "20 Triệu");
            table_nhom.Rows.Add(25000, "15 Triệu");
            table_nhom.Rows.Add(50000, "50 Triệu");



            cmb_muaban_tv_den.DataSource = table_nhom;
            cmb_muaban_tv_den.ValueMember = "id";
            cmb_muaban_tv_den.DisplayMember = "name";
            cmb_muaban_tv_den.SelectedValue = 0;
        }

        private void BindmuabanHinhThuc()
        {
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(int));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add(-1, "--Tất Cả");
            table_nhom.Rows.Add(1, "Toàn thời gian cố định");
            table_nhom.Rows.Add(2, "Toàn thời gian tạm thời");
            table_nhom.Rows.Add(3, "Bán thời gian cố định");
            table_nhom.Rows.Add(4, "Bán thời gian tạm thời");
            table_nhom.Rows.Add(5, "Theo hợp đồng tư vấn");
            table_nhom.Rows.Add(6, "Thực tập");
            table_nhom.Rows.Add(7, "Khac");

            cmb_muaban_tv_hinhthuc.DataSource = table_nhom;
            cmb_muaban_tv_hinhthuc.ValueMember = "id";
            cmb_muaban_tv_hinhthuc.DisplayMember = "name";
            cmb_muaban_tv_hinhthuc.SelectedValue = 0;
        }
        private void BindmuabanThoiGianDang()
        {
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(int));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add(0, "--Tất Cả");
            table_nhom.Rows.Add(3, "Trong vòng 03 ngày");
            table_nhom.Rows.Add(7, "Trong vòng 07 ngày");
            table_nhom.Rows.Add(15, "Trong vòng 15 ngày");
            table_nhom.Rows.Add(30, "Trong vòng 30 ngày");

            cmb_muaban_tv_thoigiandang.DataSource = table_nhom;
            cmb_muaban_tv_thoigiandang.ValueMember = "id";
            cmb_muaban_tv_thoigiandang.DisplayMember = "name";
            cmb_muaban_tv_thoigiandang.SelectedValue = 0;

        }

        private void mụcLụcToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmDM_Muaban_timviec frm = new frmDM_Muaban_timviec();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                BinddmNganhNghe(ConvertType.ToInt(cmd_muaban_tv_danhmuc.SelectedValue));
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (cmd_muaban_tv_danhmuc.SelectedValue.ToString() == "-1")
            {
                MessageBox.Show("Vui lòng chọn danh mục cần load", "Thông báo");
                return;
            }
            if (ConvertType.ToInt(cmb_muaban_tv_tu.SelectedValue) > ConvertType.ToInt(cmb_muaban_tv_den.SelectedValue))
            {
                MessageBox.Show("Vui lòng kiễm tra thông tin mức lương từ > mức lương đến");
                cmb_muaban_tv_den.SelectedValue = 0;
                cmb_muaban_tv_tu.SelectedValue = 0;
                return;
            }

            Utilities_muaban._Timeout = ConvertType.ToInt(txt_muaban_tv_timeout.Text);
            Utilities_muaban._Sleep = ConvertType.ToInt(txt_muaban_tv_sleep.Text);
            Utilities_muaban._modelTrang = _modelTrang;
            /*kiem tra neu checkpath dc chon thi luu pathlimit vao gioi hang*/
            if (!chk_muaban_tv_Trang_tatca.Checked)
                Utilities_muaban._PathLimit = ConvertType.ToInt(txt_muaban_tv_trang.Text);
            else
                Utilities_muaban._PathLimit = -1;

            if (!chk_muaban_tv_lanlap_tatca.Checked)
                Utilities_muaban._lanquetlai = ConvertType.ToInt(txt_muaban_tv_lanlap.Text);
            else
                Utilities_muaban._lanquetlai = -1;



            ParameterizedThreadStart par;
            ArrayList arr;
            try
            {
                if (btn_start_muaban_timviec.Text == "Start")
                {
                    btn_start_muaban_timviec.Text = "Stop";
                    setTitleWindow(5, true);

                    Utilities_muaban.hasProcess = true;
                    //btn_start_muaban_timviec.Enabled = false;

                    txt_muaban_tv_trang.Enabled = false;
                    btn_muaban_tv_Tranggiam.Enabled = false;
                    btn_muaban_tv_Trangtang.Enabled = false;

                    lbl_message_muaban_tv_1.Visible = true;
                    lbl_message_muaban_tv_2.Visible = true;

                    txt_muaban_tv_lanlap.Enabled = false;
                    btn_muaban_tv_lanlapgiam.Enabled = false;
                    btn_muaban_tv_lanlaptang.Enabled = false;

                    txt_muaban_tv_sleep.Enabled = false;
                    btn_muaban_tv_sleepgiam.Enabled = false;
                    btn_muaban_tv_sleeptang.Enabled = false;

                    txt_muaban_tv_timeout.Enabled = false;
                    cmd_muaban_tv_danhmuc.Enabled = false;
                    cmb_muaban_tv_nganhnghe.Enabled = false;
                    cmb_muaban_tv_tu.Enabled = false;
                    chk_muaban_tv_Trang_tatca.Enabled = false;
                    chk_muaban_tv_lanlap_tatca.Enabled = false;
                    button6.Enabled = false;
                    cmb_muaban_tv_den.Enabled = false;
                    cmb_muaban_tv_hinhthuc.Enabled = false;
                    cmb_muaban_tv_thoigiandang.Enabled = false;


                    /*kiem tra neu checkpath dc chon thi luu pathlimit vao gioi hang*/
                    if (!chk_muaban_tv_Trang_tatca.Checked)
                        Utilities_muaban._PathLimit = ConvertType.ToInt(txt_muaban_tv_trang.Text);
                    else
                        Utilities_muaban._PathLimit = -1;

                    /*load danh sách đầu số*/

                    DataTable tb_dausp = SQLDatabase.ExcDataTable("select distinct dauso dauso,lenght " +
                                                  "  from dau_so where dauso is not null and dauso <> ''");

                    Dictionary<string, int> dauso = new Dictionary<string, int>();
                    foreach (DataRow item in tb_dausp.Rows)
                    {
                        dauso.Add(item["dauso"].ToString(), ConvertType.ToInt(item["lenght"].ToString()));
                    }
                    Utilities_muaban._dau_so = dauso;
                    Utilities_muaban._regexs = SQLDatabase.LoadRegexs("select * from Regexs order by OrderID desc");
                    Utilities_muaban._thanhcong = 0;
                    Utilities_muaban._thatbai = 0;
                    Utilities_muaban._modelTrang = _modelTrang;
                    //pro_muaban_tv.Show();
                }
                else
                {
                    btn_start_muaban_timviec.Text = "Start";

                    //pro_muaban_tv.Hide();
                    Utilities_muaban.hasProcess = false;
                    setTitleWindow(5, false);

                    //btn_start_muaban_timviec.Enabled = true;

                    txt_muaban_tv_trang.Enabled = true;
                    btn_muaban_tv_Tranggiam.Enabled = true;
                    btn_muaban_tv_Trangtang.Enabled = true;

                    lbl_message_muaban_tv_1.Visible = false;
                    lbl_message_muaban_tv_2.Visible = false;

                    txt_muaban_tv_lanlap.Enabled = true;
                    btn_muaban_tv_lanlapgiam.Enabled = true;
                    btn_muaban_tv_lanlaptang.Enabled = true;

                    txt_muaban_tv_sleep.Enabled = true;
                    btn_muaban_tv_sleepgiam.Enabled = true;
                    btn_muaban_tv_sleeptang.Enabled = true;
                    txt_muaban_tv_timeout.Enabled = true;

                    txt_muaban_tv_timeout.Enabled = true;
                    cmd_muaban_tv_danhmuc.Enabled = true;
                    cmb_muaban_tv_nganhnghe.Enabled = true;
                    cmb_muaban_tv_tu.Enabled = true;
                    button6.Enabled = true;
                    cmb_muaban_tv_den.Enabled = true;
                    cmb_muaban_tv_hinhthuc.Enabled = true;
                    cmb_muaban_tv_thoigiandang.Enabled = true;
                    chk_muaban_tv_Trang_tatca.Enabled = true;
                    chk_muaban_tv_lanlap_tatca.Enabled = true;

                }

                /*Cấu hình controll*/
                Control.CheckForIllegalCrossThreadCalls = false;

                par = new ParameterizedThreadStart(ProcessMuaban);
                theardProcess = new Thread(par);

                arr = new ArrayList();
                arr.Add(lbl_message_muaban_tv_1);
                arr.Add(lbl_message_muaban_tv_2);
                arr.Add(lbl_message_muaban_tv_khoa);
                arr.Add(txt_muaban_tv_link);
                arr.Add(pro_muaban_tv);
                arr.Add(lbl_Par);



                //http://stackoverflow.com/questions/3542061/how-do-i-stop-a-thread-when-my-winform-application-closes
                theardProcess.IsBackground = true;
                theardProcess.Start(arr);
            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "button2_Click", "error1"));
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

        private void ProcessMuaban(object arrControl)
        {
            string strUrl = "";
            try
            {
                //----- Add control process from
                ArrayList arr1 = (ArrayList)arrControl;
                Label lbl_muaban_message1 = (Label)arr1[0];
                Label lbl_muaban_message2 = (Label)arr1[1];
                Label lbl_muaban_khoa = (Label)arr1[2];
                TextBox txt_muaban_tv_link = (TextBox)arr1[3];
                ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];
                Label lbl_par = (Label)arr1[5];

               
                //----- update display control
                lbl_muaban_message1.Update();
                lbl_muaban_message2.Update();
                lbl_muaban_khoa.Update();
                pro_muaban_tv.Update();

                double nMaxpage = 0;
                nMaxpage = (Math.Round((double)_modelTrang.TotalResult / _modelTrang.PageSize) >= (double)_modelTrang.TotalPagingMax ? _modelTrang.TotalPagingMax : Math.Round((double)_modelTrang.TotalResult / (double)_modelTrang.PageSize));


                strUrl = txt_muaban_tv_link.Text;
                pro_muaban_tv.Value = 0;
                pro_muaban_tv.Maximum = ConvertType.ToInt(nMaxpage);
                lbl_bds_Par.Text =  "0% Hoàn thành...";
                lbl_bds_Par.Update();


                int i = 0;
                do
                {
                    if (!Utilities_muaban.hasProcess)  break;
                    {
                        if (Utilities_muaban._PathLimit != -1 && Utilities_muaban._PathLimit == i)   break;
                        lbl_par.Text = Math.Round((i / nMaxpage) * 100, 0) + "% Hoàn thành...";
                        lbl_par.Update();
                        lbl_message_sotrang.Text = string.Format(" Kết quả {0} /{1} trang - trong {2}", i, nMaxpage, String.Format("{0:#,##0.##}", _modelTrang.TotalResult));
                        lbl_message_sotrang.Update();

                        i++;
                        string strUrl1 = "";
                        if (strUrl.IndexOf("?") > 0)
                            strUrl1 = string.Format(strUrl + "&cp={0}", i);
                        else
                            strUrl1 = string.Format(strUrl + "?cp={0}", i);

                        lbl_muaban_message1.Text = string.Format("Đang Xử Lý: {0}", cmb_muaban_tv_nganhnghe.SelectedText);
                        lbl_muaban_message1.Update();

                        Utilities_muaban.getPageInfo(strUrl1, lbl_message_muaban_tv_khoa, ref _modelTrang);
                        
                       

                        int solanlap = 0;
                        Utilities_muaban._IdDanhmuc = ConvertType.ToInt(cmd_muaban_tv_danhmuc.SelectedValue);
                        Utilities_muaban.getwebBrowser(strUrl1, ref solanlap, arrControl);

                        pro_muaban_tv.PerformStep();
                        pro_muaban_tv.Update();

                        
                    }
                } while (i <= nMaxpage);

                /*===================================================================*/
                lbl_muaban_message1.Text = Utilities_muaban.hasProcess ? "Hoàn thành load số liệu." : "Tạm dừng do người dùng!!!";
                btn_start_muaban_timviec.Text = "Start";
                setTitleWindow(5, false);
                Utilities_muaban.hasProcess = false;

                btn_start_muaban_timviec.Enabled = true;

                txt_muaban_tv_trang.Enabled = true;
                btn_muaban_tv_Tranggiam.Enabled = true;
                btn_muaban_tv_Trangtang.Enabled = true;

                //lbl_message_muaban_tv_1.Visible = false;
                //lbl_message_muaban_tv_2.Visible = false;

                txt_muaban_tv_lanlap.Enabled = true;
                btn_muaban_tv_lanlapgiam.Enabled = true;
                btn_muaban_tv_lanlaptang.Enabled = true;

                txt_muaban_tv_sleep.Enabled = true;
                btn_muaban_tv_sleepgiam.Enabled = true;
                btn_muaban_tv_sleeptang.Enabled = true;
                txt_muaban_tv_timeout.Enabled = true;

                txt_muaban_tv_timeout.Enabled = true;
                cmd_muaban_tv_danhmuc.Enabled = true;
                cmb_muaban_tv_nganhnghe.Enabled = true;
                cmb_muaban_tv_tu.Enabled = true;
                button6.Enabled = true;
                cmb_muaban_tv_den.Enabled = true;
                cmb_muaban_tv_hinhthuc.Enabled = true;
                cmb_muaban_tv_thoigiandang.Enabled = true;



                if (chk_muaban_tv_Trang_tatca.Checked)
                {
                    txt_muaban_tv_trang.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "ProcessMuabantimviec", "error1"));
                MessageBox.Show(ex.Message, "ProcessMuabantimviec");
            }
        }
        private void cmd_muaban_tv_danhmuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmd_muaban_tv_danhmuc.SelectedValue.ToString())
            {
                case "-1":
                    lbl_muaban_tv_hinhthuc.Visible = false;
                    lbl_muaban_tv_nganhnghe.Visible = false;
                    lbl_muaban_tv_tuden.Visible = false;
                    cmb_muaban_tv_nganhnghe.Visible = false;
                    cmb_muaban_tv_tu.Visible = false;
                    cmb_muaban_tv_den.Visible = false;
                    cmb_muaban_tv_hinhthuc.Visible = false;
                    button6.Visible = false;
                    cmb_muaban_tv_thoigiandang.Visible = false;
                    lbl_muaban_tv_thoigiandang.Visible = false;
                    break;
                case "1":
                case "2":
                    lbl_muaban_tv_hinhthuc.Visible = true;
                    lbl_muaban_tv_nganhnghe.Visible = true;
                    lbl_muaban_tv_tuden.Visible = true;
                    cmb_muaban_tv_nganhnghe.Visible = true;
                    cmb_muaban_tv_tu.Visible = true;
                    cmb_muaban_tv_den.Visible = true;
                    cmb_muaban_tv_hinhthuc.Visible = true;
                    button6.Visible = true;
                    cmb_muaban_tv_thoigiandang.Visible = true;
                    lbl_muaban_tv_thoigiandang.Visible = true;
                    break;
                case "3":
                case "4":

                    lbl_muaban_tv_hinhthuc.Visible = false;
                    lbl_muaban_tv_nganhnghe.Visible = false;
                    lbl_muaban_tv_tuden.Visible = false;
                    cmb_muaban_tv_nganhnghe.Visible = false;
                    cmb_muaban_tv_tu.Visible = false;
                    cmb_muaban_tv_den.Visible = false;
                    cmb_muaban_tv_hinhthuc.Visible = false;
                    button6.Visible = false;
                    cmb_muaban_tv_thoigiandang.Visible = true;
                    lbl_muaban_tv_thoigiandang.Visible = true;
                    break;
                default:
                    lbl_muaban_tv_hinhthuc.Visible = false;
                    lbl_muaban_tv_nganhnghe.Visible = false;
                    lbl_muaban_tv_tuden.Visible = false;
                    cmb_muaban_tv_nganhnghe.Visible = false;
                    cmb_muaban_tv_tu.Visible = false;
                    cmb_muaban_tv_den.Visible = false;
                    cmb_muaban_tv_hinhthuc.Visible = false;
                    button6.Visible = false;
                    cmb_muaban_tv_thoigiandang.Visible = false;
                    lbl_muaban_tv_thoigiandang.Visible = false;
                    break;
            }
            cmb_muaban_tv_nganhnghe.SelectedValue = -1;
            cmb_muaban_tv_tu.SelectedValue = 0;
            cmb_muaban_tv_den.SelectedValue = 0;
            cmb_muaban_tv_hinhthuc.SelectedValue = -1;

            BinddmNganhNghe(ConvertType.ToInt(cmd_muaban_tv_danhmuc.SelectedValue));
        }




        private void cmb_muaban_tv_nganhnghe_SelectedIndexChanged(object sender, EventArgs e)
        {
            getPathMuabantimviec();
        }
        private void cmb_muaban_tv_tu_SelectedIndexChanged(object sender, EventArgs e)
        {

            getPathMuabantimviec();
        }

        private void cmb_muaban_tv_den_SelectedIndexChanged(object sender, EventArgs e)
        {
            getPathMuabantimviec();
        }

        private void cmb_muaban_tv_hinhthuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            getPathMuabantimviec();
        }
        private void cmb_muaban_tv_thoigiandang_SelectedIndexChanged(object sender, EventArgs e)
        {
            getPathMuabantimviec();
        }
        private void getPathMuabantimviec()
        {
            string _strPath2 = "";
            string _strdk = "";
            if (cmd_muaban_tv_danhmuc.SelectedItem != null && ConvertType.ToInt(cmd_muaban_tv_danhmuc.SelectedValue) == 0)
            {
                _strPath2 = "";
                txt_muaban_tv_link.Text = "";
                return;
            }

            if (cmb_muaban_tv_nganhnghe.SelectedIndex != 0 && cmb_muaban_tv_nganhnghe.SelectedItem != null)
            {
                _strPath2 = cmb_muaban_tv_nganhnghe.SelectedValue.ToString();
            }
            else
            {
                _strPath2 = SQLDatabase.ExcDataTable(string.Format("Select path from dm_muaban where id ={0}", ConvertType.ToInt(cmd_muaban_tv_danhmuc.SelectedValue))).Rows[0][0].ToString(); //;
            }

            if (cmb_muaban_tv_tu.SelectedItem != null && cmb_muaban_tv_den.SelectedItem != null)
            {
                if (!cmb_muaban_tv_tu.SelectedValue.Equals(0) ||
                    !cmb_muaban_tv_den.SelectedValue.Equals(0))
                {
                    if (_strdk.Length == 0)
                        _strdk += string.Format("?min={0}&max={1}", ConvertType.ToInt(cmb_muaban_tv_tu.SelectedValue), ConvertType.ToInt(cmb_muaban_tv_den.SelectedValue));
                    else
                        _strdk += string.Format("&min={0}&max={1}", cmb_muaban_tv_tu.SelectedValue, cmb_muaban_tv_den.SelectedValue);
                }
            }

            if (cmb_muaban_tv_hinhthuc.SelectedItem != null &&
                cmb_muaban_tv_hinhthuc.SelectedValue.ToString() != "-1")
            {
                if (_strdk.Length == 0)
                    _strdk += string.Format("?job={0}", cmb_muaban_tv_hinhthuc.SelectedValue);
                else
                    _strdk += string.Format("&job={0}", cmb_muaban_tv_hinhthuc.SelectedValue);

            }


            if (cmb_muaban_tv_thoigiandang.SelectedItem != null &&
                cmb_muaban_tv_thoigiandang.SelectedValue.ToString() != "0")
                if (_strdk.Length == 0)
                    _strdk += string.Format("?time={0}", cmb_muaban_tv_thoigiandang.SelectedValue);
                else
                    _strdk += string.Format("&time={0}", cmb_muaban_tv_thoigiandang.SelectedValue);


            if (_strdk.Length > 1)
                txt_muaban_tv_link.Text = _strPath2 + _strdk;
            else
                txt_muaban_tv_link.Text = _strPath2;

            if (txt_muaban_tv_link.Text != "")
            {
                Utilities_muaban.getPageInfo(txt_muaban_tv_link.Text, lbl_message_muaban_tv_khoa, ref _modelTrang);
            }
            lbl_message_sotrang.Text = string.Format(" Kết quả 1 - {0} trong {1}", _modelTrang.PageSize, String.Format("{0:#,##0.##}", _modelTrang.TotalResult));


        }

        private void button6_Click(object sender, EventArgs e)
        {
            mụcLụcToolStripMenuItem1_Click(null, null);
        }

        private void xuấtFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmExporttimviec frm = new frmExporttimviec();
                frm.strDatabaseName = _strDatabase;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region batdongsan.com.vn
        public DataTable CreateTable_batdongsan()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id_chon", typeof(int));
            table.Columns.Add("name_chon", typeof(string));
            table.Columns.Add("path_chon", typeof(string));
            table.Columns.Add("parentid_chon", typeof(string));
            table.Columns.Add("trang_chon", typeof(string));
            table.Columns.Add("end_chon", typeof(string));
            table.Columns.Add("orderid_chon", typeof(string));
            return table;
        }

        public void Binddmbds()
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("select ID,NAME from dm_batdongsan where parentId is null "));
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(int));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add(-1, "---Chọn Nhóm ---");
            foreach (DataRow item in table.Rows)
                table_nhom.Rows.Add(item["id"], item["name"]);

            cmb_bds_nhom.DataSource = table_nhom;
            cmb_bds_nhom.ValueMember = "id";
            cmb_bds_nhom.DisplayMember = "name";
            cmb_bds_nhom.SelectedValue = -1;
        }

        private void mụcLụcToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmDM_batdongsan frm = new frmDM_batdongsan();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //BinddmNganhNghe(ConvertType.ToInt(cmd_muaban_tv_danhmuc.SelectedValue));
            }
        }

        public void Bindbds(object nhom)
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("[spFindDmbds] '{0}'", ConvertType.ToInt(nhom)));
            IEnumerable<DataRow> query = from order in table.AsEnumerable()
                                         .Where(s => s.Field<string>("path").Length == 0)
                                         select order;

            gw_bds_goc.DataSource = table;
        }

        private void cmb_bds_nhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmb_bds_nhom.SelectedItem != null)
                    Bindbds(cmb_bds_nhom.SelectedValue);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btn_bds_chon_Click(object sender, EventArgs e)
        {
            try
            {
                int id = 0;
                string parentId = "";
                string name = "";
                string path = "";
                string orderid = "";
                foreach (DataGridViewRow row in gw_bds_goc.SelectedRows)
                {
                    DataRow[] result = _table_batdongsan.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_bds_goc"].Value)));

                    if (result.Count() == 0 && row.Cells["path_bds_goc"].Value != null)
                    {
                        id = ConvertType.ToInt(row.Cells["id_bds_goc"].Value);
                        name = row.Cells["name_bds_goc"].Value.ToString();
                        path = row.Cells["path_bds_goc"].Value == null ? "" : row.Cells["path_bds_goc"].Value.ToString();
                        parentId = row.Cells["parentId_bds_goc"].Value == null ? "" : row.Cells["parentId_bds_goc"].Value.ToString();
                        orderid = row.Cells["orderid_bds_goc"].Value == null ? "" : row.Cells["orderid_bds_goc"].Value.ToString();

                        _table_batdongsan.Rows.Add(id, name, path, parentId,0,-1, orderid);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }
        private void btn_bds_bo_Click(object sender, EventArgs e)
        {
            try
            {
                if (gw_bds_chon.SelectedRows.Count == 0) return;
                foreach (DataGridViewRow row in gw_bds_chon.SelectedRows)
                {
                    _table_batdongsan.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_bds_chon"].Value)))[0].Delete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button3_Click");
            }
        }
        private void btn_bds_xoa_Click(object sender, EventArgs e)
        {
            try
            {
                _table_batdongsan.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button4_Click");
            }
        }
        private void btn_bds_start_Click(object sender, EventArgs e)
        {
            ParameterizedThreadStart par;
            ArrayList arr;
            try
            {
                if (gw_bds_chon.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn danh mục", "Thông báo");
                    return;
                }
                if (btn_bds_start.Text == "Start")
                {
                    btn_bds_start.Text = "Stop";
                    setTitleWindow(6, true);

                    Utilities_batdongsan.hasProcess = true;
                    btn_bds_chon.Enabled = false;
                    btn_bds_xoa.Enabled = false;
                    btn_bds_bo.Enabled = false;
                    cmb_bds_nhom.Enabled = false;
                 
                    lbl_bds_message1.Visible = true;
                    lbl_bds_message2.Visible = true;
                    lbl_bds_khoa.Visible = true;
                    gw_bds_chon.Enabled = false;
                    gw_bds_goc.Enabled = false;

                    pic_bds_save.Enabled = false;
                    pic_bds_out.Enabled = false;

                  

                    txt_bds_lanlap.Enabled = false;
                    btn_bds_lanlap_giam.Enabled = false;
                    btn_bds_lanlap_tang.Enabled = false;

                    txt_bds_sleep.Enabled = false;
                    btn_bds_sleep_tang.Enabled = false;
                    btn_bds_sleep_giam.Enabled = false;


                    txt_bds_timeout.Enabled = false;
                    btn_bds_timeout_giam.Enabled = false;
                    btn_bds_timeout_tang.Enabled = false;

                    Utilities_batdongsan._Timeout = ConvertType.ToInt(txt_bds_timeout.Text);
                    Utilities_batdongsan._Sleep = ConvertType.ToInt(txt_bds_sleep.Text);
                    Utilities_batdongsan._thanhcong = 0;
                    Utilities_batdongsan._thatbai = 0;
                    /*kiem tra neu checkpath dc chon thi luu pathlimit vao gioi hang*/
                   

                    if (!chk_muaban_tv_lanlap_tatca.Checked)
                        Utilities_batdongsan._lanquetlai = ConvertType.ToInt(txt_bds_lanlap.Text);
                    else
                        Utilities_batdongsan._lanquetlai = -1;


                    /*load danh sách đầu số*/

                    DataTable tb_dausp = SQLDatabase.ExcDataTable("select distinct dauso dauso,lenght " +
                                                  "  from dau_so where dauso is not null and dauso <> ''");

                    Dictionary<string, int> dauso = new Dictionary<string, int>();
                    foreach (DataRow item in tb_dausp.Rows)
                    {
                        dauso.Add(item["dauso"].ToString(), ConvertType.ToInt(item["lenght"].ToString()));
                    }
                    Utilities_batdongsan._dau_so = dauso;
                    Utilities_batdongsan._regexs = SQLDatabase.LoadRegexs("select * from Regexs order by OrderID desc");

                   
                }
                else
                {
                    btn_bds_start.Text = "Start";
                    Utilities_batdongsan.hasProcess = false;
                    setTitleWindow(6, false);

                    btn_bds_chon.Enabled = true;
                    btn_bds_xoa.Enabled = true;
                    btn_bds_bo.Enabled = true;
                    cmb_bds_nhom.Enabled = true;
                  
                    lbl_bds_message1.Visible = false;
                    lbl_bds_message2.Visible = false;
                    lbl_bds_khoa.Visible = false;
                    gw_bds_chon.Enabled = true;
                    gw_bds_goc.Enabled = true;

                    pic_bds_save.Enabled = true;
                    pic_bds_out.Enabled = true;

                 
                    txt_bds_lanlap.Enabled = true;
                    btn_bds_lanlap_giam.Enabled = true;
                    btn_bds_lanlap_tang.Enabled = true;

                    txt_bds_sleep.Enabled = true;
                    btn_bds_sleep_tang.Enabled = true;
                    btn_bds_sleep_giam.Enabled = true;


                    txt_bds_timeout.Enabled = true;
                    btn_bds_timeout_giam.Enabled = true;
                    btn_bds_timeout_tang.Enabled = true;

                   
                }

                /*Cấu hình controll*/
                Control.CheckForIllegalCrossThreadCalls = false;
                par = new ParameterizedThreadStart(ProcessBatDongSan);
                theardProcess = new Thread(par);

                arr = new ArrayList();
                arr.Add(lbl_bds_message1);
                arr.Add(lbl_bds_message2);
                arr.Add(lbl_bds_khoa);
                arr.Add(lbl_bds_Par);
                arr.Add(progressBar1);

                //http://stackoverflow.com/questions/3542061/how-do-i-stop-a-thread-when-my-winform-application-closes
                theardProcess.IsBackground = true;
                theardProcess.Start(arr);
            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "button2_Click", "error1"));
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }
        private void ProcessBatDongSan(object arrControl)
        {
            try
            {
                //----- Add control process from
                ArrayList arr1 = (ArrayList)arrControl;
                Label lbl_bds_message1 = (Label)arr1[0];
                Label lbl_bds_message2 = (Label)arr1[1];
                Label lbl_bds_khoa = (Label)arr1[2];
                Label lbl_bds_Par = (Label)arr1[3];
                ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];
                /*===============================================================*/
                int x = 0;
                string strUrl = gw_bds_chon.Rows[x].Cells["path_bds_chon"].Value.ToString();
                while (gw_bds_chon.Rows.Count > 0)
                {
                    if (!Utilities_batdongsan.hasProcess) break;

                    int id = Convert.ToInt32(gw_bds_chon.Rows[x].Cells["id_bds_chon"].Value);
                    string name1 = gw_bds_chon.Rows[x].Cells["name_bds_chon"].Value.ToString();
                    int pathlimit = Convert.ToInt32(gw_bds_chon.Rows[x].Cells["end_chon"].Value);
                    Utilities_batdongsan._PathLimit = pathlimit;

                    string name = SQLDatabase.ExcDataTable(string.Format(" select name = (select name from dm_batdongsan b where b.id=a.parentId) from dm_batdongsan a where id = {0}", id)).Rows[0]["name"].ToString();
                    Utilities_batdongsan._danhmuc = name1;

                    infoPathmuaban model = new infoPathmuaban();
                    switch (name.ToLower().Trim())
                    {
                        case "nhà đất bán":
                        case "nhà đất cho thuê":
                            model = Utilities_batdongsan.getPageMaxBanCho(strUrl, arrControl);
                            break;
                        case "nhà đất cần mua":
                        case "nhà đất cần thuê":
                            model = Utilities_batdongsan.getPageMaxMuaThue(strUrl, arrControl);
                            break;
                        case "danh bạ":
                            model = Utilities_batdongsan.getPageMaxNhamoigioi(strUrl, arrControl);
                            break;
                    }

                    DataTable tblFiltered = _table_batdongsan.AsEnumerable()
                                               .Where(row => row.Field<int>("id_chon") == id)
                                               .CopyToDataTable();
                    if (tblFiltered != null && tblFiltered.Rows.Count != 0)
                        model.PageNow = ConvertType.ToInt(tblFiltered.Rows[0]["trang_chon"]);
                    model.TotalPagingMax  = pathlimit == -1 ? model.TotalPagingMax : pathlimit > model.TotalPagingMax ? model.TotalPagingMax : pathlimit;

                    Utilities_batdongsan._infoPathmuaban = model;
                    /*refesh qua trang mới*/
                    pro_muaban_tv.Maximum = model.TotalPagingMax;
                    pro_muaban_tv.Minimum = model.PageNow;
                    pro_muaban_tv.Value = model.PageNow;
                    
                    lbl_bds_Par.Text = "0% Hoàn thành...";
                    lbl_bds_Par.Update();


                    int i = model.PageNow;
                    do
                    {
                        if (!Utilities_batdongsan.hasProcess)
                            break;
                        {
                            /*kiễm tra trang*/
                            if (Utilities_batdongsan._PathLimit != -1 && Utilities_batdongsan._PathLimit == i)   break;
                            lbl_bds_Par.Text = Math.Round((i / (double)model.TotalPagingMax) * 100, 0) + "% Hoàn thành...";
                            lbl_bds_Par.Update();

                            lbl_bds_message1.Text = string.Format("Đang Xử Lý: {0} - Tổng số dự kiến :{1}   ->Trang đang quét {2}/{3} trang", gw_bds_chon.Rows[x].Cells["name_bds_chon"].Value, model.TotalResult == 0 ? "--" : String.Format("{0:#,##0.##}",  model.TotalResult),i, String.Format("{0:#,##0.##}", model.TotalPagingMax));
                            lbl_bds_message1.Update();


                            i = i + 1;

                            string strUrl1 = "";
                            strUrl1 = string.Format(strUrl + "/p{0}", i);

                            int solanlap = 0;
                            Utilities_batdongsan._IdDanhmuc = id;
                           
                            switch (name.ToLower().Trim())
                            {
                                case "danh bạ":
                                    Utilities_batdongsan.getwebBrowserNhamoigioi(strUrl1, ref solanlap, arrControl);
                                    break;
                                case "nhà đất bán":
                                case "nhà đất cho thuê":
                                    Utilities_batdongsan.getwebBrowser_Canban(strUrl1, ref solanlap, arrControl);
                                    break;
                                case "nhà đất cần mua":
                                case "nhà đất cần thuê":
                                    Utilities_batdongsan.getwebBrowser_Canmua(strUrl1, ref solanlap, arrControl);
                                    break;
                            }
                        }

                        pro_muaban_tv.PerformStep();
                        pro_muaban_tv.Update();

                        
                    } while (i <= model.TotalPagingMax);

                    /*neu dang stop thi khong dc phep xoa*/
                    if (Utilities_batdongsan.hasProcess)
                    {
                        if (_table_batdongsan.Select(string.Format("id_chon={0}", id)).Count() != 0)
                            _table_batdongsan.Select(string.Format("id_chon={0}", id))[0].Delete();
                    }
                }

                /*===================================================================*/
                lbl_bds_message1.Text = Utilities_batdongsan.hasProcess ? "Hoàn thành load số liệu." : "Tạm dừng do người dùng!!!";
                btn_bds_start.Text = "Start";
                Utilities_batdongsan.hasProcess = false;
                setTitleWindow(6, false);

                btn_bds_chon.Enabled = true;
                btn_bds_xoa.Enabled = true;
                btn_bds_bo.Enabled = true;
                cmb_bds_nhom.Enabled = true;
               
                lbl_bds_khoa.Visible = false;
                gw_bds_chon.Enabled = true;
                gw_bds_goc.Enabled = true;

                pic_bds_save.Enabled = true;
                pic_bds_out.Enabled = true;

             

                txt_bds_lanlap.Enabled = true;
                btn_bds_lanlap_giam.Enabled = true;
                btn_bds_lanlap_tang.Enabled = true;

                txt_bds_sleep.Enabled = true;
                btn_bds_sleep_tang.Enabled = true;
                btn_bds_sleep_giam.Enabled = true;


                txt_bds_timeout.Enabled = true;
                btn_bds_timeout_giam.Enabled = true;
                btn_bds_timeout_tang.Enabled = true;

             

            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "ProcessBatDongSan", "error1"));
                MessageBox.Show(ex.Message, "ProcessBatDongSan");
            }
        }

        private void pictureBox2_Click_2(object sender, EventArgs e)
        {
            if (SavaAs_batdongsan())
                MessageBox.Show("Lưu dữ liệu thành công", "Thông Báo");
        }
        private bool SavaAs_batdongsan()
        {
            PleaseWait objPleaseWait = null;

            if (_table_batdongsan.Rows.Count > 0)
            {
                SaveAs sa = new SaveAs();
                objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();

                foreach (DataRow item in _table_batdongsan.Rows)
                {
                    string path = item["path_chon"].ToString();
                    DataTable tb = SQLDatabase.ExcDataTable(string.Format("select count(*) from SaveAs where scannerby='{0}' and path_chon='{1}'", "batdongsan.com.vn", item["path_chon"].ToString()));
                    if (ConvertType.ToInt(tb.Rows[0][0]) == 0)
                    {
                        SQLDatabase.AddSaveAs(new SaveAs()
                        {
                            id_chon = item["id_chon"].ToString(),
                            name_chon = item["name_chon"].ToString(),
                            path_chon = item["path_chon"].ToString(),
                            parentid_chon = item["parentid_chon"].ToString(),
                            orderid_chon = item["orderid_chon"].ToString(),
                            scannerby = "batdongsan.com.vn",
                        });
                    }
                }
                if (objPleaseWait != null)
                    objPleaseWait.Close();

                return true;
            }
            else
            {
                return true;
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

            try
            {
                List<SaveAs> save = new List<SaveAs>();
                frmSaveAs frm = new frmSaveAs();
                frm.fromparent = "batdongsan.com.vn";
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    save = frm.Saveas;
                    if (save.Count() > 0)
                    {
                        foreach (SaveAs item in save)
                        {
                            if (_table_batdongsan.Select(string.Format("path_chon = '{0}'", item.path_chon)).Count() == 0)
                                _table_batdongsan.Rows.Add(ConvertType.ToInt(item.id_chon),
                                                            item.name_chon,
                                                            item.path_chon,
                                                            item.parentid_chon,
                                                            item.orderid_chon);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "danhMucToolStripMenuItem1_Click");
            }

        }

        private void xuấtFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                frmExportbatdongsan frm = new frmExportbatdongsan();
                frm.strDatabaseName = _strDatabase;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void btn_bds_add_Click(object sender, EventArgs e)
        {
            try
            {
                frmthemLinkVinabix frm = new frmthemLinkVinabix();
                frm.fromparent = "batdongsan.com.vn";
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string name = frm.Ten;
                    string danhmucName = frm.DanhmucName;
                    string path = frm.Link;
                    int parentId = 0;
                    int orderid = 0;
                    _table_batdongsan.Rows.Add(frm.DanhmucId,string.Format("{0}   -> {1}", name,danhmucName), path, parentId, frm.Trang,frm.PathLimit, orderid);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "danhMucToolStripMenuItem1_Click");
            }
        }



        #endregion

        #region vinabiz.org
        public DataTable CreateTable_vinabiz()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id_chon", typeof(int));
            table.Columns.Add("name_chon", typeof(string));
            table.Columns.Add("path_chon", typeof(string));
            table.Columns.Add("parentid_chon", typeof(string));
            table.Columns.Add("alevel", typeof(string));
            table.Columns.Add("trang_chon", typeof(string));
            table.Columns.Add("end_chon", typeof(string));
            table.Columns.Add("orderid_chon", typeof(string));
            return table;
        }
        private void xuấtFileToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                frmExportVinabiz frm = new frmExportVinabiz();
                frm.strDatabaseName = _strDatabase;
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
        private void danhMụcToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                frm_dmVinabiz frm = new frm_dmVinabiz();
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
        private void btn_vnbizdanhmuc_Click(object sender, EventArgs e)
        {
            danhMụcToolStripMenuItem1_Click(null, null);
        }


        public void BinddmVinabiz()
        {
            getdataVinabiz();
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(int));
            table_nhom.Columns.Add("name", typeof(string));

            table_nhom.Rows.Add(-1, "---Chọn Tỉnh / Huyện---");

            foreach (DataRow item in _danhmucVinabiz.Select(string.Format("alevel=0")))
            {
                table_nhom.Rows.Add(item["id"], string.Format("Tp/Tỉnh -->{0}", item["name"]));
                foreach (dm_vinabiz item2 in SQLDatabase.Loaddm_vinabiz(string.Format("SELECT * FROM DM_VINABIZ WHERE parentId={0}", item["id"])))
                {
                    table_nhom.Rows.Add(item2.id, string.Format(" Quận/Huyện:  --> {0}", item2.name));
                }
            }


            cmb_vnBizNhom.DataSource = table_nhom;
            cmb_vnBizNhom.ValueMember = "id";
            cmb_vnBizNhom.DisplayMember = "name";

        }
        public void BindVietBiz(object nhom)
        {
            
            if (nhom == null || nhom.ToString() =="-1")
                return;
            if (nhom.ToString() == "System.Data.DataRowView")
                return;
            getdataVinabiz();
            try
            {
                DataTable tb_new = _table_vinabiz.Clone();
                tb_new.Clear();

              
                

                DataRow mang = _danhmucVinabiz.Select(string.Format("id={0}",ConvertType.ToInt(nhom))).FirstOrDefault();
                if (mang == null) return;
                
                if (mang["alevel"].Equals(0))
                {
                    tb_new.Rows.Add(mang["id"].ToString(), string.Format("Tỉnh/Thành: {0}", mang["name"]), mang["path"], mang["parentId"], mang["alevel"],0, mang["orderid"]);

                    DataRow[] dmQ = _danhmucVinabiz.Select(string.Format("parentId={0}", mang["id"]));
                    foreach (DataRow quan in dmQ)
                    {
                        tb_new.Rows.Add(quan["id"], string.Format("Quận/Huyện: {0}", quan["name"]), quan["path"], quan["parentId"], quan["alevel"],0, quan["orderid"]);
                        DataRow[] dmQ1 = _danhmucVinabiz.Select(string.Format("parentId={0}", quan["id"]));
                        foreach (DataRow item in dmQ1)
                        {
                            tb_new.Rows.Add(item["id"], string.Format("Phường/Xã: {0}", item["name"]), item["path"], item["parentId"], item["alevel"],0, item["orderid"]);
                        }
                    }
                }
                else
                {
                    tb_new.Rows.Add(mang["id"].ToString(), string.Format("Quận/Huyện: {0}", mang["name"]), mang["path"], mang["parentId"], mang["alevel"],0, mang["orderid"]);

                    DataRow[] dmQ1 = _danhmucVinabiz.Select(string.Format("parentId={0}", mang["id"]));
                    foreach (DataRow item in dmQ1)
                    {
                        tb_new.Rows.Add(item["id"], item["name"],  item["path"], item["parentId"], item["alevel"],0, item["orderid"]);
                    }
                }
                gw_vinabiz_goc.DataSource = tb_new;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BindDMSanPham", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void cmb_vnBizNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindVietBiz(cmb_vnBizNhom.SelectedValue);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void btn_vinabiz_Them_Click(object sender, EventArgs e)
        {
            try
            {
                int id = 0;
                string parentId = "";
                string name = "";
                string path = "";
                string orderid = "";

                //DataGridViewSelectedRowCollection chon = gw_vinabiz_goc.SelectedRows;

                foreach (DataGridViewRow row in gw_vinabiz_goc.SelectedRows)
                {
                    DataRow[] result= _table_vinabiz.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_vnbiz_goc"].Value)));

                    int khuvuc = radioButton1.Checked ? 0 : radioButton2.Checked ? 1 : radioButton3.Checked ? 2 : 3;
                    if (result.Count() == 0 && row.Cells["path_vnbiz_goc"].Value != null && row.Cells["alevel"].Value.Equals(khuvuc.ToString()))
                    {
                        id = ConvertType.ToInt(row.Cells["id_vnbiz_goc"].Value);
                        name = row.Cells["name_vnbiz_goc"].Value.ToString();
                        path = row.Cells["path_vnbiz_goc"].Value == null ? "" : row.Cells["path_vnbiz_goc"].Value.ToString();
                        parentId = row.Cells["parentId_vnbiz_goc"].Value == null ? "" : row.Cells["parentId_vnbiz_goc"].Value.ToString();
                        orderid = row.Cells["orderid_vnbiz_goc"].Value == null ? "" : row.Cells["orderid_vnbiz_goc"].Value.ToString();

                       
                        _table_vinabiz.Rows.Add(id, name, path, parentId,0,0,-1, orderid);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                frmthemLinkVinabix frm = new frmthemLinkVinabix();
                frm.fromparent = "vinabiz.org";
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    int id = -1;
                    string name = frm.Ten;
                    string path = frm.Link;
                    int parentId = 0;
                    int orderid = 0;
                    _table_vinabiz.Rows.Add(id, name, path, parentId,0,frm.Trang,frm.PathLimit, orderid);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "danhMucToolStripMenuItem1_Click");
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
        private void btn_vinabiz_Bo_Click(object sender, EventArgs e)
        {
            try
            {
                if (gw_vinabiz_chon.SelectedRows.Count == 0) return;
                foreach (DataGridViewRow row in gw_vinabiz_chon.SelectedRows)
                {
                    _table_vinabiz.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_vnbiz_chon"].Value)))[0].Delete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button3_Click");
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {
            ParameterizedThreadStart par;
            ArrayList arr;
            try
            {
                if (gw_vinabiz_chon.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ngành nghề kinh doanh", "Thông báo");
                    return;
                }

                Utilities_vinabiz._Timeout = ConvertType.ToInt(txt_trangvang_timeout.Text);
                Utilities_vinabiz._Sleep = ConvertType.ToInt(txt_trangvang_sleep.Text);
                Utilities_vinabiz._listquetcan = SQLDatabase.Loaddm_vinabiz_map("select * from dm_vinabiz_map");
                Utilities_vinabiz._regexs = SQLDatabase.LoadRegexs("select * from Regexs");
                /*load danh sách đầu số*/
                DataTable tb_dausp = SQLDatabase.ExcDataTable("select distinct dauso dauso,lenght " +
                                                   "  from dau_so where dauso is not null and dauso <> ''");

                Dictionary<string, int> dauso = new Dictionary<string, int>();
                foreach (DataRow item in tb_dausp.Rows)
                {
                    dauso.Add(item["dauso"].ToString(), ConvertType.ToInt(item["lenght"]));
                }
                Utilities_vinabiz._dau_so = dauso;

                if (btn_vinabiz_Start.Text == "Start")
                {
                    btn_vinabiz_Start.Text = "Stop";
                    setTitleWindow(7, true);

                    Utilities_vinabiz.hasProcess = true;
                    Utilities_vinabiz._Timeout = ConvertType.ToInt( txt_vinabiz_timeout.Text);
                    Utilities_vinabiz._Sleep = ConvertType.ToInt(txt_vinabiz_sleep.Text);
                    Utilities_vinabiz._listHsct = _listHsct;
                    Utilities_vinabiz._listTinh = _listdm_Tinh;
                    btn_vinabiz_Them.Enabled = false;
                    btn_vinabiz_Xoa.Enabled = false;
                    btn_vinabiz_Bo.Enabled = false;


                    cmb_vnBizNhom.Enabled = false;
                    chk_vinabiz_lanlap.Enabled = false;
                   

                    pic_vnbiz_import.Enabled = false;
                    pic_vnbiz_out.Enabled = false;


                    lbl_vinabiz_message1.Visible = true;
                    lbl_vinabiz_message2.Visible = true;
                    lbl_vinabiz_khoa.Visible = true;

                    gw_vinabiz_goc.Enabled = false;
                    gw_vinabiz_chon.Enabled = false;

                    txt_vinabiz_sleep.Enabled = false;
                    btn_vinabiz_sleep_giam.Enabled = false;
                    btn_vinabiz_sleep_tang.Enabled = false;

                    txt_vinabiz_lanlap.Enabled = false;
                    btn_vinabiz_lanlap_giam.Enabled = false;
                    btn_vinabiz_lanlap_tang.Enabled = false;

                    
                    txt_vinabiz_timeout.Enabled = false;
                    btn_vinabiz_timeout_giam.Enabled = false;
                    btn_vinabiz_timeout_tang.Enabled = false;

                    

                    if (!chk_vinabiz_lanlap.Checked)
                        Utilities_vinabiz._lanquetlai = ConvertType.ToInt(txt_vinabiz_lanlap.Text);
                    else
                        Utilities_vinabiz._lanquetlai = -1;

                }
                else
                {
                    btn_vinabiz_Start.Text = "Start";
                    Utilities_vinabiz.hasProcess = false;
                    setTitleWindow(7, false);

                    btn_vinabiz_Them.Enabled = true;
                    btn_vinabiz_Xoa.Enabled = true;
                    btn_vinabiz_Bo.Enabled = true;


                    cmb_vnBizNhom.Enabled = true;
                   
                    chk_vinabiz_lanlap.Enabled = true;
                   

                    pic_vnbiz_import.Enabled = true;
                    pic_vnbiz_out.Enabled = true;


                    lbl_vinabiz_message1.Visible = true;
                    lbl_vinabiz_message2.Visible = true;
                    lbl_vinabiz_khoa.Visible = true;

                    gw_vinabiz_goc.Enabled = true;
                    gw_vinabiz_chon.Enabled = true;

                    

                    txt_vinabiz_sleep.Enabled = true;
                    btn_vinabiz_sleep_giam.Enabled = true;
                    btn_vinabiz_sleep_tang.Enabled = true;

                    txt_vinabiz_lanlap.Enabled = true;
                    btn_vinabiz_lanlap_giam.Enabled = true;
                    btn_vinabiz_lanlap_tang.Enabled = true;


                    txt_vinabiz_timeout.Enabled = true;
                    btn_vinabiz_timeout_giam.Enabled = true;
                    btn_vinabiz_timeout_tang.Enabled = true;

                 

                    if (chk_vinabiz_lanlap.Checked)
                    {
                        txt_vinabiz_lanlap.Enabled = false;
                    }
                }

                /*Cấu hình controll*/
                Control.CheckForIllegalCrossThreadCalls = false;

                par = new ParameterizedThreadStart(ProcessVinabiz);
                theardProcess = new Thread(par);

                arr = new ArrayList();
                arr.Add(lbl_vinabiz_message1);
                arr.Add(lbl_vinabiz_message2);
                arr.Add(lbl_vinabiz_khoa);
                arr.Add(lbl_vinabiz_phantram);
                arr.Add(pr_vinabiz);

                //http://stackoverflow.com/questions/3542061/how-do-i-stop-a-thread-when-my-winform-application-closes
                theardProcess.IsBackground = true;
                theardProcess.Start(arr);
            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "button2_Click", "error1"));
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

       
        private void ProcessVinabiz(object arrControl)
        {
            try
            {
                //----- Add control process from
                
                ArrayList arr1 = (ArrayList)arrControl;
                Label lbl_vinabiz_message1 = (Label)arr1[0];
                Label lbl_vinabiz_message2 = (Label)arr1[1];
                Label lbl_vinabiz_khoa = (Label)arr1[2];
                Label lbl_vinabiz_phantram = (Label)arr1[3];
                ProgressBar pr_vinabiz = (ProgressBar)arr1[4];
                
                /*===============================================================*/
                int x = 0;
                string strUrl = "";
                while (gw_vinabiz_chon.Rows.Count > 0)
                {
                    if (!Utilities_vinabiz.hasProcess) break;

                    int id = Convert.ToInt32(gw_vinabiz_chon.Rows[x].Cells["id_vnbiz_chon"].Value);
                    string strPath = gw_vinabiz_chon.Rows[x].Cells["path_vnbiz_chon"].Value.ToString();
                    string strName = gw_vinabiz_chon.Rows[x].Cells["name_vnbiz_chon"].Value.ToString();
                    int pathlimit = Convert.ToInt32(gw_vinabiz_chon.Rows[x].Cells["end_chon"].Value);
                    Utilities_vinabiz._PathLimit = pathlimit;

                   

                    strUrl = strPath.Replace(" ", "").Replace("\t", "");


                   

                    infoPathmuaban pagemax = Utilities_vinabiz.getPageMax(strUrl, arrControl);

                    DataTable tblFiltered = _table_vinabiz.AsEnumerable()
                                                .Where(row => row.Field<int>("id_chon") == id)
                                                .CopyToDataTable();
                    if (tblFiltered != null && tblFiltered.Rows.Count != 0)
                        pagemax.PageNow = ConvertType.ToInt(tblFiltered.Rows[0]["trang_chon"]);
                    pagemax.TotalPagingMax = pathlimit == -1 ? pagemax.TotalPagingMax : pathlimit > pagemax.TotalPagingMax ? pagemax.TotalPagingMax : pathlimit;

                    pr_vinabiz.Maximum = pagemax.TotalPagingMax;
                    pr_vinabiz.Value = pagemax.PageNow;
                    lbl_vinabiz_phantram.Text = "0% Hoàn thành...";
                    lbl_vinabiz_phantram.Update();

                    int i = pagemax.PageNow;
                    do
                    {
                        if (!Utilities_vinabiz.hasProcess)   break;
                        {
                            /*kiễm tra trang*/
                            if (Utilities_vinabiz._PathLimit != -1 && Utilities_trangvang._PathLimit == i)   break;
                            lbl_vinabiz_message1.Text = string.Format("Đang Xử Lý: {0} \n Trang: {1}\\ Tổng Trang: {2}", strName, i, pagemax.TotalPagingMax);
                            lbl_vinabiz_message1.Update();

                            lbl_vinabiz_phantram.Text = Math.Round((i / (double)pagemax.TotalPagingMax) * 100, 0) + "% Hoàn thành...";
                            lbl_vinabiz_phantram.Update();
                            //lbl_message_sotrang.Text = string.Format(" Kết quả {0} /{1} trang - trong {2}", i, pagemax.TotalPagingMax, String.Format("{0:#,##0.##}", _modelTrang.TotalResult));
                            ///lbl_message_sotrang.Update();

                           

                            DataRow[] customerRow = _table_vinabiz.Select(string.Format("id_chon = '{0}'",id));
                            customerRow[0]["trang_chon"] = i;

                            if (i == 0) i = 1;
                            string strUrl1 = "";
                            strUrl1 = string.Format(strUrl + "/{0}", i);

                            int solanlap = 0;
                            //Utilities_vinabiz.IdDanhmuc = id;
                            Utilities_vinabiz.getwebBrowser(strUrl1, ref solanlap, arrControl);

                            i = i + 1;

                            pr_vinabiz.PerformStep();
                            pr_vinabiz.Update();
                        }
                    } while (i <= pagemax.TotalPagingMax);
                    /*neu dang stop thi khong dc phep xoa*/
                    if (Utilities_vinabiz.hasProcess)
                        if (_table_vinabiz.Select(string.Format("id_chon={0}", id)).Count() != 0)
                            _table_vinabiz.Select(string.Format("id_chon={0}", id))[0].Delete();
                }
                /*===================================================================*/
                lbl_vinabiz_message1.Text = Utilities_vinabiz.hasProcess ? "Hoàn thành load số liệu." : "Tạm dừng do người dùng!!!";
                if (Utilities_vinabiz.hasProcess)
                {
                    Utilities_vinabiz._thanhcong = 0;
                    Utilities_vinabiz._thatbai = 0;
                }

                btn_vinabiz_Start.Text = "Start";
                Utilities_vinabiz.hasProcess = false;
                setTitleWindow(7, false);

                btn_vinabiz_Them.Enabled = true;
                btn_vinabiz_Xoa.Enabled = true;
                btn_vinabiz_Bo.Enabled = true;


                cmb_vnBizNhom.Enabled = true;
            
                chk_vinabiz_lanlap.Enabled = true;
               

                pic_vnbiz_import.Enabled = true;
                pic_vnbiz_out.Enabled = true;


                //lbl_vinabiz_message1.Visible = false;
                //lbl_vinabiz_message2.Visible = false;
                lbl_vinabiz_khoa.Visible = false;

                gw_vinabiz_goc.Enabled = true;
                gw_vinabiz_chon.Enabled = true;

              

                txt_vinabiz_sleep.Enabled = true;
                btn_vinabiz_sleep_giam.Enabled = true;
                btn_vinabiz_sleep_tang.Enabled = true;

                txt_vinabiz_lanlap.Enabled = true;
                btn_vinabiz_lanlap_giam.Enabled = true;
                btn_vinabiz_lanlap_tang.Enabled = true;


                txt_vinabiz_timeout.Enabled = true;
                btn_vinabiz_timeout_giam.Enabled = true;
                btn_vinabiz_timeout_tang.Enabled = true;

               

                if (chk_vinabiz_lanlap.Checked)
                {
                    txt_vinabiz_lanlap.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "ProcessTrangVang", "error1"));
                MessageBox.Show(ex.Message, "ProcessTrangVang");
            }
        }

        private bool SavaAs_vinabiz()
        {
            PleaseWait objPleaseWait = null;

            if (_table_vinabiz.Rows.Count > 0)
            {
                SaveAs sa = new SaveAs();
                objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();

                foreach (DataRow item in _table_batdongsan.Rows)
                {
                    string path = item["path_chon"].ToString();
                    DataTable tb = SQLDatabase.ExcDataTable(string.Format("select count(*) from SaveAs where scannerby='{0}' and path_chon='{1}'", "batdongsan.com.vn", item["path_chon"].ToString()));
                    if (ConvertType.ToInt(tb.Rows[0][0]) == 0)
                    {
                        SQLDatabase.AddSaveAs(new SaveAs()
                        {
                            id_chon = item["id_chon"].ToString(),
                            name_chon = item["name_chon"].ToString(),
                            path_chon = item["path_chon"].ToString(),
                            parentid_chon = item["parentid_chon"].ToString(),
                            orderid_chon = item["orderid_chon"].ToString(),
                            scannerby = "batdongsan.com.vn",
                        });
                    }
                }
                if (objPleaseWait != null)
                    objPleaseWait.Close();

                return true;
            }
            else
            {
                return true;
            }
        }

        private void getdataVinabiz()
        {
            _danhmucVinabiz = SQLDatabase.ExcDataTable(" WITH temp(id, name,path,parentId,orderid, alevel)  as (  Select id, name, path,parentId, orderid,0 as aLevel  From dm_vinabiz  Where parentId is null  Union All  Select b.id, b.name, b.path,b.parentId,b.orderid, a.alevel + 1  From temp as a, dm_vinabiz as b  Where a.id = b.parentId  )  Select *  From temp");
        }
        private void gw_vinabiz_goc_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in this.gw_vinabiz_goc.Rows)
            {
                if (row.Cells["alevel"].Value.ToString() == "0")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
                }
                else if (row.Cells["alevel"].Value.ToString() == "1")
                {
                    row.DefaultCellStyle.BackColor = Color.Gray;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 8);
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 8);
                }
            }
        }
        #endregion


        #region Thị trường sĩ

        public void Binddmthitruongsi()
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("select ID,NAME from dm_thitruongsi where parentId is null "));
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(int));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add(-1, "---Chọn Nhóm Gian Hàng---");
            foreach (DataRow item in table.Rows)
                table_nhom.Rows.Add(item["id"], item["name"]);

            cmb_SiGoc.DataSource = table_nhom;
            cmb_SiGoc.ValueMember = "id";
            cmb_SiGoc.DisplayMember = "name";
            cmb_SiGoc.SelectedValue = -1;
        }
        public void Bindthitruongsi(object nhom)
        {
            if (nhom == null)
                return;
            if (nhom.ToString() == "System.Data.DataRowView")
                return;
            DataTable table = SQLDatabase.ExcDataTable(string.Format("[spFindDmthitruongsi] '{0}'", nhom));
            IEnumerable<DataRow> query = from order in table.AsEnumerable()
                                         .Where(s => s.Field<string>("path").Length == 0)
                                         select order;

            gw_si_goc.DataSource = table;
        }


        public bool Login()
        {
            
            try
            {
                bool islogin = false;

                wThiTruongSi.Dock = DockStyle.Fill;
                wThiTruongSi.ScriptErrorsSuppressed = true;
                wThiTruongSi.Visible = false;
                wThiTruongSi.Dock = DockStyle.Fill;
                this.Controls.Add(wThiTruongSi);
                wThiTruongSi.Navigate(@"https://thitruongsi.com/");

                // wait a little
                for (int i = 0; i < 100; i++)
                {
                    System.Threading.Thread.Sleep(10);
                    System.Windows.Forms.Application.DoEvents();
                }
                if (wThiTruongSi.Document == null)  Login();

                if (GetUsername().Length != 0) return true;

                //tìm đến button đăng nhập -> event click
                foreach (HtmlElement item in wThiTruongSi.Document.GetElementsByTagName("a"))
                {
                    if (item.OuterHtml.Contains("btn_login"))
                    {
                        item.InvokeMember("click");
                        islogin = true;
                        break;
                    }
                }
                if (!islogin) return false;
                #region set email
                HtmlElement temp = null;
                while (temp == null)
                {
                    temp = wThiTruongSi.Document.GetElementById("email_login");
                    System.Threading.Thread.Sleep(10);
                    System.Windows.Forms.Application.DoEvents();
                }

                // once we find it place the value
                temp.SetAttribute("value", txtSiusername.Text);
                #endregion

                #region set password
                temp = null;
                while (temp == null)
                {
                    temp = wThiTruongSi.Document.GetElementById("password_login");
                    System.Threading.Thread.Sleep(10);
                    System.Windows.Forms.Application.DoEvents();
                }
                temp.SetAttribute("value", txtSiPassword.Text);
                #endregion

                #region set click button đăng nhập
                var inputs = wThiTruongSi.Document.GetElementsByTagName("button");
                // iterate through all the inputs in the document
                foreach (HtmlElement btn in inputs)
                {
                    try
                    {
                        if (btn.InnerText.Contains("Đăng nhập"))
                        {
                            btn.InvokeMember("click");
                            break;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Login");
                return false;
            }
        }

     
        public string GetUsername()
        {
            try
            {
                string xxx = "";
                bool temp = true;
                while (temp)
                {
                    foreach (HtmlElement item in wThiTruongSi.Document.GetElementsByTagName("span"))
                    {
                        if (item.OuterHtml.Contains("top_user_name_txt"))
                        {
                            xxx = item.InnerHtml;
                            temp = false;
                        }
                    }
                    System.Threading.Thread.Sleep(10);
                    System.Windows.Forms.Application.DoEvents();
                    temp = false;
                }
                return xxx;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private void btnSiDangNhap_Click(object sender, EventArgs e)
        {
            PleaseWait objPleaseWait = null;
            //TODO: Stuff
            objPleaseWait = new PleaseWait();
            objPleaseWait.Show();
            objPleaseWait.Update();

            if (Login())
            {
                if (GetUsername() != "")
                {
                    lblSiUserName.Text = GetUsername();
                    objPleaseWait.Close();
                    MessageBox.Show("Đăng nhập thành công", "Thông Báo");
                    txtSiPassword.Enabled = false;
                    txtSiusername.Enabled = false;
                    btnSiDangNhap.Enabled = false;
                    
                }
            }
            else
            {
                objPleaseWait.Close();
                MessageBox.Show("Đăng nhập thất bại, vui lòng đăng nhập lại","Thông báo");
                txtSiPassword.Enabled = true;
                txtSiusername.Enabled = true;
                btnSiDangNhap.Enabled = true;
                
            }
        }

        private void btnSiStart_Click(object sender, EventArgs e)
        {
            ParameterizedThreadStart par;
            ArrayList arr;
            try
            {
                if (gw_si_chon.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn danh mục", "Thông báo");
                    return;
                }
                if (btnSiDangNhap.Enabled) {
                    MessageBox.Show("Vui lòng đăng nhập hệ thống website", "Thông báo");
                    btnSiDangNhap.Focus();
                    return;
                }
                if (btnSiStart.Text == "Start")
                {
                    btnSiStart.Text = "Stop";
                    setTitleWindow(8, true);
                    MyDownloader mydown = new MyDownloader();
                    mydown.setWebbrowser(wThiTruongSi);
                   Utilities_thitruongsi._down = mydown;
                    Utilities_thitruongsi.hasProcess = true;
                    btn_si_Them.Enabled = false;
                    btn_si_xoa.Enabled = false;
                    btn_si_bo.Enabled = false;

                    cmb_SiGoc.Enabled = false;
                   

                    lbl_si_message1.Visible = true;
                    lbl_si_message2.Visible = true;
                    lbl_si_khoa.Visible = true;

                    gw_si_chon.Enabled = false;
                    gw_si_goc.Enabled = false;

                    pic_si_save.Enabled = false;
                    pic_si_out.Enabled = false;

                 


                    txt_si_sleep.Enabled = false;
                    btn_si_lanlap_giam.Enabled = false;
                    btn_si_lanlap_tang.Enabled = false;

                    txt_si_sleep.Enabled = false;
                    btn_si_sleep_tang.Enabled = false;
                    btn_si_sleep_giam.Enabled = false;


                    txt_si_timeout.Enabled = false;
                    btn_si_timeout_giam.Enabled = false;
                    btn_si_timeout_tang.Enabled = false;

                    Utilities_thitruongsi._Timeout = ConvertType.ToInt(txt_si_timeout.Text);
                    Utilities_thitruongsi._Sleep = ConvertType.ToInt(txt_si_sleep.Text);
                    Utilities_thitruongsi._thanhcong = 0;
                    Utilities_thitruongsi._thatbai = 0;
                    Utilities_thitruongsi._wThiTruongSi = wThiTruongSi;
                    /*kiem tra neu checkpath dc chon thi luu pathlimit vao gioi hang*/
                   

                    if (!chk_si_lanlap.Checked)
                        Utilities_thitruongsi._lanquetlai = ConvertType.ToInt(txt_si_lanlap.Text);
                    else
                        Utilities_thitruongsi._lanquetlai = -1;


                    /*load danh sách đầu số*/

                    DataTable tb_dausp = SQLDatabase.ExcDataTable("select distinct dauso dauso,lenght " +
                                                  "  from dau_so where dauso is not null and dauso <> ''");

                    Dictionary<string, int> dauso = new Dictionary<string, int>();
                    foreach (DataRow item in tb_dausp.Rows)
                    {
                        dauso.Add(item["dauso"].ToString(), ConvertType.ToInt(item["lenght"].ToString()));
                    }
                    Utilities_thitruongsi._dau_so = dauso;
                    Utilities_thitruongsi._regexs = SQLDatabase.LoadRegexs("select * from Regexs order by OrderID desc");


                }
                else
                {
                    btnSiStart.Text = "Start";
                    Utilities_thitruongsi.hasProcess = false;
                    setTitleWindow(8, false);

                    btn_si_Them.Enabled = true;
                    btn_si_xoa.Enabled = true;
                    btn_si_bo.Enabled = true;

                    cmb_SiGoc.Enabled = true;
                 

                    lbl_si_message1.Visible = true;
                    lbl_si_message2.Visible = true;
                    lbl_si_khoa.Visible = false;

                    gw_si_chon.Enabled = true;
                    gw_si_goc.Enabled = true;

                    pic_si_save.Enabled = true;
                    pic_si_out.Enabled = true;

                  


                    txt_si_sleep.Enabled = true;
                    btn_si_lanlap_giam.Enabled = true;
                    btn_si_lanlap_tang.Enabled = true;

                    txt_si_sleep.Enabled = true;
                    btn_si_sleep_tang.Enabled = true;
                    btn_si_sleep_giam.Enabled = true;


                    txt_si_timeout.Enabled = true;
                    btn_si_timeout_giam.Enabled = true;
                    btn_si_timeout_tang.Enabled = true;

                   
                }

                /*Cấu hình controll*/
                Control.CheckForIllegalCrossThreadCalls = false;
                par = new ParameterizedThreadStart(ProcessThiTruongSi);
                theardProcess = new Thread(par);

                arr = new ArrayList();
                arr.Add(lbl_si_message1);
                arr.Add(lbl_si_message2);
                arr.Add(lbl_si_khoa);
                arr.Add(lbl_si_phantram);
                arr.Add(par_si);

                theardProcess.IsBackground = true;
                theardProcess.Start(arr);
            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "button2_Click", "error1"));
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

        private void danhMucToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmDM_Thitruongsi frm = new frmDM_Thitruongsi();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //BinddmNganhNghe(ConvertType.ToInt(cmd_muaban_tv_danhmuc.SelectedValue));
            }
        }

        private void cmb_SiGoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Bindthitruongsi(cmb_SiGoc.SelectedValue);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            danhMucToolStripMenuItem1_Click(null, null);
        }
        private void btn_si_Them_Click(object sender, EventArgs e)
        {
            try
            {
                int id = 0;
                string parentId = "";
                string name = "";
                string path = "";
                string orderid = "";


                foreach (DataGridViewRow row in gw_si_goc.SelectedRows)
                {
                    DataRow[] result = _table_thitruongsi.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_si_goc"].Value)));
                    if (result.Count() == 0 && row.Cells["path_si_goc"].Value != null)
                    {
                        id = ConvertType.ToInt(row.Cells["id_si_goc"].Value);
                        name = row.Cells["name_si_goc"].Value.ToString();
                        path = row.Cells["path_si_goc"].Value == null ? "" : row.Cells["path_si_goc"].Value.ToString();
                        parentId = row.Cells["parentId_si_goc"].Value == null ? "" : row.Cells["parentId_si_goc"].Value.ToString();
                        orderid = row.Cells["orderid_si_goc"].Value == null ? "" : row.Cells["orderid_si_goc"].Value.ToString();


                        _table_thitruongsi.Rows.Add(id, name, path, parentId, 0, 0,-1, orderid);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

        private void ProcessThiTruongSi(object arrControl)
        {
            try
            {
                //----- Add control process from
                ArrayList arr1 = (ArrayList)arrControl;
                Label lbl_bds_message1 = (Label)arr1[0];
                Label lbl_bds_message2 = (Label)arr1[1];
                Label lbl_bds_khoa = (Label)arr1[2];
                Label lbl_bds_Par = (Label)arr1[3];
                ProgressBar pro_thitruongsi_tv = (ProgressBar)arr1[4];
                              

                /*===============================================================*/
                int x = 0;
                string strUrl = gw_si_chon.Rows[x].Cells["path_si_chon"].Value.ToString();
                while (gw_si_chon.Rows.Count > 0)
                {
                    if (!Utilities_thitruongsi.hasProcess) break;

                    
                    int id = Convert.ToInt32(gw_si_chon.Rows[x].Cells["id_si_chon"].Value);
                    string name = gw_si_chon.Rows[x].Cells["name_si_chon"].Value.ToString();
                    int pathlimit = Convert.ToInt32(gw_si_chon.Rows[x].Cells["end_chon"].Value);

                    Utilities_thitruongsi._danhmuc = name;
                    Utilities_thitruongsi._danhmucid = id;
                    Utilities_thitruongsi._PathLimit = pathlimit;

                    infoPathmuaban model = new infoPathmuaban();
                    model = Utilities_thitruongsi.getPageMax(strUrl, arrControl);
                    Utilities_thitruongsi._infoPathmuaban = model;
                    /*refesh qua trang mới*/

                    pro_thitruongsi_tv.Value = 0;
                    pro_thitruongsi_tv.Maximum = model.TotalPagingMax;
                    lbl_bds_Par.Text = "0% Hoàn thành...";
                    lbl_bds_Par.Update();


                    DataTable tblFiltered = _table_thitruongsi.AsEnumerable()
                                               .Where(row => row.Field<int>("id_chon") == id)
                                               .CopyToDataTable();
                    if (tblFiltered != null && tblFiltered.Rows.Count != 0)
                        model.PageNow = ConvertType.ToInt(tblFiltered.Rows[0]["trang_chon"]);
                    model.TotalPagingMax = pathlimit == -1 ? model.TotalPagingMax : pathlimit > model.TotalPagingMax ? model.TotalPagingMax : pathlimit;

                    Utilities_thitruongsi._infoPathmuaban = model;
                    /*refesh qua trang mới*/
                   
                    pro_thitruongsi_tv.Maximum = model.TotalPagingMax;
                    pro_thitruongsi_tv.Minimum = model.PageNow;
                    pro_thitruongsi_tv.Value = model.PageNow;

                    lbl_bds_Par.Text = "0% Hoàn thành...";
                    lbl_bds_Par.Update();


                    int i = model.PageNow;
                    do
                    {
                        if (!Utilities_thitruongsi.hasProcess)
                            break;
                        {
                            /*kiễm tra trang*/
                            if (Utilities_thitruongsi._PathLimit != -1 && Utilities_thitruongsi._PathLimit == i) break;
                            
                            lbl_bds_Par.Text = Math.Round((i / (double)model.TotalPagingMax) * 100, 0) + "% Hoàn thành...";
                            lbl_bds_Par.Update();

                            lbl_bds_message1.Text = string.Format("Đang Xử Lý: {0} - Tổng số dự kiến :{1}   ->Trang đang quét {2}/{3} trang", name, model.TotalResult == 0 ? "--" : String.Format("{0:#,##0.##}", model.TotalResult), i, String.Format("{0:#,##0.##}", model.TotalPagingMax));
                            lbl_bds_message1.Update();



                            i = i + 1;

                            string strUrl1 = "";
                            strUrl1 = string.Format(strUrl + "?sort=up_desc&limit={0}&page={1}&deny=&price_from=&price_to=",Utilities_thitruongsi._limit ,i);
                            int solanlap = 0;

                           


                            Utilities_thitruongsi.getwebBrowser(strUrl1, ref solanlap, arrControl);

                            


                        }

                        pro_thitruongsi_tv.PerformStep();
                        pro_thitruongsi_tv.Update();


                    } while (i <= model.TotalPagingMax);

                    /*neu dang stop thi khong dc phep xoa*/
                    if (Utilities_thitruongsi.hasProcess)
                    {
                        if (_table_thitruongsi.Select(string.Format("id_chon={0}", id)).Count() != 0)
                            _table_thitruongsi.Select(string.Format("id_chon={0}", id))[0].Delete();
                    }
                }

                /*===================================================================*/
                lbl_bds_message1.Text = Utilities_batdongsan.hasProcess ? "Hoàn thành load số liệu." + lbl_bds_message1.Text : "Tạm dừng do người dùng!!! " + lbl_bds_message1.Text;
               

                btnSiStart.Text = "Start";
                Utilities_thitruongsi.hasProcess = false;
                setTitleWindow(8, false);

                btn_si_Them.Enabled = true;
                btn_si_xoa.Enabled = true;
                btn_si_bo.Enabled = true;

                cmb_SiGoc.Enabled = true;


                lbl_si_message1.Visible = true;
                lbl_si_message2.Visible = true;
                lbl_si_khoa.Visible = false;

                gw_si_chon.Enabled = true;
                gw_si_goc.Enabled = true;

                pic_si_save.Enabled = true;
                pic_si_out.Enabled = true;




                txt_si_sleep.Enabled = true;
                btn_si_lanlap_giam.Enabled = true;
                btn_si_lanlap_tang.Enabled = true;

                txt_si_sleep.Enabled = true;
                btn_si_sleep_tang.Enabled = true;
                btn_si_sleep_giam.Enabled = true;


                txt_si_timeout.Enabled = true;
                btn_si_timeout_giam.Enabled = true;
                btn_si_timeout_tang.Enabled = true;



            }
            catch (Exception ex)
            {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "ProcessBatDongSan", "error1"));
                MessageBox.Show(ex.Message, "ProcessBatDongSan");
            }
        }
        private void btn_si_add_Click(object sender, EventArgs e)
        {
            try
            {
                frmthemLinkVinabix frm = new frmthemLinkVinabix();
                frm.fromparent = "thitruongsi.com";
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    int id = frm.DanhmucId;
                    string name = frm.Ten;
                    string path = frm.Link;
                    int parentId = 0;
                    int orderid = 0;

                    _table_thitruongsi.Rows.Add(id, name, path, parentId, 0, frm.Trang,frm.PathLimit, orderid);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "danhMucToolStripMenuItem1_Click");
            }
        }

        private void btn_si_xoa_Click(object sender, EventArgs e)
        {
            try
            {
                _table_thitruongsi.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button4_Click");
            }
        }
        private void xuâtFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                frmExportvatgia frm = new frmExportvatgia();
                frm.strDatabaseName = _strDatabase;
                frm.FromParent = "thitruongsi.com";
                if (frm.ShowDialog() == DialogResult.OK)
                {
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void btn_si_bo_Click(object sender, EventArgs e)
        {
            try
            {
                if (gw_si_chon.SelectedRows.Count == 0) return;
                foreach (DataGridViewRow row in gw_si_chon.SelectedRows)
                {
                    _table_thitruongsi.Select(string.Format("id_chon={0}", ConvertType.ToInt(row.Cells["id_si_chon"].Value)))[0].Delete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "button3_Click");
            }
        }


        #endregion


    }
}


