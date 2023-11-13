using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.domain.entity;
using TaskManager.domain.repository;

namespace TaskManager.infrastructure.db
{
    class TestStatisticDbRepository : ITestStatisticRepository
    {
        public TestStatisticDbRepository()
        {
            this.dbProvider = new DataControl();

        }

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
      ,"question" };

        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>void</returns>
        public void save(TestStatisticEntity entity)
        {
            string dbColumnsText = string.Join(",", this.columns);
            string dbColumnsValue = string.Join(",@", this.columns);
            dbColumnsValue = "@" + dbColumnsValue;
            string sqlText = $"insert into TestStatistic({dbColumnsText}) values({dbColumnsValue})";
            SqlParameter[] parameters = entity.toAllSqlParameters();
            dbProvider.ExecuteNonQuery(sqlText, parameters);
        }
    }
}
