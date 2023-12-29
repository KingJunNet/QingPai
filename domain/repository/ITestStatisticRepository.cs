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
    /// 实验统计实体仓库
    /// </summary>
    public interface ITestStatisticRepository
    {
        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>void</returns>
        int save(TestStatisticEntity entity);

        /// <summary>
        /// 查询指定的vin的最新的样本简要信息
        /// </summary>
        /// <param name="vin">vin</param>
        /// <returns>样本简要信息</returns>
        SampleBrief selectLatestSampleVin(string vin);
    }
}
