using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Controls;
using ExpertLib.Controls.TitleEditor;
using LabSystem.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TaskManager.application.Iservice;
using TaskManager.application.service;
using TaskManager.application.viewmodel;
using TaskManager.common.utils;
using TaskManager.controller;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
using TaskManager.domain.service;
using TaskManager.domain.valueobject;
using TaskManager.infrastructure.db;

namespace TaskManager
{
    public partial class TestEditDialog : BaseEditDialog
    {
        private static readonly string INPUT_ITEM_PRICE_TIP_TEXT = "填写费用如100.0";

        private readonly bool IsAllocateTask;

        private ISampleQueryService sampleQueryService;
        private ISampleCommandService sampleCommandService;
        private IEquipmentQueryService equipmentQueryService;
        private IEquipmentCommandService equipmentCommandService;

        private ITestStatisticRepository testStatisticRepository;
        private IEquipmentUsageRecordRepository equipmentUsageRecordRepository;
        private IUserStructureRepository userStructureRepository;
        private ITaskRepository taskRepository;

        private int testStatisticId;

        private List<string> carVins;
        private List<string> canisterVins;
        private List<string> vins;
        private List<string> inTimeVins = new List<string>();

        private List<UserStructureLite> userStructureLites;
        private List<string> groups;
        private List<string> experimentSites;
        private List<string> locationNumbers;

        private string itemName = "";

        private string itemGroupLocationKey = "";

        private string oriItemGroupLocationKey = "";

        private List<EquipmentBreiefViewModel> equipmentBreiefViewModels;

        private List<String> inTimeEquipments = new List<string>();

        private Dictionary<string, EquipmentBreiefViewModel> equipmentMap;

        private List<EquipmentUsageRecordLite> testEquipments;
        private List<EquipmentLite> itemEquipments;
        private List<EquipmentLite> oriItemEquipments;

        private List<string> itemNowEquipmentCodes;

        private Dictionary<string, EquipmentLite> itemEquipmentMap;

        private Dictionary<string, List<EquipmentLite>> itemOriEquipmentsMap = new Dictionary<string, List<EquipmentLite>>();

        private bool isAddItemEquipments = false;

        private bool isNeedUpdateItemEquipments = false;

        private bool isUpdateItemEquipments = false;

        private TestStatisticEntity oriTestStatisticEntity;
        private List<String> oriTestEquipmentCodes;

        private TestStatisticEntity testStatisticEntity;

        private List<EquipmentUsageRecordEntity> equipmentUsageRecordEntities;

        private EquipmentUsageRecordTestPart equipmentUsageRecordTestPart;

        private SampleBrief updatedSampleBrief;

        private SampleOfVinViewModel sampleOfVin;

        private bool isVinFromStatistic = false;

        private bool isAddSample = false;

        private bool isNeedUpdateSample = false;

        private List<FieldChangedState> updateSampleChangedStates;

        private bool isUpdateSample = false;

        private bool isNeedReplaceTestEquipments = false;
        private bool isNeedUpdateEquipmentRecordTestProperty = false;

        private Button btn = new Button();

        private bool isItemNameValueInitial = true;
        private bool isVinValueInitial = true;

        private string sampleModel;

        private List<TaskBrief> taskBriefs;
        private Dictionary<string, TaskBrief> taskBriefMap = new Dictionary<string, TaskBrief>();

        private TitleCombox titleComboxSampleType;
        private TitleCombox titleComboxVin;
        private TitleCombox titleComboxPriceDetail;
        private TitleCombox titleComboxProjectTotal;

        private SampleBrief testTaskSample = null;
        private int testTaskSampleId = -1;

        private string oriSampleType;
        private string sampleType;

        private TestEditDialog() : base()
        {
            InitializeComponent();
        }

        public TestEditDialog(bool authorityEdit, GridView theView, int theHand, List<DataField> fields, FormType Type1,
            bool isAllocateTask,bool isFromCustomTemplate)
            : base(authorityEdit, theView, theHand, fields, Type1)
        {
            InitializeComponent();
            IsAllocateTask = isAllocateTask;
            this.isFromCustomTemplate = isFromCustomTemplate;

            this.sampleQueryService = new SampleQueryService();
            this.sampleCommandService = new SampleCommandService();
            this.testStatisticRepository = new TestStatisticRepository();
            this.equipmentQueryService = new EquipmentQueryService();
            this.equipmentCommandService = new EquipmentCommandService();
            this.equipmentUsageRecordRepository = new EquipmentUsageRecordRepository();
            this.userStructureRepository = new UserStructureRepository();
            this.taskRepository = new TaskRepository();
        }

        protected TitleCombox FinishState;
        protected TitleDate FinishMonth;
        protected override void BaseEditDialog_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();

            //初始化控件
            controls = GetAllControls(out Panel);
            var count = InitControls();

            //调整窗口大小
            this.setWindowSize();

            //事件注册
            this.bindEventForView();

            flowLayoutPanel1.ResumeLayout(true);
            ResumeLayout(true);
            Log.e("BaseEditDialog_Load");
        }

        private void TestEditDialog_Load(object sender, EventArgs e)
        {
            this.initPage();
            this.initControlByDepartment();
        }

        private void initPage()
        {
            this.initData();
            this.initView();
        }

        public int getId()
        {
            return this.testStatisticId;
        }

        private void initControlByDepartment()
        {
            //GetControlByFieldName("Carvin").SetReadOnly(false);
            //GetControlByFieldName("ItemBrief").SetReadOnly(false);
            //GetControlByFieldName("Taskcode").SetReadOnly(false);
            List<string> readOnlyCols = new List<string> { "Carvin", "ItemBrief", "Taskcode",
                "CarType","SampleModel","Producer","YNDirect","PowerType","TransmissionType",
                "EngineModel","EngineProduct","Drivertype","FuelType","FuelLabel"};
            readOnlyCols.ForEach(item => GetControlByFieldName(item).SetReadOnly(false));

            if (FormSignIn.CurrentUser.Department == "系统维护")
            {
                //TODO:之后记得恢复
                return;
            }
            if (FormSignIn.CurrentUser.Department != "体系组")
            {
                //非体系组不能编辑费用确认信息
                ((TitleCombox)GetControlByFieldName("MoneySure")).SetReadOnly(true);
            }

            if (FormSignIn.CurrentUser.Department == "体系组")
            {
                //所有信息不可编辑
                this.setAllControlsUnEditabled();

                //费用确认信息可编辑
                ((TitleCombox)GetControlByFieldName("MoneySure")).SetReadOnly(false);
            }
            else
            {
                //TODO:有啥用呢
                //Tirepressure
                ((TitleCombox)GetControlByFieldName("Tirepressure")).comboBox1.SelectAll();
                //FuelType
                ((TitleCombox)GetControlByFieldName("FuelType")).comboBox1.SelectAll();
                //ProjectTotal
                ((TitleCombox)GetControlByFieldName("ProjectTotal")).comboBox1.SelectAll();
            }
        }

        private void setAllControlsUnEditabled()
        {
            //所有信息不可编辑
            this.setControlsEditabled(false);
            this.titleComboxPriceItem.SetReadOnly(true);
            this.textBoxItemPrice.ReadOnly = true;
            this.btnAddPriceDetails.Enabled = false;
            

            this.titleComboxEquip.SetReadOnly(true);
            this.btnAddEquipment.Enabled = false;
        }

        private void setWindowSize()
        {
            //var borderX = Size.Width - Panel.Width;
            //var borderY = Size.Height - Panel.Height;

            //const int len = 370 + 4;
            //const int height = 30 + 4;
            //var rowCount = (int)Math.Ceiling(count / 2.0) + 1;

            //var w = len * 2 + Panel.Margin.Left + Panel.Margin.Right + borderX;
            //var h = rowCount * height + Panel.Margin.Top + Panel.Margin.Bottom + borderY;
            //Size = new Size(w, h);
        }

        private void bindEventForView()
        {
            // 总里程
            if (GetControlByFieldName("StartMileage") is TitleCombox StartMileage &&
                GetControlByFieldName("EndMileage") is TitleCombox EndMileage)
            {
                StartMileage.SetTextChange(MileageHandler);
                EndMileage.SetTextChange(MileageHandler);
                MileageHandler(null, null);
            }
            //费用总计
            //注：2023-12-11该功能注释掉
            //if (GetControlByFieldName("ProjectPrice") is TitleCombox ProjectPrice &&
            //    GetControlByFieldName("ProjectTotal") is TitleCombox ProjectTotal)
            //{
            //    ProjectPrice.SetTextChange(ProjectPriceHandler);
            //    ProjectPriceHandler(null, null);
            //}
            if (GetControlByFieldName("TestStartDate") is DateEdit StartTime &&
                GetControlByFieldName("TestEndDate") is DateEdit EndTime &&
                GetControlByFieldName("Testtime") is TitleCombox)
            {
                StartTime.SetValueChange(TimeSpanHandler);
                EndTime.SetValueChange(TimeSpanHandler);
                TimeSpanHandler(null, null);
            }
            if (GetControlByFieldName("FinishDate") is TitleDate finishMonth &&
                 GetControlByFieldName("State") is TitleCombox state)
            {
                FinishMonth = finishMonth;
                FinishState = state;
                FinishState.SetTextChange(FinishStateChangeHandler);
                FinishStateChangeHandler(null, null);
            }
        }


        protected override void BtnUpdateClick(object sender, EventArgs e)
        {
            try
            {
                //验证taskCode
                if (!validateTaskCode())
                {
                    return;
                }

                //提取统计数据
                this.extractDataFromUi();

                //参数合法性验证
                string errorInfo = "";
                if (!this.validateTestStatisticParam(out errorInfo))
                {
                    MessageBox.Show($"参数有误：{errorInfo}", "错误信息", MessageBoxButtons.OK);
                    return;
                }

                //重置保存数据策略
                this.resetSaveDataState();

                //设备记录更新策略
                this.updateTestEquipmentStrategy();

                //样本同步策略
                this.syncSampleStrategy();

                //项目设备同步策略
                this.syncItemEquipmentStrategy();
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新试验计划失败！", "错误信息", MessageBoxButtons.OK);
                return;
            }
            //是否需要将新增替换为更新，目的完善样本的vin信息，触发条件:1.样本为新增；2.样本类型未变化，vin变化;3.原样本记录存在
            if (this.isAddSample && StringUtils.Equals(this.oriSampleType, this.sampleType) && this.testTaskSampleId > 0)
            {
                string msg = $"您输入的是一个新的样本vin，您是否仅仅需要修改当前实验项目的样本信息，而不是创建一个新的样本信息";
                DialogResult result = MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.sampleAddOperationChangeToUpdate();
                }
            }

            //是否需要更新样本
            if (this.isNeedUpdateSample)
            {
                string msg = $"检测到该VIN样本信息有变化({FieldChangedState.buildChangedDescription(this.updateSampleChangedStates)})，需要将该样本信息更新至样本数据表么";
                DialogResult result = MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.isUpdateSample = true;
                }
            }
            //是否需要更新项目设备
            if (this.isNeedUpdateItemEquipments)
            {
                DialogResult result = MessageBox.Show("检测到该项目的使用设备信息有变化，需要将该项目使用设备信息更新至数据库么", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.isUpdateItemEquipments = true;
                }
            }

            //保存数据
            try
            {
                this.executeSaveData();
                DialogResult result = MessageBox.Show("更新试验计划成功", "提示", MessageBoxButtons.OK);
                if (result == DialogResult.OK)
                {
                    afterUpdateSuccess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新试验计划失败！", "错误信息", MessageBoxButtons.OK);
            }
        }

        private void sampleAddOperationChangeToUpdate()
        {
            this.updatedSampleBrief.Id = this.testTaskSampleId;
            if (this.sampleOfVin == null)
            {
                this.sampleOfVin = new SampleOfVinViewModel();
            }
            this.sampleOfVin.FromSampleTable = this.testTaskSample;
            this.isAddSample = false;
            this.isUpdateSample = true;
        }

        private void afterUpdateSuccess()
        {
            //更新控件值
            foreach (var control in controls)
            {
                var fieldName = control.Value.Tag?.ToString();
                if (string.IsNullOrWhiteSpace(fieldName))
                    continue;

                var value = control.Value.Value();
                view.SetRowCellValue(hand, fieldName, value);
            }
            //更新设备信息
            view.SetRowCellValue(hand, "Equipments", this.testStatisticEntity.Equipments);
            view.FocusedRowHandle = hand + 1;
            view.FocusedRowHandle = hand; //防止出错
            //自动更新保密级别   
            //注：2023-12-11注释该功能
            //if (this.testStatisticEntity.Taskcode != "")
            //{
            //    string sql = $"select SecurityLevel from NewTaskTable where Taskcode='{this.testStatisticEntity.Taskcode}'";
            //    if (SqlHelper.GetList(sql).Rows.Count > 0)
            //    {
            //        view.SetRowCellValue(hand, "Confidentiality", SqlHelper.GetList(sql).Rows[0][0].ToString());
            //    }
            //}
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool validateTaskCode()
        {
            string taskCode = GetControlByFieldName("Taskcode").Value().Trim();
            if (string.IsNullOrWhiteSpace(taskCode) || taskCode.Contains("?"))
            {
                return true;
            }

            //验证费用
            string sqlmoneysure = $"select count(*) from TestStatistic where Taskcode='{taskCode}' and MoneySure ='是'";
            if (SqlHelper.GetList(sqlmoneysure).Rows[0][0].ToString() != "0")
            {
                if (MessageBox.Show("该任务单号费用已确认，是否重新填写！", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    return false;
                }
                else
                {
                    GetControlByFieldName("Taskcode").SetValue(taskCode + "?");
                };
            }

            //验证任务单好和样本型号匹配
            string sampleModel = GetControlByFieldName("SampleModel").Value().Trim();
            //任务单号有疑问或者样本模型为空，不做进一步验证
            if (string.IsNullOrWhiteSpace(sampleModel))
            {
                return true;
            }

            if (!this.taskBriefMap.ContainsKey(taskCode))
            {

                if (MessageBox.Show("该任务单号与车型不一致，是否重新填写！", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    GetControlByFieldName("Taskcode").SetValue(taskCode + "?");
                }
                else
                {
                    return false;
                };
            }

            return true;
        }

        private void buildOriTestStatisticEntity()
        {
            this.oriTestStatisticEntity = new TestStatisticEntity();
            this.oriTestStatisticEntity.Id = int.Parse(this.getValue("ID"));

            this.oriTestStatisticEntity.Department = this.getValue("department");
            this.oriTestStatisticEntity.ExperimentalSite = this.getValue("ExperimentalSite");
            this.oriTestStatisticEntity.LocationNumber = this.getValue("LocationNumber");
            this.oriTestStatisticEntity.Registrant = this.getValue("Registrant");

            this.oriTestStatisticEntity.ItemType = this.getValue("ItemType");
            this.oriTestStatisticEntity.ItemBrief = this.getValue("ItemBrief");
            this.oriTestStatisticEntity.Taskcode = this.getValue("Taskcode");
            this.oriTestStatisticEntity.Taskcode2 = this.getValue("Taskcode2");
            this.oriTestStatisticEntity.StandardStage = this.getValue("StandardStage");
            this.oriTestStatisticEntity.YNcountry = this.getValue("YNcountry");
            this.oriTestStatisticEntity.Confidentiality = this.getValue("Confidentiality");
            this.oriTestStatisticEntity.ProjectPrice = this.string2Double(this.getValue("ProjectPrice"));

            this.oriTestStatisticEntity.SampleType = this.getValue("SampleType");
            this.oriTestStatisticEntity.CarVin = this.getValue("Carvin");
            this.oriTestStatisticEntity.CarType = this.getValue("CarType");
            this.oriTestStatisticEntity.SampleModel = this.getValue("SampleModel");
            this.oriTestStatisticEntity.Producer = this.getValue("Producer");
            this.oriTestStatisticEntity.PowerType = this.getValue("PowerType");
            this.oriTestStatisticEntity.EngineModel = this.getValue("EngineModel");
            this.oriTestStatisticEntity.EngineProduct = this.getValue("EngineProduct");
            this.oriTestStatisticEntity.YNDirect = this.getValue("YNDirect");
            this.oriTestStatisticEntity.TransmissionType = this.getValue("TransmissionType");
            this.oriTestStatisticEntity.DriverType = this.getValue("Drivertype");
            this.oriTestStatisticEntity.FuelType = this.getValue("FuelType");
            this.oriTestStatisticEntity.FuelLabel = this.getValue("FuelLabel");
            this.oriTestStatisticEntity.TirePressure = this.getValue("Tirepressure");
            this.oriTestStatisticEntity.CanisterCode = this.getValue("CanisterCode");
            this.oriTestStatisticEntity.CanisterType = this.getValue("CanisterType");
            this.oriTestStatisticEntity.CanisterProductor = this.getValue("CanisterProductor");

            this.oriTestStatisticEntity.CanisterTotalWeight = this.getValue("CanisterTotalWeight");
            this.oriTestStatisticEntity.ActivatedCarbonTotalWeight = this.getValue("ActivatedCarbonTotalWeight");
            this.oriTestStatisticEntity.ActivatedCarbonVolumeActual = this.getValue("ActivatedCarbonVolumeActual");
            this.oriTestStatisticEntity.CanisterEffectiveVolumeActual = this.getValue("CanisterEffectiveVolumeActual");
            this.oriTestStatisticEntity.CanisterEffectiveVolumePublic = this.getValue("CanisterEffectiveVolumePublic");
            this.oriTestStatisticEntity.CanisterEffectiveVolumeConformance = this.getValue("CanisterEffectiveVolumeConformance");
            this.oriTestStatisticEntity.CanisterEffectiveAdsorptionActual = this.getValue("CanisterEffectiveAdsorptionActual");
            this.oriTestStatisticEntity.CanisterEffectiveAdsorptionPublic = this.getValue("CanisterEffectiveAdsorptionPublic");
            this.oriTestStatisticEntity.CanisterEffectiveAdsorptionConformance = this.getValue("CanisterEffectiveAdsorptionConformance");
            this.oriTestStatisticEntity.CanisterWorkingAbilityActual = this.getValue("CanisterWorkingAbilityActual");
            this.oriTestStatisticEntity.CanisterWorkingAbilityPublic = this.getValue("CanisterWorkingAbilityPublic");
            this.oriTestStatisticEntity.CanisterWorkingAbilityConformance = this.getValue("CanisterWorkingAbilityConformance");

            this.oriTestStatisticEntity.TestStartDate = this.string2DateTime(this.getValue("TestStartDate"));
            this.oriTestStatisticEntity.TestEndDate = this.string2DateTime(this.getValue("TestEndDate"));
            this.oriTestStatisticEntity.TestTime = this.getValue("Testtime");
            this.oriTestStatisticEntity.StartMileage = this.getValue("StartMileage");
            this.oriTestStatisticEntity.EndMileage = this.getValue("EndMileage");
            this.oriTestStatisticEntity.TotalMileage = this.getValue("TotalMileage");
            this.oriTestStatisticEntity.NationalFive = this.getValue("NationalFive");
            this.oriTestStatisticEntity.NationalSix = this.getValue("NationalSix");
            this.oriTestStatisticEntity.Oil = this.getValue("Oil");
            this.oriTestStatisticEntity.QualificationStatus = this.getValue("QualificationStatus");
            this.oriTestStatisticEntity.Finishstate = this.getValue("Finishstate");
            this.oriTestStatisticEntity.Remark = this.getValue("Remark");
            this.oriTestStatisticEntity.LogisticsInformation = this.getValue("LogisticsInformation");
            this.oriTestStatisticEntity.Contacts = this.getValue("Contacts");
            this.oriTestStatisticEntity.PhoneNum = this.getValue("phoneNum");
            this.oriTestStatisticEntity.Email = this.getValue("Email");
            this.oriTestStatisticEntity.ProjectTotal = this.string2Double(this.getValue("ProjectTotal"));
            this.oriTestStatisticEntity.PriceDetail = this.getValue("PriceDetail");
            this.oriTestStatisticEntity.MoneySure = this.getValue("MoneySure");

            this.oriTestStatisticEntity.RegistrationDate = this.getValue("RegistrationDate");
            this.oriTestStatisticEntity.Question = this.getValue("question");
            this.oriTestStatisticEntity.Equipments = this.getValue("Equipments");
        }

        private TestStatisticEntity extractDataFromUi()
        {
            this.testStatisticEntity = new TestStatisticEntity();
            this.testStatisticEntity.Id = int.Parse(this.getValue("ID"));

            this.testStatisticEntity.Department = ((TitleCombox)GetControlByFieldName("department")).Text;
            this.testStatisticEntity.ExperimentalSite = ((TitleCombox)GetControlByFieldName("ExperimentalSite")).Text;
            this.testStatisticEntity.LocationNumber = ((TitleCombox)GetControlByFieldName("LocationNumber")).Text;
            this.testStatisticEntity.Registrant = ((TitleCombox)GetControlByFieldName("Registrant")).Text;

            this.testStatisticEntity.ItemType = ((TitleCombox)GetControlByFieldName("ItemType")).Text;
            this.testStatisticEntity.ItemBrief = ((TitleCombox)GetControlByFieldName("ItemBrief")).Text;
            this.testStatisticEntity.Taskcode = ((TitleCombox)GetControlByFieldName("Taskcode")).Text;
            this.testStatisticEntity.Taskcode2 = ((TitleCombox)GetControlByFieldName("Taskcode2")).Text;
            this.testStatisticEntity.StandardStage = ((TitleCombox)GetControlByFieldName("StandardStage")).Text;
            this.testStatisticEntity.YNcountry = ((TitleCombox)GetControlByFieldName("YNcountry")).Text;
            this.testStatisticEntity.Confidentiality = ((TitleCombox)GetControlByFieldName("Confidentiality")).Text;
            this.testStatisticEntity.ProjectPrice = this.string2Double((((TitleCombox)GetControlByFieldName("ProjectPrice"))).Text);

            this.testStatisticEntity.SampleType = ((TitleCombox)GetControlByFieldName("SampleType")).Text;
            this.testStatisticEntity.CarVin = ((TitleCombox)GetControlByFieldName("Carvin")).Text;
            this.testStatisticEntity.CarType = ((TitleCombox)GetControlByFieldName("CarType")).Text;
            this.testStatisticEntity.SampleModel = ((TitleCombox)GetControlByFieldName("SampleModel")).Text;
            this.testStatisticEntity.Producer = ((TitleCombox)GetControlByFieldName("Producer")).Text;
            this.testStatisticEntity.PowerType = ((TitleCombox)GetControlByFieldName("PowerType")).Text;
            this.testStatisticEntity.EngineModel = ((TitleCombox)GetControlByFieldName("EngineModel")).Text;
            this.testStatisticEntity.EngineProduct = ((TitleCombox)GetControlByFieldName("EngineProduct")).Text;
            this.testStatisticEntity.YNDirect = ((TitleCombox)GetControlByFieldName("YNDirect")).Text;
            this.testStatisticEntity.TransmissionType = ((TitleCombox)GetControlByFieldName("TransmissionType")).Text;
            this.testStatisticEntity.DriverType = ((TitleCombox)GetControlByFieldName("Drivertype")).Text;
            this.testStatisticEntity.FuelType = ((TitleCombox)GetControlByFieldName("FuelType")).Text;
            this.testStatisticEntity.FuelLabel = ((TitleCombox)GetControlByFieldName("FuelLabel")).Text;
            this.testStatisticEntity.TirePressure = ((TitleCombox)GetControlByFieldName("Tirepressure")).Text;
            this.testStatisticEntity.CanisterCode = ((TitleCombox)GetControlByFieldName("CanisterCode")).Text;
            this.testStatisticEntity.CanisterType = ((TitleCombox)GetControlByFieldName("CanisterType")).Text;
            this.testStatisticEntity.CanisterProductor = ((TitleCombox)GetControlByFieldName("CanisterProductor")).Text;

            this.testStatisticEntity.CanisterTotalWeight = ((TitleCombox)GetControlByFieldName("CanisterTotalWeight")).Text;
            this.testStatisticEntity.ActivatedCarbonTotalWeight = ((TitleCombox)GetControlByFieldName("ActivatedCarbonTotalWeight")).Text;
            this.testStatisticEntity.ActivatedCarbonVolumeActual = ((TitleCombox)GetControlByFieldName("ActivatedCarbonVolumeActual")).Text;
            this.testStatisticEntity.CanisterEffectiveVolumeActual = ((TitleCombox)GetControlByFieldName("CanisterEffectiveVolumeActual")).Text;
            this.testStatisticEntity.CanisterEffectiveVolumePublic = ((TitleCombox)GetControlByFieldName("CanisterEffectiveVolumePublic")).Text;
            this.testStatisticEntity.CanisterEffectiveVolumeConformance = ((TitleCombox)GetControlByFieldName("CanisterEffectiveVolumeConformance")).Text;
            this.testStatisticEntity.CanisterEffectiveAdsorptionActual = ((TitleCombox)GetControlByFieldName("CanisterEffectiveAdsorptionActual")).Text;
            this.testStatisticEntity.CanisterEffectiveAdsorptionPublic = ((TitleCombox)GetControlByFieldName("CanisterEffectiveAdsorptionPublic")).Text;
            this.testStatisticEntity.CanisterEffectiveAdsorptionConformance = ((TitleCombox)GetControlByFieldName("CanisterEffectiveAdsorptionConformance")).Text;
            this.testStatisticEntity.CanisterWorkingAbilityActual = ((TitleCombox)GetControlByFieldName("CanisterWorkingAbilityActual")).Text;
            this.testStatisticEntity.CanisterWorkingAbilityPublic = ((TitleCombox)GetControlByFieldName("CanisterWorkingAbilityPublic")).Text;
            this.testStatisticEntity.CanisterWorkingAbilityConformance = ((TitleCombox)GetControlByFieldName("CanisterWorkingAbilityConformance")).Text;


            this.testStatisticEntity.TestStartDate = ((DateEdit)GetControlByFieldName("TestStartDate")).Date;
            this.testStatisticEntity.TestEndDate = ((DateEdit)GetControlByFieldName("TestEndDate")).Date;
            this.testStatisticEntity.TestTime = ((TitleCombox)GetControlByFieldName("Testtime")).Text;
            this.testStatisticEntity.StartMileage = ((TitleCombox)GetControlByFieldName("StartMileage")).Text;
            this.testStatisticEntity.EndMileage = ((TitleCombox)GetControlByFieldName("EndMileage")).Text;
            this.testStatisticEntity.TotalMileage = ((TitleCombox)GetControlByFieldName("TotalMileage")).Text;
            this.testStatisticEntity.NationalFive = ((TitleCombox)GetControlByFieldName("NationalFive")).Text;
            this.testStatisticEntity.NationalSix = ((TitleCombox)GetControlByFieldName("NationalSix")).Text;
            this.testStatisticEntity.Oil = ((TitleCombox)GetControlByFieldName("Oil")).Text;
            this.testStatisticEntity.QualificationStatus = ((TitleCombox)GetControlByFieldName("QualificationStatus")).Text;
            this.testStatisticEntity.Finishstate = ((TitleCombox)GetControlByFieldName("Finishstate")).Text;
            this.testStatisticEntity.Remark = ((TitleCombox)GetControlByFieldName("Remark")).Text;
            this.testStatisticEntity.LogisticsInformation = ((TitleCombox)GetControlByFieldName("LogisticsInformation")).Text;
            this.testStatisticEntity.Contacts = ((TitleCombox)GetControlByFieldName("Contacts")).Text;
            this.testStatisticEntity.PhoneNum = ((TitleCombox)GetControlByFieldName("phoneNum")).Text;
            this.testStatisticEntity.Email = ((TitleCombox)GetControlByFieldName("Email")).Text;
            this.testStatisticEntity.ProjectTotal = this.string2Double((((TitleCombox)GetControlByFieldName("ProjectTotal"))).Text);
            this.testStatisticEntity.PriceDetail = ((TitleCombox)GetControlByFieldName("PriceDetail")).Text;
            this.testStatisticEntity.MoneySure = ((TitleCombox)GetControlByFieldName("MoneySure")).Text;

            this.testStatisticEntity.RegistrationDate = this.oriTestStatisticEntity.RegistrationDate;
            this.testStatisticEntity.Question = this.oriTestStatisticEntity.Question;
            this.testStatisticEntity.setEquipments(this.itemEquipments);

            //维护信息
            this.testStatisticEntity.CreateUser = this.oriTestStatisticEntity.CreateUser;
            this.testStatisticEntity.CreateTime = this.oriTestStatisticEntity.CreateTime;
            this.testStatisticEntity.UpdateTime = DateTime.Now;

            return testStatisticEntity;
        }

        private void buildTestEquipmentRecords()
        {
            this.equipmentUsageRecordEntities = new List<EquipmentUsageRecordEntity>();
            if (Collections.isEmpty(this.itemEquipments))
            {
                return;
            }

            this.itemEquipments.ForEach(equipment =>
            {
                EquipmentUsageRecordEntity entity = new EquipmentUsageRecordEntity()
                .defaultParam()
                .equipmentInfo(equipment)
                .fromTest(this.testStatisticEntity)
                .fixFromTestUpdated(this.testStatisticEntity);

                this.equipmentUsageRecordEntities.Add(entity);
            });
        }

        private bool validateTestStatisticParam(out string errorInfo)
        {
            errorInfo = this.testStatisticEntity.validateCreated();

            return string.IsNullOrWhiteSpace(errorInfo);
        }

        private void syncSampleStrategy()
        {
            //构造样本信息
            this.updatedSampleBrief = this.testStatisticEntity.sampleBriefInfo();
            //this.updatedSampleBrief.SampleType = titleComboxSampleType.Text;
            if (this.sampleOfVin == null || this.sampleOfVin.FromSampleTable == null)
            {
                this.isAddSample = true;
                return;
            }

            //样本类型和vin变化了，则为新增，这种情况理论上不存在
            if (!this.sampleOfVin.FromSampleTable.equalsSameSample(this.updatedSampleBrief))
            {
                this.isAddSample = true;
                return;
            }

            this.updatedSampleBrief.Id = this.sampleOfVin.FromSampleTable.Id;
            //this.updatedSampleBrief.SampleType = this.sampleOfVin.FromSampleTable.SampleType;
            this.isNeedUpdateSample = !this.sampleOfVin.FromSampleTable.equals(this.updatedSampleBrief, out this.updateSampleChangedStates);
        }

        private void syncItemEquipmentStrategyOld()
        {
            //新添加则直接保存
            this.itemNowEquipmentCodes = this.itemEquipments.Select(item => item.Code).ToList();
            List<string> itemOriEquipmentCodes = this.itemOriEquipmentsMap[this.itemGroupLocationKey].Select(item => item.Code).ToList();
            if (Collections.isEmpty(itemOriEquipmentCodes))
            {
                this.isAddItemEquipments = true;
                return;
            }

            this.isNeedUpdateItemEquipments = !Collections.equals(itemOriEquipmentCodes, itemNowEquipmentCodes);
        }

        private void syncItemEquipmentStrategy()
        {
            //新添加则直接保存
            this.itemNowEquipmentCodes = this.itemEquipments.Select(item => item.Code).ToList();
            List<string> itemOriEquipmentCodes = this.itemOriEquipmentsMap[this.itemGroupLocationKey].Select(item => item.Code).ToList();

            //原始记录的项目，组别，定位编号
            if (StringUtils.isEquals(this.itemGroupLocationKey, this.oriItemGroupLocationKey))
            {
                itemOriEquipmentCodes = this.oriItemEquipments.Select(item => item.Code).ToList();
            }
            if (Collections.isEmpty(itemOriEquipmentCodes))
            {
                this.isAddItemEquipments = true;
                return;
            }

            this.isNeedUpdateItemEquipments = false;
        }


        private void updateTestEquipmentStrategy()
        {
            //新添加则直接保存
            this.itemNowEquipmentCodes = this.itemEquipments.Select(item => item.Code).ToList();

            this.isNeedReplaceTestEquipments = !Collections.equals(oriTestEquipmentCodes, itemNowEquipmentCodes);

            //构造实验用设备记录
            if (this.isNeedReplaceTestEquipments)
            {
                this.buildTestEquipmentRecords();
            }
            else
            {
                this.equipmentUsageRecordTestPart = new EquipmentUsageRecordTestPart().fromTest(this.testStatisticEntity);
                EquipmentUsageRecordTestPart oriEquipmentUsageRecordTestPart = new EquipmentUsageRecordTestPart().fromTest(this.oriTestStatisticEntity);
                this.isNeedUpdateEquipmentRecordTestProperty = !this.equipmentUsageRecordTestPart.equals(oriEquipmentUsageRecordTestPart);
            }
        }

        private void executeSaveData()
        {
            //保存统计信息
            this.testStatisticId = this.testStatisticRepository.save(this.testStatisticEntity);
            this.testStatisticEntity.Id = this.testStatisticId;

            //保存设备使用记录
            if (this.isNeedReplaceTestEquipments)
            {
                this.equipmentUsageRecordRepository.removeByTestTaskId(testStatisticId);
                this.equipmentUsageRecordEntities.ForEach(entity => entity.TestTaskId = testStatisticId);
                this.equipmentUsageRecordRepository.batchSave(this.equipmentUsageRecordEntities);
            }
            else if (this.isNeedUpdateEquipmentRecordTestProperty)
            {
                this.equipmentUsageRecordRepository.updateTestTaskProperty(this.equipmentUsageRecordTestPart);

                this.updateEquipmentUsageRecordrRemark();
            }

            //更新样本信息
            if (isAddSample)
            {
                this.sampleCommandService.createByBrief(this.updatedSampleBrief);
                //更新内存中的样本数据
                this.updateCurFromSampleTable();
                CacheDataHandler.Instance.addVin(this.sampleType, this.updatedSampleBrief.Vin);
            }
            else if (isUpdateSample)
            {
                this.sampleCommandService.updateByBrief(this.updatedSampleBrief);
                //更新内存中的样本数据
                if (!StringUtils.isEquals(this.sampleOfVin.FromSampleTable.Vin, this.updatedSampleBrief.Vin))
                {
                    CacheDataHandler.Instance.replaceVin(this.sampleType, this.sampleOfVin.FromSampleTable.Vin, this.updatedSampleBrief.Vin);
                }

                this.sampleOfVin.FromSampleTable.copyFrom(this.updatedSampleBrief);
            }

            //更新项目设备信息
            if (isAddItemEquipments)
            {
                this.equipmentCommandService.createItemEquipments(this.testStatisticEntity.ItemBrief,
                    this.testStatisticEntity.Department, this.testStatisticEntity.LocationNumber, this.itemNowEquipmentCodes);
            }
            else if (isUpdateItemEquipments)
            {
                this.equipmentCommandService.updateItemEquipments(this.testStatisticEntity.ItemBrief,
                    this.testStatisticEntity.Department, this.testStatisticEntity.LocationNumber, this.itemNowEquipmentCodes);
            }
            //后置处理
            this.resetAfterSaveData();
        }

        private void resetAfterSaveData()
        {
            if (isAddItemEquipments || isUpdateItemEquipments)
            {
                this.itemOriEquipmentsMap[this.itemGroupLocationKey] = this.itemEquipments.Select(item => item.copy()).ToList();
            }
            this.isAddSample = false;
            this.isUpdateSample = false;
            this.isAddItemEquipments = false;
            this.isUpdateItemEquipments = false;
        }

        private void resetSaveDataState()
        {
            this.isAddSample = false;
            this.isUpdateSample = false;
            this.isNeedUpdateSample = false;

            this.isAddItemEquipments = false;
            this.isUpdateItemEquipments = false;
            this.isNeedUpdateItemEquipments = false;

            this.isNeedReplaceTestEquipments = false;
            this.isNeedUpdateEquipmentRecordTestProperty = false;
        }

        private void updateCurFromSampleTable()
        {
            if (this.sampleOfVin == null)
            {
                this.sampleOfVin = new SampleOfVinViewModel();
            }
            this.sampleOfVin.FromSampleTable = this.updatedSampleBrief;
        }

        private void FinishStateChangeHandler(object sender, EventArgs e)
        {
            if (!FinishState.Value().Trim().Equals("已完成"))
            {
                FinishMonth.SetValue("");
                return;
            }
            else
            {
                string month = DateTime.Now.ToString("yyyy-MM-dd");
                FinishMonth.SetValue(month);
            }

            //if (string.IsNullOrWhiteSpace(PlanMonth.Value()))
            //    return;

        }

        protected override bool FieldVisible(DataField field)
        {
            if (!IsAllocateTask)
                return field.EditorVisible;
            else
                return field.EditorVisible && field.Group.Equals("分配试验任务");
        }


        private void ProjectPriceHandler(object sender, EventArgs e)
        {

            if (!(GetControlByFieldName("ProjectPrice") is TitleCombox ProjectPrice) ||
               !(GetControlByFieldName("ProjectTotal") is TitleCombox ProjectTotal))
                return;
            if (ProjectTotal.Value() == "")
            {
                ProjectTotal.SetValue(ProjectPrice.Value().ToString());
            }


        }

        private void MileageHandler(object sender, EventArgs e)
        {

            if (!(GetControlByFieldName("StartMileage") is TitleCombox StartMileage) ||
                !(GetControlByFieldName("EndMileage") is TitleCombox EndMileage) ||
                !(GetControlByFieldName("TotalMileage") is TitleCombox TotalMileage))
                return;

            double.TryParse(StartMileage.Value(), out var start);
            double.TryParse(EndMileage.Value(), out var end);
            var total = end > start ? end - start : 0;
            TotalMileage.SetValue(total.ToString());
        }


        /// <summary>
        /// 耐久组
        /// </summary>
        /// <param name="department"></param>
        /// <param name="vin"></param>
        /// <param name="startDate"></param>
        /// <param name="startMileage"></param>
        /// <returns></returns>
        public static bool IsStartMileageRight(string department, string vin, DateTime startDate, double startMileage)
        {
            if (string.IsNullOrWhiteSpace(vin) || !department.Equals("耐久组"))
                return true;
            var strsql = $" select * from TaskTable where  carvin='{vin}' " + $" and ISNUMERIC(EndMileage)=1 and EndMileage>{startMileage} and TestStartDate<@startDate";

            var sql = new DataControl();

            var dt = sql.ExecuteQuery(strsql, new[] {
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


        /// <summary>
        /// 时间自动计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeSpanHandler(object sender, EventArgs e)
        {
            if (!(GetControlByFieldName("TestStartDate") is DateEdit StartTime) ||
                !(GetControlByFieldName("TestEndDate") is DateEdit EndTime) ||
                !(GetControlByFieldName("Testtime") is TitleCombox TimeSpan)) return;

            if (EndTime.Value() == null)
            {
                return;
            }
            var start = StartTime.Value().Replace("：", ":");
            var end = EndTime.Value().Replace("：", ":");

            if (!DateTime.TryParse(start, out var startTime) || !DateTime.TryParse(end, out var endTime))
                return;

            var span = endTime - startTime;
            TimeSpan.SetValue(span.TotalHours.ToString());

        }

        private void TaskEditDialog_Shown(object sender, EventArgs e)
        {
            Log.e("TaskEditDialog_Shown");

        }




        private DateTime string2DateTime(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return DateTime.MinValue;
            }

            return DateTime.Parse(content);
        }

        private double string2Double(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return -1;
            }

            return double.Parse(content);
        }

        private void initData()
        {
            this.buildOriTestStatisticEntity();
            this.testStatisticId = int.Parse(getValue("ID"));
            this.loadTestTaskSampleId();
            UseHolder.Instance.CurrentUser = FormSignIn.CurrentUser;
            this.carVins = CacheDataHandler.Instance.getCarVins();
            this.canisterVins = CacheDataHandler.Instance.getCanisterVins();
            this.oriSampleType = this.oriTestStatisticEntity.SampleType;
            this.sampleType = this.oriTestStatisticEntity.SampleType;
            if (sampleType.Equals(SampleTypeChn.整车.ToString()))
            {
                this.vins = this.carVins;
            }
            else if (sampleType.Equals(SampleTypeChn.碳罐.ToString()))
            {
                this.vins = this.canisterVins;
            }
            this.equipmentBreiefViewModels = CacheDataHandler.Instance.getCurUserEquipments();
            this.equipmentMap = new Dictionary<string, EquipmentBreiefViewModel>();
            this.equipmentBreiefViewModels.ForEach(item =>
            {
                if (!equipmentMap.ContainsKey(item.ToString()))
                {
                    equipmentMap.Add(item.ToString(), item);
                }
            });
            this.initUserStructureData();

            //查询实验任务的设备信息
            this.testEquipments = this.equipmentUsageRecordRepository.litesOfTestTask(this.testStatisticId);
            this.itemEquipments = this.testEquipments.Select(item => item.toEquipmentLite()).ToList();
            this.oriTestEquipmentCodes = this.itemEquipments.Select(item => item.Code).ToList();
            this.itemName = getValue("ItemBrief");
            if (!string.IsNullOrWhiteSpace(this.itemName))
            {
                string group = getValue("department");
                string locationNumber = getValue("LocationNumber");
                //构造主键
                this.itemGroupLocationKey = $"{this.itemName}&{group}&{locationNumber}";
                this.itemOriEquipmentsMap.Add(this.itemGroupLocationKey, this.itemEquipments.Select(item => item.copy()).ToList());

                //记住原始数据
                this.oriItemGroupLocationKey = $"{this.itemName}&{group}&{locationNumber}";
                this.oriItemEquipments = this.equipmentQueryService.equipmentsOfItem(this.itemName, group, locationNumber);
            }
        }

        private void loadTestTaskSampleId()
        {
            //TODO:并不完全严肃
            SampleOfVinViewModel curTaskSample = this.sampleQueryService.samplesOfVin(oriTestStatisticEntity.CarVin, oriTestStatisticEntity.SampleType);
            if (curTaskSample != null && curTaskSample.FromSampleTable != null)
            {
                this.testTaskSample = curTaskSample.FromSampleTable;
                this.testTaskSampleId = this.testTaskSample.Id;
            }
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
            //2024-01-25注释掉
            //Form1.ComboxDictionary["定位编号"].ForEach(item =>
            //{
            //    if (!this.locationNumbers.Contains(item))
            //    {
            //        this.locationNumbers.Add(item);
            //    }
            //});
        }

        private void initView()
        {
            this.Text = "编辑试验计划";
            this.btnAddEquipment.Enabled = !string.IsNullOrWhiteSpace(((TitleCombox)GetControlByFieldName("ItemBrief")).Text.Trim());
            this.initUsingEquipmentListView();
            this.initCombox();
            this.initViewValue();
        }

        private void initCombox()
        {
            this.titleComboxSampleType = ((TitleCombox)GetControlByFieldName("SampleType"));
            this.titleComboxVin = ((TitleCombox)GetControlByFieldName("Carvin"));
            this.titleComboxPriceDetail = ((TitleCombox)GetControlByFieldName("PriceDetail"));
            this.titleComboxProjectTotal = ((TitleCombox)GetControlByFieldName("ProjectTotal"));

            titleCombox3.SetItems(FormSignIn.UserDic.Keys);
            titleCombox1.SetItems(this.locationNumbers);
            this.titleComboxSampleType.SetItems(Form1.ComboxDictionary["样品类型"]);
            this.titleComboxVin.SetItems(this.vins);
            this.titleComboxPriceItem.SetItems(Form1.ComboxDictionary["费用名称"]);
            this.initEquipmentCombox();
            this.titleComboxSampleType.SetTextChange(SampleTypeChangeHandler);
            this.titleComboxVin.SetTextChange(VinChangeHandler);
            ((TitleCombox)GetControlByFieldName("ItemBrief")).SetTextChange(itemChangeHandler);
            ((TitleCombox)GetControlByFieldName("department")).SetTextChange(itemChangeHandler);
            ((TitleCombox)GetControlByFieldName("LocationNumber")).SetTextChange(itemChangeHandler);
            ((TitleCombox)GetControlByFieldName("SampleModel")).SetTextChange(sampleModelChangeHandler);
            ((TitleCombox)GetControlByFieldName("Taskcode")).SetTextChange(taskCodeChangeHandler);
            ((TitleCombox)GetControlByFieldName("Confidentiality")).SetTextChange(confidentialityChangeHandler);

            this.titleComboxVin.SetTextUpdate(VinTextUpdate);
            titleComboxEquip.SetTextUpdate(EquipmentTextUpdate);

            //样本类型只可选
            //this.titleComboxSampleType.SetNotAllowInput();
        }

        private void initViewValue()
        {
            //更新National信息
            string vin = ((TitleCombox)GetControlByFieldName("Carvin")).Text;
            this.updateNationalAfterVinChanged(vin);

            //更新任务单号信息
            sampleModelChangeEvent();

            //回显设备信息
            this.updateUsingEquipmentViewAfterInitLoad();

            //获取vin的样本信息
            this.getSampleOfVin(vin);
        }

        private void initUsingEquipmentListView()
        {
            this.setListViewUsingEquipmentRowHeight();
            btn.Visible = false;
            btn.Text = "删除";
            btn.Click += this.button_Click;
            this.listViewUsingEquipment.Controls.Add(btn);
        }

        private void updateUsingEquipmentViewAfterItemChanged()
        {
            btn.Visible = false;
            this.listViewUsingEquipment.Items.Clear();
            string group = ((TitleCombox)GetControlByFieldName("department")).Text.Trim();
            string locationNumber = ((TitleCombox)GetControlByFieldName("LocationNumber")).Text.Trim();
            if (string.IsNullOrEmpty(this.itemName) || string.IsNullOrWhiteSpace(group) || string.IsNullOrWhiteSpace(locationNumber))
            {
                this.btnAddEquipment.Enabled = false;
                return;
            }
            this.btnAddEquipment.Enabled = true;

            //构造主键
            this.itemGroupLocationKey = $"{this.itemName}&{group}&{locationNumber}";

            //之前已经加载过
            if (this.itemOriEquipmentsMap.ContainsKey(this.itemGroupLocationKey))
            {
                this.itemEquipments = this.itemOriEquipmentsMap[this.itemGroupLocationKey].Select(item => item.copy()).ToList();
            }
            else
            {
                this.itemEquipments = this.equipmentQueryService.equipmentsOfItem(this.itemName, group, locationNumber);
                this.itemOriEquipmentsMap.Add(this.itemGroupLocationKey, this.itemEquipments.Select(item => item.copy()).ToList());
            }


            //当前项目的设备code和设备的字典
            this.itemEquipmentMap = new Dictionary<string, EquipmentLite>();
            this.itemEquipments.ForEach(item => itemEquipmentMap.Add(item.Code, item));
            this.notifyEquipmentListViewChanged();
        }

        private void updateUsingEquipmentViewAfterInitLoad()
        {
            btn.Visible = false;
            this.listViewUsingEquipment.Items.Clear();
            if (Collections.isEmpty(this.itemEquipments))
            {
                this.itemEquipmentMap = new Dictionary<string, EquipmentLite>();
                return;
            }
            this.btnAddEquipment.Enabled = true;


            //当前项目的设备code和设备的字典
            this.itemEquipmentMap = new Dictionary<string, EquipmentLite>();
            this.itemEquipments.ForEach(item =>
            {
                if (!itemEquipmentMap.ContainsKey(item.Code))
                {
                    itemEquipmentMap.Add(item.Code, item);
                }
            });
            this.notifyEquipmentListViewChanged();
        }

        private void notifyEquipmentListViewChanged()
        {
            string testGroup = ((TitleCombox)GetControlByFieldName("department")).Text.Trim();
            UIHelp.Instance.notifyEquipmentListViewChanged(listViewUsingEquipment, btn,
                                                             itemEquipments, testGroup);
        }

        private void setListViewUsingEquipmentRowHeight()
        {
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 25);
            this.listViewUsingEquipment.SmallImageList = imgList;
        }

        private void addEquipment()
        {
            string newEquipmentValue = this.titleComboxEquip.Text;
            string testGroup = ((TitleCombox)GetControlByFieldName("department")).Text.Trim();
            UIHelp.Instance.addEquipment(listViewUsingEquipment, btn,
                itemEquipments, itemEquipmentMap, equipmentMap,
                newEquipmentValue, testGroup);
        }

        private void removeEquipment(string removedEquipmentCode)
        {
            UIHelp.Instance.removeEquipment(listViewUsingEquipment, btn, itemEquipments, itemEquipmentMap, removedEquipmentCode);
        }

        private void initEquipmentCombox()
        {
            List<Object> vms = this.equipmentBreiefViewModels.Select(item => (Object)item).ToList();
            titleComboxEquip.SetViewmModels(vms);
        }

        private void VinTextUpdate(object sender, EventArgs e)
        {
            try
            {
                int curSelectionStart = this.titleComboxVin.comboBox1.SelectionStart;
                this.titleComboxVin.comboBox1.Items.Clear();
                this.inTimeVins.Clear();
                foreach (var item in this.vins)
                {
                    if (item.Contains(this.titleComboxVin.comboBox1.Text))
                    {
                        this.inTimeVins.Add(item);
                    }
                }
                if (Collections.isEmpty(this.inTimeVins))
                {
                    this.inTimeVins.Add("无匹配数据");
                }
                this.titleComboxVin.comboBox1.Items.AddRange(this.inTimeVins.ToArray());
                this.titleComboxVin.comboBox1.SelectionStart = curSelectionStart;
                Cursor = Cursors.Default;
                this.titleComboxVin.comboBox1.DroppedDown = true;
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
        }

        private void EquipmentTextUpdate(object sender, EventArgs e)
        {
            try
            {
                int curSelectionStart = this.titleComboxEquip.comboBox1.SelectionStart;
                this.titleComboxEquip.comboBox1.Items.Clear();
                this.inTimeEquipments.Clear();
                foreach (var item in this.equipmentBreiefViewModels)
                {
                    if (item.ToString().Contains(this.titleComboxEquip.comboBox1.Text))
                    {
                        this.inTimeEquipments.Add(item.ToString());
                    }
                }
                if (Collections.isEmpty(this.inTimeEquipments))
                {
                    this.inTimeEquipments.Add("无匹配数据");
                }
                this.titleComboxEquip.comboBox1.Items.AddRange(this.inTimeEquipments.ToArray());
                this.titleComboxEquip.comboBox1.SelectionStart = curSelectionStart;
                Cursor = Cursors.Default;
                this.titleComboxEquip.comboBox1.DroppedDown = true;
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
        }

        private void SampleTypeChangeHandler(object sender, EventArgs e)
        {
            string sampleTypeValue = this.titleComboxSampleType.Text.Trim();
            if (this.sampleType.Equals(sampleTypeValue))
            {
                return;
            }
            this.sampleType = sampleTypeValue;
            this.afterSampleTypeChanged(this.sampleType);
        }

        private void VinChangeHandler(object sender, EventArgs e)
        {
            string vin = ((TitleCombox)GetControlByFieldName("Carvin")).Text;

            //更新nation信息
            this.updateNationalAfterVinChanged(vin);

            this.afterVinChanged(vin);
        }

        private void updateNationalAfterVinChanged(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
            {
                titleComboxNationalFive.SetItems(new List<string>());
                titleComboxNationalSix.SetItems(new List<string>());
                return;
            }

            string sql0 = $"select NationalFive,NationalSix from teststatistic where Carvin ='{vin}'";
            List<string> nationalFiveItems = new List<string>();
            List<string> nationalSixItems = new List<string>();
            foreach (DataRow row in SqlHelper.GetList(sql0).Rows)
            {
                string nationalFive = row[0].ToString();
                string nationalSix = row[1].ToString();
                if (nationalFive != ""&& !nationalFiveItems.Contains(nationalFive))
                {
                    nationalFiveItems.Add(nationalFive);
                }
                if (nationalSix != ""&& !nationalSixItems.Contains(nationalSix))
                {
                    nationalSixItems.Add(nationalSix);
                }
            }
            if (nationalFiveItems != null)
            {
                titleComboxNationalFive.SetItems(nationalFiveItems);
            }
            if (nationalSixItems != null)
            {
                titleComboxNationalSix.SetItems(nationalSixItems);
            }
        }

        private void updateTaskCodeAfterSampleModelChanged(string sampleModel)
        {
            //更新任务单号列表
            this.taskBriefs = this.taskRepository.selectBySampleModel(sampleModel);
            this.taskBriefMap = new Dictionary<string, TaskBrief>();
            List<string> taskCodes = new List<string>();
            this.taskBriefs.ForEach(item =>
            {
                if (!this.taskBriefMap.ContainsKey(item.TaskCode))
                {
                    this.taskBriefMap.Add(item.TaskCode, item);
                }
                taskCodes.Add(item.TaskCode);
            });
            titleComboxTaskCode.SetItems(taskCodes);

            //更新任务单号内容
            string taskCode = GetControlByFieldName("Taskcode").Value().Trim();
            if (string.IsNullOrWhiteSpace(taskCode))
            {
                return;
            }
            if (taskCode.Contains("?"))
            {
                string taskCodeHandled = taskCode.Replace("?", "");
                if (this.taskBriefMap.ContainsKey(taskCodeHandled))
                {
                    GetControlByFieldName("Taskcode").SetValue(taskCodeHandled);
                }
            }
            else
            {
                if (!this.taskBriefMap.ContainsKey(taskCode))
                {
                    GetControlByFieldName("Taskcode").SetValue(taskCode + "?");
                }
            }
        }

        private void afterVinChanged(string vin)
        {
            if (string.IsNullOrEmpty(vin))
            {
                notExistVinHnadler();
                return;
            }

            SampleBrief sample = this.getSampleOfVin(vin);
            if (sample == null)
            {
                notExistVinHnadler();
                return;
            }

            //更新ui
            //titleComboxSampleType.SetValue(sample.SampleType);
            this.setComboxValue("CarType", sample.CarType);
            this.setComboxValue("SampleModel", sample.CarModel);
            this.setComboxValue("Producer", sample.Producer);
            this.setComboxValue("PowerType", sample.PowerType);
            this.setComboxValue("EngineModel", sample.EngineModel);
            this.setComboxValue("EngineProduct", sample.EngineProducer);
            this.setComboxValue("YNDirect", sample.YNDirect);
            this.setComboxValue("TransmissionType", sample.TransType);
            this.setComboxValue("Drivertype", sample.DriverType);
            this.setComboxValue("FuelType", sample.FuelType);
            this.setComboxValue("FuelLabel", sample.Roz);
            this.setComboxValue("Tirepressure", sample.Tirepressure);

            this.setComboxValue("CanisterCode", sample.CanisterCode);
            this.setComboxValue("CanisterType", sample.CanisterType);
            this.setComboxValue("CanisterProductor", sample.CanisterProductor);
        }

        private void afterSampleTypeChanged(string sampleType)
        {
            //vin列表重新加载
            this.updateVinsAfterSampleTypeChanged(sampleType);

            //vin清空，样本信息清空
            this.resetAllSampleInfoValues();
        }

        private void updateVinsAfterSampleTypeChanged(string sampleType)
        {
            if (string.IsNullOrEmpty(sampleType))
            {
                this.vins = new List<string>();
            }
            //vin列表重新加载
            else if (sampleType.Equals("整车"))
            {
                this.vins = this.carVins;
            }
            else if (sampleType.Equals("碳罐"))
            {
                this.vins = this.canisterVins;
            }
            titleComboxVin.SetItems(this.vins);
        }

        private void notExistVinHnadler()
        {
            //resetSampleTitleComboxs();
            isVinFromStatistic = false;
        }

        private void resetSampleTitleComboxs()
        {
            this.setComboxValue("CarType", "");
            this.setComboxValue("SampleModel", "");
            this.setComboxValue("Producer", "");
            this.setComboxValue("PowerType", "");
            this.setComboxValue("EngineModel", "");
            this.setComboxValue("EngineProduct", "");
            this.setComboxValue("YNDirect", "");
            this.setComboxValue("TransmissionType", "");
            this.setComboxValue("Drivertype", "");
            this.setComboxValue("FuelType", "");
            this.setComboxValue("FuelLabel", "");
            this.setComboxValue("Tirepressure", "");

            //碳罐
            this.setComboxValue("CanisterCode", "");
            this.setComboxValue("CanisterType", "");
            this.setComboxValue("CanisterProductor", "");
        }

        private void resetAllSampleInfoValues()
        {
            titleComboxVin.SetValue("");
            this.resetSampleTitleComboxs();
        }

        private void updateEquipmentUsageRecordrRemark()
        {
            List<EquipmentUsageRecordLite> needUpdateRemarkRecords = new List<EquipmentUsageRecordLite>();

            //保密级别由A变为其他的
            if (StringUtils.isEquals(oriTestStatisticEntity.Confidentiality, "A")
                && !StringUtils.isEquals(testStatisticEntity.Confidentiality, "A"))
            {
                testEquipments.ForEach(item =>
                {
                    if (item.Remark.Contains(ConstHolder.SECURITY_LEVEL_A_REAMRK_TEXT))
                    {
                        item.Remark = item.Remark.Replace(ConstHolder.SECURITY_LEVEL_A_REAMRK_TEXT, "");
                        needUpdateRemarkRecords.Add(item);
                    }
                });
            } //保密级别由其他变为A
            else if (!StringUtils.isEquals(oriTestStatisticEntity.Confidentiality, "A")
              && StringUtils.isEquals(testStatisticEntity.Confidentiality, "A"))
            {
                testEquipments.ForEach(item =>
                {
                    if (!item.Remark.Contains(ConstHolder.SECURITY_LEVEL_A_REAMRK_TEXT))
                    {
                        item.Remark = $"{ConstHolder.SECURITY_LEVEL_A_REAMRK_TEXT}{item.Remark}";
                        needUpdateRemarkRecords.Add(item);
                    }
                });
            }

            if (Collections.isEmpty(needUpdateRemarkRecords))
            {
                return;
            }
            this.equipmentUsageRecordRepository.updateRemark(needUpdateRemarkRecords);
        }

        private SampleBrief getSampleOfVin(string vin)
        {
            if (string.IsNullOrEmpty(vin))
            {
                return null;
            }

            this.sampleType = titleComboxSampleType.Text.Trim();
            this.sampleOfVin = this.sampleQueryService.samplesOfVin(vin, this.sampleType);
            if (sampleOfVin == null)
            {
                return null;
            }
            SampleBrief sample = sampleOfVin.FromSampleTable;
            isVinFromStatistic = false;
            if (sample == null)
            {
                sample = sampleOfVin.FromStatisticTable;
                isVinFromStatistic = true;
            }

            return sample;
        }
        private void setComboxValue(string filedName, string value)
        {
            ((TitleCombox)GetControlByFieldName(filedName)).SetValue(value);
        }

        private void itemChangeHandler(object sender, EventArgs e)
        {
            itemChangeEvent();
        }

        private void sampleModelChangeHandler(object sender, EventArgs e)
        {
            sampleModelChangeEvent();
        }


        private void taskCodeChangeHandler(object sender, EventArgs e)
        {
            taskCodeChangeEvent();
        }

        private void confidentialityChangeHandler(object sender, EventArgs e)
        {

        }

        private void sampleModelChangeEvent()
        {
            this.sampleModel = ((TitleCombox)GetControlByFieldName("SampleModel")).Text.Trim();
            this.updateTaskCodeAfterSampleModelChanged(this.sampleModel);
        }

        private void taskCodeChangeEvent()
        {
            string taskCode = ((TitleCombox)GetControlByFieldName("Taskcode")).Text.Trim();
            if (this.taskBriefMap.ContainsKey(taskCode))
            {
                string securityLevel = this.taskBriefMap[taskCode].SecurityLevel;
                ((TitleCombox)GetControlByFieldName("Confidentiality")).SetValue(securityLevel);
            }
        }

        private void itemChangeEvent()
        {
            //第一次
            //if (this.isItemNameValueInitial)
            //{
            //    this.isItemNameValueInitial = false;
            //    if (Collections.isEmpty(this.itemEquipments))
            //    {
            //        this.itemName = ((TitleCombox)GetControlByFieldName("ItemBrief")).Text.Trim();
            //        this.updateUsingEquipmentViewAfterItemChanged();
            //    }
            //    return;
            //}

            this.itemName = ((TitleCombox)GetControlByFieldName("ItemBrief")).Text.Trim();
            this.updateUsingEquipmentViewAfterItemChanged();
        }

        private void button_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(this.listViewUsingEquipment.SelectedItems[0].SubItems[0].Text);
            if (this.listViewUsingEquipment.SelectedItems.Count == 0)
            {
                return;
            }

            DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string removedEquipmentCode = this.listViewUsingEquipment.SelectedItems[0].SubItems[1].Text.Trim();
                removeEquipment(removedEquipmentCode);
            }
            else
            {
                return;
            }
        }


        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void TestEditDialog_Resize(object sender, EventArgs e)
        {
            label4.Width = this.Width;
            label5.Width = this.Width;
            labelTestParam.Width = this.Width;
            label7.Width = this.Width;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAddEquipment_Click(object sender, EventArgs e)
        {
            addEquipment();
        }

        private void listViewUsingEquipment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listViewUsingEquipment.SelectedItems.Count > 0)
            {
                this.btn.Location = new Point(this.listViewUsingEquipment.SelectedItems[0].SubItems[5].Bounds.Left, this.listViewUsingEquipment.SelectedItems[0].SubItems[5].Bounds.Top);
                this.btn.Visible = true;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBoxItemPrice_Enter(object sender, EventArgs e)
        {
            if (textBoxItemPrice.Text.Trim() == INPUT_ITEM_PRICE_TIP_TEXT)
            {
                textBoxItemPrice.Text = "";
            }
        }

        private void textBoxItemPrice_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxItemPrice.Text))
            {
                textBoxItemPrice.Text = INPUT_ITEM_PRICE_TIP_TEXT;
            }
        }

        private void btnAddPriceDetails_Click(object sender, EventArgs e)
        {
            string priceItem = titleComboxPriceItem.Text.Trim();
            if (string.IsNullOrWhiteSpace(priceItem))
            {
                MessageBox.Show("费用明细名称为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            string price = textBoxItemPrice.Text.Trim();
            if (string.IsNullOrWhiteSpace(price) || StringUtils.Equals(price, INPUT_ITEM_PRICE_TIP_TEXT))
            {
                MessageBox.Show("费用为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            double priceValue = 0;
            if (!double.TryParse(price, out priceValue))
            {
                MessageBox.Show("您输入的费用格式有误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //设置费用明细
            string priceDetail = this.titleComboxPriceDetail.Text.Trim();
            string itemPriceText = $"{priceItem}{priceValue}元";
            if (priceDetail.Contains(itemPriceText))
            {
                MessageBox.Show("该费用明细已添加！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string splitChar = ((string.IsNullOrWhiteSpace(priceDetail))|| (priceDetail.ElementAt(priceDetail.Length-1).Equals('；'))) ? "" : "；";
            priceDetail = $"{priceDetail}{splitChar}{itemPriceText}";
            this.titleComboxPriceDetail.SetValue(priceDetail);

            //设置总价
            string totalPrice = this.titleComboxProjectTotal.Text.Trim();
            if (string.IsNullOrWhiteSpace(totalPrice))
            {
                totalPrice = "0";
            }
            double totalPriceValue = 0;
            if (!double.TryParse(totalPrice, out totalPriceValue))
            {
                MessageBox.Show("费用总计的格式有误，请您尽快纠正！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            totalPriceValue += priceValue;
            this.titleComboxProjectTotal.SetValue(totalPriceValue.ToString());

        }
    }
}
