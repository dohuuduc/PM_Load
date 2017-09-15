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
    public partial class frmTonghop : Form {
        private DataTable _table;
        public frmTonghop() {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }
        
        public DataTable CreateTable() {
            DataTable table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("cot", typeof(string));
            table.Columns.Add("ten", typeof(string));
            table.Columns.Add("map", typeof(string));
            return table;
        }

        private void frmTonghop_Load(object sender, EventArgs e) {
            _table = CreateTable();
            dataGridView1.DataSource = _table;
            BindColumn();
        }

        private void BindColumn() {
            try {
                for (int i = 0; i < dataGridView_tontai.Columns.Count; i++) {
                    string strname = dataGridView_tontai.Columns[i].Name;
                    dm_tonghopcol dm = SQLDatabase.Loaddm_tonghopcol(string.Format("select * from tonghopcol where cot='{0}'", strname)).FirstOrDefault();
                    if (dm != null && dm.ten.Length != 0)
                        dataGridView_tontai.Columns[i].HeaderText = dm.ten;
                }
                for (int i = 0; i < dataGrid_ListTelNumberNew.Columns.Count; i++) {
                    string strname = dataGrid_ListTelNumberNew.Columns[i].Name;
                    dm_tonghopcol dm = SQLDatabase.Loaddm_tonghopcol(string.Format("select * from tonghopcol where cot='{0}'", strname)).FirstOrDefault();
                    if (dm != null && dm.ten.Length != 0)
                        dataGrid_ListTelNumberNew.Columns[i].HeaderText = dm.ten;
                }
            }
            catch (Exception) {

                throw;
            }

        }

        private void button3_Click(object sender, EventArgs e) {
            try {
                DataTable tb = StorePhone.SQLDatabase.ExcDataTable("select * from tonghopcol");
                foreach (DataRow item in tb.Rows) {
                    _table.Rows.Add(item["id"], item["cot"],item["ten"].ToString().Length == 0 ? item["cot"].ToString(): item["ten"].ToString(),"");
                }
                
                string strNameTable = cbb_table.SelectedItem.ToString();
                DataTable tb2 = StorePhone.SQLDatabase.ExcDataTable(string.Format("SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.{0}') ", strNameTable));
                List<string> strCot = new List<string>();
                foreach (DataRow item in tb2.Rows) {
                    strCot.Add(item["name"].ToString());
                }

                DataGridViewTextBoxColumn id= new DataGridViewTextBoxColumn();
                id.HeaderText = "id";
                id.DataPropertyName = "id";
                id.Visible = false;

                DataGridViewTextBoxColumn cot = new DataGridViewTextBoxColumn();
                cot.HeaderText = "cot";
                cot.DataPropertyName = "cot";
                cot.Visible = false;

                DataGridViewTextBoxColumn ten = new DataGridViewTextBoxColumn();
                ten.HeaderText = "ten";
                ten.DataPropertyName = "ten";

                var map = new DataGridViewComboBoxColumn();
                map.DataSource = strCot;
                map.HeaderText = "map";
                map.DataPropertyName = "map";

                dataGridView1.DataSource = _table;
                dataGridView1.Columns.AddRange(id, cot, ten, map);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message,"button3_Click");
            }
        }
    }
}
