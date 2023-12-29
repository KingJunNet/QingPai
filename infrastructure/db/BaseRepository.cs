using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.common.utils;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
using TaskManager.domain.service;
using TaskManager.domain.valueobject;

namespace TaskManager.infrastructure.db
{
    /// <summary>
    /// 样本实体仓库
    /// </summary>
    public class BaseRepository<T> where T:BaseEntity
    {
        public delegate U dataRow2Model<U>(DataRow row);

        /// <summary>
        /// 数据库供应者
        /// </summary>
        protected DataControl dbProvider;

        public BaseRepository() {
            this.dbProvider = new DataControl();
        }


        /// <summary>
        /// 保存样本
        /// </summary>
        /// <param name="entity">样本实体</param>
        /// <returns>void</returns>
        public void save(T entity)
        {
            string sqlText = entity.insertSql();
            SqlParameter[] parameters = entity.toAllSqlParameters();
            dbProvider.ExecuteNonQuery(sqlText, parameters);
        }

        /// <summary>
        /// 执行写操作
        /// </summary>
        /// <param name="sqlText">sql语句</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>void</returns>
        public void executeWrite(string sqlText, SqlParameter[] parameters)
        {
            dbProvider.ExecuteNonQuery(sqlText, parameters);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="sqlText">sql语句</param>
        /// <param name="parameters">参数集合</param>
        /// <param name="dataRow2ModelFunc">数据行转换为模型方法</param>
        /// <returns>数据集合</returns>
        public List<U> selectList<U>(string sql, SqlParameter[] sqlParameters, dataRow2Model<U> dataRow2ModelFunc)
        {
            List<U> results = new List<U>();

            var dt = this.dbProvider.ExecuteQuery(sql, sqlParameters).Tables[0];
            if (dt.Rows.Count == 0)
            {
                return results;
            }
            foreach (DataRow row in dt.Rows)
            {
                U data = dataRow2ModelFunc(row);
                results.Add(data);
            }

            return results;
        }
    }
}
