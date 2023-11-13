using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Id
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string ExperimentalSite { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string LocationNumber { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Registrant { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Taskcode { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Taskcode2 { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string ItemBrief { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public DateTime TestStartDate { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public DateTime TestEndDate { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string TestTime { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string SampleModel { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Producer { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string CarVin { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Confidentiality { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string YNDirect { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string PowerType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string TransmissionType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string EngineModel { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string EngineProduct { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string DriverType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string TirePressure { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string NationalFive { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string NationalSix { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string StartMileage { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string EndMileage { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string TotalMileage { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string FuelType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Oil { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string FuelLabel { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string StandardStage { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string YNcountry { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public double ProjectPrice { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public double ProjectTotal { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string PriceDetail { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Finishstate { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string QualificationStatus { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string LogisticsInformation { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string PhoneNum { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string MoneySure { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string RegistrationDate { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Question { get; set; }

        public TestStatisticEntity()
        {
        }

        public TestStatisticEntity createdTask(string group,
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

            string itemType,
            string itemBrief,
            string standard,
            DateTime beginTime,
            string itemRemark)
        {
            this.Department = group;
            this.ExperimentalSite = area;
            this.LocationNumber = locationNo;
            this.Registrant = uesr;
            this.CarVin = vin;
            this.CarType = CarType;
            this.SampleModel = carModel;
            this.Producer = producer;
            this.PowerType = engineType;
            this.EngineModel = engineModel;
            this.EngineModel = engineProducer;
            this.YNDirect = ynDirect;
            this.TransmissionType = transType;
            this.DriverType = driverType;
            this.FuelType = fuelType;
            this.FuelLabel = roz;
            this.ItemType = itemType;
            this.ItemBrief = itemBrief;
            this.StandardStage = standard;
            this.TestStartDate = beginTime;
            this.Remark = itemRemark;

            this.ProjectPrice = -1;
            this.ProjectTotal = -1;
        

            return this;
        }

        public SqlParameter[] toAllSqlParameters()
        {
            SqlParameter[] parameters = new SqlParameter[]
              {new SqlParameter("department",ValueOrDBNullIfNull(this.Department))
               ,new SqlParameter("ExperimentalSite",ValueOrDBNullIfNull(this.ExperimentalSite))
               ,new SqlParameter("LocationNumber",ValueOrDBNullIfNull(this.LocationNumber))
               ,new SqlParameter("Registrant",ValueOrDBNullIfNull(this.Registrant))
               ,new SqlParameter("Taskcode",ValueOrDBNullIfNull(this.Taskcode))
               ,new SqlParameter("Taskcode2",ValueOrDBNullIfNull(this.Taskcode2))
               ,new SqlParameter("CarType",ValueOrDBNullIfNull(this.CarType))
               ,new SqlParameter("ItemType",ValueOrDBNullIfNull(this.ItemType))
               ,new SqlParameter("ItemBrief",ValueOrDBNullIfNull(this.ItemBrief))
               ,new SqlParameter("TestStartDate",ValueOrDBNullIfNull(this.TestStartDate))
               ,new SqlParameter("TestEndDate",ValueOrDBNullIfNull(this.TestEndDate))
               ,new SqlParameter("Testtime",ValueOrDBNullIfNull(this.TestTime))
               ,new SqlParameter("SampleModel",ValueOrDBNullIfNull(this.SampleModel))
               ,new SqlParameter("Producer",ValueOrDBNullIfNull(this.Producer))
               ,new SqlParameter("Carvin",ValueOrDBNullIfNull(this.CarVin))
               ,new SqlParameter("Confidentiality",ValueOrDBNullIfNull(this.Confidentiality))
               ,new SqlParameter("YNDirect",ValueOrDBNullIfNull(this.YNDirect))
               ,new SqlParameter("PowerType",ValueOrDBNullIfNull(this.PowerType))
               ,new SqlParameter("TransmissionType",ValueOrDBNullIfNull(this.TransmissionType))
               ,new SqlParameter("EngineModel",ValueOrDBNullIfNull(this.EngineModel))
               ,new SqlParameter("EngineProduct",ValueOrDBNullIfNull(this.EngineProduct))
               ,new SqlParameter("Drivertype",ValueOrDBNullIfNull(this.DriverType))
               ,new SqlParameter("Tirepressure",ValueOrDBNullIfNull(this.TirePressure))
               ,new SqlParameter("NationalFive",ValueOrDBNullIfNull(this.NationalFive))
               ,new SqlParameter("NationalSix",ValueOrDBNullIfNull(this.NationalSix))
               ,new SqlParameter("StartMileage",ValueOrDBNullIfNull(this.StartMileage))
               ,new SqlParameter("EndMileage",ValueOrDBNullIfNull(this.EndMileage))
               ,new SqlParameter("TotalMileage",ValueOrDBNullIfNull(this.TotalMileage))
               ,new SqlParameter("FuelType",ValueOrDBNullIfNull(this.FuelType))
               ,new SqlParameter("Oil",ValueOrDBNullIfNull(this.Oil))
               ,new SqlParameter("FuelLabel",ValueOrDBNullIfNull(this.FuelLabel))
               ,new SqlParameter("StandardStage",ValueOrDBNullIfNull(this.StandardStage))
               ,new SqlParameter("YNcountry",ValueOrDBNullIfNull(this.YNcountry))
               ,new SqlParameter("ProjectPrice",ValueOrDBNullIfNull(this.ProjectPrice))
               ,new SqlParameter("ProjectTotal",ValueOrDBNullIfNull(this.ProjectTotal))
               ,new SqlParameter("PriceDetail",ValueOrDBNullIfNull(this.PriceDetail))
               ,new SqlParameter("Finishstate",ValueOrDBNullIfNull(this.Finishstate))
               ,new SqlParameter("QualificationStatus",ValueOrDBNullIfNull(this.QualificationStatus))
               ,new SqlParameter("LogisticsInformation",ValueOrDBNullIfNull(this.LogisticsInformation))
               ,new SqlParameter("Remark",ValueOrDBNullIfNull(this.Remark))
               ,new SqlParameter("Contacts",ValueOrDBNullIfNull(this.Contacts))
               ,new SqlParameter("phoneNum",ValueOrDBNullIfNull(this.PhoneNum))
               ,new SqlParameter("Email",ValueOrDBNullIfNull(this.Email))
               ,new SqlParameter("MoneySure",ValueOrDBNullIfNull(this.MoneySure))
               ,new SqlParameter("RegistrationDate",ValueOrDBNullIfNull(this.RegistrationDate))
               ,new SqlParameter("question",ValueOrDBNullIfNull(this.Question))
                };

            return parameters;
        }

        private object ValueOrDBNullIfNull(String value)
        {
            if (value==null) return DBNull.Value;
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

        public SampleBrief sampleBriefInfo() {
            SampleBrief sampleBrief = new SampleBrief();

            sampleBrief.Vin = this.CarVin;
            sampleBrief.SampleType = "";
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

            return sampleBrief;
        }

    }
}
