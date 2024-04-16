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
    /// 设备使用记录实体
    /// </summary>
    public class EquipmentUsageRecordEntity : BaseEntity
    {
        private static readonly string DATE_TABLE_NAME = "EquipmentUsageRecordTable";

        private static readonly List<string> COLUMNS = new List<string> {
            "EquipmentCode",
            "EquipmentName",
            "EquipmentType",
            "UsePerson",
            "UseTime",
            "Purpose",
            "PreUseState",
            "UseState",
            "PostUseState",
            "PostUseProblem",
            "TestTaskId",
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
            "SecurityLevel",
             "Remark",

              "ExportTime",
              "CreateUser",
             "CreateTime",
             "UpdateTime"
    };

        public EquipmentUsageRecordEntity state(int id,string preUseState, string useState,
            string postUseState, string postUseProblem,string remark)
        {
            this.ID = id;
            PreUseState = preUseState;
            UseState = useState;
            PostUseState = postUseState;
            PostUseProblem = postUseProblem;
            this.Remark = remark;

            return this;
        }

        public EquipmentUsageRecordEntity equipmentInfo(EquipmentLite equipment)
        {
            EquipmentCode = equipment.Code;
            EquipmentName = equipment.Name;
            EquipmentType = equipment.Type;

            return this;
        }

        public EquipmentUsageRecordEntity defaultParam()
        {
            Purpose = "";
            PreUseState = "正常";
            UseState = "良好";
            PostUseState = "正常";
            PostUseProblem = "";
            Remark = "";

            return this;
        }

        public EquipmentUsageRecordEntity fromTest(TestStatisticEntity testStatisticEntity)
        {
            UsePerson = testStatisticEntity.Registrant;
            UseTime = testStatisticEntity.TestStartDate;

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
            if (StringUtils.isEquals(testStatisticEntity.Confidentiality, ConstHolder.SECURITY_LEVEL_A)) {
                Remark = ConstHolder.SECURITY_LEVEL_A_REAMRK_TEXT;
            }
            this.buildPurpose();

            //补充维护信息
            this.CreateUser = testStatisticEntity.CreateUser;
            this.CreateTime = testStatisticEntity.CreateTime;
            this.UpdateTime = testStatisticEntity.UpdateTime;

            return this;
        }

        public EquipmentUsageRecordEntity fixFromTestUpdated(TestStatisticEntity testStatisticEntity)
        {
            //补充维护信息
            this.CreateUser = testStatisticEntity.Registrant;
            this.CreateTime = testStatisticEntity.UpdateTime;
            this.UpdateTime = testStatisticEntity.UpdateTime;

            return this;
        }

        private void buildPurpose() {
            this.Purpose= $"{this.Producer}-{this.SampleModel}-{this.CarVin}-{this.ItemBrief}";
        }


        /// <summary>
        /// Id
        /// </summary>      
        public int ID { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipmentType { get; set; }

        /// <summary>
        /// 使用人
        /// </summary>
        public string UsePerson { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime UseTime { get; set; }

        /// <summary>
        /// 设备用途
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// 使用前状态
        /// </summary>
        public string PreUseState { get; set; }

        /// <summary>
        /// 使用状况
        /// </summary>
        public string UseState { get; set; }

        /// <summary>
        /// 使用后状态
        /// </summary>
        public string PostUseState { get; set; }

        /// <summary>
        /// 使用后问题
        /// </summary>
        public string PostUseProblem { get; set; }

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

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 导出时间
        /// </summary>
        public DateTime ExportTime { get; set; }

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


        override
        protected string dataTableName()
        {
            return DATE_TABLE_NAME;
        }

        override
        protected List<string> columns()
        {
            return COLUMNS;
        }

        public string ExportKey() {
            return ""; 
        }

        override
        public SqlParameter[] toAllSqlParameters()
        {
            SqlParameter[] parameters = new SqlParameter[]
              {
                   
            new SqlParameter("EquipmentCode",DbHelper.ValueOrDBNullIfNull(this.EquipmentCode)),
            new SqlParameter("EquipmentName",DbHelper.ValueOrDBNullIfNull(this.EquipmentName)),
            new SqlParameter("EquipmentType",DbHelper.ValueOrDBNullIfNull(this.EquipmentType)),
            new SqlParameter("UsePerson",DbHelper.ValueOrDBNullIfNull(this.UsePerson)),
            new SqlParameter("UseTime",DbHelper.ValueOrDBNullIfNull(this.UseTime)),
            new SqlParameter("Purpose",DbHelper.ValueOrDBNullIfNull(this.Purpose)),
            new SqlParameter("PreUseState",DbHelper.ValueOrDBNullIfNull(this.PreUseState)),
            new SqlParameter("UseState",DbHelper.ValueOrDBNullIfNull(this.UseState)),
            new SqlParameter("PostUseState",DbHelper.ValueOrDBNullIfNull(this.PostUseState)),
            new SqlParameter("PostUseProblem",DbHelper.ValueOrDBNullIfNull(this.PostUseProblem)),
            new SqlParameter("TestTaskId",DbHelper.ValueOrDBNullIfNull(this.TestTaskId)),
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
             new SqlParameter("SecurityLevel",DbHelper.ValueOrDBNullIfNull(this.SecurityLevel)),
             new SqlParameter("Remark", DbHelper.ValueOrDBNullIfNull(this.Remark)),

              new SqlParameter("ExportTime", DbHelper.ValueOrDBNullIfNull(this.ExportTime)),
              new SqlParameter("CreateUser", DbHelper.ValueOrDBNullIfNull(this.CreateUser)),
              new SqlParameter("CreateTime", DbHelper.ValueOrDBNullIfNull(this.CreateTime)),
              new SqlParameter("UpdateTime", DbHelper.ValueOrDBNullIfNull(this.UpdateTime)),
                };

            return parameters;
        }

        public string validateState()
        {
            if (string.IsNullOrWhiteSpace(this.PreUseState))
            {
                return "使用前状态为空";
            }
            if (string.IsNullOrWhiteSpace(this.UseState))
            {
                return "使用状况为空";
            }
            if (string.IsNullOrWhiteSpace(this.PostUseState))
            {
                return "使用后状态为空";
            }
         
         
            return "";
        }
    }
}
