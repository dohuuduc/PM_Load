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

namespace Load
{
    public partial class frm_chuanhoanPhone : Form
    {
        public frm_chuanhoanPhone()
        {
            InitializeComponent();
        }
        Dictionary<string, int> _dauso;
        List<regexs> _regexs;

        private void frm_chuanhoanPhone_Load(object sender, EventArgs e)
        {
            Utilities_vinabiz._listquetcan = SQLDatabase.Loaddm_vinabiz_map("select * from dm_vinabiz_map");
            _regexs = SQLDatabase.LoadRegexs("select * from Regexs");
            DataTable tb_dausp = SQLDatabase.ExcDataTable("select distinct left(dauso,2) dauso,lenght " +
                                                              "  from dau_so where dauso is not null and dauso <> '' and LEFT(dauso, 2) <> '08' " +
                                                              "  union " +
                                                              "  select distinct left(dauso, 4) dauso, lenght " +
                                                              "  from dau_so where dauso is not null and dauso <> '' and LEFT(dauso, 2) = '08'");
            _dauso = new Dictionary<string, int>();
            foreach (DataRow item in tb_dausp.Rows)
            {
                _dauso.Add(item["dauso"].ToString(), ConvertType.ToInt(item["lenght"]));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (chk_dienthoai.Checked) {
                PleaseWait objPleaseWait = null;
                //TODO: Stuff
                objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();
                if(radioButton1.Checked)
                    SQLDatabase.ExcNonQuery("update vinabiz set ttlh_dienthoaididong1='',ttlh_dienthoaididong2='',ttlh_dienthoaididong3=''");
                List<vinabiz> dm = SQLDatabase.Loadvinabiz("select  * from vinabiz where len(ttlh_dienthoai1) > 0 and ttlh_dienthoaididong1=''");
                foreach (vinabiz item in dm)
                {
                    string strPhone1 = item.ttlh_dienthoai1;
                    List<string> arrPhone = Utilities_scanner.getPhoneHTML(new List<string>() { strPhone1 }, _dauso, _regexs);
                    if (arrPhone.Count() != 0)
                    {
                        if (arrPhone.Count() != 0)
                            item.ttlh_dienthoaididong1 = arrPhone.FirstOrDefault();
                        if (arrPhone.Count() >= 2)
                            item.ttlh_dienthoaididong2 = arrPhone[1];
                        if (arrPhone.Count() >= 3)
                            item.ttlh_dienthoaididong3 = arrPhone[2];

                        SQLDatabase.UpdateChuanHoa_ttlh_dienthoai(item);
                    }
                }
                objPleaseWait.Close();
                MessageBox.Show("Đã chuẩn hoá xong 'Điện Thoại'");
            }
            if (chk_dienthoai_khachhang.Checked)
            {
                PleaseWait objPleaseWait = null;
                objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();
                if (radioButton1.Checked)
                    SQLDatabase.ExcNonQuery("update vinabiz set ttlh_dienthoai_nguoidaidien_didong=''");
                List<vinabiz> dm = SQLDatabase.Loadvinabiz("select  *  from vinabiz where len(ttlh_dienthoai_nguoidaidien) > 0 and ttlh_dienthoai_nguoidaidien_didong=''");
                foreach (vinabiz item in dm)
                {
                    string strPhone1 = item.ttlh_dienthoai_nguoidaidien;
                    List<string> arrPhone = Utilities_scanner.getPhoneHTML(new List<string>() { strPhone1 }, _dauso, _regexs);
                    if (arrPhone.Count() != 0)
                    {
                        item.ttlh_dienthoai_nguoidaidien_didong = arrPhone.FirstOrDefault();
                        SQLDatabase.UpdateChuanHoa_ttlh_dienthoai_nguoidaidien_didong(item);
                    }
                }
                objPleaseWait.Close();
                MessageBox.Show("Đã chuẩn hoá xong 'Điện thoại_Khách Hàng'");
            }
            if (chk_dienthoai_giamdoc.Checked) {
                PleaseWait objPleaseWait = null;
                objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();
                if (radioButton1.Checked)
                    SQLDatabase.ExcNonQuery("update vinabiz set ttlh_dienthoaigiamdoc_didong=''");
                List<vinabiz> dm = SQLDatabase.Loadvinabiz("select * from vinabiz where len(ttlh_dienthoaigiamdoc) > 0 and ttlh_dienthoaigiamdoc_didong=''");
                foreach (vinabiz item in dm)
                {
                    string strPhone1 = item.ttlh_dienthoaigiamdoc;
                    List<string> arrPhone = Utilities_scanner.getPhoneHTML(new List<string>() { strPhone1 }, _dauso, _regexs);
                    if (arrPhone.Count() != 0)
                    {
                        item.ttlh_dienthoaigiamdoc_didong = arrPhone.FirstOrDefault();
                        SQLDatabase.UpdateChuanHoa_ttlh_dienthoaigiamdoc(item);
                    }
                }
                objPleaseWait.Close();
                MessageBox.Show("Đã chuẩn hoá xong 'Điện Thoại_Giám Đốc'");
            }
            if (chk_dienthoai_ketoan.Checked) {
                PleaseWait objPleaseWait = null;
                objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();
                if (radioButton1.Checked)
                    SQLDatabase.ExcNonQuery("update vinabiz set ttlh_dienthoaiketoan_didong=''");
                List<vinabiz> dm = SQLDatabase.Loadvinabiz("select  * from vinabiz where len(ttlh_dienthoaiketoan) > 0 and ttlh_dienthoaiketoan_didong=''");
                foreach (vinabiz item in dm)
                {
                    string strPhone1 = item.ttlh_dienthoaiketoan;
                    List<string> arrPhone = Utilities_scanner.getPhoneHTML(new List<string>() { strPhone1 }, _dauso, _regexs);
                    if (arrPhone.Count() != 0)
                    {
                        item.ttlh_dienthoaiketoan_didong = arrPhone.FirstOrDefault();
                        SQLDatabase.UpdateChuanHoa_ttlh_dienthoaiketoan(item);
                    }
                }
                objPleaseWait.Close();
                MessageBox.Show("Đã chuẩn hoá xong 'Điện Thoại_Kế Toán'");
            }
        }

        
    }
}
