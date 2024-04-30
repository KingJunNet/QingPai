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
    /// 样本实体
    /// </summary>
    public class SampleEntity
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string CreatePeople { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string RegPeople { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string SampleType { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string SampleProducter { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string VehicleModel { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string VIN { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string VehicleQuality { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string VehicleMaxQuality { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string OptionalQuality { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string DesignPeopleCount { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string DriveFormParameter { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string GearboxForm { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string FetalPressureParameter { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string FuelType { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string FuelLabel { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string PowerType { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string EngineModel { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string EngineProducter { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string OilSupplyType { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string DirectInjection { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string NationalSixCoasting { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string NationalFiveCoasting { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string EmissionTimeLimit { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string CarbonCanisterForm1 { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string CarbonCanisterForm2 { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string CarbonCanisterNum1 { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string CarbonCanisterNum2 { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string SampleCount { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string ControlSystemType { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string CarbonCanisterProductor1 { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string CarbonCanisterProductor2 { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string OBDProductor { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string BOBForm { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string Consistent { get; set; }

        public SqlParameter[] toAllSqlParameters()
        {
            SqlParameter[] parameters = new SqlParameter[]
              {
                  new SqlParameter("CreatePeople",DbHelper.ValueOrDBNullIfNull(this.CreatePeople)),
            new SqlParameter("RegPeople",DbHelper.ValueOrDBNullIfNull(this.RegPeople)),
            new SqlParameter("UpdateDate",DbHelper.ValueOrDBNullIfNull(this.UpdateDate)),
            new SqlParameter("SampleType",DbHelper.ValueOrDBNullIfNull(this.SampleType)),
            new SqlParameter("CarType",DbHelper.ValueOrDBNullIfNull(this.CarType)),
            new SqlParameter("SampleProducter",DbHelper.ValueOrDBNullIfNull(this.SampleProducter)),
            new SqlParameter("VehicleModel",DbHelper.ValueOrDBNullIfNull(this.VehicleModel)),
            new SqlParameter("VIN",DbHelper.ValueOrDBNullIfNull(this.VIN)),
            new SqlParameter("VehicleQuality",DbHelper.ValueOrDBNullIfNull(this.VehicleQuality)),
            new SqlParameter("VehicleMaxQuality",DbHelper.ValueOrDBNullIfNull(this.VehicleMaxQuality)),
            new SqlParameter("OptionalQuality",DbHelper.ValueOrDBNullIfNull(this.OptionalQuality)),
            new SqlParameter("DesignPeopleCount",DbHelper.ValueOrDBNullIfNull(this.DesignPeopleCount)),
            new SqlParameter("DriveFormParameter",DbHelper.ValueOrDBNullIfNull(this.DriveFormParameter)),
            new SqlParameter("GearboxForm",DbHelper.ValueOrDBNullIfNull(this.GearboxForm)),
           new SqlParameter( "FetalPressureParameter",DbHelper.ValueOrDBNullIfNull(this.FetalPressureParameter)),
            new SqlParameter("FuelType",DbHelper.ValueOrDBNullIfNull(this.FuelType)),
            new SqlParameter("FuelLabel",DbHelper.ValueOrDBNullIfNull(this.FuelLabel)),
            new SqlParameter("PowerType",DbHelper.ValueOrDBNullIfNull(this.PowerType)),
            new SqlParameter("EngineModel",DbHelper.ValueOrDBNullIfNull(this.EngineModel)),
            new SqlParameter("EngineProducter",DbHelper.ValueOrDBNullIfNull(this.EngineProducter)),
            new SqlParameter("OilSupplyType",DbHelper.ValueOrDBNullIfNull(this.OilSupplyType)),
            new SqlParameter("DirectInjection",DbHelper.ValueOrDBNullIfNull(this.DirectInjection)),
            new SqlParameter("NationalSixCoasting",DbHelper.ValueOrDBNullIfNull(this.NationalSixCoasting)),
            new SqlParameter("NationalFiveCoasting",DbHelper.ValueOrDBNullIfNull(this.NationalFiveCoasting)),
            new SqlParameter("EmissionTimeLimit",DbHelper.ValueOrDBNullIfNull(this.EmissionTimeLimit)),
            new SqlParameter("CarbonCanisterForm1",DbHelper.ValueOrDBNullIfNull(this.CarbonCanisterForm1)),
            new SqlParameter("CarbonCanisterForm2",DbHelper.ValueOrDBNullIfNull(this.CarbonCanisterForm2)),
            new SqlParameter("CarbonCanisterNum1",DbHelper.ValueOrDBNullIfNull(this.CarbonCanisterNum1)),
            new SqlParameter("CarbonCanisterNum2",DbHelper.ValueOrDBNullIfNull(this.CarbonCanisterNum2)),
            new SqlParameter("SampleCount",DbHelper.ValueOrDBNullIfNull(this.SampleCount)),
            new SqlParameter("ControlSystemType",DbHelper.ValueOrDBNullIfNull(this.ControlSystemType)),
            new SqlParameter("CarbonCanisterProductor1",DbHelper.ValueOrDBNullIfNull(this.CarbonCanisterProductor1)),
            new SqlParameter("CarbonCanisterProductor2",DbHelper.ValueOrDBNullIfNull(this.CarbonCanisterProductor2)),
            new SqlParameter("OBDProductor",DbHelper.ValueOrDBNullIfNull(this.OBDProductor)),
            new SqlParameter("BOBForm",DbHelper.ValueOrDBNullIfNull(this.BOBForm)),
            new SqlParameter("Remark",DbHelper.ValueOrDBNullIfNull(this.Remark)),
            new SqlParameter("consistent",DbHelper.ValueOrDBNullIfNull(this.Consistent)),
            };

            return parameters;
        }

        public SampleEntity init(String author, DateTime time)
        {
            this.CreatePeople = author;
            this.RegPeople = author;
            this.UpdateDate = time;

            return this;
        }

        public SampleEntity fromBrief(SampleBrief brief)
        {
            this.VIN = brief.Vin;
            this.SampleType = brief.SampleType;
            this.CarType = brief.CarType;
            this.VehicleModel = brief.CarModel;
            this.SampleProducter = brief.Producer;
            this.PowerType = brief.PowerType;
            this.EngineModel = brief.EngineModel;
            this.EngineProducter = brief.EngineProducer;
            this.DirectInjection = brief.YNDirect;
            this.GearboxForm = brief.TransType;
            this.DriveFormParameter = brief.DriverType;
            this.FuelType = brief.FuelType;
            this.FuelLabel = brief.Roz;
            this.FetalPressureParameter = brief.Tirepressure;


            return this;
        }

    }
}
