using LabSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager.编辑数据
{
    public partial class SelectWebsite : Form
    {
        public SelectWebsite()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public void LoadInfo()
        {
            listBox1.Items.Clear();
            alterIndex();
            string sql = "select name from SystemWebsite order by index1";
            DataTable data = SqlHelper.GetList(sql);
            foreach(DataRow row in data.Rows)
            {
                listBox1.Items.Add(row[0].ToString());
            }

            
        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if(name.Text=="" || address.Text == "")
            {
                MessageBox.Show("请输入网站名称和网站地址");
                return;
            }
            if (listBox1.Items.Contains(name.Text))
            {
                MessageBox.Show("网站名称已存在");
                return;
            }
            string sql = $"insert into SystemWebsite(name,address) values('{name.Text}','{address.Text}')";
            SqlHelper.ExecuteNonquery(sql, CommandType.Text);
            MessageBox.Show("新增成功");
            LoadInfo();
        }

        private void SelectWebsite_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("请选择网站");
                return;
            }

            string delsql = $"delete from SystemWebsite where name ='{listBox1.SelectedItem.ToString()}'";
            SqlHelper.ExecuteNonquery(delsql, CommandType.Text);


            string sql = $"insert into SystemWebsite(name,address) values('{name.Text}','{address.Text}')";
            SqlHelper.ExecuteNonquery(sql, CommandType.Text);
            MessageBox.Show("修改成功");
            LoadInfo();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string name0 = listBox1.SelectedItem.ToString();
                string sql = $"select name,address from SystemWebsite where name ='{name0}'";

                DataTable data = SqlHelper.GetList(sql);
                name.Text = data.Rows[0][0].ToString();
                address.Text = data.Rows[0][1].ToString();
            }
            
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("请选择网站");
                return;
            }

            string delsql = $"delete from SystemWebsite where name ='{name.Text}'";
            SqlHelper.ExecuteNonquery(delsql, CommandType.Text);
            MessageBox.Show("删除成功");
            LoadInfo();

        }

        private void 打开网址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(listBox1.SelectedItem != null)
                {
                    System.Diagnostics.Process.Start(address.Text);
                }
                
            }
            catch
            {
                MessageBox.Show("网站地址有误");
            }
            
        }

   

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();


            System.Drawing.StringFormat strFmt = new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.NoClip);
            strFmt.Alignment = StringAlignment.Center; //文本垂直居中
            strFmt.LineAlignment = StringAlignment.Center; //文本水平居中

            RectangleF rf = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);





            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, new System.Drawing.SolidBrush(e.ForeColor), rf, strFmt);
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 30;
        }


        public void alterIndex()
        {
            for (var j = 0; j < listBox1.Items.Count; j++)
            {
                string sql = $"update SystemWebsite set index1={j} where name ='{listBox1.Items[j]}'";
                SqlHelper.ExecuteNonquery(sql, CommandType.Text);
            }
            //new Form1().WebsiteBind(new Form1().系统网址);
        }


        public event FreshWibesite freshwibesite;
        private void 上移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = this.listBox1.SelectedIndex;
            if (i > 0)
            {
                var aa = listBox1.SelectedItem;
                var uptest = this.listBox1.Items[i - 1];
                //把当前选择行的值与上一行互换 并将选择索引减1
                listBox1.Items[i - 1] = aa;
                listBox1.Items[i] = uptest;
                listBox1.SelectedIndex = i - 1;

                alterIndex();

                freshwibesite();
            }
            else
            {
                //button2.Enabled = false;
            }


        }

        private void 下移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            int i = this.listBox1.SelectedIndex;
            if (i < listBox1.Items.Count - 1)
            {
                var aa = listBox1.SelectedItem;
                var uptest = this.listBox1.Items[i + 1];
                //把当前选择行的值与下一行互换 并将选择索引加1
                listBox1.Items[i + 1] = aa;
                listBox1.Items[i] = uptest;
                listBox1.SelectedIndex = i + 1;


                alterIndex();
                freshwibesite();
            }
        }
    }
}
