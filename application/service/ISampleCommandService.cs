using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.application.Iservice;
using TaskManager.application.viewmodel;
using TaskManager.domain.valueobject;

namespace TaskManager.application.service
{
    /// <summary>
    /// 样品查询服务
    /// </summary>
    public class SampleCommandService : ISampleCommandService
    {
        /// <summary>
        /// 通过简要信息创建样本
        /// </summary>
        /// <param name="sampleBrief">样本简要信息</param>
        /// <returns>void</returns>
        public void createByBrief(SampleBrief sampleBrief) {
        }

        /// <summary>
        /// 更新样本
        /// </summary>
        /// <param name="sampleBrief">样本简要信息</param>
        /// <returns>void</returns>
        public void updateByBrief(SampleBrief sampleBrief) {
        }


    }
}
