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
