using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using LabSystem.DAL;
using Newtonsoft.Json.Linq;
using TaskManager.common.utils;
using TaskManager.Model;
using Xfrog.Net;

namespace TaskManager
{
    public partial class NewTaskForm : BaseForm
    {
        public static readonly string LIMS_API_HOST_LAN = "http://10.12.48.2";
        public static readonly string LIMS_API_HOST_NET = "http://rmyc6395.xicp.net:17099";

        private string limsApiHost;

        public NewTaskForm()
        {
            InitializeComponent();
            this.limsApiHost = LIMS_API_HOST_LAN;
        }

        public NewTaskForm(FormType formType, string selectedDept) : base(formType, selectedDept)
        {
            InitializeComponent();
            this.limsApiHost = LIMS_API_HOST_NET;
        }



        protected override void InitUi()
        {

            var year = DateTime.Now.Year.ToString();

            textYear.Visibility = BarItemVisibility.Never;
            comboxState.Visibility = BarItemVisibility.Never;


            textYear.EditValue = year;
            comboxState.EditValue = "所有";



            //蒸发组.Visible = !string.IsNullOrWhiteSpace(Department) && Department.Equals("蒸发组");
            barButtonItem2.Enabled = FormTable.Add;
            barButtonItem3.Enabled = FormTable.Add;

            _control._view.RowStyle += ViewOnRowStyle;
            _control._view.RowCellStyle += ViewRowCellStyle;
            _control._view.CellValueChanged += CellValueChanged;
            _control._view.RowClick += _view_RowClick;
            _control._view.FocusedRowChanged += SelectChanged;




            _control._view.Columns["Producer"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["Model"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["Clientman"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["PhoneNum"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["ChargePeople"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["AgreedDate"].OptionsColumn.ReadOnly = true;

            _control._view.Columns["ProductDate"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["DeliveryDate"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["SampleName"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["RegistrationDate"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["Taskcode"].OptionsColumn.ReadOnly = true;

            _control._view.Columns["Brand"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["CarType"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["SecurityLevel"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["ReportNum"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["FinishDate"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["State"].OptionsColumn.ReadOnly = true;


            //_control.Height = _control.Parent.Height / 2;


        }

        private void _view_RowClick(object sender, RowClickEventArgs e)
        {
            if (_control._view.RowCount == 1 && e.RowHandle >= 0)
            {
                string taskcode = _control._view.GetRowCellValue(e.RowHandle, "Taskcode").ToString();
                this.taskcode = taskcode;
                //int row = Convert.ToInt32(_control._view.GetSelectedRows()[0]);
                //MessageBox.Show(this.taskcode);


                if (taskcode == "")
                {
                    MessageBox.Show("任务单号为空");
                    return;
                }

                string json = "{'deptid': 15,'page': 1,'pageSize': 100,'taskCode': '" + this.taskcode + "'}";
                //string json = "{'operatetime_begin': '" + startdate + "','operatetime_end': '" + enddate + "','orgID': 13,'pageSize': 1000000}";

                LoadSource();

                GetLimsData(json, e.RowHandle, 2);
            }
        }

        private void SelectChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0 && Filter.state == "1")
            {
                Filter.filterText = _control._view.GetRowCellValue(e.FocusedRowHandle, "Taskcode")?.ToString().Trim();
            }
            if (_control._view.SelectedRowsCount == 1 && e.FocusedRowHandle >= 0)
            {
                //int row = Convert.ToInt32(_control._view.GetSelectedRows()[0]);

                string taskcode = _control._view.GetRowCellValue(e.FocusedRowHandle, "Taskcode").ToString();
                this.taskcode = taskcode;
                //int row = Convert.ToInt32(_control._view.GetSelectedRows()[0]);
                //MessageBox.Show(this.taskcode);


                if (taskcode == "")
                {
                    MessageBox.Show("任务单号为空");
                    return;
                }

                string json = "{'deptid': 15,'page': 1,'pageSize': 100,'taskCode': '" + this.taskcode + "'}";
                //string json = "{'operatetime_begin': '" + startdate + "','operatetime_end': '" + enddate + "','orgID': 13,'pageSize': 1000000}";

                LoadSource();

                GetLimsData(json, e.FocusedRowHandle, 2);
            }




        }


        /// <summary>
        /// 改变行颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewOnRowStyle(object sender, RowStyleEventArgs e)
        {

            //var consistent = _control._view.GetRowCellValue(e.RowHandle,"consistent")?.ToString().Trim();

            //if (e.RowHandle >=0)
            //{
            //    e.Appearance.BackColor = Color.FromArgb(193, 255, 193);
            //    for (int j = 1; j < _control._view.Columns.Count; j++)
            //    {
            //        if (_control._view.GetRowCellValue(e.RowHandle, _control._view.Columns[j])?.ToString().Trim() == "")
            //        {
            //            e.Appearance.BackColor = Color.FromArgb(237, 237, 237);
            //        }
            //    }

            //    if (consistent != "" && !consistent.Contains("是"))
            //    {
            //        e.Appearance.BackColor = Color.Orange;
            //    }


            //}

            //if (read && !state)
            //    e.Appearance.BackColor = Color.Khaki;
            //else if (!read)
            //    e.Appearance.BackColor = Color.DarkSalmon;
            //else if (read && state && !Money)
            //    e.Appearance.BackColor = Color.PaleGreen;
        }

        /// <summary>
        /// 改变单元格颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewRowCellStyle(object sender, RowCellStyleEventArgs e)
        {

            //if (e.RowHandle >= 0)
            //{
            //    var consistent = _control._view.GetRowCellValue(e.RowHandle, "consistent")?.ToString().Trim();
            //    if (consistent != null && consistent.Contains(","))
            //    {
            //        string[] name = consistent.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //        if (((IList)name).Contains(e.Column.FieldName))
            //        {

            //            e.Appearance.BackColor = Color.Red;

            //        }
            //    }
            //    //for (int j = 1; j < _control._view.Columns.Count; j++)
            //    //{
            //    //    if (_control._view.GetRowCellValue(e.RowHandle, _control._view.Columns[j])?.ToString().Trim() == "")
            //    //    {
            //    //        e.Appearance.BackColor = Color.LightSalmon;
            //    //    }
            //    //    else
            //    //    {
            //    //        e.Appearance.BackColor = Color.White;
            //    //    }
            //    //}
            //}



        }

        protected override DialogResult OpenEditForm(GridView view, int hand, List<DataField> fields)
        {
            Log.e("OpenEditForm");
            var isAllocateTask = false;
            var dialog = new NewTaskEditDialog(FormTable.Edit, view, hand, fields, isAllocateTask, FormType.NewTask);
            return dialog.ShowDialog();
        }
        protected override DialogResult OpenReplaceForm(GridView view, int hand, List<DataField> fields)
        {
            var dialog = new Dialogs.ReplaceSelectRows(FormTable.Edit, view, hand, fields, FormTable.Type);
            return dialog.ShowDialog();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dialog = new ImportData("整车", _control.Fields, _control.DataSource);
            if (dialog.ShowDialog() == DialogResult.OK)
                _control.SetSaveStatus(false);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dialog = new ImportData("炭罐", _control.Fields, _control.DataSource);
            if (dialog.ShowDialog() == DialogResult.OK)
                _control.SetSaveStatus(false);
        }

        private void _control_Load(object sender, EventArgs e)
        {
            //_control._view.FindFilterText = "222";


        }
        private Thread childThread;

        private List<DataField> Fields = new List<DataField>();

        private DataControl sql = new DataControl();
        private FormTable FormTable2;
        private void TaskForm_Load(object sender, EventArgs e)
        {





            //InitColLayout();
            //LoadSource();// 初始化gridview1
            //gridView1.OptionsView.BestFitMaxRowCount = 30;
            //gridView1.BestFitColumns();
            //gridView1.OptionsView.ColumnAutoWidth = false;//700毫秒
            //ThreadStart childref = new ThreadStart(CallToChildThread);
            //childThread = new Thread(childref);
            //childThread.Start();



        }

        /// <summary>
        /// 滚动条至最左
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            //if (_control._view.FocusedColumn == _control._view.Columns["TypeBrief"])
            //{
            //    _control._view.FocusedColumn = _control._view.Columns[0];
            //}
            //_control._view.FocusedColumn = _control._view.Columns["TypeBrief"];

            if (Templatecolumn.column == null || Templatecolumn.name == "默认模板")
            {

                _control._view.FocusedColumn = _control._view.Columns["TypeBrief"];
            }
            else
            {
                _control._view.FocusedColumn = _control._view.Columns[Templatecolumn.column[0]];
            }
        }
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {


        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Templatecolumn.column == null || Templatecolumn.name == "默认模板")
            {

                _control._view.FocusedColumn = _control._view.Columns["consistent"];
            }
            else
            {
                _control._view.FocusedColumn = _control._view.Columns[Templatecolumn.column[Templatecolumn.column.Length - 1]];
            }
        }


        private ProjectInfo project;
        private string taskcode;

        private bool showprojectinfo = false;
        /// <summary>
        /// 详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (_control.Height!= gridControl1.Height)
            //{
            // _control.Height = gridControl1.Height;
            //}
            //else
            if (showprojectinfo == true)
            {
                showprojectinfo = false;

                _control.Dock = DockStyle.Top;
                _control.Height = _control.Parent.Height / 2;
                gridControl1.Visible = true;
            }
            else
            {
                showprojectinfo = true;
                _control.Dock = DockStyle.Fill;
                gridControl1.Visible = false;
                //_control.Height = _control.Parent.Height-40;
            }



            //{

            //}


            //if (_control.Dock == DockStyle.Fill)
            //{
            //    gridControl1.Visible = true;
            //    _control.Height = 600 ;

            //}
            //{
            //    gridControl1.Visible = false;
            //    _control.Dock = DockStyle.Fill;

            //}


            //if (_control._view.SelectedRowsCount == 1)
            //{
            //    //int row = Convert.ToInt32(_control._view.GetSelectedRows()[0]);
            //    //string taskcode = _control._view.GetRowCellValue(row, "Taskcode").ToString();
            //    //this.taskcode = taskcode;

            //    //if (taskcode == "")
            //    //{
            //    //    MessageBox.Show("任务单号为空");
            //    //    return;
            //    //}

            //    //string json = "{'orgID': 13,'page': 1,'pageSize': 1000000,'taskCode': '" + this.taskcode + "'}";

            //    ////LoadSource();
            //    //GetLimsData(json);
            //    //if (project == null || project.IsDisposed)
            //    //{
            //    //    int row = Convert.ToInt32(_control._view.GetSelectedRows()[0]);
            //    //    string taskcode = _control._view.GetRowCellValue(row, "Taskcode").ToString();
            //    //    project = new ProjectInfo(taskcode);
            //    //    project.TopLevel = false;
            //    //    project.FormBorderStyle = FormBorderStyle.None;
            //    //    project.Dock = DockStyle.Fill;
            //    //    this.panel1.Controls.Clear();
            //    //    this.panel1.Controls.Add(project);
            //    //    project.Show();
            //    //}
            //    //else
            //    //{

            //    //    project.Show();
            //    //    project.Activate();

            //    //}
            //}
            //else
            //{
            //    MessageBox.Show("请选择行某一行");
            //}


        }

        public void GetLimsData(string json, int row, int ss)
        {
            //SplashScreenManager.ShowForm(typeof(WaitForm1));
            string strURL = $"{this.limsApiHost}/lims/entservice/taskinfotemp/taskinfotemp!getTaskinfoList.action";

            //创建一个HTTP请求  
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            //Post请求方式  
            request.Method = "POST";
            //内容类型
            request.ContentType = "application/x-www-form-urlencoded/json";

            //设置参数，并进行URL编码 
            string paraUrlCoded = json;
            //string paraUrlCoded = "{\r\n    \"operatetime_begin\": \"2021-07-01\",\r\n    \"operatetime_end\": \"2021-07-19\",\r\n    \"orgID\": 13,\r\n    \"page\": 1,\r\n    \"pageSize\": 3,\r\n    \"taskCode\": \"ZD21YY01\"\r\n}";
            byte[] payload;
            //将Json字符串转化为字节  
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            //设置请求的ContentLength   
            request.ContentLength = payload.Length;
            //发送请求，获得请求流 

            Stream writer;
            try
            {
                writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
            }
            catch (Exception)
            {
                writer = null;
                MessageBox.Show("连接服务器失败!");
            }
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            writer.Close();//关闭请求流
                           // String strValue = "";//strValue为http响应所返回的字符流
            HttpWebResponse response;
            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }
            Stream s = response.GetResponseStream();
            //  Stream postData = Request.InputStream;
            StreamReader sRead = new StreamReader(s);
            string postContent = sRead.ReadToEnd();
            sRead.Close();

            //JsonObject newObj1 = new JsonObject(postContent);
            JObject newObj1 = JObject.Parse(postContent);

            if (newObj1["rows"].Count() <= 0)
            {
                //MessageBox.Show("该任务单号无项目信息");
                //this.Close();
                return;

            }

            string reportstate = "已完成";


            for (int i = 0; i < newObj1["rows"][0]["tcList"].Count(); i++)
            {
                string taskcode = newObj1["rows"][0]["tvo"]["taskCode"] != null ? newObj1["rows"][0]["tvo"]["taskCode"].ToString() : "";//任务单号。

                string itemsItemCode = newObj1["rows"][0]["tcList"][i]["itemsItemCode"] != null ? newObj1["rows"][0]["tcList"][i]["itemsItemCode"].ToString() : "";//项目代码。
                string itemsName = newObj1["rows"][0]["tcList"][i]["itemsName"] != null ? newObj1["rows"][0]["tcList"][i]["itemsName"].ToString() : "";//项目名称。
                string testNumber = newObj1["rows"][0]["tcList"][i]["testNumber"] != null ? newObj1["rows"][0]["tcList"][i]["testNumber"].ToString() : "";//检验次数
                string itemsCode = newObj1["rows"][0]["tcList"][i]["itemsCode"] != null ? newObj1["rows"][0]["tcList"][i]["itemsCode"].ToString() : "";//报告编号。
                string itemsBasis = newObj1["rows"][0]["tcList"][i]["itemsBasis"] != null ? newObj1["rows"][0]["tcList"][i]["itemsBasis"].ToString() : "";//检验依据。
                string itemsFrederickItme = newObj1["rows"][0]["tcList"][i]["itemsFrederickItme"] != null ? newObj1["rows"][0]["tcList"][i]["itemsFrederickItme"].ToString() : "";//检验项目。
                string itemsItemSamplesVolume = newObj1["rows"][0]["tcList"][i]["itemsItemSamplesVolume"] != null ? newObj1["rows"][0]["tcList"][i]["itemsItemSamplesVolume"].ToString() : "";//样品数量。

                string docStatus = newObj1["rows"][0]["tcList"][i]["docStatus"] != null ? newObj1["rows"][0]["tcList"][i]["docStatus"].ToString() : "";//是否归档。

                if (docStatus == "0")
                {
                    reportstate = "未完成";
                }

                gridView1.AddNewRow();
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Taskcode", taskcode);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ProjectCode", itemsItemCode);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ProjectName", itemsName);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "TestCount", testNumber);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "TestNum", itemsCode);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "TestBasis", itemsBasis);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "TestProject", itemsFrederickItme);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "SampleName", itemsItemSamplesVolume);

                gridView1.MoveLast();

                //bool YN = true;//判断是否为新增
                //for (int j = 0; j < gridView1.RowCount; j++)
                //{

                //    if (gridView1.GetRowCellValue(j, "Taskcode").ToString() == taskcode)
                //    {
                //        YN = false;
                //        gridView1.SetRowCellValue(j, "Taskcode", taskcode);
                //        gridView1.SetRowCellValue(j, "Producer", consignor);
                //        gridView1.SetRowCellValue(j, "Model", sampleType);
                //        gridView1.SetRowCellValue(i, "Clientman", principal);
                //        gridView1.SetRowCellValue(j, "PhoneNum", companyCheckTel);
                //        gridView1.SetRowCellValue(j, "ChargePeople", commiterId);
                //        gridView1.SetRowCellValue(j, "AgreedDate", finishDate);
                //        gridView1.SetRowCellValue(j, "ProductDate", produceDate);
                //        gridView1.SetRowCellValue(j, "DeliveryDate", sampleDate);
                //        gridView1.SetRowCellValue(j, "SampleName", sampleName);
                //        gridView1.SetRowCellValue(j, "Brand", sampleTrademark);
                //        gridView1.SetRowCellValue(j, "CarType", carType);
                //        gridView1.SetRowCellValue(j, "SecurityLevel", confidentialityLevel);
                //    }


                //}

                //if (YN)
                //{

                //    MessageBox.Show(gridView1.RowCount.ToString());

                //    gridView1.AddNewRow();
                //    if (gridView1.RowCount == 0)
                //    {
                //        gridView1.AddNewRow();
                //    }

                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "Taskcode", taskcode);
                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "Producer", consignor);
                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "Model", sampleType);
                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "Clientman", principal);
                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "PhoneNum", companyCheckTel);
                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "ChargePeople", commiterId);
                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "AgreedDate", finishDate);
                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "ProductDate", produceDate);
                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "DeliveryDate", sampleDate);
                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "SampleName", sampleName);
                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "Brand", sampleTrademark);
                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "CarType", carType);
                //    gridView1.SetRowCellValue(gridView1.RowCount - 1, "SecurityLevel", confidentialityLevel);




                //}

            }
            //SplashScreenManager.CloseForm();


            if (ss == 0)
            {
                _control._view.SetRowCellValue(row, "State", reportstate);
                if (reportstate == "已完成")
                {

                    DateTime date = Convert.ToDateTime("2000-01-01 00:00");
                    for (int j = 0; j < newObj1["rows"][0]["tcList"].Count(); j++)
                    {

                        string attarchDate = newObj1["rows"][0]["tcList"][j]["attarchDate"] != null ? newObj1["rows"][0]["tcList"][j]["attarchDate"].ToString() : "";

                        if (!IsDate(attarchDate))
                        {
                            continue;
                        }
                        if (Convert.ToDateTime(attarchDate) > date)
                        {
                            date = Convert.ToDateTime(attarchDate);
                        }

                    }

                    if (date.ToString("yyyy-MM-dd") == "2000-01-01")
                    {

                        _control._view.SetRowCellValue(row, "FinishDate", "");
                    }
                    else
                    {
                        _control._view.SetRowCellValue(row, "FinishDate", date.ToString("yyyy-MM-dd"));
                    }
                }
            }



        }


        /// <summary>
        /// 判断是否为日期格式
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public bool IsDate(string strDate)
        {
            try
            {
                // strDate格式有要求，必须是yyyy-MM-dd hh:mm:ss
                DateTime.Parse(strDate);  //不是字符串时会出现异常
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected SqlConnection sqlConnection;

        protected SqlDataAdapter sqlAdapter;

        public DataTable DataSource;
        /// <summary>
        /// 查询项目信息
        /// </summary>
        public void LoadSource()
        {
            string strsql = $"select * from projectinfo where taskcode ='{taskcode}'";
            sqlConnection = new SqlConnection(sql.strCon);
            //设置select查询命令，SqlCommandBuilder要求至少有select命令 共计150毫秒
            var selectCMD = new SqlCommand(strsql, sqlConnection);
            DataSource = new DataTable();
            sqlAdapter = new SqlDataAdapter(selectCMD);
            sqlAdapter.Fill(DataSource);
            sqlConnection.Close();

            gridControl1.BeginUpdate();
            gridView1.BeginUpdate();

            gridControl1.DataSource = DataSource;

            gridView1.EndUpdate();
            gridControl1.EndUpdate();//加载数据共计250毫秒
        }

        /// <summary>
        /// 子线程，获取Lims系统数据
        /// </summary>
        public void CallToChildThread()
        {
            while (true)
            {

                Thread.Sleep(2000000);
            }
        }
        /// <summary>
        /// 获取lims数据
        /// </summary>
        public void GetLimsData(string startdate, string enddate)
        {
            string strURL = $"{this.limsApiHost}/lims/entservice/taskinfotemp/taskinfotemp!getTaskinfoList.action";

            //创建一个HTTP请求 (公用)
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            //Post请求方式  
            request.Method = "POST";
            //内容类型
            request.ContentType = "application/json";

            //设置参数，并进行URL编码 
            string paraUrlCoded = "{'operatetime_begin': '" + startdate + "','operatetime_end': '" + enddate + "','deptid': 15,'pageSize': 1000000}";
            //string paraUrlCoded = "{'operatetime_begin': '" + startdate + "','operatetime_end': '" + enddate + "','orgID': 13}";

            //string paraUrlCoded = "{\r\n    \"operatetime_begin\": \"2021-07-01\",\r\n    \"operatetime_end\": \"2021-07-19\",\r\n    \"orgID\": 13,\r\n    \"page\": 1,\r\n    \"pageSize\": 3,\r\n    \"taskCode\": \"ZD21YY01\"\r\n}";
            //byte[] payload;
            ////将Json字符串转化为字节  
            //payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            ////设置请求的ContentLength   
            //request.ContentLength = payload.Length;
            ////发送请求，获得请求流 

            //Stream writer;
            //try
            //{
            //    writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
            //}
            //catch (Exception)
            //{
            //    writer = null;
            //    MessageBox.Show("连接服务器失败!");
            //}
            ////将请求参数写入流
            //writer.Write(payload, 0, payload.Length);
            //writer.Close();//关闭请求流
            //               // String strValue = "";//strValue为http响应所返回的字符流
            //HttpWebResponse response;
            //try
            //{
            //    //获得响应流
            //    response = (HttpWebResponse)request.GetResponse();
            //}
            //catch (WebException ex)
            //{
            //    response = ex.Response as HttpWebResponse;
            //}
            //Stream s = response.GetResponseStream();
            ////  Stream postData = Request.InputStream;
            //StreamReader sRead = new StreamReader(s);
            //string postContent = sRead.ReadToEnd();
            //sRead.Close();


            ////第二种方法
            using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
            {
                dataStream.Write(paraUrlCoded);
                dataStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string postContent = reader.ReadToEnd();
            ////第二种方法

            //第三种方法
            //string postContent = "";
            //try
            //{
            //    using(HttpClient hpc =new HttpClient())
            //    {
            //        HttpContent httpContent = new StringContent(paraUrlCoded);
            //        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            //        postContent = hpc.PostAsync(strURL, httpContent).Result.Content.ReadAsStringAsync().Result;
            //    }
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //    return;
            //}

            //JsonObject newObj1 = new JsonObject(postContent);


            JObject newObj1 = JObject.Parse(postContent);
            //MessageBox.Show(newObj1["rows"].Count().ToString());
            //foreach(var e in newObj1)
            for (int i = 0; i < newObj1["rows"].Count(); i++)
            {

                string startdateLIMS = newObj1["rows"][i]["tvo"]["createdate"] != null ? newObj1["rows"][i]["tvo"]["createdate"].ToString() : "";
                try
                {
                    startdateLIMS = DateTime.ParseExact(startdateLIMS, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                }
                catch
                {
                    //MessageBox.Show("登记日期字段有误");
                    //SplashScreenManager.CloseForm();
                    continue;
                }


                string taskcode = newObj1["rows"][i]["tvo"]["taskCode"] != null ? newObj1["rows"][i]["tvo"]["taskCode"].ToString() : "";

                //MessageBox.Show(newObj1["rows"][i]["tvo"]["carLoad"].Value.ToString());
                //MessageBox.Show(taskcode);
                string consignor = newObj1["rows"][i]["tvo"]["consignor"] != null ? newObj1["rows"][i]["tvo"]["consignor"].ToString() : "";//委托单位。
                string sampleType = newObj1["rows"][i]["tvo"]["sampleType"] != null ? newObj1["rows"][i]["tvo"]["sampleType"].ToString() : "";//规格型号。
                string principal = newObj1["rows"][i]["otherVo"]["principal"] != null ? newObj1["rows"][i]["otherVo"]["principal"].ToString() : "";//委托人
                string companyCheckTel = newObj1["rows"][i]["otherVo"]["companyCheckTel"] != null ? newObj1["rows"][i]["otherVo"]["companyCheckTel"].ToString() : "";//电话。
                string commiterId = newObj1["rows"][i]["tvo"]["commiterId"] != null ? newObj1["rows"][i]["tvo"]["commiterId"].ToString() : "";//任务单负责人。
                string finishDate = newObj1["rows"][i]["tvo"]["finishDate"] != null ? newObj1["rows"][i]["tvo"]["finishDate"].ToString() : "";//商定完成时间。
                string produceDate = newObj1["rows"][i]["tvo"]["produceDate"] != null ? newObj1["rows"][i]["tvo"]["produceDate"].ToString() : "";//生产日期。
                string sampleDate = newObj1["rows"][i]["tvo"]["sampleDate"] != null ? newObj1["rows"][i]["tvo"]["sampleDate"].ToString() : "";//送样日期。
                string sampleName = newObj1["rows"][i]["tvo"]["sampleName"] != null ? newObj1["rows"][i]["tvo"]["sampleName"].ToString() : "";//样品名称。
                string sampleTrademark = newObj1["rows"][i]["tvo"]["sampleTrademark"] != null ? newObj1["rows"][i]["tvo"]["sampleTrademark"].ToString() : "";//商标。
                string carType = newObj1["rows"][i]["tvo"]["carType"] != null ? newObj1["rows"][i]["tvo"]["carType"].ToString() : "";//车辆类型。
                string confidentialityLevel = newObj1["rows"][i]["tvo"]["confidentialityLevel"] != null ? newObj1["rows"][i]["tvo"]["confidentialityLevel"].ToString() : "";//保密等级。
                string reportnum = newObj1["rows"][i]["tcList"] != null ? newObj1["rows"][i]["tcList"].Count().ToString() : "0";




                bool YN = true;//判断是否为新增

                //string sql0 = $"select count(*) from NewTaskTable where taskcode='{taskcode}'";
                //if (SqlHelper.GetList(sql0).Rows[0][0].ToString() == "0")
                //{
                //    YN = true;
                //}
                //else
                //{
                //    YN = false;
                //}
                for (int j = 0; j < _control._view.RowCount; j++)
                {

                    if (_control._view.GetRowCellValue(j, "Taskcode").ToString() == taskcode)
                    {
                        YN = false;
                        _control._view.SetRowCellValue(j, "RegistrationDate", startdateLIMS);

                        _control._view.SetRowCellValue(j, "Producer", consignor);
                        _control._view.SetRowCellValue(j, "Model", sampleType);
                        _control._view.SetRowCellValue(j, "Clientman", principal);
                        _control._view.SetRowCellValue(j, "PhoneNum", companyCheckTel);
                        _control._view.SetRowCellValue(j, "ChargePeople", commiterId);
                        _control._view.SetRowCellValue(j, "AgreedDate", finishDate);
                        _control._view.SetRowCellValue(j, "ProductDate", produceDate);
                        _control._view.SetRowCellValue(j, "DeliveryDate", sampleDate);
                        _control._view.SetRowCellValue(j, "SampleName", sampleName);
                        _control._view.SetRowCellValue(j, "Brand", sampleTrademark);
                        _control._view.SetRowCellValue(j, "CarType", carType);
                        _control._view.SetRowCellValue(j, "SecurityLevel", confidentialityLevel);
                        _control._view.SetRowCellValue(j, "ReportNum", reportnum);

                        _control._view.SetRowCellValue(j, "consistent", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        break;
                    }


                }

                //新增
                if (YN)
                {
                    _control._view.AddNewRow();

                    ///日期设置为空
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "RegistrationDate", startdateLIMS);
                    //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "FinishDate", "");
                    //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "AgreedDate", "");
                    //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "ProductDate", "");
                    //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "DeliveryDate", "");


                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "Taskcode", taskcode);
                    if (taskcode.Length >= 2)
                    {
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "TypeBrief", taskcode.Substring(0, 2));//自动获取简称
                    }


                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "Producer", consignor);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "Model", sampleType);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "Clientman", principal);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "PhoneNum", companyCheckTel);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "ChargePeople", commiterId);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "AgreedDate", finishDate);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "ProductDate", produceDate);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "DeliveryDate", sampleDate);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "SampleName", sampleName);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "Brand", sampleTrademark);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarType", carType);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "SecurityLevel", confidentialityLevel);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "ReportNum", reportnum);

                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "consistent", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

                    _control._view.MoveLast();



                }


            }


        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            if ((DateTime.Now.ToString("HH") == "02") && FormSignIn.CurrentUser.Name == "赵红星")
            {
                int monthcount = 0;

                for (int i = 0; i < 12; i++)
                {
                    //_control.SaveSource();
                    // _control.SetSaveStatus(true);
                    _control.SaveClick();

                    startdate.EditValue = DateTime.Now.AddMonths(monthcount - 1).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    enddate.EditValue = DateTime.Now.AddMonths(monthcount).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);

                    _control.RefreshClick(_year, startdate.EditValue.ToString().Trim(), enddate.EditValue.ToString().Trim(), _finishState, _group);

                    SplashScreenManager.ShowForm(typeof(WaitForm1));
                    try
                    {
                        //GetLimsData(DateTime.Now.AddMonths(monthcount - 2).ToString("yyyy-MM-dd"), DateTime.Now.AddMonths(monthcount).ToString("yyyy-MM-dd"));
                        GetLimsData(Convert.ToDateTime(_startdate).ToString("yyyy-MM-dd"), Convert.ToDateTime(_enddate).ToString("yyyy-MM-dd"));

                        for (int j = 0; j < _control._view.RowCount; j++)
                        {
                            string taskcode = _control._view.GetRowCellValue(j, "Taskcode").ToString();
                            this.taskcode = taskcode;
                            if (taskcode == "")
                            {
                                return;
                            }
                            string json = "{'deptid': 15,'page': 1,'pageSize': 100,'taskCode': '" + this.taskcode + "'}";
                            GetLimsData(json, j, 0);
                        }
                    }
                    catch
                    {

                    }
                    finally
                    {

                        SplashScreenManager.CloseForm();
                        _control.SaveClick();


                        monthcount = monthcount - 1;

                    }
                }

            };
        }

        private void timerUpdateCurDay_Tick(object sender, EventArgs e)
        {
            try
            {
                this.updateTodayTaskData();
            }
            catch (Exception ex)
            {
                Log.e($"Update Today Task Data Error: {ex}");
            }
        }

        private void updateTodayTaskData()
        {
         if (FormSignIn.CurrentUser.Name != "赵红星")
            {
                return;
            }
            int hour = DateTime.Now.Hour;
            if (hour >= 0 && hour < 6)
            {
                return;
            }
            //DateTime startTime = DateTime.Today.AddDays(-90);
            //DateTime endTime = DateTime.Today.AddDays(1);

            DateTime startTime = DateTime.Today;
            DateTime endTime = startTime.Date.AddDays(1);
            string startTimeContent = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd");
            string endTimeContent = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd");
            this.FetchLimsData(startTimeContent, endTimeContent);
        }

        /// <summary>
        /// lims接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DateTime startTime = DateTime.Today.AddDays(-20);
            //DateTime endTime = DateTime.Today.AddDays(1);

            DateTime startTime = DateTime.Today;
            DateTime endTime = startTime.Date.AddDays(1);
            string startTimeContent = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd");
            string endTimeContent = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd");

            //重置条件
            startdate.EditValue = startTimeContent;
            enddate.EditValue = endTimeContent;
            _control._view.FindFilterText = "";

            _control.RefreshClick(_year, startTimeContent, endTimeContent, _finishState, _group);

            SplashScreenManager.ShowForm(typeof(WaitForm1));
            GetLimsData(startTimeContent, endTimeContent);

            //刷新状态

            for (int j = 0; j < _control._view.RowCount; j++)
            {
                string taskcode = _control._view.GetRowCellValue(j, "Taskcode").ToString();
                this.taskcode = taskcode;
                //int row = Convert.ToInt32(_control._view.GetSelectedRows()[0]);
                //MessageBox.Show(this.taskcode);


                if (taskcode == "")
                {
                    //MessageBox.Show("任务单号为空");
                    return;
                }

                string json = "{'deptid': 15,'page': 1,'pageSize': 100,'taskCode': '" + this.taskcode + "'}";
                //string json = "{'operatetime_begin': '" + startdate + "','operatetime_end': '" + enddate + "','orgID': 13,'pageSize': 1000000}";

                //LoadSource();

                GetLimsData(json, j, 0);

            }

            MessageBox.Show("更新Lims数据成功");

            SplashScreenManager.CloseForm();

        }

        private SelectTemplate selectTemplate;
        /// <summary>
        ///选择模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (selectTemplate == null || selectTemplate.IsDisposed)
            {
                selectTemplate = new SelectTemplate("任务管理");
                selectTemplate.freshForm += SelectTemplate_freshForm;
                selectTemplate.Show();
            }
            else
            {
                selectTemplate.Show();
                selectTemplate.Activate();
            }

        }

        private void SelectTemplate_freshForm()
        {
            for (int j = 1; j < _control._view.Columns.Count; j++)
            {
                _control._view.Columns[j].Visible = false;
            }
            for (int i = Templatecolumn.column.Length - 1; i >= 0; i--)
            {
                _control._view.Columns[Templatecolumn.column[i]].Visible = true;
                // _control._view.Columns[Templatecolumn.column[i]].VisibleIndex = i;
            }
        }
        /// <summary>
        /// 默认模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            for (int j = 1; j < _control._view.Columns.Count; j++)
            {
                _control._view.Columns[j].Visible = false;
            }
            for (int j = 1; j < _control._view.Columns.Count; j++)
            {
                _control._view.Columns[j].Visible = true;
                _control._view.Columns[j].VisibleIndex = j;
            }
        }

        private void CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "State")
            {
                if (e.Value.ToString() == "未完成")
                {
                    _control._view.SetRowCellValue(e.RowHandle, "FinishDate", "");
                }

            }
            if (e.Column.FieldName == "Taskcode")
            {
                if (_control._view.GetRowCellValue(e.RowHandle, "Taskcode").ToString().Length >= 2)
                {
                    _control._view.SetRowCellValue(e.RowHandle, "TypeBrief", _control._view.GetRowCellValue(e.RowHandle, "Taskcode").ToString().Substring(0, 2));
                }

            }
            if (e.Column.FieldName == "TypeBrief")
            {
                string typeCode = e.Value.ToString();
                string typeChn = getTaskChnByCode(typeCode);
                _control._view.SetRowCellValue(e.RowHandle, "Type1", typeChn);
            }
        }

        private string getTaskChnByCode(string typeCode)
        {
            bool success = Enum.TryParse(typeCode, out TaskType taskType);
            return success ? taskType.GetDescription() : "";
        }

        private AlertTemplate alertTemplate;
        /// <summary>
        /// 自定义模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (alertTemplate == null || alertTemplate.IsDisposed)
            {
                alertTemplate = new AlertTemplate("任务管理");
                alertTemplate.Show();
            }
            else
            {
                alertTemplate.Show();
                alertTemplate.Activate();
            }
        }

        public void FetchLimsData(string startdate, string enddate)
        {
            HandleLimsResponseData(startdate, enddate);
        }

        private void HandleLimsResponseData(string startdate, string enddate)
        {
            JObject jsonObj = GetTaskDateByPost(startdate, enddate);
            DateTime nowTime = DateTime.Now;
            for (int i = 0; i < jsonObj["rows"].Count(); i++)
            {
                var obj = jsonObj["rows"][i];
                var taskObj = obj["tvo"];
                try
                {
                    this.LimsResponseJson2TaskData(obj, taskObj, nowTime);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        private void LimsResponseJson2TaskData(JToken obj, JToken taskObj, DateTime nowTime)
        {
            //提取数据
            string consistent = nowTime.ToString("yyyy-MM-dd HH:mm");
            string startdateLIMS = taskObj["createdate"] != null ? taskObj["createdate"].ToString() : "";
            try
            {
                startdateLIMS = DateTime.ParseExact(startdateLIMS, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
            }
            catch
            {
                return;
            }

            string taskcode = taskObj["taskCode"] != null ? taskObj["taskCode"].ToString() : "";
            string consignor = taskObj["consignor"] != null ? taskObj["consignor"].ToString() : "";//委托单位。
            string sampleType = taskObj["sampleType"] != null ? taskObj["sampleType"].ToString() : "";//规格型号。
            string principal = obj["otherVo"]["principal"] != null ? obj["otherVo"]["principal"].ToString() : "";//委托人
            string companyCheckTel = obj["otherVo"]["companyCheckTel"] != null ? obj["otherVo"]["companyCheckTel"].ToString() : "";//电话。
            string commiterId = taskObj["commiterId"] != null ? taskObj["commiterId"].ToString() : "";//任务单负责人。
            string finishDate = taskObj["finishDate"] != null ? taskObj["finishDate"].ToString() : "";//商定完成时间。
            string produceDate = taskObj["produceDate"] != null ? taskObj["produceDate"].ToString() : "";//生产日期。
            string sampleDate = taskObj["sampleDate"] != null ? taskObj["sampleDate"].ToString() : "";//送样日期。
            string sampleName = taskObj["sampleName"] != null ? taskObj["sampleName"].ToString() : "";//样品名称。
            string sampleTrademark = taskObj["sampleTrademark"] != null ? taskObj["sampleTrademark"].ToString() : "";//商标。
            string carType = taskObj["carType"] != null ? taskObj["carType"].ToString() : "";//车辆类型。
            string confidentialityLevel = taskObj["confidentialityLevel"] != null ? taskObj["confidentialityLevel"].ToString() : "";//保密等级。
            string reportnum = obj["tcList"] != null ? obj["tcList"].Count().ToString() : "0";
            string taskState = this.extractTaskState(obj);
            if (taskState == "已完成")
            {
                this.fixTaskFinishDate(obj, out finishDate);
            }
            string typeBrief = taskcode.Substring(0, 2);
            string typeChn = getTaskChnByCode(typeBrief);

            //验证数据是否存在
            string sql = "";
            bool taskcodeExists = ChecktaskcodeExists(taskcode);
            if (taskcodeExists)
            {
                sql = $"UPDATE NewTaskTable " +
                                   "SET " +
                                   $"TypeBrief='{typeBrief}', " +
                                   $"Type1='{typeChn}', " +
                                   $"State='{taskState}', " +
                                   $"RegistrationDate='{startdateLIMS}', " +
                                   $"Producer='{consignor}', " +
                                   $"Model='{sampleType}', " +
                                   $"ReportNum='{reportnum}', " +
                                   $"Clientman='{principal}', " +
                                   $"PhoneNum='{companyCheckTel}', " +
                                   $"ChargePeople='{commiterId}', " +
                                   $"AgreedDate='{finishDate}', " +
                                   $"ProductDate='{produceDate}', " +
                                   $"DeliveryDate='{sampleDate}', " +
                                   $"SampleName='{sampleName}', " +
                                   $"Brand='{sampleTrademark}', " +
                                   $"CarType='{carType}', " +
                                   $"SecurityLevel='{confidentialityLevel}', " +
                                   $"consistent='{consistent}' " +
                                   $"WHERE taskcode='{taskcode}'";


            }
            else
            {
                sql = $"INSERT INTO NewTaskTable (" +
                               $"Taskcode, " +
                               $"TypeBrief, " +
                               $"Type1, " +
                               $"State, " +
                               $"RegistrationDate, " +
                               $"Producer, " +
                               $"Model, " +
                               $"ReportNum, " +
                               $"Clientman, " +
                               $"PhoneNum, " +
                               $"ChargePeople, " +
                               $"AgreedDate, " +
                               $"ProductDate, " +
                               $"DeliveryDate, " +
                               $"SampleName, " +
                               $"Brand, " +
                               $"CarType, " +
                               $"SecurityLevel, " +
                               $"consistent" +
                               $") VALUES (" +
                               $"'{taskcode}', " +
                               $"'{typeBrief}', " +
                               $"'{typeChn}', " +
                               $"'{taskState}', " +
                               $"'{startdateLIMS}', " +
                               $"'{consignor}', " +
                               $"'{sampleType}', " +
                               $"'{reportnum}', " +
                               $"'{principal}', " +
                               $"'{companyCheckTel}', " +
                               $"'{commiterId}', " +
                               $"'{finishDate}', " +
                               $"'{produceDate}', " +
                               $"'{sampleDate}', " +
                               $"'{sampleName}', " +
                               $"'{sampleTrademark}', " +
                               $"'{carType}', " +
                               $"'{confidentialityLevel}', " +
                               $"'{consistent}'" +
                               $")";

            }

            //执行数据库操作
            SqlHelper.ExecuteNonquery(sql, CommandType.Text);
        }

        private string extractTaskState(JToken obj)
        {
            string state = "已完成";
            //确定reportstate
            for (int i = 0; i < obj["tcList"].Count(); i++)
            {
                string docStatus = obj["tcList"][i]["docStatus"] != null ? obj["tcList"][i]["docStatus"].ToString() : "";//是否归档。
                if (docStatus == "0")
                {
                    return "未完成";
                }
            }

            return state;
        }

        private void fixTaskFinishDate(JToken obj,out string finishDate)
        {
            DateTime date = Convert.ToDateTime("2000-01-01 00:00");
            for (int j = 0; j < obj["tcList"].Count(); j++)
            {
                string attarchDate = obj["tcList"][j]["attarchDate"] != null ? obj["tcList"][j]["attarchDate"].ToString() : "";
                if (!IsDate(attarchDate))
                {
                    continue;
                }
                if (Convert.ToDateTime(attarchDate) > date)
                {
                    date = Convert.ToDateTime(attarchDate);
                }
            }
            if (date.ToString("yyyy-MM-dd") == "2000-01-01")
            {
                finishDate = "";
            }
            else
            {
                finishDate = date.ToString("yyyy-MM-dd");
            }
        }

        private JObject GetTaskDateByPost(string startdate, string enddate)
        {
            string strURL = $"{this.limsApiHost}/lims/entservice/taskinfotemp/taskinfotemp!getTaskinfoList.action";
            // 构建请求体  
            string jsonData = "{\"operatetime_begin\": \"" + startdate + "\",\"operatetime_end\": \"" + enddate + "\",\"deptid\": 15,\"pageSize\": 1000000}";
            //创建一个HTTP请求 (公用)
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            //Post请求方式  
            request.Method = "POST";
            //内容类型
            request.ContentType = "application/json";

            using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
            {
                dataStream.Write(jsonData);
                dataStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string postContent = reader.ReadToEnd();
            JObject jsonObj = JObject.Parse(postContent);

            return jsonObj;
        }

        private bool ChecktaskcodeExists(string taskcode)
        {
            string sql = "SELECT COUNT(*)FROM NewTaskTable WHERE Taskcode = @Taskcode";
            SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("Taskcode", DbHelper.ValueOrDBNullIfNull(taskcode))
                };
            int count = (int)SqlHelper.ExcuteScalar(sql, parameters);
            return count > 0;
        }


        #region 样品信息
        public void LoadSource(string taskcode)
        {

            //string sql = $"select * from TestStatistic where taskcode ='{taskcode}'";
            //DataTable da = SqlHelper.GetList(sql);
            //gridControl1.DataSource = da;
            //gridView1.OptionsView.BestFitMaxRowCount = 30;
            //gridView1.BestFitColumns();
            //gridView1.OptionsView.ColumnAutoWidth = false;//700毫秒
        }

        //private void InitColLayout()
        //{
        //    FormTable2 = new FormTable(FormType.Test, null);
        //    Fields = sql.InitDataFields(FormTable2);// 样品信息字段
        //    gridControl1.BeginUpdate();
        //    gridView1.BeginUpdate();

        //    gridView1.Columns.Clear();

        //    var cols = new List<GridColumn>();

        //    var fixColsName = new[] { "ID" };
        //    foreach (var col in fixColsName.Select(colName => new GridColumn
        //    {
        //        Name = colName,
        //        FieldName = colName
        //    }))
        //    {
        //        col.OptionsColumn.AllowEdit = false;
        //        col.Visible = false;
        //        col.OptionsColumn.AllowMove = false;
        //        col.OptionsColumn.ShowInCustomizationForm = false;
        //        cols.Add(col);
        //    }

        //    for (var i = 0; i < Fields.Count; i++)
        //    {
        //        var col = new GridColumn
        //        {
        //            Tag = Fields[i],
        //            Name = Fields[i].Eng,
        //            FieldName = Fields[i].Eng,
        //            Caption = Fields[i].Chs,
        //            VisibleIndex = i,
        //            Visible = Fields[i].ColumnVisible,
        //            Fixed = Fields[i].DisplayLevel == 0 ? FixedStyle.Left : FixedStyle.None
        //        };
        //        col.OptionsFilter.AllowFilter = true;
        //        col.OptionsFilter.AllowAutoFilter = true;

        //        col.OptionsColumn.AllowEdit = Fields[i].AllowEdit;

        //        cols.Add(col);
        //    }

        //    gridView1.Columns.AddRange(cols.ToArray());


        //    gridView1.EndUpdate();
        //    gridControl1.EndUpdate();
        //}


        #endregion

        /// <summary>
        /// 分配任务信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {

            var hand = _control.FocusedRowHandle;
            if (hand < 0)
            {
                MessageBox.Show("请选择一行数据!");
                return;
            }
            //else if (!AuthorityAllocate)
            //{
            //    MessageBox.Show("所需权限: 试验组综合管理:分配试验任务");
            //    return;
            //}

            Log.e("Open AllocateTaskDialog");
            var dialog = new AllocateTaskDialog(_control._view, hand);
            dialog.ShowDialog();

            _control._view.FocusedRowHandle = hand + 1;
            _control._view.FocusedRowHandle = hand; //防止出错
            _control.SetSaveStatus(false);
        }

        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 样品信息_ItemClick(object sender, ItemClickEventArgs e)
        {


        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (_control._view.SelectedRowsCount == 1)
            //{
            //    int row = Convert.ToInt32(_control._view.GetSelectedRows()[0]);
            //    string taskcode = _control._view.GetRowCellValue(row, "Taskcode").ToString();

            //    LoadSource(taskcode);
            //}
        }

        /// <summary>
        /// 查看源数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_control._view.SelectedRowsCount == 1)
            {
                int row = Convert.ToInt32(_control._view.GetSelectedRows()[0]);
                string taskcode = _control._view.GetRowCellValue(row, "Taskcode").ToString();
                GetLimsData(taskcode, row);
            }
            else
            {
                MessageBox.Show("请选中某条数据进行还原！");
            }
        }
        public void GetLimsData(string taskcode0, int row)
        {
            SplashScreenManager.ShowForm(typeof(WaitForm1));
            string strURL = $"{this.limsApiHost}/lims/entservice/taskinfotemp/taskinfotemp!getTaskinfoList.action";
            //创建一个HTTP请求  
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            //Post请求方式  
            request.Method = "POST";
            //内容类型
            request.ContentType = "application/x-www-form-urlencoded/json";

            //设置参数，并进行URL编码 

            string paraUrlCoded = "{'deptid': 15,'page': 1,'pageSize': 100,'taskCode': '" + taskcode0 + "'}";
            //string paraUrlCoded = "{'operatetime_begin': '" + startdate + "','operatetime_end': '" + enddate + "','orgID': 13,'pageSize': 10}";
            //string paraUrlCoded = "{'orgID': 13,'page': 1,'pageSize': 100000,'taskCode': ''}";
            //string paraUrlCoded = "{\r\n    \"operatetime_begin\": \"2021-07-01\",\r\n    \"operatetime_end\": \"2021-07-19\",\r\n    \"orgID\": 13,\r\n    \"page\": 1,\r\n    \"pageSize\": 3,\r\n    \"taskCode\": \"ZD21YY01\"\r\n}";
            byte[] payload;
            //将Json字符串转化为字节  
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            //设置请求的ContentLength   
            request.ContentLength = payload.Length;
            //发送请求，获得请求流 

            Stream writer;
            try
            {
                writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
            }
            catch (Exception)
            {
                writer = null;
                MessageBox.Show("连接服务器失败!");
            }
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            writer.Close();//关闭请求流
                           // String strValue = "";//strValue为http响应所返回的字符流
            HttpWebResponse response;
            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }
            Stream s = response.GetResponseStream();
            //  Stream postData = Request.InputStream;
            StreamReader sRead = new StreamReader(s);
            string postContent = sRead.ReadToEnd();
            sRead.Close();

            //JsonObject newObj1 = new JsonObject(postContent);
            JObject newObj1 = JObject.Parse(postContent);

            for (int i = 0; i < newObj1["rows"].Count(); i++)
            {

                string taskcode = newObj1["rows"][i]["tvo"]["taskCode"] != null ? newObj1["rows"][i]["tvo"]["taskCode"].ToString() : "";//委托单位。
                if (taskcode0 == taskcode)
                {
                    string consignor = newObj1["rows"][i]["tvo"]["consignor"] != null ? newObj1["rows"][i]["tvo"]["consignor"].ToString() : "";//委托单位。
                    string sampleType = newObj1["rows"][i]["tvo"]["sampleType"] != null ? newObj1["rows"][i]["tvo"]["sampleType"].ToString() : "";//规格型号。
                    string principal = newObj1["rows"][i]["otherVo"]["principal"] != null ? newObj1["rows"][i]["otherVo"]["principal"].ToString() : "";//委托人
                    string companyCheckTel = newObj1["rows"][i]["otherVo"]["companyCheckTel"] != null ? newObj1["rows"][i]["otherVo"]["companyCheckTel"].ToString() : "";//电话。
                    string commiterId = newObj1["rows"][i]["tvo"]["commiterId"] != null ? newObj1["rows"][i]["tvo"]["commiterId"].ToString() : "";//任务单负责人。
                    string finishDate = newObj1["rows"][i]["tvo"]["commiterId"] != null ? newObj1["rows"][i]["tvo"]["finishDate"].ToString() : "";//商定完成时间。
                    string produceDate = newObj1["rows"][i]["tvo"]["produceDate"] != null ? newObj1["rows"][i]["tvo"]["produceDate"].ToString() : "";//生产日期。
                    string sampleDate = newObj1["rows"][i]["tvo"]["sampleDate"] != null ? newObj1["rows"][i]["tvo"]["sampleDate"].ToString() : "";//送样日期。
                    string sampleName = newObj1["rows"][i]["tvo"]["sampleName"] != null ? newObj1["rows"][i]["tvo"]["sampleName"].ToString() : "";//样品名称。
                    string sampleTrademark = newObj1["rows"][i]["tvo"]["sampleTrademark"] != null ? newObj1["rows"][i]["tvo"]["sampleTrademark"].ToString() : "";//商标。
                    string carType = newObj1["rows"][i]["tvo"]["carType"] != null ? newObj1["rows"][i]["tvo"]["carType"].ToString() : "";//车辆类型。
                    string confidentialityLevel = newObj1["rows"][i]["tvo"]["confidentialityLevel"] != null ? newObj1["rows"][i]["tvo"]["confidentialityLevel"].ToString() : "";//保密等级。

                    string reportnum = newObj1["rows"][i]["tcList"] != null ? newObj1["rows"][i]["tcList"].Count().ToString() : "0";

                    _control._view.SetRowCellValue(row, "Taskcode", taskcode);
                    _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "TypeBrief", taskcode.Substring(0, 2));//自动获取简称

                    _control._view.SetRowCellValue(row, "Producer", consignor);
                    _control._view.SetRowCellValue(row, "Model", sampleType);
                    _control._view.SetRowCellValue(row, "Clientman", principal);
                    _control._view.SetRowCellValue(row, "PhoneNum", companyCheckTel);
                    _control._view.SetRowCellValue(row, "ChargePeople", commiterId);
                    _control._view.SetRowCellValue(row, "AgreedDate", finishDate);
                    _control._view.SetRowCellValue(row, "ProductDate", produceDate);
                    _control._view.SetRowCellValue(row, "DeliveryDate", sampleDate);
                    _control._view.SetRowCellValue(row, "SampleName", sampleName);
                    _control._view.SetRowCellValue(row, "Brand", sampleTrademark);
                    _control._view.SetRowCellValue(row, "CarType", carType);
                    _control._view.SetRowCellValue(row, "SecurityLevel", confidentialityLevel);
                    _control._view.SetRowCellValue(row, "ReportNum", reportnum);
                };
            }
            SplashScreenManager.CloseForm();

        }

        private void _control_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void _control_Click(object sender, EventArgs e)
        {

        }

        private void NewTaskForm_Shown(object sender, EventArgs e)
        {
            _control.Height = _control.Parent.Height / 2;

        }

        private void NewTaskForm_Activated(object sender, EventArgs e)
        {
            Filter.Moudle = "任务管理";
            //_control._view.FindFilterText = Filter.filterText;
        }

        private void NewTaskForm_Leave(object sender, EventArgs e)
        {
            Filter.state = "0";
        }

        private void NewTaskForm_Enter(object sender, EventArgs e)
        {
            Filter.state = "1";
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

      
    }
}
