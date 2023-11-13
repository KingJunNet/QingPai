using LabSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class FormLog : Form
    {
        public FormLog()
        {
            InitializeComponent();
            //foreach(string log in Log.logs)
            //{
            //    if (log.Length <= 1) continue;
            //    string type = log.Substring(0, 1);
            //    string value = log.Substring(1);
            //    if (type == "i")
            //        _box.AppendTextColorful(value, Color.Black);
            //    else if (type == "w")
            //        _box.AppendTextColorful(value, Color.Orange);
            //    else if(type=="e")
            //        _box.AppendTextColorful(value, Color.Red);
            //    else
            //        _box.AppendTextColorful(value, Color.Blue);
            //}
            //_box.SelectionLength = 0;
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SaveFileDialog dialog = new SaveFileDialog();
            //dialog.FileName = "运行日志.rtf";
            //if (dialog.ShowDialog() != DialogResult.OK) return;
            //_box.SaveFile(dialog.FileName, RichTextBoxStreamType.RichText);
            //MessageBox.Show("文件已成功保存");
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Log.logs.Clear();
            //_box.Clear();
        }

        private void FormLog_Load(object sender, EventArgs e)
        {
            LoadSource();
        }

        private void LoadSource()
        {
            string sql = "select * from formlog";
            DataTable da = SqlHelper.GetList(sql);
            gridControl1.DataSource = da;
        }
    }


    public static class Log
    {
        public static List<string> logs = new List<string>();

        public static DateTime Begin;

        public static DateTime Last;

        public static void i(string recorder,string moudle)
        {
            string date = DateTime.Now.ToString();
            //WriteLog("i", value);
            string sql = $"insert into formlog(datetime,name,module,recorder) values('{date}','{FormSignIn.CurrentUser.Name.ToString()}','{moudle}','{recorder}')";
            SqlHelper.ExecuteNonquery(sql, CommandType.Text);
        }

        public static void w(string value)
        {
            //WriteLog("w", value);
        }

        public static void e(string value)
        {
            //WriteLog("e", value);
        }

        //private static void WriteLog(string type, string value)
        //{
        //    DateTime Now = DateTime.Now;
        //    string seconds = String.Format("{0,-6}", Math.Ceiling((Now - Begin).TotalMilliseconds));
        //    string lastSecondes = String.Format("{0,-4}", Math.Ceiling((Now - Last).TotalMilliseconds));
        //    logs.Add(type + Now.ToString("HH:mm:ss") + "[" + seconds + "](" + lastSecondes + ")-> " + value);
        //    Last = Now;
        //}

        //public static void AppendTextColorful(this RichTextBox rtBox, string text, Color color, bool addNewLine = true)
        //{
        //    if (addNewLine)
        //    {
        //        text += Environment.NewLine;
        //    }
        //    rtBox.SelectionStart = rtBox.TextLength;
        //    rtBox.SelectionLength = 0;
        //    rtBox.SelectionColor = color;
        //    rtBox.AppendText(text);
        //    rtBox.SelectionColor = rtBox.ForeColor;
        //}
    }
}
