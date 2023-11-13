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
    public partial class AlertTemplate : DevExpress.XtraEditors.XtraForm
    {
        public AlertTemplate()
        {
            InitializeComponent();
        }
        private string moudle;
        public AlertTemplate(string moudle)
        {
            this.moudle = moudle;
            InitializeComponent();
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
            string sql = $"select chs from FieldDefinitionTable where category = '{moudle}' order by tableIndex";
            DataTable da =SqlHelper.GetList(sql);
            foreach(DataRow row in da.Rows)
            {
                checkedListBoxControl1.Items.Add(row[0].ToString());
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
            if (checkedListBoxControl1.CheckedItems.Count==0)
            {
                MessageBox.Show("请选择模板列");
                return;
            }
            string column = "";
            for (int i=0; i<checkedListBoxControl1.CheckedItems.Count; i++)
            {
                string sql0 = $"select eng from FieldDefinitionTable where category='{moudle}' and chs ='{checkedListBoxControl1.CheckedItems[i].ToString()}'";
                
                column += SqlHelper.GetList(sql0).Rows[0][0].ToString()+",";
            }

            column = column.Remove(column.Length-1);
            string sql = $"insert into definetemplate(TemplateName,TemplateColumn,Type) values('{textBox1.Text}','{column}','{moudle}')";
            SqlHelper.ExecuteNonquery(sql, CommandType.Text);
            MessageBox.Show("定义模板列成功,左侧导航栏加载模板需要重新启动软件,如需进行选择模板,可点击模板管理进行选择！");
            this.Close();
        }
    }
}