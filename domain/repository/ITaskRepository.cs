using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.domain.entity;
using TaskManager.domain.valueobject;

namespace TaskManager.domain.repository
{
    /// <summary>
    /// 任务实体仓库
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// 查询指定样本型号的任务信息
        /// </summary>
        /// <param name="sampleModel">vin</param>
        /// <returns>任务简要信息</returns>
        List<TaskBrief> selectBySampleModel(string sampleModel);    
   }
}
