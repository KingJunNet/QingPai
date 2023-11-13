using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Controls;

namespace TaskManager
{
    public partial class AgainEditDialog : BaseEditDialog
    {
        private readonly bool IsAllocateTask;
        
        private AgainEditDialog():base()
        {
            InitializeComponent();
        }

        public TestEditDialog(bool authorityEdit, GridView theView, int theHand, List<DataField> fields,
            bool isAllocateTask)
            : base(authorityEdit, theView, theHand, fields)
        {
            InitializeComponent();
            IsAllocateTask = isAllocateTask;
        }

        protected TitleCombox FinishState;
        protected TitleDate FinishMonth;
        protected override void BaseEditDialog_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            controls = GetAllControls(out Panel);
            var count = InitControls();

            #region 调整窗口大小

            var borderX = Size.Width - Panel.Width;
            var borderY = Size.Height - Panel.Height;

            const int len = 370 + 4;
            const int height = 30 + 4;
            var rowCount = (int)Math.Ceiling(count / 2.0) + 1;

            var w = len * 2 + Panel.Margin.Left + Panel.Margin.Right + borderX;
            var h = rowCount * height + Panel.Margin.Top + Panel.Margin.Bottom + borderY;
            Size = new Size(w, h);

            #endregion

            #region 事件注册

            //// 认证/非认证 显示状态切换
            //if (GetControlByFieldName("certification") is TitleCombox certification)
            //{
            //    certification.SetTextChange(CertificationOnTextChanged);
            //    CertificationOnTextChanged(certification.comboBox1, null);
            //}

            //// 总里程
            //if (GetControlByFieldName("StartMileage") is TitleTextBox StartMileage &&
            //    GetControlByFieldName("EndMileage") is TitleTextBox EndMileage)
            //{
            //    StartMileage.SetTextChange(MileageHandler);
            //    EndMileage.SetTextChange(MileageHandler);
            //    MileageHandler(null, null);
            //}

            ////自动生成报告员
            //if(GetControlByFieldName("Taskcode") is TitleCombox Taskcode && GetControlByFieldName("Reporter") is TitleCombox Reporter)
            //{
            //    Taskcode.SetTextChange(TaskcodeHandle);
            //    TaskcodeHandle(null,null);
            //

            if (GetControlByFieldName("TestStartDate") is TitleCombox StartTime &&
                GetControlByFieldName("TestEndDate") is TitleCombox EndTime &&
                GetControlByFieldName("Testtime") is TitleTextBox)
            {
                StartTime.SetTextChange(TimeSpanHandler);
                EndTime.SetTextChange(TimeSpanHandler);
                TimeSpanHandler(null, null);
            }

            if ( GetControlByFieldName("FinishDate") is TitleDate finishMonth &&
             GetControlByFieldName("State") is TitleCombox state)
            {
                FinishMonth = finishMonth;
                FinishState = state;
                FinishState.SetTextChange(FinishStateChangeHandler);
                FinishStateChangeHandler(null, null);
            }

            #endregion

            flowLayoutPanel1.ResumeLayout(true);
            ResumeLayout(true);
            Log.e("BaseEditDialog_Load");
        }


        private void FinishStateChangeHandler(object sender, EventArgs e)
        {



            if (!FinishState.Value().Trim().Equals("已完成"))
            {
                FinishMonth.SetValue("");
                return;
            }
            else
            {
                string month = DateTime.Now.ToString("yyyy-MM-dd");

                FinishMonth.SetValue(month);
            }

            //if (string.IsNullOrWhiteSpace(PlanMonth.Value()))
            //    return;

        }

        protected override bool FieldVisible(DataField field)
        {
            if (!IsAllocateTask)
                return field.EditorVisible;
            else
                return field.EditorVisible && field.Group.Equals("分配试验任务");
        }

        private void CertificationOnTextChanged(object sender, EventArgs e)
        {
            if (!(sender is ComboBox combox))
                return;

            var value = combox.Text.Trim().Replace(" ", "");
            var bl = value.Equals("yes", StringComparison.CurrentCultureIgnoreCase) ||
                     value.Equals("是") ||
                     value.Equals("y") ||
                     value.Equals("1");

            var fields1 = bl // read only 
                ? Fields.Where(f => f.Group.Equals("非认证")).ToList()
                : Fields.Where(f => f.Group.Equals("认证")).ToList();

            var fields2 = !bl
                ? Fields.Where(f => f.Group.Equals("非认证")).ToList()
                : Fields.Where(f => f.Group.Equals("认证")).ToList();

            foreach (var field in fields1)
            {
                var control = GetControlByFieldName(field.Eng);
                control?.SetReadOnly(true);
            }

            foreach (var field in fields2)
            {
                var control = GetControlByFieldName(field.Eng);
                control?.SetReadOnly(control.OriginalReadOnly);
            }
        }

      
        private void MileageHandler(object sender, EventArgs e)
        {
           
            if (!(GetControlByFieldName("StartMileage") is TitleTextBox StartMileage) ||
                !(GetControlByFieldName("EndMileage") is TitleTextBox EndMileage) ||
                !(GetControlByFieldName("TotalMileage") is TitleTextBox TotalMileage)||
                !(GetControlByFieldName("Carvin") is TitleCombox VIN)||
                !(GetControlByFieldName("TestStartDate") is TitleDate TestStartDate) ||
                !(GetControlByFieldName("department") is TitleCombox department))
                return;
            if (!department.Value().Equals("耐久组"))
            {
                double.TryParse(StartMileage.Value(), out var start);
                var startMil = OtherMileageRight(department.Value(), VIN.Value(), TestStartDate.Date, start);
                if (!startMil)
                    Text = " 同一VIN不同试验，日期靠后的里程应大于日期靠前的里程";
                else
                    Text = "编辑";
            }
            else
            {
                double.TryParse(StartMileage.Value(), out var start);
                double.TryParse(EndMileage.Value(), out var end);
                var total = end > start ? end - start : 0;
                TotalMileage.SetValue(total.ToString());

                var endMil = end >= start;
                var startMil = IsStartMileageRight(department.Value(), VIN.Value(), TestStartDate.Date, start);

                btnUpdate.Enabled = startMil & endMil;
                Text = btnUpdate.Enabled ? "编辑" : "里程不合理";

                if (!endMil)
                    Text += "结束里程应大于开始里程";
                if (!startMil)
                    Text += " 同一VIN不同试验，日期靠后的开始里程应大于日期靠前的结束里程";
            }
               
            
        }


        /// <summary>
        /// 耐久组
        /// </summary>
        /// <param name="department"></param>
        /// <param name="vin"></param>
        /// <param name="startDate"></param>
        /// <param name="startMileage"></param>
        /// <returns></returns>
        public static bool IsStartMileageRight(string department,string vin,DateTime startDate,double startMileage)
        {
            if (string.IsNullOrWhiteSpace(vin) || !department.Equals("耐久组"))
                return true;
            var strsql = $" select * from TaskTable where  carvin='{vin}' " + $" and ISNUMERIC(EndMileage)=1 and EndMileage>{startMileage} and TestStartDate<@startDate";

                var sql = new DataControl();
              
                var dt = sql.ExecuteQuery(strsql,new[] {
                    new SqlParameter("startDate",startDate)
                });
            return dt.Tables[0].Rows.Count == 0;
            
        }
        /// <summary>
        /// 其他组
        /// </summary>
        /// <param name="department"></param>
        /// <param name="vin"></param>
        /// <param name="startDate"></param>
        /// <param name="startMileage"></param>
        /// <returns></returns>
        public static bool OtherMileageRight(string department, string vin, DateTime startDate, double startMileage)
        {
            if (string.IsNullOrWhiteSpace(vin))
                return true;
            var strsql = $" select * from TaskTable where  carvin='{vin}' " + $" and  startMileage>{startMileage} and TestStartDate<@startDate";

            var sql = new DataControl();

            var dt = sql.ExecuteQuery(strsql, new[] {
                    new SqlParameter("startDate",startDate)
                });
            return dt.Tables[0].Rows.Count == 0;

        }


        /// <summary>
        /// 时间自动计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeSpanHandler(object sender, EventArgs e)
        {
            if (!(GetControlByFieldName("TestStartDate") is TitleCombox StartTime) ||
                !(GetControlByFieldName("TestEndDate") is TitleCombox EndTime) ||
                !(GetControlByFieldName("Testtime") is TitleTextBox TimeSpan)) return;
            var start = StartTime.Value().Replace("：",":");
            var end = EndTime.Value().Replace("：", ":");

            if (!DateTime.TryParse(start, out var startTime) || !DateTime.TryParse(end, out var endTime)) 
                return;

            var span = endTime - startTime;
            TimeSpan.SetValue(span.TotalHours.ToString());

        }

        private void TaskEditDialog_Shown(object sender, EventArgs e)
        {
            Log.e("TaskEditDialog_Shown");
          
        }

        //自动生成报告员
        private void TaskcodeHandle(object sender,EventArgs e)
        {
            if (!(GetControlByFieldName("Taskcode") is TitleCombox Taskcode) || !(GetControlByFieldName("Reporter") is TitleCombox Reporter))
                return;
            var taskcode = Taskcode.Value().ToString();
            var reporter = Reporter.Value().ToString();
            string sql = $"select Reporter from ReportTable where taskcode = '{taskcode}'";
            DataControl data = new DataControl();
            DataTable dt = data.ExecuteQuery(sql).Tables[0];
            if (dt.Rows.Count != 0 && dt.Rows[0][0] != null)
            {
                Reporter.SetValue(dt.Rows[0][0].ToString());
            }
            
        }
    }
}
