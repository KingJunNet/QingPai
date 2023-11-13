using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class AuthorityForm : Form
    {
        public AuthorityForm()
        {
            InitializeComponent();
            Sql = new DataControl();
            Authority = Sql.AuthorityCheck2("系统维护", "用户管理");
        }

        public DataControl Sql;

        public bool Authority;

        private void AuthorityForm_Shown(object sender, EventArgs e)
        {
            LoadData();
            if (Authority)
                return;

            btnUpdate.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void LoadData()
        {
            var dt = Sql.ExecuteQuery("select distinct module from AuthorityTable2 order by module").Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                var groupName = row[0].ToString().Trim();
                listView1.Groups.Add(groupName, groupName);
            }


            dt = Sql.ExecuteQuery("select * from AuthorityTable2 order by module").Tables[0];
            
            listView1.BeginUpdate();
            listView1.Items.Clear();

            foreach (DataRow row in dt.Rows)
            {
                var module = row["module"].ToString().Trim();
                var item = new ListViewItem(module);
                item.SubItems.Add(row["operate"].ToString().Trim());
                item.SubItems.Add(row["remark"].ToString().Trim());
                item.SubItems.Add(row["userNames"].ToString().Trim());

                listView1.Items.Add(item);
                item.Group = listView1.Groups[module];
            }

            listView1.EndUpdate();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var items = listView1.SelectedItems;
            if (items.Count == 0)
                return;

            var userNames = items[0].SubItems[3].Text.Trim();
            textBox1.Text = userNames;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var items = listView1.SelectedItems;
            if (items.Count == 0)
                return;

            var userNames = textBox1.Text.Replace(Environment.NewLine, ",");
            userNames = userNames.Replace("，", ",");
            userNames = userNames.Replace("；", ",");
            userNames = userNames.Replace(";", ",");
            userNames = userNames.Replace("|", ",");
            userNames = userNames.Replace("\\", ",");
            userNames = userNames.Replace("/", ",");
            userNames = userNames.Replace(".", ",");
            userNames = userNames.Replace("。", ",");
            userNames = userNames.Replace(",,,,", ",");
            userNames = userNames.Replace(",,,", ",");
            userNames = userNames.Replace(",,", ",");

            var module = items[0].SubItems[0].Text.Trim();
            var operate = items[0].SubItems[1].Text.Trim();

            var strsql = $"update AuthorityTable2 set userNames='{userNames}' " +
                         $" where module='{module}' and operate='{operate}'";

            Sql.ExecuteNonQuery(strsql);
            LoadData();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AuthorityForm_Load(object sender, EventArgs e)
        {

        }
    }
}
