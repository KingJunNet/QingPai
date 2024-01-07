using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using LabSystem.DAL;
using TaskManager.controller;
using TaskManager.主界面;
using Xfrog.Net;
using Word = Microsoft.Office.Interop.Word;

namespace TaskManager
{
    public partial class SampleForm : BaseForm
    {

        public const string RootFolder = "轻排参数表服务器";
        public readonly string Server;
        public string Folder => $"\\\\{Server}\\{RootFolder}\\参数表";
        public SampleForm()
        {
            InitializeComponent();

        }

        public SampleForm(FormType formType, string selectedDept) : base(formType, selectedDept)
        {
            InitializeComponent();
            var sql = new DataControl();
            Server = sql.ServerIP;
            if (!Server.EndsWith("\\"))
                Server += "\\";

            this._control.afterSavedHandle = new TableControl.AfterSavedEvent(handleAfterSaved);
        }


        protected override void InitUi()
        {
            var year = DateTime.Now.Year.ToString();

            textYear.Visibility = BarItemVisibility.Never;
            comboxState.Visibility = BarItemVisibility.Never;
            startdate.Visibility = BarItemVisibility.Never;
            enddate.Visibility = BarItemVisibility.Never;
            textYear.EditValue = year;
            comboxState.EditValue = "所有";

            //隐藏分配试验任务功能
            btnAssignTestTask.Visibility = BarItemVisibility.Never;

            //蒸发组.Visible = !string.IsNullOrWhiteSpace(Department) && Department.Equals("蒸发组");
            barButtonItem2.Enabled = FormTable.Add;
            barButtonItem3.Enabled = FormTable.Add;

            _control._view.RowStyle += ViewOnRowStyle;
            _control._view.RowCellStyle += ViewRowCellStyle;

            _control._view.FocusedRowChanged += SelectChanged;

           

            _control._view.Columns["VIN"].OptionsColumn.ReadOnly = true;

            //设置字段只读
            _control._view.Columns["VehicleModel"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["SampleProducter"].OptionsColumn.ReadOnly = true;
           
            _control._view.Columns["DirectInjection"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["PowerType"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["EngineModel"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["GearboxForm"].OptionsColumn.ReadOnly = true;

            _control._view.Columns["EngineProducter"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["FuelLabel"].OptionsColumn.ReadOnly = true;



            _control._view.Columns["SampleType"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["CarType"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["VehicleQuality"].OptionsColumn.ReadOnly = true;

            _control._view.Columns["VehicleMaxQuality"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["OptionalQuality"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["DesignPeopleCount"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["DriveFormParameter"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["FetalPressureParameter"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["FuelType"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["NationalSixCoasting"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["OilSupplyType"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["NationalFiveCoasting"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["CarbonCanisterForm1"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["CarbonCanisterForm2"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["CarbonCanisterNum1"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["CarbonCanisterNum2"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["ControlSystemType"].OptionsColumn.ReadOnly = true;
            
            _control._view.Columns["CarbonCanisterProductor1"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["CarbonCanisterProductor2"].OptionsColumn.ReadOnly = true;
            _control._view.Columns["OBDProductor"].OptionsColumn.ReadOnly = true;

            _control._view.Columns["SampleCount"].OptionsColumn.ReadOnly = true;


            _control._view.InitNewRow += InitNewRow;
            _control._view.CellValueChanged += CellValueChanged;
        }

        private void handleAfterSaved()
        {
            CacheDataHandler.Instance.asyncLoadVins();
        }

        private void _view_InitNewRow(object sender, InitNewRowEventArgs e)
        {
             
        }

        private void CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if(e.Column.FieldName != "RegPeople")
            {
                _control._view.SetRowCellValue(e.RowHandle, "RegPeople", FormSignIn.CurrentUser.Name.ToString());
            }
            

        }
            /// <summary>
            /// 初始新行
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
        private void InitNewRow(object sender, InitNewRowEventArgs e)
        {
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["SampleType"], "整车");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["EmissionTimeLimit"], "—");

            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["SampleCount"], "1");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["CarbonCanisterForm1"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["CarbonCanisterForm2"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["CarbonCanisterNum1"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["CarbonCanisterNum2"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["ControlSystemType"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["CarbonCanisterProductor1"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["CarbonCanisterProductor2"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["OBDProductor"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["BOBForm"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["Remark"], "—");
        }
        private void SelectChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0 && Filter.state == "1")
            {

                
                Filter.filterText = _control._view.GetRowCellValue(e.FocusedRowHandle, "VIN")?.ToString().Trim();
            }


        }

        /// <summary>
        /// 单元格变色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewRowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                var consistent = _control._view.GetRowCellValue(e.RowHandle, "consistent")?.ToString().Trim();
                if (consistent != null && consistent.Contains(","))
                {
                    string[] name = consistent.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (((IList)name).Contains(e.Column.FieldName))
                    {

                        e.Appearance.BackColor = Color.Red;

                    }
                }
                //var CreatePeople = _control._view.GetRowCellValue(e.RowHandle, "CreatePeople")?.ToString().Trim();
                //var RegPeople = _control._view.GetRowCellValue(e.RowHandle, "RegPeople")?.ToString().Trim();
                //var UpdateDate = _control._view.GetRowCellValue(e.RowHandle, "UpdateDate")?.ToString().Trim();

                //var SampleType = _control._view.GetRowCellValue(e.RowHandle, "SampleType")?.ToString().Trim();
                //var CarType = _control._view.GetRowCellValue(e.RowHandle, "CarType")?.ToString().Trim();
                //var SampleProducter = _control._view.GetRowCellValue(e.RowHandle, "SampleProducter")?.ToString().Trim();
                //var VehicleModel = _control._view.GetRowCellValue(e.RowHandle, "VehicleModel")?.ToString().Trim();
                //var VIN = _control._view.GetRowCellValue(e.RowHandle, "VIN")?.ToString().Trim();
                //var VehicleQuality = _control._view.GetRowCellValue(e.RowHandle, "VehicleQuality")?.ToString().Trim();
                //var VehicleMaxQuality = _control._view.GetRowCellValue(e.RowHandle, "VehicleMaxQuality")?.ToString().Trim();
                //var OptionalQuality = _control._view.GetRowCellValue(e.RowHandle, "OptionalQuality")?.ToString().Trim();
                //var ConversionLoadingQuality = _control._view.GetRowCellValue(e.RowHandle, "ConversionLoadingQuality")?.ToString().Trim();

                //var DesignPeopleCount = _control._view.GetRowCellValue(e.RowHandle, "DesignPeopleCount")?.ToString().Trim();
                //var DriveFormTask = _control._view.GetRowCellValue(e.RowHandle, "DriveFormTask")?.ToString().Trim();
                //var GearboxForm = _control._view.GetRowCellValue(e.RowHandle, "GearboxForm")?.ToString().Trim();
                //var FetalPressureTask = _control._view.GetRowCellValue(e.RowHandle, "FetalPressureTask")?.ToString().Trim();
                //var FuelType = _control._view.GetRowCellValue(e.RowHandle, "FuelType")?.ToString().Trim();
                //var FuelLabel = _control._view.GetRowCellValue(e.RowHandle, "FuelLabel")?.ToString().Trim();
                //var PowerType = _control._view.GetRowCellValue(e.RowHandle, "PowerType")?.ToString().Trim();
                //var EngineModel = _control._view.GetRowCellValue(e.RowHandle, "EngineModel")?.ToString().Trim();

                //var EngineProducter = _control._view.GetRowCellValue(e.RowHandle, "EngineProducter")?.ToString().Trim();
                //var NationalSixCoasting = _control._view.GetRowCellValue(e.RowHandle, "NationalSixCoasting")?.ToString().Trim();
                //var NationalFiveCoasting = _control._view.GetRowCellValue(e.RowHandle, "NationalFiveCoasting")?.ToString().Trim();
                //var EmissionTimeLimit = _control._view.GetRowCellValue(e.RowHandle, "EmissionTimeLimit")?.ToString().Trim();


                //if (CreatePeople == "" && e.Column.Caption == "创建人")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}

                //if (RegPeople == "" && e.Column.Caption == "登记人")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}

                //if (SampleType == "" && e.Column.Caption == "样品类型")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}

                //if (UpdateDate == "" && e.Column.Caption == "更新日期")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}

                //if (CarType == "" && e.Column.Caption == "车辆类型")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}

                //if (SampleProducter == "" && e.Column.Caption == "样品生产厂家")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (VehicleModel == "" && e.Column.Caption == "车辆型号")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (VIN == "" && e.Column.Caption == "VIN")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (VehicleQuality == "" && e.Column.Caption == "整车整备质量")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (VehicleMaxQuality == "" && e.Column.Caption == "整车最大总质量")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (OptionalQuality == "" && e.Column.Caption == "选装装备质量")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (ConversionLoadingQuality == "" && e.Column.Caption == "转股加载质量")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (DesignPeopleCount == "" && e.Column.Caption == "设计乘员数")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (DriveFormTask == "" && e.Column.Caption == "驱动形式（试验）")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (GearboxForm == "" && e.Column.Caption == "变速器型式")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (FetalPressureTask == "" && e.Column.Caption == "胎压（试验）")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (FuelType == "" && e.Column.Caption == "燃油种类")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (FuelLabel == "" && e.Column.Caption == "燃油标号")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (PowerType == "" && e.Column.Caption == "动力类型")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (EngineModel == "" && e.Column.Caption == "发动机型号")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (EngineProducter == "" && e.Column.Caption == "发动机生产厂")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (NationalSixCoasting == "" && e.Column.Caption == "国六道路系数")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (NationalFiveCoasting == "" && e.Column.Caption == "国五道路系数")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
                //if (EmissionTimeLimit == "" && e.Column.Caption == "排放限值档")
                //{
                //    e.Appearance.BackColor = Color.LightSalmon;
                //}
            }
    
        }
        /// <summary>
        /// 行变色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewOnRowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
               

                //for (int j = 1; j < _control._view.Columns.Count; j++)
                //{
                //    if (_control._view.Columns[j].ToString() == "BOB型号" && _control._view.GetRowCellValue(e.RowHandle, _control._view.Columns[j])?.ToString().Trim() == "")
                //    {
                //        _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns[j], "—");
                //    }
                //    if (_control._view.Columns[j].ToString() == "排放限值档" && _control._view.GetRowCellValue(e.RowHandle, _control._view.Columns[j])?.ToString().Trim() == "")
                //    {
                //        _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns[j], "—");
                //    }
                //    if (_control._view.Columns[j].ToString() == "备注" && _control._view.GetRowCellValue(e.RowHandle, _control._view.Columns[j])?.ToString().Trim() == "")
                //    {
                //        _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns[j], "—");
                //    }
                //}

                e.Appearance.BackColor = Color.FromArgb(193, 255, 193);
                //e.Appearance.BackColor = Color.LightGreen;

                //var CreatePeople = _control._view.GetRowCellValue(e.RowHandle, "CreatePeople")?.ToString().Trim();
                //var RegPeople = _control._view.GetRowCellValue(e.RowHandle, "RegPeople")?.ToString().Trim();
                //var UpdateDate = _control._view.GetRowCellValue(e.RowHandle, "UpdateDate")?.ToString().Trim();

                //var SampleType = _control._view.GetRowCellValue(e.RowHandle, "SampleType")?.ToString().Trim();
                //var CarType = _control._view.GetRowCellValue(e.RowHandle, "CarType")?.ToString().Trim();
                //var SampleProducter = _control._view.GetRowCellValue(e.RowHandle, "SampleProducter")?.ToString().Trim();
                //var VehicleModel = _control._view.GetRowCellValue(e.RowHandle, "VehicleModel")?.ToString().Trim();
                //var VIN = _control._view.GetRowCellValue(e.RowHandle, "VIN")?.ToString().Trim();
                //var VehicleQuality = _control._view.GetRowCellValue(e.RowHandle, "VehicleQuality")?.ToString().Trim();
                //var VehicleMaxQuality = _control._view.GetRowCellValue(e.RowHandle, "VehicleMaxQuality")?.ToString().Trim();
                //var OptionalQuality = _control._view.GetRowCellValue(e.RowHandle, "OptionalQuality")?.ToString().Trim();
                //var ConversionLoadingQuality = _control._view.GetRowCellValue(e.RowHandle, "ConversionLoadingQuality")?.ToString().Trim();

                //var DesignPeopleCount = _control._view.GetRowCellValue(e.RowHandle, "DesignPeopleCount")?.ToString().Trim();
                //var DriveFormTask = _control._view.GetRowCellValue(e.RowHandle, "DriveFormTask")?.ToString().Trim();
                //var GearboxForm = _control._view.GetRowCellValue(e.RowHandle, "GearboxForm")?.ToString().Trim();
                //var FetalPressureTask = _control._view.GetRowCellValue(e.RowHandle, "FetalPressureTask")?.ToString().Trim();
                //var FuelType = _control._view.GetRowCellValue(e.RowHandle, "FuelType")?.ToString().Trim();
                //var FuelLabel = _control._view.GetRowCellValue(e.RowHandle, "FuelLabel")?.ToString().Trim();
                //var PowerType = _control._view.GetRowCellValue(e.RowHandle, "PowerType")?.ToString().Trim();
                //var EngineModel = _control._view.GetRowCellValue(e.RowHandle, "EngineModel")?.ToString().Trim();
                //var EngineProducter = _control._view.GetRowCellValue(e.RowHandle, "EngineProducter")?.ToString().Trim();
                //var NationalSixCoasting = _control._view.GetRowCellValue(e.RowHandle, "NationalSixCoasting")?.ToString().Trim();
                //var NationalFiveCoasting = _control._view.GetRowCellValue(e.RowHandle, "NationalFiveCoasting")?.ToString().Trim();
                //var EmissionTimeLimit = _control._view.GetRowCellValue(e.RowHandle, "EmissionTimeLimit")?.ToString().Trim();

                //if (CreatePeople==""|| RegPeople=="" || UpdateDate=="" || SampleType == "" || CarType == "" || SampleProducter == "" || VehicleModel == "" || VIN == "" || VehicleQuality == "" || VehicleMaxQuality == "" || OptionalQuality == "" || ConversionLoadingQuality == "" || DesignPeopleCount == "" || DriveFormTask == "" || GearboxForm == "" || FetalPressureTask == "" || FuelType == "" || FuelLabel == "" || PowerType == "" || EngineModel == "" || NationalSixCoasting == "" || NationalFiveCoasting == "" || EmissionTimeLimit == "" || EngineProducter =="")
                //{
                //    e.Appearance.BackColor = Color.LightGray;
                //}
                //else
                //{
                //    e.Appearance.BackColor = Color.LightGreen;
                //}

                for (int j = 1; j < _control._view.Columns.Count; j++)
                {
                    if (_control._view.GetRowCellValue(e.RowHandle, _control._view.Columns[j])?.ToString().Trim() == "" && _control._view.Columns[j].ToString()!= "consistent")
                    {
                        //if (_control._view.GetRowCellValue(e.RowHandle, "consistent").ToString().Trim() != "")
                        //{
                            e.Appearance.BackColor = Color.FromArgb(237, 237, 237);
                        //}
                    }
                }

                if (_control._view.GetRowCellValue(e.RowHandle, "consistent").ToString().Trim().Contains("否"))
                {
                    e.Appearance.BackColor = Color.Orange;
                }
            }
               

            //if (consistent == "否")
            //{

            //    e.Appearance.BackColor = Color.Yellow;
            //}
            //else if(consistent == "是")
            //{

            //    e.Appearance.BackColor = Color.Green;
            //}



        }

        protected override DialogResult OpenEditForm(GridView view, int hand, List<DataField> fields)
        {
            Log.e("OpenEditForm");
            var isAllocateTask = false;
            var dialog = new SampleEditDialog(FormTable.Edit, view, hand, fields,FormType.Sample, isAllocateTask);
            //dialog.freshcorrect += Website_freshsample;

            if (dialog.ShowDialog() == DialogResult.OK )
            {
                if (hand >= 0)
                {
                    if (MessageBox.Show("是否校验样品信息？", " 提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        barButtonItem14_ItemClick(null, null);
                    }
                }
               
                return DialogResult.OK;
            }
            else
            {
                return DialogResult.Cancel;
            }
            
        }
        /// <summary>
        /// 事件委托 样品校验
        /// </summary>
        private void Website_freshsample()
        {
            barButtonItem14_ItemClick(null, null);


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

        }
        private Thread childThread;
        private void TaskForm_Load(object sender, EventArgs e)
        {
           
            //ThreadStart childref = new ThreadStart(CallToChildThread);
          
            //childThread = new Thread(childref);
            //childThread.Start();
        }

        /// <summary>
        /// 滚动条至最左
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void barButtonItemleft_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (Templatecolumn.column == null || Templatecolumn.name == "默认模板")
            {

                _control._view.FocusedColumn = _control._view.Columns["CreatePeople"];
            }
            else
            {
                _control._view.FocusedColumn = _control._view.Columns[Templatecolumn.column[0]];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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





        /// <summary>
        /// 子线程，获取Lims系统数据
        /// </summary>
        public  void CallToChildThread()
        {
            while (true)
            {

                Thread.Sleep(2000000);
            }
        }

        /// <summary>
        /// 1983参数表
        /// </summary>
        /// <param name="fileName"></param>
        public void GetWordData83(string[] fileName) {
            SplashScreenManager.ShowForm(typeof(WaitForm1));
      
            MessageFilter.Register();

            //var files = Directory.GetFiles(fileName);
            var files = fileName;

            

            for (var i = 0; i < files.Length; i++)
            {
                Word.Application app = new Word.Application();//可以打开word
                Word.Document doc = null;      //需要记录打开的word

                object missing = System.Reflection.Missing.Value;
                object file = files[i];
                object readOnly = true;//不是只读
                object isVisible = false;

                //object unknow = Type.Missing;

                try
                {
                    doc = app.Documents.Open(ref file, ref missing, ref readOnly,
                     ref missing, ref missing, ref missing, ref missing, ref missing,
                     ref missing, ref missing, ref missing, ref isVisible, ref missing,
                     ref missing, ref missing, ref missing);
                    string SampleType ="整车";
              
                    string CarType = doc.Tables[1].Cell(3, 3).Range.Text.Remove(doc.Tables[1].Cell(3, 3).Range.Text.Length - 1).Trim();
                    string SampleProductor = doc.Tables[1].Cell(5, 3).Range.Text.Remove(doc.Tables[1].Cell(5, 3).Range.Text.Length - 1).Trim();
                    string VehicleModel = doc.Tables[1].Cell(4, 5).Range.Text.Remove(doc.Tables[1].Cell(4, 5).Range.Text.Length - 1).Trim();
                    string VIN = doc.Tables[1].Cell(6, 4).Range.Text.Remove(doc.Tables[1].Cell(6, 4).Range.Text.Length - 1).Trim();

                    string VehicleQuality = doc.Tables[1].Cell(9, 3).Range.Text.Remove(doc.Tables[1].Cell(9, 3).Range.Text.Length - 1).Trim();
                    string VehicleMaxQuality = doc.Tables[1].Cell(10, 3).Range.Text.Remove(doc.Tables[1].Cell(10, 3).Range.Text.Length - 1).Trim();
                    string OptionalQuality = doc.Tables[1].Cell(13, 3).Range.Text.Remove(doc.Tables[1].Cell(13, 3).Range.Text.Length - 1).Trim();
                    
                    string DesignPeopleCount = doc.Tables[1].Cell(16, 3).Range.Text.Remove(doc.Tables[1].Cell(16, 3).Range.Text.Length - 1).Trim();
                    string DriveFormParameter = doc.Tables[1].Cell(19,3).Range.Text.Remove(doc.Tables[1].Cell(19, 3).Range.Text.Length - 1).Trim();
                 
                    string GearboxForm = doc.Tables[1].Cell(20, 3).Range.Text.Remove(doc.Tables[1].Cell(20, 3).Range.Text.Length - 1).Trim();
                    string FetalPressureParameter = doc.Tables[1].Cell(21, 3).Range.Text.Remove(doc.Tables[1].Cell(21, 3).Range.Text.Length - 1).Trim();
                
                    string FuelType = doc.Tables[1].Cell(25, 3).Range.Text.Remove(doc.Tables[1].Cell(25, 3).Range.Text.Length - 1).Trim();

                    string FuelLabel = doc.Tables[1].Cell(26, 3).Range.Text.Remove(doc.Tables[1].Cell(26, 3).Range.Text.Length - 1).Trim();
                    //string PowerType = doc.Tables[1].Cell(12, 6).Range.Text.Remove(doc.Tables[1].Cell(12, 6).Range.Text.Length - 1).Trim();
                    string EngineModel = doc.Tables[1].Cell(6, 7).Range.Text.Remove(doc.Tables[1].Cell(6, 7).Range.Text.Length - 1).Trim();
                    string EngineProducter = doc.Tables[1].Cell(8, 7).Range.Text.Remove(doc.Tables[1].Cell(8, 7).Range.Text.Length - 1).Trim();
                    string OilSupplyType = doc.Tables[1].Cell(37, 6).Range.Text.Remove(doc.Tables[1].Cell(37, 6).Range.Text.Length - 1).Trim();
                    //string DirectInjection = doc.Tables[1].Cell(13, 6).Range.Text.Remove(doc.Tables[1].Cell(13, 6).Range.Text.Length - 1).Trim();
                    string NationalSixCoastingA = doc.Tables[2].Cell(1, 2).Range.Text.Remove(doc.Tables[2].Cell(1, 2).Range.Text.Length - 1).Trim();
                    string NationalSixCoastingB = doc.Tables[2].Cell(1, 3).Range.Text.Remove(doc.Tables[2].Cell(1, 3).Range.Text.Length - 1).Trim();
                    string NationalSixCoastingC = doc.Tables[2].Cell(1, 4).Range.Text.Remove(doc.Tables[2].Cell(1, 4).Range.Text.Length - 1).Trim();

                    string NationalSixCoasting = NationalSixCoastingA +";"+ NationalSixCoastingB +";"+ NationalSixCoastingC+";";

                    string NationalFiveCoastingA = doc.Tables[2].Cell(2, 2).Range.Text.Remove(doc.Tables[2].Cell(2, 2).Range.Text.Length - 1).Trim();
                    string NationalFiveCoastingB = doc.Tables[2].Cell(2, 3).Range.Text.Remove(doc.Tables[2].Cell(2, 3).Range.Text.Length - 1).Trim();
                    string NationalFiveCoastingC = doc.Tables[2].Cell(2, 4).Range.Text.Remove(doc.Tables[2].Cell(2, 4).Range.Text.Length - 1).Trim();

                    string NationalFiveCoasting = NationalFiveCoastingA +";" +NationalFiveCoastingB +";"+ NationalFiveCoastingC+";";
                    //string EmissionTimeLimit = doc.Tables[1].Cell(13, 6).Range.Text.Remove(doc.Tables[1].Cell(13, 6).Range.Text.Length - 1).Trim();
                    string CarbonCanisterForm1 = doc.Tables[1].Cell(90, 4).Range.Text.Remove(doc.Tables[1].Cell(90, 4).Range.Text.Length - 1).Trim();
                    string CarbonCanisterForm2 = doc.Tables[1].Cell(97, 4).Range.Text.Remove(doc.Tables[1].Cell(97, 4).Range.Text.Length - 1).Trim();
                    string CarbonCanisterNum1 = doc.Tables[7].Cell(2, 2).Range.Text.Remove(doc.Tables[7].Cell(2, 2).Range.Text.Length - 1).Trim();
                    string CarbonCanisterNum2 = doc.Tables[7].Cell(2, 2).Range.Text.Remove(doc.Tables[7].Cell(2, 2).Range.Text.Length - 1).Trim();
                    //string SampleCount = doc.Tables[1].Cell(13, 6).Range.Text.Remove(doc.Tables[1].Cell(13, 6).Range.Text.Length - 1).Trim();
                    string ControlSystemType = doc.Tables[1].Cell(43,3).Range.Text.Remove(doc.Tables[1].Cell(43, 3).Range.Text.Length - 1).Trim();

                    string CarbonCanisterProductor1 = doc.Tables[1].Cell(95, 4).Range.Text.Remove(doc.Tables[1].Cell(95, 4).Range.Text.Length - 1).Trim();
                    string CarbonCanisterProductor2 = doc.Tables[1].Cell(102, 4).Range.Text.Remove(doc.Tables[1].Cell(102, 4).Range.Text.Length - 1).Trim();
                    string OBDProductor = doc.Tables[1].Cell(95, 7).Range.Text.Remove(doc.Tables[1].Cell(95, 7).Range.Text.Length - 1).Trim();


                    bool YN = true;//判断是否为新增
                    for (int j = 0; j < _control._view.RowCount; j++)
                    {

                        if (_control._view.GetRowCellValue(j, "VIN").ToString() == VIN)
                        {
                            YN = false;

                            if (MessageBox.Show($"{VIN}样品信息已存在，是否合并到原有样品信息中?", "提示", MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                

                                _control._view.SetRowCellValue(j, "SampleType", SampleType);
                                _control._view.SetRowCellValue(j, "CarType", CarType);
                                _control._view.SetRowCellValue(j, "SampleProducter", SampleProductor);
                                _control._view.SetRowCellValue(j, "VehicleModel", VehicleModel);
                                _control._view.SetRowCellValue(j, "VIN", VIN);
                                _control._view.SetRowCellValue(j, "VehicleQuality", VehicleQuality);
                                _control._view.SetRowCellValue(j, "VehicleMaxQuality", VehicleMaxQuality);
                                _control._view.SetRowCellValue(j, "OptionalQuality", OptionalQuality);
                                _control._view.SetRowCellValue(j, "DesignPeopleCount", DesignPeopleCount);
                                _control._view.SetRowCellValue(j, "DriveFormParameter", DriveFormParameter);
                                _control._view.SetRowCellValue(j, "GearboxForm", GearboxForm);
                                _control._view.SetRowCellValue(j, "FetalPressureParameter", FetalPressureParameter);
                                _control._view.SetRowCellValue(j, "FuelType", FuelType);
                                _control._view.SetRowCellValue(j, "FuelLabel", FuelLabel);
                                _control._view.SetRowCellValue(j, "EngineModel", EngineModel);
                                _control._view.SetRowCellValue(j, "EngineProducter", EngineProducter);
                                _control._view.SetRowCellValue(j, "OilSupplyType", OilSupplyType);
                                _control._view.SetRowCellValue(j, "NationalSixCoasting", NationalSixCoasting);
                                //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalSixCoastingB", NationalSixCoastingB);
                                //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalSixCoastingC", NationalSixCoastingC);
                                _control._view.SetRowCellValue(j, "NationalFiveCoasting", NationalFiveCoasting);
                                //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalFiveCoastingB", NationalFiveCoastingB);
                                //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalFiveCoastingC", NationalFiveCoastingC);
                                _control._view.SetRowCellValue(j, "CarbonCanisterForm1", CarbonCanisterForm1);
                                _control._view.SetRowCellValue(j, "CarbonCanisterForm2", CarbonCanisterForm2);
                                _control._view.SetRowCellValue(j, "CarbonCanisterNum1", CarbonCanisterNum1);
                                _control._view.SetRowCellValue(j, "CarbonCanisterNum2", CarbonCanisterNum2);
                                _control._view.SetRowCellValue(j, "ControlSystemType", ControlSystemType);
                                //_control._view.SetRowCellValue(j, "Brand", ControlSystemType);
                                _control._view.SetRowCellValue(j, "CarbonCanisterProductor1", CarbonCanisterProductor1);
                                _control._view.SetRowCellValue(j, "CarbonCanisterProductor2", CarbonCanisterProductor2);
                                _control._view.SetRowCellValue(j, "OBDProductor", OBDProductor);

                                _control._view.SetRowCellValue(j, "consistent", "83");



                                if (MessageBox.Show("是否将改动的样品信息同步到试验统计对应的信息？", " 提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {
                                    string sql = $"update TestStatistic  set SampleModel ='{VehicleModel}',Producer='{SampleProductor}',EngineModel='{ EngineModel}',EngineProduct='{EngineProducter}',FuelLabel='{FuelLabel}',FuelType='{FuelType}',CarType='{CarType}' where Carvin='{VIN}'";
                                    SqlHelper.ExecuteNonquery(sql, System.Data.CommandType.Text);
                                    MessageBox.Show("同步成功");
                                }

                            }
                            else
                            {
                                return;
                            }


                            
                        }
                       
                    }

                    if (YN)
                    {

                        _control._view.AddNewRow();
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CreatePeople", FormSignIn.CurrentUser.Name);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "RegPeople", FormSignIn.CurrentUser.Name);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "UpdateDate", DateTime.Now.ToString("yyyy/MM/dd"));
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "SampleType", SampleType);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarType", CarType);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "SampleProducter", SampleProductor);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "VehicleModel", VehicleModel);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "VIN", VIN);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "VehicleQuality", VehicleQuality);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "VehicleMaxQuality", VehicleMaxQuality);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "OptionalQuality", OptionalQuality);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "DesignPeopleCount", DesignPeopleCount);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "DriveFormParameter", DriveFormParameter);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "GearboxForm", GearboxForm);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "FetalPressureParameter", FetalPressureParameter);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "FuelType", FuelType);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "FuelLabel", FuelLabel);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "EngineModel", EngineModel);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "EngineProducter", EngineProducter);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "OilSupplyType", OilSupplyType);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalSixCoasting", NationalSixCoasting);
                        //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalSixCoastingB", NationalSixCoastingB);
                        //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalSixCoastingC", NationalSixCoastingC);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalFiveCoasting", NationalFiveCoasting);
                        //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalFiveCoastingB", NationalFiveCoastingB);
                        //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalFiveCoastingC", NationalFiveCoastingC);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterForm1", CarbonCanisterForm1);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterForm2", CarbonCanisterForm2);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterNum1", CarbonCanisterNum1);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterNum2", CarbonCanisterNum2);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "ControlSystemType", ControlSystemType);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "Brand", ControlSystemType);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterProductor1", CarbonCanisterProductor1);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterProductor2", CarbonCanisterProductor2);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "OBDProductor", OBDProductor);

                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "consistent", "83");



                        _control._view.MoveLast();



                    }

                    string folders = $"{Folder}\\{VIN}";
                    var dir = new DirectoryInfo(folders);
                    if (!dir.Exists)
                        dir.Create();

                    var target = $"{folders}\\{VIN}.doc";

                    if (File.Exists(target))
                    {
                        if (MessageBox.Show($"{target}已存在是否覆盖?", "提示", MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) != DialogResult.OK)
                            continue;
                    }

                    File.Copy(file.ToString(), target, true);

                }
                catch(Exception ex) {
                    MessageBox.Show(ex.ToString());
                }

                finally
                {
                    if (doc != null)
                    {
                        doc.Close(ref missing, ref missing, ref missing);
                        doc = null;
                    }

                    if (app != null)
                    {
                        app.Quit(ref missing, ref missing, ref missing);
                        app = null;
                    }

                    MessageFilter.Revoke();



                    SplashScreenManager.CloseForm();

                }

               
            }


            

        }
        /// <summary>
        /// 1982参数表
        /// </summary>
        /// <param name="fileName"></param>
        public void GetWordData82(string[] fileName)
        {
            SplashScreenManager.ShowForm(typeof(WaitForm1));
            MessageFilter.Register();
            var files = fileName;

            //string folders = Folder;
            //var dir = new DirectoryInfo(folders);
            //if (!dir.Exists)
            //    dir.Create();

            for (var i = 0; i < files.Length; i++)
            {
                Word.Application app = new Word.Application();//可以打开word
                Word.Document doc = null;      //需要记录打开的word

                object missing = System.Reflection.Missing.Value;
                object file = files[i];
                object readOnly = true;//不是只读
                object isVisible = false;

                //object unknow = Type.Missing;

                try
                {
                    doc = app.Documents.Open(ref file, ref missing, ref readOnly,
                     ref missing, ref missing, ref missing, ref missing, ref missing,
                     ref missing, ref missing, ref missing, ref isVisible, ref missing,
                     ref missing, ref missing, ref missing);
                    //string SampleType = doc.Tables[1].Cell(3, 3).Range.Text.Remove(doc.Tables[1].Cell(3, 3).Range.Text.Length - 1).Trim();

                    string SampleType = "炭罐";

                    //string CarType = doc.Tables[1].Cell(3, 3).Range.Text.Remove(doc.Tables[1].Cell(3, 3).Range.Text.Length - 1).Trim();
                    string SampleProductor = doc.Tables[1].Cell(6, 3).Range.Text.Remove(doc.Tables[1].Cell(6, 3).Range.Text.Length - 1).Trim();
                    string VehicleModel = doc.Tables[1].Cell(15, 3).Range.Text.Remove(doc.Tables[1].Cell(16, 3).Range.Text.Length - 1).Trim();
                    string VIN = VehicleModel;

                  
                    string CarbonCanisterForm1 = doc.Tables[1].Cell(4, 3).Range.Text.Remove(doc.Tables[1].Cell(4, 3).Range.Text.Length - 1).Trim();
                    //string CarbonCanisterForm2 = doc.Tables[1].Cell(97, 4).Range.Text.Remove(doc.Tables[1].Cell(97, 4).Range.Text.Length - 1).Trim();
                    string CarbonCanisterNum1 = doc.Tables[1].Cell(8, 3).Range.Text.Remove(doc.Tables[1].Cell(8, 3).Range.Text.Length - 1).Trim();
                    //string CarbonCanisterNum2 = doc.Tables[7].Cell(2, 2).Range.Text.Remove(doc.Tables[7].Cell(2, 2).Range.Text.Length - 1).Trim();
                    string SampleCount = doc.Tables[1].Cell(9, 3).Range.Text.Remove(doc.Tables[1].Cell(9, 3).Range.Text.Length - 1).Trim();
                    //string ControlSystemType = doc.Tables[1].Cell(43, 3).Range.Text.Remove(doc.Tables[1].Cell(43, 3).Range.Text.Length - 1).Trim();

                    string CarbonCanisterProductor1 = doc.Tables[1].Cell(6, 3).Range.Text.Remove(doc.Tables[1].Cell(6, 3).Range.Text.Length - 1).Trim();
                  

                    bool YN = true;//判断是否为新增
                    for (int j = 0; j < _control._view.RowCount; j++)
                    {

                        if (_control._view.GetRowCellValue(j, "VIN").ToString() == VIN)
                        {

                            YN = false;


                            if (MessageBox.Show($"{VIN}样品信息已存在，是否合并到原有样品信息中?", "提示", MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Warning) == DialogResult.OK)
                            {

                                _control._view.SetRowCellValue(j, "SampleType", SampleType);
                             
                                _control._view.SetRowCellValue(j, "SampleProducter", SampleProductor);
                                _control._view.SetRowCellValue(j, "VehicleModel", VehicleModel);
                                _control._view.SetRowCellValue(j, "VIN", VIN);
                                _control._view.SetRowCellValue(j, "CarbonCanisterForm1", CarbonCanisterForm1);
                                _control._view.SetRowCellValue(j, "CarbonCanisterNum1", CarbonCanisterNum1);
                                _control._view.SetRowCellValue(j, "SampleCount", SampleCount);
                                _control._view.SetRowCellValue(j, "CarbonCanisterProductor1", CarbonCanisterProductor1);

                                _control._view.SetRowCellValue(j, "consistent", "82");

                                if (MessageBox.Show("是否将改动的样品信息同步到试验统计对应的信息？", " 提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {
                                    string sql = $"update TestStatistic  set SampleModel ='{VehicleModel}',Producer='{SampleProductor}' where Carvin='{VIN}'";
                                    SqlHelper.ExecuteNonquery(sql, System.Data.CommandType.Text);
                                    MessageBox.Show("同步成功");
                                }

                            }
                            else
                            {
                                return;
                            }
                            //if (_control._view.GetRowCellValue(j, "SampleType").ToString() != SampleType ||  _control._view.GetRowCellValue(j, "SampleProductor").ToString() != SampleProductor || _control._view.GetRowCellValue(j, "VehicleModel").ToString() != VehicleModel || _control._view.GetRowCellValue(j, "VIN").ToString() != VIN || _control._view.GetRowCellValue(j, "CarbonCanisterForm1").ToString() != CarbonCanisterForm1  || _control._view.GetRowCellValue(j, "CarbonCanisterNum1").ToString() != CarbonCanisterNum1 ||  _control._view.GetRowCellValue(j, "SampleCount").ToString() != SampleCount || _control._view.GetRowCellValue(j, "CarbonCanisterProductor1").ToString() != CarbonCanisterProductor1)
                            //{
                            //    _control._view.SetRowCellValue(j, "Consistent", "否");
                            //}
                            //else if (_control._view.GetRowCellValue(j, "Consistent").ToString() == "否")
                            //{
                            //    _control._view.SetRowCellValue(j, "Consistent", "是");
                            //}
                        }

                    }

                    if (YN)
                    {
                        _control._view.AddNewRow();

                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CreatePeople", FormSignIn.CurrentUser.Name);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "RegPeople", FormSignIn.CurrentUser.Name);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "UpdateDate", DateTime.Now.ToString("yyyy/MM/dd"));

                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "SampleType", SampleType);
                      
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "SampleProducter", SampleProductor);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "VehicleModel", VehicleModel);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "VIN", VIN);
                   
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterForm1", CarbonCanisterForm1);
                
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterNum1", CarbonCanisterNum1);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "SampleCount", SampleCount);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterProductor1", CarbonCanisterProductor1);


                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "consistent", "82");


                    }

                    string folders = $"{Folder}\\{VIN}";
                    var dir = new DirectoryInfo(folders);
                    if (!dir.Exists)
                        dir.Create();

                    var target = $"{folders}\\{VIN}.doc";

                    if (File.Exists(target))
                    {
                        if (MessageBox.Show($"{target}已存在是否覆盖?", "提示", MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) != DialogResult.OK)
                            continue;
                    }

                    File.Copy(file.ToString(), target, true);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                finally
                {
                    if (doc != null)
                    {
                        doc.Close(ref missing, ref missing, ref missing);
                        doc = null;
                    }

                    if (app != null)
                    {
                        app.Quit(ref missing, ref missing, ref missing);
                        app = null;
                    }

                    MessageFilter.Revoke();



                    SplashScreenManager.CloseForm();

                }


            }


           

        }

        /// <summary>
        /// 1979参数表
        /// </summary>
        /// <param name="fileName"></param>
        public void GetWordData79(string[] fileName)
        {
            SplashScreenManager.ShowForm(typeof(WaitForm1));
             MessageFilter.Register();

            var files = fileName;

          

            for (var i = 0; i < files.Length; i++)
            {
   
                Word.Application app = new Word.Application();//可以打开word
                Word.Document doc = null;      //需要记录打开的word

                object missing = System.Reflection.Missing.Value;
                object file = files[i];
                object readOnly = true;//不是只读
                object isVisible = false;

                //object unknow = Type.Missing;
                string VIN;
                try
                {
                    doc = app.Documents.Open(ref file, ref missing, ref readOnly,
                     ref missing, ref missing, ref missing, ref missing, ref missing,
                     ref missing, ref missing, ref missing, ref isVisible, ref missing,
                     ref missing, ref missing, ref missing);
                    string SampleType = "整车";

                    string CarType = doc.Tables[1].Cell(6, 3).Range.Text.Remove(doc.Tables[1].Cell(6, 3).Range.Text.Length - 1).Trim();

                    string SampleProductor = doc.Tables[1].Cell(9, 3).Range.Text.Remove(doc.Tables[1].Cell(9, 3).Range.Text.Length - 1).Trim();
                    string VehicleModel = doc.Tables[1].Cell(7, 3).Range.Text.Remove(doc.Tables[1].Cell(7, 3).Range.Text.Length - 1).Trim();
                    VIN = doc.Tables[1].Cell(4, 3).Range.Text.Remove(doc.Tables[1].Cell(4, 3).Range.Text.Length - 1).Trim();

                    string VehicleQuality = doc.Tables[1].Cell(11, 3).Range.Text.Remove(doc.Tables[1].Cell(11, 3).Range.Text.Length - 1).Trim();
                    string VehicleMaxQuality = doc.Tables[1].Cell(12, 3).Range.Text.Remove(doc.Tables[1].Cell(12, 3).Range.Text.Length - 1).Trim();
                
                    string DriveFormParameter = doc.Tables[1].Cell(28, 3).Range.Text.Remove(doc.Tables[1].Cell(28, 3).Range.Text.Length - 1).Trim();
                    //  string DriveFormTask = doc.Tables[1].Cell(13, 6).Range.Text.Remove(doc.Tables[1].Cell(13, 6).Range.Text.Length - 1).Trim();

                    string GearboxForm = doc.Tables[1].Cell(26, 3).Range.Text.Remove(doc.Tables[1].Cell(26, 3).Range.Text.Length - 1).Trim();
                    string FetalPressureParameter = doc.Tables[1].Cell(30, 3).Range.Text.Remove(doc.Tables[1].Cell(30, 3).Range.Text.Length - 1).Trim();

                    string NationalSixCoasting = doc.Tables[1].Cell(16, 3).Range.Text.Remove(doc.Tables[1].Cell(16, 3).Range.Text.Length - 1).Trim();
                    //string[] NationalSixCoasting = doc.Tables[1].Cell(16, 3).Range.Text.Remove(doc.Tables[1].Cell(16, 3).Range.Text.Length - 1).Trim().Split(new char[]{ '；'},StringSplitOptions.RemoveEmptyEntries);
                    //string NationalSixCoastingA = NationalSixCoasting[0];
                    //string NationalSixCoastingB = NationalSixCoasting[1];
                    //string NationalSixCoastingC = NationalSixCoasting[2];



                    bool YN = true;//判断是否为新增
                    for (int j = 0; j < _control._view.RowCount; j++)
                    {

                        if (_control._view.GetRowCellValue(j, "VIN").ToString() == VIN)
                        {
                            YN = false;

                            if (MessageBox.Show($"{VIN}样品信息已存在，是否合并到原有样品信息中?", "提示", MessageBoxButtons.OKCancel,
                                     MessageBoxIcon.Warning) == DialogResult.OK)
                            {

                                _control._view.SetRowCellValue(j, "SampleType", SampleType);
                                _control._view.SetRowCellValue(j, "CarType", CarType);
                                _control._view.SetRowCellValue(j, "SampleProducter", SampleProductor);
                                _control._view.SetRowCellValue(j, "VehicleModel", VehicleModel);
                                _control._view.SetRowCellValue(j, "VIN", VIN);
                                _control._view.SetRowCellValue(j, "VehicleQuality", VehicleQuality);
                                _control._view.SetRowCellValue(j, "VehicleMaxQuality", VehicleMaxQuality);
                                //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "OptionalQuality", OptionalQuality);
                                //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "DesignPeopleCount", DesignPeopleCount);
                                _control._view.SetRowCellValue(j, "DriveFormParameter", DriveFormParameter);
                                _control._view.SetRowCellValue(j, "GearboxForm", GearboxForm);
                                _control._view.SetRowCellValue(j, "FetalPressureParameter", FetalPressureParameter);

                                _control._view.SetRowCellValue(j, "NationalSixCoasting", NationalSixCoasting);

                           


                                _control._view.SetRowCellValue(j, "consistent", "79");

                                if (MessageBox.Show("是否将改动的样品信息同步到试验统计对应的信息？", " 提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {
                                    string sql = $"update TestStatistic  set SampleModel ='{VehicleModel}',Producer='{SampleProductor}', TransmissionType='{GearboxForm}' where Carvin='{VIN}'";
                                    SqlHelper.ExecuteNonquery(sql, System.Data.CommandType.Text);
                                    MessageBox.Show("同步成功");
                                }

                            }
                            else
                            {
                                return;
                            }
                            //if (_control._view.GetRowCellValue(j, "SampleType").ToString() != SampleType || _control._view.GetRowCellValue(j, "CarType").ToString() != CarType || _control._view.GetRowCellValue(j, "SampleProductor").ToString() != SampleProductor || _control._view.GetRowCellValue(j, "VehicleModel").ToString() != VehicleModel || _control._view.GetRowCellValue(j, "VIN").ToString() != VIN || _control._view.GetRowCellValue(j, "VehicleQuality").ToString() != VehicleQuality || _control._view.GetRowCellValue(j, "VehicleMaxQuality").ToString() != VehicleMaxQuality || _control._view.GetRowCellValue(j, "DriveFormParameter").ToString() != DriveFormParameter || _control._view.GetRowCellValue(j, "GearboxForm").ToString() != GearboxForm || _control._view.GetRowCellValue(j, "FetalPressureParameter").ToString() != FetalPressureParameter || _control._view.GetRowCellValue(j, "NationalSixCoastingA").ToString() != NationalSixCoastingA || _control._view.GetRowCellValue(j, "NationalSixCoastingB").ToString() != NationalSixCoastingB || _control._view.GetRowCellValue(j, "NationalSixCoastingC").ToString() != NationalSixCoastingC)
                            //{
                            //    _control._view.SetRowCellValue(j, "Consistent", "否");
                            //}
                            //else if (_control._view.GetRowCellValue(j, "Consistent").ToString() == "否")
                            //{
                            //    _control._view.SetRowCellValue(j, "Consistent", "是");
                            //}
                        }

                    }

                    if (YN)
                    {



                        _control._view.AddNewRow();

                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CreatePeople", FormSignIn.CurrentUser.Name);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "RegPeople", FormSignIn.CurrentUser.Name);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "UpdateDate", DateTime.Now.ToString("yyyy/MM/dd"));
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "SampleType", SampleType);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarType", CarType);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "SampleProducter", SampleProductor);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "VehicleModel", VehicleModel);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "VIN", VIN);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "VehicleQuality", VehicleQuality);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "VehicleMaxQuality", VehicleMaxQuality);
                        //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "OptionalQuality", OptionalQuality);
                        //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "DesignPeopleCount", DesignPeopleCount);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "DriveFormParameter", DriveFormParameter);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "GearboxForm", GearboxForm);
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "FetalPressureParameter", FetalPressureParameter);
                     
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalSixCoasting", NationalSixCoasting);
                        //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalSixCoastingB", NationalSixCoastingB);
                        //_control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalSixCoastingC", NationalSixCoastingC);

                        //默认为--
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "OptionalQuality", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "DesignPeopleCount", "—");


                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "PowerType", "EV");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "FuelLabel", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "FuelType", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "EngineModel", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "EngineProducter", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "OilSupplyType", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "DirectInjection", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "NationalFiveCoasting", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "EmissionTimeLimit", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterForm1", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterForm2", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterNum1", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterNum2", "—");
                        //_control._view.SetRowCellValue(j, "SampleCount", "——");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "ControlSystemType", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterProductor1", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "CarbonCanisterProductor2", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "OBDProductor", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "BOBForm", "—");
                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "Remark", "—");



                        _control._view.SetRowCellValue(_control._view.FocusedRowHandle, "consistent", "79");


                    }
                    string folders = $"{Folder}\\{VIN}";
                    var dir = new DirectoryInfo(folders);
                    if (!dir.Exists)
                        dir.Create();
                    var target = $"{folders}\\{VIN}.doc";

                    if (File.Exists(target))
                    {
                        if (MessageBox.Show($"{target}已存在是否覆盖?", "提示", MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) != DialogResult.OK)
                            continue;
                    }

                    File.Copy(file.ToString(), target, true);

                    _control._view.MoveLast();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                finally
                {
                    if (doc != null)
                    {
                        doc.Close(ref missing, ref missing, ref missing);
                        doc = null;
                    }

                    if (app != null)
                    {
                        app.Quit(ref missing, ref missing, ref missing);
                        app = null;
                    }

                    MessageFilter.Revoke();



                    SplashScreenManager.CloseForm();

                }

              
            }


            

        }

        /// <summary>
        /// 校验83参数表
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="CarVIN"></param>
        /// <param name="hand"></param>
        public void CheckWordData83(string fileName, string CarVIN, int hand)
        {
            SplashScreenManager.ShowForm(typeof(WaitForm1));

            MessageFilter.Register();



            //var files = Directory.GetFiles(fileName);
            var files = fileName;

            //string folders = $"{Folder}\\{CarVIN}";
            //var dir = new DirectoryInfo(folders);
            //if (!dir.Exists)
            //{

            //}
                

            Word.Application app = new Word.Application();//可以打开word
            Word.Document doc = null;      //需要记录打开的word

            object missing = System.Reflection.Missing.Value;
            object file = files;
            object readOnly = true;//不是只读
            object isVisible = false;

            //object unknow = Type.Missing;

            try
            {
                doc = app.Documents.Open(ref file, ref missing, ref readOnly,
                 ref missing, ref missing, ref missing, ref missing, ref missing,
                 ref missing, ref missing, ref missing, ref isVisible, ref missing,
                 ref missing, ref missing, ref missing);
                //string SampleType = "传统车/混合动力";

                string CarType = doc.Tables[1].Cell(3, 3).Range.Text.Remove(doc.Tables[1].Cell(3, 3).Range.Text.Length - 1).Trim();
                string SampleProductor = doc.Tables[1].Cell(5, 3).Range.Text.Remove(doc.Tables[1].Cell(5, 3).Range.Text.Length - 1).Trim();
                string VehicleModel = doc.Tables[1].Cell(4, 5).Range.Text.Remove(doc.Tables[1].Cell(4, 5).Range.Text.Length - 1).Trim();
                string VIN = doc.Tables[1].Cell(6, 4).Range.Text.Remove(doc.Tables[1].Cell(6, 4).Range.Text.Length - 1).Trim();

                string VehicleQuality = doc.Tables[1].Cell(9, 3).Range.Text.Remove(doc.Tables[1].Cell(9, 3).Range.Text.Length - 1).Trim();
                string VehicleMaxQuality = doc.Tables[1].Cell(10, 3).Range.Text.Remove(doc.Tables[1].Cell(10, 3).Range.Text.Length - 1).Trim();
                string OptionalQuality = doc.Tables[1].Cell(13, 3).Range.Text.Remove(doc.Tables[1].Cell(13, 3).Range.Text.Length - 1).Trim();

                string DesignPeopleCount = doc.Tables[1].Cell(16, 3).Range.Text.Remove(doc.Tables[1].Cell(16, 3).Range.Text.Length - 1).Trim();
                string DriveFormParameter = doc.Tables[1].Cell(19, 3).Range.Text.Remove(doc.Tables[1].Cell(19, 3).Range.Text.Length - 1).Trim();

                string GearboxForm = doc.Tables[1].Cell(20, 3).Range.Text.Remove(doc.Tables[1].Cell(20, 3).Range.Text.Length - 1).Trim();
                string FetalPressureParameter = doc.Tables[1].Cell(21, 3).Range.Text.Remove(doc.Tables[1].Cell(21, 3).Range.Text.Length - 1).Trim();

                string FuelType = doc.Tables[1].Cell(25, 3).Range.Text.Remove(doc.Tables[1].Cell(25, 3).Range.Text.Length - 1).Trim();

                string FuelLabel = doc.Tables[1].Cell(26, 3).Range.Text.Remove(doc.Tables[1].Cell(26, 3).Range.Text.Length - 1).Trim();
                //string PowerType = doc.Tables[1].Cell(12, 6).Range.Text.Remove(doc.Tables[1].Cell(12, 6).Range.Text.Length - 1).Trim();
                string EngineModel = doc.Tables[1].Cell(6, 7).Range.Text.Remove(doc.Tables[1].Cell(6, 7).Range.Text.Length - 1).Trim();
                string EngineProducter = doc.Tables[1].Cell(8, 7).Range.Text.Remove(doc.Tables[1].Cell(8, 7).Range.Text.Length - 1).Trim();
                string OilSupplyType = doc.Tables[1].Cell(37, 6).Range.Text.Remove(doc.Tables[1].Cell(37, 6).Range.Text.Length - 1).Trim();
                //string DirectInjection = doc.Tables[1].Cell(13, 6).Range.Text.Remove(doc.Tables[1].Cell(13, 6).Range.Text.Length - 1).Trim();
                string NationalSixCoastingA = doc.Tables[2].Cell(1, 2).Range.Text.Remove(doc.Tables[2].Cell(1, 2).Range.Text.Length - 1).Trim();
                string NationalSixCoastingB = doc.Tables[2].Cell(1, 3).Range.Text.Remove(doc.Tables[2].Cell(1, 3).Range.Text.Length - 1).Trim();
                string NationalSixCoastingC = doc.Tables[2].Cell(1, 4).Range.Text.Remove(doc.Tables[2].Cell(1, 4).Range.Text.Length - 1).Trim();

                string NationalSixCoasting = NationalSixCoastingA + ";" + NationalSixCoastingB + ";" + NationalSixCoastingC + ";";

                string NationalFiveCoastingA = doc.Tables[2].Cell(2, 2).Range.Text.Remove(doc.Tables[2].Cell(2, 2).Range.Text.Length - 1).Trim();
                string NationalFiveCoastingB = doc.Tables[2].Cell(2, 3).Range.Text.Remove(doc.Tables[2].Cell(2, 3).Range.Text.Length - 1).Trim();
                string NationalFiveCoastingC = doc.Tables[2].Cell(2, 4).Range.Text.Remove(doc.Tables[2].Cell(2, 4).Range.Text.Length - 1).Trim();

                string NationalFiveCoasting = NationalFiveCoastingA + ";" + NationalFiveCoastingB + ";" + NationalFiveCoastingC + ";";
                //string EmissionTimeLimit = doc.Tables[1].Cell(13, 6).Range.Text.Remove(doc.Tables[1].Cell(13, 6).Range.Text.Length - 1).Trim();
                string CarbonCanisterForm1 = doc.Tables[1].Cell(90, 4).Range.Text.Remove(doc.Tables[1].Cell(90, 4).Range.Text.Length - 1).Trim();
                string CarbonCanisterForm2 = doc.Tables[1].Cell(97, 4).Range.Text.Remove(doc.Tables[1].Cell(97, 4).Range.Text.Length - 1).Trim();
                string CarbonCanisterNum1 = doc.Tables[7].Cell(2, 2).Range.Text.Remove(doc.Tables[7].Cell(2, 2).Range.Text.Length - 1).Trim();
                string CarbonCanisterNum2 = doc.Tables[7].Cell(2, 2).Range.Text.Remove(doc.Tables[7].Cell(2, 2).Range.Text.Length - 1).Trim();
                //string SampleCount = doc.Tables[1].Cell(13, 6).Range.Text.Remove(doc.Tables[1].Cell(13, 6).Range.Text.Length - 1).Trim();
                string ControlSystemType = doc.Tables[1].Cell(43, 3).Range.Text.Remove(doc.Tables[1].Cell(43, 3).Range.Text.Length - 1).Trim();

                string CarbonCanisterProductor1 = doc.Tables[1].Cell(95, 4).Range.Text.Remove(doc.Tables[1].Cell(95, 4).Range.Text.Length - 1).Trim();
                string CarbonCanisterProductor2 = doc.Tables[1].Cell(102, 4).Range.Text.Remove(doc.Tables[1].Cell(102, 4).Range.Text.Length - 1).Trim();
                string OBDProductor = doc.Tables[1].Cell(95, 7).Range.Text.Remove(doc.Tables[1].Cell(95, 7).Range.Text.Length - 1).Trim();



                if (CarVIN != "")
                {

                    if (_control._view.GetRowCellValue(hand, "CarType").ToString() != CarType || _control._view.GetRowCellValue(hand, "SampleProducter").ToString() != SampleProductor || _control._view.GetRowCellValue(hand, "VehicleModel").ToString() != VehicleModel || _control._view.GetRowCellValue(hand, "VIN").ToString() != VIN || _control._view.GetRowCellValue(hand, "VehicleQuality").ToString() != VehicleQuality || _control._view.GetRowCellValue(hand, "VehicleMaxQuality").ToString() != VehicleMaxQuality || _control._view.GetRowCellValue(hand, "OptionalQuality").ToString() != OptionalQuality || _control._view.GetRowCellValue(hand, "DesignPeopleCount").ToString() != DesignPeopleCount || _control._view.GetRowCellValue(hand, "DriveFormParameter").ToString() != DriveFormParameter || _control._view.GetRowCellValue(hand, "GearboxForm").ToString() != GearboxForm || _control._view.GetRowCellValue(hand, "FetalPressureParameter").ToString() != FetalPressureParameter || _control._view.GetRowCellValue(hand, "FuelType").ToString() != FuelType || _control._view.GetRowCellValue(hand, "FuelLabel").ToString() != FuelLabel || _control._view.GetRowCellValue(hand, "EngineModel").ToString() != EngineModel || _control._view.GetRowCellValue(hand, "EngineProducter").ToString() != EngineProducter || _control._view.GetRowCellValue(hand, "OilSupplyType").ToString() != OilSupplyType || _control._view.GetRowCellValue(hand, "NationalSixCoasting").ToString() != NationalSixCoasting || _control._view.GetRowCellValue(hand, "NationalFiveCoasting").ToString() != NationalFiveCoasting || _control._view.GetRowCellValue(hand, "CarbonCanisterForm1").ToString() != CarbonCanisterForm1 || _control._view.GetRowCellValue(hand, "CarbonCanisterForm2").ToString() != CarbonCanisterForm2 || _control._view.GetRowCellValue(hand, "CarbonCanisterNum1").ToString() != CarbonCanisterNum1 || _control._view.GetRowCellValue(hand, "CarbonCanisterNum2").ToString() != CarbonCanisterNum2 || _control._view.GetRowCellValue(hand, "ControlSystemType").ToString() != ControlSystemType || _control._view.GetRowCellValue(hand, "CarbonCanisterProductor1").ToString() != CarbonCanisterProductor1 || _control._view.GetRowCellValue(hand, "CarbonCanisterProductor2").ToString() != CarbonCanisterProductor2 || _control._view.GetRowCellValue(hand, "OBDProductor").ToString() != OBDProductor)
                    {

                        string name = "83否,";
                        //设置错误字段

                        if (_control._view.GetRowCellValue(hand, "CarType").ToString() != CarType)
                        {
                            name += "CarType" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "SampleProducter").ToString() != SampleProductor)
                        {
                            name += "SampleProducter" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "VehicleModel").ToString() != VehicleModel)
                        {
                            name += "VehicleModel" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "VIN").ToString() != VIN)
                        {
                            name += "VIN" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "VehicleMaxQuality").ToString() != VehicleMaxQuality)
                        {
                            name += "VehicleMaxQuality" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "VehicleQuality").ToString() != VehicleQuality)
                        {
                            name += "VehicleQuality" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "OptionalQuality").ToString() != OptionalQuality)
                        {
                            name += "OptionalQuality" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "DesignPeopleCount").ToString() != DesignPeopleCount)
                        {
                            name += "DesignPeopleCount" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "DriveFormParameter").ToString() != DriveFormParameter)
                        {
                            name += "DriveFormParameter" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "GearboxForm").ToString() != GearboxForm)
                        {
                            name += "GearboxForm" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "FetalPressureParameter").ToString() != FetalPressureParameter)
                        {
                            name += "FetalPressureParameter" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "FuelType").ToString() != FuelType)
                        {
                            name += "FuelType" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "FuelLabel").ToString() != FuelLabel)
                        {
                            name += "FuelLabel" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "EngineModel").ToString() != EngineModel)
                        {
                            name += "EngineModel" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "EngineProducter").ToString() != EngineProducter)
                        {
                            name += "EngineProducter" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "OilSupplyType").ToString() != OilSupplyType)
                        {
                            name += "OilSupplyType" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "NationalSixCoasting").ToString() != NationalSixCoasting)
                        {
                            name += "NationalSixCoasting" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "NationalFiveCoasting").ToString() != NationalFiveCoasting)
                        {
                            name += "NationalFiveCoasting" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "CarbonCanisterForm1").ToString() != CarbonCanisterForm1)
                        {
                            name += "CarbonCanisterForm1" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "CarbonCanisterForm2").ToString() != CarbonCanisterForm2)
                        {
                            name += "CarbonCanisterForm2" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "CarbonCanisterNum1").ToString() != CarbonCanisterNum1)
                        {
                            name += "CarbonCanisterNum1" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "CarbonCanisterNum2").ToString() != CarbonCanisterNum2)
                        {
                            name += "CarbonCanisterNum2" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "ControlSystemType").ToString() != ControlSystemType)
                        {
                            name += "ControlSystemType" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "CarbonCanisterProductor1").ToString() != CarbonCanisterProductor1)
                        {
                            name += "CarbonCanisterProductor1" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "CarbonCanisterProductor2").ToString() != CarbonCanisterProductor2)
                        {
                            name += "CarbonCanisterProductor2" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "OBDProductor").ToString() != OBDProductor)
                        {
                            name += "OBDProductor" + ",";
                        }
                        _control._view.SetRowCellValue(hand, "consistent", name);

                    }
                    else
                    {
                        _control._view.SetRowCellValue(hand, "consistent", "83是");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (doc != null)
                {
                    doc.Close(ref missing, ref missing, ref missing);
                    doc = null;
                }

                if (app != null)
                {
                    app.Quit(ref missing, ref missing, ref missing);
                    app = null;
                }

                MessageFilter.Revoke();



                SplashScreenManager.CloseForm();

            }
            
            MessageBox.Show("校验成功，请点击保存");
        }



        /// <summary>
        /// 校验82参数表
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="CarVIN"></param>
        /// <param name="hand"></param>
        public void CheckWordData82(string fileName, string CarVIN, int hand)
        {
            SplashScreenManager.ShowForm(typeof(WaitForm1));

            MessageFilter.Register();



            //var files = Directory.GetFiles(fileName);
            var files = fileName;

       

            Word.Application app = new Word.Application();//可以打开word
            Word.Document doc = null;      //需要记录打开的word

            object missing = System.Reflection.Missing.Value;
            object file = files;
            object readOnly = true;//不是只读
            object isVisible = false;

            //object unknow = Type.Missing;

            try
            {
                doc = app.Documents.Open(ref file, ref missing, ref readOnly,
                 ref missing, ref missing, ref missing, ref missing, ref missing,
                 ref missing, ref missing, ref missing, ref isVisible, ref missing,
                 ref missing, ref missing, ref missing);
                //string SampleType = "传统车/混合动力";

                string SampleProductor = doc.Tables[1].Cell(6, 3).Range.Text.Remove(doc.Tables[1].Cell(6, 3).Range.Text.Length - 1).Trim();
                string VehicleModel = doc.Tables[1].Cell(15, 3).Range.Text.Remove(doc.Tables[1].Cell(16, 3).Range.Text.Length - 1).Trim();
                string VIN = VehicleModel;


                string CarbonCanisterForm1 = doc.Tables[1].Cell(4, 3).Range.Text.Remove(doc.Tables[1].Cell(4, 3).Range.Text.Length - 1).Trim();
                //string CarbonCanisterForm2 = doc.Tables[1].Cell(97, 4).Range.Text.Remove(doc.Tables[1].Cell(97, 4).Range.Text.Length - 1).Trim();
                string CarbonCanisterNum1 = doc.Tables[1].Cell(8, 3).Range.Text.Remove(doc.Tables[1].Cell(8, 3).Range.Text.Length - 1).Trim();
                //string CarbonCanisterNum2 = doc.Tables[7].Cell(2, 2).Range.Text.Remove(doc.Tables[7].Cell(2, 2).Range.Text.Length - 1).Trim();
                string SampleCount = doc.Tables[1].Cell(9, 3).Range.Text.Remove(doc.Tables[1].Cell(9, 3).Range.Text.Length - 1).Trim();
                //string ControlSystemType = doc.Tables[1].Cell(43, 3).Range.Text.Remove(doc.Tables[1].Cell(43, 3).Range.Text.Length - 1).Trim();

                string CarbonCanisterProductor1 = doc.Tables[1].Cell(6, 3).Range.Text.Remove(doc.Tables[1].Cell(6, 3).Range.Text.Length - 1).Trim();




                if (CarVIN != "")
                {

                    if (_control._view.GetRowCellValue(hand, "SampleProducter").ToString() != SampleProductor || _control._view.GetRowCellValue(hand, "VehicleModel").ToString() != VehicleModel || _control._view.GetRowCellValue(hand, "VIN").ToString() != VIN || _control._view.GetRowCellValue(hand, "CarbonCanisterNum1").ToString() != CarbonCanisterNum1 || _control._view.GetRowCellValue(hand, "CarbonCanisterProductor1").ToString() != CarbonCanisterProductor1 || _control._view.GetRowCellValue(hand, "CarbonCanisterForm1").ToString() != CarbonCanisterForm1 || _control._view.GetRowCellValue(hand, "SampleCount").ToString() != SampleCount)
                    {

                        string name = "82否,";
                        //设置错误字段

                      
                        if (_control._view.GetRowCellValue(hand, "SampleProducter").ToString() != SampleProductor)
                        {
                            name += "SampleProducter" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "VehicleModel").ToString() != VehicleModel)
                        {
                            name += "VehicleModel" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "VIN").ToString() != VIN)
                        {
                            name += "VIN" + ",";
                        }
                    
                        if (_control._view.GetRowCellValue(hand, "CarbonCanisterForm1").ToString() != CarbonCanisterForm1)
                        {
                            name += "CarbonCanisterForm1" + ",";
                        }
                      
                        if (_control._view.GetRowCellValue(hand, "CarbonCanisterNum1").ToString() != CarbonCanisterNum1)
                        {
                            name += "CarbonCanisterNum1" + ",";
                        }
                      
                        if (_control._view.GetRowCellValue(hand, "CarbonCanisterProductor1").ToString() != CarbonCanisterProductor1)
                        {
                            name += "CarbonCanisterProductor1" + ",";
                        }

                        if (_control._view.GetRowCellValue(hand, "SampleCount").ToString() != SampleCount)
                        {
                            name += "SampleCount" + ",";
                        }

                        _control._view.SetRowCellValue(hand, "consistent", name);

                    }
                    else
                    {
                        _control._view.SetRowCellValue(hand, "consistent", "82是");
                    }
                }

            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (doc != null)
                {
                    doc.Close(ref missing, ref missing, ref missing);
                    doc = null;
                }

                if (app != null)
                {
                    app.Quit(ref missing, ref missing, ref missing);
                    app = null;
                }

                MessageFilter.Revoke();



                SplashScreenManager.CloseForm();

            }
            
            MessageBox.Show("校验成功，请点击保存");
        }


        /// <summary>
        /// 校验79参数表
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="CarVIN"></param>
        /// <param name="hand"></param>
        public void CheckWordData79(string fileName, string CarVIN, int hand)
        {
            //Form1.ShowWaitForm();
            SplashScreenManager.ShowForm(typeof(WaitForm1));

            MessageFilter.Register();



            //var files = Directory.GetFiles(fileName);
            var files = fileName;


            Word.Application app = new Word.Application();//可以打开word
            Word.Document doc = null;      //需要记录打开的word

            object missing = System.Reflection.Missing.Value;
            object file = files;
            object readOnly = true;//不是只读
            object isVisible = false;

            //object unknow = Type.Missing;

            try
            {
                doc = app.Documents.Open(ref file, ref missing, ref readOnly,
                 ref missing, ref missing, ref missing, ref missing, ref missing,
                 ref missing, ref missing, ref missing, ref isVisible, ref missing,
                 ref missing, ref missing, ref missing);
                //string SampleType = "传统车/混合动力";

                string CarType = doc.Tables[1].Cell(6, 3).Range.Text.Remove(doc.Tables[1].Cell(6, 3).Range.Text.Length - 1).Trim();

                string SampleProductor = doc.Tables[1].Cell(9, 3).Range.Text.Remove(doc.Tables[1].Cell(9, 3).Range.Text.Length - 1).Trim();
                string VehicleModel = doc.Tables[1].Cell(7, 3).Range.Text.Remove(doc.Tables[1].Cell(7, 3).Range.Text.Length - 1).Trim();
                string VIN = doc.Tables[1].Cell(4, 3).Range.Text.Remove(doc.Tables[1].Cell(4, 3).Range.Text.Length - 1).Trim();

                string VehicleQuality = doc.Tables[1].Cell(11, 3).Range.Text.Remove(doc.Tables[1].Cell(11, 3).Range.Text.Length - 1).Trim();
                string VehicleMaxQuality = doc.Tables[1].Cell(12, 3).Range.Text.Remove(doc.Tables[1].Cell(12, 3).Range.Text.Length - 1).Trim();

                string DriveFormParameter = doc.Tables[1].Cell(28, 3).Range.Text.Remove(doc.Tables[1].Cell(28, 3).Range.Text.Length - 1).Trim();
                //  string DriveFormTask = doc.Tables[1].Cell(13, 6).Range.Text.Remove(doc.Tables[1].Cell(13, 6).Range.Text.Length - 1).Trim();

                string GearboxForm = doc.Tables[1].Cell(26, 3).Range.Text.Remove(doc.Tables[1].Cell(26, 3).Range.Text.Length - 1).Trim();
                string FetalPressureParameter = doc.Tables[1].Cell(30, 3).Range.Text.Remove(doc.Tables[1].Cell(30, 3).Range.Text.Length - 1).Trim();

                string NationalSixCoasting = doc.Tables[1].Cell(16, 3).Range.Text.Remove(doc.Tables[1].Cell(16, 3).Range.Text.Length - 1).Trim();



                if (CarVIN != "")
                {

                    if (_control._view.GetRowCellValue(hand, "CarType").ToString() != CarType || _control._view.GetRowCellValue(hand, "SampleProducter").ToString() != SampleProductor || _control._view.GetRowCellValue(hand, "VehicleModel").ToString() != VehicleModel || _control._view.GetRowCellValue(hand, "VIN").ToString() != VIN || _control._view.GetRowCellValue(hand, "VehicleQuality").ToString() != VehicleQuality || _control._view.GetRowCellValue(hand, "VehicleMaxQuality").ToString() != VehicleMaxQuality || _control._view.GetRowCellValue(hand, "DriveFormParameter").ToString() != DriveFormParameter || _control._view.GetRowCellValue(hand, "GearboxForm").ToString() != GearboxForm || _control._view.GetRowCellValue(hand, "FetalPressureParameter").ToString() != FetalPressureParameter || _control._view.GetRowCellValue(hand, "NationalSixCoasting").ToString() != NationalSixCoasting)
                    {

                        string name = "79否,";
                        //设置错误字段

                        if (_control._view.GetRowCellValue(hand, "CarType").ToString() != CarType)
                        {
                            name += "CarType" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "SampleProducter").ToString() != SampleProductor)
                        {
                            name += "SampleProducter" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "VehicleModel").ToString() != VehicleModel)
                        {
                            name += "VehicleModel" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "VIN").ToString() != VIN)
                        {
                            name += "VIN" + ",";
                        }


                        if (_control._view.GetRowCellValue(hand, "VehicleQuality").ToString() != VehicleQuality)
                        {
                            name += "VehicleQuality" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "VehicleMaxQuality").ToString() != VehicleMaxQuality)
                        {
                            name += "VehicleMaxQuality" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "DriveFormParameter").ToString() != DriveFormParameter)
                        {
                            name += "DriveFormParameter" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "GearboxForm").ToString() != GearboxForm)
                        {
                            name += "GearboxForm" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "FetalPressureParameter").ToString() != FetalPressureParameter)
                        {
                            name += "FetalPressureParameter" + ",";
                        }
                        if (_control._view.GetRowCellValue(hand, "NationalSixCoasting").ToString() != NationalSixCoasting)
                        {
                            name += "NationalSixCoasting" + ",";
                        }

                        _control._view.SetRowCellValue(hand, "consistent", name);

                    }
                    else
                    {
                        _control._view.SetRowCellValue(hand, "consistent", "79是");
                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (doc != null)
                {
                    doc.Close(ref missing, ref missing, ref missing);
                    doc = null;
                }

                if (app != null)
                {
                    app.Quit(ref missing, ref missing, ref missing);
                    app = null;
                }

                MessageFilter.Revoke();



                SplashScreenManager.CloseForm();

            }
            
            MessageBox.Show("校验成功，请点击保存");
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            //GetLimsData();
        }

        private string vinname;
        /// <summary>
        /// lims接口83
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
          
            var fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "请选择源文件",
                //Filter = "WORD文件(*.doc)|*.doc|WORD文件(*.docx)|*.docx"
                Filter = "文档|*.doc;*.docx"
            };
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            //string filename = AppDomain.CurrentDomain.BaseDirectory+"样品参数表\\1983";
            GetWordData83(fileDialog.FileNames);
            
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {



           // string filename = AppDomain.CurrentDomain.BaseDirectory + "样品参数表\\1982";
            var fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "请选择源文件",
                Filter = "文档|*.doc;*.docx"
            };
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            GetWordData82(fileDialog.FileNames);
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            //string filename = AppDomain.CurrentDomain.BaseDirectory + "样品参数表\\1979";
            var fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "请选择源文件",
                Filter = "文档|*.doc;*.docx"
            };
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            GetWordData79(fileDialog.FileNames);
        }

        private AlertTemplate alertTemplate;
        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (alertTemplate == null || alertTemplate.IsDisposed)
            {
                alertTemplate = new AlertTemplate("样品信息");
                alertTemplate.Show();
            }
            else
            {
                alertTemplate.Show();
                alertTemplate.Activate();
            }
        }
        private SelectTemplate selectTemplate;
        /// <summary>
        /// 选择模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (selectTemplate == null || selectTemplate.IsDisposed)
            {
                selectTemplate = new SelectTemplate("样品信息");
                selectTemplate.freshForm += SelectTemplate_freshForm1;
                selectTemplate.Show();
            }
            else
            {
                selectTemplate.Show();
                selectTemplate.Activate();
            }
        }

        private void SelectTemplate_freshForm1()
        {
            throw new NotImplementedException();
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
        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
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

        /// <summary>
        /// 分配试验任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void barButtonItem13_ItemClick_1(object sender, ItemClickEventArgs e)
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
            //_control.SetSaveStatus(false);
        }

        private void SampleForm_Activated(object sender, EventArgs e)
        {
           
            Filter.Moudle = "样品信息";
        }

        private void SampleForm_Enter(object sender, EventArgs e)
        {
            Filter.state = "1";
        }
        private void SampleForm_Leave(object sender, EventArgs e)
        {
            Filter.state = "0";
        }


        /// <summary>
        /// 样品信息校验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_control._view.SelectedRowsCount == 1)
            {
                int row = Convert.ToInt32(_control._view.GetSelectedRows()[0]);
                string vin = _control._view.GetRowCellValue(row, "VIN").ToString();
                string Consistent = _control._view.GetRowCellValue(row, "consistent").ToString();
                // GetLimsData(taskcode, row);
                var name = vin + ".doc";

                var sourcePath = $"{Folder}\\{vin}\\{name}";
                if (Consistent.Contains("83") && File.Exists(sourcePath))
                {
                    
                    CheckWordData83(sourcePath, vin, row);
                }else if (Consistent.Contains("82") && File.Exists(sourcePath))
                {
                    CheckWordData82(sourcePath, vin, row);
                }
                else if (Consistent.Contains("79") && File.Exists(sourcePath))
                {
                    CheckWordData79(sourcePath, vin, row);
                }
                else
                {
                    MessageBox.Show("参数信息表不存在");
                }
                

            }
            else
            {
                MessageBox.Show("请选中某条数据进行校验！");
            }
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_control._view.SelectedRowsCount == 1)
            {
                var fileDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Title = "请选择源文件",
                    //Filter = "WORD文件(*.doc)|*.doc|WORD文件(*.docx)|*.docx"
                    Filter = "文档|*.doc;*.docx"
                };
                if (fileDialog.ShowDialog() != DialogResult.OK)
                    return;

                int row = Convert.ToInt32(_control._view.GetSelectedRows()[0]);
                string VIN = _control._view.GetRowCellValue(row, "VIN").ToString();

                for (var i = 0; i < fileDialog.FileNames.Length; i++)
                {
                    
                    var target = $"{Folder}\\{VIN}\\{System.IO.Path.GetFileName(fileDialog.FileNames[i])}";

                    string folders = $"{Folder}\\{VIN}";
                    var dir = new DirectoryInfo(folders);
                    if (!dir.Exists)
                        dir.Create();

                    //if (File.Exists(target))
                    //{
                    //    if (MessageBox.Show($"{target}已存在是否覆盖?", "提示", MessageBoxButtons.OKCancel,
                    //            MessageBoxIcon.Warning) != DialogResult.OK)
                    //        return;
                    //}

                    File.Copy(fileDialog.FileNames[i].ToString(), target, true);
                }
                  
                MessageBox.Show("上传附件成功");
            }
            else
            {
                MessageBox.Show("请选中某条数据！");
            }
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }
    }


}
