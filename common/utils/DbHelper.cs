using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.common.utils
{
    public class DbHelper
    {
        public static object ValueOrDBNullIfNull(String value)
        {
            if (value == null) return DBNull.Value;
            return value;
        }

        public static object ValueOrDBNullIfNull(double value)
        {
            if (value < 0) return DBNull.Value;
            return value;
        }

        public static object ValueOrDBNullIfNull(int value)
        {
            if (value < 0) return DBNull.Value;
            return value;
        }

        public static object ValueOrDBNullIfNull(DateTime value)
        {
            if (value.Year == 1) return DBNull.Value;
            return value;
        }

        public static string dataColumn2String(object dataValue)
        {
            if (dataValue is DBNull)
            {
                return null;
            }
            return dataValue.ToString().Trim();
        }

        public static DateTime dataColumn2DateTime(object dataValue)
        {
            if (!(dataValue is DBNull))
            {
                return (DateTime)dataValue;
            }

            return DateTime.MinValue;
        }
    }
}
