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
using ExpertLib.Controls.TitleEditor;
using LabSystem.DAL;

namespace TaskManager
{
    public partial class AddTestForm : BaseEditDialog
    {
        private readonly bool IsAllocateTask;
        
        private AddTestForm():base()
        {
            InitializeComponent();
        }

        public AddTestForm(bool authorityEdit, GridView theView, int theHand, List<DataField> fields, FormType Type1,
            bool isAllocateTask)
            : base(authorityEdit, theView, theHand, fields,Type1)
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

            //var borderX = Size.Width - Panel.Width;
            //var borderY = Size.Height - Panel.Height;

            //const int len = 370 + 4;
            //const int height = 30 + 4;
            //var rowCount = (int)Math.Ceiling(count / 2.0) + 1;

            //var w = len * 2 + Panel.Margin.Left + Panel.Margin.Right + borderX;
            //var h = rowCount * height + Panel.Margin.Top + Panel.Margin.Bottom + borderY;
            //Size = new Size(w, h);

            #endregion

            #region 事件注册



            // 总里程
            if (GetControlByFieldName("StartMileage") is TitleCombox StartMileage &&
                GetControlByFieldName("EndMileage") is TitleCombox EndMileage)
            {
                StartMileage.SetTextChange(MileageHandler);
                EndMileage.SetTextChange(MileageHandler);
                MileageHandler(null, null);
            }


            //费用总计
            if (GetControlByFieldName("ProjectPrice") is TitleCombox ProjectPrice &&
                GetControlByFieldName("ProjectTotal") is TitleCombox ProjectTotal)
            {
                ProjectPrice.SetTextChange(ProjectPriceHandler);

                ProjectPriceHandler(null, null);
            }


            if (GetControlByFieldName("TestStartDate") is DateEdit  StartTime &&
                GetControlByFieldName("TestEndDate") is DateEdit EndTime &&
                GetControlByFieldName("Testtime") is TitleCombox)
            {
                StartTime.SetValueChange(TimeSpanHandler);
                EndTime.SetValueChange(TimeSpanHandler);
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


        private void ProjectPriceHandler(object sender, EventArgs e)
        {

            if (!(GetControlByFieldName("ProjectPrice") is TitleCombox ProjectPrice) ||
              !(GetControlByFieldName("ProjectTotal") is TitleCombox ProjectTotal))
                return;
            if (ProjectTotal.Value() == "")
            {
                ProjectTotal.SetValue(ProjectPrice.Value().ToString());
            }
            

        }


        private void MileageHandler(object sender, EventArgs e)
        {
           
            if (!(GetControlByFieldName("StartMileage") is TitleCombox StartMileage) ||
                !(GetControlByFieldName("EndMileage") is TitleCombox EndMileage) ||
                !(GetControlByFieldName("TotalMileage") is TitleCombox TotalMileage))
                return;
 
                double.TryParse(StartMileage.Value(), out var start);
                double.TryParse(EndMileage.Value(), out var end);
                var total = end > start ? end - start : 0;
                TotalMileage.SetValue(total.ToString());

       
               
            
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
            if (!(GetControlByFieldName("TestStartDate") is DateEdit StartTime) ||
                !(GetControlByFieldName("TestEndDate") is DateEdit EndTime) ||
                !(GetControlByFieldName("Testtime") is TitleCombox TimeSpan)) return;

            if (EndTime.Value() == null)
            {
                return;
            }
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


        protected override void BtnUpdateClick(object sender, EventArgs e)
        {

            var taskcode0 = GetControlByFieldName("Taskcode").Value().Trim();
            var SampleModel0 = GetControlByFieldName("SampleModel").Value().Trim();
            if (taskcode0 != "")
            {
                string sqlmoneysure = $"select count(*) from TestStatistic where Taskcode='{taskcode0}' and MoneySure ='是'";
                if (SqlHelper.GetList(sqlmoneysure).Rows[0][0].ToString() != "0" && !taskcode0.Contains("?"))
                {

                    if (MessageBox.Show("该任务单号费用已确认，是否重新填写！", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    {
                        GetControlByFieldName("Taskcode").SetValue(taskcode0 + "?");
                    }
                    else
                    {
                        return;
                    };
                    
                }

                string sqlcartype = $"select count(*) from NewTaskTable where Taskcode='{taskcode0}' and Model ='{SampleModel0}'";
                if (SqlHelper.GetList(sqlcartype).Rows[0][0].ToString() == "0" && !taskcode0.Contains("?"))
                {

                    if (MessageBox.Show("该任务单号与车型不一致，是否重新填写！", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    {
                        GetControlByFieldName("Taskcode").SetValue(taskcode0 + "?");
                    }
                    else
                    {
                        return;
                    };
         
                }
            }


            //判断登记日期
       
          

            foreach (var control in controls)
            {
                var fieldName = control.Value.Tag?.ToString();
                if (string.IsNullOrWhiteSpace(fieldName))
                    continue;

                var value = control.Value.Value();
                view.SetRowCellValue(hand, fieldName, value);
            }
            view.FocusedRowHandle = hand + 1;
            view.FocusedRowHandle = hand; //防止出错

            //string taskcode = view.GetRowCellValue(hand, "Taskcode").ToString();
            //if (btnUpdate.Text == "新增")
            //{
            //    view.SetRowCellValue(hand, "RegistrationDate", $"{DateTime.Now:yyyy/MM/dd HH:mm}");
            //}
                //自动更新保密级别    
                if (taskcode0 != "")
                {
                    string sql = $"select SecurityLevel from NewTaskTable where Taskcode='{taskcode0}'";
                    if (SqlHelper.GetList(sql).Rows.Count > 0)
                    {
                        view.SetRowCellValue(hand, "Confidentiality", SqlHelper.GetList(sql).Rows[0][0].ToString());
                    }
                }

                //string sql2 = $"update TaskManager.dbo.TaskTable set MoneySure='{view.GetRowCellValue(hand, "MoneySure").ToString()}' where TaskTable.Taskcode='{view.GetRowCellValue(hand, "Taskcode").ToString()}'";
                //SqlHelper.ExecuteNonquery(sql2, CommandType.Text);

            //}
           





                DialogResult = DialogResult.OK;
            Close();


        }


        private void AddTestForm_Load(object sender, EventArgs e)
        {
            string vin = view.GetRowCellValue(hand, "Carvin").ToString();

            string sql0 = $"select NationalFive,NationalSix from teststatistic where Carvin ='{vin}'";
            List<string> list1 = new List<string>();
            //list1 = null;
            List<string> list2 = new List<string>();
            //list2 = null;
            foreach (DataRow row in SqlHelper.GetList(sql0).Rows)
            {
                if (row[0].ToString() != "")
                {
                    list1.Add(row[0].ToString());
                }
                if (row[1].ToString() != "")
                {
                    list2.Add(row[1].ToString());
                }
                

            }
            if (list1 != null)
            {
                titleCombox25.SetItems(list1);
            }
            if (list2 != null)
            {
                titleCombox26.SetItems(list2);
            }


            string SampleModel = view.GetRowCellValue(hand, "SampleModel").ToString();

            string sql1 = $"select taskcode from newtasktable where model ='{SampleModel}'";
            List<string> list3 = new List<string>();
          
            foreach (DataRow row in SqlHelper.GetList(sql1).Rows)
            {
                if (row[0].ToString() != "")
                {
                    list3.Add(row[0].ToString());
                }
               

            }
            if (list3 != null)
            {
                titleCombox11.SetItems(list3);
            }

            //查询试验地点和定位编号
            string sqlnum = $"select Experimentsite,Locationnumber from UserStructure where  Username like '%{FormSignIn.CurrentUser.Name.ToString()}%'";
            DataTable danum = SqlHelper.GetList(sqlnum);
            if (danum.Rows.Count > 0)
            {
                List<string> listsite = new List<string>();
                List<string> listnumber = new List<string>();
                foreach (DataRow row in danum.Rows)
                {
                    if (row[0].ToString() != "" && !listsite.Contains(row[0].ToString()))
                    {
                        listsite.Add(row[0].ToString());
                    }
                    if (row[1].ToString() != "" && !listnumber.Contains(row[1].ToString()))
                    {
                        listnumber.Add(row[1].ToString());
                    }
                }
                titleCombox3.SetItems(listsite);
                titleCombox1.SetItems(listnumber);
                titleCombox3.SetValue(listsite[0].ToString());
                titleCombox1.SetValue(listnumber[0].ToString());


            }


            

            if (FormSignIn.CurrentUser.Department == "系统维护"){
                return;
            }
            if (FormSignIn.CurrentUser.Department != "体系组")
            {
                titleCombox41.SetReadOnly(true);
            }

            if (FormSignIn.CurrentUser.Department == "体系组")
            {
                titleCombox1.SetReadOnly(true);
                titleCombox2.SetReadOnly(true);
                titleCombox3.SetReadOnly(true);
                titleCombox40.SetReadOnly(true);

                titleCombox7.SetReadOnly(true);
                dateEdit1.SetReadOnly(true);
                dateEdit2.SetReadOnly(true);
                titleCombox9.SetReadOnly(true);
                titleCombox10.SetReadOnly(true);
                titleCombox11.SetReadOnly(true);

                titleCombox4.SetReadOnly(true);
                titleCombox5.SetReadOnly(true);
                titleCombox8.SetReadOnly(true);
                titleCombox12.SetReadOnly(true);
                titleCombox13.SetReadOnly(true);
                titleCombox33.SetReadOnly(true);
                titleCombox17.SetReadOnly(true);

                titleCombox38.SetReadOnly(true);
                titleCombox22.SetReadOnly(true);

                titleCombox24.SetReadOnly(true);
                titleCombox25.SetReadOnly(true);
                titleCombox26.SetReadOnly(true);
                titleCombox27.SetReadOnly(true);
                titleCombox28.SetReadOnly(true);
                titleCombox29.SetReadOnly(true);
                titleCombox30.SetReadOnly(true);
                titleCombox31.SetReadOnly(true);
                titleCombox32.SetReadOnly(true);
                titleCombox34.SetReadOnly(true);
                titleCombox35.SetReadOnly(true);
                titleCombox36.SetReadOnly(true);
                titleCombox37.SetReadOnly(true);

                titleCombox16.SetReadOnly(true);
                titleCombox15.SetReadOnly(true);
                titleCombox6.SetReadOnly(true);
                titleCombox14.SetReadOnly(true);
               
                titleCombox39.SetReadOnly(true);
                titleCombox18.SetReadOnly(true);
                titleCombox19.SetReadOnly(true);
                titleCombox20.SetReadOnly(true);
                titleCombox21.SetReadOnly(true);
                titleCombox23.SetReadOnly(true);

                titleCombox42.SetReadOnly(true);

            }
            else
            {
                titleCombox22.comboBox1.SelectAll();
                titleCombox24.comboBox1.SelectAll();
                titleCombox28.comboBox1.SelectAll();
            }
            //titleCombox16.SetReadOnly(true);
            //titleCombox14.SetReadOnly(true);
            //titleCombox6.SetReadOnly(true);
            //titleCombox15.SetReadOnly(true);
            //titleCombox19.SetReadOnly(true);
            //titleCombox20.SetReadOnly(true);
            //titleCombox21.SetReadOnly(true);
            //titleCombox23.SetReadOnly(true);
            //titleCombox22.comboBox1.BackColor = Color.AliceBlue;
            //titleCombox24.comboBox1.BackColor = Color.AliceBlue;
            //titleCombox28.comboBox1.BackColor = Color.AliceBlue;

            //转动惯量NationalFive
            //阻力 NationalSix
        


        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void AddTestForm_Resize(object sender, EventArgs e)
        {
            label4.Width = this.Width;
            label5.Width = this.Width;
            label6.Width = this.Width;
            label7.Width = this.Width;
        }

        private void AddTestForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
