using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.common.utils
{
    public class ValidateException:Exception
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int Code{ get; set; }

        public ValidateException()
        {

        }

        public ValidateException(int code,string message):base(message)
        {
            this.Code = code;
        }
    }
}
