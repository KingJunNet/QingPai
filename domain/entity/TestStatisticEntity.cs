using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.common.utils;
using TaskManager.domain.valueobject;

namespace TaskManager.domain.entity
{
    /// <summary>
    /// 实验统计实体
    /// </summary>
    public class TestStatisticEntity
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 试验地点
        /// </summary>
        public string ExperimentalSite { get; set; }

        /// <summary>
        /// 定位编号
        /// </summary>
        public string LocationNumber { get; set; }

        /// <summary>
        /// 登记人
        /// </summary>
        public string Registrant { get; set; }



        /// <summary>
        /// 项目类型
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// 项目简称
        /// </summary>
        public string ItemBrief { get; set; }

        /// <summary>
        /// 任务单号
        /// </summary>
        public string Taskcode { get; set; }

        /// <summary>
        /// 任务单号备注
        /// </summary>
        public string Taskcode2 { get; set; }

        /// <summary>
        /// 标准阶段
        /// </summary>
        public string StandardStage { get; set; }

        /// <summary>
        /// 是否报国环
        /// </summary>
        public string YNcountry { get; set; }

        /// <summary>
        /// 保密级别
        /// </summary>
        public string Confidentiality { get; set; }

        /// <summary>
        /// 项目单价
        /// </summary>
        public double ProjectPrice { get; set; }



        /// <summary>
        /// 样本类型
        /// </summary>
        public string SampleType { get; set; }

        /// <summary>
        /// VIN
        /// </summary>
        public string CarVin { get; set; }

        /// <summary>
        /// 车辆类型
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 车辆型号
        /// </summary>
        public string SampleModel { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Producer { get; set; }

        /// <summary>
        /// 动力类型
        /// </summary>
        public string PowerType { get; set; }

        /// <summary>
        /// 发动机型号
        /// </summary>
        public string EngineModel { get; set; }

        /// <summary>
        /// 发动机厂家
        /// </summary>
        public string EngineProduct { get; set; }

        /// <summary>
        /// 是否直喷
        /// </summary>
        public string YNDirect { get; set; }

        /// <summary>
        /// 变速器形式
        /// </summary>
        public string TransmissionType { get; set; }

        /// <sumary>
        /// 驱动形式
        /// </summary>
        public string DriverType { get; set; }

        /// <summary>
        /// 燃油种类
        /// </summary>
        public string FuelType { get; set; }

        /// <summary>
        /// 燃油标号
        /// </summary>
        public string FuelLabel { get; set; }

        /// <summary>
        /// 胎压
        /// </summary>
        public string TirePressure { get; set; }

        /// <summary>
        /// 碳罐编号
        /// </summary>
        public string CanisterCode { get; set; }

        /// <summary>
        /// 碳罐型号
        /// </summary>
        public string CanisterType { get; set; }

        /// <summary>
        ///碳罐生产厂
        /// </summary>
        public string CanisterProductor { get; set; }


        /// <summary>
        /// 碳罐总重量
        /// </summary>
        public string CanisterTotalWeight { get; set; }

        /// <summary>
        /// 活性炭总重量
        /// </summary>
        public string ActivatedCarbonTotalWeight { get; set; }

        /// <summary>
        /// 活性炭容积实测值
        /// </summary>
        public string ActivatedCarbonVolumeActual { get; set; }

        /// <summary>
        /// 碳罐有效容积实测值
        /// </summary>
        public string CanisterEffectiveVolumeActual { get; set; }

        /// <summary>
        /// 碳罐有效容积公开值
        /// </summary>
        public string CanisterEffectiveVolumePublic { get; set; }

        /// <summary>
        /// 碳罐有效容积符合性判定
        /// </summary>
        public string CanisterEffectiveVolumeConformance { get; set; }

        /// <summary>
        ///  碳罐有效吸附量实测值
        /// </summary>
        public string CanisterEffectiveAdsorptionActual { get; set; }

        /// <summary>
        ///  碳罐有效吸附量公开值
        /// </summary>
        public string CanisterEffectiveAdsorptionPublic { get; set; }

        /// <summary>
        ///  碳罐有效吸附量符合性判定
        /// </summary>
        public string CanisterEffectiveAdsorptionConformance { get; set; }

        /// <summary>
        /// 碳罐初始工作能力实测值
        /// </summary>
        public string CanisterWorkingAbilityActual { get; set; }

        /// <summary>
        /// 碳罐初始工作能力公开值
        /// </summary>
        public string CanisterWorkingAbilityPublic { get; set; }

        /// <summary>
        /// 碳罐初始工作能力符合性判定
        /// </summary>
        public string CanisterWorkingAbilityConformance { get; set; }



        /// <summary>
        /// 试验开始时间
        /// </summary>
        public DateTime TestStartDate { get; set; }

        /// <summary>
        /// 试验结束时间
        /// </summary>
        public DateTime TestEndDate { get; set; }

        /// <summary>
        /// 试验时长
        /// </summary>
        public string TestTime { get; set; }

        /// <summary>
        /// 开始里程
        /// </summary>
        public string StartMileage { get; set; }

        /// <summary>
        /// 结束里程
        /// </summary>
        public string EndMileage { get; set; }

        /// <summary>
        /// 总里程
        /// </summary>
        public string TotalMileage { get; set; }

        /// <summary>
        /// 转股惯量
        /// </summary>
        public string NationalFive { get; set; }

        /// <summary>
        /// 转股阻力
        /// </summary>
        public string NationalSix { get; set; }

        /// <summary>
        /// 用油量
        /// </summary>
        public string Oil { get; set; }

        /// <summary>
        /// 数据合格状态
        /// </summary>
        public string QualificationStatus { get; set; }

        /// <summary>
        /// 完成状态
        /// </summary>
        public string Finishstate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///  物流信息
        /// </summary>
        public string LogisticsInformation { get; set; }

        /// <summary>
        /// 客户联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNum { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 费用总计
        /// </summary>
        public double ProjectTotal { get; set; }

        /// <summary>
        /// 费用明细
        /// </summary>
        public string PriceDetail { get; set; }

        /// <summary>
        /// 费用确认
        /// </summary>
        public string MoneySure { get; set; }


        /// <summary>
        /// 创建日期
        /// </summary>
        public string RegistrationDate { get; set; }

        /// <summary>
        /// 登记状态
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public string Equipments { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        public TestStatisticEntity()
        {
        }

        public TestStatisticEntity createdTask(
            string group,
            string area,
            string locationNo,
            string uesr,

            string sampleType,
            string vin,
            string carType,
            string carModel,
            string producer,
            string engineType,
            string engineModel,
            string engineProducer,
            string ynDirect,
            string transType,
            string driverType,
            string fuelType,
            string roz,
            string tirepressure,
            string canisterCode,
            string canisterType,
            string canisterProductor,

            string itemType,
            string itemBrief,
            string taskCode,
            string taskCodeRemark,
            string standard,
            string securityLevel,
            DateTime beginTime,
            string itemRemark,

            string registrationDate,
            List<EquipmentLite> equipments,
            string createUser,
            DateTime nowTime)
        {
            this.Department = group;
            this.ExperimentalSite = area;
            this.LocationNumber = locationNo;
            this.Registrant = uesr;

            this.ItemType = itemType;
            this.ItemBrief = itemBrief;
            this.Taskcode = taskCode;
            this.Taskcode2 = taskCodeRemark;
            this.StandardStage = standard;
            this.Confidentiality = securityLevel;
            this.ProjectPrice = -1;

            this.SampleType = sampleType;
            this.CarVin = vin;
            this.CarType = carType;
            this.SampleModel = carModel;
            this.Producer = producer;
            this.PowerType = engineType;
            this.EngineModel = engineModel;
            this.EngineProduct = engineProducer;
            this.YNDirect = ynDirect;
            this.TransmissionType = transType;
            this.DriverType = driverType;
            this.FuelType = fuelType;
            this.FuelLabel = roz;
            this.TirePressure = tirepressure;
            this.CanisterCode = canisterCode;
            this.CanisterType = canisterType;
            this.CanisterProductor = canisterProductor;

            this.TestStartDate = beginTime;
            this.Remark = itemRemark;
            this.Finishstate = "未完成";

            this.ProjectTotal = -1;
           
            this.RegistrationDate = registrationDate;
            this.Equipments = this.buildEquipmentsContent(equipments);
            this.CreateUser = createUser;
            this.CreateTime = nowTime;
            this.UpdateTime = nowTime;

            return this;
        }

        public TestStatisticEntity lite(int id, string sampleType, string vin, string itemType, string itemBrief)
        {
            this.Id = Id;
            this.SampleType = sampleType;
            this.CarVin = vin;
            this.ItemType = itemType;
            this.ItemBrief = itemBrief;

            return this;
        }

        public void setEquipments(List<EquipmentLite> equipments)
        {
            this.Equipments = this.buildEquipmentsContent(equipments);
        }

        private string buildEquipmentsContent(List<EquipmentLite> equipments)
        {
            string result = "";

            if (Collections.isEmpty(equipments))
            {
                return result;
            }
            equipments.ForEach(equipment =>
            {
                result = $"{result}{(string.IsNullOrWhiteSpace(result) ? "" : ",")}{equipment.Code}（{equipment.Name}）";
            });

            return result;
        }

        public SqlParameter[] toAllSqlParameters()
        {
            SqlParameter[] parameters = new SqlParameter[]
              {new SqlParameter("department",ValueOrDBNullIfNull(this.Department))
               ,new SqlParameter("ExperimentalSite",ValueOrDBNullIfNull(this.ExperimentalSite))
               ,new SqlParameter("LocationNumber",ValueOrDBNullIfNull(this.LocationNumber))
               ,new SqlParameter("Registrant",ValueOrDBNullIfNull(this.Registrant))

               ,new SqlParameter("ItemType",ValueOrDBNullIfNull(this.ItemType))
               ,new SqlParameter("ItemBrief",ValueOrDBNullIfNull(this.ItemBrief))
               ,new SqlParameter("Taskcode",ValueOrDBNullIfNull(this.Taskcode))
               ,new SqlParameter("Taskcode2",ValueOrDBNullIfNull(this.Taskcode2))
               ,new SqlParameter("StandardStage",ValueOrDBNullIfNull(this.StandardStage))
               ,new SqlParameter("YNcountry",ValueOrDBNullIfNull(this.YNcountry))
               ,new SqlParameter("Confidentiality",ValueOrDBNullIfNull(this.Confidentiality))
               ,new SqlParameter("ProjectPrice",ValueOrDBNullIfNull(this.ProjectPrice))

               ,new SqlParameter("SampleType",ValueOrDBNullIfNull(this.SampleType))
               ,new SqlParameter("Carvin",ValueOrDBNullIfNull(this.CarVin))
               ,new SqlParameter("CarType",ValueOrDBNullIfNull(this.CarType))
               ,new SqlParameter("SampleModel",ValueOrDBNullIfNull(this.SampleModel))
               ,new SqlParameter("Producer",ValueOrDBNullIfNull(this.Producer))
               ,new SqlParameter("PowerType",ValueOrDBNullIfNull(this.PowerType))
               ,new SqlParameter("EngineModel",ValueOrDBNullIfNull(this.EngineModel))
               ,new SqlParameter("EngineProduct",ValueOrDBNullIfNull(this.EngineProduct))
               ,new SqlParameter("YNDirect",ValueOrDBNullIfNull(this.YNDirect))
               ,new SqlParameter("TransmissionType",ValueOrDBNullIfNull(this.TransmissionType))
               ,new SqlParameter("Drivertype",ValueOrDBNullIfNull(this.DriverType))
               ,new SqlParameter("FuelType",ValueOrDBNullIfNull(this.FuelType))
               ,new SqlParameter("FuelLabel",ValueOrDBNullIfNull(this.FuelLabel))
               ,new SqlParameter("Tirepressure",ValueOrDBNullIfNull(this.TirePressure))
               ,new SqlParameter("CanisterCode",ValueOrDBNullIfNull(this.CanisterCode))
               ,new SqlParameter("CanisterType",ValueOrDBNullIfNull(this.CanisterType))
               ,new SqlParameter("CanisterProductor",ValueOrDBNullIfNull(this.CanisterProductor))

                ,new SqlParameter("CanisterTotalWeight",ValueOrDBNullIfNull(this.CanisterTotalWeight))
                ,new SqlParameter("ActivatedCarbonTotalWeight",ValueOrDBNullIfNull(this.ActivatedCarbonTotalWeight))
                ,new SqlParameter("ActivatedCarbonVolumeActual",ValueOrDBNullIfNull(this.ActivatedCarbonVolumeActual))
                ,new SqlParameter("CanisterEffectiveVolumeActual",ValueOrDBNullIfNull(this.CanisterEffectiveVolumeActual))
                ,new SqlParameter("CanisterEffectiveVolumePublic",ValueOrDBNullIfNull(this.CanisterEffectiveVolumePublic))
                ,new SqlParameter("CanisterEffectiveVolumeConformance",ValueOrDBNullIfNull(this.CanisterEffectiveVolumeConformance))
                ,new SqlParameter("CanisterEffectiveAdsorptionActual",ValueOrDBNullIfNull(this.CanisterEffectiveAdsorptionActual))
                ,new SqlParameter("CanisterEffectiveAdsorptionPublic",ValueOrDBNullIfNull(this.CanisterEffectiveAdsorptionPublic))
                ,new SqlParameter("CanisterEffectiveAdsorptionConformance",ValueOrDBNullIfNull(this.CanisterEffectiveAdsorptionConformance))
                ,new SqlParameter("CanisterWorkingAbilityActual",ValueOrDBNullIfNull(this.CanisterWorkingAbilityActual))
                ,new SqlParameter("CanisterWorkingAbilityPublic",ValueOrDBNullIfNull(this.CanisterWorkingAbilityPublic))
                ,new SqlParameter("CanisterWorkingAbilityConformance",ValueOrDBNullIfNull(this.CanisterWorkingAbilityConformance))

               ,new SqlParameter("TestStartDate",ValueOrDBNullIfNull(this.TestStartDate))
               ,new SqlParameter("TestEndDate",ValueOrDBNullIfNull(this.TestEndDate))
               ,new SqlParameter("Testtime",ValueOrDBNullIfNull(this.TestTime))
               ,new SqlParameter("StartMileage",ValueOrDBNullIfNull(this.StartMileage))
               ,new SqlParameter("EndMileage",ValueOrDBNullIfNull(this.EndMileage))
               ,new SqlParameter("TotalMileage",ValueOrDBNullIfNull(this.TotalMileage))
               ,new SqlParameter("NationalFive",ValueOrDBNullIfNull(this.NationalFive))
               ,new SqlParameter("NationalSix",ValueOrDBNullIfNull(this.NationalSix))
               ,new SqlParameter("Oil",ValueOrDBNullIfNull(this.Oil))
               ,new SqlParameter("QualificationStatus",ValueOrDBNullIfNull(this.QualificationStatus))
               ,new SqlParameter("Finishstate",ValueOrDBNullIfNull(this.Finishstate))
               ,new SqlParameter("Remark",ValueOrDBNullIfNull(this.Remark))

               ,new SqlParameter("LogisticsInformation",ValueOrDBNullIfNull(this.LogisticsInformation))
               ,new SqlParameter("Contacts",ValueOrDBNullIfNull(this.Contacts))
               ,new SqlParameter("phoneNum",ValueOrDBNullIfNull(this.PhoneNum))
               ,new SqlParameter("Email",ValueOrDBNullIfNull(this.Email))
               ,new SqlParameter("ProjectTotal",ValueOrDBNullIfNull(this.ProjectTotal))
               ,new SqlParameter("PriceDetail",ValueOrDBNullIfNull(this.PriceDetail))
               ,new SqlParameter("MoneySure",ValueOrDBNullIfNull(this.MoneySure))

               ,new SqlParameter("RegistrationDate",ValueOrDBNullIfNull(this.RegistrationDate))
               ,new SqlParameter("question",ValueOrDBNullIfNull(this.Question))
               ,new SqlParameter("Equipments",ValueOrDBNullIfNull(this.Equipments))
               ,new SqlParameter("CreateUser",ValueOrDBNullIfNull(this.CreateUser))
               ,new SqlParameter("CreateTime",ValueOrDBNullIfNull(this.CreateTime))
               ,new SqlParameter("UpdateTime",ValueOrDBNullIfNull(this.UpdateTime))
                };

            return parameters;
        }

        private object ValueOrDBNullIfNull(String value)
        {
            if (value == null) return DBNull.Value;
            return value;
        }

        private object ValueOrDBNullIfNull(double value)
        {
            if (value < 0) return DBNull.Value;
            return value;
        }

        private object ValueOrDBNullIfNull(DateTime value)
        {
            if (value.Year == 1) return DBNull.Value;
            return value;
        }

        public SampleBrief sampleBriefInfo()
        {
            SampleBrief sampleBrief = new SampleBrief();

            sampleBrief.SampleType = this.SampleType;
            sampleBrief.Vin = this.CarVin;
            sampleBrief.CarType = this.CarType;
            sampleBrief.CarModel = this.SampleModel;
            sampleBrief.Producer = this.Producer;
            sampleBrief.PowerType = this.PowerType;
            sampleBrief.EngineModel = this.EngineModel;
            sampleBrief.EngineProducer = this.EngineProduct;
            sampleBrief.YNDirect = this.YNDirect;
            sampleBrief.TransType = this.TransmissionType;
            sampleBrief.DriverType = this.DriverType;
            sampleBrief.FuelType = this.FuelType;
            sampleBrief.Roz = this.FuelLabel;
            sampleBrief.Tirepressure = this.TirePressure;

            //碳罐信息
            sampleBrief.CanisterCode =this.CanisterCode ;
            sampleBrief.CanisterType = this.CanisterType;
            sampleBrief.CanisterProductor = this.CanisterProductor;

            return sampleBrief;
        }

        public string validateBack()
        {
            if (string.IsNullOrWhiteSpace(this.Department))
            {
                return "组别为空";
            }
            if (string.IsNullOrWhiteSpace(this.ExperimentalSite))
            {
                return "实验地点为空";
            }
            if (string.IsNullOrWhiteSpace(this.LocationNumber))
            {
                return "定位编号为空";
            }
            if (string.IsNullOrWhiteSpace(this.Registrant))
            {
                return "登记人为空";
            }
            if (string.IsNullOrWhiteSpace(this.CarVin))
            {
                return "VIN为空";
            }
            if (string.IsNullOrWhiteSpace(this.ItemType))
            {
                return "项目类型为空";
            }
            if (string.IsNullOrWhiteSpace(this.ItemBrief))
            {
                return "项目简称为空";
            }
            if (this.TestStartDate.Year == 1)
            {
                return "实验开始时间为空";
            }

            return "";
        }

        public string validateCreated()
        {
            if (string.IsNullOrWhiteSpace(this.Department))
            {
                return "组别为空";
            }
            if (string.IsNullOrWhiteSpace(this.ExperimentalSite))
            {
                return "实验地点为空";
            }
            if (string.IsNullOrWhiteSpace(this.LocationNumber))
            {
                return "定位编号为空";
            }
            if (string.IsNullOrWhiteSpace(this.Registrant))
            {
                return "登记人为空";
            }
            if (string.IsNullOrWhiteSpace(this.SampleType))
            {
                return "样本类型为空";
            }
            if (string.IsNullOrWhiteSpace(this.CarVin))
            {
                return "VIN为空";
            }
            if (string.IsNullOrWhiteSpace(this.ItemType))
            {
                return "项目类型为空";
            }
            if (string.IsNullOrWhiteSpace(this.ItemBrief))
            {
                return "项目简称为空";
            }
            if (this.TestStartDate.Year == 1)
            {
                return "实验开始时间为空";
            }

            if (this.SampleType.Equals(SampleTypeChn.碳罐.ToString()))
            {
                if (string.IsNullOrWhiteSpace(this.CanisterCode))
                {
                    return "碳罐编号为空";
                }
            }

            return "";
        }
    }
}
