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
using LabSystem.DAL;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Base;
using System.Net;
using DevExpress.XtraSplashScreen;
using System.IO;
using Xfrog.Net;

namespace TaskManager
{
    public partial class ProjectInfo : DevExpress.XtraEditors.XtraForm
    {
        public ProjectInfo()
        {
            InitializeComponent();
        }

        private string taskcode;
        public ProjectInfo(string taskcode)
        {
            InitializeComponent();
            this.taskcode = taskcode;
        }



        public DataControl sql = new DataControl();

        protected SqlConnection sqlConnection;

        protected SqlDataAdapter sqlAdapter;

        public DataTable DataSource;
        private void ProjectInfo_Load(object sender, EventArgs e)
        {
            //gridView1.CellValueChanged += CellValueChanged;

            if (this.taskcode == "")
            {
                MessageBox.Show("任务单号为空");
                return;
            }

            string json = "{'operatetime_begin': '2021-07-01','operatetime_end': '2021-07-19','orgID': 13,'page': 1,'pageSize': 5,'taskCode': '"+this.taskcode+"'}";
            
            LoadSource();
            GetLimsData(json);
        }

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

        public void GetLimsData(string json)
        {
            SplashScreenManager.ShowForm(typeof(WaitForm1));
            string strURL = "http://rmyc6395.xicp.net:18085/tjoa/entservice/taskinfotemp/taskinfotemp!getTaskinfoList.action";
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

            JsonObject newObj1 = new JsonObject(postContent);
            if (newObj1["rows"].Count <= 0)
            {
                MessageBox.Show("该任务单号无项目信息");
                this.Close();
                return;
                
            }
            
            for (int i = 0; i < newObj1["rows"][0]["tcList"].Count; i++)
            {
                
                string taskcode = newObj1["rows"][0]["tvo"]["taskCode"] != null ? newObj1["rows"][0]["tvo"]["taskCode"].Value : "";//任务单号。

                string itemsItemCode = newObj1["rows"][0]["tcList"][i]["itemsItemCode"] != null ? newObj1["rows"][0]["tcList"][i]["itemsItemCode"].Value : "";//项目代码。
                string itemsName = newObj1["rows"][0]["tcList"][i]["itemsName"] != null ? newObj1["rows"][0]["tcList"][i]["itemsName"].Value : "";//项目名称。
                string testNumber = newObj1["rows"][0]["tcList"][i]["testNumber"] != null ? newObj1["rows"][0]["tcList"][i]["testNumber"].Value : "";//检验次数
                string itemsCode = newObj1["rows"][0]["tcList"][i]["itemsCode"] != null ? newObj1["rows"][0]["tcList"][i]["itemsCode"].Value : "";//报告编号。
                string itemsBasis = newObj1["rows"][0]["tcList"][i]["itemsBasis"] != null ? newObj1["rows"][0]["tcList"][i]["itemsBasis"].Value : "";//检验依据。
                string itemsFrederickItme = newObj1["rows"][0]["tcList"][i]["itemsFrederickItme"] != null ? newObj1["rows"][0]["tcList"][i]["itemsFrederickItme"].Value : "";//检验项目。
                string itemsItemSamplesVolume = newObj1["rows"][0]["tcList"][i]["itemsItemSamplesVolume"] != null ? newObj1["rows"][0]["tcList"][i]["itemsItemSamplesVolume"].Value : "";//样品数量。

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

         
            }
            SplashScreenManager.CloseForm();

        }

        private void CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
         

            
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSource();
            LoadSource();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public void SaveSource()
        {
            //添加任务单号
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                gridView1.SetRowCellValue(i, "Taskcode", this.taskcode);
            }

            if (sqlAdapter == null || DataSource == null)
                return;

            if (DataSource.GetChanges() == null)
                return;

            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
                // ReSharper disable once UnusedVariable
                var commandBuilder = new SqlCommandBuilder(sqlAdapter);//必须要有这一句
                var updateTable = DataSource.GetChanges();

                //执行更新
                sqlAdapter.Update(updateTable ?? throw new InvalidOperationException());
                //使DataTable保存更新
                DataSource.AcceptChanges();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadSource();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridView1.DeleteSelectedRows();
        }

        //添加新行
        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
        }
    }
}