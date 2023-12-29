using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.common.utils;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
using TaskManager.domain.service;
using TaskManager.domain.valueobject;

namespace TaskManager.infrastructure.db
{
    /// <summary>
    /// 样本实体仓库
    /// </summary>
    public class SampleRepository : ISampleRepository
    {
        /// <summary>
        /// 数据库供应者
        /// </summary>
        private DataControl dbProvider;

        private List<string> columns = new List<string> {
            "CreatePeople",
            "RegPeople",
            "UpdateDate",
            "SampleType",
            "CarType",
            "SampleProducter",
            "VehicleModel",
            "VIN",
            "VehicleQuality",
            "VehicleMaxQuality",
            "OptionalQuality",
            "DesignPeopleCount",
            "DriveFormParameter",
            "GearboxForm",
            "FetalPressureParameter",
            "FuelType",
            "FuelLabel",
            "PowerType",
            "EngineModel",
            "EngineProducter",
            "OilSupplyType",
            "DirectInjection",
            "NationalSixCoasting",
            "NationalFiveCoasting",
            "EmissionTimeLimit",
            "CarbonCanisterForm1",
            "CarbonCanisterForm2",
            "CarbonCanisterNum1",
            "CarbonCanisterNum2",
            "SampleCount",
            "ControlSystemType",
            "CarbonCanisterProductor1",
            "CarbonCanisterProductor2",
            "OBDProductor",
            "BOBForm",
            "Remark",
            "consistent" };

        private List<string> briefColumns = new List<string> {
            "RegPeople",
            "UpdateDate",
            "SampleType",
            "CarType",
            "SampleProducter",
            "VehicleModel",
            "VIN",
            "DriveFormParameter",
            "GearboxForm",
            "FuelType",
            "FuelLabel",
            "PowerType",
            "EngineModel",
            "EngineProducter",
            "DirectInjection",
             };

        public SampleRepository()
        {
            this.dbProvider = new DataControl();
        }


        /// <summary>
        /// 查询制定vin的样本简要信息
        /// </summary>
        /// <param name="vin">vin</param>
        /// <returns>样本简要信息</returns>
        public SampleBrief selectByVin(string vin)
        {
            SampleBrief result = null;

            string sql = $"select * from SampleTable where VIN=@vin";
            var dt = this.dbProvider.ExecuteQuery(sql, new[] {
                    new SqlParameter("vin",vin)
                }).Tables[0];

            if (dt.Rows.Count == 0)
            {
                return result;
            }
            result = this.dataRow2SampleBrief(dt.Rows[0]);

            return result;
        }

        /// <summary>
        /// 保存样本
        /// </summary>
        /// <param name="entity">样本实体</param>
        /// <returns>void</returns>
        public int save(SampleEntity entity)
        {
            string dbColumnsText = string.Join(",", this.columns);
            string dbColumnsValue = string.Join(",@", this.columns);
            dbColumnsValue = "@" + dbColumnsValue;
            string sqlText = $"insert into SampleTable({dbColumnsText}) values({dbColumnsValue})";
            SqlParameter[] parameters = entity.toAllSqlParameters();

            return dbProvider.ExecuteAddQuery(sqlText, parameters);
        }

        /// <summary>
        /// 更新样本
        /// </summary>
        /// <param name="sampleBrief">样本简要信息</param>
        /// <returns>void</returns>
        public void updateByBrief(SampleBrief sampleBrief)
        {
            string sqlText = this.buildUpdateByBriefSql();
            SqlParameter[] parameters = this.buildUpdateByBriefParameters(sampleBrief);
            dbProvider.ExecuteNonQuery(sqlText, parameters);
        }

        private string buildUpdateByBriefSql()
        {
            List<string> setValues = new List<string>();
            this.briefColumns.ForEach(item => setValues.Add($"{item}=@{item}"));
            string sqlSetText = string.Join(",", setValues);
            string sqlText = $"update SampleTable set {sqlSetText} where ID=@id";

            return sqlText;
        }

        private SqlParameter[] buildUpdateByBriefParameters(SampleBrief sampleBrief)
        {
            SqlParameter[] parameters = {
            new SqlParameter("RegPeople",DbHelper.ValueOrDBNullIfNull(UseHolder.Instance.CurrentUser.Name)),
            new SqlParameter("UpdateDate",DbHelper.ValueOrDBNullIfNull(DateTime.Now)),
            new SqlParameter("SampleType",DbHelper.ValueOrDBNullIfNull(sampleBrief.SampleType)),
            new SqlParameter("CarType",DbHelper.ValueOrDBNullIfNull(sampleBrief.CarType)),
            new SqlParameter("SampleProducter",DbHelper.ValueOrDBNullIfNull(sampleBrief.Producer)),
            new SqlParameter("VehicleModel",DbHelper.ValueOrDBNullIfNull(sampleBrief.CarModel)),
            new SqlParameter("VIN",DbHelper.ValueOrDBNullIfNull(sampleBrief.Vin)),
            new SqlParameter("DriveFormParameter",DbHelper.ValueOrDBNullIfNull(sampleBrief.DriverType)),
            new SqlParameter("GearboxForm",DbHelper.ValueOrDBNullIfNull(sampleBrief.TransType)),
            new SqlParameter("FuelType",DbHelper.ValueOrDBNullIfNull(sampleBrief.FuelType)),
            new SqlParameter("FuelLabel",DbHelper.ValueOrDBNullIfNull(sampleBrief.Roz)),
            new SqlParameter("PowerType",DbHelper.ValueOrDBNullIfNull(sampleBrief.PowerType)),
            new SqlParameter("EngineModel",DbHelper.ValueOrDBNullIfNull(sampleBrief.EngineModel)),
            new SqlParameter("EngineProducter",DbHelper.ValueOrDBNullIfNull(sampleBrief.EngineProducer)),
            new SqlParameter("DirectInjection",DbHelper.ValueOrDBNullIfNull(sampleBrief.YNDirect)),
              new SqlParameter("id",sampleBrief.Id)
            };

            return parameters;
        }

        private SampleBrief dataRow2SampleBrief(DataRow row)
        {
            SampleBrief sampleBrief = new SampleBrief();
            sampleBrief.Id =int.Parse(row["ID"].ToString().Trim());
            sampleBrief.Vin = row["VIN"].ToString().Trim();
            sampleBrief.SampleType = row["SampleType"].ToString().Trim();
            sampleBrief.CarType = row["CarType"].ToString().Trim();
            sampleBrief.CarModel = row["VehicleModel"].ToString().Trim();
            sampleBrief.Producer = row["SampleProducter"].ToString().Trim();
            sampleBrief.PowerType = row["PowerType"].ToString().Trim();
            sampleBrief.EngineModel = row["EngineModel"].ToString().Trim();
            sampleBrief.EngineProducer = row["EngineProducter"].ToString().Trim();
            sampleBrief.YNDirect = row["DirectInjection"].ToString().Trim();
            sampleBrief.TransType = row["GearboxForm"].ToString().Trim();
            sampleBrief.DriverType = row["DriveFormParameter"].ToString().Trim();
            sampleBrief.FuelType = row["FuelType"].ToString().Trim();
            sampleBrief.Roz = row["FuelLabel"].ToString().Trim();

            return sampleBrief;
        }
    }
}
