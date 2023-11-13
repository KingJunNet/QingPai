using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Utils;

namespace TaskManager
{
    public partial class EquipmentForm : BaseForm
    {
        private DataControl sql = new DataControl();
        public readonly string Server;
        private const string RootFolder = "轻排参数表服务器";
        private const string Category = "设备管理";
        protected EquipmentForm()
        {
            InitializeComponent();
        }
        public EquipmentForm(FormType formType, string selectedDept) : base(formType, selectedDept)
        {
            InitializeComponent();
            if (DesignMode)
                return;

            Server = sql.ServerIP;
            if (!Server.EndsWith("\\"))
                Server += "\\";



            _control.BeforeAddRowOnImportExcel += BeforeAddRowOnImportExcel;
            _control._view.RowStyle += ViewOnRowStyle;
        }

        protected override void InitUi()
        {
            textYear.Visibility = BarItemVisibility.Never;
            comboxState.Visibility = BarItemVisibility.Never;

            startdate.Visibility = BarItemVisibility.Never;
            enddate.Visibility = BarItemVisibility.Never;
        }

        /// <summary>
        /// 导入数据前先处理一下日期问题
        /// </summary>
        /// <param name="targetRow"></param>
        /// <param name="sourceRow"></param>
        private static void BeforeAddRowOnImportExcel(DataRow targetRow, DataRow sourceRow)
        {
            var period = targetRow.CorrectValue("Period", 1.0);
            var months = (int) (period * 12);
            var checkDate = targetRow.CorrectValue("CheckDate", DateTime.Now.Date);

            var checkEndDate = checkDate.AddMonths(months).AddDays(-1);
            targetRow["CheckEndDate"] = checkEndDate;
        }
        
        /// <summary>
        /// 行标色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewOnRowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                string f1 = "", f2 = "", f3 = "";
                var days = ReminderDialog.EquipWarning;
                var date1 = DateTime.Now.Date.AddDays(days);

                //设备名称
                var equipname = _control._view.GetRowCellValue(e.RowHandle, "EquipName")?.ToString().Trim();
                //检定方式
                var checktype = _control._view.GetRowCellValue(e.RowHandle, "CheckType")?.ToString().Trim();

                var EquipCode = _control._view.GetRowCellValue(e.RowHandle, "EquipCode")?.ToString().Trim();

                //检定日期
                var checkdate = Convert.ToDateTime(_control._view.GetRowCellValue(e.RowHandle, "CheckDate")?.ToString().Trim()).Date.ToString("yyyy-MM-dd");


                var sState = _control._view.GetRowCellValue(e.RowHandle, "EquipState")?.ToString().Trim();
                var sEndDate = _control._view.GetRowCellValue(e.RowHandle, "CheckEndDate")?.ToString().Trim();

                //DataSet d1 = sql.ExecuteQuery("select f1,f2,f3 from EquipmentFileTable where EquipName='" + equipname + EquipCode + "' and Checkdate='" + checkdate + "'");
                DataSet d1 = sql.ExecuteQuery("select f1,f2,f3 from EquipmentFileTable where EquipName='" + equipname + EquipCode + "'");
                if (d1.Tables[0].Rows.Count != 0)
                {
                    f1 = d1.Tables[0].Rows[0]["f1"].ToString().Trim();
                    f2 = d1.Tables[0].Rows[0]["f2"].ToString().Trim();
                    f3 = d1.Tables[0].Rows[0]["f3"].ToString().Trim();
                }
                DateTime.TryParse(sEndDate, out var endDate);
                if (date1 >= endDate)
                {
                    e.Appearance.BackColor = Color.Orange;
                    if (sState != null && sState.Contains("中"))
                    {
                        //设备状态是检定设置为黄色
                        e.Appearance.BackColor = Color.Yellow;
                        //

                        if ((checktype == "外部检定") && (f1 != "") && (f2 == ""))
                            e.Appearance.BackColor = Color.LightGreen;
                        if ((checktype == "外部检定") && (f1 != "") && (f2 != ""))
                            e.Appearance.BackColor = Color.White;
                        /////////////
                        if ((checktype == "外部校准") && (f1 != "") && (f2 == ""))
                            e.Appearance.BackColor = Color.LightGreen;

                        if ((checktype == "外部校准") && (f1 != "") && (f2 != ""))
                            e.Appearance.BackColor = Color.White;
                        /////
                        if ((checktype == "内部核查") && (f3 == ""))
                            e.Appearance.BackColor = Color.Yellow;
                        if ((checktype == "内部核查") && (f3 != ""))
                            e.Appearance.BackColor = Color.White;
                    }
                }
                else
                {

                    if (sState != null && sState.Contains("中"))
                    {
                        //设备状态是检定设置为黄色
                        e.Appearance.BackColor = Color.Yellow;
                        //

                        if ((checktype == "外部检定") && (f1 != "") && (f2 == ""))
                            e.Appearance.BackColor = Color.LightGreen;
                        if ((checktype == "外部检定") && (f1 != "") && (f2 != ""))
                            e.Appearance.BackColor = Color.White;
                        /////////////
                        if ((checktype == "外部校准") && (f1 != "") && (f2 == ""))
                            e.Appearance.BackColor = Color.LightGreen;

                        if ((checktype == "外部校准") && (f1 != "") && (f2 != ""))
                            e.Appearance.BackColor = Color.White;
                        /////
                        if ((checktype == "内部核查") && (f3 == ""))
                            e.Appearance.BackColor = Color.Yellow;
                        if ((checktype == "内部核查") && (f3 != ""))
                            e.Appearance.BackColor = Color.White;
                    }
                    else if (sState != null && sState.Contains("停"))
                    {
                        //设备状态为停用设置为灰色
                        e.Appearance.BackColor = Color.DarkGray;
                    }
                    else if (sState != null && sState.Contains("使用"))//使用中
                    {
                        //设备状态是使用为白色
                        e.Appearance.BackColor = Color.White;

                    }
                    else
                    {
                        e.Appearance.BackColor = Color.Orange;
                    }
                }
            }
        

         


            if (e.RowHandle >= 0)
            {
                if(_control._view.GetRowCellValue(e.RowHandle, _control._view.Columns["EquipState"])?.ToString() == "停用")
                {
                    e.Appearance.BackColor = Color.DarkGray;
                }
            }

            if (e.RowHandle >= 0)
            {
                if (e.Appearance.BackColor == Color.Orange)
                {
                    if (_control._view.GetRowCellValue(e.RowHandle, _control._view.Columns["Checksite"])?.ToString() == "送检" && _control._view.GetRowCellValue(e.RowHandle, _control._view.Columns["EquipState"])?.ToString() != "检定")
                    {
                        if (_control._view.GetRowCellValue(e.RowHandle, _control._view.Columns["alert"])?.ToString() != "是")
                        {
                            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["alert"], "是");
                        }

                    }
                    if(_control._view.GetRowCellValue(e.RowHandle, _control._view.Columns["EquipState"])?.ToString() == "检定")
                    {
                        if (_control._view.GetRowCellValue(e.RowHandle, _control._view.Columns["alert"])?.ToString() != "")
                        {
                            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["alert"], "");
                        }
                        
                        
                        
                    }
                }
                else
                {
                    if (_control._view.GetRowCellValue(e.RowHandle, _control._view.Columns["alert"])?.ToString() == "是")
                    {
                        _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["alert"], "");
                    }
                    

                }
            }


        }

        protected override DialogResult OpenEditForm(GridView view, int hand, List<DataField> fields)
        {
            Log.e("OpenEditForm");
            var dialog = new EquipEditDialog(FormTable.Edit, _control._view,hand, fields,FormType.Equipment);
            return dialog.ShowDialog();
        }
        protected override DialogResult OpenReplaceForm(GridView view, int hand, List<DataField> fields)
        {
            var dialog = new Dialogs.ReplaceSelectRows(FormTable.Edit, view, hand, fields, FormTable.Type);
            return dialog.ShowDialog();
        }

        private void EquipmentForm_Load(object sender, EventArgs e)
        {

        }
    }
}
