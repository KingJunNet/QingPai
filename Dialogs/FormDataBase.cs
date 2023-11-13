using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class FormDataBase : DevExpress.XtraEditors.XtraForm
    {
        private string FilePath = "";
        private const int timeout = 4;

        public FormDataBase(string title)
        {
            InitializeComponent();
            Text = title;
        }

        private void FormDataBase_Load(object sender, EventArgs e)
        {
            FilePath = AppDomain.CurrentDomain.BaseDirectory + "constring.ini";
            var temp = new StringBuilder(1024);
            DataControl.GetPrivateProfileString("a", "server ", "10.8.48.202", temp, 1024, FilePath);
            textEdit1.Text = temp.ToString().Trim();

            DataControl.GetPrivateProfileString("a", "pwd ", "SAUCADCAM", temp, 1024, FilePath);
            textEdit2.Text = temp.ToString().Trim();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.Text = "正在连接服务器....";
            statusStrip1.Visible = true;
            _bar.Value = 0;
            _bar.Maximum = timeout * 2;
            timer1.Enabled = true;
            var args = new[] { textEdit1.Text.Trim(), textEdit2.Text.Trim() };
            _worker.RunWorkerAsync(args);
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var server = (e.Argument as string[])?[0];
            var pwd = (e.Argument as string[])?[1];

            var sql = new DataControl();

            if (sql.TestConnection(timeout, out var errorInfo, server, pwd))
            {
                MessageBox.Show("连接成功");
                e.Result = "连接成功";
            }
            else
            {
                MessageBox.Show($"{sql.strCon}\n{errorInfo}", "连接失败");
                e.Result = "连接失败";
            }
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            textEdit1.Focus();
            button1.Enabled = true;
            button1.Text = "连接";
            statusStrip1.Visible = false;
            timer1.Enabled = false;
            if (!(e.Result is string) || (string) e.Result != "连接成功") 
                return;

            DataControl.WritePrivateProfileString("a", "server", textEdit1.Text, FilePath);
            DataControl.WritePrivateProfileString("a", "pwd", textEdit2.Text, FilePath);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _bar.PerformStep();
        }
    }
}
