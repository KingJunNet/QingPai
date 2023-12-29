using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.common.utils
{
    public class Collections
    {
        public static bool isEmpty<T>(List<T> list) {
            return list == null || list.Count == 0;
        }

        public static bool equals(List<string> list0, List<string> list1)
        {
            if (list0.Count != list1.Count) {
                return false;
            }

            if (list0.Count == 0 && list1.Count == 0) {
                return true;
            }

            List<string> exceptList0= list0.Except(list1).ToList();
            List<string> exceptList1 = list1.Except(list0).ToList();
            return exceptList0.Count == 0 && exceptList1.Count == 0;
        }
    }
}
