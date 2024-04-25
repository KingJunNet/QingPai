using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Controls;
using LabSystem.DAL;
using TaskManager.controller;
using MSWord = Microsoft.Office.Interop.Word;

namespace TaskManager
{
    public partial class SampleEditDialog : BaseEditDialog
    {
        private readonly bool IsAllocateTask;

        public readonly string Server;
        public string Folder;
        private SampleEditDialog():base()
        {
            InitializeComponent();
        }

        public SampleEditDialog(bool authorityEdit, GridView theView, int theHand, List<DataField> fields, FormType Type1,
            bool isAllocateTask)
            : base(authorityEdit, theView, theHand, fields,Type1)
        {
            InitializeComponent();
            
            IsAllocateTask = isAllocateTask;

            var sql = new DataControl();
            Server = sql.ServerIP;
            if (!Server.EndsWith("\\"))
                Server += "\\";

            Folder=ServerConfig.Instance.ParamTableFolder+ "\\";
        }

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
            //const int height = 28 + 4;
            //var rowCount = (int)Math.Ceiling(count / 2.0) + 1;

            //var w = len * 2 + Panel.Margin.Left + Panel.Margin.Right + borderX;
            ////var h = rowCount * height + Panel.Margin.Top + Panel.Margin.Bottom + borderY;
            //var h = 770;
            //Size = new Size(w, h);

            #endregion
            
            #region 事件注册

            // 认证/非认证 显示状态切换
            if (GetControlByFieldName("certification") is TitleCombox certification)
            {
                certification.SetTextChange(CertificationOnTextChanged);
                CertificationOnTextChanged(certification.comboBox1, null);
            }

            // 总里程
            if (GetControlByFieldName("StartMileage") is TitleTextBox StartMileage &&
                GetControlByFieldName("EndMileage") is TitleTextBox EndMileage)
            {
                StartMileage.SetTextChange(MileageHandler);
                EndMileage.SetTextChange(MileageHandler);
                MileageHandler(null, null);
            }

            //自动生成报告员
            if(GetControlByFieldName("Taskcode") is TitleCombox Taskcode && GetControlByFieldName("Reporter") is TitleCombox Reporter)
            {
                Taskcode.SetTextChange(TaskcodeHandle);
                TaskcodeHandle(null,null);
            }

            if (GetControlByFieldName("Testtime") is TitleTextBox StartTime &&
                GetControlByFieldName("TestEndTime") is TitleTextBox EndTime &&
                GetControlByFieldName("TestTimeSpan") is TitleCombox)
            {
                StartTime.SetTextChange(TimeSpanHandler);
                EndTime.SetTextChange(TimeSpanHandler);
                TimeSpanHandler(null, null);
            }

            #endregion

            flowLayoutPanel1.ResumeLayout(true);
            ResumeLayout(true);

     

            Log.e("BaseEditDialog_Load");
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
            var strsql = $" select * from TaskTable where carvin='{vin}' " + $" and ISNUMERIC(EndMileage)=1 and EndMileage>{startMileage} and TestStartDate<@startDate";

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
        private void TimeSpanHandler(object sender, EventArgs e)
        {
            if (!(GetControlByFieldName("Testtime") is TitleTextBox StartTime) ||
                !(GetControlByFieldName("TestEndTime") is TitleTextBox EndTime) ||
                !(GetControlByFieldName("TestTimeSpan") is TitleCombox TimeSpan)) return;
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
            if (titleCombox24.Text == "")
            {
                titleCombox24.SetValue("1");
            }
            if (titleCombox21.Text == "")
            {
                titleCombox21.SetValue(FormSignIn.CurrentUser.Name.ToString());
            }
            if (titleCombox34.Text == "")
            {
                titleCombox34.SetValue(FormSignIn.CurrentUser.Name.ToString());
            }

       
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

        private MSWord.Application m_word;
        //private MSWord.Document m_doc;
        /// <summary>
        /// 附件下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewParamFile_Click(object sender, EventArgs e)
        {
            //无法连接文件服务
            if (ServerConfig.Instance.CanNotConnectBlobServer) {
                MessageBox.Show(ServerConfig.Instance.CannNotConnectBlobServerTips, "提醒", MessageBoxButtons.OK);
                return;
            }

            if (!(GetControlByFieldName("VIN") is TitleCombox VIN)) {
                return;
            }
            if (VIN.Value().ToString() == "")
            {
                MessageBox.Show("无附件");
                return;
            }
            var name = VIN.Value().ToString()+".doc";              
            var sourcePath = $"{Folder}\\{VIN.Value().ToString()}\\{name}";
            if (!File.Exists(sourcePath))
            {
                MessageBox.Show($"参数表文件不存在", "提醒");
                return;
            }

            //var fileDialog = new SaveFileDialog
            //{
            //    Title = $"下载 {name}",
            //    FileName = name,
            //    RestoreDirectory = true,
            //    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
            //};
            //fileDialog.ShowDialog();

            //if (fileDialog.ShowDialog() != DialogResult.OK)
            //    return;

            //var targetPath = fileDialog.FileName;
            //File.Copy(sourcePath, targetPath, true);
            //MessageBox.Show("附件下载成功！");

            m_word = new MSWord.Application();
            Object filename = name;
            Object filefullname = sourcePath;
            Object confirmConversions = Type.Missing;
            Object readOnly = Type.Missing;
            Object addToRecentFiles = Type.Missing;
            Object passwordDocument = Type.Missing;
            Object passwordTemplate = Type.Missing;
            Object revert = Type.Missing;
            Object writePasswordDocument = Type.Missing;
            Object writePasswordTemplate = Type.Missing;
            Object format = Type.Missing;
            Object encoding = Type.Missing;
            Object visible = Type.Missing;
            Object openConflictDocument = Type.Missing;
            Object openAndRepair = Type.Missing;
            Object documentDirection = Type.Missing;
            Object noEncodingDialog = Type.Missing;

            for (int i = 1; i <= m_word.Documents.Count; i++)
            {
                String str = m_word.Documents[i].FullName.ToString();
                if (str == filefullname.ToString())
                {
                    MessageBox.Show("请勿重复打开该文档");
                    return;
                }
            }
            try
            {
                m_word.Documents.Open(ref filefullname,
                        ref confirmConversions, ref readOnly, ref addToRecentFiles,
                        ref passwordDocument, ref passwordTemplate, ref revert,
                        ref writePasswordDocument, ref writePasswordTemplate,
                        ref format, ref encoding, ref visible, ref openConflictDocument,
                        ref openAndRepair, ref documentDirection, ref noEncodingDialog
                        );
                m_word.Visible = true;

                //MessageBox.Show(m_word.Documents.Count.ToString());
                //MessageBox.Show(m_word.Documents[1].FullName.ToString());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("打开Word文档出错");
            }

        // this.TopLevel = Bottom;
        }

        public FreshCorrect freshcorrect;
        protected override void BtnUpdateClick(object sender, EventArgs e)
        {
            //无法连接文件服务
            if (ServerConfig.Instance.CanNotConnectBlobServer)
            {
                MessageBox.Show(ServerConfig.Instance.CannNotConnectBlobServerTips, "提醒", MessageBoxButtons.OK);
                return;
            }

            string oriVin = view.GetRowCellValue(hand, "VIN").ToString();
            string newVin = GetControlByFieldName("VIN").Value().Trim();
            if (btnUpdate.Text == "新增")
            {  
                string sql = $"select count(*) from sampletable where VIN ='{newVin}'";
                if (SqlHelper.GetList(sql).Rows[0][0].ToString() != "0")
                {
                    MessageBox.Show("VIN已存在，无法继续录入");
                    return;
                };
            }
            else
            {
                if (oriVin != newVin)
                {
                    string sql = $"select count(*) from sampletable where VIN ='{newVin}'";
                    if (SqlHelper.GetList(sql).Rows[0][0].ToString() != "0")
                    {
                        MessageBox.Show("VIN已存在，无法继续录入");
                        return;
                    };
                }
            }

            //同步至表单
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

            //创建样本的文件目录
            var dir = new DirectoryInfo($"{Folder}{oriVin}");
            if (!dir.Exists)
                dir.Create();
            if (btnUpdate.Text == "更新"){
                //string samesql = $"select * from TestStatistic  where  SampleModel ='{GetControlByFieldName("VehicleModel").Value().ToString()}' and Producer='{GetControlByFieldName("SampleProducter").Value().ToString()}' and Carvin='{GetControlByFieldName("VIN").Value().ToString()}' and YNDirect='{GetControlByFieldName("DirectInjection").Value().ToString()}' and PowerType='{GetControlByFieldName("PowerType").Value().ToString()}' and EngineModel='{GetControlByFieldName("EngineModel").Value().ToString()}' and TransmissionType='{GetControlByFieldName("GearboxForm").Value().ToString()}' and EngineProduct='{GetControlByFieldName("EngineProducter").Value().ToString()}' and FuelLabel='{GetControlByFieldName("FuelLabel").Value().ToString()}' and FuelType='{GetControlByFieldName("FuelType").Value().ToString()}' and CarType='{GetControlByFieldName("CarType").Value().ToString()}'";

                //DataTable samedata = SqlHelper.GetList(samesql);
                //if (samedata.Rows.Count == 0)
                //{
                //更新共享文件夹名字                                     
                if(oriVin!= newVin) { 
                    if (File.Exists($"{Folder}{oriVin}\\{oriVin}.doc"))
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.RenameFile($"{Folder}{oriVin}\\{oriVin}.doc", $"{newVin}.doc");
                    }
                    
                    Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory($"{Folder}{oriVin}", newVin);
                    string sql = $"update sampletable set VIN = '{newVin}' where VIN ='{oriVin}'";
                    SqlHelper.ExecuteNonquery(sql, CommandType.Text);
                }
                //TODO:该功能于2024-04废除掉
                //if (MessageBox.Show("是否将改动的样品信息同步到试验统计对应的信息？", " 提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                //{
                //    string sql = $"update TestStatistic  set SampleModel ='{GetControlByFieldName("VehicleModel").Value().ToString()}',Producer='{GetControlByFieldName("SampleProducter").Value().ToString()}',Carvin='{GetControlByFieldName("VIN").Value().ToString()}',YNDirect='{GetControlByFieldName("DirectInjection").Value().ToString()}',PowerType='{GetControlByFieldName("PowerType").Value().ToString()}',EngineModel='{GetControlByFieldName("EngineModel").Value().ToString()}',TransmissionType='{GetControlByFieldName("GearboxForm").Value().ToString()}',EngineProduct='{GetControlByFieldName("EngineProducter").Value().ToString()}',FuelLabel='{GetControlByFieldName("FuelLabel").Value().ToString()}',FuelType='{GetControlByFieldName("FuelType").Value().ToString()}',CarType='{GetControlByFieldName("CarType").Value().ToString()}' where Carvin='{oriVin}'";
                //    SqlHelper.ExecuteNonquery(sql, System.Data.CommandType.Text);
                //    MessageBox.Show("同步成功");
                // }
                //}

                //if (MessageBox.Show("是否校验样品信息？", " 提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                //{
                //    freshcorrect();
                //}
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void SampleEditDialog_Load(object sender, EventArgs e)
        {
            //titleCombox2.comboBox1.BackColor = Color.LightBlue;
            //titleCombox14.comboBox1.BackColor = Color.LightBlue;
            //titleCombox3.comboBox1.select
            
        }

      
        private void btnViewFile_Click(object sender, EventArgs e)
        {
            //无法连接文件服务
            if (ServerConfig.Instance.CanNotConnectBlobServer)
            {
                MessageBox.Show(ServerConfig.Instance.CannNotConnectBlobServerTips, "提醒", MessageBoxButtons.OK);
                return;
            }

            if ((GetControlByFieldName("VIN") is TitleCombox VIN))
            {
                string fileDirectory = $"{Folder}{VIN.Value().ToString()}";
                if (Directory.Exists(fileDirectory))
                    System.Diagnostics.Process.Start("Explorer.exe", fileDirectory);
                else
                    MessageBox.Show($"附件目录‘{fileDirectory}’不存在", "提醒");

            }
        }
    }
}
