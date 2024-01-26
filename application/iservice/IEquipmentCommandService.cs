using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.application.viewmodel;

namespace TaskManager.application.Iservice
{
    /// <summary>
    /// 设备命令服务
    /// </summary>
    interface IEquipmentCommandService
    {
        /// <summary>
        /// 创建项目设备信息
        /// </summary>
        /// <param name="itemName">项目名称</param>
        /// <param name="group">组别信息</param>
        /// <param name="locationNumber">定位编号</param>
        /// <param name="equipmentCodes">设备编号集合</param>
        /// <returns>void</returns>
        void createItemEquipments(string itemName,string group,string locationNumber,List<string> equipmentCodes);

        /// <summary>
        /// 更新项目设备信息
        /// </summary>
        /// <param name="itemName">项目名称</param>
        /// <param name="group">组别信息</param>
        /// <param name="locationNumber">定位编号</param>
        /// <param name="equipmentCodes">设备编号集合</param>
        /// <returns>void</returns>
        void updateItemEquipments(string itemName, string group, string locationNumber, List<string> equipmentCodes);
    }
}
