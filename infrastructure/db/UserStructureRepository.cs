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
    /// 用户组织实体仓库
    /// </summary>
    public class UserStructureRepository : BaseRepository<UserStructureEntity>, IUserStructureRepository
    {

        public UserStructureRepository() : base()
        {

        }

        /// <summary>
        /// 查询指定用户的>用户组织简要信息
        /// </summary>     
        /// <param name="userName">用户名称</param>
        /// <returns>用户组织简要信息集合</returns>
        public List<UserStructureLite> selectByUser(string userName)
        {
            string sql = $"select Experimentsite,Group1,Locationnumber,Username  " +
                $"from UserStructure " +
                $"WHERE Username like  '%{userName}%'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            List<UserStructureLite> results = selectList<UserStructureLite>(sql, sqlParameters, (row) =>
                  dataRow2UserStructureLite(row)
            );

            return results;
        }

        private UserStructureLite dataRow2UserStructureLite(DataRow row)
        {
            UserStructureLite model = new UserStructureLite();
            model.ExperimentSite = row["Experimentsite"].ToString().Trim();
            model.Group = row["Group1"].ToString().Trim();
            model.LocationNumber = row["Locationnumber"].ToString().Trim();
            model.UserName = row["Username"].ToString().Trim();
           
            return model;
        }
    }


}
