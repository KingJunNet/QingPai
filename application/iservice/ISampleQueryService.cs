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
        List<string> allCarSampleVins();

        List<string> allCanisterSampleVins();

        /// <summary>
        /// 获取vin对应的样本信息
        /// </summary>
        /// <param name="vin">vin</param>
        /// <param name="sampleType">样本类型</param>
        /// <returns>样本信息</returns>
        SampleOfVinViewModel samplesOfVin(string vin,string sampleType);

 
    }
}
