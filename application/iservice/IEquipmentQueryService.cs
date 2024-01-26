using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.application.viewmodel;
using TaskManager.domain.valueobject;

namespace TaskManager.application.Iservice
{
    /// <summary>
    /// 设备查询服务
    /// </summary>
    interface IEquipmentQueryService
    {
        /// <summary>
        /// 获取使用中的设备
        /// </summary>
        /// <param name="group">组别</param>
        /// <returns>设备简要信息</returns>
        List<EquipmentBreiefViewModel> usingEquipments(string group);

        /// <summary>
        /// 查询指定项目的设备信息
        /// </summary>
        /// <param name="itemName">项目名称</param>
        /// <param name="group">组别</param>
        /// <param name="locationNumber">定位编号</param> 
        /// <returns>设备简要信息集合</returns>
        List<EquipmentLite> equipmentsOfItem(string itemName,string group,string locationNumber);

        /// <summary>
        /// 查询指定实验任务的设备信息
        /// </summary>
        /// <param name="testTaskId">实验任务Id</param>
        /// <returns>设备简要信息集合</returns>
        List<EquipmentLite> equipmentsOfTestTask(int testTaskId);
    }
}
