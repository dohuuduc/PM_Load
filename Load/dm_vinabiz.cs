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
    public partial class frm_dmVinabiz : Form
    {
        DataTable _table;
        DataTable _toancuc;
        public frm_dmVinabiz()
        {
            InitializeComponent();
            _table = CreateTable_vinabiz();
            getdata();
            BindDM_NhomVatGia();
            BindDMSanPham(-1);

        }

        private void frm_dmVinabiz_Load(object sender, EventArgs e)
        {
            BindDM_NhomVatGia();
        }

        private void getdata() {
            _toancuc = SQLDatabase.ExcDataTable(" WITH temp(id, name,path,parentId,orderid, alevel)  as (  Select id, name, path,parentId, orderid,0 as aLevel  From dm_vinabiz  Where parentId is null  Union All  Select b.id, b.name, b.path,b.parentId,b.orderid, a.alevel + 1  From temp as a, dm_vinabiz as b  Where a.id = b.parentId  )  Select *  From temp");
        }
        private void BindDM_NhomVatGia()
        {
            try
            {

                getdata();
                DataTable table_nhom = new DataTable();
                table_nhom.Columns.Add("id", typeof(int));
                table_nhom.Columns.Add("name", typeof(string));

                table_nhom.Rows.Add(-1, "---Chọn Tỉnh / Huyện---");

                foreach (DataRow item in _toancuc.Select(string.Format("alevel=0")))
                {
                    table_nhom.Rows.Add(item["id"],string.Format("Tp/Tỉnh -->{0}", item["name"]));
                    foreach (dm_vinabiz item2 in SQLDatabase.Loaddm_vinabiz(string.Format("SELECT * FROM DM_VINABIZ WHERE parentId={0}", item["id"])))
                    {
                        table_nhom.Rows.Add(item2.id,string.Format(" Quận/Huyện:  --> {0}", item2.name));
                    }
                }
                   

                comboBox1.DataSource = table_nhom;
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BindDM_NhomVatGia", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataTable CreateTable_vinabiz()
        {
            DataTable table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("parentId", typeof(int));
            table.Columns.Add("path", typeof(string));
            table.Columns.Add("alevel", typeof(string));
            table.Columns.Add("orderid", typeof(int));
            return table;
        }

        private void BindDMSanPham(int id)
        {
            try
            {
                _table.Clear();
                if (_table ==null) return;
                /*if id chọn tất cả show all*/
                if (id == 0) {
                    DataRow[] mangall = _toancuc.Select(string.Format("parentId is null"));
                    foreach (DataRow item in mangall)
                    {
                        _table.Rows.Add(item["id"], item["name"], item["parentId"], item["path"], item["alevel"], item["orderid"]);
                    }
                    dataGridView1.DataSource = _table;
                    return;
                }

                DataRow mang = _toancuc.Select(string.Format("id={0}",id)).FirstOrDefault();
                if (mang == null) return;

                _table.Rows.Add(mang["id"].ToString(), mang["name"], mang["parentId"], mang["path"],mang["alevel"],mang["orderid"]);
                if (mang["alevel"].Equals(0))
                {
                    DataRow[] dmQ = _toancuc.Select(string.Format("parentId={0}", mang["id"]));
                    foreach (DataRow quan in dmQ)
                    {
                        _table.Rows.Add(quan["id"],  quan["name"], quan["parentId"], quan["path"], quan["alevel"], quan["orderid"]);
                        DataRow[] dmQ1 = _toancuc.Select(string.Format("parentId={0}", quan["id"]));
                        foreach (DataRow item in dmQ1)
                        {
                            _table.Rows.Add(item["id"], item["name"], item["parentId"], item["path"], item["alevel"], item["orderid"]);
                        }
                    }
                }
                else {
                 
                    DataRow[] dmQ1 = _toancuc.Select(string.Format("parentId={0}", mang["id"]));
                        foreach (DataRow item in dmQ1)
                        {
                            _table.Rows.Add(item["id"], item["name"], item["parentId"], item["path"], item["alevel"], item["orderid"]);
                        }
                }
                dataGridView1.DataSource = _table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BindDMSanPham", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       

        public List<dm_vinabiz> getdanhmuc(int id) {
            try
            {
                if (id == -1) return new List<dm_vinabiz>();
                List<dm_vinabiz> model = new List<dm_vinabiz>();
                foreach (dm_vinabiz item in SQLDatabase.Loaddm_vinabiz(string.Format("select * from dm_vinabiz where id={0} order by name asc",id)))
                {
                    model.Add(item);
                    foreach (dm_vinabiz con in SQLDatabase.Loaddm_vinabiz(string.Format("select * from dm_vinabiz where parentId={0}",item.id)))
                    {
                        model.Add(con);
                    }
                }
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strsql = "";
            strsql = "select max(orderid) as vitri from dm_vinabiz";
            DataTable table = SQLDatabase.ExcDataTable(strsql);

            txt_name.Text = "";
            txt_id.Text = "";
            txt_orderid.Text = (ConvertType.ToInt(table.Rows[0]["vitri"]) + 1).ToString();
            txt_path.Text = "";
            txt_name.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool isnew = true;
            int vistrisua = 0;

           
            dm_vinabiz dm = new dm_vinabiz();
            dm.id = ConvertType.ToInt(txt_id.Text);
            dm.name = txt_name.Text.Trim();
            dm.path = txt_path.Text;
            dm.paren_id =  comboBox1.SelectedValue.Equals(-1) ? (int?)null : ConvertType.ToInt(comboBox1.SelectedValue);
          
            dm.orderid = ConvertType.ToInt(txt_orderid.Text);
            if (dm.id == 0)
            {
                isnew = true;
                SQLDatabase.AdddmVinabiz(dm);
            }
            else
            {
                isnew = false;
                vistrisua = dataGridView1.SelectedRows[0].Index;
                SQLDatabase.Updatedm_vinabiz(dm);
            }

            int vttruoc = comboBox1.SelectedIndex;
            BindDM_NhomVatGia();
            BindDMSanPham(ConvertType.ToInt(comboBox1.SelectedValue));
            comboBox1.SelectedIndex = vttruoc;


            if (isnew)
            {
                int nRowIndex = dataGridView1.Rows.Count - 1;
                if (dataGridView1.Rows.Count - 1 >= nRowIndex)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = nRowIndex;
                    dataGridView1.Rows[nRowIndex].Selected = true;
                    dataGridView1.Rows[nRowIndex].Cells[0].Selected = true;
                }
            }
            else
            {
                dataGridView1.Rows[vistrisua].Selected = true;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDMSanPham(ConvertType.ToInt(comboBox1.SelectedValue));
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            try
            {
                var rowsCount = dataGridView1.SelectedRows.Count;
                if (rowsCount == 0)
                    return;

                var row = dataGridView1.SelectedRows[0];
                if (row == null) return;

                txt_id.Text = row.Cells["id"].Value.ToString();
                txt_name.Text = row.Cells["name"].Value.ToString();
                txt_path.Text = row.Cells["path"].Value.ToString();
                txt_orderid.Text = row.Cells["orderid"].Value.ToString();



               
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
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
                else {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 8);
                }
            }
        }
    }
}
