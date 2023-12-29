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
    /// 用户信息实体
    /// </summary>
    public class UserEntity : BaseEntity
    {
        private static readonly string DATE_TABLE_NAME = "UserTable";

        private static readonly List<string> COLUMNS = new List<string> {
            "userID"
      ,"userName"
      ,"company"
      ,"section"
      ,"office"
      ,"department"
      ,"role"
    };

        
        /// <summary>
        /// Id
        /// </summary>      
        public int ID { get; set; }

        /// <summary>
        /// Id
        /// </summary>      
        public string UserID { get; set; }

        /// <summary>
        /// Id
        /// </summary>      
        public string UserName { get; set; }

        /// <summary>
        /// Id
        /// </summary>      
        public string Company { get; set; }

        /// <summary>
        /// Id
        /// </summary>      
        public string Section { get; set; }

        /// <summary>
        /// Id
        /// </summary>      
        public string Office { get; set; }

        /// <summary>
        /// Id
        /// </summary>      
        public string Department { get; set; }

        /// <summary>
        /// Id
        /// </summary>      
        public string Role { get; set; }


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
                   
          
         
                };

            return parameters;
        }      
    }
}
