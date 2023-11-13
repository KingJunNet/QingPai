using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.domain.entity;

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
        void save(TestStatisticEntity entity);
    }
}
