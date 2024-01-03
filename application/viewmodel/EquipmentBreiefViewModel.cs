using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.domain.valueobject;

namespace TaskManager.application.viewmodel
{
    /// <summary>
    /// 视图模型-设备简要信息
    /// </summary>
    public class EquipmentBreiefViewModel
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

        /// <summary>
        /// 视图名称
        /// </summary>
        public String ViewName { get; set; }

        public EquipmentBreiefViewModel fromEntity(EquipmentBreief Entity)
        {
            Id = Entity.Id;
            Group = Entity.Group;
            LocationNo = Entity.LocationNo;
            RegUser = Entity.RegUser;
            Name = Entity.Name;
            Type = Entity.Type;
            Code = Entity.Code;
            State = string.IsNullOrWhiteSpace(Entity.State) ? "": Entity.State;


            return this;
        }

        public EquipmentBreiefViewModel setViewName(string group)
        {
            this.ViewName = $"{this.State}-{this.Code}-{this.Name}";
            if (!this.Group.Equals(group))
            {
                this.ViewName = $"{this.ViewName}({this.Group})";
            }
            else {
                this.ViewName = $"{this.ViewName}({this.Group})";
            }

            return this;
        }

        override
        public string ToString()
        {
            return this.ViewName;
        }

        public EquipmentLite toEquipmentLite() {
            EquipmentLite lite = new EquipmentLite();
            lite.Code = this.Code;
            lite.Name = this.Name;
            lite.Type = this.Type;
            lite.Group = this.Group;

            return lite;
        }

    }
}
