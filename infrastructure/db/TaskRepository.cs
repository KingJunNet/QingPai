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
    ///任务实体仓库
    /// </summary>
    public class TaskRepository : BaseRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository() : base()
        {

        }

        /// <summary>
        /// 查询指定样本型号的任务信息
        /// </summary>
        /// <param name="sampleModel">vin</param>
        /// <returns>任务简要信息</returns>
        public List<TaskBrief> selectBySampleModel(string sampleModel)
        {
            if (string.IsNullOrWhiteSpace(sampleModel)) {
                return new List<TaskBrief>();
            }

            string sql = $"select ID, Taskcode,Model,SecurityLevel " +
                $"from NewTaskTable " +
                $"WHERE Model=@Model";
            SqlParameter[] sqlParameters = new[] {
                    new SqlParameter("Model",sampleModel)
                };
            List<TaskBrief> results = selectList<TaskBrief>(sql, sqlParameters, (row) =>
                  dataRow2TaskBrief(row)
            );

            return results;
        }

        private TaskBrief dataRow2TaskBrief(DataRow row)
        {
            TaskBrief model = new TaskBrief();
         
            model.Id = int.Parse(row["ID"].ToString().Trim());
            model.TaskCode = row["Taskcode"].ToString().Trim();
            model.Model = row["Model"].ToString().Trim();
            model.SecurityLevel = row["SecurityLevel"].ToString().Trim();
         
            return model;
        }
    }
}
