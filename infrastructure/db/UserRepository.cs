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
    /// 用户信息实体仓库
    /// </summary>
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {

        public UserRepository() : base()
        {

        }

        /// <summary>
        /// 查询所有用户信息
        /// </summary>     
        /// <returns>用户信息集合</returns>
        public List<UserEntity> selectAll() {
            string sql = $"select *  from UserTable ";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            List<UserEntity> results = selectList<UserEntity>(sql, sqlParameters, (row) =>
                  dataRow2User(row)
            );

            return results;
        }

        private UserEntity dataRow2User(DataRow row)
        {
            UserEntity model = new UserEntity();
            model.ID = int.Parse(row["ID"].ToString().Trim());
            model.UserID= DbHelper.dataColumn2String(row["userID"]);
            model.UserName = DbHelper.dataColumn2String(row["userName"]);
            model.Company = DbHelper.dataColumn2String(row["company"]);
            model.Section = DbHelper.dataColumn2String(row["section"]);
            model.Office = DbHelper.dataColumn2String(row["office"]);
            model.Department = DbHelper.dataColumn2String(row["department"]);
            model.Role = DbHelper.dataColumn2String(row["role"]);

            return model;
        }
    }


}
