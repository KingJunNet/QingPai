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
using DevExpress.XtraEditors.Controls;

namespace TaskManager
{
    public partial class EditTemplate : DevExpress.XtraEditors.XtraForm
    {
        public EditTemplate()
        {
            InitializeComponent();
        }
        private string moudle;
        private string name;
        public EditTemplate(string moudle,string name)
        {
            this.moudle = moudle;
            this.name = name;
            
            
            InitializeComponent();
            if (name != "新增")
            {
                this.Text = "编辑" + name + "模板";
            }
            else
            {
                this.Text = "新增模板";
            }
            
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AlertTemplate_Load(object sender, EventArgs e)
        {

            string sql = $"select chs from FieldDefinitionProTable where category = '{moudle}' order by tableIndex";
            DataTable da = SqlHelper.GetList(sql);
            foreach (DataRow row in da.Rows)
            {
                checkedListBoxControl1.Items.Add(row[0].ToString());
            }

            if (name != "新增")
            {
                this.textBox1.Text = name;

                string templateSql = $"select TemplateColumn from DefineTemplate where TemplateName='{name}' and Type ='{moudle}' ";
                string columns = SqlHelper.GetList(templateSql).Rows[0][0].ToString();
                string[] column = columns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < column.Length; i++)
                {
                    string sql0 = $"select chs from FieldDefinitionProTable where category='{moudle}' and eng ='{column[i].ToString()}'";

                    checkedListBoxControl2.Items.Add(SqlHelper.GetList(sql0).Rows[0][0].ToString());
                }
            }


        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入模板名称");
                return;
            }
            if (checkedListBoxControl2.Items.Count==0)
            {
                MessageBox.Show("请选择模板列");
                return;
            }
            string column = "";
            for (int i=0; i<checkedListBoxControl2.Items.Count; i++)
            {
                string sql0 = $"select eng from FieldDefinitionProTable where category='{moudle}' and chs ='{checkedListBoxControl2.Items[i].ToString()}'";
                
                column += SqlHelper.GetList(sql0).Rows[0][0].ToString()+",";
            }
            if (name != "新增")
            {
                string delstr = $"delete from DefineTemplate where TemplateName='{name}' and Type ='{moudle}' ";
                SqlHelper.ExecuteNonquery(delstr, CommandType.Text);
                column = column.Remove(column.Length - 1);
                string sql = $"insert into definetemplate(TemplateName,TemplateColumn,Type) values('{textBox1.Text}','{column}','{moudle}')";
                SqlHelper.ExecuteNonquery(sql, CommandType.Text);
                MessageBox.Show("修改模板列成功");
            }
            else
            {
                column = column.Remove(column.Length - 1);
                string sql = $"insert into definetemplate(TemplateName,TemplateColumn,Type) values('{textBox1.Text}','{column}','{moudle}')";
                SqlHelper.ExecuteNonquery(sql, CommandType.Text);
                MessageBox.Show("新增模板列成功");
            }
            
            this.Close();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxControl1.CheckedItems.Count; i++)
            {
                if (!checkedListBoxControl2.Items.Contains(checkedListBoxControl1.CheckedItems[i].ToString()))
                {
                    checkedListBoxControl2.Items.Add(checkedListBoxControl1.CheckedItems[i].ToString());
                }
                
                //string sql0 = $"select eng from FieldDefinitionProTable where category='{moudle}' and chs ='{checkedListBoxControl1.CheckedItems[i].ToString()}'";

                //column += SqlHelper.GetList(sql0).Rows[0][0].ToString() + ",";
            }
            checkedListBoxControl2.Refresh();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            //checkedListBoxControl2.Items.RemoveAt(checkedListBoxControl2.SelectedIndex);
            List<string> list1 = new List<string>();
            for (int i = 0; i < checkedListBoxControl2.Items.Count; i++)
            {
                if(checkedListBoxControl2.Items[i].CheckState== CheckState.Unchecked)
                {
                    list1.Add(checkedListBoxControl2.Items[i].ToString());
                }
      
            }
            checkedListBoxControl2.Items.Clear();
            foreach(string item in list1)
            {
                checkedListBoxControl2.Items.Add(item);
            }
            
            checkedListBoxControl2.Refresh();

        }

        //上移
        private void button5_Click(object sender, EventArgs e)
        {
            int i = this.checkedListBoxControl2.SelectedIndex;
            if (i > 0)
            {
                var aa = checkedListBoxControl2.SelectedItem;
                var uptest = this.checkedListBoxControl2.Items[i - 1];
                //把当前选择行的值与上一行互换 并将选择索引减1
                checkedListBoxControl2.Items[i - 1] = (CheckedListBoxItem)aa;
                checkedListBoxControl2.Items[i] = uptest;
                checkedListBoxControl2.SelectedIndex = i - 1;
            }
            else
            {
                //button2.Enabled = false;
            }

        }

        //下移
        private void button6_Click(object sender, EventArgs e)
        {
            int i = this.checkedListBoxControl2.SelectedIndex;
            if (i < checkedListBoxControl2.Items.Count - 1)
            {
                var aa = checkedListBoxControl2.SelectedItem;
                var uptest = this.checkedListBoxControl2.Items[i + 1];
                //把当前选择行的值与下一行互换 并将选择索引加1
                checkedListBoxControl2.Items[i + 1] = (CheckedListBoxItem)aa;
                checkedListBoxControl2.Items[i] = uptest;
                checkedListBoxControl2.SelectedIndex = i + 1;
            }


        }
    }
}