using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.domain.valueobject
{
    /// <summary>
    /// 设备使用记录简要信息
    /// </summary>
    public class EquipmentUsageRecordLite
    {
        public EquipmentLite toEquipmentLite()
        {
            EquipmentLite copy = new EquipmentLite();

            copy.Code = this.Code;
            copy.Name = this.Name;
            copy.Type = this.Type;
            copy.Group = this.Group;

            return copy;
        }

        /// <summary>
        /// Id
        /// </summary>      
        public int ID { get; set; }

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

        /// <summary>
        /// 保密级别
        /// </summary>
        public string SecurityLevel { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
