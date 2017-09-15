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
    public partial class frmthemLinkVinabix : Form
    {
        public frmthemLinkVinabix()
        {
            InitializeComponent();
        }
        public string fromparent;
        public string Fromparent
        {
            get { return fromparent; }
            set { fromparent = value; }
        }

        private int pathlimit;
        public int PathLimit{
            get { return pathlimit; }
            set { pathlimit = value; }
        }

        private int danhmucdid;
        public int DanhmucId
        {
            get { return danhmucdid; }
            set { danhmucdid = value; }
        }
        private string danhmucName;
        public string DanhmucName
        {
            get { return danhmucName; }
            set { danhmucName = value; }
        }


        private string strTen;
        public string Ten
        {
            get { return strTen; }
            set { strTen = value; }
        }

        private string strLink;
        public string Link
        {
            get { return strLink; }
            set { strLink = value; }
        }

        private int strTrang;
        public int Trang
        {
            get { return strTrang; }
            set { strTrang = value; }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            #region Kiễm tra điều khiện
            if (txtvnbiz_ten.Text == "")
            {
                MessageBox.Show("Vui lòng đặt tên cho mục quét", "Thông Báo");
                txtvnbiz_ten.Focus();
                return;
            }
            if (txt_vnbiz_link.Text == "")
            {
                MessageBox.Show("Vui lòng nhập địa chỉ link cần quét", "Thông Báo");
                txt_vnbiz_link.Focus();
                return;
            }
            if (!checkBox1.Checked) {
                if (ConvertType.ToInt(txt_vnbiz_trang.Text) > ConvertType.ToInt(txt_vnbiz_den.Text) && ConvertType.ToInt(txt_vnbiz_den) == 0) {
                    MessageBox.Show("Trang từ phài nhỏ hơn hay bằng trang đến và trang đến không được bằng 0", "Thông Báo");
                    txt_vnbiz_den.Focus();
                    return;
                }
            }
            if (!fromparent.Contains("vinabiz.org"))
            {
                if (comboBox2.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn ngành nghề", "Thông Báo");
                    comboBox2.Focus();
                    return;
                }
            }
            #endregion

            strLink = txt_vnbiz_link.Text;
            strTen = txtvnbiz_ten.Text;
            strTrang = ConvertType.ToInt(txt_vnbiz_trang.Text);
            if (checkBox1.Checked)
                pathlimit = -1;
            else
                pathlimit = ConvertType.ToInt(txt_vnbiz_den.Text);


            switch (fromparent)
            {
                case "batdongsan.com.vn":
                    DanhmucId = ConvertType.ToInt(comboBox2.SelectedValue);
                    danhmucName = SQLDatabase.ExcDataTable(string.Format("select name from dm_batdongsan where id={0}", comboBox2.SelectedValue)).Rows[0][0].ToString();
                    break;
                case "thitruongsi.com":
                    DanhmucId = ConvertType.ToInt(comboBox2.SelectedValue);
                    danhmucName = SQLDatabase.ExcDataTable(string.Format("select name from dm_thitruongsi where id={0}", comboBox2.SelectedValue)).Rows[0][0].ToString();
                    break;
                case "vinabiz.org":
                    break;
                default:
                    break;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void txt_vnbiz_trang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }

        private void frmthemLinkVinabix_Load(object sender, EventArgs e)
        {
            try
            {
                switch (fromparent)
                {
                    case "batdongsan.com.vn":
                        Binddmbds();
                        comboBox1.Enabled = true;
                        comboBox2.Enabled = true;
                        break;
                    case "thitruongsi.com":
                        Binddmthitruongsi();
                        comboBox1.Enabled = true;
                        comboBox2.Enabled = true;
                        break;
                    default:
                        comboBox1.Enabled = false;
                        comboBox2.Enabled = false;
                        break;
                }
                comboBox1.SelectedValue = -1;
                this.Text = fromparent;
            }
            catch (Exception)
            {
                comboBox1.Enabled = false; 
            }
           
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

            comboBox1.DataSource = table_nhom;
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "name";
            comboBox1.SelectedValue = -1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (fromparent)
                {
                    case "batdongsan.com.vn":
                        if (comboBox1.SelectedItem != null)
                            Bindbds(comboBox1.SelectedValue);
                        break;
                    case "thitruongsi.com":
                        if (comboBox1.SelectedItem != null)
                            Bindthitruongsi(comboBox1.SelectedValue);
                        break;
                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Bindbds(object nhom)
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("[spFindDmbds] '{0}'", ConvertType.ToInt(nhom)));
            IEnumerable<DataRow> query = from order in table.AsEnumerable()
                                         .Where(s => s.Field<string>("path").Length == 0)
                                         select order;

            comboBox2.DataSource = table;
            comboBox2.ValueMember = "id";
            comboBox2.DisplayMember = "name";
            comboBox2.SelectedValue = -1;
        }
        public void Binddmthitruongsi()
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("select ID,NAME from dm_thitruongsi where parentId is null "));
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(int));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add(-1, "---Chọn Nhóm Gian Hàng---");
            foreach (DataRow item in table.Rows)
                table_nhom.Rows.Add(item["id"], item["name"]);

            comboBox1.DataSource = table_nhom;
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "name";
            comboBox1.SelectedValue = -1;
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
            comboBox2.DataSource = table;
            comboBox2.ValueMember = "id";
            comboBox2.DisplayMember = "name";
            comboBox2.SelectedValue = -1;
            comboBox2.DataSource = table;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txt_vnbiz_den.Enabled = !checkBox1.Checked;
        }
    }
}
