using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.domain.valueobject
{
    public class FieldChangedState
    {
        public FieldChangedState(string name, string oriValue, string newValue)
        {
            Name = name;
            OriValue = oriValue;
            NewValue = newValue;
        }

        /// <summary>
        /// 字段名
        /// </summary>
        /// <value>The id.</value>
        public string Name { get; set; }

        /// <summary>
        /// 原有值
        /// </summary>
        /// <value>The id.</value>
        public string OriValue { get; set; }

        /// <summary>
        /// 新值
        /// </summary>
        /// <value>The id.</value>
        public string NewValue { get; set; }



        public string changedDescription() {
            return $"{this.Name}由‘{this.OriValue}’修改为‘{this.NewValue}’";
        }

        public static string buildChangedDescription(List<FieldChangedState> changedStates) {
            List<string> changeddDescriptions = changedStates.Select(item => item.changedDescription()).ToList();
            string result = string.Join("，",changeddDescriptions);

            return result;
        }
    }
}
