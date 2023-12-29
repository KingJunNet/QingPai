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
    /// 设备实体仓库
    /// </summary>
    public interface IEquipmentRepository
    {
        /// <summary>
        /// 查询设备
        /// </summary>     
        /// <param name="group">state</param>
        /// <returns>设备简要信息</returns>
        List<EquipmentBreief> select(string state);
    }
}
