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
    /// 任务实体
    /// </summary>
    public class TaskEntity : BaseEntity
    {
        private static readonly string DATE_TABLE_NAME = "NewTaskTable";

        private static readonly List<string> COLUMNS = new List<string> {
            "TypeBrief",
            "Taskcode",
            "Model",
            "SecurityLevel"
           };

        /// <summary>
        /// Id
        /// </summary>      
        public int Id { get; set; }

        /// <summary>
        /// 类型简要信息
        /// </summary>      
        public string TypeBrief { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>      
        public string TaskCode { get; set; }

        /// <summary>
        /// 样本型号
        /// </summary>      
        public string Model { get; set; }

        /// <summary>
        /// 保密级别
        /// </summary>      
        public string SecurityLevel { get; set; }

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
