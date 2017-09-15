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
    public partial class frmSaveAs : Form {
        public string fromparent;
        public List<SaveAs> saveas;
        public string FromParent {
            get { return fromparent; }
            set { fromparent = value; }
        }
        public List<SaveAs> Saveas {
            get { return saveas; }
            set { saveas = value; }
        }
        public frmSaveAs() {
            InitializeComponent();
        }


        private void frmSaveAs_Load(object sender, EventArgs e) {
            try {
                DataTable tb = StorePhone.SQLDatabase.ExcDataTable(string.Format("select id, id_chon,  cap_id_chon, group_id_chon, name_chon, ma_chon, parentid_chon,path_chon, orderid_chon, scannerby, createdate from saveas where scannerby='{0}'", fromparent));
                if(!fromparent.Contains("hosocongty.vn"))
                    dataGridView1.Columns[3].Visible = false;
                dataGridView1.DataSource = tb;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "frmSaveAs_Load");                
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            PleaseWait objPleaseWait = null;
            try {
                objPleaseWait = new PleaseWait();
                objPleaseWait.Show();
                objPleaseWait.Update();

                saveas = new List<SaveAs>();
                foreach (DataGridViewRow row in dataGridView1.SelectedRows) {
                    int id = ConvertType.ToInt(row.Cells["id"].Value.ToString());
                    string id_chon = row.Cells["id_chon"].Value.ToString();
                    string ma_chon = row.Cells["ma_chon"].Value.ToString();
                    string orderid_chon = row.Cells["orderid_chon"].Value.ToString();
                    string parentid_chon = row.Cells["parentid_chon"].Value.ToString();
                    string path_chon = row.Cells["path_chon"].Value.ToString();

                    saveas.Add(new SaveAs() {
                        id = ConvertType.ToInt(row.Cells["id"].Value.ToString()),
                        id_chon = row.Cells["id_chon"].Value.ToString(),
                        ma_chon = row.Cells["ma_chon"].Value.ToString(),
                        name_chon = row.Cells["name_chon"].Value.ToString(),
                        orderid_chon = row.Cells["orderid_chon"].Value.ToString(),
                        parentid_chon = row.Cells["parentid_chon"].Value.ToString(),
                        path_chon = row.Cells["path_chon"].Value.ToString()
                    });
                    SQLDatabase.DelSaveAs(new SaveAs(){ id =  ConvertType.ToInt(row.Cells["id"].Value.ToString())});
                }
                if (objPleaseWait != null)
                    objPleaseWait.Close();

                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            catch (Exception ex) {
                if (objPleaseWait != null)
                    objPleaseWait.Close();

                MessageBox.Show(ex.Message, "button1_Click");
            }
        }
    }
}
