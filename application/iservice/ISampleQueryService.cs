using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.application.viewmodel;

namespace TaskManager.application.Iservice
{
    /// <summary>
    /// 样品查询服务
    /// </summary>
    interface ISampleQueryService
    {
        List<string> allSampleVins();

        /// <summary>
        /// 获取vin对应的样本信息
        /// </summary>
        /// <param name="vin">vin</param>
        /// <returns>样本信息</returns>
        SampleOfVinViewModel SamplesOfVin(string vin);

 
    }
}
