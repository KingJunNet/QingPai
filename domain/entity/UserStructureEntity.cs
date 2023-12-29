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
    /// 用户组织实体
    /// </summary>
    public class UserStructureEntity : BaseEntity
    {
        private static readonly string DATE_TABLE_NAME = "UserStructure";

        private static readonly List<string> COLUMNS = new List<string> {
            "Unit"
      ,"Section"
      ,"Office"
      ,"Experimentsite"
      ,"Group1"
      ,"Locationnumber"
      ,"Username"
    };

        
        /// <summary>
        /// Id
        /// </summary>      
        public int ID { get; set; }

        /// <summary>
        /// Id
        /// </summary>      
        public string Unit { get; set; }

        /// <summary>
        /// Id
        /// </summary>      
        public string Section { get; set; }

        /// <summary>
        /// Id
        /// </summary>      
        public string Office { get; set; }

        /// <summary>
        /// 实验地点
        /// </summary>      
        public string ExperimentSite { get; set; }

        /// <summary>
        /// 组
        /// </summary>      
        public string Group { get; set; }

        /// <summary>
        /// 定位编号
        /// </summary>      
        public string LocationNumber { get; set; }

        /// <summary>
        /// Id
        /// </summary>      
        public string UserName { get; set; }

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

        override
        public SqlParameter[] toAllSqlParameters()
        {
            SqlParameter[] parameters = new SqlParameter[]
              {
                   
            new SqlParameter("Unit",DbHelper.ValueOrDBNullIfNull(this.Unit))
         
                };

            return parameters;
        }      
    }
}
