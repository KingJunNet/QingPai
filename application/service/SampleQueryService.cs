using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.application.Iservice;
using TaskManager.application.viewmodel;

namespace TaskManager.application.service
{
    public class SampleQueryService :ISampleQueryService
    {
        public List<string> allSampleVins() {
            return new List<string>();
        }

        /// <summary>
        /// 获取vin对应的样本信息
        /// </summary>
        /// <param name="vin">vin</param>
        /// <returns>样本信息</returns>
        public SampleOfVinViewModel SamplesOfVin(string vin)
        {
            return new SampleOfVinViewModel();
        }
    }
}
