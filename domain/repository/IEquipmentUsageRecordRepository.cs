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
    /// 设备使用记录实体仓库
    /// </summary>
    public interface IEquipmentUsageRecordRepository
    {    
        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>void</returns>
        void save(EquipmentUsageRecordEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>void</returns>
        void update(EquipmentUsageRecordEntity entity);

        /// <summary>
        /// 批量保存实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>void</returns>
        void batchSave(List<EquipmentUsageRecordEntity> entities);

        /// <summary>
        /// 查询指定实验任务的设备使用记录简要信息
        /// </summary>
        /// <param name="testTaskId">实验任务Id</param>
        /// <returns>设备使用记录简要信息集合</returns>
        List<EquipmentUsageRecordLite> litesOfTestTask(int testTaskId);

        /// <summary>
        /// 删除指定测试任务的设备使用记录
        /// </summary>
        /// <param name="testTaskId">实验任务Id</param>
        /// <returns>void</returns>
        void removeByTestTaskId(int testTaskId);

        /// <summary>
        /// 更新测试任务属性
        /// </summary>
        /// <param name="testPart">设备使用记录测试任务属性</param>
        /// <returns>void</returns>
        void updateTestTaskProperty(EquipmentUsageRecordTestPart testPart);

        /// <summary>
        /// 更新备注信息
        /// </summary>
        /// <param name="needUpdateRemarkRecords">需要更新备注信息的记录</param>
        /// <returns>void</returns>
        void updateRemark(List<EquipmentUsageRecordLite> needUpdateRemarkRecords);
    }
}
