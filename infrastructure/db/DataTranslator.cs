using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.common.utils;
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
    }
}
