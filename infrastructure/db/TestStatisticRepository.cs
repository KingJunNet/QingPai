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
    class TestStatisticRepository : ITestStatisticRepository
    {
        public TestStatisticRepository()
        {
            this.dbProvider = new DataControl();

        }

        /// <summary>
        /// 数据库供应者
        /// </summary>
        private DataControl dbProvider;

        private List<string> columns = new List<string> {  "department"
      ,"ExperimentalSite"
      ,"LocationNumber"
      ,"Registrant"
      ,"Taskcode"
      ,"Taskcode2"
      ,"CarType"
      ,"ItemType"
      ,"ItemBrief"
      ,"TestStartDate"
      ,"TestEndDate"
      ,"Testtime"
      ,"SampleModel"
      ,"Producer"
      ,"Carvin"
      ,"Confidentiality"
      ,"YNDirect"
      ,"PowerType"
      ,"TransmissionType"
      ,"EngineModel"
      ,"EngineProduct"
      ,"Drivertype"
      ,"Tirepressure"
      ,"NationalFive"
      ,"NationalSix"
      ,"StartMileage"
      ,"EndMileage"
      ,"TotalMileage"
      ,"FuelType"
      ,"Oil"
      ,"FuelLabel"
      ,"StandardStage"
      ,"YNcountry"
      ,"ProjectPrice"
      ,"ProjectTotal"
      ,"PriceDetail"
      ,"Finishstate"
      ,"QualificationStatus"
      ,"LogisticsInformation"
      ,"Remark"
      ,"Contacts"
      ,"phoneNum"
      ,"Email"
      ,"MoneySure"
      ,"RegistrationDate"
      ,"question"
      ,"Equipments"
        ,"CreateUser"
        ,"CreateTime"
        ,"UpdateTime"};

        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>void</returns>
        public int save(TestStatisticEntity entity)
        {
            if (entity.Id <= 0)
            {
                return this.add(entity);
            }
            else
            {
                return this.update(entity);
            }
        }

        private int add(TestStatisticEntity entity)
        {
            string dbColumnsText = string.Join(",", this.columns);
            string dbColumnsValue = string.Join(",@", this.columns);
            dbColumnsValue = "@" + dbColumnsValue;
            string sqlText = $"insert into TestStatistic({dbColumnsText}) values({dbColumnsValue})";
            SqlParameter[] parameters = entity.toAllSqlParameters();

            return dbProvider.ExecuteAddQuery(sqlText, parameters);
        }

        private int update(TestStatisticEntity entity)
        {
            List<string> valueConditions = this.buildUpdateValueConditions();
            string dbSetText = string.Join(",", valueConditions);
            string sqlText = $"UPDATE TestStatistic SET {dbSetText} where ID=@ID";
            SqlParameter[] parameters = entity.toAllSqlParameters();
            Array.Resize(ref parameters, parameters.Length + 1);
            parameters[parameters.Length - 1] = new SqlParameter("ID", DbHelper.ValueOrDBNullIfNull(entity.Id));
            dbProvider.ExecuteNonQuery(sqlText, parameters);

            return entity.Id;
        }

        private List<string> buildUpdateValueConditions()
        {
            List<string> results = new List<string>();

            this.columns.ForEach(value =>
            {
                string text = $"{value}=@{value}";
                results.Add(text);
            });

            return results;
        }

        /// <summary>
        /// 查询指定的vin的最新的样本简要信息
        /// </summary>
        /// <param name="vin">vin</param>
        /// <returns>样本简要信息</returns>
        public SampleBrief selectLatestSampleVin(string vin)
        {
            SampleBrief result = null;

            string sql = $"SELECT TOP 1 * FROM TestStatistic WHERE Carvin=@vin ORDER BY ID DESC ";
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
        /// 查询指定的id的试验部分属性
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns>设备使用记录测试任务属性集合</returns>
        public List<EquipmentUsageRecordTestPart> selectPartsByIds(List<int> ids) {
            List<EquipmentUsageRecordTestPart> results = new List<EquipmentUsageRecordTestPart>();

            string sql = $"SELECT ID,department,LocationNumber,Registrant,ItemBrief,TestStartDate,TestEndDate,SampleModel,Producer,Carvin,Finishstate,Confidentiality " +
                $" FROM TestStatistic " +
                $" WHERE ID in {DbHelper.buildInCondition(ids)}";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            var dt = this.dbProvider.ExecuteQuery(sql, sqlParameters).Tables[0];
            if (dt.Rows.Count == 0)
            {
                return results;
            }
            foreach (DataRow row in dt.Rows)
            {
                results.Add(DataTranslator.dataRow2EquipmentUsageRecordTestPart(row));
            }

            return results;
        }

        private SampleBrief dataRow2SampleBrief(DataRow row)
        {
            SampleBrief sampleBrief = new SampleBrief();
            sampleBrief.Vin = row["Carvin"].ToString().Trim();
            sampleBrief.SampleType = "整车";
            sampleBrief.CarType = row["CarType"].ToString().Trim();
            sampleBrief.CarModel = row["SampleModel"].ToString().Trim();
            sampleBrief.Producer = row["Producer"].ToString().Trim();
            sampleBrief.PowerType = row["PowerType"].ToString().Trim();
            sampleBrief.EngineModel = row["EngineModel"].ToString().Trim();
            sampleBrief.EngineProducer = row["EngineProduct"].ToString().Trim();
            sampleBrief.YNDirect = row["YNDirect"].ToString().Trim();
            sampleBrief.TransType = row["TransmissionType"].ToString().Trim();
            sampleBrief.DriverType = row["Drivertype"].ToString().Trim();
            sampleBrief.FuelType = row["FuelType"].ToString().Trim();
            sampleBrief.Roz = row["FuelLabel"].ToString().Trim();

            return sampleBrief;
        }

        
    }
}
