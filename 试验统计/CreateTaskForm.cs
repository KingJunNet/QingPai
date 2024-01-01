using LabSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    public partial class CreateTaskForm : Form
    {

        private DataControl sql = new DataControl();
        public readonly string Server;
        private const string RootFolder = "轻排参数表服务器";
        private const string Category = "设备管理";

        private FormType type;

        private CreateTestTaskFrom from;

        private TestStatisticEntity baseTestStatistic;

        private ISampleQueryService sampleQueryService;
        private ISampleCommandService sampleCommandService;
        private IEquipmentQueryService equipmentQueryService;
        private IEquipmentCommandService equipmentCommandService;

        private ITestStatisticRepository testStatisticRepository;
        private IEquipmentUsageRecordRepository equipmentUsageRecordRepository;
        private IUserStructureRepository userStructureRepository;
        private ITaskRepository taskRepository;

        private List<string> vins;

        private List<UserStructureLite> userStructureLites;
        private List<string> groups;
        private List<string> experimentSites;
        private List<string> locationNumbers;

        private string itemName = "";

        private List<EquipmentBreiefViewModel> equipmentBreiefViewModels;
        private Dictionary<string, EquipmentBreiefViewModel> equipmentMap;

        private List<EquipmentLite> itemEquipments;

        private List<string> itemNowEquipmentCodes;

        private Dictionary<string, EquipmentLite> itemEquipmentMap;

        private Dictionary<string, List<EquipmentLite>> itemOriEquipmentsMap = new Dictionary<string, List<EquipmentLite>>();

        private bool isAddItemEquipments = false;

        private bool isNeedUpdateItemEquipments = false;

        private bool isUpdateItemEquipments = false;

        private TestStatisticEntity testStatisticEntity;

        private List<EquipmentUsageRecordEntity> equipmentUsageRecordEntities;

        public int testStatisticId;

        private SampleBrief updatedSampleBrief;

        private SampleOfVinViewModel sampleOfVin;

        private bool isVinFromStatistic = false;

        private bool isAddSample = false;

        private bool isNeedUpdateSample = false;

        private bool isUpdateSample = false;

        private Button btn = new Button();

        public bool isNeedRedirectToEditForm;

        private string sampleModel;

        private List<TaskBrief> taskBriefs;
        private Dictionary<string, TaskBrief> taskBriefMap=new Dictionary<string, TaskBrief>();

        public CreateTaskForm(CreateTestTaskFrom from)
        {
            InitializeComponent();
            this.from = from;
            this.sampleQueryService = new SampleQueryService();
            this.sampleCommandService = new SampleCommandService();
            this.testStatisticRepository = new TestStatisticRepository();
            this.equipmentQueryService = new EquipmentQueryService();
            this.equipmentCommandService = new EquipmentCommandService();
            this.equipmentUsageRecordRepository = new EquipmentUsageRecordRepository();
            this.userStructureRepository = new UserStructureRepository();
            this.taskRepository = new TaskRepository();
        }

        public void setBaseTestStatistic(TestStatisticEntity testStatistic) {
            this.baseTestStatistic = testStatistic;
        }

        private void initPage()
        {
            this.initData();
            this.initView();

            //Action action = () =>
            //{
            //    initView();
            //};
            //Invoke(action);
        }

        private void initView()
        {
            this.Text = "创建试验计划";
            this.initCombox();
            this.initViewValue();
            this.btnAddEquipment.Enabled = false;
            this.initUsingEquipmentListView();

            //回显基础信息
            this.reshowBaseTestStatisticInfo();
        }

        private void initData()
        {
            UseHolder.Instance.CurrentUser = FormSignIn.CurrentUser;
            this.vins = CacheDataHandler.Instance.getVins();
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
        }

        private void initUserStructureData() {
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

        private void reshowBaseTestStatisticInfo() {
            if (this.baseTestStatistic == null) {
                return;
            }
            string baseVin = this.baseTestStatistic.CarVin;
            string baseItemBrief = this.baseTestStatistic.ItemBrief;

            //回显样本信息
            this.titleComboxVin.SetValue(baseVin);
            this.afterVinChanged(baseVin);

            //回显项目信息
            this.titleComboxItemType.SetValue(this.baseTestStatistic.ItemType);
            this.titleComboxItemBrief.SetValue(this.baseTestStatistic.ItemBrief);

            //回显设备信息
            this.itemName = this.baseTestStatistic.ItemBrief;
            this.updateUsingEquipmentViewAfterItemChanged();

            this.titleComboxStandard.SetValue("");
            this.titleComboxItemRemark.SetValue("");

            //样本vin不可修改  
            this.titleComboxVin.SetReadOnly(true);
            this.titleComboxVin.OriginalReadOnly = true;
        }

        private void initCombox()
        {
            titleComboxGroup.SetItems(FormSignIn.UserDic.Keys);
            titleComboxArea.SetItems(this.experimentSites);
            titleComboxLocationNo.SetItems(this.locationNumbers);
            titleComboxUser.SetItems(FormSignIn.Users);
            //样本vin
            if (this.baseTestStatistic == null)
            {
                titleComboxVin.SetItems(this.vins);
            }
            else {
                titleComboxVin.SetItems(new List<string>());
            }            
            titleComboxSampleType.SetItems(Form1.ComboxDictionary["样品类型"]);
            titleComboxCarType.SetItems(Form1.ComboxDictionary["车辆类型"]);
            titleComboxCarModel.SetItems(new List<string>());
            titleComboxProducer.SetItems(Form1.ComboxDictionary["生产厂家"]);
            titleComboxEngineType.SetItems(Form1.ComboxDictionary["动力类型"]);
            titleComboxEngineModel.SetItems(new List<string>());
            titleComboxEngineProducer.SetItems(Form1.ComboxDictionary["发动机生产厂"]);
            titleComboxYNDirect.SetItems(Form1.ComboxDictionary["是否直喷"]);
            titleComboxTransType.SetItems(Form1.ComboxDictionary["变速器形式"]);
            titleComboxDriverType.SetItems(Form1.ComboxDictionary["驱动形式"]);
            titleComboxFuelType.SetItems(Form1.ComboxDictionary["燃料类型"]);
            titleComboxRoz.SetItems(Form1.ComboxDictionary["燃油标号"]);

            titleComboxItemType.SetItems(Form1.ComboxDictionary["项目类型"]);
            titleComboxItemBrief.SetItems(Form1.ComboxDictionary["项目简称"]);
            titleComboxStandard.SetItems(Form1.ComboxDictionary["标准阶段"]);
            titleComboxItemRemark.SetItems(new List<string>());

            this.initEquipmentCombox();

            titleComboxVin.SetTextChange(VinChangeHandler);
            titleComboxItemBrief.SetTextChange(itemChangeHandler);
            titleComboxCarModel.SetTextChange(sampleModelChangeHandler);
            titleComboxTaskCode.SetTextChange(taskCodeChangeHandler);
        }

        private void initUsingEquipmentListView()
        {
            this.setListViewUsingEquipmentRowHeight();
            btn.Visible = false;
            btn.Text = "删除";
            btn.Click += this.button_Click;
            this.listViewUsingEquipment.Controls.Add(btn);

            //ListViewItem item1 = new ListViewItem("NJ.18.8.1102");
            //item1.SubItems.Add("烟度计");
            //item1.SubItems.Add("MEXA-600S");         
            //this.listViewUsingEquipment.Items.Add(item1);

            //ListViewItem item2 = new ListViewItem("NJ.18.8.1103");
            //item2.SubItems.Add("便携式总辐射表");
            //item2.SubItems.Add("CMP21");         
            //this.listViewUsingEquipment.Items.Add(item2);

            //ImageList imgList = new ImageList();
            //imgList.ImageSize = new Size(1, 25);
            //listViewUsingEquipment.SmallImageList = imgList;


            //ListViewItem[] lvs = new ListViewItem[2];
            //lvs[0] = new ListViewItem(new string[] { "NJ.18.8.1102", "烟度计", "MEXA-600S", "" });
            //lvs[1] = new ListViewItem(new string[] { "NJ.18.8.1103", "便携式总辐射表", "CMP21", "" });
            //this.listViewUsingEquipment.Items.AddRange(lvs);

            //btn.Visible = true;
            //btn.Text = "删除";
            //btn.Click += this.button_Click;
            //this.listViewUsingEquipment.Controls.Add(btn);
            //this.btn.Size = new Size(this.listViewUsingEquipment.Items[0].SubItems[3].Bounds.Width,
            //this.listViewUsingEquipment.Items[0].SubItems[3].Bounds.Height);
        }

        private void updateUsingEquipmentViewAfterItemChanged()
        {
            btn.Visible = false;
            this.listViewUsingEquipment.Items.Clear();
            if (string.IsNullOrEmpty(this.itemName))
            {
                return;
            }
            this.btnAddEquipment.Enabled = true;

            //之前已经加载过
            if (this.itemOriEquipmentsMap.ContainsKey(this.itemName))
            {
                this.itemEquipments = this.itemOriEquipmentsMap[this.itemName];
            }
            else {
                this.itemEquipments = this.equipmentQueryService.equipmentsOfItem(this.itemName, this.titleComboxGroup.Text.Trim());
                this.itemOriEquipmentsMap.Add(this.itemName, this.itemEquipments.Select(item => item.copy() ).ToList());
            }

          
            //当前项目的设备code和设备的字典
            this.itemEquipmentMap = new Dictionary<string, EquipmentLite>();
            this.itemEquipments.ForEach(item => itemEquipmentMap.Add(item.Code, item));
            this.notifyEquipmentListViewChanged();
        }

        private void notifyEquipmentListViewChanged()
        {
            if (Collections.isEmpty(this.itemEquipments))
            {
                return;
            }
            this.setListViewUsingEquipmentRowHeight();

            //填充数据
            ListViewItem[] lvs = new ListViewItem[this.itemEquipments.Count];
            for (int index = 0; index < this.itemEquipments.Count; index++)
            {
                EquipmentLite curEquipment = this.itemEquipments[index];
                lvs[index] = new ListViewItem(new string[] { curEquipment.Code, curEquipment.Name, curEquipment.Type, curEquipment.Group, "" });
            }

            //更新ui
            this.listViewUsingEquipment.Items.Clear();
            this.listViewUsingEquipment.Items.AddRange(lvs);

            //设置删除事件
            this.setEquipmentListViewItemRemoveButton();
        }

        private void setListViewUsingEquipmentRowHeight() {
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 25);
            this.listViewUsingEquipment.SmallImageList = imgList;
        }

        private void setEquipmentListViewItemRemoveButton()
        {
            //删除按钮         
            this.btn.Size = new Size(this.listViewUsingEquipment.Items[0].SubItems[4].Bounds.Width,
            this.listViewUsingEquipment.Items[0].SubItems[4].Bounds.Height);
        }

        private void addEquipment()
        {
            string newEquipmentValue = this.titleComboxEquipment.Text;
            string newEquipmentCode = this.equipmentMap[newEquipmentValue].Code;
            if (this.itemEquipmentMap.ContainsKey(newEquipmentCode))
            {
                return;
            }
            EquipmentBreiefViewModel newEquipment = this.equipmentMap[newEquipmentValue];
            EquipmentLite addedEquipmentLite = newEquipment.toEquipmentLite();
            this.itemEquipments.Add(addedEquipmentLite);
            this.itemEquipmentMap.Add(addedEquipmentLite.Code, addedEquipmentLite);

            //填充数据
            ListViewItem lvItem = new ListViewItem(new string[] { addedEquipmentLite.Code, addedEquipmentLite.Name, addedEquipmentLite.Type, addedEquipmentLite.Group, "" });
            //更新ui
            this.listViewUsingEquipment.Items.Add(lvItem);
            if (this.listViewUsingEquipment.Items.Count == 1)
            {
                //设置删除事件
                this.setEquipmentListViewItemRemoveButton();
            }

        }

        private void removeEquipment(string removedEquipmentCode)
        {
            //数据同步
            int removedIndex = this.itemEquipments.FindIndex(item => item.Code.Equals(removedEquipmentCode));
            this.itemEquipments.RemoveAt(removedIndex);
            this.itemEquipmentMap.Remove(removedEquipmentCode);


            //更新ui
            this.listViewUsingEquipment.Items.RemoveAt(removedIndex);
            btn.Visible = false;
        }

        private void initEquipmentCombox()
        {
            List<Object> vms = this.equipmentBreiefViewModels.Select(item => (Object)item).ToList();
            titleComboxEquipment.SetViewmModels(vms);
        }

        private void VinChangeHandler(object sender, EventArgs e)
        {
            string vin = titleComboxVin.Text;
            this.afterVinChanged(vin);
        }

        private void afterVinChanged(string vin) {
            if (string.IsNullOrEmpty(vin))
            {
                return;
            }

            this.sampleOfVin = this.sampleQueryService.samplesOfVin(vin);
            if (sampleOfVin == null)
            {
                return;
            }
            SampleBrief sample = sampleOfVin.FromSampleTable;
            isVinFromStatistic = false;
            if (sample == null)
            {
                sample = sampleOfVin.FromStatisticTable;
                isVinFromStatistic = true;
            }
            //更新ui
            titleComboxSampleType.SetValue(sample.SampleType);
            titleComboxCarType.SetValue(sample.CarType);
            titleComboxCarModel.SetValue(sample.CarModel);
            titleComboxProducer.SetValue(sample.Producer);
            titleComboxEngineType.SetValue(sample.PowerType);
            titleComboxEngineModel.SetValue(sample.EngineModel);
            titleComboxEngineProducer.SetValue(sample.EngineProducer);
            titleComboxYNDirect.SetValue(sample.YNDirect);
            titleComboxTransType.SetValue(sample.TransType);
            titleComboxDriverType.SetValue(sample.DriverType);
            titleComboxFuelType.SetValue(sample.FuelType);
            titleComboxRoz.SetValue(sample.Roz);

            //
        }

        private void itemChangeHandler(object sender, EventArgs e)
        {
            this.itemName = this.titleComboxItemBrief.Text;
            this.updateUsingEquipmentViewAfterItemChanged();
        }

        private void sampleModelChangeHandler(object sender, EventArgs e)
        {
            sampleModelChangeEvent();
        }

        private void taskCodeChangeHandler(object sender, EventArgs e)
        {
            taskCodeChangeEvent();
        }

        private void taskCodeChangeEvent()
        {
            string taskCode = this.titleComboxTaskCode.Text.Trim();
            if (this.taskBriefMap.ContainsKey(taskCode))
            {
                string securityLevel = this.taskBriefMap[taskCode].SecurityLevel;
                this.titleComboxSecurityLevel.SetValue(securityLevel);
            }
        }

        private void sampleModelChangeEvent()
        {
            this.sampleModel =this.titleComboxCarModel.Text.Trim();
            this.updateTaskCodeAfterSampleModelChanged(this.sampleModel);
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
            string taskCode = titleComboxTaskCode.Value().Trim();
            if (string.IsNullOrWhiteSpace(taskCode))
            {
                return;
            }
            if (taskCode.Contains("?"))
            {
                string taskCodeHandled = taskCode.Replace("?", "");
                if (this.taskBriefMap.ContainsKey(taskCodeHandled))
                {
                    titleComboxTaskCode.SetValue(taskCodeHandled);
                }
            }
            else
            {
                if (!this.taskBriefMap.ContainsKey(taskCode))
                {
                    titleComboxTaskCode.SetValue(taskCode + "?");
                }
            }
        }

        private void initViewValue()
        {
            titleComboxGroup.SetValue(FormSignIn.CurrentUser.Department);
            titleComboxUser.SetValue(FormSignIn.CurrentUser.Name);
            titleComboxArea.SetValue(this.experimentSites[0]);
            titleComboxLocationNo.SetValue(this.locationNumbers[0]);
            string nowTime= DateTime.Now.ToString("yyyy/MM/dd HH:mm", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            titleComboxBeginTime.SetValue(nowTime);
        }

        private void CreateTaskForm_Load(object sender, EventArgs e)
        {
            this.initPage();
        }

        private void titleCombox23_Load(object sender, EventArgs e)
        {

        }

        private void titleCombox39_Load(object sender, EventArgs e)
        {

        }

        private void btnSaveItemTask_Click(object sender, EventArgs e)
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

                //构造实验用设备记录
                this.buildTestEquipmentRecords();

                //参数合法性验证
                string errorInfo = "";
                if (!this.validateTestStatisticParam(out errorInfo))
                {
                    MessageBox.Show($"参数有误：{errorInfo}", "错误信息", MessageBoxButtons.OK);
                    return;
                }

                //样本同步策略
                this.syncSampleStrategy();

                //项目设备同步策略
                this.syncItemEquipmentStrategy();
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建试验计划失败！", "错误信息", MessageBoxButtons.OK);
                return;
            }

            //是否需要更新样本
            if (this.isNeedUpdateSample)
            {
                DialogResult result = MessageBox.Show("检测到该VIN样本信息有变化，需要将该样本信息更新至样本数据表么", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.isUpdateSample = true;
                }
            }
            //是否需要更新样本
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
                DialogResult result = MessageBox.Show("创建试验计划成功", "提示", MessageBoxButtons.OK);
                if (result == DialogResult.OK)
                {
                    if (this.from.Equals(CreateTestTaskFrom.MAIN_FORM))
                    {

                    }
                    else if (this.from.Equals(CreateTestTaskFrom.TEST_STATISTIC_LIST_FORM)) {
                        DialogResult redirectToEditResult = MessageBox.Show("是否需要进入编辑界面以补充其他信息", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (redirectToEditResult == DialogResult.Yes)
                        {
                            this.isNeedRedirectToEditForm = true;
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        else {
                            this.isNeedRedirectToEditForm = false;
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建试验计划失败！", "错误信息", MessageBoxButtons.OK);
            }
        }

        private bool validateTaskCode()
        {
            string taskCode = titleComboxTaskCode.Value().Trim();
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
                    titleComboxTaskCode.SetValue(taskCode + "?");
                };
            }

            //验证任务单好和样本型号匹配
            string sampleModel = titleComboxCarModel.Value().Trim();
            //任务单号有疑问或者样本模型为空，不做进一步验证
            if (string.IsNullOrWhiteSpace(sampleModel))
            {
                return true;
            }

            if (!this.taskBriefMap.ContainsKey(taskCode))
            {

                if (MessageBox.Show("该任务单号与车型不一致，是否重新填写！", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    titleComboxTaskCode.SetValue(taskCode + "?");
                }
                else
                {
                    return false;
                };
            }

            return true;
        }

        private TestStatisticEntity extractDataFromUi()
        {
            string group = titleComboxGroup.Text;
            string area = titleComboxArea.Text;
            string locationNo = titleComboxLocationNo.Text;
            string uesr = titleComboxUser.Text;


            string vin = titleComboxVin.Text;
            string sampleType = titleComboxSampleType.Text;
            string carType = titleComboxCarType.Text;
            string carModel = titleComboxCarModel.Text;
            string producer = titleComboxProducer.Text;
            string engineType = titleComboxEngineType.Text;
            string engineModel = titleComboxEngineModel.Text;
            string engineProducer = titleComboxEngineProducer.Text;
            string ynDirect = titleComboxYNDirect.Text;
            string transType = titleComboxTransType.Text;
            string driverType = titleComboxDriverType.Text;
            string fuelType = titleComboxFuelType.Text;
            string roz = titleComboxRoz.Text;

            string itemType = titleComboxItemType.Text;
            string itemBrief = titleComboxItemBrief.Text;
            string standard = titleComboxStandard.Text;
            DateTime beginTime = titleComboxBeginTime.Date;
            string itemRemark = titleComboxItemRemark.Text;

            string taskCode = titleComboxTaskCode.Text;
            string taskCodeRemark = titleComboxTaskCodeRemark.Text;
            string securityLevel = titleComboxSecurityLevel.Text;

            DateTime nowTime = DateTime.Now;
            string registrationDate = nowTime.ToString("yyyy/MM/dd HH:mm", System.Globalization.DateTimeFormatInfo.InvariantInfo); ;
            string createUser = FormSignIn.CurrentUser.Name;

            this.testStatisticEntity = new TestStatisticEntity()
                .createdTask(group = group, area = area, locationNo = locationNo, uesr = uesr,
             sampleType = sampleType, vin = vin, carType = carType, carModel = carModel,
             producer = producer, engineType = engineType, engineModel = engineModel, engineProducer = engineProducer,
             ynDirect = ynDirect, transType = transType, driverType = driverType, fuelType = fuelType, roz = roz,
             itemType = itemType, itemBrief = itemBrief, standard = standard, beginTime = beginTime, itemRemark,
             taskCode,taskCodeRemark, securityLevel,
             registrationDate,this.itemEquipments,createUser,nowTime);

            return testStatisticEntity;
        }

        private void buildTestEquipmentRecords() {
            this.equipmentUsageRecordEntities = new List<EquipmentUsageRecordEntity>();

            this.itemEquipments.ForEach(equipment =>
            {
                EquipmentUsageRecordEntity entity = new EquipmentUsageRecordEntity()
                .defaultParam()
                .equipmentInfo(equipment)
                .fromTest(this.testStatisticEntity);

                this.equipmentUsageRecordEntities.Add(entity);
            });
        }

        private bool validateTestStatisticParam(out string errorInfo)
        {
            errorInfo = this.testStatisticEntity.validate();

            return string.IsNullOrWhiteSpace(errorInfo);
        }

        private void syncSampleStrategy()
        {
            //构造样本信息
            this.updatedSampleBrief = this.testStatisticEntity.sampleBriefInfo();
            this.updatedSampleBrief.SampleType = titleComboxSampleType.Text;
            if (this.sampleOfVin == null || this.sampleOfVin.FromSampleTable == null)
            {
                this.isAddSample = true;
                return;
            }
            this.updatedSampleBrief.Id = this.sampleOfVin.FromSampleTable.Id;
            this.isNeedUpdateSample = !this.sampleOfVin.FromSampleTable.equals(this.updatedSampleBrief);
        }

        private void syncItemEquipmentStrategy()
        {
            //新添加则直接保存
            this.itemNowEquipmentCodes = this.itemEquipments.Select(item => item.Code).ToList();
            List<string> itemOriEquipmentCodes = this.itemOriEquipmentsMap[this.itemName].Select(item=>item.Code).ToList();
            if (Collections.isEmpty(itemOriEquipmentCodes))
            {
                this.isAddItemEquipments = true;
                return;
            }
          
            this.isNeedUpdateItemEquipments = !Collections.equals(itemOriEquipmentCodes, itemNowEquipmentCodes);
        }

        private void executeSaveData()
        {
            //保存统计信息
            this.testStatisticId = this.testStatisticRepository.save(this.testStatisticEntity);
            this.testStatisticEntity.Id = this.testStatisticId;

            //保存设备使用记录
            this.equipmentUsageRecordEntities.ForEach(entity => entity.TestTaskId = testStatisticId);
            this.equipmentUsageRecordRepository.batchSave(this.equipmentUsageRecordEntities);

            //更新样本信息
            if (isAddSample)
            {
                this.sampleCommandService.createByBrief(this.updatedSampleBrief);
            }
            else if (isUpdateSample)
            {
                this.sampleCommandService.updateByBrief(this.updatedSampleBrief);
            }

            //更新项目设备信息
            if (isAddItemEquipments)
            {
                this.equipmentCommandService.createItemEquipments(this.testStatisticEntity.ItemBrief,
                    this.testStatisticEntity.Department, this.itemNowEquipmentCodes);
            }
            else if (isUpdateItemEquipments)
            {
                this.equipmentCommandService.updateItemEquipments(this.testStatisticEntity.ItemBrief,
                    this.testStatisticEntity.Department, this.itemNowEquipmentCodes);
            }
            this.itemOriEquipmentsMap[this.testStatisticEntity.ItemBrief] = this.itemEquipments;

        }

        private void button_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(this.listViewUsingEquipment.SelectedItems[0].SubItems[0].Text);
            if (this.listViewUsingEquipment.SelectedItems.Count == 0) {
                return;
            }

            DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string removedEquipmentCode = this.listViewUsingEquipment.SelectedItems[0].SubItems[0].Text.Trim();
                removeEquipment(removedEquipmentCode);
            }
            else {
                return;
            }
        }

        private void btnCancelItemTask_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定关闭么，关闭后您的输入的信息讲不被保存！", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {

            }
            else
            {
                Close();
            };


        }

        private void listViewUsingEquipment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listViewUsingEquipment.SelectedItems.Count > 0)
            {
                this.btn.Location = new Point(this.listViewUsingEquipment.SelectedItems[0].SubItems[4].Bounds.Left, this.listViewUsingEquipment.SelectedItems[0].SubItems[4].Bounds.Top);
                this.btn.Visible = true;
            }
        }

        private void btnAddEquipment_Click(object sender, EventArgs e)
        {
            addEquipment();
        }

        private void btnCancelItemTask_Click_1(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
