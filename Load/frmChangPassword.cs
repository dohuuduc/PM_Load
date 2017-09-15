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
    public partial class frmChangPassword : Form {
        public frmChangPassword() {
            InitializeComponent();
            writer = LogWriter.Instance;
        }

        private LogWriter writer;
        private string username;
        private string matkhaucu;

        public string UserName {
            get { return username; }
            set { username = value; }
        }

        public string MatKhauCu {
            get { return matkhaucu; }
            set { matkhaucu = value; }
        }
        private void button1_Click(object sender, EventArgs e) {
            try {
                if (textBox2.Text != textBox3.Text) {
                    label4.Text = "Mật khẩu và mật khẩu xác nhận không khớp! Vui lòng kiễm tra lại.";
                    return;
                }
                if (textBox3.Text.Trim() == "") {
                    label4.Text = "Vui lòng nhập mật khẩu mới";
                    return;
                }

                if (StorePhone.SQLDatabase.ExcNonQuery(string.Format("spChangPassWord {0},{1}", textBox1.Text, textBox2.Text))) {
                    MessageBox.Show("Đổi mật khẩu thành công!");
                    writer.WriteToLog(string.Format("Đổi mật khẩu- UserName: {0}   -Login ", username));
                    this.Close();
                }
                
            }
            catch (Exception) {
                
                throw;
            }
        }

        private void frmChangPassword_Load(object sender, EventArgs e) {
            this.textBox1.Text = username;
            this.textBox2.Text = matkhaucu;
           
        }
    }
}
