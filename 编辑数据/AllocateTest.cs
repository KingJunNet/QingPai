using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Controls;
using LabSystem.DAL;

namespace TaskManager
{
    public partial class AllocateTest : Form
    {
        public string VIN; //实际代表VIN

        private readonly GridView Grid;

        private readonly int Hand;

        public List<TitleControl> TitleControls;

        public AllocateTest(GridView grid, int hand)
        {
            InitializeComponent();

            if (DesignMode)
                return; 
            
            Hand = hand;
            Grid = grid;
            VIN = Grid.GetRowCellValue(Hand, "Carvin").ToString().Trim();

        }

        private void AllocateTaskDialog_Load(object sender, EventArgs e)
        {
            

            Text = $"分配试验任务: {VIN}";
            comboxDepartment.SetItems(FormSignIn.UserDic.Keys.ToList());
            ExperimentalSite.SetItems(Form1.ComboxDictionary["实验地点"]);
            //LocationNumber.SetItems(Form1.ComboxDictionary["定位编号"]);
            Registrant.SetItems(FormSignIn.Users);
            SampleType.SetItems(Form1.ComboxDictionary["样品类型"]);
            ItemType.SetItems(Form1.ComboxDictionary["项目类型"]);
            Producer.SetItems(Form1.ComboxDictionary["生产厂家"]);
            YNDirect.SetItems(Form1.ComboxDictionary["是否直喷"]);
            PowerType.SetItems(Form1.ComboxDictionary["动力类型"]);
            TransmissionType.SetItems(Form1.ComboxDictionary["变速器形式"]);
            EngineProduct.SetItems(Form1.ComboxDictionary["发动机生产厂"]);
            //Drivertype.SetItems(Form1.ComboxDictionary["驱动形式"]);
            StandardStage.SetItems(Form1.ComboxDictionary["标准阶段"]);

            FuelLabel.SetItems(Form1.ComboxDictionary["燃油标号"]);

            CarType.SetItems(Form1.ComboxDictionary["车辆类型"]);
            //添加备选项
            PowerType2.SetItems(Form1.ComboxDictionary["动力类型"]);
            FuelType.SetItems(Form1.ComboxDictionary["燃料类型"]);
           
            YNDirect2.SetItems(Form1.ComboxDictionary["是否直喷"]);
            ItemRemark.SetItems(Form1.ComboxDictionary["项目备注"]);
            ItemName.SetItems(Form1.ComboxDictionary["项目名称"]);
            InspectionBasis1.SetItems(Form1.ComboxDictionary["检验依据1"]);
            SingleTestMileage.SetItems(Form1.ComboxDictionary["试验里程数"]);
            SinglePretreatmentMileage.SetItems(Form1.ComboxDictionary["预处理里程数"]);
            ItemCode.SetItems(Form1.ComboxDictionary["项目代码"]);
            PriceUnit.SetItems(Form1.ComboxDictionary["价格单位"]);
            TestCycle.SetItems(Form1.ComboxDictionary["测试周期"]);
            ProjectPrice.SetItems(Form1.ComboxDictionary["单价"]);


            //查询项目简称
            string sql = "select distinct ItemBrief from ProjectQuotation";
            DataTable da = SqlHelper.GetList(sql);
            if (da.Rows.Count>0)
            {
                List<string> list1 = new List<string>();
                foreach (DataRow row in da.Rows)
                {
                    if (row[0].ToString() != "")
                    {
                        list1.Add(row[0].ToString());
                    }         
                }
                ItemBrief.SetItems(list1);
            }


            //选中行赋值
            comboxDepartment.SetValue(FormSignIn.CurrentUser.Department.ToString());
            Registrant.SetValue(FormSignIn.CurrentUser.Name.ToString());
            //comboxDepartment.SetValue(Grid.GetRowCellValue(Hand, "department").ToString().Trim());
            //ExperimentalSite.SetValue(Grid.GetRowCellValue(Hand, "ExperimentalSite").ToString().Trim());
            //LocationNumber.SetValue(Grid.GetRowCellValue(Hand, "LocationNumber").ToString().Trim());
            //Registrant.SetValue(Grid.GetRowCellValue(Hand, "Registrant").ToString().Trim());
            ItemType.SetValue(Grid.GetRowCellValue(Hand, "ItemType").ToString().Trim());
            ItemBrief.SetValue(Grid.GetRowCellValue(Hand, "ItemBrief").ToString().Trim());
            //YNDirect.SetValue(Grid.GetRowCellValue(Hand, "TestStartDate").ToString().Trim());

            dateEdit1.SetValue(Grid.GetRowCellValue(Hand, "TestStartDate").ToString().Trim());

            SampleModel.SetValue(Grid.GetRowCellValue(Hand, "SampleModel").ToString().Trim());
            Producer.SetValue(Grid.GetRowCellValue(Hand, "Producer").ToString().Trim());
            TaskcodeCombox.SetValue(Grid.GetRowCellValue(Hand, "Carvin").ToString().Trim());
            YNDirect.SetValue(Grid.GetRowCellValue(Hand, "YNDirect").ToString().Trim());
            PowerType.SetValue(Grid.GetRowCellValue(Hand, "PowerType").ToString().Trim());
            TransmissionType.SetValue(Grid.GetRowCellValue(Hand, "TransmissionType").ToString().Trim());
            EngineModel.SetValue(Grid.GetRowCellValue(Hand, "EngineModel").ToString().Trim());
            EngineProduct.SetValue(Grid.GetRowCellValue(Hand, "EngineProduct").ToString().Trim());
            //Drivertype.SetValue(Grid.GetRowCellValue(Hand, "Drivertype").ToString().Trim());
            StandardStage.SetValue(Grid.GetRowCellValue(Hand, "StandardStage").ToString().Trim());
            FuelLabel.SetValue(Grid.GetRowCellValue(Hand, "FuelLabel").ToString().Trim());
            ProjectPrice.SetValue(Grid.GetRowCellValue(Hand, "ProjectPrice").ToString().Trim());

            Confidentiality.SetValue(Grid.GetRowCellValue(Hand, "Confidentiality").ToString().Trim());
            Taskcode.SetValue(Grid.GetRowCellValue(Hand, "Taskcode").ToString().Trim());

            CarType.SetValue(Grid.GetRowCellValue(Hand, "CarType").ToString().Trim());

            string selsql = $"select SampleType,FuelType,DirectInjection,PowerType from SampleTable where VIN ='{VIN}'";
            DataTable sqlda = SqlHelper.GetList(selsql);
            if (sqlda.Rows.Count > 0)
            {
                SampleType.SetValue(sqlda.Rows[0]["SampleType"].ToString());
                YNDirect2.SetValue(sqlda.Rows[0]["DirectInjection"].ToString());
                PowerType2.SetValue(sqlda.Rows[0]["PowerType"].ToString());
                FuelType.SetValue(sqlda.Rows[0]["FuelType"].ToString());
            }


            //查询试验地点和定位编号
            string sql1 = $"select Experimentsite,Locationnumber from UserStructure where  Username like '%{FormSignIn.CurrentUser.Name.ToString()}%'";
            DataTable da1 = SqlHelper.GetList(sql1);
            if (da1.Rows.Count > 0)
            {
                List<string> listsite = new List<string>();
                List<string> listnumber = new List<string>();
                foreach (DataRow row in da1.Rows)
                {
                    if (row[0].ToString() != "" && !listsite.Contains(row[0].ToString()))
                    {
                        listsite.Add(row[0].ToString());
                    }
                    if(row[1].ToString() != "" && !listnumber.Contains(row[1].ToString()))
                    {
                        listnumber.Add(row[1].ToString());
                    }
                }
                ExperimentalSite.SetItems(listsite);
                LocationNumber.SetItems(listnumber);
                ExperimentalSite.SetValue(listsite[0].ToString());
                LocationNumber.SetValue(listnumber[0].ToString());
              

            }
            

            RefreshView();
            taskpreview.listView1.MultiSelect = false;
            taskpreview.listView1.SelectedIndexChanged += SelectedIndexChanged;

            TitleControls = new List<TitleControl>();
            foreach (var control in tableLayoutPanel1.Controls)
            {
                if (control is TitleControl c)
                    TitleControls.Add(c);
            }

            ///事件注册
            ItemBrief.SetTextChange(ItemBriefHandle);
            ItemBriefHandle(null,null);

        



        }

        private void AllocateTaskDialog_Shown(object sender, EventArgs e)
        {
            Log.i("进行试验分配","样品信息");
        }

        /// <summary>
        /// 选中试验任务后显示在面板上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            var items = taskpreview.listView1.SelectedItems;
            button2.Enabled = items.Count > 0;
            button3.Enabled = items.Count > 0;

            if (items.Count == 0)
                return;

            foreach (var control in TitleControls)
            {
                taskpreview.SetValue2Control(items[0], control);
            }
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
            Regex reg = new Regex("^[0-9]+$");
            Match ma1 = reg.Match(this.ProjectPrice.Text);
            Match ma2 = reg.Match(this.SingleTestMileage.Text);
            Match ma3 = reg.Match(this.SinglePretreatmentMileage.Text);
            if (!ma1.Success && this.ProjectPrice.Text != "")
            {
                MessageBox.Show("项目单价格式不正确");
                return;
            }
            if (!ma2.Success && this.SingleTestMileage.Text != "")
            {
                MessageBox.Show("单次试验里程格式不正确");
                return;
            }
            if (!ma3.Success && this.SinglePretreatmentMileage.Text != "")
            {
                MessageBox.Show("单次预处理里程格式不正确");
                return;
            }




            var cols = new[]
            {
                "department","ExperimentalSite","LocationNumber","Registrant","CarType" 
                                ,"ItemType","ItemBrief","TestStartDate","SampleModel","Producer","Carvin","YNDirect","PowerType","TransmissionType","EngineModel","EngineProduct","StandardStage","ProjectPrice","Finishstate","MoneySure","FuelLabel","Drivertype","Tirepressure","Taskcode","Confidentiality","FuelType","Taskcode2","Testtime","Oil","PriceDetail","Remark","Contacts","phoneNum","Email","EndMileage","TotalMileage","ProjectTotal","LogisticsInformation","TestEndDate","RegistrationDate"
            };

            var strsql = "insert into TestStatistic(";
            for(var i=0;i<cols.Length;i++)
            {
                strsql += cols[i];
                if (i == cols.Length - 1)
                    strsql += ")";
                else
                    strsql += ",";
            }

            strsql += "values(";

            for (var i = 0; i < cols.Length; i++)
            {
                strsql += "@" + cols[i];
                if (i == cols.Length - 1)
                    strsql += ")";
                else
                    strsql += ",";
            }

            var parameters = new[]
            {
                new SqlParameter("department",comboxDepartment.Text),
                new SqlParameter("Carvin",VIN),
             
                new SqlParameter("ExperimentalSite",ExperimentalSite.Text),
                new SqlParameter("LocationNumber",LocationNumber.Text),
                new SqlParameter("Registrant",Registrant.Text),
      
                new SqlParameter("CarType",CarType.Text),
                new SqlParameter("ItemType",ItemType.Text),
                new SqlParameter("ItemBrief",ItemBrief.Text),
               
                new SqlParameter("TestStartDate",dateEdit1.Value().ToString()),

                new SqlParameter("SampleModel",SampleModel.Text),
                  new SqlParameter("Producer",Producer.Text),
                   new SqlParameter("YNDirect",YNDirect.Text),
                new SqlParameter("PowerType",PowerType.Text),
                  new SqlParameter("TransmissionType",TransmissionType.Text),
                  new SqlParameter("EngineModel",EngineModel.Text),
                  new SqlParameter("EngineProduct",EngineProduct.Text),
                
                  new SqlParameter("StandardStage",StandardStage.Text),
                  new SqlParameter("ProjectPrice",ProjectPrice.Text),

                  new SqlParameter("Finishstate","未完成"),
                  new SqlParameter("MoneySure","否"),
                  new SqlParameter("FuelLabel",FuelLabel.Text),

                  new SqlParameter("Drivertype",Grid.GetRowCellValue(Hand, "Drivertype").ToString().Trim()),
                   new SqlParameter("Tirepressure",Grid.GetRowCellValue(Hand, "Tirepressure").ToString().Trim()),

                   new SqlParameter("Taskcode",""),
                   new SqlParameter("Confidentiality",Confidentiality.Text),
                   new SqlParameter("FuelType",FuelType.Text),


                   new SqlParameter("Taskcode2","—"),
                     new SqlParameter("Testtime","—"),
                      new SqlParameter("Oil","—"),
                       new SqlParameter("PriceDetail","—"),
                        new SqlParameter("Remark","—"),
                        new SqlParameter("Contacts","—"),
                       new SqlParameter("phoneNum","—"),
                      new SqlParameter("Email","—"),

                     new SqlParameter("EndMileage","0"),

                     new SqlParameter("TotalMileage","0"),
                     new SqlParameter("ProjectTotal","0"),
                     new SqlParameter("LogisticsInformation","—"),
                     new SqlParameter("TestEndDate",$"{DateTime.Now:yyyy/MM/dd HH:mm}"),
                     new SqlParameter("RegistrationDate",$"{DateTime.Now:yyyy/MM/dd HH:mm}"),
            };

            var sql = new DataControl();
            sql.ExecuteNonQuery(strsql, parameters);
            RefreshView();

            string sql0 = $"select * from ProjectQuotation where SampleType='{SampleType.Text}' and PowerType='{PowerType2.Text}' and FuelType='{FuelType.Text}' and YNDirect='{YNDirect2.Text}' and ItemBrief='{ItemBrief.Text}' and StandardStage='{StandardStage.Text}' and  Remark='{Remark.Text}' and ItemName='{ItemName.Text}' and InspectionBasis1='{InspectionBasis1.Text}' and InspectionBasis2='{InspectionBasis2.Text}' and ItemCode='{ItemCode.Text}' and SingleTestMileage='{SingleTestMileage.Text}' and SinglePretreatmentMileage='{SinglePretreatmentMileage.Text}' and Price='{ProjectPrice.Text}' and PriceUnit='{PriceUnit.Text}' and TestCycle='{TestCycle.Text}' and ItemRemark='{ItemRemark.Text}'";
            if (SqlHelper.GetList(sql0).Rows.Count == 0)
            {
                if (MessageBox.Show("当前项目报价板块不包含该项目信息，是否在项目报价板块添加该项目信息", " 提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    string stringadd = $"insert into ProjectQuotation(SampleType,PowerType,FuelType,YNDirect,ItemBrief,StandardStage,Remark,ItemName,InspectionBasis1,InspectionBasis2,ItemCode,SingleTestMileage,SinglePretreatmentMileage,Price,PriceUnit,TestCycle,ItemRemark) values('{SampleType.Text}','{PowerType2.Text}','{FuelType.Text}','{YNDirect2.Text}','{ItemBrief.Text}','{StandardStage.Text}','{Remark.Text}','{ItemName.Text}','{InspectionBasis1.Text}','{InspectionBasis2.Text}','{ItemCode.Text}','{SingleTestMileage.Text}','{SinglePretreatmentMileage.Text}','{ProjectPrice.Text}','{PriceUnit.Text}','{TestCycle.Text}','{ItemRemark.Text}')";
                    if (SqlHelper.ExecuteNonquery(stringadd, System.Data.CommandType.Text) > 0)
                    {
                        MessageBox.Show("添加项目信息成功");
                    }
                }

            }
        }

        private void BtnUpdateClick(object sender, EventArgs e)
        {

            if (MessageBox.Show("是否更新选中行？", " 提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            Regex reg = new Regex("^[0-9]+$");
            Match ma1 = reg.Match(this.ProjectPrice.Text);
            Match ma2 = reg.Match(this.SingleTestMileage.Text);
            Match ma3 = reg.Match(this.SinglePretreatmentMileage.Text);
            if (!ma1.Success && this.ProjectPrice.Text != "")
            {
                MessageBox.Show("项目单价格式不正确");
                return;
            }
            if (!ma2.Success && this.SingleTestMileage.Text != "")
            {
                MessageBox.Show("单次试验里程格式不正确");
                return;
            }
            if (!ma3.Success && this.SinglePretreatmentMileage.Text != "")
            {
                MessageBox.Show("单次预处理里程格式不正确");
                return;
            }

            var items = taskpreview.listView1.SelectedItems;
            if (items.Count == 0)
                return;

            foreach (var control in TitleControls)
            {
                taskpreview.SetItem(items[0], control);
            }

            taskpreview.UpdateItem(items[0]);
            RefreshView();
        }

        private void BtnDeleteClick(object sender, EventArgs e)
        {
            var items = taskpreview.listView1.SelectedItems;
            if (items.Count == 0)
                return;

            var id = items[0].SubItems[0].Text.Trim();
            var department = items[0].SubItems[1].Text.Trim();
            if (MessageBox.Show($"将要删除ID={id}的{department}试验\n删除后不能恢复, 是否继续？", "提醒",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                return;

            var strsql = $"delete TestStatistic where ID={id}";

            var sql = new DataControl();
            sql.ExecuteNonQuery(strsql);
            RefreshView();
        }

        private void RefreshView()
        {
            taskpreview.InitView(VIN);
            //Grid.SetRowCellValue(Hand, "Reportnum", taskPreview1.TotalCount);
        }

        /// <summary>
        /// 筛选项目简称  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        
        private void ItemBriefHandle(object sender, EventArgs e)
        {

            string[] fuellist = new string[] { "汽油", "柴油", "甲醇", "天燃气", "LNG", "LPG", };
            string fueltype0 = "";
            for (int i = 0; i < fuellist.Length; i++)
            {
                if (FuelType.Text.Contains(fuellist[i]))
                {
                    fueltype0 = fuellist[i];
                }
            }


            string sql = $"select *  from ProjectQuotation where  FuelType ='{fueltype0}'  and SampleType='{SampleType.Text}' and YNDirect ='{YNDirect2.Text}' and PowerType='{PowerType2.Text}' and ItemBrief = '{ItemBrief.Text}'";
            DataTable da = SqlHelper.GetList(sql);
            if (da.Rows.Count > 0)
            {
                SampleType.SetValue(da.Rows[0]["SampleType"].ToString());
                StandardStage.SetValue(da.Rows[0]["StandardStage"].ToString());
                ProjectPrice.SetValue(da.Rows[0]["Price"].ToString());

                PowerType2.SetValue(da.Rows[0]["PowerType"].ToString());
                FuelType.SetValue(da.Rows[0]["FuelType"].ToString());
                YNDirect2.SetValue(da.Rows[0]["YNDirect"].ToString());
                ItemRemark.SetValue(da.Rows[0]["ItemRemark"].ToString());
                ItemName.SetValue(da.Rows[0]["ItemName"].ToString());
                InspectionBasis1.SetValue(da.Rows[0]["InspectionBasis1"].ToString());
                InspectionBasis2.SetValue(da.Rows[0]["InspectionBasis2"].ToString());
                ItemCode.SetValue(da.Rows[0]["ItemCode"].ToString());
                SingleTestMileage.SetValue(da.Rows[0]["SingleTestMileage"].ToString());
                SinglePretreatmentMileage.SetValue(da.Rows[0]["SinglePretreatmentMileage"].ToString());
                PriceUnit.SetValue(da.Rows[0]["PriceUnit"].ToString());
                TestCycle.SetValue(da.Rows[0]["TestCycle"].ToString());
                Remark.SetValue(da.Rows[0]["Remark"].ToString());
            }
        }

        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void tableLayoutPanel1_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void tableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
        
        }

       
        private void ItemBrief_Load(object sender, EventArgs e)
        {

        }

        private void TransmissionType_Load(object sender, EventArgs e)
        {

        }

        private void taskpreview_Load(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void titleCombox1_Load(object sender, EventArgs e)
        {

        }
    }
}
