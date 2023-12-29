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
    /// 设备实体
    /// </summary>
    public class EquipmentEntity : BaseEntity
    {
        private static readonly string DATE_TABLE_NAME = "NewEquipmentTable";

        private static readonly List<string> COLUMNS = new List<string> {
            "ItemName",
            "Group",
            "EquipmentCode",
            "CreateUser",
            "CreateTime"
           };

        /// <summary>
        /// Id
        /// </summary>      
        public int Id { get; set; }

        /// <summary>
        /// 组
        /// </summary>
        public String Group { get; set; }

        /// <summary>
        /// 定位编号
        /// </summary>
        public String LocationNo { get; set; }

        /// <summary>
        /// 登记人
        /// </summary>
        public String RegUser { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public String State { get; set; }

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
                
                };

            return parameters;
        }
    }
}
