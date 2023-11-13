using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ExpertLib.Controls;

namespace TaskManager
{
    /// <summary>
    /// 数据字段类 继承自ExpertLib.Controls.Field
    /// </summary>
    /// 开发说明：如何在生产环境添加列
    /// 杨尧-2017-08-28
    /// 注意：一定要****先备份数据库*****
    /// 情况：在使用过程中，用户希望增加一列名叫[A列],英文名叫[ACOL]
    /// 步骤：
    /// Step1. SQL2008 找到对应的表T（例如：VehicleDataTable/CanisterDataTable/SimulationDataTable...）手动增加字段名叫[ACOL]
    /// Step2. 找到对应的分类[CATEGORY](例如：整车/炭罐/..) SQL2008 编辑表FieldDefintionTable 删除category=[CATEGORY]的内容
    /// Step3. 打开“字段定义.xlxs”文件，添加该列
    /// Step4. 把category=[CATEGORY]导入到SQL2008数据库中
    [Serializable]
    public class DataField : Field
    {
        public readonly List<string> IncludeDepts;
        public readonly List<string> ExcludeDepts;
        public readonly int X = 1;
        public readonly int Y = 1;

        public readonly string VehicleMap = "";
        public readonly string CanisterMap = "";

        /// <summary>
        ///  字段类型：
        /// 日期\数字\是否\用户
        /// </summary>
        public readonly string Format;

        /// <summary>
        /// 字段是否在表格中可见
        /// </summary>
        public bool ColumnVisible;

        /// <summary>
        /// 字段是否在编辑对话框中可见
        /// </summary>
        public bool EditorVisible;

        public readonly bool AllowEdit;

        public DataField(DataRow dr, FormTable table)
        {
            Chs = dr["chs"].ToString();
            Eng = dr["eng"].ToString();
            Category = dr["category"].ToString();
            Group = !Convert.IsDBNull(dr["groupName"]) ? dr["groupName"].ToString() : "";
            Format = !Convert.IsDBNull(dr["format"]) ? dr["format"].ToString() : "";
            Remark = !Convert.IsDBNull(dr["remark"]) ? dr["remark"].ToString() : "";

            int.TryParse(dr["displayLevel"].ToString(), out var level);
            int.TryParse(dr["tableIndex"].ToString(), out var index);

            DisplayLevel = level;
            DisplayIndex = index;

            if (!Convert.IsDBNull(dr["X"]))
                int.TryParse(dr["X"].ToString(), out X);
            if (!Convert.IsDBNull(dr["Y"]))
                int.TryParse(dr["Y"].ToString(), out Y);
            if (!Convert.IsDBNull(dr["VehicleMap"]))
                VehicleMap = dr["VehicleMap"].ToString();
            if (!Convert.IsDBNull(dr["CanisterMap"]))
                CanisterMap = dr["CanisterMap"].ToString();

            IncludeDepts = !Convert.IsDBNull(dr["IncludeDepts"])
                ? GetList(dr["IncludeDepts"].ToString())
                : new List<string>();

            ExcludeDepts = !Convert.IsDBNull(dr["ExcludeDepts"])
                ? GetList(dr["ExcludeDepts"].ToString())
                : new List<string>();

            ColumnVisible = GetColumnVisible(table, this);
            EditorVisible = GetEditorVisible(table, this);

            if (table.ReadOnlyCols.Contains(Eng))
                AllowEdit = false;
            else if (table.EditableCols.Contains(Eng))
                AllowEdit = ColumnVisible;
            else
                AllowEdit = table.DefaultEditable && ColumnVisible;
        }

        public static List<string> GetList(string s)
        {
            var list = new List<string>();
            var items = s.Replace("，", ",").Split(',').ToList();
            foreach (var item in items)
            {
                if(string.IsNullOrWhiteSpace(item))
                    continue;

                list.Add(item.Trim());
            }

            return list.Distinct().ToList();
        }

        public static bool GetEditorVisible(FormTable table, DataField field)
        {
            // 排除显示: Table目标部门和字段排除部门有交叉
            var exclude = table.TargetDepts.Intersect(field.ExcludeDepts).Any();

            // 包含显示
            var include = table.OnlyPublicField
                // 如果Table只显示公共字段, 那么只有字段的包含部门==null的字段可以显示
                ? !field.IncludeDepts.Any()
                // 如果Table不止显示公共字段, 只要Table目标部门==null 或者 字段的包含部门==null 或者 两者有交叉就可以显示
                : !table.TargetDepts.Any() ||
                  !field.IncludeDepts.Any() ||
                  table.TargetDepts.Intersect(field.IncludeDepts).Any();

            //var displayLevel = field.DisplayLevel <= 1;

            return include && !exclude;
        }

        public static bool GetColumnVisible(FormTable table, DataField field)
        {
            return GetEditorVisible(table, field) && field.DisplayLevel <= 1;
        }

        public static DataField GetFieldByEng(List<DataField> fields,string eng)
        {
            foreach (var f in fields)
            {
                if (f.Eng.Equals(eng))
                {
                    return f;
                }
            }

            return null;
        }

        public static DataField GetFieldByChs(List<DataField> fields, string chs)
        {
            foreach (DataField f in fields)
            {
                if (f.Chs.Equals(chs))
                {
                    return f;
                }
            }

            return null;
        }

        public static List<DataField> GetFieldByFormat(List<DataField> fields, string format)
        {
            var values = new List<DataField>();
            foreach (var f in fields)
            {
                if (f.Format.Equals(format))
                    values.Add(f);
            }

            return values;
        }

    }
}
