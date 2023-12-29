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
    public class EquipmentLite
    {
        public EquipmentLite copy()
        {
            EquipmentLite copy = new EquipmentLite();

            copy.Code = this.Code;
            copy.Name = this.Name;
            copy.Type = this.Type;
            copy.Group = this.Group;

            return copy;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 所属组别
        /// </summary>
        public string Group { get; set; }


    }
}
