using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.common.utils;
using TaskManager.domain.entity;
using TaskManager.domain.valueobject;

namespace TaskManager.infrastructure.db
{
    public class DataTranslator
    {
        public static EquipmentUsageRecordTestPart dataRow2EquipmentUsageRecordTestPart(DataRow row)
        {
            EquipmentUsageRecordTestPart result = new EquipmentUsageRecordTestPart();

            result.TestTaskId = int.Parse(row["ID"].ToString().Trim());
            result.Department = DbHelper.dataColumn2String(row["department"]);
            result.LocationNumber = DbHelper.dataColumn2String(row["LocationNumber"]);
            result.Registrant = DbHelper.dataColumn2String(row["Registrant"]);
            result.ItemBrief = DbHelper.dataColumn2String(row["ItemBrief"]);
            result.TestStartDate = DbHelper.dataColumn2DateTime(row["TestStartDate"]);
            result.TestEndDate = DbHelper.dataColumn2DateTime(row["TestEndDate"]);
            result.SampleModel = DbHelper.dataColumn2String(row["SampleModel"]);
            result.Producer = DbHelper.dataColumn2String(row["Producer"]);
            result.CarVin = DbHelper.dataColumn2String(row["Carvin"]);
            result.TestState = DbHelper.dataColumn2String(row["Finishstate"]);
            result.SecurityLevel = DbHelper.dataColumn2String(row["Confidentiality"]);
            result.buildPurpose();

            return result;
        }

        public static TestStatisticIdInfo dataRow2TestStatisticIdInfo(DataRow row)
        {
            TestStatisticIdInfo result = new TestStatisticIdInfo();

            result.Id = int.Parse(row["ID"].ToString().Trim());
            result.VisualId = DbHelper.dataColumn2StringNoNull(row["question"]);
           
            return result;
        }

        public static TestStatisticEntity dataRow2LiteTestStatisticEntity(DataRow row)
        {
            TestStatisticEntity result = new TestStatisticEntity();

            result.Department = DbHelper.dataColumn2StringNoNull(row["department"]);
            result.ExperimentalSite = DbHelper.dataColumn2StringNoNull(row["ExperimentalSite"]);
            result.LocationNumber = DbHelper.dataColumn2StringNoNull(row["LocationNumber"]);
            result.Registrant = DbHelper.dataColumn2StringNoNull(row["Registrant"]);

            result.ItemType = DbHelper.dataColumn2StringNoNull(row["ItemType"]);
            result.ItemBrief = DbHelper.dataColumn2StringNoNull(row["ItemBrief"]);
            result.Confidentiality = DbHelper.dataColumn2StringNoNull(row["Confidentiality"]);

            result.SampleType = DbHelper.dataColumn2StringNoNull(row["SampleType"]);
            result.CarVin = DbHelper.dataColumn2StringNoNull(row["Carvin"]);
            result.SampleModel = DbHelper.dataColumn2StringNoNull(row["SampleModel"]);
            result.Producer = DbHelper.dataColumn2StringNoNull(row["Producer"]);
            result.CanisterCode = DbHelper.dataColumn2StringNoNull(row["CanisterCode"]);

            result.TestStartDate = DbHelper.dataColumn2DateTime(row["TestStartDate"]);
            result.TestEndDate = DbHelper.dataColumn2DateTime(row["TestEndDate"]);
            result.Finishstate = DbHelper.dataColumn2StringNoNull(row["Finishstate"]);

            //补充维护信息
            DateTime nowTime = DateTime.Now;
            result.CreateUser = result.Registrant;
            result.CreateTime = nowTime;
            result.UpdateTime = nowTime;

            return result;
        }
    }
}
