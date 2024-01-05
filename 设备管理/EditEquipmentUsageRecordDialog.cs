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
    public partial class EditEquipmentUsageRecordDialog :  BaseEditDialog
    {
        private readonly bool IsAllocateTask;

        private IEquipmentQueryService equipmentQueryService;
        private IEquipmentUsageRecordRepository equipmentUsageRecordRepository;
        private IUserStructureRepository userStructureRepository;

        private EquipmentUsageRecordEntity baseEquipmentUsageRecord;
        private EquipmentUsageRecordEntity updatedEquipmentUsageRecordEntity;

        private List<EquipmentBreiefViewModel> equipmentBreiefViewModels;
        private List<EquipmentLite> equipments;
        private List<string> equipmentCodeTexts;

        private List<string> inTimeEquipmentCodeTexts=new List<string>();
        private Dictionary<string, EquipmentLite> equipmentMap;
        private Dictionary<string, string> equipmenCodeValueMap;

        private List<UserStructureLite> userStructureLites;
        private List<string> groups;
        private List<string> experimentSites;
        private List<string> locationNumbers;

        public EditEquipmentUsageRecordDialog() : base()
        {
            InitializeComponent();
        }

        public EditEquipmentUsageRecordDialog(bool authorityEdit, GridView theView, int theHand, List<DataField> fields, FormType Type1,
           bool isAllocateTask)
           : base(authorityEdit, theView, theHand, fields, Type1)
        {
            InitializeComponent();
            IsAllocateTask = isAllocateTask;

          
            this.equipmentQueryService = new EquipmentQueryService();
            this.equipmentUsageRecordRepository = new EquipmentUsageRecordRepository();
            this.userStructureRepository = new UserStructureRepository();
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

        public void setEquipmentUsageRecord(EquipmentUsageRecordEntity baseEquipmentUsageRecord)
        {
            this.baseEquipmentUsageRecord = baseEquipmentUsageRecord;
        }

        protected override string buildTitle()
        {
            string title = "";
            if (this.operation.Equals(OperationType.ADD))
            {
                title = "新增设备使用记录";
            }
            else if (this.operation.Equals(OperationType.EDIT))
            {
                title = "编辑设备使用记录";
            }

            return title;
        }

        private void EditEquipmentUsageRecordDialog_Load(object sender, EventArgs e)
        {
            if (this.operation.Equals(OperationType.EDIT)) {
                return;
            }
            this.inittAddOperationPage();
        }

        private void inittAddOperationPage() {
            this.initData();
            this.initAddOperationView();
        }

        private void initData()
        {
            UseHolder.Instance.CurrentUser = FormSignIn.CurrentUser;
            this.equipmentBreiefViewModels = this.equipmentQueryService.usingEquipments(UseHolder.Instance.CurrentUser.Department);
            this.equipments = this.equipmentBreiefViewModels.Select(item => item.toEquipmentLite()).ToList();
            this.equipmentMap = new Dictionary<string, EquipmentLite>();
            this.equipmenCodeValueMap = new Dictionary<string, string>();
            this.equipmentCodeTexts = new List<string>();
            this.equipments.ForEach(item => {
                string codeText = item.Group.Equals(UseHolder.Instance.CurrentUser.Department) ? item.Code : $"{item.Code}({item.Group})";
                if (!this.equipmentMap.ContainsKey(codeText)) {
                    this.equipmentMap.Add(codeText, item);
                    this.equipmentCodeTexts.Add(codeText);
                }
                if (!this.equipmenCodeValueMap.ContainsKey(item.Code)) {
                    this.equipmenCodeValueMap.Add(item.Code, codeText);
                }

            });
            this.initUserStructureData();
        }

        private void initAddOperationView() {
           //控件修改为可修改
           foreach(var titleControl in fieldControlMap.Values) {
                titleControl.SetReadOnly(false);
                titleControl.OriginalReadOnly = false;
            }

           //隐藏样本信息
            this.titleComboxItemCarVin.Visible = false;
            this.titleComboxSampleModel.Visible = false;
            this.titleComboxProducer.Visible = false;

            this.initCombox();
            this.initViewValue();
        }

        private void initCombox()
        {
            titleComboxEquipmentCode.SetItems(this.equipmentCodeTexts);
            titleComboxUsePerson.SetItems(FormSignIn.Users);
            titleComboxDepartment.SetItems(FormSignIn.UserDic.Keys);
            titleComboxLocationNo.SetItems(this.locationNumbers);
            titleComboxItem.SetItems(new List<string>());
            titleComboxEquipmentCode.SetTextChange(equipmentCodeChangeHandler);

            titleComboxEquipmentCode.SetTextUpdate(equipmentCodeTextUpdate);
        }

        private void titleComboxLocationNo_Load(object sender, EventArgs e)
        {

        }

        private void initUserStructureData()
        {
            this.userStructureLites = this.userStructureRepository.selectByUser(FormSignIn.CurrentUser.Name);
            this.groups = new List<string>();
            this.experimentSites = new List<string>();
            this.locationNumbers = new List<string>();
            this.userStructureLites.ForEach(item =>
            {
                if (!this.groups.Contains(item.Group))
                {
                    this.groups.Add(item.Group);
                }
                if (!this.experimentSites.Contains(item.ExperimentSite))
                {
                    this.experimentSites.Add(item.ExperimentSite);
                }
                if (!this.locationNumbers.Contains(item.LocationNumber))
                {
                    this.locationNumbers.Add(item.LocationNumber);
                }
            });
            Form1.ComboxDictionary["实验地点"].ForEach(item =>
            {
                if (!this.experimentSites.Contains(item))
                {
                    this.experimentSites.Add(item);
                }
            });
            Form1.ComboxDictionary["定位编号"].ForEach(item =>
            {
                if (!this.locationNumbers.Contains(item))
                {
                    this.locationNumbers.Add(item);
                }
            });
        }

        private void initViewValue()
        {
            titleComboxDepartment.SetValue(FormSignIn.CurrentUser.Department);
            titleComboxUsePerson.SetValue(FormSignIn.CurrentUser.Name);
            titleComboxLocationNo.SetValue(this.locationNumbers[0]);
            titleComboxPreUseState.SetValue("正常");
            titleComboxUseState.SetValue("良好");
            titleComboxPostUseState.SetValue("正常");
            string nowTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            titleComboxUseTime.SetValue(nowTime);

            reshowBaseEquipmentUsageRecord();
        }

        private void reshowBaseEquipmentUsageRecord()
        {
            if (this.baseEquipmentUsageRecord == null)
            {
                return;
            }
          
            //回显设备信息
            this.titleComboxEquipmentCode.SetValue(this.baseEquipmentUsageRecord.EquipmentCode);
            this.titleComboxEquipmentName.SetValue(this.baseEquipmentUsageRecord.EquipmentName);
            this.titleComboxEquipmentType.SetValue(this.baseEquipmentUsageRecord.EquipmentType);

            //回显使用情况
            this.titleComboxPurpose.SetValue(this.baseEquipmentUsageRecord.Purpose);

            //回显项目信息
            this.titleComboxItem.SetValue(this.baseEquipmentUsageRecord.ItemBrief);

            //设备编码内容处理
            string curCode = this.titleComboxEquipmentCode.Text.Trim();
            if (!string.IsNullOrWhiteSpace(curCode) && this.equipmenCodeValueMap.ContainsKey(curCode))
            {
                titleComboxEquipmentCode.SetValue(this.equipmenCodeValueMap[curCode]);
            }
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
                if (this.operation.Equals(OperationType.ADD) && fieldName.Equals("EquipmentCode")) {
                    value = this.equipmentMap[value].Code;
                }
                view.SetRowCellValue(hand, fieldName, value);
            }
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

        private void equipmentCodeChangeHandler(object sender, EventArgs e)
        {
            equipmentCodeChangeEvent();
        }

        private void equipmentCodeChangeEvent()
        {
            string equipmentCode = this.titleComboxEquipmentCode.Text.Trim();
            this.afterEquipmentCodeChanged(equipmentCode);
        }

        private void afterEquipmentCodeChanged(string equipmentCode)
        {
            if (this.equipmentMap.ContainsKey(equipmentCode))
            {
                EquipmentLite equipment = this.equipmentMap[equipmentCode];
                titleComboxEquipmentName.SetValue(equipment.Name);
                titleComboxEquipmentType.SetValue(equipment.Type);
            }
            else {
                titleComboxEquipmentName.SetValue("");
                titleComboxEquipmentType.SetValue("");
            }
        }

        private void equipmentCodeTextUpdate(object sender, EventArgs e)
        {
            try
            {
                int curSelectionStart = this.titleComboxEquipmentCode.comboBox1.SelectionStart;
                this.titleComboxEquipmentCode.comboBox1.Items.Clear();
                this.inTimeEquipmentCodeTexts.Clear();
                foreach (var item in this.equipmentCodeTexts)
                {
                    if (item.Contains(this.titleComboxEquipmentCode.comboBox1.Text))
                    {
                        this.inTimeEquipmentCodeTexts.Add(item);
                    }
                }
                if (Collections.isEmpty(this.inTimeEquipmentCodeTexts))
                {
                    this.inTimeEquipmentCodeTexts.Add("无匹配数据");
                }
                this.titleComboxEquipmentCode.comboBox1.Items.AddRange(this.inTimeEquipmentCodeTexts.ToArray());
                this.titleComboxEquipmentCode.comboBox1.SelectionStart = curSelectionStart;
                Cursor = Cursors.Default;
                this.titleComboxEquipmentCode.comboBox1.DroppedDown = true;
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
        }

        private bool validateParam(out string errorMsg) {
            errorMsg = "";
            if (string.IsNullOrWhiteSpace(this.titleComboxEquipmentCode.Text)) {
                errorMsg = "输入参数有误：设备编码为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.titleComboxEquipmentName.Text))
            {
                errorMsg = "输入参数有误：设备名称为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.titleComboxEquipmentType.Text))
            {
                errorMsg = "输入参数有误：设备型号为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.titleComboxUsePerson.Text))
            {
                errorMsg = "输入参数有误：使用人为空！";
                return false;
            }
            if (this.titleComboxUseTime.Date.Year==1)
            {
                errorMsg = "输入参数有误：使用时间为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.titleComboxPreUseState.Text))
            {
                errorMsg = "输入参数有误：使用前状态为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.titleComboxUseState.Text))
            {
                errorMsg = "输入参数有误：使用状况为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.titleComboxPostUseState.Text))
            {
                errorMsg = "输入参数有误：使用后状态为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.titleComboxDepartment.Text))
            {
                errorMsg = "输入参数有误：组别为空！";
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.titleComboxItem.Text))
            {
                errorMsg = "输入参数有误：项目为空！";
                return false;
            }
            if (this.operation.Equals(OperationType.ADD) && !this.equipmentMap.ContainsKey(this.titleComboxEquipmentCode.Text)) {
                errorMsg = "输入参数有误：该设备编码不存在，请先在设备管理中添加该设备！";
                return false;
            }
            if (!this.titleComboxPostUseState.Text.Equals("正常")&& string.IsNullOrWhiteSpace(this.titleComboxPostUseProblem.Text)) {
                errorMsg = "请补充使用后问题！";
                return false;
            }

            return true;
        }

        private EquipmentUsageRecordEntity extractDataFromUi() {
            string preUseState = GetControlByFieldName("PreUseState").Value().Trim();
            string useState = GetControlByFieldName("UseState").Value().Trim();
            string postUseState = GetControlByFieldName("PostUseState").Value().Trim();
            string postUseProblem = GetControlByFieldName("PostUseProblem").Value().Trim();
            string remark = GetControlByFieldName("Remark").Value().Trim();

            this.updatedEquipmentUsageRecordEntity = new EquipmentUsageRecordEntity()
                .state(this.recordId,preUseState,useState,postUseState,
                postUseProblem,remark);

            return this.updatedEquipmentUsageRecordEntity;
        }

        private bool validateTestStatisticParam(out string errorInfo)
        {
            errorInfo = this.updatedEquipmentUsageRecordEntity.validateState();

            return string.IsNullOrWhiteSpace(errorInfo);
        }
    }
}
