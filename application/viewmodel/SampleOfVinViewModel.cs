using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.domain.valueobject;

namespace TaskManager.application.viewmodel
{
    public class SampleOfVinViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public SampleBrief FromSampleTable { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public SampleBrief FromStatisticTable { get; set; }
    }
}
