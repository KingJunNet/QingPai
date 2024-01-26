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
    /// 项目设备关系实体
    /// </summary>
    public class ItemEquipmentEntity: BaseEntity
    {
        private static readonly string DATE_TABLE_NAME = "ItemEquipmentTable";

        private static readonly List<string> COLUMNS = new List<string> {
            "ItemName",
            "GroupName",
            "LocationNumber",
            "EquipmentCode",
            "CreateUser",
            "CreateTime"
           };

        public ItemEquipmentEntity() {

        }

        public ItemEquipmentEntity(string itemName, string group, string locationNumber,
            string equipmentCode, string createUser, DateTime createTime)
        {
            ItemName = itemName;
            Group = group;
            LocationNumber = locationNumber;
            EquipmentCode = equipmentCode;
            CreateUser = createUser;
            CreateTime = createTime;
        }

        /// <summary>
        /// 应用Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 定位编号
        /// </summary>
        public string LocationNumber { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 应用Id
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public DateTime CreateTime { get; set; }

        override
        protected  string dataTableName() {
            return DATE_TABLE_NAME;
        }

        override
        protected List<string> columns() {
            return COLUMNS;
        }

        override
        public SqlParameter[] toAllSqlParameters()
        {
            SqlParameter[] parameters = new SqlParameter[]
              {
                  new SqlParameter("ItemName",DbHelper.ValueOrDBNullIfNull(this.ItemName)),
                  new SqlParameter("GroupName",DbHelper.ValueOrDBNullIfNull(this.Group)),
                   new SqlParameter("LocationNumber",DbHelper.ValueOrDBNullIfNull(this.LocationNumber)),
                  new SqlParameter("EquipmentCode",DbHelper.ValueOrDBNullIfNull(this.EquipmentCode)),
                  new SqlParameter("CreateUser",DbHelper.ValueOrDBNullIfNull(this.CreateUser)),
                  new SqlParameter("CreateTime",DbHelper.ValueOrDBNullIfNull(this.CreateTime))
                };

            return parameters;
        }
    }
}
