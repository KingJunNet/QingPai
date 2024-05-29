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
        /// <param name="sampleType">样本类型</param>
        /// <returns>样本简要信息</returns>
        SampleBrief selectLatestSampleVin(string vin, string sampleType);

        /// <summary>
        /// 查询指定的id的试验部分属性
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns>设备使用记录测试任务属性集合</returns>
        List<EquipmentUsageRecordTestPart> selectPartsByIds(List<int> ids);

        /// <summary>
        /// 通过记录的虚拟Id查询真实Id
        /// </summary>
        /// <param name="visualIds">虚拟id集合</param>
        /// <returns>试验统计Id信息集合</returns>
        List<TestStatisticIdInfo> selectIdInfosByVisualIds(List<string> visualIds);

        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <returns>void</returns>
        void updateFieldValue(List<int> ids,string fieldName,string fieldValue);
    }
}
