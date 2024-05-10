using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.domain.valueobject
{
    public class ConstHolder
    {
        public static readonly string SECURITY_LEVEL_A = "A";
        public static readonly string SECURITY_LEVEL_A_REAMRK_TEXT = "A级保密；";

        public static readonly List<string> EQUIPMENT_STATE_CHN_NAMES = new List<string> { "使用中","未启用", "待检定", "停用", "报废" };
        public static readonly List<string> EQUIPMENT_TRACEABILITY_STATE_CHN_NAMES = new List<string> { "无", "失效", "有效" };

        /// <summary>
        /// 第三方应用配置文件名称
        /// </summary>
        public static readonly string THIRD_APP_CONFIG_FILE_NAME = "third-app-config.bin";

        public static readonly Color CONST_OTHER_GROUP_EQUIPMENT_BACK_COLOR = Color.LightYellow;
        public static readonly Color CONST_OTHER_GROUP_EQUIPMENT_FORE_COLOR = Color.Blue;
    }
}
