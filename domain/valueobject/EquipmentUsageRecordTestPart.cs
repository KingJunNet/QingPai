using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.common.utils;
using TaskManager.domain.entity;
using TaskManager.domain.valueobject;

namespace TaskManager.domain.valueobject
{
    /// <summary>
    /// 设备使用记录测试任务属性
    /// </summary>
    public class EquipmentUsageRecordTestPart
    {
        private List<string> columns = new List<string> {
             "UsePerson",
            "UseTime",
             "Purpose",
            "Department",
            "LocationNumber",
            "Registrant",
            "ItemBrief",
            "TestStartDate",
             "TestEndDate",
             "SampleModel",
            "Producer",
            "CarVin",
            "TestState",
            "SecurityLevel" };

        /// <summary>
        /// 设备用途
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// 实验任务Id
        /// </summary>      
        public int TestTaskId { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 定位编号
        /// </summary>
        public string LocationNumber { get; set; }

        /// <summary>
        /// 登记人
        /// </summary>
        public string Registrant { get; set; }

        /// <summary>
        /// 项目简称使用
        /// </summary>
        public string ItemBrief { get; set; }

        /// <summary>
        /// 实验开始时间
        /// </summary>
        public DateTime TestStartDate { get; set; }

        /// <summary>
        /// 实验结束时间
        /// </summary>
        public DateTime TestEndDate { get; set; }

        /// <summary>
        /// 样本型号
        /// </summary>
        public string SampleModel { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Producer { get; set; }

        /// <summary>
        /// VIN
        /// </summary>
        public string CarVin { get; set; }

        /// <summary>
        /// 实验状态
        /// </summary>
        public string TestState { get; set; }

        /// <summary>
        /// 保密级别
        /// </summary>
        public string SecurityLevel { get; set; }

        public List<string> buildUpdateValueConditions()
        {
            List<string> results = new List<string>();

            this.columns.ForEach(value =>
            {
                string text = $"{value}=@{value}";
                results.Add(text);
            });

            return results;
        }

        public EquipmentUsageRecordTestPart fromTest(TestStatisticEntity testStatisticEntity)
        {
            TestTaskId = testStatisticEntity.Id;
            Department = testStatisticEntity.Department;
            LocationNumber = testStatisticEntity.LocationNumber;
            Registrant = testStatisticEntity.Registrant;
            ItemBrief = testStatisticEntity.ItemBrief;
            TestStartDate = testStatisticEntity.TestStartDate;
            TestEndDate = testStatisticEntity.TestEndDate;
            SampleModel = testStatisticEntity.SampleModel;
            Producer = testStatisticEntity.Producer;
            CarVin = testStatisticEntity.CarVin;
            TestState = testStatisticEntity.Finishstate;
            SecurityLevel = testStatisticEntity.Confidentiality;

            this.buildPurpose();

            return this;
        }

        public void buildPurpose()
        {
            this.Purpose = $"{this.Producer}-{this.SampleModel}-{this.CarVin}-{this.ItemBrief}";
        }


        public SqlParameter[] toAllSqlParameters()
        {
            SqlParameter[] parameters = new SqlParameter[]
              {
                   new SqlParameter("UsePerson",DbHelper.ValueOrDBNullIfNull(this.Registrant)),
                    new SqlParameter("UseTime",DbHelper.ValueOrDBNullIfNull(this.TestStartDate)),
                      new SqlParameter("Purpose",DbHelper.ValueOrDBNullIfNull(this.Purpose)),

                   new SqlParameter("Department",DbHelper.ValueOrDBNullIfNull(this.Department)),
            new SqlParameter("LocationNumber",DbHelper.ValueOrDBNullIfNull(this.LocationNumber)),
            new SqlParameter("Registrant",DbHelper.ValueOrDBNullIfNull(this.Registrant)),
            new SqlParameter("ItemBrief",DbHelper.ValueOrDBNullIfNull(this.ItemBrief)),
            new SqlParameter("TestStartDate",DbHelper.ValueOrDBNullIfNull(this.TestStartDate)),
             new SqlParameter("TestEndDate",DbHelper.ValueOrDBNullIfNull(this.TestEndDate)),
             new SqlParameter("SampleModel",DbHelper.ValueOrDBNullIfNull(this.SampleModel)),
            new SqlParameter("Producer",DbHelper.ValueOrDBNullIfNull(this.Producer)),
            new SqlParameter("CarVin",DbHelper.ValueOrDBNullIfNull(this.CarVin)),
            new SqlParameter("TestState",DbHelper.ValueOrDBNullIfNull(this.TestState)),
            new SqlParameter("SecurityLevel",DbHelper.ValueOrDBNullIfNull(this.SecurityLevel))
                };

            return parameters;
        }

        public bool equals(EquipmentUsageRecordTestPart another)
        {
            bool result = StringUtils.isEquals(this.Department, another.Department) &&
            StringUtils.isEquals(this.LocationNumber, another.LocationNumber) &&
            StringUtils.isEquals(this.Registrant, another.Registrant) &&
            StringUtils.isEquals(this.ItemBrief, another.ItemBrief) &&
            StringUtils.isEquals(this.TestStartDate.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo), another.TestStartDate.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo)) &&
            StringUtils.isEquals(this.TestEndDate.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo), another.TestEndDate.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo)) &&
            StringUtils.isEquals(this.SampleModel, another.SampleModel) &&
            StringUtils.isEquals(this.Producer, another.Producer) &&
            StringUtils.isEquals(this.CarVin, another.CarVin) &&
            StringUtils.isEquals(this.TestState, another.TestState) &&
            StringUtils.isEquals(this.SecurityLevel, another.SecurityLevel);

            return result;
        }
    }
}
