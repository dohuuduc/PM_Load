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
    public partial class frmDM_Muaban_timviec : Form
    {
        private DataTable _table;
        private String _path = "https://muaban.net/nguoi-tim-viec-toan-quoc-l0-c12";
        public frmDM_Muaban_timviec()
        {
            InitializeComponent();
            _table = CreateTable();
            BindDmNhom();
            gv_muaban_timviec.DataSource = _table;
        }

        public void BindDmNhom()
        {
            DataTable table = SQLDatabase.ExcDataTable(string.Format("select id,name from dm_muaban where parentId is null"));
            DataTable table_nhom = new DataTable();
            table_nhom.Columns.Add("id", typeof(string));
            table_nhom.Columns.Add("name", typeof(string));
            table_nhom.Rows.Add(-1, "---Tất Cả---");
            foreach (DataRow item in table.Rows)
                table_nhom.Rows.Add(item["id"], item["name"]);

            cmd_muaban_tv_danhmuc.DataSource = table_nhom;
            cmd_muaban_tv_danhmuc.ValueMember = "id";
            cmd_muaban_tv_danhmuc.DisplayMember = "name";
            cmd_muaban_tv_danhmuc.SelectedValue = -1;
        }

        private void frmDM_Muaban_timviec_Load(object sender, EventArgs e)
        {

        }
       

        public DataTable CreateTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("stt", typeof(int));
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("path", typeof(string));
            return table;
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            
            if (gv_muaban_timviec.Rows[e.RowIndex].Cells["id"].Value.ToString() == "")
            {
                gv_muaban_timviec.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
            }
        }

        private void gv_muaban_timviec_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            label4.Text = string.Format("Mới: {0}     -   Cũ: {1}  /  Tổng:  {2}", _table.Select("id is null").Count(), _table.Select("id is not null").Count(), gv_muaban_timviec.Rows.Count);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PleaseWait objPleaseWait = new PleaseWait();
            try
            {

                objPleaseWait.Show();
                objPleaseWait.Update();

                foreach (DataRow row in _table.Rows)
                {
                    dm_muaban dm = new dm_muaban();
                    dm.name = row["name"].ToString();
                    dm.path = row["path"].ToString();
                    dm.parentId =ConvertType.ToInt(txtid.Text);
                    if (ConvertType.ToInt(SQLDatabase.ExcScalar(string.Format("select count(*) from dm_muaban where path='{0}'", dm.path))) > 0)
                    {
                        SQLDatabase.UpdatedmMuaban(dm);
                    }
                    else
                    {
                        SQLDatabase.AddmMuaban(dm);
                        row["id"] = dm.id;
                    }

                }
                cmd_muaban_tv_danhmuc_SelectedIndexChanged(null,null);
                objPleaseWait.Close();
            }
            catch (Exception ex)
            {
                if (objPleaseWait != null)
                    objPleaseWait.Close();

                MessageBox.Show(ex.Message, "button2_Click");
            }
        }

        

        private void gv_muaban_timviec_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

            var rowsCount = gv_muaban_timviec.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;

            var row = gv_muaban_timviec.SelectedRows[0];
            if (row == null) return;

        }

        private void frmDM_Muaban_timviec_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void cmd_muaban_tv_danhmuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            PleaseWait objPleaseWait = new PleaseWait();
            try
            {
                _table.Clear();
                int id = ConvertType.ToInt(cmd_muaban_tv_danhmuc.SelectedValue);
                txtid.Text = id.ToString();
                if (id !=1 && id !=2) return;

                objPleaseWait.Show();
                objPleaseWait.Update();

                string _path = SQLDatabase.ExcDataTable(string.Format("select * from dm_muaban where id={0}", id)).Rows[0]["path"].ToString();
                Dictionary<string, string> dm = new Dictionary<string, string>();
                dm = Utilities_muaban.getDanhMuc(_path, label4);

                DataTable tb1 = SQLDatabase.ExcDataTable(string.Format("select * from dm_muaban  where parentId={0}", id));
                foreach (DataRow item in tb1.Rows)
                {
                    _table.Rows.Add(_table.Rows.Count + 1, item["id"], item["name"], item["path"]);
                }
                foreach (KeyValuePair<string, string> entry in dm)
                {
                    if (_table != null)
                    {
                        var k = (from r in _table.Rows.OfType<DataRow>()
                                 where r["path"].ToString() == entry.Key
                                 select r).FirstOrDefault();
                        if (k == null)
                        {
                            _table.Rows.Add(_table.Rows.Count + 1, null, entry.Value, entry.Key);
                        }
                    }
                }
                objPleaseWait.Close();
            }
            catch (Exception ex)
            {
                objPleaseWait.Close();
                MessageBox.Show(ex.Message, "cmd_trangvang_SelectedIndexChanged");
            }
        }
    }
}
