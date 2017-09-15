using StorePhone;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Load {
    public partial class frmLogin : Form {
        #region Fields
        private LogWriter writer;
        private string right;

        private int rightid;

        private string username;

        private bool dangnhap;

        #endregion // Fields

        #region Properties

        public int RightID {
            get { return rightid; }
            set { rightid = value; }
        }

        public string Right {
            get { return right; }
            set { right = value; }
        }

        public string UserName {
            get { return username; }
            set { username = value; }
        }

        #endregion // Properties

        public frmLogin() {
            InitializeComponent();
            writer = LogWriter.Instance;
           
            dangnhap = false;
        }

      
        private void button1_Click(object sender, EventArgs e) {
            try {
                if (textBox1.Text == "" || textBox2.Text == "") {
                    label3.Text = "Vui lòng nhập người dùng và mật khẩu.";
                    textBox1.Focus();
                    return;
                }
                
                
                    DataTable kq = StorePhone.SQLDatabase.ExcDataTable(string.Format("spLogin {0},{1}", textBox1.Text, textBox2.Text));
                    if (kq.Rows.Count > 0) {
                        this.right = kq.Rows[0]["name"].ToString();
                        this.rightid = ConvertType.ToInt(kq.Rows[0]["rightid"]);
                        this.username = kq.Rows[0]["username"].ToString();

                        writer.WriteToLog(string.Format("Đăng nhập hệ thống: UserName: {0}   -Login ", username));

                        this.DialogResult = DialogResult.OK;
                        this.Close();

                    }
                    else {
                        label3.Text = "Đăng nhập không thành công!!!";
                    }
               
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "button1_Click");
            }
        }

       

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar.Equals((char)13)) {
                // call your method for action on enter
                button1_Click(null, null);
                e.Handled = true; // suppress default handling
            }
        }
    }
}
