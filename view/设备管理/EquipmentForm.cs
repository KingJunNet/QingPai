﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Utils;
using TaskManager.common.utils;
using TaskManager.controller;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
using TaskManager.domain.valueobject;
using TaskManager.infrastructure.db;

namespace TaskManager
{
    public partial class EquipmentForm : BaseForm
    {
        private DataControl sql = new DataControl();
        public readonly string Server;
        private const string RootFolder = "轻排参数表服务器";
        private const string Category = "设备管理";
        private Dictionary<string, DataRow> nowEquipmentCodeTableRowMap;
        private List<string> newImportEquipmentCodes;

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
            _control.importExcelPreHandler += preHandlerOnImportExcel;
            _control._view.RowStyle += ViewOnRowStyle;
            this._control.afterSavedHandle = new TableControl.AfterSavedEvent(handleAfterSaved);
        }

        protected override void InitUi()
        {
            textYear.Visibility = BarItemVisibility.Never;
            comboxState.Visibility = BarItemVisibility.Never;

            startdate.Visibility = BarItemVisibility.Never;
            enddate.Visibility = BarItemVisibility.Never;

            //操作区功能可见性
            btnNew.Visibility = BarItemVisibility.Never;
            btnEdit.Visibility = BarItemVisibility.Never;
            btnCopyPaste.Visibility = BarItemVisibility.Never;
            btnEditCfgItems.Visibility = BarItemVisibility.Never;
            btnBatchReplace.Visibility = BarItemVisibility.Never;

            //隐藏右键菜单功能
            this.hideAddEtitCopyItem();
        }

        private void handleAfterSaved()
        {
            Thread exportWordThread = new Thread(updateEquipmentCacheData);
            exportWordThread.IsBackground = true;
            exportWordThread.Start();
        }

        private void updateEquipmentCacheData()
        {
            CacheDataHandler.Instance.reloadCurUserEquipments();
        }

        /// <summary>
        /// 导入数据前先处理一下日期问题
        /// </summary>
        /// <param name="targetRow"></param>
        /// <param name="sourceRow"></param>
        private static void BeforeAddRowOnImportExcelBack(DataRow targetRow, DataRow sourceRow)
        {
            var period = targetRow.CorrectValue("Period", 1.0);
            var months = (int)(period * 12);
            var checkDate = targetRow.CorrectValue("CheckDate", DateTime.Now.Date);

            var checkEndDate = checkDate.AddMonths(months).AddDays(-1);
            targetRow["CheckEndDate"] = checkEndDate;
        }

        /// <summary>
        /// 导入数据前先处理一下日期问题
        /// </summary>
        /// <param name="targetRow"></param>
        /// <param name="sourceRow"></param>
        private bool BeforeAddRowOnImportExcel(DataRow targetRow, DataRow sourceRow)
        {
            //1.替换现有行，不添加重复行
            string equipCode = DbHelper.dataColumn2String(targetRow["EquipCode"]);
            if (this.newImportEquipmentCodes.Contains(equipCode))
            {
                return false;
            }
            this.newImportEquipmentCodes.Add(equipCode);
            if (this.nowEquipmentCodeTableRowMap.ContainsKey(equipCode))
            {
                //_control.DataSource.Rows.Remove(this.equipmentCodeTableRowIndexMap[equipCode]);
                //删除原有数据
                this.nowEquipmentCodeTableRowMap[equipCode].Delete();
            }

            //2.处置有效期
            double period = targetRow.CorrectValue("CalibrationCycle", 12.0);
            var months = (int)(period * 1);
            DateTime calibratingDate = targetRow.CorrectValue("CalibrationDate", DateTime.Now.Date);
            DateTime expireDate = targetRow.CorrectValue("ExpireDate", calibratingDate.AddMonths(months).AddDays(-1));

            //3.处置组别
            string owner = DbHelper.dataColumn2String(targetRow["Owner"]);
            if (string.IsNullOrWhiteSpace(owner))
            {
                return true;
            }
            //反推组别信息
            UserHelper.Instance.loadUsers();
            if (UserHelper.Instance.UserMap.ContainsKey(owner))
            {
                targetRow["GroupName"] = UserHelper.Instance.UserMap[owner].Department;
            }

            return true;
        }

        private void preHandlerOnImportExcel()
        {
            this.newImportEquipmentCodes = new List<string>();
            this.nowEquipmentCodeTableRowMap = new Dictionary<string, DataRow>();
            DataRowCollection rows = _control.DataSource.Rows;
            for (int index = 0; index < rows.Count; index++)
            {
                DataRow row = rows[index];
                string equipCode = DbHelper.dataColumn2String(row["EquipCode"]);
                if (!this.nowEquipmentCodeTableRowMap.ContainsKey(equipCode))
                {
                    this.nowEquipmentCodeTableRowMap.Add(equipCode, row);
                }
            }
        }


        /// <summary>
        /// 行标色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewOnRowStyleBack(object sender, RowStyleEventArgs e)
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
                if (_control._view.GetRowCellValue(e.RowHandle, _control._view.Columns["EquipState"])?.ToString() == "停用")
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
                    if (_control._view.GetRowCellValue(e.RowHandle, _control._view.Columns["EquipState"])?.ToString() == "检定")
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

        /// <summary>
        /// 行标色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewOnRowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                string groupName = _control._view.GetRowCellValue(e.RowHandle, "GroupName")?.ToString().Trim();
                if (!StringUtils.isEquals(groupName, FormSignIn.CurrentUser.Department.ToString()))
                {
                    e.Appearance.BackColor = Color.LightGreen;
                }              
                string state = _control._view.GetRowCellValue(e.RowHandle, "State")?.ToString().Trim();
                if (StringUtils.isEquals(state, EquipmentStateChn.使用中.ToString()))
                {

                }
                else if (StringUtils.isEquals(state, EquipmentStateChn.未启用.ToString()))
                {
                    e.Appearance.BackColor = Color.Red;
                }
                else if (StringUtils.isEquals(state, EquipmentStateChn.待检定.ToString()))
                {
                    e.Appearance.BackColor = Color.Yellow;
                }
                else if (StringUtils.isEquals(state, EquipmentStateChn.停用.ToString()))
                {
                    e.Appearance.BackColor = Color.Red;
                }
                else if (StringUtils.isEquals(state, EquipmentStateChn.报废.ToString()))
                {
                    e.Appearance.BackColor = Color.Gray;
                }
            }

        }

        protected override DialogResult OpenEditForm(GridView view, int hand, List<DataField> fields)
        {
            Log.e("OpenEditForm");
            var dialog = new EquipEditDialog(FormTable.Edit, _control._view, hand, fields, FormType.Equipment);
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
