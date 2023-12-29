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
    /// 项目设备关系实体仓库
    /// </summary>
    public interface IItemEquipmentRepository
    {
        /// <summary>
        /// 查询指定项目的设备信息
        /// </summary>
        /// <param name="itemName">项目名称</param>
        /// <param name="group">组别</param>
        /// <returns>设备简要信息</returns>
        List<EquipmentLite> equipmentsOfItem(string itemName, string group);

        /// <summary>
        /// 查询指定实验任务的设备信息
        /// </summary>
        /// <param name="testTaskId">实验任务Id</param>
        /// <returns>设备简要信息集合</returns>
        List<EquipmentLite> equipmentsOfTestTask(int testTaskId);

        /// <summary>
        /// 删除指定项目的设备信息
        /// </summary>
        /// <param name="itemName">项目名称</param>
        /// <param name="group">组别</param>
        /// <returns>void</returns>
        void removeEquipmentsOfItem(string itemName, string group);

        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>void</returns>
        void save(ItemEquipmentEntity entity);


    }
}
