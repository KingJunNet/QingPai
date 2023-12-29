using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.common.utils
{
    public class StringUtils
    {
        public static bool isEquals(string arg0,string arg1) {
            if (arg0 == null && arg1 == null) {
                return true;
            }

            if ((arg0 == null && arg1 != null)||(arg0 != null && arg1 == null)) {
                return false;
            }

            return arg0.Equals(arg1);
        }
    }
}
