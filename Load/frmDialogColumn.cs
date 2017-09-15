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
    public partial class frmDialogColumn : Form {
        public frmDialogColumn() {
            InitializeComponent();

           
        }

        public string fromparent;
        public string strcolumn;
        public string strdatabasename;
        public string batdongsanmau;

        public string FromParent {
            get { return fromparent; }
            set { fromparent = value; }
        }
        public string strColumn {
            get { return strcolumn; }
            set { strcolumn = value; }
        }
        public string strDatabaseName {
            get { return strdatabasename; }
            set { strdatabasename = value; }
        }
        public string strbatdongsanmau
        {
            get { return batdongsanmau; }
            set { batdongsanmau = value; }
        }
        private void button1_Click(object sender, EventArgs e) {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn cột cần import", "Thông báo");
                return;
            }
            string strsql = "";
            if (fromparent.Contains("hosocongty.vn")) {
                foreach (var item in checkedListBox1.CheckedItems) {
                    var row = (item as DataRowView).Row;
                    strsql  = strsql + string.Format("{0},", row["name"]);
                }
            }
            else if (fromparent.Contains("trangvang.com")) {
                foreach (var item in checkedListBox1.CheckedItems) {

                    if (item.ToString().Contains("nganh_nghe"))
                        strsql = strsql + string.Format("{0},", "nganh_nghe = (select name from {0}.dbo.dm_trangvang b where a.danhmucid=b.id)",strdatabasename);
                    else
                        strsql = strsql + string.Format("{0},", item.ToString());
                }
            }
            else if (fromparent.Contains("vatgia.com")) {
                foreach (var item in checkedListBox1.CheckedItems) {
                    if (item.ToString().Contains("Gian_Hang")) {
                        strsql = strsql + string.Format("{0},", "Gian_Hang = (select name from {0}.dbo.dm_vatgia c where c.id= b.parentid)",strdatabasename);
                    }else  if (item.ToString().Contains("Nganh_Nghe"))
                        strsql = strsql + string.Format("{0},", "b.name as Nganh_Nghe");
                    else
                            strsql = strsql + string.Format("{0},", item.ToString());
                }
            }
            else if (fromparent.Contains("muaban.net"))
            {
                foreach (var item in checkedListBox1.CheckedItems)
                {

                    if (item.ToString().Contains("Nganh_Nghe"))
                        strsql = strsql + string.Format("{0},", "nganh_nghe = (select name from {0}.dbo.dm_muaban b where a.danhmucid=b.id)", strdatabasename);
                    else
                        strsql = strsql + string.Format("{0},", item.ToString());
                }
            }
            else if (fromparent.Contains("batdongsan.com.vn"))
            {
                foreach (var item in checkedListBox1.CheckedItems)
                {

                    if (item.ToString().Contains("danhmuc"))
                        strsql = strsql + string.Format("{0},", "danhmuc = (select name from {0}.dbo.dm_batdongsan b where a.danhmucid=b.id)", strdatabasename);
                    else
                        strsql = strsql + string.Format("{0},", item.ToString());
                }
            }
            else if (fromparent.Contains("vinabiz.org"))
            {
                foreach (var item in checkedListBox1.CheckedItems)
                {
                   
                    if ((((StorePhone.ListItemBox)item)).Name.Contains("nganh_nghe"))
                        strsql = strsql + string.Format("{0},", "nganh_nghe = (select name from {0}.dbo.dm_hsct b where a.danhmucid=b.id)", strdatabasename);
                    else if (item.ToString().Contains("nganh_nghe_chinh"))
                        strsql = strsql + string.Format("{0},", "nganh_nghe_chinh = (select name from {0}.dbo.dm_hsct b where a.ds_nganhnghekinhdoanhID=b.id)", strdatabasename);
                    else
                        strsql = strsql + string.Format("{0},", (((StorePhone.ListItemBox)item)).Name);
                }
            }
            else if (fromparent.Contains("thitruongsi.com"))
            {
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    if (item.ToString().Contains("nganh_nghe"))
                        strsql = strsql + string.Format("{0},", "nganh_nghe = (select name from {0}.dbo.dm_thitruongsi b where a.danhmucid=b.id)", strdatabasename);
                    else
                        strsql = strsql + string.Format("{0},", item.ToString());
                }
            }
           
            if (strsql.Length >1)
            strColumn = strsql.Substring(0,strsql.Length -1);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public void BindHoSoCongTy() {
            DataTable tb = StorePhone.SQLDatabase.ExcDataTable("SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.tempExport') order by column_id");
            ((ListBox)checkedListBox1).DataSource = tb;
            ((ListBox)checkedListBox1).DisplayMember = "name";
            ((ListBox)checkedListBox1).ValueMember = "name";
        }
        public void BindTrangVang() {
            List<string> ds = new List<string>();
            ds.Add("msthue");
            ds.Add("ten_cong_ty");
            ds.Add("dien_thoai");
            ds.Add("dia_chi");
            ds.Add("xa");
            ds.Add("huyen");
            ds.Add("tinh");
            ds.Add("email_cty");
            ds.Add("website");
            ds.Add("website2");
            ds.Add("loai_hinh");
            ds.Add("fax");
            ds.Add("thi_truong");
            ds.Add("so_nhan_vien");
            ds.Add("nganh_nghe");
            ds.Add("nam_thanh_lap");
            ds.Add("chung_nhan");
            ds.Add("nguoi_lien_he1");
            ds.Add("di_dong1");
            ds.Add("di_dong2");
            ds.Add("email1");
            ds.Add("chuc_vu1");
            ds.Add("Skype1");
            ds.Add("nguoi_lien_he2");
            ds.Add("dien_thoai2");
            ds.Add("email2");
            ds.Add("chuc_vu2");
            ds.Add("Skype2");
            ds.Add("sys_diachiweb");
            ((ListBox)checkedListBox1).DataSource = ds;
        }

        private void BindVatGia() { 
            List<string> ds = new List<string>();
            ds.Add("Gian_Hang");
            ds.Add("Nganh_Nghe");
            ds.Add("tencongty");
            ds.Add("dia_chi");
            ds.Add("hotline1");
            ds.Add("hotline2");
            ds.Add("fax");
            ds.Add("website");
            ds.Add("lienhe");
            ds.Add("didong");
            ds.Add("email");
            ds.Add("sys_diachiweb");

          

            ((ListBox)checkedListBox1).DataSource = ds;

      
        }

        private void BindMuaban()
        {
            List<string> ds = new List<string>();
            ds.Add("Nganh_Nghe");
            ds.Add("idtin");
            ds.Add("danhmucnghe");
            ds.Add("doituongkh");
            ds.Add("daxacthuc");
            ds.Add("duyetboi");
            ds.Add("tieude");
            ds.Add("lienhe");
            ds.Add("diachi");
            ds.Add("quan");
            ds.Add("thanhpho");
            ds.Add("dienthoai");
            ds.Add("noidung");
            ds.Add("dienthoai_nd");
            ds.Add("email_nd");
            ds.Add("mucluong");
            ds.Add("mucluongtuden");
            ds.Add("hoten");
            ds.Add("gioitinh");
            ds.Add("ngoaingu");
            ds.Add("kinhnghiem");
            ds.Add("namsinh");
            ds.Add("bangcap");
            ds.Add("loaihinhcv");
            ds.Add("vitri");
            ds.Add("ngaydang");
            ds.Add("sys_diachiweb");
            ((ListBox)checkedListBox1).DataSource = ds;
        }

        private void BindBatdongsan_canmua()
        {
            //id, matindang, danhmucid, tieude,  quanhuyen_canmua, thanhpho_canmua, 
            //giaban, dientich, noidung, dienthoai_nd, email_nd, loaihinhtindang, ngaydang, ngayhethan, dd_bds_loaitinrao, dd_bds_diachi, dd_bds_huongbancong, dd_bds_sophongngu, dd_bds_sotoilet, dd_bds_noithat, dd_bds_huongnha, dd_bds_sotang, dd_bds_mattien, dd_bds_duongvao, lienhe_tenlienlac, lienhe_diachi, lienhe_dienthoai, lienhe_mobilde, lienhe_email, ttduan_tenduan, ttduan_chudautu, ttduan_quymo, moigioi_tieude, moigioi_diachi, moigioi_soban, moigioi_didong, moigioi_fax, moigioi_email, moigioi_website, moigioi_khuvucmoigioi, moigioi_masothue, sys_diachiweb, ischuan, createdate
            List<string> ds = new List<string>();
            ds.Add("matindang");
            ds.Add("danhmuc");
            ds.Add("tieude");
            ds.Add("quanhuyen_canmua");
            ds.Add("thanhpho_canmua");
            ds.Add("giaban");
            ds.Add("dientich");
            ds.Add("noidung");
            ds.Add("dienthoai_nd");
            ds.Add("email_nd");
            ds.Add("loaihinhtindang");
            ds.Add("ngaydang");
            ds.Add("ngayhethan");
            ds.Add("dd_bds_loaitinrao");
            ds.Add("dd_bds_diachi");
            ds.Add("dd_bds_huongbancong");
            ds.Add("dd_bds_sophongngu");
            ds.Add("dd_bds_sotoilet");
            ds.Add("dd_bds_noithat");
            ds.Add("dd_bds_huongnha");
            ds.Add("dd_bds_sotang");
            ds.Add("dd_bds_mattien");
            ds.Add("dd_bds_duongvao");
            ds.Add("lienhe_tenlienlac");
            ds.Add("lienhe_diachi");
            ds.Add("lienhe_dienthoai");
            ds.Add("lienhe_mobilde");
            ds.Add("lienhe_email");
            ds.Add("ttduan_tenduan");
            ds.Add("ttduan_chudautu");
            ds.Add("ttduan_quymo");
            ds.Add("sys_diachiweb");
            ((ListBox)checkedListBox1).DataSource = ds;
        }

        private void BindBatdongsan_canban()
        {
            List<string> ds = new List<string>();
            ds.Add("matindang");
            ds.Add("danhmuc");
            ds.Add("tieude");
            ds.Add("khuvuc_canban");
            ds.Add("quan_canban");
            ds.Add("thanhpho_canban");
            ds.Add("giaban");
            ds.Add("dientich");
            ds.Add("noidung");
            ds.Add("dienthoai_nd");
            ds.Add("email_nd");
            ds.Add("loaihinhtindang");
            ds.Add("ngaydang");
            ds.Add("ngayhethan");
            ds.Add("dd_bds_loaitinrao");
            ds.Add("dd_bds_diachi");
            ds.Add("dd_bds_huongbancong");
            ds.Add("dd_bds_sophongngu");
            ds.Add("dd_bds_sotoilet");
            ds.Add("dd_bds_noithat");
            ds.Add("dd_bds_huongnha");
            ds.Add("dd_bds_sotang");
            ds.Add("dd_bds_mattien");
            ds.Add("dd_bds_duongvao");
            ds.Add("lienhe_tenlienlac");
            ds.Add("lienhe_diachi");
            ds.Add("lienhe_dienthoai");
            ds.Add("lienhe_mobilde");
            ds.Add("lienhe_email");
            ds.Add("ttduan_tenduan");
            ds.Add("ttduan_chudautu");
            ds.Add("ttduan_quymo");
            ds.Add("sys_diachiweb");
            ((ListBox)checkedListBox1).DataSource = ds;
        }
        private void BindBatdongsan_moigioi()
        {
            List<string> ds = new List<string>();
            ds.Add("matindang");
            ds.Add("danhmuc");
            ds.Add("tieude");
            ds.Add("moigioi_tieude");
            ds.Add("moigioi_diachi");
            ds.Add("moigioi_soban");
            ds.Add("moigioi_didong");
            ds.Add("moigioi_fax");
            ds.Add("moigioi_email");
            ds.Add("moigioi_website");
            ds.Add("moigioi_khuvucmoigioi");
            ds.Add("moigioi_masothue");
            ds.Add("sys_diachiweb");
            ((ListBox)checkedListBox1).DataSource = ds;
        }

        private void BindThitruongsi() {
            List<string> ds = new List<string>();
            ds.Add("nganh_nghe");
            ds.Add("tieude");
            ds.Add("gia_tu_den");
            ds.Add("toithieu");
            ds.Add("loaidoanhnghiep");
            ds.Add("mohinhkinhdoanh");
            ds.Add("tendoanhnghiep");
            ds.Add("thoigianhoatdong");
            ds.Add("ttdn_diachi");
            ds.Add("ttdn_msthue");
            ds.Add("ttncc_diachi");
            ds.Add("ttncc_sdt");
            ds.Add("ttncc_sdt_didong");
            ds.Add("daxacthuc_cogiayphepkinhdoanh");
            ds.Add("daxacthuc_cokhohang");
            ds.Add("listdanhmuc");
            ds.Add("daidien_hoten");
            ds.Add("daidien_dienthoai");
            ds.Add("daidien_email");
            ds.Add("lienhe_kinhdoanh_hoten1");
            ds.Add("lienhe_kinhdoanh_sodienthoai1");
            ds.Add("lienhe_kinhdoanh_sodienthoaibydidong1");
            ds.Add("lienhe_kinhdoanh_hoten2");
            ds.Add("lienhe_kinhdoanh_sodienthoai2");
            ds.Add("lienhe_kinhdoanh_sodienthoaibydidong2");
            ds.Add("quymosx_thitruong");
            ds.Add("quymosx_nganhhang");
            ds.Add("quymosx_nhansu");
            ds.Add("quymosx_sanluong");
            ds.Add("khohang_diachi");
            ds.Add("cuahang_diachi");
            ds.Add("hinhanhxuong_diachi");
          
            ds.Add("sys_diachiweb");
          
            ((ListBox)checkedListBox1).DataSource = ds;
        }
       
        private void BindVinabiz()
        {
            List<ListItemBox> ds = new List<ListItemBox>();

           // ds.Add(new ListItemBox() { Name = "id", Path = "STT", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "danhmucid", Path = "Mục Lục_Quét", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_nguoidaidien", Path = "Khách Hàng", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_dienthoai1", Path = "Điện Thoại", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_dienthoaididong1", Path = "Di Động 1", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_dienthoaididong2", Path = "Di Động 2", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_dienthoaididong3", Path = "Di Động 3", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_dienthoai_nguoidaidien", Path = "Điện Thoại_Khách Hàng", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_dienthoai_nguoidaidien_didong", Path = "Di Động_Khách Hàng", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_diachitruso", Path = "Địa Chỉ Công Ty", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_tinh", Path = "Tỉnh", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_huyen", Path = "Huyện", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_xa", Path = "Xã", IsChecked = false });

            ds.Add(new ListItemBox() { Name = "ttlh_diachinguoidaidien", Path = "Địa Chỉ_Khách Hàng", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_email", Path = "Email", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttdk_tenchinhthuc", Path = "Công Ty", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttdk_tengiaodich", Path = "Tên Giao Dịch", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttdk_msthue", Path = "Mã Số Thuế", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttdk_ngaycap", Path = "Ngày Cấp", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttdk_ngaybatdauhoatdong", Path = "Ngày Hoạt Động", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttdk_trangthai", Path = "Trạng Thái Hoạt Động", IsChecked = false });

            ds.Add(new ListItemBox() { Name = "danhmucbyVnbiz", Path = "Ngành Nghề Chính", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "nganhnghechinh2", Path = "Ngành Nghề Chính_HSCT", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttdk_coquanthuequanly", Path = "Cơ Quan Thuế", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_fax", Path = "Fax", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_website", Path = "Website", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_giamdoc", Path = "Giám Đốc", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_dienthoaigiamdoc", Path = "Điện Thoại_Giám Đốc", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_dienthoaigiamdoc_didong", Path = "Di Động_Giám Đốc", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_diachigiamdoc", Path = "Địa Chỉ_Giám Đốc", IsChecked = false });

            ds.Add(new ListItemBox() { Name = "ttlh_ketoan", Path = "Kế Toán", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_dienthoaiketoan", Path = "Điện Thoại_Kế Toán", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_dienthoaiketoan_didong", Path = "Di Động_Kế Toán", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ttlh_diachiketoan", Path = "Địa Chỉ_Kế Toán", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "lvhd_loaihinhkinhte", Path = "Loại Hình Kinh Tế", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "lvhd_linhvuckinhte", Path = "Lĩnh Vực Kinh Tế", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "lvhd_loaihinhtochuc", Path = "Loại Hình Tổ Chức", IsChecked = false });

            ds.Add(new ListItemBox() { Name = "lvhd_capchuong", Path = "Cấp Chương", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "lvhd_loaikhoan", Path = "Loại Khoản", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ds_nganhnghekinhdoanh", Path = "Ngành Nghề Kinh Doanh", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ds_nganhnghekinhdoanhId", Path = "Ngành Nghề Kinh Doanh_ID", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "ds_thuephainop", Path = "Thuế Phải Nộp", IsChecked = false });
            ds.Add(new ListItemBox() { Name = "web_nguon_url", Path = "Link", IsChecked = false });
            
            ds.Add(new ListItemBox() { Name = "create_date", Path = "Ngày Tạo", IsChecked = false });
            
            ((ListBox)checkedListBox1).DataSource = ds;
            ((ListBox)checkedListBox1).DisplayMember = "Path";
            ((ListBox)checkedListBox1).ValueMember = "Name";
        }

        private void frmDialogColumn_Load(object sender, EventArgs e) {
            switch (fromparent) {
                case "hosocongty.vn":
                    BindHoSoCongTy();
                    checkBox1_CheckedChanged(null, null);
                    break;
                case "trangvang.com":
                    BindTrangVang();
                    checkBox1_CheckedChanged(null, null);
                    break;
                case "vatgia.com":
                    BindVatGia();
                    checkBox1_CheckedChanged(null, null);
                    break;
                case "muaban.net":
                    BindMuaban();
                    checkBox1_CheckedChanged(null, null);
                    break;
                case "batdongsan.com.vn":
                    if (strbatdongsanmau == "canmua")
                        BindBatdongsan_canmua();
                    else if (strbatdongsanmau == "canban")
                        BindBatdongsan_canban();
                    else
                        BindBatdongsan_moigioi();
                    checkBox1_CheckedChanged(null, null);
                    break;
                case "vinabiz.org":
                    LoadVinabiz();
                    break;
                case "thitruongsi.com":
                    BindThitruongsi();
                    checkBox1_CheckedChanged(null, null);
                    break;
            }
           // checkBox1_CheckedChanged(null, null);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            setCheckedListBox(checkBox1.Checked);
        }

        private void setCheckedListBox(bool temp) {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemChecked(i, temp);
        }
        private void LoadVinabiz() {
            BindVinabiz();
            setCheckedListBox(false);

            List<dm_DialogColumn> column = SQLDatabase.Loaddm_DialogColumn(string.Format("select * from dm_DialogColumn where IDLoad='{0}'", fromparent));
            for (int i = 0; i < checkedListBox1.Items.Count && column.Count()>0; i++)
            {
                ListItemBox view = (ListItemBox)checkedListBox1.Items[i];
                string value = view.Name;
                if (column.Count(p=>p.Keys== value)>0)
                    checkedListBox1.SetItemChecked(i, true);
            }
        }
        private void SaveVinabiz(string fromparent)
        {
            SQLDatabase.ExcNonQuery(string.Format("delete from dm_DialogColumn where IDLoad='{0}'", fromparent));
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    var company = (ListItemBox)item;
                    SQLDatabase.Adddm_DialogColumn(new dm_DialogColumn() { Keys = company.Name, name = company.Path, IDLoad = fromparent });
                }
                MessageBox.Show("Đã lưu thành công", "Thông Báo");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switch (fromparent)
            {
                case "vinabiz.org":
                    SaveVinabiz(fromparent);
                    break;
            }
        }

        
    }
}
