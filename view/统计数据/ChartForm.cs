using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraCharts;

namespace TaskManager.统计
{
    public partial class ChartForm : DevExpress.XtraEditors.XtraForm
    {
        public ChartForm()
        {
            InitializeComponent();
            LoadAll("按部门","2020");
        }
        /// <summary>
        /// 默认加载数据
        /// </summary>
        public void LoadAll(string type,string year,string startdate="",string enddate="")
        {   //Series  对象表示数据系列，并且存储在 SeriesCollection 类中。
            Series s1 = this.chartControl1.Series[0];//新建一个series类并给控件赋值
            Series s2 = this.chartControl2.Series[0];
            if (type == "按部门")
            {
                s1.DataSource = GetDepartmentCount(year,1,startdate,enddate);//设置实例对象s1的数据源
                s2.DataSource = GetDepartmentCount(year,2,startdate, enddate);
            }
            else if(type == "按认证项目类型")
            {
                s1.DataSource = GetYItemCount(year, 1, startdate, enddate);
                s2.DataSource = GetYItemCount(year, 2, startdate, enddate);
            }else if (type=="按非认证项目类型")
            {
                s1.DataSource = GetNItemCount(year, 1, startdate, enddate);
                s2.DataSource = GetNItemCount(year, 2, startdate, enddate);
            }else if (type == "按设备")
            {
                s1.DataSource = GetEquipCount(year, 1, startdate, enddate);
                s2.DataSource = GetEquipCount(year, 2, startdate, enddate);
            }
            else if (type=="按月份")
            {
                if (startdate != "" && enddate != "")
                {
                    MessageBox.Show("请切换展示类型");
                    return;
                }
                s1.DataSource = GetMonthCount(year,1);
                s2.DataSource = GetMonthCount(year,2);
            }
          
            s1.ArgumentDataMember = "group";//绑定图表的横坐标
            s2.ArgumentDataMember = "group";
            s1.ValueDataMembers[0] = "count"; //绑定图表的纵坐标坐标
            s2.ValueDataMembers[0] = "count"; 
            s1.LegendText = "总产值";//设置图例文字 就是右上方的小框框
            s2.LegendText = "总车数";
        }

        private void ChartForm_Load(object sender, EventArgs e)
        {
            this.comboBoxEdit1.SelectedIndex = 0;
            this.cbotype1.SelectedIndex = 0;
            this.cboyear1.SelectedIndex = 0;
        }



        #region 按部门
        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="year"></param>
        /// <param name="index">车数或价格</param>
        /// <returns></returns>
        public static DataTable GetDepartmentCount(string year,int index,string startdate, string enddate)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("group", typeof(string));
            dt.Columns.Add("count", typeof(int));
            dt.Rows.Add("常温组", Getdepartment("常温组",year,index,startdate, enddate));
            dt.Rows.Add("海拔组", Getdepartment("海拔组",year,index,startdate, enddate));
            dt.Rows.Add("低温组", Getdepartment("低温组",year,index,startdate, enddate));
            dt.Rows.Add("蒸发组", Getdepartment("蒸发组",year,index,startdate, enddate));
            dt.Rows.Add("PVE", Getdepartment("PVE组",year,index, startdate, enddate));
            dt.Rows.Add("RDE", Getdepartment("RDE组",year,index, startdate, enddate));
            dt.Rows.Add("耐久组", Getdepartment("耐久组",year,index, startdate, enddate));
            return dt;
        }
     
        public static int Getdepartment(string department,string year,int index,string startdate, string enddate)
        {
            
            DataControl data = new DataControl();
            string sql1 = "";
            if (startdate != "" && enddate != "")
            {
                if (index == 1)
                    sql1 = "select SUM(price) as totalprice from TaskTable where department ='" + department + "' and  TestStartDate<= '" + enddate + "' and TestStartDate>='" + startdate + "'";
                else
                    sql1 = "select COUNT(distinct(Carvin)) as count from TaskTable where department ='" + department + "' and  TestStartDate<= '" + enddate + "' and TestStartDate>='" + startdate + "'";
            }
            else
            {
                if (index == 1)
                    sql1 = "select SUM(price) as totalprice from TaskTable where department ='" + department + "' and TestStartDate like '" + year + "%'";
                else
                    sql1 = "select COUNT(distinct(Carvin)) as count from TaskTable where department ='" + department + "' and TestStartDate like '" + year + "%'";
            }           
            DataTable dt = data.ExecuteQuery(sql1).Tables[0];
            if (dt.Rows[0][0].ToString()!=""){
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            else
            {
                return 0;
            }           
        }
        #endregion

        #region 按项目类型

        /// <summary>
        /// 认证
        /// </summary>
        /// <param name="year"></param>
        /// <param name="index"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static DataTable GetYItemCount(string year,int index, string startdate, string enddate)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("group", typeof(string));
            dt.Columns.Add("count", typeof(int));
            dt.Rows.Add("出口", GetYitem("出口",year,index,startdate,enddate));
            dt.Rows.Add("公告", GetYitem("公告", year, index, startdate, enddate));
            dt.Rows.Add("商检", GetYitem("商检", year, index, startdate, enddate));
            dt.Rows.Add("环保", GetYitem("环保", year, index, startdate, enddate));
            dt.Rows.Add("委托", GetYitem("委托", year, index, startdate, enddate));
            return dt;
        }

        /// <summary>
        /// 非认证
        /// </summary>
        /// <param name="year"></param>
        /// <param name="index"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static DataTable GetNItemCount(string year, int index, string startdate, string enddate)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("group", typeof(string));
            dt.Columns.Add("count", typeof(int));
            dt.Rows.Add("CCRT", GetYitem("CCRT", year, index, startdate, enddate));
            dt.Rows.Add("EV-TEST", GetYitem("EV-TEST", year, index, startdate, enddate));
            dt.Rows.Add("比对试验", GetYitem("比对试验", year, index, startdate, enddate));
            dt.Rows.Add("海拔测试", GetYitem("海拔测试", year, index, startdate, enddate));
            dt.Rows.Add("低温冷启动", GetYitem("低温冷启动", year, index, startdate, enddate));
            dt.Rows.Add("环境模式", GetYitem("环境模式", year, index, startdate, enddate));
            return dt;
        }

        public static int GetYitem(string item, string year, int index, string startdate, string enddate)
        {
            DataControl data = new DataControl();
            string sql1 = "";
            if (startdate != "" && enddate != "")
            {
                if (index == 1)
                    sql1 = "select SUM(price) as totalprice from TaskTable where type1 ='" + item + "' and  TestStartDate<= '" + enddate + "' and TestStartDate>='" + startdate + "'";
                else
                    sql1 = "select COUNT(distinct(Carvin)) as count from TaskTable where type1 ='" + item + "' and  TestStartDate<= '" + enddate + "' and TestStartDate>='" + startdate + "'";
            }
            else
            {
                if (index == 1)
                    sql1 = "select SUM(price) as totalprice from TaskTable where type1 ='" + item + "' and TestStartDate like '" + year + "%'";
                else
                    sql1 = "select COUNT(distinct(Carvin)) as count from TaskTable where type1 ='" + item + "' and TestStartDate like '" + year + "%'";
            }
            DataTable dt = data.ExecuteQuery(sql1).Tables[0];
            if (dt.Rows[0][0].ToString() != "")
            {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }


        #endregion

        #region 按月份
        public static DataTable GetMonthCount(string year,int index)
        {
            DataTable dt = new DataTable();
            int nowmonth = 12;
            if (year == DateTime.Now.ToString("yyyy"))
                nowmonth = int.Parse(DateTime.Now.ToString("MM"));
            
            dt.Columns.Add("group", typeof(string));
            dt.Columns.Add("count", typeof(int));
            for (int i = 1; i <= nowmonth; i++)
            {

                dt.Rows.Add(i + "月", GetMonth(i, year,index));
            }
            return dt;
        }

        public static int GetMonth(int i,string year,int index)
        {
            string month = i.ToString();
            if (i <= 10)
                month = "0" + i.ToString();
            string date = year + "-" + month;
            DataControl data = new DataControl();
            string sql1 = "";
            if (index==1)
                 sql1 = "select SUM(price) as totalprice from TaskTable where TestStartDate like '" + date + "%'";
            else
                sql1 = "select COUNT(distinct(Carvin)) as count  from TaskTable where TestStartDate like '" + date + "%'";

            DataTable dt = data.ExecuteQuery(sql1).Tables[0];
            if (dt.Rows[0][0].ToString() != "")
                return Convert.ToInt32(dt.Rows[0][0]);           
            else
                return 0;
        }
        #endregion


        #region 按设备

        public static DataTable GetEquipCount(string year, int index, string startdate, string enddate)
        {
            DataControl data = new DataControl();
            DataTable dt = new DataTable();

            string sql0= "select distinct(drum) from TaskTable where Drum <> ''";
            DataTable da = data.ExecuteQuery(sql0).Tables[0];
            dt.Columns.Add("group", typeof(string));
            dt.Columns.Add("count", typeof(int));
            foreach (DataRow row in da.Rows)
            {
                dt.Rows.Add(row[0].ToString(), GetEquip(row[0].ToString(),year,index,startdate,enddate));
            }           
            return dt;

           
        }

        public static int GetEquip(string equip, string year, int index, string startdate, string enddate)
        {

            DataControl data = new DataControl();
            string sql1 = "";
            if (startdate != "" && enddate != "")
            {
                if (index == 1)
                    sql1 = "select SUM(price) as totalprice from TaskTable where drum ='" + equip + "' and  TestStartDate<= '" + enddate + "' and TestStartDate>='" + startdate + "'";
                else
                    sql1 = "select COUNT(distinct(Carvin)) as count from TaskTable where drum ='" + equip + "' and  TestStartDate<= '" + enddate + "' and TestStartDate>='" + startdate + "'";
            }
            else
            {
                if (index == 1)
                    sql1 = "select SUM(price) as totalprice from TaskTable where drum ='" + equip + "' and TestStartDate like '" + year + "%'";
                else
                    sql1 = "select COUNT(distinct(Carvin)) as count from TaskTable where drum ='" + equip + "' and TestStartDate like '" + year + "%'";
            }
            DataTable dt = data.ExecuteQuery(sql1).Tables[0];
            if (dt.Rows[0][0].ToString() != "")
            {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }
        #endregion
        private void comboBoxEdit1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (this.comboBoxEdit1.Text == "--请选择图表类型--") return;
            if (this.comboBoxEdit1.Text == "柱形图")
            {
                StackedBarSeriesView stackedBarSeriesView1 = new StackedBarSeriesView();
                StackedBarSeriesView stackedBarSeriesView2 = new StackedBarSeriesView();
                this.chartControl1.Series[0].View = stackedBarSeriesView1;
                this.chartControl2.Series[0].View = stackedBarSeriesView2;
            }
            if (this.comboBoxEdit1.Text == "折线图")
            {
                LineSeriesView lineSeriesView1 = new LineSeriesView();
                LineSeriesView lineSeriesView2 = new LineSeriesView();
                this.chartControl1.Series[0].View = lineSeriesView1;
                this.chartControl2.Series[0].View = lineSeriesView2;

            }
        }

       


        /// <summary>
        /// 年份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboyear1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = this.cbotype1.Text;
            string year = this.cboyear1.Text;
            LoadAll(type,year);
        }

        /// <summary>
        /// 展示类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbotype1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string year = this.cboyear1.Text;
            string type = this.cbotype1.Text;
            LoadAll(type, year);
        }

        /// <summary>
        /// 上一周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string type = cbotype1.Text;
            string year = cboyear1.Text;
            int n = (int)DateTime.Now.DayOfWeek;
            string startDate = DateTime.Now.AddDays(-7 - n).ToString("yyyy-MM-dd");//上周第一天
            string endDate = DateTime.Now.AddDays(-n).ToString("yyyy-MM-dd");//上周最后一天
            LoadAll(type, year, startDate, endDate);
        }

        /// <summary>
        /// 上一月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string type = cbotype1.Text;
            string year = cboyear1.Text;
            int month = DateTime.Now.Month-1;
            int nowyear = DateTime.Now.Year;
            string startdate= DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01");
            string enddate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-31");
            if (month == 2&&nowyear%4==0)
                enddate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-29");
            if (month == 2 && nowyear % 4 != 0)
                enddate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-28");
            if (month==4||month==6||month==9||month==11)
                enddate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-30");
            LoadAll(type, year, startdate, enddate);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string type = cbotype1.Text;
            string year = cboyear1.Text;
            LoadAll(type, year, dateTimePicker1.Value.Date.ToString("yyyy-MM-dd"), dateTimePicker2.Value.Date.ToString("yyyy-MM-dd"));
        }
    }
}