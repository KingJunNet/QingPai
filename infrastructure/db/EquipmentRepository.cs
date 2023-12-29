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
    public class EquipmentRepository : BaseRepository<EquipmentEntity>, IEquipmentRepository
    {
        public EquipmentRepository() : base()
        {

        }

        /// <summary>
        /// 查询设备
        /// </summary>     
        /// <param name="group">state</param>
        /// <returns>设备简要信息</returns>
        public List<EquipmentBreief> select(string state)
        {
            string sql = $"select * " +
                $"from NewEquipmentTable ";
            SqlParameter[] sqlParameters = new SqlParameter[0];

            //有状态条件的
            if (!string.IsNullOrWhiteSpace(state)) {
                sql = $"{sql} WHERE State=@EquipState";
                sqlParameters = new[] {
                    new SqlParameter("EquipState",state)
                };
            }

            List<EquipmentBreief> results = selectList<EquipmentBreief>(sql, sqlParameters, (row) =>
                  dataRow2EquipmentBreief(row)
            );

            return results;
        }

        private EquipmentBreief dataRow2EquipmentBreief(DataRow row)
        {
            EquipmentBreief model = new EquipmentBreief();
            model.Code = row["EquipCode"].ToString().Trim();
            model.Name = row["EquipName"].ToString().Trim();
            model.Type = row["EquipType"].ToString().Trim();

            model.Id = int.Parse(row["ID"].ToString().Trim());
            model.Group = row["GroupName"].ToString().Trim();
            model.LocationNo = "";
            model.RegUser = row["Owner"].ToString().Trim();
            model.State = row["State"].ToString().Trim();

            return model;
        }
    }


}
