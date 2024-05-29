using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.domain.entity;

namespace TaskManager.model
{
    public class TestStatisticEntityDataRowPair
    {
        /// <summary>
        /// 实体信息
        /// </summary>
        public TestStatisticEntity Entity { get; set; }

        /// <summary>
        /// DataRow
        /// </summary>
        public DataRow DataRow { get; set; }

        public TestStatisticEntityDataRowPair()
        {

        }


        public TestStatisticEntityDataRowPair(TestStatisticEntity entity, DataRow dataRow)
        {
            Entity = entity;
            DataRow = dataRow;
        }
    }

    public class TestStatisticUsageRecordsPair
    {
        /// <summary>
        /// 试验统计信息
        /// </summary>
        public TestStatisticEntity TestStatistic { get; set; }

        /// <summary>
        /// 设备使用记录集合
        /// </summary>
        public List<EquipmentUsageRecordEntity> EquipmentUsageRecords { get; set; }

        public TestStatisticUsageRecordsPair()
        {

        }


        public TestStatisticUsageRecordsPair(TestStatisticEntity testStatistic, 
            List<EquipmentUsageRecordEntity> equipmentUsageRecords)
        {
            this.TestStatistic = testStatistic;
            this.EquipmentUsageRecords = equipmentUsageRecords;
        }
    }
}
