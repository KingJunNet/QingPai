using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.domain.entity;

namespace TaskManager.domain.service
{
    public class GenerateWordFileUnit
    {
        public string equipmentCodeDirectory;
        public string person;
        public string day;
        public string equipmentCode;
        public string equipmentName;
        public string equipmentType;
        public List<EquipmentUsageRecordEntity> equipmentUsageRecords;

        public GenerateWordFileUnit(string equipmentCodeDirectory,
            string person,
            string day,
            string equipmentCode,
            string equipmentName,
            string equipmentType,
            List<EquipmentUsageRecordEntity> equipmentUsageRecords)
        {
            this.equipmentCodeDirectory = equipmentCodeDirectory;
            this.person = person;
            this.day = day;
            this.equipmentCode = equipmentCode;
            this.equipmentName = equipmentName;
            this.equipmentType = equipmentType;
            this.equipmentUsageRecords = equipmentUsageRecords;
        }
    }

    public class GenerateWordFileUnitOri
    {
        public string personDirectory;
        public string day;
        public string equipmentCode;
        public string equipmentName;
        public string equipmentType;
        public List<EquipmentUsageRecordEntity> equipmentUsageRecords;

        public GenerateWordFileUnitOri(string personDirectory,
            string day,
            string equipmentCode,
            string equipmentName,
            string equipmentType,
            List<EquipmentUsageRecordEntity> equipmentUsageRecords)
        {
            this.personDirectory = personDirectory;
            this.day = day;
            this.equipmentCode = equipmentCode;
            this.equipmentName = equipmentName;
            this.equipmentType = equipmentType;
            this.equipmentUsageRecords = equipmentUsageRecords;
        }
    }
}
