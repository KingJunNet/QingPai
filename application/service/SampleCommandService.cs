using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.application.Iservice;
using TaskManager.application.viewmodel;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
using TaskManager.domain.service;
using TaskManager.domain.valueobject;
using TaskManager.infrastructure.db;

namespace TaskManager.application.service
{
    /// <summary>
    /// 样品命令服务
    /// </summary>
    public class SampleCommandService : ISampleCommandService
    {
        /// <summary>
        /// 样本实体仓库
        /// </summary>
        private ISampleRepository sampleRepository;

     
        public SampleCommandService()
        {
            this.sampleRepository = new SampleRepository();
        }

        /// <summary>
        /// 通过简要信息创建样本
        /// </summary>
        /// <param name="sampleBrief">样本简要信息</param>
        /// <returns>void</returns>
        public void createByBrief(SampleBrief sampleBrief) {
            SampleEntity entity = new SampleEntity()
                .init(UseHolder.Instance.CurrentUser.Name, DateTime.Now)
                .fromBrief(sampleBrief);

            this.sampleRepository.save(entity);
        }

        /// <summary>
        /// 更新样本
        /// </summary>
        /// <param name="sampleBrief">样本简要信息</param>
        /// <returns>void</returns>
        public void updateByBrief(SampleBrief sampleBrief) {
            this.sampleRepository.updateByBrief(sampleBrief);
        }
    }
}
