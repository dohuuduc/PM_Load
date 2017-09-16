using StorePhone;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Load {
    public partial class frmDM_scanner : Form {
        private string _domain;
        private LinkQueues2 _queue;
        private Thread theardProcess;
        private LogWriter writer;
        private string _comboxnhom;

        private DataTable _table_moi;
        private DataTable _table_cu;

        private bool _block;

        public frmDM_scanner() {
            InitializeComponent();
        }
       

        private void button1_Click(object sender, EventArgs e) {
            try {
                frmDM_scanner2 frm = new frmDM_scanner2();
                if (frm.ShowDialog() == DialogResult.OK) {
                    BindDmNhom();
                }
            }
            catch (Exception) {

                throw;
            }
        }

        private void frmDM_scanner_Load(object sender, EventArgs e) {
            try {
                BindDmNhom();

                _table_moi = CreateTable();
                dv_moi.DataSource = _table_moi;
                dv_moi.Sort(dv_moi.Columns["id"], ListSortDirection.Ascending);


                _table_cu = CreateTable();
                dv_cu.DataSource = _table_cu;
                dv_cu.Sort(dv_cu.Columns["id_cu"], ListSortDirection.Ascending);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            
        }

        public DataTable CreateTable() {
            DataTable table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("domain", typeof(string));
            table.Columns.Add("path", typeof(string));
            table.Columns.Add("parentid", typeof(string));
            table.Columns.Add("orderid", typeof(string));
            table.Columns.Add("statur", typeof(string));
            table.Columns.Add("dosau", typeof(string));
            return table;
        }

        

        public void BindDmNhom() {
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            PleaseWait objPleaseWait = null;
            try {

                object id = ConvertType.ToInt(comboBox1.SelectedValue);
                if (id.ToString() == "0")
                    return;
                if (id.ToString() != "0") {
                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();

                BindCu(id);
                BindMoi(id);
               
                    DataTable tb = SQLDatabase.ExcDataTable(string.Format("select * from dm_scanner where id='{0}'",id));
                    if (tb == null || tb.Rows.Count==0)
                        return;
                    _domain = tb.Rows[0]["domain"].ToString();
                   
                    objPleaseWait.Close();
                    
                }
            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message, "cmb_nhomhang_SelectedIndexChanged");
            }
        }

        public void BindCu(object nhom) {
            if (nhom == null) return;
            _table_cu = SQLDatabase.ExcDataTable(string.Format("select * from dm_scanner_ct where parentid={0} and statur=1", nhom));
            dv_cu.DataSource = _table_cu;
            
        }
        public void BindMoi(object nhom) {
            if (nhom == null)
                return;
            _table_moi = SQLDatabase.ExcDataTable(string.Format("select * from dm_scanner_ct  where parentid={0} and statur=0", nhom));
            dv_moi.DataSource = _table_moi;
            lbl_moi.Text = string.Format("II-Danh Sách Liên Kết Mới:{0}", _table_moi.Rows.Count);
        }
        private void dv_cu_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
            lbl_cu.Text = string.Format("Danh Sách Liên Kết Củ: {0}", dv_cu.Rows.Count);
        }
        private void dv_moi_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
            //lbl_moi.Text = string.Format("Danh Sách Liên Kết Mới: {0}", dv_moi.Rows.Count);
        }
        
        private void button2_Click(object sender, EventArgs e) {
            
         
            ArrayList arr;

            try {
                
                if (comboBox1.SelectedValue.ToString() == "-1") {
                    MessageBox.Show("Vui lòng nhập liên kết.", "Thông Báo");
                    comboBox1.Focus();
                    return;
                }
                if (_domain == "") {
                    MessageBox.Show("Vui lòng thông tin domain của liên kết.", "Thông Báo");
                    btn_tao.Focus();
                    return;
                }
                int _tonglink = 0;
                _comboxnhom = comboBox1.SelectedValue.ToString();
                _queue = LinkQueues2.Instance;
                _queue.setIdLienKet(ConvertType.ToInt(_comboxnhom));
                _queue.InitBindLinsAll(ConvertType.ToInt(_comboxnhom),ref _tonglink);
                _block = false;

                Utilities_scanner._regexs = SQLDatabase.LoadRegexs("select * from Regexs order by OrderID desc");

                if (btn_start.Text == "Start") {
                    btn_start.Text = "Stop";
                    if (_queue.CountQueue1() == 0) {
                        lbl_message.Text = "Đang dò tìm liên kết ";
                        lbl_message.Update();
                    }
                    else {
                        lbl_message.Text = "Bắt đầu quay lại công việc dò tìm.... ";
                        lbl_message.Update();
                    }

                    Utilities_scanner.hasProcess = true;
                    Utilities_scanner._doman = _domain;
                    Utilities_scanner._sleep = ConvertType.ToInt(txtThoiGianCho.Text);
                    Utilities_scanner._lanquetlai = ConvertType.ToInt(txt_lanlap.Text);
                    Utilities_scanner._timeout = ConvertType.ToInt(txt_timeout.Text);
                    Utilities_scanner._gioihan_lienket = ConvertType.ToInt(txtGioiHan.Text);
                    /*load danh sách đầu số*/

                    DataTable tb_dausp = SQLDatabase.ExcDataTable("select distinct dauso dauso,lenght " +
                                                   "  from dau_so where dauso is not null and dauso <> ''");
                    Dictionary<string, int> dauso = new Dictionary<string, int>();
                    foreach (DataRow item in tb_dausp.Rows) {
                        dauso.Add(item["dauso"].ToString(), ConvertType.ToInt(item["lenght"].ToString()));
                    }
                    Utilities_scanner._dau_so = dauso;

                    dv_cu.Visible = true;
                    dv_moi.Visible = true;

                    comboBox1.Enabled = false;
                    txtDoSau.Enabled = false;
                    txtSoLuong.Enabled = false;
                    txtThoiGianCho.Enabled = false;
                    txt_timeout.Enabled=false;
                    txt_lanlap.Enabled = false;

                    btn_chon_scanner.Enabled = false;
                    btn_scanner_bo.Enabled = false;
                    btn_scanner_xoa_cu.Enabled = false;
                    btn_tao.Enabled = false;
                    btn_scanner_xoa_moi.Enabled = false;

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

                    
                    /*kiem tra neu checkpath dc chon thi luu pathlimit vao gioi hang*/
                   
                    Utilities_scanner._dosau = ConvertType.ToInt(txtDoSau.Text);
                    if (!checkBox1.Checked)
                        Utilities_scanner._lanquetlai = ConvertType.ToInt(txt_lanlap.Text);
                    else
                        Utilities_scanner._lanquetlai = -1;
                   
                }
                else {
                    btn_start.Text = "Start";
                    Utilities_scanner.hasProcess = false;

                    dv_cu.Visible = true;
                    dv_moi.Visible = true;

                    comboBox1.Enabled = true;
                    txtDoSau.Enabled = true;
                    txtSoLuong.Enabled = true;

                    btn_chon_scanner.Enabled = true;
                    btn_scanner_bo.Enabled = true;
                    btn_scanner_xoa_cu.Enabled = true;
                    btn_tao.Enabled = true;
                    txtThoiGianCho.Enabled = true;
                    txt_timeout.Enabled = true;
                    txt_lanlap.Enabled = true;
                    btn_scanner_xoa_moi.Enabled = true;

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

                    txtDoSau.Enabled = false;
                   
                }
                /*Cấu hình controll*/
                Control.CheckForIllegalCrossThreadCalls = false;

                ParameterizedThreadStart par;
                int islandau=0;
                do {
                    if (_queue.CountQueue1() > 10 || islandau==0) {
                        par = new ParameterizedThreadStart(ProcessScanner);
                        theardProcess = new Thread(par);

                        arr = new ArrayList();
                        arr.Add(lbl_message);
                        arr.Add(dv_moi);
                        arr.Add(lbl_moi);
                        arr.Add(lbl_scanner_khoa);
                        arr.Add(lbl_handoi);
                        theardProcess.Start(arr);
                        islandau++;
                    }
                    if (islandau > ConvertType.ToInt(txtSoLuong.Text)) {
                        break;
                    }

                } while (1 == 1);

                //for (int i = 0; i < ConvertType.ToInt(txtSoLuong.Text);i++ ) {
                //    par = new ParameterizedThreadStart(ProcessScanner);
                //    theardProcess = new Thread(par);

                //    arr = new ArrayList();
                //    arr.Add(lbl_message);
                //    arr.Add(dv_moi);
                //    arr.Add(lbl_moi);
                //    arr.Add(lbl_scanner_khoa);
                //    arr.Add(lbl_handoi);
                //    theardProcess.Start(arr);
                //    //Thread.Sleep();  
                //}
            }
            catch (Exception ex) {
                writer.WriteToLog(string.Format("{0}   - {1} - {2}", ex.Message, "button5_Click_1", "error1"));
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

       
        private void ProcessScanner(object arrControl) {
            try {
                //----- Add control process from
                ArrayList arr1 = (ArrayList)arrControl;
                Label lbl_message = (Label)arr1[0];
                DataGridView dv_moi = (DataGridView)arr1[1];
                Label lbl_moi = (Label)arr1[2];
                Label lbl_scanner_khoa = (Label)arr1[3];
                Label lbl_handoi = (Label)arr1[4];

                bool dosaugioihang = false;
                
                /*===============================================================*/
                while (true) {
                    dm_scanner_ct link = null;
                    
                    try {
                        if (_queue.CountQueue1() == 0)
                            break;
                        link = _queue.DequeueLinks1();
                       

                        if (!Utilities_scanner.hasProcess ||
                         //Utilities_scanner._gioihan_lienket <= _queue.CountLinksAll() ||
                         Utilities_scanner._dosau <= ConvertType.ToInt(link.dosau)) {

                             if (Utilities_scanner._dosau <= ConvertType.ToInt(link.dosau))
                                 dosaugioihang = true;

                            break;
                        }
                    }
                    catch { break; }
                    if (null != link) {
                        int solanlap = 0;
                        Utilities_scanner.getwebBrowserFindLink(link, ref _queue, ref arrControl,  ref solanlap);

                        /*khóa 1 lần*/
                        //if (!_block && _queue.CountQueue1() > 0) {
                        //    if (!Utilities_scanner.hasProcess ||
                        //        Utilities_scanner._gioihan_lienket <= _queue.CountLinksAll() ||
                        //         Utilities_scanner._dosau <= ConvertType.ToInt(link.dosau)) {
                        //             if (Utilities_scanner._dosau <= ConvertType.ToInt(link.dosau))
                        //                 dosaugioihang = true;
                        //        break;
                        //    }


                        //    //_block = true;
                        //    for (int i = 0; i < ConvertType.ToInt(txtSoLuong.Text); i++) {
                        //        ParameterizedThreadStart par;
                        //        Control.CheckForIllegalCrossThreadCalls = false;

                        //        par = new ParameterizedThreadStart(CacTieuTrinh);
                        //        theardProcess = new Thread(par);
                        //        theardProcess.Name = string.Format("TieuTrinh_{0}", i);

                        //        theardProcess.Start(arrControl);
                        //    }
                        //}
                    }
                    //lbl_handoi.Text = string.Format("Hàng Đợi : {0}",  _queue.CountQueue1());
                    //lbl_handoi.Update();

                    lbl_handoi.BeginInvoke((MethodInvoker)delegate() {
                        lbl_handoi.Text = string.Format("Hàng Đợi : {0}", _queue.CountQueue1());
                        lbl_handoi.Update();
                    });

                }
                //if (!Utilities_scanner.hasProcess) {
                //    lbl_scanner_khoa.Text += string.Format("Tạm dừng do người dùng....");
                //    lbl_scanner_khoa.Update();
                //    return;
                //}
                //else if (_queue.CountQueue1() == 0) {
                //    lbl_scanner_khoa.Text += string.Format("Hoành tất việc dò tìm....");
                //    lbl_scanner_khoa.Update();
                //    return;
                //}
                //else if (Utilities_scanner._gioihan_lienket <= _queue.CountLinksAll()) {
                //    lbl_scanner_khoa.Text += string.Format("{0} Đã hoàn thành việc dò tìm tới giới hạn liên kết {0}", Utilities_scanner._gioihan_lienket);
                //    lbl_scanner_khoa.Update();
                //    return;
                //}  else if (dosaugioihang) {
                //    lbl_scanner_khoa.Text += string.Format("{0} Đã hoàn thành việc dò tìm tới độ sâu giới hạn {0}", Utilities_scanner._dosau);
                //    lbl_scanner_khoa.Update();
                //    return;
                //}
                //else {
                //    lbl_scanner_khoa.Text += string.Format("Đã kết thúc tiến trình : {0}", theardProcess.Name);
                //    lbl_scanner_khoa.Update();
                //    return;
                //}

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "ProcessScanner");
            }
        }

      
        private void btn_chon_scanner_Click(object sender, EventArgs e) {
            try {
                foreach (DataGridViewRow row in dv_moi.SelectedRows) {
                    dm_scanner_ct link = SQLDatabase.Loaddm_scanner_ct(string.Format("select * from dm_scanner_ct where id='{0}'", row.Cells["id"].Value)).FirstOrDefault();
                    if (link != null) {
                        link.statur = true;
                        SQLDatabase.Updatedm_scanner_ct(link);
                    }
                }
                comboBox1_SelectedIndexChanged(null, null);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

        private void btn_scanner_bo_Click(object sender, EventArgs e) {
            try {
                foreach (DataGridViewRow row in dv_cu.SelectedRows) {
                    dm_scanner_ct link = SQLDatabase.Loaddm_scanner_ct(string.Format("select * from dm_scanner where id='{0}'", row.Cells["id"].Value)).FirstOrDefault();
                    if (link != null) {
                        link.statur = false;
                        SQLDatabase.Updatedm_scanner_ct(link);
                    }
                }
                comboBox1_SelectedIndexChanged(null, null);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

        private void button1_Click_1(object sender, EventArgs e) {
            PleaseWait objPleaseWait = null;
            try {
                   if ((MessageBox.Show("Bạn có chắc chắn là muốn xoá liên hệ mới trong danh sách đã chọn không?", "Thông Báo", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, 
                        MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                        {
                            
                            objPleaseWait = new PleaseWait();
                            objPleaseWait.Show();
                            objPleaseWait.Update();
                            //TODO: Stuff
                            foreach (DataGridViewRow row in dv_moi.SelectedRows)
                                SQLDatabase.ExcNonQuery(string.Format("delete from dm_scanner where id='{0}'", row.Cells["id"].Value));
                            }
                             object id = ConvertType.ToInt(comboBox1.SelectedValue);
                             if (id.ToString() == "0")    return;
                                 BindMoi(id);
                                 objPleaseWait.Close();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message,"button1_Click_1");
            }
        }

        private void btn_scanner_xoa_Click(object sender, EventArgs e) {
            PleaseWait objPleaseWait = null;
            try {
                if ((MessageBox.Show("Bạn có chắc chắn là muốn xoá liên hệ củ trong danh sách đã chọn không?", "Thông Báo",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)) {

                    objPleaseWait = new PleaseWait();
                    objPleaseWait.Show();
                    objPleaseWait.Update();
                    //TODO: Stuff
                    foreach (DataGridViewRow row in dv_cu.SelectedRows) {
                        SQLDatabase.ExcNonQuery(string.Format("delete from dm_scanner where id='{0}'", row.Cells["id_cu"].Value));
                    }
                    object id = ConvertType.ToInt(comboBox1.SelectedValue);
                    if (id.ToString() == "0")
                        return;
                    BindCu(id);
                    objPleaseWait.Close();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "button1_Click_1");
            }
        }

        private void dv_moi_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            e.Cancel = true;
        }

        private const int sleep_bat = 100;
        private const int timeout_bat = 500;
        private const int lanlap_bat = 1;
        private const int soluong_bat = 1;
        private const int dosau_bat = 1;
        private const int gioihan_bat = 50;
        private void btn_scanner_sleep_tang_Click(object sender, EventArgs e) {
            try {
                if (ConvertType.ToInt(txtThoiGianCho.Text) >= 1000)
                    return;
                txtThoiGianCho.Text =( ConvertType.ToInt(txtThoiGianCho.Text)+sleep_bat).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message,"btn_scanner_sleep_tang_Click");
            }
        }

        private void btn_scanner_sleep_giam_Click(object sender, EventArgs e) {
            try {
                if (ConvertType.ToInt(txtThoiGianCho.Text) <= 0)
                    return;
                txtThoiGianCho.Text = (ConvertType.ToInt(txtThoiGianCho.Text) - sleep_bat).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "btn_scanner_sleep_giam_Click");
            }
        }

        private void btn_scanner_timeout_giam_Click(object sender, EventArgs e) {
             try {
                if (ConvertType.ToInt(txt_timeout.Text) <= 500)
                    return;
                txt_timeout.Text = (ConvertType.ToInt(txt_timeout.Text) - timeout_bat).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "btn_scanner_timeout_giam_Click");
            }
        }

        private void btn_scanner_timeout_tang_Click(object sender, EventArgs e) {
            try {
                if (ConvertType.ToInt(txt_timeout.Text) >= 100000)
                    return;
                txt_timeout.Text = (ConvertType.ToInt(txt_timeout.Text) + timeout_bat).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "btn_scanner_sleep_tang_Click");
            }
        }

        private void btn_scanner_lanlap_tang_Click(object sender, EventArgs e) {
            try {
                if (ConvertType.ToInt(txt_lanlap.Text) >= 100)
                    return;
                txt_lanlap.Text = (ConvertType.ToInt(txt_lanlap.Text) + lanlap_bat).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "btn_scanner_lanlap_tang_Click");
            }
        }

        private void btn_scanner_lanlap_giam_Click(object sender, EventArgs e) {
            try {
                if (ConvertType.ToInt(txt_lanlap.Text) <= 0)
                    return;
                txt_lanlap.Text = (ConvertType.ToInt(txt_lanlap.Text) - lanlap_bat).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "btn_scanner_lanlap_giam_Click");
            }
        }

        private void btn_scanner_soluong_tang_Click(object sender, EventArgs e) {
            try {
                if (ConvertType.ToInt(txtSoLuong.Text) >= 100)
                    return;
                txtSoLuong.Text = (ConvertType.ToInt(txtSoLuong.Text) + soluong_bat).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "btn_scanner_soluong_tang_Click");
            }
        }

        private void btn_scanner_soluong_giam_Click(object sender, EventArgs e) {
            try {
                if (ConvertType.ToInt(txtSoLuong.Text) <= 0)
                    return;
                txtSoLuong.Text = (ConvertType.ToInt(txtSoLuong.Text) - soluong_bat).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "btn_scanner_soluong_giam_Click");
            }
        }

        private void btn_scanner_dosau_giam_Click(object sender, EventArgs e) {
            try {
                if (ConvertType.ToInt(txtDoSau.Text) <= 0)
                    return;
                txtDoSau.Text = (ConvertType.ToInt(txtDoSau.Text) - dosau_bat).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "btn_scanner_dosau_giam_Click");
            }
        }

        private void btn_scanner_dosau_tang_Click(object sender, EventArgs e) {
            try {
                if (ConvertType.ToInt(txtDoSau.Text) >= 20)
                    return;
                txtDoSau.Text = (ConvertType.ToInt(txtDoSau.Text) + dosau_bat).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "btn_scanner_dosau_tang_Click");
            }
        }

        private void btn_scanner_giaihan_giam_Click(object sender, EventArgs e) {
            try {
                if (ConvertType.ToInt(txtGioiHan.Text) <= 0)
                    return;
                txtGioiHan.Text = (ConvertType.ToInt(txtGioiHan.Text) - gioihan_bat).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "btn_scanner_giaihan_giam_Click");
            }
        }

        private void btn_scanner_giaihan_tang_Click(object sender, EventArgs e) {
            try {
                if (ConvertType.ToInt(txtGioiHan.Text) >= 5000000)
                    return;
                txtGioiHan.Text = (ConvertType.ToInt(txtGioiHan.Text) + gioihan_bat).ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "btn_scanner_giaihan_tang_Click");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            txt_lanlap.Enabled = !checkBox1.Checked;
        }

    }
}
