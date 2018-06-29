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
  public partial class frmAddListVinabiz : Form {
    private List<int> listId;
    public frmAddListVinabiz() {
      InitializeComponent();
      getdataVinabiz();
    }
    public List<int> ListId {
      get { return listId; }
      set { listId = value; }
    }

    private void frmAddListVinabiz_Load(object sender, EventArgs e) {

    }
    private void getdataVinabiz() {
      foreach (DataRow item in SQLDatabase.ExcDataTable("select a.id,a.name +'_'+CONVERT(nvarchar(10), isnull(b.tongsl,0)) as name from dm_vinabiz a left join dm_Tinh b on a.dmtinhid=b.id where parentId is null").Rows) {
       TreeNode treeNode=  treeView1.Nodes.Add(item["id"].ToString(),item["name"].ToString());
        treeNode.Tag = item["id"].ToString();
        foreach (DataRow item2 in SQLDatabase.ExcDataTable(string.Format("select id, name from dm_vinabiz where parentId={0}", item["id"])).Rows) {
          TreeNode treeNode2 = treeNode.Nodes.Add(item2["id"].ToString(), item2["name"].ToString());
          treeNode2.Tag = item2["id"].ToString();
          foreach (DataRow item3 in SQLDatabase.ExcDataTable(string.Format("select id, name from dm_vinabiz where parentId={0}", item2["id"])).Rows) {
            treeNode2.Nodes.Add(item3["id"].ToString(), item3["name"].ToString()).Tag= item3["id"].ToString();
          }
        }
      }
    }
    private void CheckTreeViewNode(TreeNode node, Boolean isChecked) {
      foreach (TreeNode item in node.Nodes) {
        item.Checked = isChecked;

        if (item.Nodes.Count > 0) {
          this.CheckTreeViewNode(item, isChecked);
        }
      }
    }
    private void checkBox1_CheckedChanged(object sender, EventArgs e) {
      foreach (TreeNode item in treeView1.Nodes) {
        item.Checked = checkBox1.Checked;
        CheckTreeViewNode(item, checkBox1.Checked);
      }
    }
    private void GetCheckTreeViewNode(TreeNode node,ref List<int> listid) {
      foreach (TreeNode item in node.Nodes) {
        if (item.Checked)
          listid.Add(ConvertType.ToInt(item.Tag));

        if (item.Nodes.Count > 0) {
          this.GetCheckTreeViewNode(item, ref listid);
        }
      }
    }
    private void button1_Click(object sender, EventArgs e) {
      List<int> listid = new List<int>();
      foreach (TreeNode item in treeView1.Nodes) {
        if (item.Checked) listid.Add(ConvertType.ToInt(item.Tag));
        GetCheckTreeViewNode(item, ref listid);
      }
      if (listid.Count() == 0) {
        MessageBox.Show("Vui lòng chọn 1 trong các tỉnh thành - huyện - xã", "Thông Báo");
        return;
      }
      ListId = listid;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    // Updates all child tree nodes recursively.
    private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked) {
      foreach (TreeNode node in treeNode.Nodes) {
        node.Checked = nodeChecked;
        if (node.Nodes.Count > 0) {
          // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
          this.CheckAllChildNodes(node, nodeChecked);
        }
      }
    }

    private void treeView1_AfterCheck(object sender, TreeViewEventArgs e) {
      if (e.Action != TreeViewAction.Unknown) {
        if (e.Node.Nodes.Count > 0) {
          /* Calls the CheckAllChildNodes method, passing in the current 
          Checked value of the TreeNode whose checked state changed. */
          this.CheckAllChildNodes(e.Node, e.Node.Checked);
        }
      }
    }
  }
}
