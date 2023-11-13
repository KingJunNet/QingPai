using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.application.viewmodel;
using TaskManager.domain.valueobject;

namespace TaskManager.application.Iservice
{
    /// <summary>
    /// 样品命令服务
    /// </summary>
    interface ISampleCommandService
    {
        /// <summary>
        /// 通过简要信息创建样本
        /// </summary>
        /// <param name="sampleBrief">样本简要信息</param>
        /// <returns>void</returns>
        void createByBrief(SampleBrief sampleBrief);

        /// <summary>
        /// 更新样本
        /// </summary>
        /// <param name="sampleBrief">样本简要信息</param>
        /// <returns>void</returns>
        void updateByBrief(SampleBrief sampleBrief);


    }
}
