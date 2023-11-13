using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using ExpertLib.Controls;

namespace TaskManager
{
    public partial class TaskPreview : UserControl
    {
        public string Title;

        public string Cols;

        public DataControl Sql = new DataControl();

        public Dictionary<string,int> ColIndex = new Dictionary<string, int>();

        public int TotalCount { get; set; }

        public TaskPreview()
        {
            InitializeComponent();
            TotalCount = 0;

            Cols = "ID,department,ExperimentalSite,LocationNumber,Registrant,CarType" +
                               ",ItemBrief,TestStartDate,SampleModel,Producer,Carvin,YNDirect,PowerType,TransmissionType,EngineModel,EngineProduct,StandardStage,FuelType,FuelLabel,ProjectPrice";

            var items = Cols.Split(',');
            var i = 0;
            foreach (var col in items)
            {
                ColIndex.Add(col, i++);
            }
        }

        public void InitView(string VIN)
        {
            var strsql = $"select {Cols} from TestStatistic where LTRIM(RTrim(Carvin)) = '{VIN}' " +
                  "and Carvin is not null and Carvin<>'' ";

            var sql = new DataControl();
            var dt = sql.ExecuteQuery(strsql).Tables[0];

            TotalCount = dt.Rows.Count;
            var finishTaskCount = 0;
            //foreach (DataRow row in dt.Rows)
            //{
            //    if (!row["Finishstate"].ToString().Trim().Equals("已完成")) 
            //        continue;
            //    finishTaskCount++;
            //}

            Title = $"试验任务已完成: {finishTaskCount}/{TotalCount}";

            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                var item = new ListViewItem(row["ID"].ToString());
                foreach (var col in ColIndex)
                {
                    if (col.Key.Equals("ID"))
                        continue;

                    item.SubItems.Add(dt.Columns[col.Value].DataType == typeof(DateTime)
                        ? $"{row[col.Value]:yyyy/MM/dd hh:mm}"
                        : row[col.Value].ToString());
                }

                listView1.Items.Add(item);

                //var sRead = row["IsRead"].ToString();
                //var sState = row["Finishstate"].ToString();

                //var read = sRead.Equals("是");
                //var state = sState.Equals("已完成");

                //if (read && !state)
                //    item.BackColor = Color.Khaki;
                //else if (!read && !state)
                //    item.BackColor = Color.Chocolate;
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listView1.EndUpdate();
        }

        public void SetValue2Control(ListViewItem item, TitleControl control)
        {
            var fieldName = control.Tag.ToString();
            if (string.IsNullOrWhiteSpace(fieldName))
                return;
            if (!ColIndex.ContainsKey(fieldName))
                return;

            var index = ColIndex[fieldName];
            var value = item.SubItems[index].Text.Trim();

            control.SetValue(value);
        }

        public void SetItem(ListViewItem item, TitleControl control)
        {
            var fieldName = control.Tag.ToString();
            if (string.IsNullOrWhiteSpace(fieldName))
                return;
            if (!ColIndex.ContainsKey(fieldName))
                return;
            if (fieldName.Equals("ID", StringComparison.CurrentCultureIgnoreCase))
                return;

            var index = ColIndex[fieldName];
            var value = control.Value();

            item.SubItems[index].Text = value;
        }

        public void UpdateItem(ListViewItem item)
        {
            if (item == null)
                return;

            var id = item.SubItems[0].Text.Trim();
            if (string.IsNullOrWhiteSpace(id))
                return;

            var update = "";
            var parameters = new List<SqlParameter>();

            foreach (var colIndex in ColIndex)
            {
                var colName = colIndex.Key;

                if (colName.Equals("ID", StringComparison.CurrentCultureIgnoreCase))
                    continue;

                var index = ColIndex[colName];
                var value = item.SubItems[index].Text.Trim();

                if (!string.IsNullOrWhiteSpace(update))
                    update += ",";
                update += $" {colName}=@{colName} ";
                parameters.Add(new SqlParameter(colName, value));
            }

            var strsql = $"update TestStatistic set {update}  where ID = {id}";

            Sql.ExecuteNonQuery(strsql, parameters.ToArray());
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
