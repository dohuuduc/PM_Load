using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using StorePhone;
using System.Globalization;

namespace Load {
    public partial class frmCauHinh : Form {
        private List<string> AppConfig = new List<string>();
        private string _name;
        private string _log;
        public frmCauHinh() {
            InitializeComponent();
        }

        private void frmCauHinh_Load(object sender, EventArgs e) {
            try {
                txt_hosocongty.Text = System.Configuration.ConfigurationSettings.AppSettings.Get("ConnectionString");
                BindData();
            }
            catch (System.Exception ex) {
                
                throw;
            }
        }
        
        private void button2_Click(object sender, System.EventArgs e) {
            try {
                if (chk_hosocongty.Checked) {
                    StorePhone.SQLDatabase.ExcDataTable(" delete from dbo.tempExport ");
                    StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[hosocongty]', RESEED, 0);  delete from hosocongty");
                }
                   if(chk_TrangVang.Checked)
                       StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[trangvang]', RESEED, 0);  delete from trangvang");
                    if(chk_vatgia.Checked)
                        StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[vatgia]', RESEED, 0);  delete from vatgia");
                    if (chk_scanner.Checked) {
                        StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[scanner_email]', RESEED, 0);  delete from scanner_email");
                        StorePhone.SQLDatabase.ExcDataTable("DBCC CHECKIDENT ('[scanner_phone]', RESEED, 0);  delete from scanner_phone");
                    }

                    MessageBox.Show("Đã hoàn thành", "Thông Báo");

            }
            catch (System.Exception ex) {
                
                throw;
            }
        }

       

        private void button3_Click(object sender, System.EventArgs e) {
            try {
                if (MessageBox.Show("Bạn có muốn Xoá Log và Nén dữ liệu hệ thống không?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                              MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes) {

                                  SQLDatabase.ExcNonQuery("delete from dbo.tempExport");

                                  string strSQL = string.Format("USE [master]   "+
                                                  "  "+
                                                  " ALTER DATABASE {0} SET RECOVERY SIMPLE WITH NO_WAIT "+
                                                  "  "+
                                                  " USE {0} "+
                                                  " DBCC SHRINKFILE({1}, 1) "+
                                                  "  "+
                                                  " ALTER DATABASE {0} SET RECOVERY FULL WITH NO_WAIT ", _name, _log);
                                  if (SQLDatabase.ExcNonQuery(strSQL)) {
                                      MessageBox.Show("Xoá Log thành công hệ thống");
                                  }

                                  SQLDatabase.ExcNonQuery(string.Format("DBCC SHRINKDATABASE ({0}, 10);", _name));

                                  BindData();

                }
            }
            catch (System.Exception ex) {
                MessageBox.Show(ex.Message,"button3_Click");
            }
        }

        private void BindData() {
            try {
                DataTable table = SQLDatabase.ExcDataTable(string.Format("SELECT file_id, name, type_desc, physical_name, size, max_size  ,state_desc FROM sys.database_files "));
                _name = table.Rows[0]["name"].ToString();
                _log = table.Rows[1]["name"].ToString();
               
                DataTable table_nhom = new DataTable();
                table_nhom.Columns.Add("file_id", typeof(int));
                table_nhom.Columns.Add("name", typeof(string));
                table_nhom.Columns.Add("type_desc", typeof(string));
                table_nhom.Columns.Add("physical_name", typeof(string));
                table_nhom.Columns.Add("size", typeof(string));
                table_nhom.Columns.Add("max_size", typeof(string));
                table_nhom.Columns.Add("state_desc", typeof(string));
                foreach (DataRow item in table.Rows)
                    table_nhom.Rows.Add(item["file_id"], 
                        item["name"], 
                        item["type_desc"].ToString().ToLower(), 
                        item["physical_name"], 
                        ConvertType.ToInt(item["size"]).ToString("#,#", CultureInfo.InvariantCulture),
                        ConvertType.ToInt(item["max_size"]).ToString("#,#", CultureInfo.InvariantCulture),
                                                                item["state_desc"].ToString().ToLower()
                        );

                dataGridView1.DataSource = table_nhom;
                dataGridView1.Refresh();
            }
            catch (System.Exception ex) {
                MessageBox.Show(ex.Message,"BindData");
            }
        }

        private void button1_Click_1(object sender, System.EventArgs e) {
            try {
                if (txt_hosocongty.Text == "") {
                    MessageBox.Show("Vui lòng nhập chuổi kết nối dữ liệu", "Thông Báo");
                    return;
                }
                Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                config.AppSettings.Settings["ConnectionString"].Value = txt_hosocongty.Text;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                MessageBox.Show("Đã thay đổi cấu hình kết nối, Vui lòng khởi động lại phần mềm.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex) {
                MessageBox.Show(ex.Message, "button1_Click");
            }
        }

    }
}
