using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.domain.valueobject
{
    public class TestStatisticIdInfo
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// 虚拟Id
        /// </summary>
        public string VisualId { get; set; }
    }
}
