using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LabSystem.DAL;



namespace TaskManager
{
    public partial class SelectTemplate : DevExpress.XtraEditors.XtraForm
    {
        public SelectTemplate()
        {
            InitializeComponent();
        }

        private string moudle;
        public SelectTemplate(string moudle)
        {
            this.moudle = moudle;
            InitializeComponent();
        }

        private void SelectTemplate_Load(object sender, EventArgs e)
        {
            LoadSource();
        }

        public void LoadSource()
        {
            string sql = $"select distinct templatename,index1 from definetemplate where type ='{moudle}' order by index1";
            DataTable dt = SqlHelper.GetList(sql);
            listBoxControl1.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                listBoxControl1.Items.Add(row[0].ToString());
            }
        }

        public event FreshForm freshForm;
        /// <summary>
        /// 选择模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxControl1_DoubleClick(object sender, EventArgs e)
        {
            string sql = $"select templatecolumn from definetemplate where type ='{moudle}' and templatename ='{listBoxControl1.SelectedItem.ToString()}'";
            DataTable da = SqlHelper.GetList(sql);
            string columns = da.Rows[0][0].ToString();
            string[] column = columns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Templatecolumn.column = column;
            freshForm();
            this.Close();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxControl1.SelectedItems.Count > 0)
            {
                string sql = $"delete  from definetemplate where type ='{moudle}' and templatename ='{listBoxControl1.SelectedItem.ToString()}'";
                if (SqlHelper.ExecuteNonquery(sql, CommandType.Text) > 0)
                {
                    MessageBox.Show("删除成功");
                    LoadSource();
                };
            }
            else
            {
                MessageBox.Show("请选择需要删除模板!");
            }
         
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxControl1.SelectedItems.Count > 0)
            {
                
                EditTemplate edit = new EditTemplate(moudle, listBoxControl1.SelectedItem.ToString());
                edit.ShowDialog();
                LoadSource();
            }
            else
            {
                MessageBox.Show("请选择需要编辑的模板!");
            }
        }

        private void listBoxControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
      

                EditTemplate edit = new EditTemplate(moudle, "新增");
                edit.ShowDialog();
                LoadSource();


        }
        //public event FreshTemplate freshtemplate;
        private void 上移ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int i = this.listBoxControl1.SelectedIndex;
            if (i > 0)
            {
                var aa = listBoxControl1.SelectedItem;
                var uptest = this.listBoxControl1.Items[i - 1];
                //把当前选择行的值与上一行互换 并将选择索引减1
                listBoxControl1.Items[i - 1] = aa;
                listBoxControl1.Items[i] = uptest;
                listBoxControl1.SelectedIndex = i - 1;

                updateIndex();
            }
          
        }

        private void 下移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = listBoxControl1.SelectedIndex;
            if (i < listBoxControl1.Items.Count - 1)
            {
                var aa = listBoxControl1.SelectedItem;
                var uptest = listBoxControl1.Items[i + 1];
                //把当前选择行的值与下一行互换 并将选择索引加1
                listBoxControl1.Items[i + 1] = aa;
                listBoxControl1.Items[i] = uptest;
                listBoxControl1.SelectedIndex = i + 1;
                updateIndex();
            }
        }

        private void updateIndex()
        {
            for (int j = 0; j < listBoxControl1.Items.Count; j++)
            {
                string sql = $"update  definetemplate set index1='{j}' where type ='{moudle}' and TemplateName ='{listBoxControl1.Items[j].ToString()}'";
                SqlHelper.ExecuteNonquery(sql, CommandType.Text);
            }
            //LoadSource();
            //freshtemplate();
        }
    }
}