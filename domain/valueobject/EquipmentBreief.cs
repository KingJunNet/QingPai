using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.domain.valueobject
{
    /// <summary>
    /// 设备简要信息
    /// </summary>
    public class EquipmentBreief
    {
        /// <summary>
        /// Id
        /// </summary>      
        public int Id { get; set; }

        /// <summary>
        /// 组
        /// </summary>
        public String Group { get; set; }

        /// <summary>
        /// 定位编号
        /// </summary>
        public String LocationNo { get; set; }

        /// <summary>
        /// 登记人
        /// </summary>
        public String RegUser { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public String State { get; set; }

    }
}
