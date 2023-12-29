using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.domain.valueobject
{
    public class TaskBrief
    {
        /// <summary>
        /// Id
        /// </summary>      
        public int Id { get; set; }

  
        /// <summary>
        /// 任务编号
        /// </summary>      
        public string TaskCode { get; set; }

        /// <summary>
        /// 样本型号
        /// </summary>      
        public string Model { get; set; }

        /// <summary>
        /// 保密级别
        /// </summary>      
        public string SecurityLevel { get; set; }
     
    }
}
