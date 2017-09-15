using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StorePhone;
namespace Load {
    public partial class frmDM_TongHopCol : Form {
        private DataTable _table;
        public frmDM_TongHopCol() {
            InitializeComponent();
            _table = CreateTable();
            dataGridView1.DataSource = _table;
        }

      
        private void frmDM_TongHopCol_Load(object sender, EventArgs e) {
            BindData();
        }

        public DataTable CreateTable() {
            DataTable table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("cot", typeof(string));
            table.Columns.Add("ten", typeof(string));
            return table;
        }

        private void BindData() {
            try {
                PleaseWait objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();
                _table.Clear();
                DataTable tb = SQLDatabase.ExcDataTable(string.Format("select * from TongHopCol"));
                foreach (DataRow item in tb.Rows)
                    _table.Rows.Add(item["id"], item["cot"], item["ten"]);

                DataTable tb2 = StorePhone.SQLDatabase.ExcDataTable("SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.tonghop') ");
                foreach (DataRow item in tb2.Rows) {
                    if (_table.Select(string.Format("cot='{0}'", item["name"])).Count() == 0)
                        _table.Rows.Add(-1, item["name"], "");

                    if (dataGridView1.Rows.Count > 0) {
                        dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];
                    }
                    SendKeys.Send("{DOWN}");

                }
                objPleaseWait.Close();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            try {
                StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[tonghopcol]', RESEED, 0);  delete from tonghopcol");
                foreach (DataGridViewRow row in dataGridView1.Rows) {
                    dm_tonghopcol dm = new dm_tonghopcol();
                    dm.id = ConvertType.ToInt(row.Cells["id"].Value);
                    dm.ten = row.Cells["ten"].Value.ToString();
                    dm.cot = row.Cells["cot"].Value.ToString();
                    SQLDatabase.Adddm_tonghopcol(dm);
                    //dataGridView1.CurrentCell = dataGridView1.Rows[row.Index].rows[0];
                }
                BindData();


               
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message,"button1_Click");
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
            if (dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() == "-1") {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Beige;

            }
        }

        private void button2_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
