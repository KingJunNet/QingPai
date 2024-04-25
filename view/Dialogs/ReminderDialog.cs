using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class ReminderDialog : Form
    {
        public static string FilePath => $"{Application.StartupPath}\\设置.txt";

        /// <summary>
        /// 时间间隔 分钟
        /// </summary>
        public static double Interval = 10.0;

        public static double EquipWarning = 30.0;

        public bool IsReminder => no_read_no_finish > 0 || read_no_finish > 0 || equipment > 0;

        public int no_read_no_finish;

        public int read_no_finish;

        public int equipment;

        public DataTable dt;

        public DataTable dt2;

        public ReminderDialog()
        {
            InitializeComponent();

            using (var fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var s = line.Trim();
                        if(s.StartsWith("#"))
                            continue;

                        if (s.StartsWith("提醒间隔时间"))
                        {
                            var items = s.Split('=');
                            if (items.Length != 2)
                                continue;

                            if (!double.TryParse(items[1], out Interval))
                                Interval = 10.0;
                        }
                        else if (s.StartsWith("设备检定提醒天数"))
                        {
                            var items = s.Split('=');
                            if (items.Length != 2)
                                continue;

                            if (!double.TryParse(items[1], out EquipWarning))
                                EquipWarning = 30.0;
                        }
                        
                    }
                }
            }

            textBox1.Text = Interval.ToString();
            textBox2.Text = EquipWarning.ToString();

            if (FormSignIn.CurrentUser.Department.Contains("组") && !FormSignIn.CurrentUser.Department.Equals("报告组"))
                GetDepartment();
            else
                GetAll();
        }

        private void ReminderDialog_Load(object sender, EventArgs e)
        {
            if (FormSignIn.CurrentUser.Department.Contains("组") && !FormSignIn.CurrentUser.Department.Equals("报告组"))
                ShowDepartment();
            else
                ShowAll();

            label1.Text = FilePath;
        }

        private void GetDepartment()
        {
            var strsql = $"SELECT '{FormSignIn.CurrentUser.Department}' as department,'是' as isRead,COUNT(*) as num " +
                         "FROM TaskTable " +
                         "where LTRIM(RTRIM(Finishstate))<>'已完成' and LTRIM(RTRIM(IsRead))='是' " +
                         $"and department ='{FormSignIn.CurrentUser.Department}'";

            strsql += " union all ";
            strsql += $" SELECT '{FormSignIn.CurrentUser.Department}' as department,'否' as isRead,COUNT(*) as num " +
                      "FROM TaskTable " +
                      "where LTRIM(RTRIM(Finishstate))<> '已完成'  and LTRIM(RTRIM(IsRead))<> '是' " +
                      $"and department ='{FormSignIn.CurrentUser.Department}'";

            strsql += " order by department";

            var sql = new DataControl();
            dt = sql.ExecuteQuery(strsql).Tables[0];

            int.TryParse(dt.Compute("sum(num)", "isRead='否'").ToString(), out no_read_no_finish);
            int.TryParse(dt.Compute("sum(num)", "isRead='是'").ToString(), out read_no_finish);

            #region 设备

            strsql =
                $"SELECT '{FormSignIn.CurrentUser.Department}' as department,COUNT(*) as num FROM EquipmentTable  " +
                "where EquipState<>'检定' and EquipState<>'停用' " +
                "and CheckEndDate is not null and DATEDIFF(day, CheckEndDate, DATEADD(DAY,30,GETDATE()))>= 0 " +
                $"and department='{FormSignIn.CurrentUser.Department}'";
            dt2 = sql.ExecuteQuery(strsql).Tables[0];
            int.TryParse(dt2.Compute("sum(num)", "").ToString(), out equipment);

            #endregion
        }

        private void GetAll()
        {
            #region 任务

            var strsql = "SELECT department,'是' as isRead,COUNT(*) as num FROM TaskTable " +
                         "where LTRIM(RTRIM(Finishstate))<>'已完成' and LTRIM(RTRIM(IsRead))='是' group by department ";

            strsql += " union all ";
            strsql += " SELECT department,'否' as isRead,COUNT(*) as num FROM TaskTable " +
                      "where LTRIM(RTRIM(Finishstate))<> '已完成'  and LTRIM(RTRIM(IsRead))<> '是' group by department ";

            strsql += " order by department";

            var sql = new DataControl();
            dt = sql.ExecuteQuery(strsql).Tables[0];

            int.TryParse(dt.Compute("sum(num)", "isRead='否'").ToString(), out no_read_no_finish);
            int.TryParse(dt.Compute("sum(num)", "isRead='是'").ToString(), out read_no_finish);

            #endregion

            #region 设备

            strsql =
                "SELECT department,COUNT(*) as num FROM EquipmentTable  " +
                "where EquipState<>'检定' and EquipState<>'停用' " +
                "and CheckEndDate is not null and DATEDIFF(day, CheckEndDate, DATEADD(DAY,30,GETDATE()))>= 0 " +
                "group by department";
            dt2 = sql.ExecuteQuery(strsql).Tables[0];
            int.TryParse(dt2.Compute("sum(num)", "").ToString(), out equipment);

            #endregion
        }

        private void ShowDepartment()
        {
            AppendTextColorful($"{FormSignIn.CurrentUser.Department} 试验任务:", Color.Black);
            AppendTextColorful($"    未读且未完成 : {no_read_no_finish}", Color.Red);
            AppendTextColorful($"    已读但未完成 : {read_no_finish}", Color.DarkOrange);
            AppendTextColorful("", BackColor);
            AppendTextColorful($"{FormSignIn.CurrentUser.Department} 待检定设备:{equipment}", Color.DarkOrange);
        }

        private void ShowAll()
        {
            AppendTextColorful("试验任务:", Color.Black);
            AppendTextColorful($"    未读且未完成 : {no_read_no_finish}", Color.Red);
            AppendTextColorful($"    已读但未完成 : {read_no_finish}", Color.DarkOrange);

            foreach (DataRow dr in dt.Rows)
            {
                var department = dr[0].ToString();
                var isRead = dr[1].ToString().Equals("是");
                int.TryParse(dr[2].ToString(), out var num);

                if(!isRead)
                    AppendTextColorful($"|--{department} 未读且未完成 : {num}", Color.Red);
                else
                    AppendTextColorful($"|--{department} 已读但未完成 : {num}", Color.DarkOrange);
            }

            AppendTextColorful("", BackColor);
            AppendTextColorful($"待检定设备:{equipment}", Color.DarkOrange);
            foreach (DataRow dr in dt2.Rows)
            {
                var department = dr[0].ToString();
                int.TryParse(dr[1].ToString(), out var num);

                AppendTextColorful($"|--{department} : {num}", Color.DarkOrange);
            }
        }

        public void AppendTextColorful(string text, Color color,bool addNewLine = true)
        {
            if (addNewLine)
            {
                text += Environment.NewLine;
            }
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = color;
            richTextBox1.AppendText(text);
            richTextBox1.SelectionColor = richTextBox1.ForeColor;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(textBox1.Text, out Interval))
                Interval = 10.0;
            if (Interval < 0.1)
                Interval = 0.1;

            if (!double.TryParse(textBox2.Text, out EquipWarning))
                EquipWarning = 30.0;
            if (EquipWarning < 1)
                EquipWarning = 1;

            var list = new List<string>();

            using (var fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim().StartsWith("提醒间隔时间") || 
                            line.Trim().StartsWith("设备检定提醒天数"))
                            continue;
                        list.Add(line);
                    }
                }
            }

            using (var fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"提醒间隔时间={Interval}");
                    sw.WriteLine($"设备检定提醒天数={EquipWarning}");
                    foreach (var s in list)
                    {
                        sw.WriteLine(s);
                    }
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
