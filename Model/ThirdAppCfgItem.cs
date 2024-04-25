using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model
{
    /// <summary>
    /// 第三方应用配置项
    /// </summary>
    [Serializable]
    public class ThirdAppCfgItem
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 应用路径
        /// </summary>
        public string Path { get; set; }

        // 无参数的构造函数，用于反序列化时创建对象  
        public ThirdAppCfgItem()
        {
        }

        public ThirdAppCfgItem(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}
