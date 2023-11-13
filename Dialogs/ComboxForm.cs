using LabSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class ComboxForm : Form
    {
        public ComboxForm()
        {
            InitializeComponent();
        }

        private TableControl control;
        private string moudle;
        public  Dictionary<string, List<string>> ComboxDictionary = new Dictionary<string, List<string>>();
        public ComboxForm(string moudle, TableControl control)
        {
            InitializeComponent();
            this.moudle=moudle;
            this.control = control;
            var sql0 = new DataControl();
            string sql = $"select distinct format from ComboxTable where moudle like'%{moudle}%'";
            DataTable da = SqlHelper.GetList(sql);

            foreach(DataRow row in da.Rows)
            {

                var list = sql0.GetStringList("select distinct item from ComboxTable " +
                                             $"where format='{row[0].ToString()}' order by item");
                ComboxDictionary.Add(row[0].ToString(),list);
            }
            comboBox1.Items.AddRange(ComboxDictionary.Keys.ToArray());
            comboBox1.SelectedIndex = _lastIndex;
            ShowCurrentKeyList(comboBox1.SelectedIndex);
        }

        private int _lastIndex = 0;

        private void ComboxForm_Shown(object sender, EventArgs e)
        {

           

            //comboBox1.Items.AddRange(Form1.ComboxDictionary.Keys.ToArray());
            //comboBox1.SelectedIndex = _lastIndex;
            //ShowCurrentKeyList(comboBox1.SelectedIndex);
        }

        private void ShowCurrentKeyList(int index)
        {
            var key = comboBox1.Items[index].ToString();
            if (!ComboxDictionary.ContainsKey(key))
                return;

            var dt = new DataTable();
     
            dt.Columns.Add("item", typeof(string));

            string sql = $"select distinct item from ComboxTable where format='{key}' order by item";
            var data = SqlHelper.GetList(sql);
            foreach (DataRow row in data.Rows)
            {
                dt.Rows.Add(row[0].ToString());
            }
            //foreach (var pair in ComboxDictionary[key])
            //{
            //    dt.Rows.Add(pair);
            //}
            gridControl1.DataSource = dt;

            //textBox1.Clear();
            //textBox1.Lines = ComboxDictionary[key].ToArray();
            _lastIndex = index;
        }

        private void SaveLastKeyIndex(int index)
        {
            //var lastList = textBox1.Lines.Select(t => t.Trim()).Distinct().ToList();
            //var lastKey = comboBox1.Items[index].ToString();

            //if (ComboxDictionary.ContainsKey(lastKey))
            //    ComboxDictionary[lastKey] = lastList;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //var sql0 = new DataControl();
            //string sql = $"select distinct format from ComboxTable where moudle like'%{moudle}%'";
            //DataTable da = SqlHelper.GetList(sql);

            //ComboxDictionary.Clear();
            //comboBox1.Items.Clear();

            //foreach (DataRow row in da.Rows)
            //{

            //    var list = sql0.GetStringList("select distinct item from ComboxTable " +
            //                                 $"where format='{row[0].ToString()}' order by item");
            //    ComboxDictionary.Add(row[0].ToString(), list);
            //}
            //comboBox1.Items.AddRange(ComboxDictionary.Keys.ToArray());

            // save last index to dictionary
            // SaveLastKeyIndex(_lastIndex);

            // show current index
            ShowCurrentKeyList(comboBox1.SelectedIndex);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var key = comboBox1.Items[comboBox1.SelectedIndex].ToString();

            string sql = $"select distinct Moudle from ComboxTable where format='{key}'";
            string moudle = SqlHelper.GetList(sql).Rows[0][0].ToString();

            string sqldle = $"delete from ComboxTable where format='{key}'";
            SqlHelper.ExecuteNonquery(sqldle,CommandType.Text);

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                string item =gridView1.GetRowCellValue(i, "item").ToString();
                string sqiinsert = $"insert into ComboxTable(format,item,Moudle) values('{key}','{item}','{moudle}')";
                SqlHelper.ExecuteNonquery(sqiinsert, CommandType.Text);
            }

            //Form1.ComboxDictionary.Clear(); //清空字典

            //Form1.InitComboxDictionary(); //重新加载字典

            //control.reLoadCombox();

            //SaveLastKeyIndex(comboBox1.SelectedIndex);

            //var sql = new DataControl();
            //sql.ExecuteNonQuery("truncate table ComboxTable");

            //var dt = new DataTable();
            //dt.Columns.Add("format", typeof(string));
            //dt.Columns.Add("item", typeof(string));
            //foreach (var pair in ComboxDictionary)
            //{
            //    var key = pair.Key;
            //    var list = pair.Value;
            //    foreach (var item in list)
            //        dt.Rows.Add(key, item);
            //}

            //sql.DataTable2SqlServer(dt, "ComboxTable");



            //Close();
        }

        private void ComboxForm_Load(object sender, EventArgs e)
        {

        }


        
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要删除选中项吗？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                gridView1.DeleteSelectedRows();
            }
            
        }

        private void ComboxForm_Leave(object sender, EventArgs e)
        {
         
        }

        private void ComboxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.ComboxDictionary.Clear(); //清空字典

            Form1.InitComboxDictionary(); //重新加载字典

            control.reLoadCombox();
        }
    }
}
