using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.application.Iservice;
using TaskManager.application.viewmodel;
using TaskManager.domain.repository;
using TaskManager.domain.valueobject;
using TaskManager.infrastructure.db;

namespace TaskManager.application.service
{
    public class SampleQueryService : ISampleQueryService
    {

        /// <summary>
        /// 数据库供应者
        /// </summary>
        private DataControl dbProvider;

        /// <summary>
        /// 样本实体仓库
        /// </summary>
        private ISampleRepository sampleRepository;

        /// <summary>
        /// 实验统计实体仓库
        /// </summary>
        private ITestStatisticRepository testStatisticRepository;

        public SampleQueryService()
        {
            this.dbProvider = new DataControl();
            this.sampleRepository = new SampleRepository();
            this.testStatisticRepository = new TestStatisticRepository();
        }

        /// <summary>
        /// 获取所有的win
        /// </summary>
        /// <param name="vin">vin</param>
        /// <returns>样本信息</returns>
        public List<string> allSampleVinsBack()
        {
            //执行数据库查询
            String sql = "SELECT VIN FROM SampleTable WHERE VIN IS NOT NULL " +
                           "union all SELECT Carvin AS VIN FROM TestStatistic WHERE Carvin IS NOT NULL";
            List<string> vins = this.dbProvider.GetStringList(sql);

            //过滤
            List<string> notValidVins = new List<string> { "-", "——", "无" };
            List<string> results = new List<string>();
            foreach (string vin in vins)
            {
                if (results.Contains(vin))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(vin) && !notValidVins.Contains(vin))
                {
                    results.Add(vin);
                }
            }

            return results;
        }

        /// <summary>
        /// 获取所有整车的win
        /// </summary>
        /// <param name="vin">vin</param>
        /// <returns>样本信息</returns>
        public List<string> allCarSampleVins()
        {
            return this.allSampleVins("整车");
        }

        public List<string> allCanisterSampleVins()
        {
            return this.allSampleVins("碳罐");
        }

        private List<string> allSampleVins(string sampleType)
        {

            //执行数据库查询
            String sql = $"SELECT VIN FROM SampleTable WHERE VIN IS NOT NULL AND SampleType = '{sampleType}'";
            List<string> vins = this.dbProvider.GetStringList(sql);

            //过滤
            List<string> notValidVins = new List<string> { "-", "——", "无" };
            List<string> results = new List<string>();
            foreach (string vin in vins)
            {
                if (results.Contains(vin))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(vin) && !notValidVins.Contains(vin))
                {
                    results.Add(vin);
                }
            }

            return results;
        }



        /// <summary>
        /// 获取vin对应的样本信息
        /// </summary>
        /// <param name="vin">vin</param>
        /// <param name="sampleType">样本类型</param>
        /// <returns>样本信息</returns>
        public SampleOfVinViewModel samplesOfVin(string vin, string sampleType)
        {
            SampleBrief sampleFromSample = this.sampleRepository.selectByVin(vin,sampleType);
            SampleBrief sampleFromStatistic = this.testStatisticRepository.selectLatestSampleVin(vin,sampleType);
            if (sampleFromSample == null && sampleFromStatistic == null)
            {
                return null;
            }

            return new SampleOfVinViewModel(sampleFromSample, sampleFromStatistic);
        }
    }
}
