using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using ExpertLib.Utils;
using TaskManager.统计;

namespace TaskManager
{
    public partial class StatisticForm : Form
    {
        public DataTable DataSource;

        public Dictionary<string, int> Cols;

        public DataControl Sql;


        public StatisticForm()
        {
            InitializeComponent();
            Sql = new DataControl();
            Cols = new Dictionary<string, int>
            {
                {"department", 0},
                {"Drum", 1},
                {"Type1", 2},
            };
            view.CellMerge += ViewCellMerge;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            dateTimePicker1.Value = DateTime.Now.GetWeekFirstDayMon();
            dateTimePicker2.Value = DateTime.Now.GetWeekLastDaySun();
            comboBoxEdit1.SelectedIndex = 0;

        }

        #region 日期

        private void button1_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.GetWeekFirstDayMon().AddDays(-7);
            dateTimePicker2.Value = DateTime.Now.GetWeekLastDaySun().AddDays(-7);
        }

        private void btnMonthClick(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
            dateTimePicker2.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddDays(-1);
        }

        private void comboBoxEdit1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBoxEdit1.SelectedIndex == 0)
                return;
            int nowmonth = DateTime.Now.Month;
            int selectmonth = int.Parse(comboBoxEdit1.Text);
            dateTimePicker1.Value =DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(selectmonth - nowmonth);
            dateTimePicker2.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddDays(-1).AddMonths(selectmonth - nowmonth);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dateTimePicker2.Value = new DateTime(DateTime.Now.Year + 1, 1, 1).AddDays(-1);
        }

        #endregion

        #region GridView

        private void LoadDataSource()
        {
            const string strsql =
                "select department,Drum,Type1,Producer,COUNT(distinct(Carvin)) as CarCount,SUM(Price) as TotalPrice" +
                " from(SELECT department, drum, Type1, Producer, Carvin, Price," +
                " (CASE WHEN TestEndDate is not null THEN TestEndDate ELSE TestStartDate END) as TaskDate" +
                " FROM TaskTable where  TestStartDate is not null) taskView" +
                " where TaskDate>=@date1 and TaskDate<=@date2" +
                " group by department, Drum, Type1, Producer order by department,Drum,Type1,Producer";

            var parameters = new[]
            {
                new SqlParameter("date1", dateTimePicker1.Value.Date),
                new SqlParameter("date2", dateTimePicker2.Value.Date)
            };

            DataSource = Sql.ExecuteQuery(strsql, parameters).Tables[0];

            _control.BeginUpdate();
            view.BeginUpdate();

            _control.DataSource = DataSource;

            view.EndUpdate();
            _control.EndUpdate();
        }

        private void ViewCellMerge(object sender, CellMergeEventArgs e)
        {
            if (DataSource == null)
                return;

            if (!Cols.ContainsKey(e.Column.FieldName))
            {
                e.Merge = false;
                e.Handled = false;
                return;
            }

            var index = Cols[e.Column.FieldName];

            var merge = true;
            foreach (var item in Cols)
            {
                if (item.Value > index)
                    continue;

                var value1 = view.GetRowCellValue(e.RowHandle1, view.Columns[item.Key]).ToString();
                var value2 = view.GetRowCellValue(e.RowHandle2, view.Columns[item.Key]).ToString();
                if (value1.Equals(value2))
                    continue;

                merge = false;
                break;
            }

            e.Merge = merge;
            e.Handled = merge;
        }

        #endregion

        private void BtnRefreshClick(object sender, EventArgs e)
        {
            LoadDataSource();
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "导出Excel",
                Filter = "Excel文件(*.xlsx)|*.xlsx",
                FileName = $"试验统计{dateTimePicker1.Value.Date:yyyy-MM-dd}_{dateTimePicker2.Value.Date:yyyy-MM-dd}"
            };
            var dialogResult = saveFileDialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
                return;
            var options = new XlsxExportOptions
            {
                ExportMode = XlsxExportMode.SingleFile
            };

            _control.ExportToXlsx(saveFileDialog.FileName, options);
            
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 打印PToolStripButton_Click(object sender, EventArgs e)
        {
            var print = new PrintingSystem();
            var link = new PrintableComponentLink(print);
            print.Links.Add(link);
            link.Component = _control;//这里可以是可打印的部件
            if (link.PageHeaderFooter is PageHeaderFooter phf)
            {
                phf.Header.Content.Clear();
                phf.Header.Content.AddRange(new string[] {"", "", ""});
                phf.Header.Font = new System.Drawing.Font("宋体", 14, System.Drawing.FontStyle.Bold);
                phf.Header.LineAlignment = BrickAlignment.Center;
            }

            link.CreateDocument(); //建立文档
            print.PreviewFormEx.Show();//进行预览
        }

        private void 图表ToolStripButton1_Click(object sender, EventArgs e)
        {
            ChartForm chartForm = new ChartForm();
            chartForm.Show();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ShowInfo chart = new ShowInfo();
            chart.Show();
        }
    }
}
