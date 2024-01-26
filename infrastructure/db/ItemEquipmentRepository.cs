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
    public class ItemEquipmentRepository : BaseRepository<ItemEquipmentEntity>, IItemEquipmentRepository
    {

        public ItemEquipmentRepository() : base()
        {

        }

        /// <summary>
        /// 查询指定项目的设备信息
        /// </summary>
        /// <param name="itemName">项目名称</param>
        /// <param name="group">组别</param>
        /// <param name="locationNumber">定位编号</param> 
        /// <returns>设备简要信息集合</returns>
        public List<EquipmentLite> equipmentsOfItem(string itemName, string group, string locationNumber)
        {
            string sql = $"select DISTINCT E.EquipCode,E.EquipName,E.EquipType,E.GroupName as GroupName " +
                $"from ItemEquipmentTable IE " +
                $"INNER JOIN NewEquipmentTable E " +
                $"ON IE.EquipmentCode=E.EquipCode " +
                $"WHERE IE.ItemName=@ItemName and IE.GroupName=@GroupName and IE.LocationNumber=@LocationNumber";
            SqlParameter[] sqlParameters = new[] {
                    new SqlParameter("ItemName",itemName),
                    new SqlParameter("GroupName",group),
                    new SqlParameter("LocationNumber",locationNumber)
                };

            List<EquipmentLite> results = selectList<EquipmentLite>(sql, sqlParameters, (row) =>
                  dataRow2EquipmentLite(row)
            );

            return results;
        }

        /// <summary>
        /// 查询指定实验任务的设备信息
        /// </summary>
        /// <param name="testTaskId">实验任务Id</param>
        /// <returns>设备简要信息集合</returns>
        public List<EquipmentLite> equipmentsOfTestTask(int testTaskId) {
            string sql = $"select E.EquipCode,E.EquipName,E.EquipType,E.GroupName as GroupName " +
               $"from EquipmentUsageRecordTable IE " +
               $"INNER JOIN NewEquipmentTable E " +
               $"ON IE.EquipmentCode=E.EquipCode " +
               $"WHERE IE.TestTaskId=@TestTaskId ";
            SqlParameter[] sqlParameters = new[] {
                    new SqlParameter("TestTaskId",testTaskId)
                };

            List<EquipmentLite> results = selectList<EquipmentLite>(sql, sqlParameters, (row) =>
                  dataRow2EquipmentLite(row)
            );

            return results;
        }

        /// <summary>
        /// 删除指定项目的设备信息
        /// </summary>
        /// <param name="itemName">项目名称</param>
        /// <param name="group">组别</param>
        /// <param name="locationNumber">定位编号</param>
        /// <returns>void</returns>
        public void removeEquipmentsOfItem(string itemName, string group, string locationNumber)
        {
            string sql = $"delete  " +
               $"from ItemEquipmentTable " +            
               $"WHERE ItemName=@ItemName and GroupName=@GroupName and LocationNumber=@LocationNumber";
            SqlParameter[] sqlParameters = new[] {
                    new SqlParameter("ItemName",itemName),
                    new SqlParameter("GroupName",group),
                    new SqlParameter("LocationNumber",locationNumber)
                };

            this.executeWrite(sql,sqlParameters);
        }

        private EquipmentLite dataRow2EquipmentLite(DataRow row)
        {
            EquipmentLite equipmentLite = new EquipmentLite();
            equipmentLite.Code = row["EquipCode"].ToString().Trim();
            equipmentLite.Name = row["EquipName"].ToString().Trim();
            equipmentLite.Type = row["EquipType"].ToString().Trim();
            equipmentLite.Group = row["GroupName"].ToString().Trim();

            return equipmentLite;
        }
    }


}
