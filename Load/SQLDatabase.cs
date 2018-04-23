
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Load;

namespace StorePhone
{

    public class MyClass {
        public string name;
        public bool check;
        public bool show;
        public MyClass() {
            name = "";

            check = false;
            show = false;
        }
        public MyClass(string name, bool check, bool show) {
            this.name = name;
            this.check = check;
            this.show = show;
        }
    }
    public class thongbaokiemtra
    {
        public bool trangthai;
        public string NoiDung;
    }

    public class ComboxComlumn {
        public string column { get; set; }
        public string name { get; set; }

    }
    public class ListItem
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
    public class ListItemBox
    {
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public ListItemBox()
        {
            this.IsChecked = true;
            Name = "name1";
            Path = "";
        }
    }
    public class dm_DialogColumn
    {
        public int id { get; set; }
        public string Keys { get; set; }
        public string name { get; set; }
        public string IDLoad { get; set; }
    }
        public class DiaChi_Info
    {
        public string dia_chi { get; set; }
        public string tinh { get; set; }
        public string huyen { get; set; }
        public string xa { get; set; }
    }
    public class PathNext
    {
        public string code { get; set; }
        public string id { get; set; }
        public string last_id { get; set; }
        public string page { get; set; }
        public string album_num { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }

    public class dm_hsct
    {
        #region Fields

        public int id;
        public int? parenid;
        public string maid;
        public string name;
        public string path;
        public int capid;
        public int orderid;

        #endregion

        public dm_hsct()
        {
            parenid = null;
            maid = "";
            name = "";
            path = "";
            capid = 0;
            orderid = 0;
        }
    }
    public class dm_cap
    {
        #region Fields

        public int id;
        public string name;
        #endregion

    }
    public class dm_vatgia
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? paren_id { get; set; }
        public string path { get; set; }
        public int orderid { get; set; }
    }

    public class dm_trangvang {
        public int id { get; set; }
        public string name { get; set; }
        public int? paren_id { get; set; }
        public string path { get; set; }
        public int orderid { get; set; }
    }

    public class dm_tonghopcol {
        public int id { get; set; }
        public string cot { get; set; }
        public string ten { get; set; }

    }

    public class dm_muaban
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? parentId { get; set; }
        public string path { get; set; }
        public int orderid { get; set; }
    }

    public class infoPathmuaban {
        public int PageNow { get; set; }
        public int TotalResult { get; set; }
        public int TotalPagingMax { get; set; }
        public int PageSize { get; set; }

        public infoPathmuaban() {
            this.PageNow = 0;
            this.TotalPagingMax = 0;
            this.TotalResult = 0;
            this.PageSize = 0;
        }
    }

    public class muaban {
        public int id { get; set; }
        public string idtin { get; set; }
        public int danhmucid { get; set; }
        public string danhmucnghe { get; set; }
        public string doituongkh { get; set; }
        public string daxacthuc { get; set; }
        public string duyetboi { get; set; }
        public string tieude { get; set; }
        public string lienhe { get; set; }
        public string diachi { get; set; }
        public string khuvuc { get; set; }
        public string quan { get; set; }
        public string thanhpho { get; set; }
        public string dienthoai { get; set; }
        public string noidung { get; set; }
        public string dienthoai_nd { get; set; }
        public string email_nd { get; set; }
        public string skye_nd { get; set; }
        public string mucluong { get; set; }
        public string mucluongtuden { get; set; }
        public string hoten { get; set; }
        public string gioitinh { get; set; }
        public string ngoaingu { get; set; }
        public string kinhnghiem { get; set; }
        public string namsinh { get; set; }
        public string bangcap { get; set; }
        public string loaihinhcv { get; set; }
        public string vitri { get; set; }
        public string ngaydang { get; set; }
        public string sys_diachiweb { get; set; }
        public muaban()
        {

            this.idtin = "";
            this.danhmucid = 0;
            this.danhmucnghe = "";
            this.doituongkh = "";
            this.daxacthuc = "";
            this.duyetboi = "";
            this.tieude = "";
            this.lienhe = "";
            this.diachi = "";
            this.khuvuc = "";
            this.quan = "";
            this.thanhpho = "";
            this.dienthoai = "";
            this.noidung = "";
            this.dienthoai_nd = "";
            this.email_nd = "";
            this.skye_nd = "";
            this.mucluong = "";
            this.mucluongtuden = "";
            this.hoten = "";
            this.gioitinh = "";
            this.ngoaingu = "";
            this.kinhnghiem = "";
            this.namsinh = "";
            this.bangcap = "";
            this.loaihinhcv = "";
            this.vitri = "";
            this.ngaydang = "";
            this.sys_diachiweb = "";
    }
}

    public class trangvang {
        public int id { get; set; }
        public int danhmucid { get; set; }
        public string msthue { get; set; }
        public string ten_cong_ty { get; set; }
        public string dien_thoai { get; set; }
        public string di_dong { get; set; }
        public string dia_chi { get; set; }
        public string email_cty { get; set; }
        public string website { get; set; }
        public string website2 { get; set; }
        public string loai_hinh { get; set; }
        public string fax { get; set; }
        public string thi_truong { get; set; }
        public string so_nhan_vien { get; set; }
        public string chung_nhan { get; set; }
        public string nganh_nghe { get; set; }
        public string nam_thanh_lap { get; set; }
        public string nguoi_lien_he1 { get; set; }
        public string di_dong1 { get; set; }
        public string di_dong2 { get; set; }
        public string email1 { get; set; }
        public string chuc_vu1 { get; set; }
        public string Skype1 { get; set; }
        public string nguoi_lien_he2 { get; set; }
        public string dien_thoai1 { get; set; }
        public string dien_thoai2 { get; set; }
        public string email2 { get; set; }
        public string Skype2 { get; set; }
        public string chuc_vu2 { get; set; }
        public string sys_diachiweb { get; set; }

        public trangvang() {
            this.id = 0;
            this.danhmucid = 0;
            this.msthue = "";
            this.ten_cong_ty = "";
            this.dien_thoai = "";
            this.di_dong = "";
            this.dia_chi = "";
            this.email_cty = "";
            this.website = "";
            this.website2 = "";
            this.loai_hinh = "";
            this.fax = "";
            this.thi_truong = "";
            this.so_nhan_vien = "";
            this.chung_nhan = "";
            this.nganh_nghe = "";
            this.nam_thanh_lap = "";
            this.nguoi_lien_he1 = "";
            this.di_dong1 = "";
            this.di_dong2 = "";
            this.email1 = "";
            this.Skype1 = "";
            this.chuc_vu1 = "";
            this.nguoi_lien_he2 = "";
            this.dien_thoai1 = "";
            this.dien_thoai2 = "";
            this.email2 = "";
            this.chuc_vu2 = "";
            this.Skype2 = "";
            this.sys_diachiweb = "";
        }


    }

    public class myXML {
        public string id { get; set; }
        public string name { get; set; }
        public string parentid { get; set; }
        public string path { get; set; }
        public string ma { get; set; }
        public string cap { get; set; }
        public string orderid { get; set; }
    }

    public class vatgia
    {
        public int id { get; set; }
        public int danhmucid { get; set; }
        public string tencongty { get; set; }
        public string dia_chi { get; set; }
        public string hotline1 { get; set; }
        public string hotline2 { get; set; }
        public string fax { get; set; }
        public string website { get; set; }
        public string yahoo { get; set; }
        public string lienhe { get; set; }
        public string didong1 { get; set; }
        public string didong2 { get; set; }
        public string didong3 { get; set; }
        public string email1 { get; set; }
        public string email2 { get; set; }
        public string email3 { get; set; }
        public string sys_diachiweb { get; set; }


        public vatgia()
        {
            danhmucid = 0;
            tencongty = "";
            dia_chi = "";
            hotline1 = "";
            hotline2 = "";
            fax = "";
            website = "";
            yahoo = "";
            lienhe = "";
            didong1 = "";
            didong2 = "";
            didong3 = "";
            email1 = "";
            email2 = "";
            email3 = "";
            sys_diachiweb = "";
        }
    }

    public class json_data
    {

        public string total;

        public string last_id;


        public List<album> album;

    }

    public class album
    {
        public string id { get; set; }

        public string title { get; set; }

        public string url { get; set; }

        public string mst { get; set; }

        public string add { get; set; }
    }

    public class hosocongty1
    {
        public int id;
        public string ms_thue;
        public string giam_doc;
        public string dien_thoai;
        public string dia_chi;
        public int danhmucid;
        public string website_cty;
        public string email_cty;
        public string ten_cong_ty;
        public string ten_quoc_te;
        public string ten_giao_dich;
        public string gp_kinh_doanh;
        public string ngay_cap;
        public string ngay_hoat_dong;
        public string ngan_hang;
        public string so_tai_khoan;
        public string ngay_dong_cua;
        public string fax;
        public string nganh_nghe_chinh;
        public string web_nguon_id;
        public string web_nguon_url;
        public string web_nguon_code;

        public hosocongty1()
        {
            ms_thue = "";
            giam_doc = "";
            dien_thoai = "";
            dia_chi = "";
            danhmucid = 0;
            website_cty = "";
            email_cty = "";
            ten_cong_ty = "";
            ten_quoc_te = "";
            ten_giao_dich = "";
            gp_kinh_doanh = "";
            ngay_cap = "";
            ngay_hoat_dong = "";
            ngan_hang = "";
            so_tai_khoan = "";
            ngay_dong_cua = "";
            fax = "";
            nganh_nghe_chinh = "";
            web_nguon_id = "";
            web_nguon_url = "";
            web_nguon_code = "";
        }
    }

    public class dau_so {
        public int id { get; set; }
        public string nhanamg { get; set; }
        public int parentid { get; set; }
        public string dauso { get; set; }
        public int length { get; set; }
        public int orderid { get; set; }
    }

    public class regexs {
        public int id { get; set; }
        public string RegexName { get; set; }
        public string Regex { get; set; }
        public string Example { get; set; }
        public int OrderId { get; set; }
    }

    public class scanner_phone {
        public Guid id { get; set; }
        public Guid? dm_scanner_ct_id { get; set; }
        public string phone { get; set; }
    }

    public class scanner_email {
        public Guid id { get; set; }
        public Guid? dm_scanner_ct_id { get; set; }
        public string email { get; set; }
    }

    public class dm_scanner {
        public int id { get; set; }
        public string name { get; set; }
        public string lienket { get; set; }
        public string domain { get; set; }

        public int orderid { get; set; }
    }

    public class dm_scanner_ct {
        public Guid id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        // public string domain { get; set; }
        public int? parentid { get; set; }
        public int dosau { get; set; }
        //public int orderid { get; set; }
        public bool statur { get; set; }

        public dm_scanner_ct() {
            //id = Guid.NewGuid;
            name = "";
            path = "";
            //domain = "";
            dosau = 0;

            statur = false;
        }

    }
    
    public class SaveAs {
        public int id { get; set; }
        public string id_chon { get; set; }
        public string path_chon { get; set; }
        public string cap_id_chon { get; set; }
        public string group_id_chon { get; set; }
        public string name_chon { get; set; }
        public string ma_chon { get; set; }
        public string parentid_chon { get; set; }
        public string orderid_chon { get; set; }
        public string scannerby { get; set; }

        public SaveAs()
        {
            this.id = 0;
            this.id_chon = "";
            this.path_chon = "";
            this.cap_id_chon = "";
            this.group_id_chon = "";
            this.name_chon = "";
            this.ma_chon = "";
            this.parentid_chon = "";
            this.orderid_chon = "";
            this.scannerby = "";
         }
    }

    public class tonghop {
        public int id { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public string Column6 { get; set; }
        public string Column7 { get; set; }
        public string Column8 { get; set; }
        public string Column9 { get; set; }
        public string Column10 { get; set; }
        public string Column11 { get; set; }
        public string Column12 { get; set; }
        public string Column13 { get; set; }
        public string Column14 { get; set; }
        public string Column15 { get; set; }
        public string Column16 { get; set; }
        public string Column17 { get; set; }
        public string Column18 { get; set; }
        public string Column19 { get; set; }
        public string Column20 { get; set; }
        public string Column21 { get; set; }
        public string Column22 { get; set; }
        public string Column23 { get; set; }
        public string Column24 { get; set; }
        public string Column25 { get; set; }
        public string Column26 { get; set; }
        public string Column27 { get; set; }
        public string Column28 { get; set; }
        public string Column29 { get; set; }
        public string Column30 { get; set; }
        public string Column31 { get; set; }
        public string Column32 { get; set; }
        public string Column33 { get; set; }
        public string Column34 { get; set; }
        public string Column35 { get; set; }
        public string Column36 { get; set; }
        public string Column37 { get; set; }
        public string Column38 { get; set; }
        public string Column39 { get; set; }
        public string Column40 { get; set; }
        public string Column41 { get; set; }
        public string Column42 { get; set; }
        public string Column43 { get; set; }
        public string Column44 { get; set; }
        public string Column45 { get; set; }
        public string Column46 { get; set; }
        public string Column47 { get; set; }
        public string Column48 { get; set; }
        public string Column49 { get; set; }
        public string Column50 { get; set; }
        //Key, Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8, Column9, Column10, Column11, Column12, Column13, Column14, Column15, Column16, Column17, Column18, Column19, Column20, Column21, Column22, Column23, Column24, Column25, Column26, Column27, Column28, Column29, Column30, Column31, Column32, Column33, Column34, Column35, Column36, Column37, Column38, Column39, Column40, Column41, Column42, Column43, Column44, Column45, Column46, Column47, Column48, Column49, Column50
    }

    public class dm_batdongsan
    {
        #region Fields

        public int id;
        public int? parenid;
        public string name;
        public string path;
        public int orderid;

        #endregion

        public dm_batdongsan()
        {
            parenid = null;
            name = "";
            path = "";
            orderid = 0;
        }
    }

    public class batdongsan {
    
        public int id { get; set; }
        public int danhmucid { get; set; }
        public string tieude { get; set; }

        public string khuvuc_canban { get; set; }
        public string quan_canban { get; set; }
        public string thanhpho_canban { get; set; }

        public string quanhuyen_canmua { get; set; }
        public string thanhpho_canmua { get; set; }

        public string giaban { get; set; }
        public string dientich { get; set; }
        public string matindang { get; set; }
        public string noidung { get; set; }
        public string dienthoai_nd { get; set; }
        public string email_nd { get; set; }
        public string loaihinhtindang { get; set; }
        public string ngaydang { get; set; }
        public string ngayhethan { get; set; }
        public string dd_bds_loaitinrao { get; set; }
        public string dd_bds_diachi { get; set; }
        public string dd_bds_huongbancong { get; set; }
        public string dd_bds_sophongngu { get; set; }
        public string dd_bds_sotoilet { get; set; }
        public string dd_bds_noithat { get; set; }

        public string dd_bds_huongnha { get; set; }
        public string dd_bds_sotang { get; set; }
        public string dd_bds_mattien { get; set; }
        public string dd_bds_duongvao { get; set; }
        public string lienhe_tenlienlac { get; set; }
        public string lienhe_diachi { get; set; }
        public string lienhe_dienthoai { get; set; }
        public string lienhe_mobilde { get; set; }
        public string lienhe_email { get; set; }
        public string ttduan_tenduan { get; set; }
        public string ttduan_chudautu { get; set; }
        public string ttduan_quymo { get; set; }
        public string moigioi_tieude { get; set; }
        public string moigioi_diachi { get; set; }

        public string moigioi_soban { get; set; }
        public string moigioi_soban_bydidong { get; set; }
        
        public string moigioi_didong { get; set; }
        public string moigioi_didong_bydidong { get; set; }
        
        public string moigioi_fax { get; set; }
        public string moigioi_email { get; set; }
        public string moigioi_website { get; set; }
        public string moigioi_masothue { get; set; }
        
        public string moigioi_khuvucmoigioi { get; set; }
        public string sys_diachiweb { get; set; }

    }

    public class vinabiz
    {

        public int id { get; set; }
        public int danhmucid { get; set; }
        public string danhmucbyVnbiz { get; set; }
        public string ttdk_msthue { get; set; }
        public string ttdk_tenchinhthuc { get; set; }
        public string ttdk_coquanthuequanly { get; set; }
        public string ttdk_tengiaodich { get; set; }
        public string ttdk_ngaycap { get; set; }
        public string ttdk_ngaybatdauhoatdong { get; set; }
        public string ttdk_trangthai { get; set; }
        public string ttlh_diachitruso { get; set; }
        public int ttlh_tinhid { get; set; }
        public string ttlh_tinh { get; set; }
        public string ttlh_huyen { get; set; }
        public string ttlh_xa { get; set; }
        public string ttlh_dienthoai1 { get; set; }
        public string ttlh_dienthoaididong1 { get; set; }
        public string ttlh_dienthoaididong2 { get; set; }
        public string ttlh_dienthoaididong3 { get; set; }

        public string ttlh_dienthoai_nguoidaidien { get; set; }
        public string ttlh_dienthoai_nguoidaidien_didong { get; set; }

        public string ttlh_email { get; set; }
        public string ttlh_nguoidaidien { get; set; }
        public string ttlh_diachinguoidaidien { get; set; }
        public string ttlh_giamdoc { get; set; }

        public string ttlh_dienthoaigiamdoc { get; set; }
        public string ttlh_dienthoaigiamdoc_didong { get; set; }

        public string ttlh_diachigiamdoc { get; set; }
        public string ttlh_ketoan { get; set; }

        public string ttlh_dienthoaiketoan { get; set; }
        public string ttlh_dienthoaiketoan_didong { get; set; }

        public string ttlh_diachiketoan { get; set; }
        public string ttlh_fax { get; set; }
        public string ttlh_website { get; set; }
        public string ds_nganhnghekinhdoanh { get; set; }
        public string ds_nganhnghekinhdoanhid { get; set; }
        public string nganhnghechinh2 { get; set; }
        public string ds_thuephainop { get; set; }
        public string lvhd_loaihinhkinhte { get; set; }
        public string lvhd_linhvuckinhte { get; set; }
        public string lvhd_loaihinhtochuc { get; set; }
        public string lvhd_capchuong { get; set; }
        public string lvhd_loaikhoan { get; set; }
        public string web_nguon_url { get; set; }

    }

    public class dm_vinabiz
    {
        public int id { get; set; }
        public string ma { get; set; }
        public string name { get; set; }
        public int? paren_id { get; set; }
        public string path { get; set; }
        public int alevel { get; set; }
        public int orderid { get; set; }
    }

    public class dm_vinabiz_map
    {
        public int id { get; set; }
        public string danhmucbyVnbiz { get; set; }
        public string dmhosocongty { get; set; }
        public int hosocongtyid { get; set; }
    }

    public class dm_Tinh {
        public int id { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
    }

    public class dm_thitruongsi
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? paren_id { get; set; }
        public string path { get; set; }
        public int orderid { get; set; }
    }

    public class thitruongsi {
        public int id { get; set; }
        public int danhmucid { get; set; }
        public string tieude { get; set; }
        public string gia_tu_den { get; set; }
        public string toithieu { get; set; }
        public string loaidoanhnghiep { get; set; }
        public string mohinhkinhdoanh { get; set; }
        public string tendoanhnghiep { get; set; }
        public string thoigianhoatdong { get; set; }
        public string ttdn_diachi { get; set; }
        public string ttdn_msthue { get; set; }
        public string ttncc_diachi { get; set; }
        public string ttncc_sdt { get; set; }
        public string ttncc_sdt_didong { get; set; }
        public string ttncc_email { get; set; }
        public string daxacthuc_cogiayphepkinhdoanh { get; set; }
        public string daxacthuc_cokhohang { get; set; }
        public string listdanhmuc { get; set; }
        public string daidien_hoten { get; set; }
        public string daidien_dienthoai { get; set; }
        public string daidien_email { get; set; }
        public string lienhe_kinhdoanh_hoten1 { get; set; }
        public string lienhe_kinhdoanh_sodienthoai1 { get; set; }
        public string lienhe_kinhdoanh_sodienthoaibydidong1 { get; set; }
        public string lienhe_kinhdoanh_hoten2 { get; set; }
        public string lienhe_kinhdoanh_sodienthoai2 { get; set; }
        public string lienhe_kinhdoanh_sodienthoaibydidong2 { get; set; }
        public string quymosx_thitruong { get; set; }
        public string quymosx_nganhhang { get; set; }
        public string quymosx_nhansu { get; set; }
        public string quymosx_sanluong { get; set; }
        public string khohang_diachi { get; set; }
        public string cuahang_diachi { get; set; }
        public string hinhanhxuong_diachi { get; set; }
        public string sys_diachiweb { get; set; }
    }
    //id, name, username, password
    public class CauHinh {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
    public class data
    {
        public string fieldName;

        public string value;

        public data(string name, string val)
        {
            this.fieldName = name;
            this.value = val;
        }

        public data(string line)
        {
            char[] op = new char[]
            {
                '='
            };
            string[] s = line.Split(op);
            if (s.Length == 1 || s.Length == 0)
            {
                this.fieldName = null;
                this.value = null;
            }
            else if (s.Length == 2)
            {
                this.fieldName = s[0];
                this.value = s[1];
            }
            else
            {
                this.fieldName = s[0];
                this.value = line.Substring(s[0].Length + 1);
            }
        }
    }
    public class FileDataDictionary
    {
        private System.Collections.Generic.Dictionary<string, string> ds;

        public FileDataDictionary(string fileName)
        {
            try
            {
                this.ds = new System.Collections.Generic.Dictionary<string, string>();
                System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                while (sr.Peek() != -1)
                {
                    string line = sr.ReadLine();
                    if (line.Substring(0, 2) != "//")
                    {
                        data d = new data(line);
                        if (d.value != null)
                        {
                            this.ds.Add(d.fieldName, d.value);
                        }
                    }
                }
                sr.Close();
            }
            catch (System.Exception e_8D)
            {
            }
        }

        public string getValue(string name)
        {
            string result;
            if (this.ds.ContainsKey(name))
            {
                result = this.ds[name];
            }
            else
            {
                result = null;
            }
            return result;
        }

        public bool writeFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            System.IO.TextWriter writer = new System.IO.StreamWriter(filePath);
            foreach (System.Collections.Generic.KeyValuePair<string, string> pair in this.ds)
            {
                string line = pair.Key + "=" + pair.Value;
                writer.WriteLine(line);
            }
            writer.Close();
            return false;
        }
    }

    class SQLDatabase
    {

        #region Fields
        public static string ConnectionString
        {
            get
            {
                FileDataDictionary FDD = new FileDataDictionary("setup.con");
                return FDD.getValue("connectionstring");
            }
        }
        private static LogWriter writer;

        #endregion // Fields

        #region Hệ Thống
        public static bool Adddm_DialogColumn(dm_DialogColumn record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into dm_DialogColumn(keys,name, IDLoad)" +
                                    "values(@keys,@name, @IDLoad);" +
                                    "Select SCOPE_IDENTITY();";
              
                cmd.Parameters.AddWithValue("@keys", record.Keys);
                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@IDLoad", record.IDLoad);
            

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value) return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                writer.WriteToLog(string.Format("{0}-{1}-{2}", ex.Message, "dm_DialogColumn", "error1"));
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static List<dm_DialogColumn> Loaddm_DialogColumn(string sql)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            dm_DialogColumn InfoCOMMANDTABLE;
            List<dm_DialogColumn> InfoCOMMANDTABLEs = null;

            try
            {
                InfoCOMMANDTABLEs = new List<dm_DialogColumn>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = cnn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    InfoCOMMANDTABLE = new dm_DialogColumn();


                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.Keys = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.name = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        InfoCOMMANDTABLE.IDLoad = reader.GetString(3);
                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        #endregion

        #region Hồ sơ công ty

        public static List<dm_hsct> Loaddm_hsct(string sql)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            dm_hsct InfoCOMMANDTABLE;
            List<dm_hsct> InfoCOMMANDTABLEs = null;

            try
            {
                InfoCOMMANDTABLEs = new List<dm_hsct>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = cnn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    InfoCOMMANDTABLE = new dm_hsct();


                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.parenid = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.maid = reader.GetString(2);
                    if (!reader.IsDBNull(4))
                        InfoCOMMANDTABLE.name = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        InfoCOMMANDTABLE.path = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        InfoCOMMANDTABLE.capid = reader.GetInt32(6);
                    if (!reader.IsDBNull(7))
                        InfoCOMMANDTABLE.orderid = reader.GetInt32(7);
                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }


        public static bool AddHosocongty(hosocongty1 record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into hosocongty(danhmucId,ms_thue, ten_cong_ty, ten_quoc_te, ten_giao_dich, giam_doc, dien_thoai, fax,email, dia_chi,  website_cty, ngan_hang,so_tai_khoan,ngay_dong_cua, gp_kinh_doanh, ngay_cap,ngay_hoat_dong,nganh_nghe_chinh, web_nguon_id, web_nguon_url,web_nguon_code)" +
                                    "values(@danhmucId,@ms_thue, @ten_cong_ty, @ten_quoc_te, @ten_giao_dich, @giam_doc, @dien_thoai, @fax,@email, @dia_chi,  @website_cty, @ngan_hang,@so_tai_khoan,@ngay_dong_cua, @gp_kinh_doanh, @ngay_cap,@ngay_hoat_dong,@nganh_nghe_chinh, @web_nguon_id, @web_nguon_url,@web_nguon_code);" +
                                    "Select SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@danhmucId", record.danhmucid);
                cmd.Parameters.AddWithValue("@ms_thue", record.ms_thue);
                cmd.Parameters.AddWithValue("@ten_cong_ty", record.ten_cong_ty);
                cmd.Parameters.AddWithValue("@ten_quoc_te", record.ten_quoc_te);
                cmd.Parameters.AddWithValue("@ten_giao_dich", record.ten_giao_dich);
                cmd.Parameters.AddWithValue("@giam_doc", record.giam_doc);
                cmd.Parameters.AddWithValue("@dien_thoai", record.dien_thoai);
                cmd.Parameters.AddWithValue("@fax", record.fax);
                cmd.Parameters.AddWithValue("@email", record.email_cty);
                cmd.Parameters.AddWithValue("@dia_chi", record.dia_chi);
                cmd.Parameters.AddWithValue("@website_cty", record.website_cty);
                cmd.Parameters.AddWithValue("@ngan_hang", record.ngan_hang);
                cmd.Parameters.AddWithValue("@so_tai_khoan", record.so_tai_khoan);
                cmd.Parameters.AddWithValue("@ngay_dong_cua", record.ngay_dong_cua);
                cmd.Parameters.AddWithValue("@gp_kinh_doanh", record.gp_kinh_doanh);
                cmd.Parameters.AddWithValue("@ngay_cap", record.ngay_cap);
                cmd.Parameters.AddWithValue("@ngay_hoat_dong", record.ngay_hoat_dong);
                cmd.Parameters.AddWithValue("@nganh_nghe_chinh", record.nganh_nghe_chinh);
                cmd.Parameters.AddWithValue("@web_nguon_id", record.web_nguon_id);
                cmd.Parameters.AddWithValue("@web_nguon_url", record.web_nguon_url);
                cmd.Parameters.AddWithValue("@web_nguon_code", record.web_nguon_code);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value) return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                writer.WriteToLog(string.Format("{0}-{1}-MST:{2}-{3}", ex.Message, "AddHosocongty", record.ms_thue, "error1"));
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool Adddm_hsct(dm_hsct record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into dm_hsct(parentId,maid, name, path, capid, orderid)" +
                                    "values(@parentId,@maid, @name, @path, @capid, @orderid);" +
                                    "Select SCOPE_IDENTITY();";
                if (record.parenid == null)
                    cmd.Parameters.AddWithValue("@parentId", SqlInt32.Null);
                else
                    cmd.Parameters.AddWithValue("@parentId", record.parenid);
                cmd.Parameters.AddWithValue("@maid", record.maid);
                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@path", record.path);
                cmd.Parameters.AddWithValue("@capid", record.capid);
                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value) return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                writer.WriteToLog(string.Format("{0}-{1}-{2}", ex.Message, "AddHosocongty", "error1"));
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool Updatedm_hsct(dm_hsct record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null) return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;
                //id, parentId, ma, name, path, capid, orderid, webId, createdate
                cmd.CommandText = "Update dm_hsct " +
                                    " Set   name=@name, " +
                                    "       maid=@maid," +
                                    "       path=@path," +
                                    "       capid=@capid," +
                                    "       orderid=@orderid," +
                                    "       parentId=@parentId" +
                                    " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                if (record.parenid == null)
                    cmd.Parameters.AddWithValue("@parentId", SqlInt32.Null);
                else
                    cmd.Parameters.AddWithValue("@parentId", record.parenid);
                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@maid", record.maid);
                cmd.Parameters.AddWithValue("@path", record.path);
                cmd.Parameters.AddWithValue("@capid", record.capid);
                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Updatedm_hsct");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        #endregion

        #region Trang Vàng

        public static bool AdddmTrangVang(dm_trangvang record) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into dm_trangvang( name, parentId, path, orderid)" +
                                    "values( @name, @parentId, @path, @orderid);" +
                                    "Select SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@name", record.name);
                if (record.paren_id == null)
                    cmd.Parameters.AddWithValue("@parentId", SqlInt32.Null);
                else
                    cmd.Parameters.AddWithValue("@parentId", record.paren_id);
                cmd.Parameters.AddWithValue("@path", record.path);
                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value)
                    return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex) {
                writer = LogWriter.Instance;
                return false;
            }
            finally {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool UpdatedmTrangVang(dm_trangvang record) {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try {
                if (record == null)
                    return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "Update [dm_vatgia] Set name=@name, "
                                    + " path=@path,parentId=@parentId,orderid=@orderid "
                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@path", record.path);
                if (record.paren_id == null)
                    cmd.Parameters.AddWithValue("@parentId", SqlInt32.Null);
                else
                    cmd.Parameters.AddWithValue("@parentId", record.paren_id);

                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "UpdatedmVatGia");
                return false;
            }
            finally {
                if (connection != null)
                    connection.Close();
            }
        }


        public static bool AddTrangVang(trangvang record) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into trangvang(danhmucid, msthue, ten_cong_ty, dien_thoai,di_dong, dia_chi,  email_cty, website, website2, loai_hinh, fax, thi_truong, so_nhan_vien, chung_nhan, nganh_nghe, nam_thanh_lap, nguoi_lien_he1, di_dong1, di_dong2, email1, chuc_vu1, nguoi_lien_he2, dien_thoai1, dien_thoai2, email2, chuc_vu2, sys_diachiweb)" +
                                    "values(@danhmucid, @msthue, @ten_cong_ty, @dien_thoai,@di_dong, @dia_chi,  @email_cty, @website, @website2, @loai_hinh, @fax, @thi_truong, @so_nhan_vien, @chung_nhan, @nganh_nghe, @nam_thanh_lap, @nguoi_lien_he1, @di_dong1, @di_dong2, @email1, @chuc_vu1, @nguoi_lien_he2, @dien_thoai1, @dien_thoai2, @email2, @chuc_vu2, @sys_diachiweb);" +
                                    "Select SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@danhmucid", record.danhmucid);
                cmd.Parameters.AddWithValue("@msthue", record.msthue);
                cmd.Parameters.AddWithValue("@ten_cong_ty", record.ten_cong_ty);
                cmd.Parameters.AddWithValue("@dien_thoai", record.dien_thoai);
                cmd.Parameters.AddWithValue("@di_dong", record.di_dong);
                cmd.Parameters.AddWithValue("@dia_chi", record.dia_chi);
                cmd.Parameters.AddWithValue("@email_cty", record.email_cty);
                cmd.Parameters.AddWithValue("@website", record.website);
                cmd.Parameters.AddWithValue("@website2", record.website2);
                cmd.Parameters.AddWithValue("@loai_hinh", record.loai_hinh);
                cmd.Parameters.AddWithValue("@fax", record.fax);
                cmd.Parameters.AddWithValue("@thi_truong", record.thi_truong);
                cmd.Parameters.AddWithValue("@so_nhan_vien", record.so_nhan_vien);
                cmd.Parameters.AddWithValue("@chung_nhan", record.chung_nhan);
                cmd.Parameters.AddWithValue("@nganh_nghe", record.nganh_nghe);
                cmd.Parameters.AddWithValue("@nam_thanh_lap", record.nam_thanh_lap);
                cmd.Parameters.AddWithValue("@nguoi_lien_he1", record.nguoi_lien_he1);
                cmd.Parameters.AddWithValue("@di_dong1", record.di_dong1);
                cmd.Parameters.AddWithValue("@di_dong2", record.di_dong2);
                cmd.Parameters.AddWithValue("@email1", record.email1);
                cmd.Parameters.AddWithValue("@chuc_vu1", record.chuc_vu1);
                cmd.Parameters.AddWithValue("@Skype1", record.Skype1);
                cmd.Parameters.AddWithValue("@nguoi_lien_he2", record.nguoi_lien_he2);
                cmd.Parameters.AddWithValue("@dien_thoai1", record.dien_thoai1);
                cmd.Parameters.AddWithValue("@dien_thoai2", record.dien_thoai2);
                cmd.Parameters.AddWithValue("@email2", record.email2);
                cmd.Parameters.AddWithValue("@chuc_vu2", record.chuc_vu2);
                cmd.Parameters.AddWithValue("@Skype2", record.Skype2);
                cmd.Parameters.AddWithValue("@sys_diachiweb", record.sys_diachiweb);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value)
                    return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex) {
                writer = LogWriter.Instance;
                writer.WriteToLog(string.Format("{0}-{1}-MST:{2}-{3}", ex.Message, "AddTrangVang", record.msthue, "error1"));
                return false;
            }
            finally {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        #endregion

        #region Vật Gía

        public static bool AdddmVatGia(dm_vatgia record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into dm_vatgia( name, parentId, path, orderid)" +
                                    "values( @name, @parentId, @path, @orderid);" +
                                    "Select SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@name", record.name);
                if (record.paren_id == null)
                    cmd.Parameters.AddWithValue("@parentId", SqlInt32.Null);
                else
                    cmd.Parameters.AddWithValue("@parentId", record.paren_id);
                cmd.Parameters.AddWithValue("@path", record.path);
                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value) return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool UpdatedmVatGia(dm_vatgia record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null) return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "Update [dm_vatgia] Set name=@name, "
                                    + " path=@path,parentId=@parentId,orderid=@orderid "
                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@path", record.path);
                if (record.paren_id == null)
                    cmd.Parameters.AddWithValue("@parentId", SqlInt32.Null);
                else
                    cmd.Parameters.AddWithValue("@parentId", record.paren_id);

                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdatedmVatGia");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static bool DelVatGia(dm_vatgia recode)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                if (recode == null)
                    return false;

                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DELETE FROM dm_vatgia WHERE ID ='" + recode.id + "'";

                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DelVatGia");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static bool AddVatGia(vatgia record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into vatgia(danhmucid, tencongty, dia_chi, hotline1, hotline2, fax, website,yahoo, lienhe, didong1,didong2,didong3, email1,email2,email3,sys_diachiweb)" +
                                    "values( @danhmucid, @tencongty, @dia_chi, @hotline1, @hotline2, @fax, @website,@yahoo, @lienhe, @didong1,@didong2,@didong3, @email1,@email2,@email3,@sys_diachiweb);" +
                                    "Select SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@danhmucid", record.danhmucid);
                cmd.Parameters.AddWithValue("@tencongty", record.tencongty);
                cmd.Parameters.AddWithValue("@dia_chi", record.dia_chi);
                cmd.Parameters.AddWithValue("@hotline1", record.hotline1);
                cmd.Parameters.AddWithValue("@hotline2", record.hotline2);
                cmd.Parameters.AddWithValue("@fax", record.fax);
                cmd.Parameters.AddWithValue("@website", record.website);
                cmd.Parameters.AddWithValue("@yahoo", record.yahoo);
                cmd.Parameters.AddWithValue("@lienhe", record.lienhe);
                cmd.Parameters.AddWithValue("@didong1", record.didong1);
                cmd.Parameters.AddWithValue("@didong2", record.didong2);
                cmd.Parameters.AddWithValue("@didong3", record.didong3);
                cmd.Parameters.AddWithValue("@email1", record.email1);
                cmd.Parameters.AddWithValue("@email2", record.email2);
                cmd.Parameters.AddWithValue("@email3", record.email3);
                cmd.Parameters.AddWithValue("@sys_diachiweb", record.sys_diachiweb);


                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value) return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                writer.WriteToLog(string.Format("{0}-{1}-diachiwebsite:{2}-{3}", ex.Message, "AddVatGia", record.sys_diachiweb, "error1"));
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }


        #endregion

        #region Đầu số vs Regexs
        public static List<dau_so> Loaddau_so(string sql) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            dau_so InfoCOMMANDTABLE;
            List<dau_so> InfoCOMMANDTABLEs = null;

            try {
                InfoCOMMANDTABLEs = new List<dau_so>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = cnn;
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    InfoCOMMANDTABLE = new dau_so();


                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.nhanamg = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.parentid = reader.GetInt32(2);
                    if (!reader.IsDBNull(3))
                        InfoCOMMANDTABLE.dauso = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        InfoCOMMANDTABLE.length = reader.GetInt32(4);
                    if (!reader.IsDBNull(5))
                        InfoCOMMANDTABLE.orderid = reader.GetInt32(5);
                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex) {
                throw ex;
            }
            finally {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }
        public static List<regexs> LoadRegexs(string sql) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            regexs InfoCOMMANDTABLE;
            List<regexs> InfoCOMMANDTABLEs = null;

            try {
                InfoCOMMANDTABLEs = new List<regexs>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = cnn;
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    InfoCOMMANDTABLE = new regexs();


                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.RegexName = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.Regex = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        InfoCOMMANDTABLE.Example = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        InfoCOMMANDTABLE.OrderId = reader.GetInt32(4);
                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex) {
                throw ex;
            }
            finally {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        //private static object threadLock_Scanner_phone = new object();
        public static bool AddScanner_phone(scanner_phone record) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

           // lock (threadLock_Scanner_phone)
            {
                try
                {
                    if (record == null)
                        return false;

                    cnn = new SqlConnection();
                    cnn.ConnectionString = ConnectionString;
                    cnn.FireInfoMessageEventOnUserErrors = false;
                    cnn.Open();

                    cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    //--- Insert Record

                    cmd.CommandText = "Insert into scanner_phone( phone,dm_scanner_ct_id)" +
                                        " OUTPUT inserted.id " +
                                       " values( @phone, @dm_scanner_ct_id); ";


                    cmd.Parameters.AddWithValue("@phone", record.phone);
                    cmd.Parameters.AddWithValue("@dm_scanner_ct_id", record.dm_scanner_ct_id);

                    Guid guid = (Guid)cmd.ExecuteScalar();

                    if (guid == null || guid == Guid.Empty)
                        return false;

                    record.id = new Guid(guid.ToString());

                    return true;
                }
                catch (Exception ex)
                {
                    writer = LogWriter.Instance;
                    writer.WriteToLog(string.Format("{0}-{1}-AddScanner_phone:{2}", ex.Message, "AddVatGia", record.phone));
                    return false;
                }
                finally
                {
                    if (cnn.State == ConnectionState.Open)
                        cnn.Close();
                }
            }
        }
        

        public static bool AddScanner_email(scanner_email record) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;


          
                try
                {
                    if (record == null)
                        return false;

                    cnn = new SqlConnection();
                    cnn.ConnectionString = ConnectionString;
                    cnn.FireInfoMessageEventOnUserErrors = false;
                    cnn.Open();

                    cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    //--- Insert Record
                    cmd.CommandText = "Insert into scanner_email( email, dm_scanner_ct_id)" +
                                        " OUTPUT inserted.id " +
                                        " values( @email, @dm_scanner_ct_id);";

                    cmd.Parameters.AddWithValue("@email", record.email);
                    cmd.Parameters.AddWithValue("@dm_scanner_ct_id", record.dm_scanner_ct_id);

                    Guid guid = (Guid)cmd.ExecuteScalar();

                    if (guid == null || guid == Guid.Empty)
                        return false;

                    record.id = new Guid(guid.ToString());

                    return true;
                }
                catch (Exception ex)
                {
                    writer = LogWriter.Instance;
                    writer.WriteToLog(string.Format("{0}-{1}-AddScanner_email:{2}", ex.Message, "AddScanner_email", record.email));
                    return false;
                }
                finally
                {
                    if (cnn.State == ConnectionState.Open)
                        cnn.Close();
                }
        }
        //private static object threadLock_Add_dm_scanner_ct = new object();
        public static bool Add_dm_scanner_ct(dm_scanner_ct record) {
            //lock (threadLock_Add_dm_scanner_ct) {
                try {
                    DataTable tb = SQLDatabase.ExcDataTable(string.Format("[CapNhat_dm_scanner_ct] '{0}','{1}',{2},{3},'{4}'", record.name, record.path, record.parentid,record.statur, record.dosau));
                    return ConvertType.ToInt(tb.Rows[0][0]) == 1 ? true : false;
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Add_Spdm_scanner");
                    return false;
                }
            //}
        }

        
        public static bool Adddm_scanner_ct1(dm_scanner_ct record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
                try
                {
                    if (record == null)
                        return false;

                    cnn = new SqlConnection();
                    cnn.ConnectionString = ConnectionString;
                    cnn.FireInfoMessageEventOnUserErrors = false;
                    cnn.Open();

                    cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    //--- Insert Record
                    cmd.CommandText = "Insert into dm_scanner_ct( name, path, parentid,  statur, dosau)" +
                                        " OUTPUT inserted.id " +
                                        "values(@name, @path, @parentid, @statur, @dosau);";

                    cmd.Parameters.AddWithValue("@name", record.name);
                    cmd.Parameters.AddWithValue("@path", record.path);
                    cmd.Parameters.AddWithValue("@parentid", record.parentid);
                    cmd.Parameters.AddWithValue("@statur", record.statur);
                    cmd.Parameters.AddWithValue("@dosau", record.dosau);

                    Guid guid = (Guid)cmd.ExecuteScalar();

                    if (guid == null || guid == Guid.Empty)
                        return false;

                    record.id = new Guid(guid.ToString());

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (cnn.State == ConnectionState.Open)
                        cnn.Close();
                }
        }

       // private static object threadLock_dm_scanner = new object();
        public static bool Adddm_scanner(dm_scanner record) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            //lock (threadLock_dm_scanner)
            {
                try
                {
                    if (record == null)
                        return false;

                    cnn = new SqlConnection();
                    cnn.ConnectionString = ConnectionString;
                    cnn.FireInfoMessageEventOnUserErrors = false;
                    cnn.Open();

                    cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    //--- Insert Record
                    cmd.CommandText = "Insert into dm_scanner( name, lienket, domain, orderid)" +
                                        "values(@name, @lienket, @domain, @orderid);" +
                                        "Select SCOPE_IDENTITY();";

                    cmd.Parameters.AddWithValue("@name", record.name);
                    cmd.Parameters.AddWithValue("@lienket", record.lienket);
                    cmd.Parameters.AddWithValue("@domain", record.domain);
                    cmd.Parameters.AddWithValue("@orderid", record.orderid);
                    

                    objectID = cmd.ExecuteScalar();

                    if (objectID == null || objectID == DBNull.Value)
                        return false;

                    record.id = Convert.ToInt32(objectID);

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (cnn.State == ConnectionState.Open)
                        cnn.Close();
                }
            }
        }

       


        public static List<dm_scanner> Loaddm_scanner(string sql) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            dm_scanner InfoCOMMANDTABLE;
            List<dm_scanner> InfoCOMMANDTABLEs = null;

            try {
                InfoCOMMANDTABLEs = new List<dm_scanner>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = cnn;
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    InfoCOMMANDTABLE = new dm_scanner();

                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.name = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.lienket = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        InfoCOMMANDTABLE.domain = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        InfoCOMMANDTABLE.orderid = reader.GetInt32(4);
                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex) {
                throw ex;
            }
            finally {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static List<dm_scanner_ct> Loaddm_scanner_ct(string sql) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            dm_scanner_ct InfoCOMMANDTABLE;
            List<dm_scanner_ct> InfoCOMMANDTABLEs = null;

            try {
                InfoCOMMANDTABLEs = new List<dm_scanner_ct>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = cnn;
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    InfoCOMMANDTABLE = new dm_scanner_ct();
                                 
                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetGuid(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.name = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.path = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        InfoCOMMANDTABLE.parentid = reader.GetInt32(3);
                   
                    if (!reader.IsDBNull(4))
                        InfoCOMMANDTABLE.statur = reader.GetBoolean(4);
                    if (!reader.IsDBNull(5))
                        InfoCOMMANDTABLE.dosau = reader.GetInt32(5);
                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex) {
                throw ex;
            }
            finally {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        //private static object threadLock_dm_scanner_ct = new object();
        public static bool Updatedm_scanner_ct(dm_scanner_ct record) {
            SqlConnection connection = null;
            SqlCommand cmd = null;

           // lock (threadLock_dm_scanner_ct)
            {
                try
                {
                    if (record == null)
                        return false;

                    // Make connection to database
                    connection = new SqlConnection();
                    connection.ConnectionString = ConnectionString;
                    connection.FireInfoMessageEventOnUserErrors = false;
                    connection.Open();
                    // Create command to update GeneralGuessGroup record
                    cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Update [dm_scanner_ct] Set name=@name, path=@path, parentid=@parentid, statur=@statur,dosau=@dosau "
                                        + " where ID='" + record.id + "'";
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@name", record.name);
                    cmd.Parameters.AddWithValue("@path", record.path);
                  
                    cmd.Parameters.AddWithValue("@parentid", record.parentid);
                    cmd.Parameters.AddWithValue("@statur", record.statur);
                    cmd.Parameters.AddWithValue("@dosau", record.dosau);

                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "Updatedm_scanner");
                    return false;
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
            }
        }

        public static bool Updatedm_scanner(dm_scanner record) {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try {
                if (record == null)
                    return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "Update [dm_scanner] Set name=@name,lienket=@lienket, domain=@domain,@orderid=@orderid "
                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@lienket", record.lienket);
                cmd.Parameters.AddWithValue("@domain", record.domain);
                cmd.Parameters.AddWithValue("@orderid", record.orderid);
                

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex) {
                //MessageBox.Show(ex.Message, "Updatedm_scanner");
                return false;
            }
            finally {
                if (connection != null)
                    connection.Close();
            }
        }
        public static bool Deldm_scanner(dm_scanner_ct recode) {
            SqlConnection connection = null;
            SqlCommand command = null;

            try {
                if (recode == null)
                    return false;

                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DELETE FROM dm_scanner WHERE ID ='" + recode.id + "'";

                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Deldm_scanner");
                return false;
            }
            finally {
                if (connection != null)
                    connection.Close();
            }
        }
        #endregion

        #region Tổng Hop

        public static bool AddTongHop(tonghop record) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into tonghop( Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8, Column9, Column10, Column11, Column12, Column13, Column14, Column15, Column16, Column17, Column18, Column19, Column20, Column21, Column22, Column23, Column24, Column25, Column26, Column27, Column28, Column29, Column30, Column31, Column32, Column33, Column34, Column35, Column36, Column37, Column38, Column39, Column40, Column41, Column42, Column43, Column44, Column45, Column46, Column47, Column48, Column49, Column50)" +
                                    "values(@Column1, @Column2, @Column3, @Column4, @Column5, @Column6, @Column7, @Column8, @Column9, @Column10, @Column11, @Column12, @Column13, @Column14, @Column15, @Column16, @Column17, @Column18, @Column19, @Column20, @Column21, @Column22, @Column23, @Column24, @Column25, @Column26, @Column27, @Column28, @Column29, @Column30, @Column31, @Column32, @Column33, @Column34, @Column35, @Column36, @Column37, @Column38, @Column39, @Column40, @Column41, @Column42, @Column43, @Column44, @Column45, @Column46, @Column47, @Column48, @Column49, @Column50);" +
                                    "Select SCOPE_IDENTITY();";

                
                cmd.Parameters.AddWithValue("@Column1", record.Column1);
                cmd.Parameters.AddWithValue("@Column2", record.Column1);
                cmd.Parameters.AddWithValue("@Column3", record.Column1);
                cmd.Parameters.AddWithValue("@Column4", record.Column1);
                cmd.Parameters.AddWithValue("@Column5", record.Column1);
                cmd.Parameters.AddWithValue("@Column6", record.Column1);
                cmd.Parameters.AddWithValue("@Column7", record.Column1);
                cmd.Parameters.AddWithValue("@Column8", record.Column1);
                cmd.Parameters.AddWithValue("@Column9", record.Column1);
                cmd.Parameters.AddWithValue("@Column10", record.Column1);
                cmd.Parameters.AddWithValue("@Column11", record.Column1);
                cmd.Parameters.AddWithValue("@Column12", record.Column1);
                cmd.Parameters.AddWithValue("@Column13", record.Column1);
                cmd.Parameters.AddWithValue("@Column14", record.Column1);
                cmd.Parameters.AddWithValue("@Column15", record.Column1);
                cmd.Parameters.AddWithValue("@Column16", record.Column1);
                cmd.Parameters.AddWithValue("@Column17", record.Column1);
                cmd.Parameters.AddWithValue("@Column18", record.Column1);
                cmd.Parameters.AddWithValue("@Column19", record.Column1);
                cmd.Parameters.AddWithValue("@Column20", record.Column1);
                cmd.Parameters.AddWithValue("@Column21", record.Column1);
                cmd.Parameters.AddWithValue("@Column22", record.Column1);
                cmd.Parameters.AddWithValue("@Column23", record.Column1);
                cmd.Parameters.AddWithValue("@Column24", record.Column1);
                cmd.Parameters.AddWithValue("@Column25", record.Column1);
                cmd.Parameters.AddWithValue("@Column26", record.Column1);
                cmd.Parameters.AddWithValue("@Column27", record.Column1);
                cmd.Parameters.AddWithValue("@Column28", record.Column1);
                cmd.Parameters.AddWithValue("@Column29", record.Column1);
                cmd.Parameters.AddWithValue("@Column30", record.Column1);
                cmd.Parameters.AddWithValue("@Column31", record.Column1);
                cmd.Parameters.AddWithValue("@Column32", record.Column1);
                cmd.Parameters.AddWithValue("@Column33", record.Column1);
                cmd.Parameters.AddWithValue("@Column34", record.Column1);
                cmd.Parameters.AddWithValue("@Column35", record.Column1);
                cmd.Parameters.AddWithValue("@Column36", record.Column1);
                cmd.Parameters.AddWithValue("@Column37", record.Column1);
                cmd.Parameters.AddWithValue("@Column38", record.Column1);
                cmd.Parameters.AddWithValue("@Column39", record.Column1);
                cmd.Parameters.AddWithValue("@Column40", record.Column1);
                cmd.Parameters.AddWithValue("@Column41", record.Column1);
                cmd.Parameters.AddWithValue("@Column42", record.Column1);
                cmd.Parameters.AddWithValue("@Column43", record.Column1);
                cmd.Parameters.AddWithValue("@Column44", record.Column1);
                cmd.Parameters.AddWithValue("@Column45", record.Column1);
                cmd.Parameters.AddWithValue("@Column46", record.Column1);
                cmd.Parameters.AddWithValue("@Column47", record.Column1);
                cmd.Parameters.AddWithValue("@Column48", record.Column1);
                cmd.Parameters.AddWithValue("@Column49", record.Column1);
                cmd.Parameters.AddWithValue("@Column50", record.Column1);
                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value)
                    return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex) {
                writer = LogWriter.Instance;
                return false;
            }
            finally {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static List<dm_tonghopcol> Loaddm_tonghopcol(string sql) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            dm_tonghopcol InfoCOMMANDTABLE;
            List<dm_tonghopcol> InfoCOMMANDTABLEs = null;

            try {
                InfoCOMMANDTABLEs = new List<dm_tonghopcol>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = cnn;
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    InfoCOMMANDTABLE = new dm_tonghopcol();


                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.cot = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.ten = reader.GetString(2);
                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex) {
                throw ex;
            }
            finally {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool Adddm_tonghopcol(dm_tonghopcol record) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into tonghopcol(cot, ten)" +
                                    "values(@cot, @ten);" +
                                    "Select SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@cot", record.cot);
                cmd.Parameters.AddWithValue("@ten", record.ten);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value)
                    return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex) {
                writer = LogWriter.Instance;
                return false;
            }
            finally {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool Updatedm_tonghopcol(dm_tonghopcol record) {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try {
                if (record == null)
                    return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "Update [tonghopcol] Set ten=@ten, "
                                    + " cot=@column"
                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@cot", record.cot);
                cmd.Parameters.AddWithValue("@ten", record.ten);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Updatedm_tonghopcol");
                return false;
            }
            finally {
                if (connection != null)
                    connection.Close();
            }
        }

        public static List<SaveAs> LoadSaveAs(string sql) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            SaveAs InfoCOMMANDTABLE;
            List<SaveAs> InfoCOMMANDTABLEs = null;

            try {
                InfoCOMMANDTABLEs = new List<SaveAs>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = cnn;
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    InfoCOMMANDTABLE = new SaveAs();


                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.id_chon = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.path_chon = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        InfoCOMMANDTABLE.cap_id_chon = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        InfoCOMMANDTABLE.group_id_chon = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        InfoCOMMANDTABLE.name_chon = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        InfoCOMMANDTABLE.parentid_chon = reader.GetString(6);
                    if (!reader.IsDBNull(7))
                        InfoCOMMANDTABLE.orderid_chon = reader.GetString(7);
                     if (!reader.IsDBNull(7))
                         InfoCOMMANDTABLE.scannerby = reader.GetString(7);
                    
                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex) {
                throw ex;
            }
            finally {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool AddSaveAs(SaveAs record) {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into SaveAs( id_chon, path_chon, cap_id_chon, group_id_chon, name_chon,ma_chon, parentid_chon, orderid_chon,scannerby)" +
                                    "values( @id_chon, @path_chon, @cap_id_chon, @group_id_chon, @name_chon,@ma_chon, @parentid_chon, @orderid_chon,@scannerby);" +
                                    "Select SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@id_chon", record.id_chon);
                cmd.Parameters.AddWithValue("@path_chon", record.path_chon);
                cmd.Parameters.AddWithValue("@cap_id_chon", record.cap_id_chon);
                cmd.Parameters.AddWithValue("@group_id_chon", record.group_id_chon);
                cmd.Parameters.AddWithValue("@name_chon", record.name_chon);
                cmd.Parameters.AddWithValue("@ma_chon", record.ma_chon);
                cmd.Parameters.AddWithValue("@parentid_chon", record.parentid_chon);
                cmd.Parameters.AddWithValue("@orderid_chon", record.orderid_chon);
                cmd.Parameters.AddWithValue("@scannerby", record.scannerby);
                
                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value)
                    return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex) {
                writer = LogWriter.Instance;
                return false;
            }
            finally {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }


        public static bool DelSaveAs(SaveAs recode) {
            SqlConnection connection = null;
            SqlCommand command = null;

            try {
                if (recode == null)
                    return false;

                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DELETE FROM SaveAs WHERE ID ='" + recode.id + "'";

                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "DelSaveAs");
                return false;
            }
            finally {
                if (connection != null)
                    connection.Close();
            }
        }

        #endregion

        #region muaban

        public static bool AddmMuaban(dm_muaban record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into dm_muaban( name,parentId, path, orderid)" +
                                    "values( @name,@parentId,  @path, @orderid);" +
                                    "Select SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@parentId", record.parentId);
                cmd.Parameters.AddWithValue("@path", record.path);
                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value)
                    return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }


        public static bool UpdatedmMuaban(dm_muaban record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null)
                    return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "Update [dm_muaban] Set name=@name, "
                                    + " path=@path,orderid=@orderid "
                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@path", record.path);
                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Updatedmmuaban_timviec");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }


        public static bool AddMuaban_timviec(muaban record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into muaban(idtin, danhmucid,danhmucnghe,doituongkh ,daxacthuc,duyetboi,tieude, lienhe, diachi,khuvuc,quan,thanhpho,  dienthoai, noidung, dienthoai_nd, email_nd, skye_nd, mucluong, mucluongtuden, hoten, gioitinh, ngoaingu, kinhnghiem, namsinh, bangcap, loaihinhcv, vitri, ngaydang, sys_diachiweb)" +
                                    "values(@idtin, @danhmucid,@danhmucnghe,@doituongkh ,@daxacthuc,@duyetboi,@tieude, @lienhe, @diachi,@khuvuc,@quan,@thanhpho , @dienthoai, @noidung, @dienthoai_nd, @email_nd, @skye_nd, @mucluong, @mucluongtuden, @hoten, @gioitinh, @ngoaingu, @kinhnghiem, @namsinh, @bangcap, @loaihinhcv, @vitri, @ngaydang, @sys_diachiweb);" +
                                    "Select SCOPE_IDENTITY();";
                cmd.Parameters.AddWithValue("@idtin", record.idtin ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@danhmucid", record.danhmucid);
                cmd.Parameters.AddWithValue("@danhmucnghe", record.danhmucnghe ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@daxacthuc", record.daxacthuc ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@duyetboi", record.duyetboi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@doituongkh", record.doituongkh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@tieude", record.tieude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@lienhe", record.lienhe ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@diachi", record.diachi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@khuvuc", record.khuvuc ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@quan", record.quan ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@thanhpho", record.thanhpho ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dienthoai", record.dienthoai ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@noidung", record.noidung ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dienthoai_nd", record.dienthoai_nd ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@email_nd", record.email_nd ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@skye_nd", record.skye_nd ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@mucluong", record.mucluong ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@mucluongtuden", record.mucluongtuden ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@hoten", record.hoten ?? (object) DBNull.Value);
                cmd.Parameters.AddWithValue("@gioitinh", record.gioitinh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ngoaingu", record.ngoaingu ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@kinhnghiem", record.kinhnghiem ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@namsinh", record.namsinh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@bangcap", record.bangcap ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@loaihinhcv", record.loaihinhcv ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@vitri", record.vitri ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ngaydang", record.ngaydang ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@sys_diachiweb", record.sys_diachiweb);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value)
                    return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                writer.WriteToLog(string.Format("{0}-{1}-Mã tin:{2}-{3}", ex.Message, "AddMuaban", record.idtin, "error1"));
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }






        #endregion

        #region batdongsan
        public static bool Adddm_batdongsang(dm_batdongsan record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into dm_batdongsan(parentid, name, path, orderid)" +
                                    "values(@parentid, @name, @path, @orderid);" +
                                    "Select SCOPE_IDENTITY();";
                if (record.parenid == null)
                    cmd.Parameters.AddWithValue("@parentId", SqlInt32.Null);
                else
                    cmd.Parameters.AddWithValue("@parentId", record.parenid);
                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@path", record.path);
                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value) return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                writer.WriteToLog(string.Format("{0}-{1}-{2}", ex.Message, "Adddm_batdongsang", "error1"));
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool DelDM_batdongsan(dm_batdongsan recode)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                if (recode == null)
                    return false;

                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DELETE FROM dm_batdongsan WHERE ID ='" + recode.id + "'";

                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DelDM_batdongsan");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static bool Updatedm_batdongsan(dm_batdongsan record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null) return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "Update dm_batdongsan " +
                                    " Set   name=@name, " +
                                    "       path=@path," +
                                    "       orderid=@orderid," +
                                    "       parentId=@parentId" +
                                    " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                if (record.parenid == null)
                    cmd.Parameters.AddWithValue("@parentId", SqlInt32.Null);
                else
                    cmd.Parameters.AddWithValue("@parentId", record.parenid);
                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@path", record.path);
                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Updatedm_batdongsan");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }


        public static bool AddBatdongsan(batdongsan record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

        

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into batdongsan(danhmucid, tieude, khuvuc_canban,quan_canban,thanhpho_canban,quanhuyen_canmua,thanhpho_canmua, giaban, dientich, matindang, noidung, dienthoai_nd, email_nd, loaihinhtindang, ngaydang, ngayhethan, dd_bds_loaitinrao, dd_bds_diachi, dd_bds_huongbancong, dd_bds_sophongngu, dd_bds_sotoilet, dd_bds_noithat,dd_bds_huongnha,dd_bds_sotang,dd_bds_mattien,dd_bds_duongvao, lienhe_tenlienlac, lienhe_diachi, lienhe_dienthoai, lienhe_mobilde, lienhe_email, ttduan_tenduan, ttduan_chudautu, ttduan_quymo, moigioi_tieude, moigioi_diachi, moigioi_soban,moigioi_soban_bydidong, moigioi_didong,moigioi_didong_bydidong,moigioi_fax, moigioi_email, moigioi_website, moigioi_khuvucmoigioi,moigioi_masothue,sys_diachiweb)" +
                                    "values(@danhmucid, @tieude, @khuvuc_canban,@quan_canban,@thanhpho_canban,@quanhuyen_canmua,@thanhpho_canmua, @giaban, @dientich, @matindang, @noidung, @dienthoai_nd, @email_nd, @loaihinhtindang, @ngaydang, @ngayhethan, @dd_bds_loaitinrao, @dd_bds_diachi, @dd_bds_huongbancong, @dd_bds_sophongngu, @dd_bds_sotoilet, @dd_bds_noithat,@dd_bds_huongnha,@dd_bds_sotang,@dd_bds_mattien,@dd_bds_duongvao, @lienhe_tenlienlac, @lienhe_diachi, @lienhe_dienthoai, @lienhe_mobilde, @lienhe_email, @ttduan_tenduan, @ttduan_chudautu, @ttduan_quymo, @moigioi_tieude, @moigioi_diachi, @moigioi_soban,@moigioi_soban_bydidong, @moigioi_didong,@moigioi_didong_bydidong,@moigioi_fax, @moigioi_email, @moigioi_website, @moigioi_khuvucmoigioi,@moigioi_masothue,@sys_diachiweb);" +
                                    "Select SCOPE_IDENTITY();";
                cmd.Parameters.AddWithValue("@danhmucid", record.danhmucid);
                cmd.Parameters.AddWithValue("@tieude", record.tieude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@khuvuc_canban", record.khuvuc_canban ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@quan_canban", record.quan_canban ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@thanhpho_canban", record.thanhpho_canban ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@quanhuyen_canmua", record.quanhuyen_canmua ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@thanhpho_canmua", record.thanhpho_canmua ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@giaban", record.giaban ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dientich", record.dientich ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@matindang", record.matindang ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@noidung", record.noidung ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dienthoai_nd", record.dienthoai_nd ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@email_nd", record.email_nd ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@loaihinhtindang", record.loaihinhtindang ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ngaydang", record.ngaydang ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ngayhethan", record.ngayhethan ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dd_bds_loaitinrao", record.dd_bds_loaitinrao ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dd_bds_diachi", record.dd_bds_diachi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dd_bds_huongbancong", record.dd_bds_huongbancong ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dd_bds_sophongngu", record.dd_bds_sophongngu ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dd_bds_sotoilet", record.dd_bds_sotoilet ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dd_bds_noithat", record.dd_bds_noithat ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dd_bds_huongnha", record.dd_bds_huongnha ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dd_bds_sotang", record.dd_bds_sotang ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dd_bds_mattien", record.dd_bds_mattien ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dd_bds_duongvao", record.dd_bds_duongvao ?? (object)DBNull.Value);
                

                cmd.Parameters.AddWithValue("@lienhe_tenlienlac", record.lienhe_tenlienlac ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@lienhe_diachi", record.lienhe_diachi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@lienhe_dienthoai", record.lienhe_dienthoai ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@lienhe_mobilde", record.lienhe_mobilde ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@lienhe_email", record.lienhe_email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ttduan_tenduan", record.ttduan_tenduan ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ttduan_chudautu", record.ttduan_chudautu ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ttduan_quymo", record.ttduan_quymo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@moigioi_tieude", record.moigioi_tieude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@moigioi_diachi", record.moigioi_diachi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@moigioi_soban", record.moigioi_soban ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@moigioi_soban_bydidong", record.moigioi_soban_bydidong ?? (object)DBNull.Value);
                
                cmd.Parameters.AddWithValue("@moigioi_didong", record.moigioi_didong ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@moigioi_didong_bydidong", record.moigioi_didong_bydidong ?? (object)DBNull.Value);
                
                cmd.Parameters.AddWithValue("@moigioi_fax", record.moigioi_fax ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@moigioi_email", record.moigioi_email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@moigioi_website", record.moigioi_website ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@moigioi_khuvucmoigioi", record.moigioi_khuvucmoigioi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@moigioi_masothue", record.moigioi_masothue ?? (object)DBNull.Value);
                

                cmd.Parameters.AddWithValue("@sys_diachiweb", record.sys_diachiweb);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value)
                    return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                writer.WriteToLog(string.Format("{0}-{1}-Mã tin:{2}-{3}", ex.Message, "AddBatdongsan", record.matindang, "error1"));
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        #endregion

        #region vinabiz
        public static List<dm_Tinh> Loaddm_tinh(string sql)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            dm_Tinh InfoCOMMANDTABLE;
            List<dm_Tinh> InfoCOMMANDTABLEs = null;

            try
            {
                InfoCOMMANDTABLEs = new List<dm_Tinh>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = cnn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    InfoCOMMANDTABLE = new dm_Tinh();


                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.ma = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.ten = reader.GetString(2);
                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static List<dm_vinabiz> Loaddm_vinabiz(string sql)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            dm_vinabiz InfoCOMMANDTABLE;
            List<dm_vinabiz> InfoCOMMANDTABLEs = null;

            try
            {
                InfoCOMMANDTABLEs = new List<dm_vinabiz>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = cnn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    InfoCOMMANDTABLE = new dm_vinabiz();


                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.name = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.paren_id = reader.GetInt32(2);
                    if (!reader.IsDBNull(3))
                        InfoCOMMANDTABLE.path = reader.GetString(3);
                   
                    if (!reader.IsDBNull(4))
                        InfoCOMMANDTABLE.orderid = reader.GetInt32(4);
                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool AdddmVinabiz(dm_vinabiz record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into dm_vinabiz( name, parentId, path, orderid)" +
                                    "values( @name, @parentId, @path, @orderid);" +
                                    "Select SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@name", record.name);
                if (record.paren_id == null)
                    cmd.Parameters.AddWithValue("@parentId", SqlInt32.Null);
                else
                    cmd.Parameters.AddWithValue("@parentId", record.paren_id);
                cmd.Parameters.AddWithValue("@path", record.path);
                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value) return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool Updatedm_vinabiz(dm_vinabiz record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null) return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "Update [dm_vinabiz] Set name=@name, "
                                    + " path=@path,parentId=@parentId,orderid=@orderid "
                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@path", record.path);
                if (record.paren_id == null)
                    cmd.Parameters.AddWithValue("@parentId", SqlInt32.Null);
                else
                    cmd.Parameters.AddWithValue("@parentId", record.paren_id);
                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Updatedm_vinabiz");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static bool Deldm_vinabiz(dm_vinabiz recode)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                if (recode == null)
                    return false;

                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DELETE FROM dm_vinabiz WHERE ID ='" + recode.id + "'";

                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Deldm_vinabiz");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static bool AddVinabiz(vinabiz record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into vinabiz(danhmucid,danhmucbyVnbiz, ttdk_msthue, ttdk_tenchinhthuc, ttdk_coquanthuequanly,       ttdk_tengiaodich, ttdk_ngaycap, ttdk_ngaybatdauhoatdong, ttdk_trangthai, ttlh_diachitruso,ttlh_tinhid,       ttlh_tinh, ttlh_huyen, ttlh_xa, ttlh_dienthoai1, ttlh_dienthoaididong1,ttlh_dienthoaididong2,ttlh_dienthoaididong3, ttlh_dienthoai_nguoidaidien,ttlh_dienthoai_nguoidaidien_didong, ttlh_email, ttlh_nguoidaidien, ttlh_diachinguoidaidien, ttlh_giamdoc,ttlh_dienthoaigiamdoc,           ttlh_dienthoaigiamdoc_didong, ttlh_diachigiamdoc, ttlh_ketoan,ttlh_dienthoaiketoan,ttlh_dienthoaiketoan_didong, ttlh_diachiketoan, ttlh_fax, ttlh_website, ds_nganhnghekinhdoanh,nganhnghechinh2, ds_thuephainop, lvhd_loaihinhkinhte, lvhd_linhvuckinhte, lvhd_loaihinhtochuc, lvhd_capchuong, lvhd_loaikhoan, web_nguon_url)" +
                                    "values          ( @danhmucid,@danhmucbyVnbiz, @ttdk_msthue, @ttdk_tenchinhthuc, @ttdk_coquanthuequanly, @ttdk_tengiaodich, @ttdk_ngaycap, @ttdk_ngaybatdauhoatdong, @ttdk_trangthai, @ttlh_diachitruso,@ttlh_tinhid, @ttlh_tinh, @ttlh_huyen, @ttlh_xa, @ttlh_dienthoai1,@ttlh_dienthoaididong1,@ttlh_dienthoaididong2,@ttlh_dienthoaididong3, @ttlh_dienthoai_nguoidaidien,@ttlh_dienthoai_nguoidaidien_didong, @ttlh_email, @ttlh_nguoidaidien, @ttlh_diachinguoidaidien, @ttlh_giamdoc,@ttlh_dienthoaigiamdoc,@ttlh_dienthoaigiamdoc_didong, @ttlh_diachigiamdoc, @ttlh_ketoan,@ttlh_dienthoaiketoan,@ttlh_dienthoaiketoan_didong, @ttlh_diachiketoan, @ttlh_fax, @ttlh_website, @ds_nganhnghekinhdoanh,@nganhnghechinh2, @ds_thuephainop,@lvhd_loaihinhkinhte, @lvhd_linhvuckinhte, @lvhd_loaihinhtochuc, @lvhd_capchuong, @lvhd_loaikhoan, @web_nguon_url);" +
                                    "Select SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@danhmucid", record.danhmucid);
                cmd.Parameters.AddWithValue("@danhmucbyVnbiz", record.danhmucbyVnbiz ?? "");
                cmd.Parameters.AddWithValue("@ttdk_msthue", record.ttdk_msthue ?? "");
                cmd.Parameters.AddWithValue("@ttdk_tenchinhthuc", record.ttdk_tenchinhthuc ?? "");
                cmd.Parameters.AddWithValue("@ttdk_coquanthuequanly", record.ttdk_coquanthuequanly ?? "");
                cmd.Parameters.AddWithValue("@ttdk_tengiaodich", record.ttdk_tengiaodich ?? "");
                cmd.Parameters.AddWithValue("@ttdk_ngaycap", record.ttdk_ngaycap ?? "");
                cmd.Parameters.AddWithValue("@ttdk_ngaybatdauhoatdong", record.ttdk_ngaybatdauhoatdong ?? "");
                cmd.Parameters.AddWithValue("@ttdk_trangthai", record.ttdk_trangthai ?? "");
                cmd.Parameters.AddWithValue("@ttlh_diachitruso", record.ttlh_diachitruso ?? "");
                cmd.Parameters.AddWithValue("@ttlh_tinhid", record.ttlh_tinhid);
                cmd.Parameters.AddWithValue("@ttlh_tinh", record.ttlh_tinh ?? "");
                cmd.Parameters.AddWithValue("@ttlh_huyen", record.ttlh_huyen ?? "");
                cmd.Parameters.AddWithValue("@ttlh_xa", record.ttlh_xa ?? "");

                cmd.Parameters.AddWithValue("@ttlh_dienthoai1", record.ttlh_dienthoai1 ?? "");
                cmd.Parameters.AddWithValue("@ttlh_dienthoaididong1", record.ttlh_dienthoaididong1 ?? "");
                cmd.Parameters.AddWithValue("@ttlh_dienthoaididong2", record.ttlh_dienthoaididong2 ?? "");
                cmd.Parameters.AddWithValue("@ttlh_dienthoaididong3", record.ttlh_dienthoaididong3 ?? "");

                cmd.Parameters.AddWithValue("@ttlh_dienthoai_nguoidaidien", record.ttlh_dienthoai_nguoidaidien ?? "");
                cmd.Parameters.AddWithValue("@ttlh_dienthoai_nguoidaidien_didong", record.ttlh_dienthoai_nguoidaidien_didong ?? "");

                
                cmd.Parameters.AddWithValue("@ttlh_email", record.ttlh_email ?? "");
                cmd.Parameters.AddWithValue("@ttlh_nguoidaidien", record.ttlh_nguoidaidien ?? "");
                cmd.Parameters.AddWithValue("@ttlh_diachinguoidaidien", record.ttlh_diachinguoidaidien ?? "");
                cmd.Parameters.AddWithValue("@ttlh_giamdoc", record.ttlh_giamdoc ?? "");

                cmd.Parameters.AddWithValue("@ttlh_dienthoaigiamdoc", record.ttlh_dienthoaigiamdoc ?? "");
                cmd.Parameters.AddWithValue("@ttlh_dienthoaigiamdoc_didong", record.ttlh_dienthoaigiamdoc_didong ?? "");
                
                cmd.Parameters.AddWithValue("@ttlh_diachigiamdoc", record.ttlh_diachigiamdoc ?? "");
                cmd.Parameters.AddWithValue("@ttlh_ketoan", record.ttlh_ketoan ?? "");

                cmd.Parameters.AddWithValue("@ttlh_dienthoaiketoan", record.ttlh_dienthoaiketoan ?? "");
                cmd.Parameters.AddWithValue("@ttlh_dienthoaiketoan_didong", record.ttlh_dienthoaiketoan_didong ?? "");
                

                cmd.Parameters.AddWithValue("@ttlh_diachiketoan", record.ttlh_diachiketoan ?? "");
                cmd.Parameters.AddWithValue("@ttlh_fax", record.ttlh_fax ?? "");
                cmd.Parameters.AddWithValue("@ttlh_website", record.ttlh_website ?? "");
                cmd.Parameters.AddWithValue("@ds_nganhnghekinhdoanh", record.ds_nganhnghekinhdoanh ?? "");
                cmd.Parameters.AddWithValue("@nganhnghechinh2", record.nganhnghechinh2 ?? "");
                cmd.Parameters.AddWithValue("@ds_thuephainop", record.ds_thuephainop ?? "");
                cmd.Parameters.AddWithValue("@lvhd_loaihinhkinhte", record.lvhd_loaihinhkinhte ?? "");
                cmd.Parameters.AddWithValue("@lvhd_linhvuckinhte", record.lvhd_linhvuckinhte ?? "");
                cmd.Parameters.AddWithValue("@lvhd_loaihinhtochuc", record.lvhd_loaihinhtochuc ?? "");
                cmd.Parameters.AddWithValue("@lvhd_capchuong", record.lvhd_capchuong ?? "");
                cmd.Parameters.AddWithValue("@lvhd_loaikhoan", record.lvhd_loaikhoan ?? "");
                cmd.Parameters.AddWithValue("@web_nguon_url", record.web_nguon_url ?? (object)DBNull.Value);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value) return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                writer.WriteToLog(string.Format("{0}-{1}-diachiwebsite:{2}-{3}", ex.Message, "AddVinabiz", record.ttdk_msthue, "error1"));
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool UpdateChuanHoaDanhSachNganhNghe(vinabiz record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null) return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "Update [vinabiz] Set ds_nganhnghekinhdoanhId=@ds_nganhnghekinhdoanhId "
                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@ds_nganhnghekinhdoanhId", record.ds_nganhnghekinhdoanhid);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdateChuanHoa");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
        public static bool UpdateChuanHoaNganhNgheChinh(vinabiz record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null) return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = " Update [vinabiz] Set danhmucid=@danhmucid ,danhmucbyVnbiz =@danhmucbyVnbiz "
                                + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@danhmucid", record.danhmucid);
                cmd.Parameters.AddWithValue("@danhmucbyVnbiz", record.danhmucbyVnbiz);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdateChuanHoa");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static List<dm_vinabiz_map> Loaddm_vinabiz_map(string sql)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            dm_vinabiz_map InfoCOMMANDTABLE;
            List<dm_vinabiz_map> InfoCOMMANDTABLEs = null;

            try
            {
                InfoCOMMANDTABLEs = new List<dm_vinabiz_map>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = cnn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    InfoCOMMANDTABLE = new dm_vinabiz_map();



                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.danhmucbyVnbiz = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.dmhosocongty = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        InfoCOMMANDTABLE.hosocongtyid = reader.GetInt32(3);

                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static List<vinabiz> Loadvinabiz(string strSQL)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            vinabiz InfoCOMMANDTABLE;
            List<vinabiz> InfoCOMMANDTABLEs = null;

            try
            {
                InfoCOMMANDTABLEs = new List<vinabiz>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = strSQL;
                cmd.Connection = cnn;
                cmd.CommandTimeout = 9999;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    InfoCOMMANDTABLE = new vinabiz();

                    
                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.danhmucid = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.danhmucbyVnbiz = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        InfoCOMMANDTABLE.nganhnghechinh2 = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        InfoCOMMANDTABLE.ttdk_msthue = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        InfoCOMMANDTABLE.ttdk_tenchinhthuc = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        InfoCOMMANDTABLE.ttdk_coquanthuequanly = reader.GetString(6);
                    if (!reader.IsDBNull(7))
                        InfoCOMMANDTABLE.ttdk_tengiaodich = reader.GetString(7);
                    if (!reader.IsDBNull(8))
                        InfoCOMMANDTABLE.ttdk_ngaycap = reader.GetString(8);
                    if (!reader.IsDBNull(9))
                        InfoCOMMANDTABLE.ttdk_ngaybatdauhoatdong = reader.GetString(9);
                    if (!reader.IsDBNull(10))
                        InfoCOMMANDTABLE.ttdk_trangthai = reader.GetString(10);
                    if (!reader.IsDBNull(11))
                        InfoCOMMANDTABLE.ttlh_diachitruso = reader.GetString(11);
                    if (!reader.IsDBNull(12))
                        InfoCOMMANDTABLE.ttlh_tinhid = reader.GetInt32(12);
                    if (!reader.IsDBNull(13))
                        InfoCOMMANDTABLE.ttlh_tinh = reader.GetString(13);
                    if (!reader.IsDBNull(14))
                        InfoCOMMANDTABLE.ttlh_huyen = reader.GetString(14);
                    if (!reader.IsDBNull(15))
                        InfoCOMMANDTABLE.ttlh_xa = reader.GetString(15);
                    if (!reader.IsDBNull(16))
                        InfoCOMMANDTABLE.ttlh_dienthoai1 = reader.GetString(16);
                    if (!reader.IsDBNull(17))
                        InfoCOMMANDTABLE.ttlh_dienthoaididong1 = reader.GetString(17);
                    if (!reader.IsDBNull(18))
                        InfoCOMMANDTABLE.ttlh_dienthoaididong2 = reader.GetString(18);
                    if (!reader.IsDBNull(19))
                        InfoCOMMANDTABLE.ttlh_dienthoaididong3 = reader.GetString(19);
                    if (!reader.IsDBNull(20))
                        InfoCOMMANDTABLE.ttlh_dienthoai_nguoidaidien = reader.GetString(20);
                    if (!reader.IsDBNull(21))
                        InfoCOMMANDTABLE.ttlh_dienthoai_nguoidaidien_didong = reader.GetString(21);
                   if (!reader.IsDBNull(22))
                        InfoCOMMANDTABLE.ttlh_email = reader.GetString(22);
                    if (!reader.IsDBNull(23))
                        InfoCOMMANDTABLE.ttlh_nguoidaidien = reader.GetString(23);
                    if (!reader.IsDBNull(24))
                        InfoCOMMANDTABLE.ttlh_diachinguoidaidien = reader.GetString(24);
                    if (!reader.IsDBNull(25))
                        InfoCOMMANDTABLE.ttlh_giamdoc = reader.GetString(25);
                    if (!reader.IsDBNull(26))
                        InfoCOMMANDTABLE.ttlh_dienthoaigiamdoc = reader.GetString(26);
                    if (!reader.IsDBNull(27))
                        InfoCOMMANDTABLE.ttlh_dienthoaigiamdoc_didong = reader.GetString(27);
                    if (!reader.IsDBNull(28))
                        InfoCOMMANDTABLE.ttlh_diachigiamdoc = reader.GetString(28);
                    if (!reader.IsDBNull(29))
                        InfoCOMMANDTABLE.ttlh_ketoan = reader.GetString(29);
                    if (!reader.IsDBNull(30))
                        InfoCOMMANDTABLE.ttlh_dienthoaiketoan = reader.GetString(30);
                    if (!reader.IsDBNull(31))
                        InfoCOMMANDTABLE.ttlh_dienthoaiketoan_didong = reader.GetString(31);
                    if (!reader.IsDBNull(32))
                        InfoCOMMANDTABLE.ttlh_diachiketoan = reader.GetString(32);
                    if (!reader.IsDBNull(33))
                        InfoCOMMANDTABLE.ttlh_fax = reader.GetString(33);
                    if (!reader.IsDBNull(34))
                        InfoCOMMANDTABLE.ttlh_website = reader.GetString(34);
                    if (!reader.IsDBNull(35))
                        InfoCOMMANDTABLE.lvhd_loaihinhkinhte = reader.GetString(35);
                    if (!reader.IsDBNull(36))
                        InfoCOMMANDTABLE.lvhd_linhvuckinhte = reader.GetString(36);
                    if (!reader.IsDBNull(37))
                        InfoCOMMANDTABLE.lvhd_loaihinhtochuc = reader.GetString(37);
                    if (!reader.IsDBNull(38))
                        InfoCOMMANDTABLE.lvhd_capchuong = reader.GetString(38);
                    if (!reader.IsDBNull(39))
                        InfoCOMMANDTABLE.lvhd_loaikhoan = reader.GetString(39);
                    if (!reader.IsDBNull(40))
                        InfoCOMMANDTABLE.ds_nganhnghekinhdoanh = reader.GetString(40);
                    if (!reader.IsDBNull(41))
                        InfoCOMMANDTABLE.ds_nganhnghekinhdoanhid = reader.GetString(41);
                    if (!reader.IsDBNull(42))
                        InfoCOMMANDTABLE.ds_thuephainop = reader.GetString(42);
                    if (!reader.IsDBNull(43))
                        InfoCOMMANDTABLE.web_nguon_url = reader.GetString(43);
                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool UpdateChuanHoa_ttlh_dienthoai(vinabiz record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null) return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "Update [vinabiz] Set ttlh_dienthoaididong1=@ttlh_dienthoaididong1,ttlh_dienthoaididong2=@ttlh_dienthoaididong2,ttlh_dienthoaididong3=@ttlh_dienthoaididong3 "

                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                
                cmd.Parameters.AddWithValue("@ttlh_dienthoaididong1", record.ttlh_dienthoaididong1 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ttlh_dienthoaididong2", record.ttlh_dienthoaididong2 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ttlh_dienthoaididong3", record.ttlh_dienthoaididong3 ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdateChuanHoa");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
        public static bool UpdateChuanHoa_ttlh_dienthoaigiamdoc(vinabiz record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null) return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "Update [vinabiz] Set ttlh_dienthoaigiamdoc_didong=@ttlh_dienthoaigiamdoc_didong "

                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@ttlh_dienthoaigiamdoc_didong", record.ttlh_dienthoaigiamdoc_didong ?? (object)DBNull.Value);


                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdateChuanHoa");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
        public static bool UpdateChuanHoa_ttlh_dienthoai_nguoidaidien_didong(vinabiz record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null) return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "Update [vinabiz] Set ttlh_dienthoai_nguoidaidien_didong=@ttlh_dienthoai_nguoidaidien_didong "

                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@ttlh_dienthoai_nguoidaidien_didong", record.ttlh_dienthoai_nguoidaidien_didong ?? (object)DBNull.Value);


                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdateChuanHoa");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
        public static bool UpdateChuanHoa_ttlh_dienthoaiketoan(vinabiz record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null) return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "Update [vinabiz] Set ttlh_dienthoaiketoan_didong=@ttlh_dienthoaiketoan_didong "

                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@ttlh_dienthoaiketoan_didong", record.ttlh_dienthoaiketoan_didong ?? (object)DBNull.Value);


                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdateChuanHoa");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        #endregion

        #region Thị trường sỉ
        public static bool Deldm_thitruongsi(dm_thitruongsi recode)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                if (recode == null)
                    return false;

                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DELETE FROM dm_thitruongsi WHERE ID ='" + recode.id + "'";

                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Deldm_thitruongsi");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static bool AdddmThiTruongSi(dm_thitruongsi record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into dm_thitruongsi( name, parentId, path, orderid)" +
                                    "values( @name, @parentId, @path, @orderid);" +
                                    "Select SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@name", record.name);
                if (record.paren_id == null)
                    cmd.Parameters.AddWithValue("@parentId", SqlInt32.Null);
                else
                    cmd.Parameters.AddWithValue("@parentId", record.paren_id);
                cmd.Parameters.AddWithValue("@path", record.path);
                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value) return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool Updatedm_thitruongsi(dm_thitruongsi record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null) return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "Update [dm_thitruongsi] Set name=@name, "
                                    + " path=@path,parentId=@parentId,orderid=@orderid "
                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@path", record.path);
                if (record.paren_id == null)
                    cmd.Parameters.AddWithValue("@parentId", SqlInt32.Null);
                else
                    cmd.Parameters.AddWithValue("@parentId", record.paren_id);

                cmd.Parameters.AddWithValue("@orderid", record.orderid);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Updatedm_thitruongsi");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static bool Addthitruongsi(thitruongsi record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
        
                cmd.CommandText = "Insert into thitruongsi( danhmucid, tieude, gia_tu_den, toithieu, loaidoanhnghiep, mohinhkinhdoanh, tendoanhnghiep, thoigianhoatdong, ttdn_diachi, ttdn_msthue, ttncc_diachi, ttncc_sdt,            ttncc_sdt_didong, ttncc_email, daxacthuc_cogiayphepkinhdoanh, daxacthuc_cokhohang, listdanhmuc,  daidien_hoten, daidien_dienthoai, daidien_email, lienhe_kinhdoanh_hoten1,           lienhe_kinhdoanh_sodienthoai1, lienhe_kinhdoanh_sodienthoaibydidong1,  lienhe_kinhdoanh_hoten2,     lienhe_kinhdoanh_sodienthoai2, lienhe_kinhdoanh_sodienthoaibydidong2, quymosx_thitruong, quymosx_nganhhang, quymosx_nhansu,    quymosx_sanluong, khohang_diachi, cuahang_diachi, hinhanhxuong_diachi, sys_diachiweb)" +
                                    "values(                @danhmucid, @tieude, @gia_tu_den, @toithieu, @loaidoanhnghiep, @mohinhkinhdoanh, @tendoanhnghiep, @thoigianhoatdong, @ttdn_diachi, @ttdn_msthue, @ttncc_diachi, @ttncc_sdt,@ttncc_sdt_didong, @ttncc_email, @daxacthuc_cogiayphepkinhdoanh, @daxacthuc_cokhohang, @listdanhmuc,  @daidien_hoten, @daidien_dienthoai, @daidien_email, @lienhe_kinhdoanh_hoten1, @lienhe_kinhdoanh_sodienthoai1,@lienhe_kinhdoanh_sodienthoaibydidong1, @lienhe_kinhdoanh_hoten2,    @lienhe_kinhdoanh_sodienthoai2,@lienhe_kinhdoanh_sodienthoaibydidong2, @quymosx_thitruong, @quymosx_nganhhang, @quymosx_nhansu, @quymosx_sanluong, @khohang_diachi, @cuahang_diachi, @hinhanhxuong_diachi,  @sys_diachiweb);" +
                                    "Select SCOPE_IDENTITY();";
                                                            
                cmd.Parameters.AddWithValue("@danhmucid", record.danhmucid);
                cmd.Parameters.AddWithValue("@tieude", record.tieude ?? "");
                cmd.Parameters.AddWithValue("@gia_tu_den", record.gia_tu_den ?? "");
                cmd.Parameters.AddWithValue("@toithieu", record.toithieu ?? "");
                cmd.Parameters.AddWithValue("@loaidoanhnghiep", record.loaidoanhnghiep ?? "");
                cmd.Parameters.AddWithValue("@mohinhkinhdoanh", record.mohinhkinhdoanh ?? "");
                cmd.Parameters.AddWithValue("@tendoanhnghiep", record.tendoanhnghiep ?? "");
                cmd.Parameters.AddWithValue("@thoigianhoatdong", record.thoigianhoatdong ?? "");
                cmd.Parameters.AddWithValue("@ttdn_diachi", record.ttdn_diachi ?? "");
                cmd.Parameters.AddWithValue("@ttdn_msthue", record.ttdn_msthue ?? "");
                cmd.Parameters.AddWithValue("@ttncc_diachi", record.ttncc_diachi ?? "");
                cmd.Parameters.AddWithValue("@ttncc_sdt", record.ttncc_sdt ?? "");
                cmd.Parameters.AddWithValue("@ttncc_sdt_didong", record.ttncc_sdt_didong ?? "");
                cmd.Parameters.AddWithValue("@ttncc_email", record.ttncc_email ?? "");
                cmd.Parameters.AddWithValue("@daxacthuc_cogiayphepkinhdoanh", record.daxacthuc_cogiayphepkinhdoanh ?? "");
                cmd.Parameters.AddWithValue("@daxacthuc_cokhohang", record.daxacthuc_cokhohang ?? "");
                cmd.Parameters.AddWithValue("@listdanhmuc", record.listdanhmuc ?? "");
                cmd.Parameters.AddWithValue("@daidien_hoten", record.daidien_hoten ?? "");
                cmd.Parameters.AddWithValue("@daidien_dienthoai", record.daidien_dienthoai ?? "");
                cmd.Parameters.AddWithValue("@daidien_email", record.daidien_email ?? "");
                cmd.Parameters.AddWithValue("@lienhe_kinhdoanh_hoten1", record.lienhe_kinhdoanh_hoten1 ?? "");
                cmd.Parameters.AddWithValue("@lienhe_kinhdoanh_sodienthoai1", record.lienhe_kinhdoanh_sodienthoai1 ?? "");
                cmd.Parameters.AddWithValue("@lienhe_kinhdoanh_sodienthoaibydidong1", record.lienhe_kinhdoanh_sodienthoaibydidong1 ?? "");
                cmd.Parameters.AddWithValue("@lienhe_kinhdoanh_hoten2", record.lienhe_kinhdoanh_hoten2 ?? "");
                cmd.Parameters.AddWithValue("@lienhe_kinhdoanh_sodienthoai2", record.lienhe_kinhdoanh_sodienthoai2 ?? "");
                cmd.Parameters.AddWithValue("@lienhe_kinhdoanh_sodienthoaibydidong2", record.lienhe_kinhdoanh_sodienthoaibydidong2 ?? "");
                cmd.Parameters.AddWithValue("@quymosx_thitruong", record.quymosx_thitruong ?? "");
                cmd.Parameters.AddWithValue("@quymosx_nganhhang", record.quymosx_nganhhang ?? "");
                cmd.Parameters.AddWithValue("@quymosx_nhansu", record.quymosx_nhansu ?? "");
                cmd.Parameters.AddWithValue("@quymosx_sanluong", record.quymosx_sanluong ?? "");
                cmd.Parameters.AddWithValue("@khohang_diachi", record.khohang_diachi ?? "");
                cmd.Parameters.AddWithValue("@cuahang_diachi", record.cuahang_diachi ?? "");
                cmd.Parameters.AddWithValue("@hinhanhxuong_diachi", record.hinhanhxuong_diachi ?? "");
                cmd.Parameters.AddWithValue("@sys_diachiweb", record.sys_diachiweb ?? "");
                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value) return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                writer.WriteToLog(string.Format("{0}-{1}-diachiwebsite:{2}-{3}", ex.Message, "Addthitruongsi", record.sys_diachiweb, "error1"));
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        #endregion

        #region Cấu Hình
        public static List<CauHinh> LoadCauHinh(string sql)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            CauHinh InfoCOMMANDTABLE;
            List<CauHinh> InfoCOMMANDTABLEs = null;

            try
            {
                InfoCOMMANDTABLEs = new List<CauHinh>();

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
                cnn.FireInfoMessageEventOnUserErrors = false;

                cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = cnn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    InfoCOMMANDTABLE = new CauHinh();


                    if (!reader.IsDBNull(0))
                        InfoCOMMANDTABLE.id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        InfoCOMMANDTABLE.name = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        InfoCOMMANDTABLE.username = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        InfoCOMMANDTABLE.password = reader.GetString(3);
                    InfoCOMMANDTABLEs.Add(InfoCOMMANDTABLE);
                }
                return InfoCOMMANDTABLEs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool AddCauHinh(CauHinh record)
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            object objectID;
            try
            {
                if (record == null)
                    return false;

                cnn = new SqlConnection();
                cnn.ConnectionString = ConnectionString;
                cnn.FireInfoMessageEventOnUserErrors = false;
                cnn.Open();

                cmd = new SqlCommand();
                cmd.Connection = cnn;
                //--- Insert Record
                cmd.CommandText = "Insert into CauHinh( name, username, password)" +
                                    "values( @name, @username, @password);" +
                                    "Select SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@username", record.username);
                cmd.Parameters.AddWithValue("@password", record.password);

                objectID = cmd.ExecuteScalar();

                if (objectID == null || objectID == DBNull.Value) return false;

                record.id = Convert.ToInt32(objectID);

                return true;
            }
            catch (Exception ex)
            {
                writer = LogWriter.Instance;
                return false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

        public static bool UpdateCauHinh(CauHinh record)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;

            try
            {
                if (record == null) return false;

                // Make connection to database
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                // Create command to update GeneralGuessGroup record
                cmd = new SqlCommand();
                cmd.Connection = connection;
                //id, name, username, password
                cmd.CommandText = "Update [CauHinh] Set name=@name, "
                                    + " username=@username,password=@password "
                                    + " where ID='" + record.id + "'";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@name", record.name);
                cmd.Parameters.AddWithValue("@username", record.username);
                cmd.Parameters.AddWithValue("@password", record.password);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdatedmVatGia");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        #endregion


        #region Execute SQL

        public static bool ExcNonQuery(string sqlcommand)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.Open();
                command = new SqlCommand(sqlcommand, connection);
                command.CommandTimeout = 36000;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ExcNonQuery");
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static object ExcScalar(string sqlcommand)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            object result = null;

            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.Open();
                command = new SqlCommand(sqlcommand, connection);
                command.CommandTimeout = 36000;
                result = command.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ExcScalar");
                return null;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static DataTable ExcDataTable(string sqlcommand)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataAdapter adp = null;
            DataTable table = null;

            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.Open();
                command = new SqlCommand(sqlcommand, connection);
                command.CommandTimeout = 36000;
                table = new DataTable();
                adp = new SqlDataAdapter(command);
                adp.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "ExcDataTable");
                return null;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static bool CheckExist(string sqlcommand)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = ConnectionString;
                connection.FireInfoMessageEventOnUserErrors = false;
                connection.Open();
                command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sqlcommand;
                command.CommandType = CommandType.Text;
                reader = command.ExecuteReader();
                if (reader.Read())
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CheckExist");
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        #endregion  // Execute SQL

        #region Execute OleDB

        public static DataTable ExcOleDbSchemaTable(string connectionString)
        {
            OleDbConnection oleConnect = null;
            DataTable table = null;

            try
            {
                oleConnect = new OleDbConnection();
                oleConnect.ConnectionString = connectionString;
                oleConnect.Open();
                table = oleConnect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ExcOleDbSchemaTable");
                return null;
            }

        }

        public static DataTable ExcOleDbSchemaColumn(string connectionString, string tableName)
        {
            OleDbConnection oleConnect = null;
            DataTable table = null;

            try
            {
                oleConnect = new OleDbConnection();
                oleConnect.ConnectionString = connectionString;
                oleConnect.Open();
                table = oleConnect.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });
                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ExcOleDbSchemaColumn");
                return null;
            }
        }

        public static OleDbDataReader ExcOleReaderDataSource(string connectionString, string tableName, string[] columnNames)
        {
            OleDbConnection oleConnect = null;
            OleDbCommand oleCommand = null;
            OleDbDataReader oleReader = null;
            string sqlcommand = "Select ";
            string[] getType;

            try
            {
                getType = connectionString.ToString().Split('.');
                //----- Get string command
                switch (getType[getType.Length - 1])
                {
                    case "mdb":
                    case "MDB":
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            if (i == columnNames.Length - 1)
                                sqlcommand += "[" + columnNames[i] + "] ";
                            else
                                sqlcommand += "[" + columnNames[i] + "],";
                        }
                        sqlcommand += "FROM [" + tableName + "]";
                        break;
                    case "dbf":
                    case "DBF":
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            if (i == columnNames.Length - 1)
                                sqlcommand += columnNames[i] + " ";
                            else
                                sqlcommand += columnNames[i] + ",";
                        }
                        sqlcommand += "FROM [" + tableName + "] Order by " + columnNames[0];
                        break;
                    default:
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            if (i == columnNames.Length - 1)
                                sqlcommand += "[" + columnNames[i] + "] ";
                            else
                                sqlcommand += "[" + columnNames[i] + "],";
                        }
                        sqlcommand += "FROM [" + tableName + "$]";
                        break;
                }

                oleConnect = new OleDbConnection(connectionString);
                oleConnect.Open();
                oleCommand = new OleDbCommand(sqlcommand, oleConnect);
                oleReader = oleCommand.ExecuteReader();
                return oleReader;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ExcOleReaderDataSource");
                return null;
            }
        }

        public static OleDbDataReader ExcOleReaderDataSource(string connectionString, string tableName, string[] columnNames, string stringWhere)
        {
            OleDbConnection oleConnect = null;
            OleDbCommand oleCommand = null;
            OleDbDataReader oleReader = null;
            string sqlcommand = "Select ";
            string[] getType;

            try
            {
                getType = connectionString.ToString().Split('.');
                //----- Get string command
                switch (getType[getType.Length - 1])
                {
                    case "mdb":
                    case "MDB":
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            if (i == columnNames.Length - 1)
                                sqlcommand += "[" + columnNames[i] + "] ";
                            else
                                sqlcommand += "[" + columnNames[i] + "],";
                        }
                        sqlcommand += "FROM [" + tableName + "] Where " + stringWhere;
                        break;
                    case "dbf":
                    case "DBF":
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            if (i == columnNames.Length - 1)
                                sqlcommand += columnNames[i] + " ";
                            else
                                sqlcommand += columnNames[i] + ",";
                        }
                        sqlcommand += "FROM [" + tableName + "] Where " + stringWhere + " Order by " + columnNames[0];
                        break;
                    default:
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            if (i == columnNames.Length - 1)
                                sqlcommand += "[" + columnNames[i] + "] ";
                            else
                                sqlcommand += "[" + columnNames[i] + "],";
                        }
                        sqlcommand += "FROM [" + tableName + "$] Where " + stringWhere;
                        break;
                }

                oleConnect = new OleDbConnection(connectionString);
                oleConnect.Open();
                oleCommand = new OleDbCommand(sqlcommand, oleConnect);
                oleReader = oleCommand.ExecuteReader();
                return oleReader;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ExcOleReaderDataSource");
                return null;
            }
        }

        public static OleDbDataReader ExcOleReaderDataSource(string connectionString, string tableName, string[] columnNames, int topRow)
        {
            OleDbConnection oleConnect = null;
            OleDbCommand oleCommand = null;
            OleDbDataReader oleReader = null;
            string sqlcommand = "Select ";
            string[] getType;

            try
            {
                getType = connectionString.ToString().Split('.');
                sqlcommand += "Top " + topRow + " ";
                //----- Get string command
                switch (getType[getType.Length - 1])
                {
                    case "mdb":
                    case "MDB":
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            if (i == columnNames.Length - 1)
                                sqlcommand += "[" + columnNames[i] + "] ";
                            else
                                sqlcommand += "[" + columnNames[i] + "],";
                        }
                        sqlcommand += "FROM [" + tableName + "]";
                        break;
                    case "dbf":
                    case "DBF":
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            if (i == columnNames.Length - 1)
                                sqlcommand += columnNames[i] + " ";
                            else
                                sqlcommand += columnNames[i] + ",";
                        }
                        sqlcommand += "FROM [" + tableName + "] Order by " + columnNames[0] + " desc";
                        break;
                    default:
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            if (i == columnNames.Length - 1)
                                sqlcommand += "[" + columnNames[i] + "] ";
                            else
                                sqlcommand += "[" + columnNames[i] + "],";
                        }
                        sqlcommand += "FROM [" + tableName + "$]";
                        break;
                }

                oleConnect = new OleDbConnection(connectionString);
                oleConnect.Open();
                oleCommand = new OleDbCommand(sqlcommand, oleConnect);
                oleReader = oleCommand.ExecuteReader();
                return oleReader;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ExcOleReaderDataSource");
                return null;
            }
        }

        public static object ExcOleScalar(string connectionString, string sqlcommand)
        {
            OleDbConnection oleConnect = null;
            OleDbCommand oleCommand = null;
            object result;

            try
            {
                oleConnect = new OleDbConnection(connectionString);
                oleConnect.Open();
                oleCommand = new OleDbCommand(sqlcommand, oleConnect);
                result = oleCommand.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ExcOleScalar");
                return null;
            }
        }

        #endregion  // Execute OleDB
    }
}
