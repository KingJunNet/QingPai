using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model
{

    public enum ThirdAppName
    {
        [Description("蒸发客户端")]
        EVAPORATION_SYSTEM = 0,

        [Description("称重客户端")]
        WEIGHT_CLIENT = 1
    }

    public enum FileCategory
    {
        [Description("安装包")]
        APP_EXE = 0,

        [Description("项目代码")]
        PROJECT_CODE = 1
    }

    public enum TaskType
    {
        [Description("公告")]
        QA = 0,

        [Description("环保")]
        HB = 1,

        [Description("委托")]
        WT = 2,

        [Description("修改")]
        XG = 3,

        [Description("技研")]
        JY = 4,

        [Description("研发")]
        YF = 5,

        [Description("出口")]
        CK = 6,

        [Description("商检")]
        SZ = 7,

        [Description("测评")]
        CA = 8,

        [Description("3C")]
        HC = 9,
    }

    static class EnumExtensions
    {
        public static string GetDescription(this Enum val)
        {
            var field = val.GetType().GetField(val.ToString());
            var customAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return customAttribute == null ? val.ToString() : ((DescriptionAttribute)customAttribute).Description;
        }
    }

}
