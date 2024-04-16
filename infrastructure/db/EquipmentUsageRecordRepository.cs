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
    public class EquipmentUsageRecordRepository : BaseRepository<EquipmentUsageRecordEntity>, IEquipmentUsageRecordRepository
    {

        public EquipmentUsageRecordRepository() : base()
        {

        }


        /// <summary>
        /// 批量保存实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>void</returns>
        public void batchSave(List<EquipmentUsageRecordEntity> entities)
        {
            if (Collections.isEmpty(entities))
            {
                return;
            }

            entities.ForEach(entity => save(entity));

        }

        /// <summary>
        /// 查询指定实验任务的设备使用记录简要信息
        /// </summary>
        /// <param name="testTaskId">实验任务Id</param>
        /// <returns>设备使用记录简要信息集合</returns>
        public List<EquipmentUsageRecordLite> litesOfTestTask(int testTaskId)
        {
            string sql = $"select DISTINCT IE.ID, E.EquipCode,E.EquipName,E.EquipType,E.GroupName as GroupName,IE.SecurityLevel,IE.Remark " +
               $"from EquipmentUsageRecordTable IE " +
               $"INNER JOIN NewEquipmentTable E " +
               $"ON IE.EquipmentCode=E.EquipCode " +
               $"WHERE IE.TestTaskId=@TestTaskId ";
            SqlParameter[] sqlParameters = new[] {
                    new SqlParameter("TestTaskId",testTaskId)
                };

            List<EquipmentUsageRecordLite> results = selectList<EquipmentUsageRecordLite>(sql, sqlParameters, (row) =>
                  dataRow2EquipmentLite(row)
            );

            return results;
        }

        private EquipmentUsageRecordLite dataRow2EquipmentLite(DataRow row)
        {
            EquipmentUsageRecordLite equipmentLite = new EquipmentUsageRecordLite();
            equipmentLite.ID= int.Parse(row["ID"].ToString().Trim());
            equipmentLite.Code = row["EquipCode"].ToString().Trim();
            equipmentLite.Name = row["EquipName"].ToString().Trim();
            equipmentLite.Type = row["EquipType"].ToString().Trim();
            equipmentLite.Group = row["GroupName"].ToString().Trim();
            equipmentLite.SecurityLevel = DbHelper.dataColumn2String(row["SecurityLevel"]);
            equipmentLite.Remark = DbHelper.dataColumn2String(row["Remark"]);

            return equipmentLite;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>void</returns>
        public void update(EquipmentUsageRecordEntity entity) {
        }

        /// <summary>
        /// 更新测试任务属性
        /// </summary>
        /// <param name="testPart">设备使用记录测试任务属性</param>
        /// <returns>void</returns>
        public void updateTestTaskProperty(EquipmentUsageRecordTestPart testPart)
        {
            List<string> valueConditions = testPart.buildUpdateValueConditions();
            string dbSetText = string.Join(",", valueConditions);
            string sqlText = $"UPDATE EquipmentUsageRecordTable SET {dbSetText} where TestTaskId=@TestTaskId";
            SqlParameter[] parameters = testPart.toAllSqlParameters();
            Array.Resize(ref parameters, parameters.Length + 1);
            parameters[parameters.Length - 1] = new SqlParameter("TestTaskId", DbHelper.ValueOrDBNullIfNull(testPart.TestTaskId));
            dbProvider.ExecuteNonQuery(sqlText, parameters);
        }

        /// <summary>
        /// 更新备注信息
        /// </summary>
        /// <param name="needUpdateRemarkRecords">需要更新备注信息的记录</param>
        /// <returns>void</returns>
        public void updateRemark(List<EquipmentUsageRecordLite> needUpdateRemarkRecords) {
            if (Collections.isEmpty(needUpdateRemarkRecords)) {
                return;
            }
            //逐个更新
            needUpdateRemarkRecords.ForEach(item =>
            {
                string sqlText = $"UPDATE EquipmentUsageRecordTable SET Remark=@Remark where ID=@ID";
                SqlParameter[] sqlParameters = new[] {
                    new SqlParameter("Remark", DbHelper.ValueOrDBNullIfNull(item.Remark)),
                     new SqlParameter("ID", DbHelper.ValueOrDBNullIfNull(item.ID))
                };
                dbProvider.ExecuteNonQuery(sqlText, sqlParameters);
            });
        }

        /// <summary>
        /// 更新指定记录的导出时间
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="exportTime">导出时间</param>
        /// <returns>void</returns>
        public void updateExportTime(List<int> ids, DateTime exportTime) {
            if (Collections.isEmpty(ids))
            {
                return;
            }

            string sqlText = $"UPDATE EquipmentUsageRecordTable SET ExportTime=@ExportTime where ID in {DbHelper.buildInCondition(ids)}";
            SqlParameter[] sqlParameters = new[] {
                    new SqlParameter("ExportTime", DbHelper.ValueOrDBNullIfNull(exportTime))
                };
            dbProvider.ExecuteNonQuery(sqlText, sqlParameters);
        }

     

        /// <summary>
        /// 删除指定测试任务的设备使用记录
        /// </summary>
        /// <param name="testTaskId">实验任务Id</param>
        /// <returns>void</returns>
        public void removeByTestTaskId(int testTaskId) {
            string sql = $"delete  " +
              $"from EquipmentUsageRecordTable " +
              $"WHERE TestTaskId=@TestTaskId ";
            SqlParameter[] sqlParameters = new[] {
                    new SqlParameter("TestTaskId",testTaskId)
                };

            this.executeWrite(sql, sqlParameters);
        }
    }


}
