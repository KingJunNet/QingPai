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
using ExpertLib.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Columns;

namespace TaskManager.统计
{
    public partial class ShowInfo : DevExpress.XtraEditors.XtraForm
    {
        private static string department;
        private DataControl data = new DataControl();
        public ShowInfo()
        {
            
            InitializeComponent();
            dateTimePicker1.Text = DateTime.Now.GetWeekFirstDayMon().AddDays(-7).ToString("yyyy-MM-dd");
            dateTimePicker2.Text = DateTime.Now.GetWeekLastDaySun().AddDays(-7).ToString("yyyy-MM-dd");
            department = "常温组";
            calcinfo.Text = "负荷比例： 工作日平均创收额/标准负荷值\r\n标准负荷值：29万（常温组2019年创收额 / 海拔组2019年创收额 * 海拔组标准负荷值）\r\n注：负荷比例未包含非创收性工作，如：课题，内部比对，预处理等";
            ShowAllinfo();
            progressLoad(department);
        }

        public void ShowAllinfo()
        {
            gridView1.Columns[0].Visible = true;
            gridView1.Columns[1].Caption = "生产厂家";
            gridView1.Columns[2].Caption = "产家车数";
            gridView1.Columns[3].Caption = "收入";
            gridView1.Columns[2].SummaryItem.DisplayFormat = "总车数：{0}";
            gridView1.Columns[3].SummaryItem.DisplayFormat = "总收入：{0}";

            gridView2.Columns[0].Caption = "非认证项目";
            gridView2.Columns[1].Caption = "次数";

            gridView3.Columns[0].Caption = "认证项目";
            gridView3.Columns[1].Caption = "次数";

            LoadInfo(department);
            LoadInfo2(department);
            LoadInfo3(department);

            LoadSafeInfo(department);

            label4.Text = department;
            panel1.Visible = true;
            splitContainer1.Visible = true;
            chartControl1.Visible = false;
            calcmethod.Visible = false;
            toggle.Visible = false;
        }

        /// <summary>
        /// 认证项目
        /// </summary>
        /// <param name="date"></param>
        /// <param name="department"></param>
        private void LoadInfo(string department)
        {
            DataControl data = new DataControl();
            string sql = "select testsample,count(testsample) as count from TaskTable  where certification = '是' and department = '" + department + "' and TestStartDate<= '" + dateTimePicker2.Value + "' and TestStartDate>= '" + dateTimePicker1.Value + "' group by Testsample";
            DataTable da = data.ExecuteQuery(sql).Tables[0];
            gridControl1.DataSource = da;
        }

        /// <summary>
        /// 非认证项目
        /// </summary>
        /// <param name="date"></param>
        /// <param name="department"></param>
        private void LoadInfo3(string department)
        {
            DataControl data = new DataControl();
           
            string sql = "select testsample,count(testsample) as count from TaskTable  where certification = '否' and department = '" + department + "' and TestStartDate<= '" + dateTimePicker2.Value + "' and TestStartDate>= '" + dateTimePicker1.Value+ "' group by Testsample";
            DataTable da = data.ExecuteQuery(sql).Tables[0];
            gridControl3.DataSource = da;
        }


        private void LoadSafeInfo(string department)
        {
            
            string sql = "";
            if (department == "盐城和团泊")
            {
                sql = $"select groups as 组别, RegistrantPeople as 登记人,RegistrationTime as 登记时间,EquipLog as 设备日志,EuipDrum as 转股或设备编号,Class as 情况等级 from SafeTable where (Groups ='团泊' or Groups ='盐城' or Groups ='盐城常温' or Groups ='盐城低温' or Groups ='盐城蒸发' or Groups ='盐城耐久1' or Groups ='盐城耐久2' or Groups ='盐城耐久3')  and RegistrationTime<= '{dateTimePicker2.Value}' and RegistrationTime>= '{dateTimePicker1.Value}'";
                gridControl4.Visible = false;
                gridControl5.Visible = true;
            }
            else
            {
                sql = $"select groups as 组别, RegistrantPeople as 登记人,RegistrationTime as 登记时间,EquipLog as 设备日志,EuipDrum as 转股或设备编号,Class as 情况等级 from SafeTable where Groups ='{department}' and RegistrationTime<= '{dateTimePicker2.Value}' and RegistrationTime>= '{dateTimePicker1.Value}'";
                gridControl4.Visible = true;
                gridControl5.Visible = false;
            }

            
            DataTable da = data.ExecuteQuery(sql).Tables[0];
            gridControl4.DataSource = da;
            gridControl5.DataSource = da;
            gridView5.Columns[3].ColumnEdit = repoLog;
            gridView5.Columns[3].Width = 300;

        }


        #region 厂家车数
        public void LoadInfo2(string department)
        {
            DataControl data = new DataControl();
            
            string sql = "select Type1,Producer,COUNT(distinct(Carvin)) as count ,sum(price) as price from TaskTable  where department ='"+department+"' and TestStartDate<= '" + dateTimePicker2.Value + "' and TestStartDate>='" + dateTimePicker1.Value+ "' group by type1,Producer";
            DataTable da = data.ExecuteQuery(sql).Tables[0];
            gridControl2.DataSource = da;
        }


        #endregion
        /// <summary>
        /// 上周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            dateTimePicker1.Text = DateTime.Now.GetWeekFirstDayMon().AddDays(-7).ToString("yyyy-MM-dd");
            dateTimePicker2.Text = DateTime.Now.GetWeekLastDaySun().AddDays(-7).ToString("yyyy-MM-dd");
            if (department == "报告组")
            {
                reportratio();
                return;
            }
            if (department == "盐城和团泊")
            {
                LoadSafeInfo(department);
                return;
            }
          
            if (department == "耐久组")
            {
                shownaijiuinfo();         
            }
            else
            {
                ShowAllinfo();
            }
            progressLoad(department);

        }

        /// <summary>
        /// 上月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime d1 = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
            DateTime d2 = d1.AddMonths(1).AddDays(-1);
            dateTimePicker1.Text = d1.ToString("yyyy-MM-dd");
            dateTimePicker2.Text = d2.ToString("yyyy-MM-dd");

            if (department == "报告组")
            {
                reportratio();
                return;
            }
            if (department == "盐城和团泊")
            {
                LoadSafeInfo(department);
                return;
            }

            if (department == "耐久组")
            {
                shownaijiuinfo();
            }
            else
            {
                ShowAllinfo();
            }
            progressLoad(department);
        }

        private void changwen_Click(object sender, EventArgs e)
        {
            department = "常温组";
            calcinfo.Text = "负荷比例： 工作日平均创收额/标准负荷值\r\n标准负荷值：29万（常温组2019年创收额 / 海拔组2019年创收额 * 海拔组标准负荷值）\r\n注：负荷比例未包含非创收性工作，如：课题，内部比对，预处理等.";
            ShowAllinfo();
            progressLoad(department);
        }

        private void haiba_Click(object sender, EventArgs e)
        {
            department = "海拔组";
            calcinfo.Text = "负荷比例： 工作日平均创收额/标准负荷值\r\n标准负荷值： 22.5万（6次排放 + 3次油耗）\r\n注：负荷比例未包含非创收性工作,如：课题,内部比对,预处理等";
            ShowAllinfo();
            progressLoad(department);
        }

        private void diwen_Click(object sender, EventArgs e)
        {
            department = "低温组";
            calcinfo.Text = "负荷比例： 工作日平均创收额/标准负荷值\r\n标准负荷值：29万（低温组2019年创收额 / 海拔组2019年创收额 * 海拔组标准负荷值）\r\n注：负荷比例未包含非创收性工作，如：课题，内部比对，预处理等";
            toggle.Visible = false;
            ShowAllinfo();
            progressLoad(department);
        }

        private void zhengfa_Click(object sender, EventArgs e)
        {
            department = "蒸发组";
            calcinfo.Text = "负荷比例： 工作日平均创收额 / 标准负荷值\r\n标准负荷值：16.6万（2019年工作日平均产值） \r\n注：负荷比例未包含非创收性工作，如：课题，内部比对，预处理等";

            ShowAllinfo();
            progressLoad(department);
        }

        private void pve_Click(object sender, EventArgs e)
        {
            department = "PVE组";
            calcinfo.Text = "负荷比例： 工作日平均创收额/标准负荷值\r\n标准负荷值：24万（两台次）\r\n注：负荷比例未包含非创收性工作，如：课题，内部比对，预处理等";
            ShowAllinfo();
            progressLoad(department);
        }

        private void rde_Click(object sender, EventArgs e)
        {
            department = "RDE组";
            calcinfo.Text = "负荷比例： 工作日平均创收额/标准负荷值\r\n标准负荷值： 22.5万（6次排放 + 3次油耗）\r\n注：负荷比例未包含非创收性工作,如：课题,内部比对,预处理等";
            ShowAllinfo();
            progressLoad(department);
        }

        private void naijiu_Click(object sender, EventArgs e)
        {
            department = "耐久组";
            toggle.Text= "试验室负荷";
            calcmethod.Text = "负荷比例：\r\n电动车：每日试验时长 / 15小时\r\n传统车：每日试验时长 / 20小时 + 每日里程数 / 1400km\r\n标准负荷值：\r\n电动车：15小时 / 天\r\n传统车：20小时 或 1400km\r\n注：负荷比例未包含非创收性工作，如：课题，内部比对，预处理等";
            shownaijiuinfo();
            
            progressLoad(department);
            chartControl1.Visible = false;
            panel1.Visible = false;
            toggle.Visible = true;
            splitContainer1.Visible = true;
        }

        private void shownaijiuinfo()
        {
            LoadSafeInfo(department);
            if (toggle.Text == "试验室负荷")
            {
                gridControl4.Visible = true;
                calcmethod.Visible = false;
            }
            else
            {
                gridControl4.Visible = false;
                calcmethod.Visible = true;
            }
           
            DataControl data = new DataControl();
          
            //试验时长
            string sql0 = "select testsample,SUM(CASE WHEN ISNUMERIC(TestTimeSpan)=1 THEN CAST(TestTimeSpan as float) ELSE 0 END) as count from TaskTable  where  department = '" + department + "' and TestStartDate<= '" + dateTimePicker2.Value + "' and TestStartDate>= '" + dateTimePicker1.Value + "' group by Testsample";
            DataTable da0 = data.ExecuteQuery(sql0).Tables[0];
            gridView3.Columns[0].Caption = "试验项目";
            gridView3.Columns[1].Caption = "试验时长";
            gridControl1.DataSource = da0;

            //试验里程
            string sql1 = "select testsample,SUM(CASE WHEN ISNUMERIC(TotalMileage)=1 THEN CAST(TotalMileage as float) ELSE 0 END) as COUNT from TaskTable  where department = '" + department + "' and TestStartDate<= '" + dateTimePicker2.Value + "' and TestStartDate>= '" + dateTimePicker1.Value + "' group by Testsample";
            DataTable da1 = data.ExecuteQuery(sql1).Tables[0];
            gridView2.Columns[0].Caption = "试验项目";
            gridView2.Columns[1].Caption = "试验里程";
            gridControl3.DataSource = da1;

            //厂家
            string sql2 = "select Producer,SUM(CASE WHEN ISNUMERIC(TestTimeSpan)=1 THEN CAST(TestTimeSpan as float) ELSE 0 END) as count,SUM(CASE WHEN ISNUMERIC(TotalMileage)=1 THEN CAST(TotalMileage as float) ELSE 0 END) as price from TaskTable  where department = '" + department + "' and TestStartDate<= '" + dateTimePicker2.Value+ "' and TestStartDate>= '" + dateTimePicker1.Value + "' group by Producer";
            DataTable da2 = data.ExecuteQuery(sql2).Tables[0];
            gridView1.Columns[0].Visible = false;
            gridView1.Columns[1].Caption = "生产厂家";
            gridView1.Columns[2].Caption = "试验时长";
            gridView1.Columns[3].Caption = "试验里程";
            gridView1.Columns[2].SummaryItem.DisplayFormat = "总时长：{0}";
            gridView1.Columns[3].SummaryItem.DisplayFormat = "总里程：{0}";
            gridControl2.DataSource = da2;

            label4.Text = department;
        }

        private void ShowInfo_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 加载进度条
        /// </summary>
        /// <param name="date"></param>
        /// <param name="department"></param>
       private void progressLoad(string department)
       {
            DataControl data = new DataControl();

            string standard = "";
            string intstandard = "";
            string realvalue = "";
            string scale = "";           
            
            switch (department)
            {
                case "常温组":
                    standard = "29万";
                    intstandard = "290000";
                    break;
                case "海拔组":
                    standard = "22.5万";
                    intstandard = "225000";
                    break;
                case "低温组":
                    standard = "41.8万";
                    intstandard = "418000";
                    break;
                case "蒸发组":
                    standard = "16.6万";
                    intstandard = "166000";
                    break;
                case "RDE组":
                    standard = "24万";
                    intstandard = "240000";
                    break;
                default:
                    standard = "";
                    intstandard = "";
                    break;
            }

            int daycount = returnDays();
            if (daycount <= 0)
            {
                MessageBox.Show("日期筛选不合规范");
                return;
            }          

            if (department == "PVE组")
            {
                actually.Text = "";
                standarded.Text = "无标准   ";
                progressBarControl1.Position = 0;
                return;
            }

            standarded.Text = $"100% {standard}(标准)";
            if (department == "耐久组")
            {
                
                Series s1 = this.chartControl1.Series[0];
                DataTable dt = new DataTable();
                dt.Columns.Add("drum", typeof(string));
                dt.Columns.Add("count", typeof(double));
                string drumsql = "select distinct drum  from tasktable where department ='耐久组'";
                DataTable drumda = data.ExecuteQuery(drumsql).Tables[0];
                foreach (DataRow row in drumda.Rows)
                {
                    var druminfo = row[0].ToString();
                    if (druminfo != "")
                    {
                        if(druminfo == "耐久20"|| druminfo == "耐久21"|| druminfo == "耐久9")
                        {
                            dt.Rows.Add(druminfo+"(EV)", naijiuratio(druminfo));
                        }
                        else
                        {
                            dt.Rows.Add(druminfo, naijiuratio(druminfo));
                        }
                       
                    }
                   
                }
                s1.DataSource = null;
                s1.ArgumentDataMember = "drum";//绑定图表的横坐标
                s1.ValueDataMembers[0] = "count"; //绑定图表的纵坐标坐标
                s1.LegendText = "负荷比例(百分比)";//设置图例文字 就是右上方的小框框
                s1.DataSource = dt;
            }
            else
            {
                string sql = $"select sum(Price) from tasktable where department = '{department}' and TestStartDate<= '{dateTimePicker2.Value}' and TestStartDate>= '{dateTimePicker1.Value}'";
                DataTable da = data.ExecuteQuery(sql).Tables[0];
                
                
                realvalue = da.Rows[0][0].ToString();

                
                if (realvalue == "" || standard == "")
                {
                    scale = "0%";
                    realvalue = "0";
                }
                else
                {
                    scale = ((Convert.ToDouble(realvalue) / daycount) / Convert.ToDouble(intstandard)).ToString("0.#%");
                    realvalue = ((int)(Convert.ToDouble(realvalue) / daycount)).ToString();
                }

                actually.Text = $"{scale} {realvalue}(实际)";
                if (scale == "0")
                {
                    progressBarControl1.Position = 0;
                    actually.Location= new Point(148, 305);
                    
                    actually.Visible = true;
                }
                else
                {
                    progressBarControl1.Position = (int)(Convert.ToDouble(scale.Substring(0, scale.Length - 1)) / 1.2);
                    int y = 305 - (int)((int)(Convert.ToDouble(scale.Substring(0, scale.Length - 1)))*1.833);
                   
                    actually.Location = new Point(148, y);
                    if (142 < y &&y<=158)
                    {
                        actually.Location = new Point(148, 180);
                    }else if (y>158&&y<174)
                    {
                        actually.Location = new Point(148, 210);
                    }

                    if (y <= 85)
                    {
                        actually.Location = new Point(148, 85);
                    }
                    
                }
                    
            }
                
       }


       
        /// <summary>
        /// 更改日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectdate_Click(object sender, EventArgs e)
        {
            if (department == "报告组")
            {
                reportratio();
                return;
            }
            if (department == "盐城和团泊")
            {
                LoadSafeInfo(department);
                return;
            }
            if (department == "耐久组")
            {
                shownaijiuinfo();
            }
            else
            {
                ShowAllinfo();
            }
            progressLoad(department);
        }

        /// <summary>
        /// 获取工作日天数
        /// </summary>
        /// <returns></returns>
        private int returnDays()
        {
            DateTime start = Convert.ToDateTime(dateTimePicker1.Value);
            DateTime end = Convert.ToDateTime(dateTimePicker2.Value);
            TimeSpan span = end - start;


            //int totleDay=span.Days;
            //DateTime spanNu = DateTime.Now.Subtract(span);
            int AllDays = Convert.ToInt32(span.TotalDays) + 1;//差距的所有天数

           
            int totleWeek = AllDays / 7;//差别多少周
            int yuDay = AllDays % 7; //除了整个星期的天数
            int lastDay = 0;
            if (yuDay == 0) //正好整个周
            {
                lastDay = AllDays - (totleWeek * 2);
            }
            else
            {
                int weekDay = 0;
                int endWeekDay = 0; //多余的天数有几天是周六或者周日
                switch (start.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        weekDay = 1;
                        break;
                    case DayOfWeek.Tuesday:
                        weekDay = 2;
                        break;
                    case DayOfWeek.Wednesday:
                        weekDay = 3;
                        break;
                    case DayOfWeek.Thursday:
                        weekDay = 4;
                        break;
                    case DayOfWeek.Friday:
                        weekDay = 5;
                        break;
                    case DayOfWeek.Saturday:
                        weekDay = 6;
                        break;
                    case DayOfWeek.Sunday:
                        weekDay = 7;
                        break;
                }
                if ((weekDay == 6 && yuDay >= 2) || (weekDay == 7 && yuDay >= 1) || (weekDay == 5 && yuDay >= 3) || (weekDay == 4 && yuDay >= 4) || (weekDay == 3 && yuDay >= 5) || (weekDay == 2 && yuDay >= 6) || (weekDay == 1 && yuDay >= 7))
                {
                    endWeekDay = 2;
                }
                if ((weekDay == 6 && yuDay < 1) || (weekDay == 7 && yuDay < 5) || (weekDay == 5 && yuDay < 2) || (weekDay == 4 && yuDay < 3) || (weekDay == 3 && yuDay < 4) || (weekDay == 2 && yuDay < 5) || (weekDay == 1 && yuDay < 6))
                {
                    endWeekDay = 1;
                }
                lastDay = AllDays - (totleWeek * 2) - endWeekDay;

                if (end.DayOfWeek == DayOfWeek.Saturday)
                {
                    lastDay = lastDay - 1;
                }else if (end.DayOfWeek == DayOfWeek.Sunday)
                {
                    
                }
                else
                {
                    lastDay = lastDay +1;
                }
            }
            
            return lastDay;
        }

        //报告组
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("注意：报告组统计的是第一个时间筛选框所在的年份和月份");
            department = "报告组";
            gridControl4.Visible = false;
            gridControl5.Visible = false;
            calcmethod.Visible = true;
            calcmethod.Text = "负荷比例：每月出具报告量/标准负荷值\r\n标准负荷值：210份 / 人 / 月（2019年1月至2020年6月 每人月平均报告出具量）\r\n注：负荷比例未包含报告出具外工作负荷";
            reportratio();


        }
        /// <summary>
        /// 报告组负荷
        /// </summary>
        private void reportratio()
        {

            panel1.Visible = false;
            splitContainer1.Visible = false;
            toggle.Visible = false;
           
            string year = dateTimePicker1.Value.Year.ToString().Substring(2, 2);
            string month = dateTimePicker1.Value.Month.ToString();
            if (month != "10" && month != "11" && month != "12")
            {
                month = "0" + month;
            }
            string date = year + month;
            label4.Text = department + "(" + date + ")";
            chartControl1.Visible = true;

            DataControl data = new DataControl();
            Series s1 = this.chartControl1.Series[0];
            DataTable dt = new DataTable();
            dt.Columns.Add("reporter", typeof(string));
            dt.Columns.Add("count", typeof(double));
            string drumsql = "select distinct Reporter from ReportTable";
            DataTable drumda = data.ExecuteQuery(drumsql).Tables[0];
            foreach (DataRow row in drumda.Rows)
            {
                if (row[0].ToString() != "")
                {
                    dt.Rows.Add(row[0].ToString(), reportdata(row[0].ToString()));
                }

            }
            s1.DataSource = null;
            s1.ArgumentDataMember = "reporter";//绑定图表的横坐标
            s1.ValueDataMembers[0] = "count"; //绑定图表的纵坐标坐标
            s1.LegendText = "负荷比例(百分比)";//设置图例文字 就是右上方的小框框
            s1.DataSource = dt;
        }

        private double reportdata(string reporter)
        {
            string year = dateTimePicker1.Value.Year.ToString().Substring(2,2);
            string month = dateTimePicker1.Value.Month.ToString();
            if (month != "10" && month !="11" && month != "12")
            {
                month = "0" + month;
            }
            string date = year + month;
            string num = "";
            double scale = 0;
            string sql = $"select SUM(CASE WHEN ISNUMERIC(Reportnum)=1 THEN CAST(Reportnum as float) ELSE 0 END) from ReportTable where reporter='{reporter}' and Finishmonth ='{date}' group by Reporter  ";
            DataTable da = data.ExecuteQuery(sql).Tables[0];
            if (da.Rows.Count != 0)
            {
                num = da.Rows[0][0].ToString();
            }
            else
            {
                num = "0";
            }
            scale = Convert.ToDouble(num) / 210;


            return Math.Round(scale * 100, 1);
        }

        /// <summary>
        /// 耐久负荷
        /// </summary>
        private double naijiuratio(string drum)
        {
            string naijiuelectrocar = "15小时";
            string naijiutime = "20小时";
            string naijiudistance = "1400km";
            string intdistance = "1400";
            double scale = 0;
            
            int daycount = returnDays();          
            double scale1 = 0, scale2 = 0;
            double value1 = 0;
            double value2 = 0;
            //时间不为空的情况
            string sql1 = $"select SUM(CASE WHEN ISNUMERIC(TestTimeSpan)=1 THEN CAST(TestTimeSpan as float) ELSE 0 END) from tasktable where TestTimeSpan<>'0' and  ISNUMERIC(TestTimeSpan)=1 and drum='{drum}' and department = '{department}' and TestStartDate<= '{dateTimePicker2.Value}' and TestStartDate>= '{dateTimePicker1.Value}' group by drum";
            DataTable da1 = data.ExecuteQuery(sql1).Tables[0];

            if (da1.Rows.Count != 0 && da1.Rows[0][0] != null)
            {
                value1 = Convert.ToDouble(da1.Rows[0][0]);
            }

            //时间为空的情况
            string sql0 = $"select SUM(CASE WHEN ISNUMERIC(TotalMileage)=1 THEN CAST(TotalMileage as float) ELSE 0 END) from tasktable where (TestTimeSpan='0' or  ISNUMERIC(TestTimeSpan)=0) and drum='{drum}' and department = '{department}' and TestStartDate<= '{dateTimePicker2.Value}' and TestStartDate>= '{dateTimePicker1.Value}' group by drum";
            DataTable da0 = data.ExecuteQuery(sql0).Tables[0];
            if (da0.Rows.Count != 0 && da0.Rows[0][0] != null)
            {
                value2 = Convert.ToDouble(da0.Rows[0][0]);
            }

            //电车
            if (drum == "耐久20" || drum == "耐久21" || drum == "耐久9")
            {
                scale1 = ((value1 / daycount) / Convert.ToDouble(naijiuelectrocar.Substring(0, naijiuelectrocar.Length - 2)));

            }
            //传统车
            else
            {
                scale1 = ((value1 / daycount) / Convert.ToDouble(naijiutime.Substring(0, naijiutime.Length - 2)));

            }
            scale2 = ((value2 / daycount) / Convert.ToDouble(intdistance));
            scale = scale1 + scale2;

            return Math.Round(scale*100,1);
        }

        /// <summary>
        /// 耐久切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggle_Click(object sender, EventArgs e)
        {
            if (toggle.Text == "试验室负荷")
            {
                chartControl1.Visible = true;
                panel1.Visible = false;
                splitContainer1.Visible = false;
                toggle.Text = "试验情况";
                calcmethod.Visible = true;
                gridControl4.Visible = false;
            }
            else
            {
                chartControl1.Visible = false;
                panel1.Visible = false;
                splitContainer1.Visible = true;
                toggle.Text = "试验室负荷";
                calcmethod.Visible = false;
                gridControl4.Visible = true;

            }
        }

        /// <summary>
        /// 盐城和团泊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            label4.Text = "盐城和团泊";
            department = "盐城和团泊";
            LoadSafeInfo(department);
            splitContainer1.Visible = false;
            panel1.Visible = false;
            toggle.Visible = false;
            calcmethod.Visible = false;
            chartControl1.Visible = false;
            gridControl4.Visible = false;
            gridControl5.Visible = true;
            

        }

        
    }
};