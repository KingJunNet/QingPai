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
    /// 配置项实体
    /// </summary>
    public class ConfigItemEntity : BaseEntity
    {
        private static readonly string DATE_TABLE_NAME = "ConfigItemTable";

        private static readonly List<string> COLUMNS = new List<string> {
            "Name"
      ,"Value"
      ,"GroupName"
      ,"Moudle"
      ,"Registrant"
    };

        
        /// <summary>
        /// Id
        /// </summary>      
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 选项
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public string Moudle { get; set; }

        /// <summary>
        /// 登记人
        /// </summary>
        public string Registrant { get; set; }

        public ConfigItemEntity()
        {

        }

        public ConfigItemEntity(string name, string value, string groupName)
        {
            Name = name;
            Value = value;
            GroupName = groupName;
        }

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
                new SqlParameter("Name",DbHelper.ValueOrDBNullIfNull(this.Name)),
                new SqlParameter("Value",DbHelper.ValueOrDBNullIfNull(this.Value)),
                new SqlParameter("GroupName",DbHelper.ValueOrDBNullIfNull(this.GroupName)),
                new SqlParameter("Moudle",DbHelper.ValueOrDBNullIfNull(this.Moudle)),
                new SqlParameter("Registrant",DbHelper.ValueOrDBNullIfNull(this.Registrant)),
               };

            return parameters;
        }

        public bool isValid()
        {
            return !string.IsNullOrWhiteSpace(this.Name)
                 && !string.IsNullOrWhiteSpace(this.Value);
        }

        public bool isMatchGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(this.GroupName))
            {
                return true;
            }

            return this.GroupNames().Contains(groupName);
        }

        private List<string> GroupNames()
        {
            return this.GroupName.Split(',').ToList();
        }
    }
}
