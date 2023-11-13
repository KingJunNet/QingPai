using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManager.application.Iservice;
using TaskManager.application.service;
using TaskManager.application.viewmodel;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
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

        private ISampleQueryService sampleQueryService;
        private ISampleCommandService sampleCommandService;

        private ITestStatisticRepository testStatisticRepository;

        private List<string> vins;

        private TestStatisticEntity testStatisticEntity;

        private SampleBrief updatedSampleBrief;

        private SampleOfVinViewModel sampleOfVin;

        private bool isVinFromStatistic = false;

        private bool isAddSample=false;

        private bool isNeedUpdateSample=false;

        private bool isUpdateSample=false;

        public CreateTaskForm()
        {
            InitializeComponent();
            this.sampleQueryService = new SampleQueryService();
            this.sampleCommandService = new SampleCommandService();
            this.testStatisticRepository = new TestStatisticDbRepository();

        }

        private void initPage()
        {
            this.initData();
            this.Text = "创建实验任务";
            this.initCombox();
            this.initParamValue();

        }

        private void initData()
        {
            this.vins = this.sampleQueryService.allSampleVins();
        }

        private void initCombox()
        {
            titleComboxGroup.SetItems(FormSignIn.UserDic.Keys);
            titleComboxArea.SetItems(Form1.ComboxDictionary["实验地点"]);
            titleComboxLocationNo.SetItems(Form1.ComboxDictionary["定位编号"]);
            titleComboxUser.SetItems(FormSignIn.Users);

            titleComboxVin.SetItems(this.vins);
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

            titleComboxVin.SetTextChange(VinChangeHandler);
        }

        private void VinChangeHandler(object sender, EventArgs e)
        {
            string vin = titleComboxVin.Text;
            this.sampleOfVin = this.sampleQueryService.SamplesOfVin(vin);
            if (sampleOfVin == null)
            {
                return;
            }
            SampleBrief sample = sampleOfVin.FromStatisticTable;
            isVinFromStatistic = true;
            if (sample == null)
            {
                sample = sampleOfVin.FromSampleTable;
                isVinFromStatistic = false;
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

        private void initParamValue()
        {
            titleComboxGroup.SetValue(FormSignIn.CurrentUser.Department);
            titleComboxUser.SetValue(FormSignIn.CurrentUser.Name);
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
            //提取统计数据
            this.extractDataFromUi();

            //参数合法性验证
            if (!this.validateTestStatisticParam())
            {
                MessageBox.Show("输入参数有误！", "错误信息", MessageBoxButtons.OK);
                return;
            }

            //样本同步策略
            this.syncSampleStrategy();
            
            //是否需要更新样本
            if (this.isNeedUpdateSample)
            {
                DialogResult result = MessageBox.Show("检测到该VIN样本信息有变化，需要将该样本信息更新至样本数据表么", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.isUpdateSample = true;
                }
            }

            //保存数据
            try
            {
                this.executeSaveData();
                MessageBox.Show("创建测试项目任务成功", "提示", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建测试项目任务失败！", "错误信息", MessageBoxButtons.OK);
            }
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
            this.testStatisticEntity = new TestStatisticEntity()
                .createdTask(group = group, area = area, locationNo = locationNo, uesr = uesr,
             sampleType = sampleType, vin = vin, carType = carType, carModel = carModel,
             producer = producer, engineType = engineType, engineModel = engineModel, engineProducer = engineProducer,
             ynDirect = ynDirect, transType = transType, driverType = driverType, fuelType = fuelType, roz = roz,
             itemType = itemType, itemBrief = itemBrief, standard = standard, beginTime = beginTime, itemRemark);

            return testStatisticEntity;
        }

        private bool validateTestStatisticParam()
        {
            return true;
        }

        private void syncSampleStrategy()
        {
            //构造样本信息
            this.updatedSampleBrief = this.testStatisticEntity.sampleBriefInfo();
            this.updatedSampleBrief.SampleType= titleComboxSampleType.Text;
            if (this.sampleOfVin==null||this.sampleOfVin.FromSampleTable == null)
            {
                this.isAddSample = true;
                return;
            }
            this.isNeedUpdateSample = !this.sampleOfVin.FromSampleTable.equals(this.updatedSampleBrief);
        }

        private void executeSaveData() {
            this.testStatisticRepository.save(this.testStatisticEntity);
            if (isAddSample)
            {
                this.sampleCommandService.createByBrief(this.updatedSampleBrief);
            }
            else if (isUpdateSample)
            {
                this.sampleCommandService.updateByBrief(this.updatedSampleBrief);
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
    }
}
