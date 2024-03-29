using System;
using System.Collections.Generic;
using DevExpress.XtraGrid.Views.Grid;

namespace TaskManager
{
    public enum FormType
    {       
        Equipment = 401,        // 设备管理
        EquipmentDepartment = 402,
        EquipmentUsageRecord = 403,


        Safe =601,

        NewTask=701,       //任务管理
        Sample =801,       //样品信息
        Test = 802,       //试验统计
        Project =803      //项目报价
    };

    public class FormTable
    {
        #region 成员

        #region 字段定义

        public FormType Type;

        /// <summary>
        /// SQL 表名称
        /// </summary>
        public string TableName;

        /// <summary>
        /// 窗体名称
        /// </summary>
        public string FormTitle;

        /// <summary>
        /// 用于FieldDefinitionTable中定义的各个字段的中文名等数据
        /// </summary>
        public string Category;

        public string DateCol;

        public string OrderCol;

        public string StateCol;

        public string MoneySureCol;

        /// <summary>
        /// 除了ID以外的唯一标识字段; 用于复制粘贴的时候
        /// </summary>
        public List<string> IdCols = new List<string>();

        public string ReminderCol;

        #endregion

        #region 分部门定义

        /// <summary>
        /// 是否只显示Field.Department=null的字段。用来控制表格字段的显示状态
        /// </summary>
        public readonly bool OnlyPublicField;

        /// <summary>
        /// 用于FormType.DepartmentTask
        /// </summary>
        public readonly string Department;

        /// <summary>
        /// 配合DataField的IncludeDepts和ExcludeDepts来控制DataField显示状态
        /// </summary>
        public readonly List<string> TargetDepts = new List<string>();

        #endregion

        #region 权限

        /// <summary>
        /// 权限所需的Module, 为空表示任意组别都可以
        /// </summary>
        public string Module;

        /// <summary>
        /// 权限名称, 为空表示不需要权限
        /// </summary>
        public string Operate;

        /// <summary>
        /// 是否可以新增数据
        /// </summary>
        public bool Add = true;

        /// <summary>
        /// 编辑
        /// </summary>
        public bool Edit = true;

        /// <summary>
        /// 是否可以删除数据
        /// </summary>
        public bool Delete = true;

        public bool Save => Edit || Add || Delete;

        #endregion

        #region 某列的编辑性

        public bool DefaultEditable = true; // 默认每列都可以编辑
        public List<string> ReadOnlyCols = new List<string>();
        public List<string> EditableCols = new List<string>();

        #endregion

        #endregion

        public FormTable(FormType formType, string department, params string[] targetDepts)
        {
            Type = formType;
            Department = department;
            TargetDepts.AddRange(targetDepts);

            
            var sql = new DataControl();

            //if ((int)formType < 500 && (int)formType >= 400)
            //{
            //    TableName = "EquipmentTable";
            //    Category = "设备管理";
            //    DateCol = "CheckEndDate";
            //    OrderCol = "EquipState";

                
            //    StateCol = "";
            //    ReminderCol = "";
            //    ReadOnlyCols = new List<string> { "CheckEndDate" };
            //}
            if ((int)formType < 500 && (int)formType >= 400)
            {
                TableName = "NewEquipmentTable";
                Category = "设备信息";
                DateCol = "ExpireDate";
                OrderCol = "GroupName";


                StateCol = "";
                ReminderCol = "";
                ReadOnlyCols = new List<string> { };
            }
            if (formType == FormType.Equipment)
            {
                FormTitle = "设备管理";
                Module = "设备管理";

                //Operate = "设备管理";
                Add = true;
                Edit = true;
                Delete = true;
            }

            if (formType == FormType.EquipmentUsageRecord)
            {
                TableName = "EquipmentUsageRecordTable";
                Category = "设备使用记录";
                FormTitle = "设备使用记录";
                Module = "设备使用记录";
                StateCol = "";
                ReminderCol = "";
                DateCol = "UseTime";
                OrderCol = "UseTime";

                ReadOnlyCols = new List<string> { "EquipmentCode",
                      "EquipmentName",
                      "EquipmentType",
                      "UsePerson",
                      "UseTime",
                      "Purpose",                                                     
                       "Department",
                      "LocationNumber",                    
                      "ItemBrief",                    
                       "SampleModel",
                       "Producer",
                       "CarVin"
                       };

                Add = true;
                Edit = true;
                Delete = true;
            }

            //     else if (formType == FormType.EquipmentDepartment)
            //     {
            //         FormTitle = $"{Department}设备管理";
            //         Module = Department;
            //         Operate = "设备管理";
            //         Add = true;
            //         Edit = true;
            //         Delete = true;

            //         ReadOnlyCols.Add("department");
            //     }
            // }
            // //安全管理
            // else if((int)formType == 601)
            // {
            //     TableName = "SafeTable";
            //     Category = "安全管理";
            //     FormTitle = "安全管理";
            //     Module = "试验组综合管理";
            //     Operate = "安全管理";
            //     DateCol = "RegistrationTime";
            //     OrderCol = "RegistrationTime";

            //     Add = true;
            //     Edit = true;
            //     Delete = true;
            // }
            //任务管理
            if ((int)formType == 701)
            {
                TableName = "NewTaskTable";
                Category = "任务管理";
                FormTitle = "任务管理";
                Module = "任务管理";
                StateCol = "State";
                FormTitle = "任务管理";
                //Operate = "全功能";
                DateCol = "RegistrationDate";
                OrderCol = "RegistrationDate";

                ReadOnlyCols.Add("consistent");

                Add = true;
                Edit = true;
                Delete = true;
            }
            //样品信息
            else if ((int)formType == 801)
            {
                TableName = "SampleTable";
                Category = "样品信息";
                FormTitle = "样品信息";
                Module = "样品信息";
                FormTitle = "样品信息";
                //Operate = "全功能";
                OrderCol = "UpdateDate";
                ReadOnlyCols.Add("consistent");
                //ReadOnlyCols.Add("VIN");
                Add = true;
                Edit = true;
                Delete = true;
            }
            //试验统计
            else if((int)formType == 802)
            {
                TableName = "TestStatistic";
                Category = "试验统计";
                FormTitle = "试验统计";
                Module = "试验统计";
                DateCol = "TestStartDate";
                FormTitle = "试验统计";
                OrderCol = "TestStartDate";
                //Operate = "全功能";

                //ReadOnlyCols = new List<string> { "Carvin", "ItemBrief", "Taskcode", "Equipments"};
                //ReadOnlyCols = new List<string> { "Carvin", "ItemBrief", "Taskcode", "Equipments",
                //"CarType","SampleModel","Producer","YNDirect","PowerType","TransmissionType",
                //"EngineModel","EngineProduct","Drivertype","FuelType","FuelLabel"};

                Add = true;
                Edit = true;
                Delete = true;
            }
            //项目报价
            else if ((int)formType == 803)
            {
                TableName = "ProjectQuotation";
                Category = "项目报价";
                FormTitle = "项目报价";
                Module = "项目报价";
                FormTitle = "项目报价";
                StateCol = "Finishstate";
                OrderCol = "ItemBrief";
                //Operate = "全功能";

                Add = true;
                Edit = true;
                Delete = true;
            }
           

            var userName = FormSignIn.CurrentUser.Name;

            Add &= sql.AuthorityCheck2(Module, Operate, userName);
            Edit &= sql.AuthorityCheck2(Module, Operate, userName);
            Delete &= sql.AuthorityCheck2(Module, Operate, userName);

            if((int) formType == 601){
                Add = true;
                Edit = true;
                Delete = true;
            }
        }

        #region strSql

        public string GetSqlString(string year, string department, string searchUserName, int finishState,string startdate, string enddate)
        {
            var strsql = "select * from " + TableName;
            List<string> sWhere;

            if (Type == FormType.NewTask)
                sWhere = NewTaskSqlString(year, finishState, startdate, enddate);
            else if (Type == FormType.Sample)
                sWhere = SampleSqlString(year);
            else if (Type == FormType.Test)
                sWhere = TestSqlString(year, department);
            else if (Type == FormType.Project)
                sWhere = ProjectSqlString(year);
            else if (Type == FormType.Equipment)
            {
                strsql = "select ROW_NUMBER() OVER(ORDER BY GroupName ASC) as [Order],* from NewEquipmentTable";
                sWhere = EquipmentSqlString(year);
            }
            else if (Type == FormType.EquipmentUsageRecord)
                sWhere = EquipmentUsageRecordSqlString(year,department,searchUserName,startdate,enddate);
            else
                throw new Exception("GetSqlString no form type");

            if (sWhere.Count > 0)
            {
                strsql += $" where {sWhere[0]}";
                for (var i = 1; i < sWhere.Count; i++)
                    strsql += $" and {sWhere[i]}";
            }

          
            if (!string.IsNullOrWhiteSpace(OrderCol))
                strsql += $" order by {OrderCol} desc";
            
            return strsql;
        }

        private List<string> EquipmentSqlString(string department)
        {
            var sWhere = new List<string>();

            //if (!string.IsNullOrWhiteSpace(department))
            //    sWhere.Add($" department='{department}'");

            return sWhere;
        }
        

        private List<string> SafeSqlString(string year)
        {
            var sWhere = new List<string>();
            if (!string.IsNullOrWhiteSpace(year))
                sWhere.Add($" (YEAR({DateCol})= {year} or {DateCol} IS NULL) ");

            return sWhere;
        }

        private List<string> NewTaskSqlString(string year,int finishState,string startdate,string enddate)
        {
            var sWhere = new List<string>();
            //if (!string.IsNullOrWhiteSpace(year))
            //    sWhere.Add($" (YEAR({DateCol})= {year} or {DateCol} IS NULL) ");
            if (!string.IsNullOrWhiteSpace(startdate) && !string.IsNullOrWhiteSpace(enddate))
                sWhere.Add($" ({DateCol}>='{startdate}' and {DateCol}<='{enddate}')");

          
            //if (finishState == 1)
            //{
            //    sWhere.Add($" (State ='已完成')");
            //}else if (finishState == -1)
            //{
            //    sWhere.Add($" (State ='未完成')");
            //}
                
            return sWhere;
        }


        private List<string> SampleSqlString(string year)
        {
            var sWhere = new List<string>();

            return sWhere;
        }

        private List<string> TestSqlString(string year,string department)
        {
            var sWhere = new List<string>();

            if (!string.IsNullOrWhiteSpace(year))
                sWhere.Add($" (YEAR({DateCol})= {year} or {DateCol} IS NULL) ");
            if (!string.IsNullOrWhiteSpace(department) && department!="所有组别") {
                sWhere.Add($"department ='{department}'");
            }
                

            return sWhere;
        }

        private List<string> EquipmentUsageRecordSqlString(string year, string department,string user,string startdate, string enddate)
        {
            var sWhere = new List<string>();

            sWhere.Add("(TestState='已完成' or TestTaskId is null)");

            if (!string.IsNullOrWhiteSpace(year) && year != "所有项目")
            {
                sWhere.Add($"ItemBrief ='{year}'");
            }
            if (!string.IsNullOrWhiteSpace(department) && department != "所有组别")
            {
                sWhere.Add($"Department ='{department}'");
            }
            if (!string.IsNullOrWhiteSpace(user) && user != "所有人")
            {
                sWhere.Add($"UsePerson ='{user}'");
            }
            if (!string.IsNullOrWhiteSpace(startdate) && !string.IsNullOrWhiteSpace(enddate)) {
                sWhere.Add($" ({DateCol}>='{startdate}' and {DateCol}<='{enddate}')");
            }
                
            return sWhere;
        }

        private List<string> ProjectSqlString(string year)
        {
            var sWhere = new List<string>();

            return sWhere;
        }
        #endregion

        #region InitNewRow

        public void InitNewRow(GridView view, int hand)
        {
            view.SetRowCellValue(hand, "ID", -1);
            view.SetRowCellValue(hand, "userName", FormSignIn.CurrentUser.Name);

            if (!string.IsNullOrWhiteSpace(DateCol))
                view.SetRowCellValue(hand, DateCol, $"{DateTime.Now.Date:yyyy/MM/dd}");
            if (!string.IsNullOrWhiteSpace(StateCol))
                view.SetRowCellValue(hand, StateCol, "未完成");
            if (!string.IsNullOrWhiteSpace(ReminderCol))
                view.SetRowCellValue(hand, ReminderCol, "否");
            if (!string.IsNullOrWhiteSpace(MoneySureCol))
                view.SetRowCellValue(hand, MoneySureCol, "否");

           if(Type ==  FormType.Equipment || Type ==  FormType.EquipmentDepartment)
            {
                view.SetRowCellValue(hand, "CheckDate", $"{DateTime.Now.Date:yyyy/MM/dd}");
                view.SetRowCellValue(hand, "Period", 1.0);
                view.SetRowCellValue(hand, "CheckEndDate", $"{DateTime.Now.Date.AddMonths(12).AddDays(-1):yyyy/MM/dd}");
            }

            if (Type ==FormType.Test)
            {

                view.SetRowCellValue(hand, "Finishstate", "未完成");
                view.SetRowCellValue(hand, "MoneySure", "否");
            }
           

        }

        #endregion
    }
}
