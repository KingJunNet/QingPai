using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Controls;

namespace TaskManager
{
    public partial class AllocateAgain : Form
    {
        public string VIN; //实际代表VIN

        private readonly GridView Grid;

        private readonly int Hand;

        public List<TitleControl> TitleControls;

        public AllocateAgain(GridView grid, int hand)
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
            Text = $"试验再分配: 请重新选择试验组别，时间，地点";
            comboxDepartment.SetItems(FormSignIn.UserDic.Keys.ToList());
            ExperimentalSite.SetItems(Form1.ComboxDictionary["实验地点"]);
            //ExperimentalSite.SetItems(Form1.ComboxDictionary["生产厂家"]);
            //Registrant.SetItems(Form1.ComboxDictionary["报告类型"]);

            // 从报告任务中读取数据
            comboxDepartment.SetValue(Grid.GetRowCellValue(Hand, "department").ToString().Trim());
            ExperimentalSite.SetValue(Grid.GetRowCellValue(Hand, "ExperimentalSite").ToString().Trim());
            //LocationNumber.SetValue(Grid.GetRowCellValue(Hand, "LocationNumber").ToString().Trim());
            //Registrant.SetValue(Grid.GetRowCellValue(Hand, "Registrant").ToString().Trim());
            //TaskcodeCombox.SetValue(Grid.GetRowCellValue(Hand, "Carvin").ToString().Trim());
            //SampleType.SetValue(Grid.GetRowCellValue(Hand, "SampleType").ToString().Trim());
            //ItemType.SetValue(Grid.GetRowCellValue(Hand, "ItemType").ToString().Trim());
            //ItemBrief.SetValue(Grid.GetRowCellValue(Hand, "ItemBrief").ToString().Trim());
            //ExperimentalSite.SetValue(Grid.GetRowCellValue(Hand, "Producer").ToString().Trim());
            //comboxCarType.SetValue(Grid.GetRowCellValue(Hand, "Cartype").ToString().Trim());
            starttime.SetValue(Grid.GetRowCellValue(Hand, "TestStartDate").ToString().Trim());
            
            //comboxReporter.SetReadOnly(true);

            RefreshView();
           

            TitleControls = new List<TitleControl>();
            foreach (var control in tableLayoutPanel1.Controls)
            {
                if (control is TitleControl c)
                    TitleControls.Add(c);
            }
        }

        private void AllocateTaskDialog_Shown(object sender, EventArgs e)
        {
            Log.i("进行任务再分配","试验统计");
        }

        /// <summary>
        /// 选中试验任务后显示在面板上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
  
        }

        private void BtnUpdateClick(object sender, EventArgs e)
        {
        
        }

        private void BtnDeleteClick(object sender, EventArgs e)
        {
          
        }

        private void RefreshView()
        {
        
            //Grid.SetRowCellValue(Hand, "Reportnum", taskPreview1.TotalCount);
        }

        /// <summary>
        /// 试验再分配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
   
            Grid.AddNewRow();
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "LocationNumber", Grid.GetRowCellValue(Hand, "LocationNumber").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "Registrant", Grid.GetRowCellValue(Hand, "Registrant").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "SampleType", Grid.GetRowCellValue(Hand, "SampleType").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "ItemType", Grid.GetRowCellValue(Hand, "ItemType").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "ItemBrief", Grid.GetRowCellValue(Hand, "ItemBrief").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "SampleModel", Grid.GetRowCellValue(Hand, "SampleModel").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "Producer", Grid.GetRowCellValue(Hand, "Producer").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "Carvin", Grid.GetRowCellValue(Hand, "Carvin").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "YNDirect", Grid.GetRowCellValue(Hand, "YNDirect").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "PowerType", Grid.GetRowCellValue(Hand, "PowerType").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "TransmissionType", Grid.GetRowCellValue(Hand, "TransmissionType").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "EngineModel", Grid.GetRowCellValue(Hand, "EngineModel").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "EngineProduct", Grid.GetRowCellValue(Hand, "EngineProduct").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "Drivertype", Grid.GetRowCellValue(Hand, "Drivertype").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "StandardStage", Grid.GetRowCellValue(Hand, "StandardStage").ToString().Trim());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "ProjectPrice", Grid.GetRowCellValue(Hand, "ProjectPrice").ToString().Trim());
            

            Grid.SetRowCellValue(Grid.FocusedRowHandle,"department",comboxDepartment.Text.ToString());
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "ExperimentalSite", ExperimentalSite.Text.ToString());
            
            Grid.SetRowCellValue(Grid.FocusedRowHandle, "TestStartDate", starttime.Value().ToString());
            Grid.MoveLast();
            this.Close();
        }
    }
}
