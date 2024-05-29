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
using TaskManager.application.Iservice;
using TaskManager.application.viewmodel;
using TaskManager.common.utils;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
using TaskManager.domain.service;
using TaskManager.domain.valueobject;
using TaskManager.infrastructure.db;

namespace TaskManager
{
    public partial class EditConfigItemDialog : BaseEditDialog
    {

        private ConfigItemEntity baseConfigItem;

        private string groupName = "";

        public EditConfigItemDialog() : base()
        {
            InitializeComponent();
        }

        public EditConfigItemDialog(bool authorityEdit, GridView theView,
            int theHand, List<DataField> fields, FormType fromType)
           : base(authorityEdit, theView, theHand, fields, fromType)
        {
            InitializeComponent();
        }

        public void setBaseConfigItem(ConfigItemEntity baseConfigItem)
        {
            this.baseConfigItem = baseConfigItem;
        }

        protected override void BaseEditDialog_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            controls = GetAllControls(out Panel);
            var count = InitControls();

            #region 调整窗口大小

            #endregion

            #region 事件注册

            #endregion

            flowLayoutPanel1.ResumeLayout(true);
            ResumeLayout(true);
            Log.e("BaseEditDialog_Load");
        }



        protected override string buildTitle()
        {
            string title = "";
            if (this.operation.Equals(OperationType.ADD))
            {
                title = "新增备选项";
            }
            else if (this.operation.Equals(OperationType.EDIT))
            {
                title = "编辑备选项";
            }

            return title;
        }

        private void EditEquipmentUsageRecordDialog_Load(object sender, EventArgs e)
        {
            if (this.operation.Equals(OperationType.EDIT))
            {
                this.initEditOperationPage();
                return;
            }
            this.initAddOperationPage();
        }

        private void initAddOperationPage()
        {
            this.initData();
            this.initAddOperationView();
        }

        private void initEditOperationPage()
        {
            this.initEditOperationView();
        }

        private void initHandledUsageRecordEditOperationPage()
        {
            if (!this.isHandledUsageRecord())
            {
                return;
            }
            this.setAllControlsCanEdit();
        }

        private bool isHandledUsageRecord()
        {
            string vin = this.getValue("CarVin");
            return string.IsNullOrWhiteSpace(vin);
        }

        private void initData()
        {

        }

        private void initAddOperationView()
        {
            //控件修改为可修改
            this.setAllControlsCanEdit();

            this.initCombox();
            this.initViewValue();
            this.reshowBaseConfigItem();
        }

        private void initEditOperationView()
        {
            //登记人可编辑自己的记录，管理员可编辑所有记录
            if (RoleManager.isAdmin(FormSignIn.CurrentUser.Role) || StringUtils.isEquals(FormSignIn.CurrentUser.Name, this.getValue("Registrant")))
            {
                this.setControlsEditabled(true);
                this.titleMutiComboxGroup.SetReadOnly(false);
                this.titleMutiComboxGroup.OriginalReadOnly = false;
            }
            else
            {
                this.setControlsEditabled(false);
                this.titleMutiComboxGroup.SetReadOnly(true);
                this.titleMutiComboxGroup.OriginalReadOnly = true;
                this.setOkBtnEnabled(false);
            }

            this.initCombox();
        }

        private void setAllControlsCanEdit()
        {
            foreach (var titleControl in fieldControlMap.Values)
            {
                titleControl.SetReadOnly(false);
                titleControl.OriginalReadOnly = false;
            }
        }

        private void initCombox()
        {
            this.titleComboxName.SetItems(Form1.ComboxDictionary["配置项名称"]);
            this.titleComboxRegistrant.SetItems(FormSignIn.Users);
            this.initGroupCombox();
        }

        private void initGroupCombox()
        {
            List<CheckItem> checkItems = this.initGroupCheckItems();
            this.titleMutiComboxGroup.SetCheckedItems(checkItems);
        }

        private List<CheckItem> initGroupCheckItems()
        {
            List<CheckItem> checkItems = new List<CheckItem>();

            List<string> groupNames = FormSignIn.UserDic.Keys.ToList();
            List<string> nowGroupNames = this.extractGroupNames();
            groupNames.ForEach(item =>
            {
                CheckItem checkItem = new CheckItem(item, nowGroupNames.Contains(item));
                checkItems.Add(checkItem);
            });

            return checkItems;
        }

        private List<string> extractGroupNames()
        {
            List<string> nowGroupNames = new List<string>();

            string groupValue = "";
            if (this.operation.Equals(OperationType.EDIT))
            {
                 groupValue = view.GetRowCellValue(hand, "GroupName").ToString().Trim();
               
            }
            else if (this.operation.Equals(OperationType.ADD) && this.baseConfigItem != null)
            {
                groupValue = this.baseConfigItem.GroupName;
            }
            if (!string.IsNullOrWhiteSpace(groupValue))
            {
                nowGroupNames = groupValue.Split(',').ToList(); 
            }

            return nowGroupNames;
        }

        private void groupChangeHandler(object sender, EventArgs e)
        {
            groupChangeEvent();
        }

        private void groupChangeEvent()
        {
            //string newGroupName = this.titleComboxGroup.Text.Trim();
            //if (StringUtils.isEquals(newGroupName, this.groupName))
            //{
            //    return;
            //}
            //string spliterChar = string.IsNullOrWhiteSpace(this.groupName) ? "" : "，";
            //this.groupName = $"{ this.groupName}{spliterChar}{newGroupName}";
            //this.titleComboxGroup.SetValue(this.groupName);
        }

        private void initViewValue()
        {
            this.titleComboxRegistrant.SetValue(FormSignIn.CurrentUser.Name);
        }

        private void reshowBaseConfigItem()
        {
            if (this.baseConfigItem == null)
            {
                return;
            }

            //回显配置项信息
            this.titleComboxName.SetValue(this.baseConfigItem.Name);
            //this.titleComboxGroup.SetValue(this.baseConfigItem.GroupName);
            this.titleComboxModule.SetValue(this.baseConfigItem.Moudle);
        }

        protected override void BtnUpdateClick(object sender, EventArgs e)
        {
            //参数合法性验证
            if (!this.validateFormParam())
            {
                return;
            }

            //更新列表数据
            foreach (var control in controls)
            {
                var fieldName = control.Value.Tag?.ToString();
                if (string.IsNullOrWhiteSpace(fieldName))
                    continue;
                var value = control.Value.Value();
                view.SetRowCellValue(hand, fieldName, value);
            }
            view.SetRowCellValue(hand, "GroupName", this.titleMutiComboxGroup.GetSelectedItemsText());

            view.FocusedRowHandle = hand + 1;
            view.FocusedRowHandle = hand; //防止出错

            DialogResult = DialogResult.OK;
            Close();
        }

        protected override bool validateFormParam()
        {
            string errorMsg = "";
            if (!this.validateParam(out errorMsg))
            {
                MessageBox.Show(errorMsg, "错误信息", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }



        private bool validateParam(out string errorMsg)
        {
            errorMsg = "";
            if (string.IsNullOrWhiteSpace(this.titleComboxName.Text))
            {
                errorMsg = "输入参数有误：选项类型为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.titleComboxValue.Text))
            {
                errorMsg = "输入参数有误：选项名称为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.titleComboxRegistrant.Text))
            {
                errorMsg = "输入参数有误：登记人为空！";
                return false;
            }

            return true;
        }
    }
}
