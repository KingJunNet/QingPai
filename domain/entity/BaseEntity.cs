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
    public abstract class BaseEntity
    {
        protected abstract string dataTableName();
        protected abstract List<string> columns();
        public abstract SqlParameter[] toAllSqlParameters();


    
        public string insertSql() {
            string dbColumnsText = string.Join(",", columns());
            string dbColumnsValue = string.Join(",@", columns());
            dbColumnsValue = "@" + dbColumnsValue;
            string sqlText = $"insert into {dataTableName()}({dbColumnsText}) values({dbColumnsValue})";

            return sqlText;
        }
    }
}
