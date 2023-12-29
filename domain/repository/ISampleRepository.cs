using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.domain.entity;
using TaskManager.domain.valueobject;

namespace TaskManager.domain.repository
{
    /// <summary>
    /// 样本实体仓库
    /// </summary>
    public interface ISampleRepository
    {
        /// <summary>
        /// 查询制定vin的样本简要信息
        /// </summary>
        /// <param name="vin">vin</param>
        /// <returns>样本简要信息</returns>
        SampleBrief selectByVin(string vin);

        /// <summary>
        /// 保存样本
        /// </summary>
        /// <param name="entity">样本实体</param>
        /// <returns>void</returns>
        int save(SampleEntity entity);

        /// <summary>
        /// 更新样本
        /// </summary>
        /// <param name="sampleBrief">样本简要信息</param>
        /// <returns>void</returns>
        void updateByBrief(SampleBrief sampleBrief);

    }
}
