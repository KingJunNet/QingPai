using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.common.utils;
using TaskManager.domain.valueobject;

namespace TaskManager.domain.valueobject
{
    /// <summary>
    /// 用户组织几简要信息
    /// </summary>
    public class UserStructureLite
    {
        /// <summary>
        /// 实验地点
        /// </summary>      
        public string ExperimentSite { get; set; }

        /// <summary>
        /// 组
        /// </summary>      
        public string Group { get; set; }

        /// <summary>
        /// 定位编号
        /// </summary>      
        public string LocationNumber { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>      
        public string UserName { get; set; }
    }
}
