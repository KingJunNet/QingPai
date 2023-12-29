using System;
using System.Collections.Generic;
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
    }
}
