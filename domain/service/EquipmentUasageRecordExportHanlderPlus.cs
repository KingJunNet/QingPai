
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.common.utils;
using TaskManager.controller;
using TaskManager.domain.entity;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;

namespace TaskManager.domain.service
{
    class EquipmentUasageRecordExportHanlderPlus
    {
        private MaskLayer maskLayer;
        private List<EquipmentUsageRecordEntity> equipmentUsageRecords;

        private string equipmentUasageRecordWordTplFilePath;

        public static string EQUIPMENT_USAGE_RECORD_WORD_TPL_RELAITIVE_PATH = @"template\equipment_usage_record_tpl_a.docx";

        private static readonly string BASE_DIRECTORY_NAME = "设备使用记录";

        private static readonly int WORD_PER_PAGE_USAGERECORD_COUNT = 6;

        private List<GenerateWordFileUnit> generateWordFileUnits;

        public bool isAbort = false;

        /// <summary>
        /// 文件基础地址
        /// </summary>
        public string FileBasePath { get; set; }

        public EquipmentUasageRecordExportHanlderPlus(List<EquipmentUsageRecordEntity> equipmentUsageRecords, MaskLayer lMaskLayer1)
        {
            this.equipmentUsageRecords = equipmentUsageRecords;
            this.maskLayer = lMaskLayer1;
        }

        public void work()
        {
            this.generateWordFileUnits = new List<GenerateWordFileUnit>();
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            this.FileBasePath = Path.Combine(appDirectory, BASE_DIRECTORY_NAME);
            this.equipmentUasageRecordWordTplFilePath = Path.Combine(appDirectory, EQUIPMENT_USAGE_RECORD_WORD_TPL_RELAITIVE_PATH);
            this.createDirectory(this.FileBasePath);
            this.generateWordFiles();
            this.executeGenerate();
        }

        private void executeGenerate() {
            maskLayer.progressBar.Maximum = this.generateWordFileUnits.Count;
            for (   int i = 0; i < this.generateWordFileUnits.Count; i++)
            {
                if (UIHelp.Instance.AbortEquipmentUasageRecordExportWork) {
                    isAbort = true;
                    return;
                }
                this.generateUsageRecordWordFiles(this.generateWordFileUnits[i]);
                maskLayer.SetProgressBarValue(i+1);
            }
        }

        private void generateWordFiles()
        {
            //按照日期-部门-人分组
            var groupByDayDepPersonResults = this.equipmentUsageRecords.GroupBy(p => p, new EquipmentUasageRecordByDayDepPersonComparer());
            foreach (var groupByDayDepPerson in groupByDayDepPersonResults)
            {
                this.handleGroupByDayDepPerson(groupByDayDepPerson);
            }
        }

        private void handleGroupByDayDepPerson(IGrouping<EquipmentUsageRecordEntity, EquipmentUsageRecordEntity> groupByDayDepPerson)
        {
            string day = groupByDayDepPerson.Key.UseTime.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string department = groupByDayDepPerson.Key.Department;
            string person = groupByDayDepPerson.Key.UsePerson;

            //创建目录
            string personDirectory = this.createExportFileDirectory(day, department, person);

            //按照设备分组，生成记录
            var groupByEquipmentResults = groupByDayDepPerson.ToList().GroupBy(p => p, new EquipmentUasageRecordByEquipmentComparer());
            foreach (var groupByEquipment in groupByEquipmentResults)
            {
                this.handleGroupByEquipment(groupByEquipment, personDirectory, day);
            }
        }

        private void handleGroupByEquipment(IGrouping<EquipmentUsageRecordEntity, EquipmentUsageRecordEntity> groupByEquipment, string personDirectory, string day)
        {
            string equipmentCode = groupByEquipment.Key.EquipmentCode;
            string equipmentName = groupByEquipment.Key.EquipmentName;
            string equipmentType = groupByEquipment.Key.EquipmentType;

            //生成word文件
            GenerateWordFileUnit unit = new GenerateWordFileUnit(personDirectory, day, equipmentCode, equipmentName, equipmentType, groupByEquipment.ToList());
            this.generateWordFileUnits.Add(unit);
        }

        private void generateUsageRecordWordFiles(GenerateWordFileUnit unit)
        {
            this.generateUsageRecordWordFiles(unit.personDirectory,
                unit.day,
                unit.equipmentCode, 
                unit.equipmentName,
                unit.equipmentType,
                unit.equipmentUsageRecords);
        }

        private void generateUsageRecordWordFiles(string personDirectory,
            string day,
                                    string equipmentCode,
                                    string equipmentName,
                                    string equipmentType,
                                    List<EquipmentUsageRecordEntity> equipmentUsageRecords)
        {
            //排序
            equipmentUsageRecords.Sort((arg0, arg1) => arg0.UseTime.Ticks.CompareTo(arg1.UseTime.Ticks));

            int totalCount = equipmentUsageRecords.Count;
            if (totalCount <= WORD_PER_PAGE_USAGERECORD_COUNT)
            {
                string fileName = $"{equipmentName}({equipmentCode})-{day}.doc";
                string filePath = Path.Combine(personDirectory, fileName);
                saveUsageRecordWordFile(filePath, equipmentCode, equipmentName, equipmentType, equipmentUsageRecords);
                return;
            }

            //记录过多就拆分
            int startIndex = 0;
            int number = 1;
            while (startIndex < totalCount)
            {
                int curCount = (totalCount - startIndex) < WORD_PER_PAGE_USAGERECORD_COUNT ?
                    (totalCount - startIndex) :
                    WORD_PER_PAGE_USAGERECORD_COUNT;
                List<EquipmentUsageRecordEntity> subEquipmentUsageRecords = equipmentUsageRecords.GetRange(startIndex, curCount);
                string fileName = $"{equipmentName}({equipmentCode})-{day}-{number}.doc";
                string filePath = Path.Combine(personDirectory, fileName);
                saveUsageRecordWordFile(filePath, equipmentCode, equipmentName, equipmentType, subEquipmentUsageRecords);
                startIndex = startIndex + curCount;
                number++;
            }
        }
    
        private void saveUsageRecordWordFile(string filePath,
                                  string equipmentCode,
                                  string equipmentName,
                                  string equipmentType,
                                  List<EquipmentUsageRecordEntity> equipmentUsageRecords)
        {
            ReportPlus report = new ReportPlus();
            report.CreateNewDocument(this.equipmentUasageRecordWordTplFilePath);

            //操作表格
            TableReport tableReport = report.createTableReport(0);

            //插入设备信息
            tableReport.InsertValue(2,1, StringUtils.null2Empty(equipmentName));
            tableReport.InsertValue(2, 3, StringUtils.null2Empty(equipmentType));
            tableReport.InsertValue(2, 5, StringUtils.null2Empty(equipmentCode));
               
            //添加使用记录行
            int count = equipmentUsageRecords.Count;
            //tableReport.AddRowEx(5, count-1);
            for (int index = 0; index < equipmentUsageRecords.Count; index++)
            {
                int rowIndex = 5 + index;
                string[] values = this.equipmentUsageRecordToCellValues(equipmentUsageRecords[index]);
                tableReport.InsertCell(rowIndex, values);
            }

            //删除多余行
            if (count < WORD_PER_PAGE_USAGERECORD_COUNT) {
                tableReport.RemoveRow(5 + count, 10);
            }
           
            //保存文档
            report.SaveDocument(filePath);
        }

        private string[] equipmentUsageRecordToCellValues(EquipmentUsageRecordEntity equipmentUsageRecord)
        {
            string[] values = new string[9];

            values[0] = equipmentUsageRecord.UseTime.ToString("yyyy.MM.dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            if (equipmentUsageRecord.PreUseState.Equals("正常"))
            {
                values[1] = "√";
                values[2] = "";
            }
            else
            {
                values[1] = "";
                values[2] = "√";
            }
            values[3] =StringUtils.null2Empty(equipmentUsageRecord.Purpose);
            values[4] = StringUtils.null2Empty(equipmentUsageRecord.UseState);
            if (equipmentUsageRecord.PostUseState.Equals("正常"))
            {
                values[5] = "√";
            }
            else
            {
                values[5] = "";
            }
            values[6] = StringUtils.null2Empty(equipmentUsageRecord.PostUseProblem);
            values[7] = StringUtils.null2Empty(equipmentUsageRecord.UsePerson);
            values[8] = StringUtils.null2Empty(equipmentUsageRecord.Remark);

            return values;
        }

        private string createExportFileDirectory(string day, string department, string person)
        {
            //构造目录
            string dayDirectory = Path.Combine(this.FileBasePath, day);
            string departmentDirectory = Path.Combine(dayDirectory, department);
            string personDirectory = Path.Combine(departmentDirectory, person);

            //创建目录
            this.createDirectory(dayDirectory);
            this.createDirectory(departmentDirectory);
            this.createDirectory(personDirectory);

            return personDirectory;
        }

        private void createDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                return;
            }
            Directory.CreateDirectory(directoryPath);
        }

        class EquipmentUasageRecordByDayDepPersonComparer : IEqualityComparer<EquipmentUsageRecordEntity>
        {
            public bool Equals(EquipmentUsageRecordEntity x, EquipmentUsageRecordEntity y)
            {
                string xDate = x.UseTime.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                string yDate = y.UseTime.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);

                return StringUtils.isEquals(xDate, yDate)
                    && StringUtils.isEquals(x.Department, y.Department)
                    && StringUtils.isEquals(x.UsePerson, y.UsePerson);
            }


            public int GetHashCode(EquipmentUsageRecordEntity obj) => obj.Department.GetHashCode();
        }

        class EquipmentUasageRecordByEquipmentComparer : IEqualityComparer<EquipmentUsageRecordEntity>
        {
            public bool Equals(EquipmentUsageRecordEntity x, EquipmentUsageRecordEntity y)
            {
                return StringUtils.isEquals(x.EquipmentCode, y.EquipmentCode)
                    && StringUtils.isEquals(x.EquipmentName, y.EquipmentName)
                    && StringUtils.isEquals(x.EquipmentType, y.EquipmentType);
            }


            public int GetHashCode(EquipmentUsageRecordEntity obj) => obj.EquipmentCode.GetHashCode();
        }
    }

   
    
}
