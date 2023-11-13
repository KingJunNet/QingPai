using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class ImportData : Form
    {
        public string TableName;

        public List<DataField> Fields;

        public DataTable DataSource;

        public string ViewName;

        public string DateColName;

        public ImportData(string tableName, IEnumerable<DataField> fields, DataTable dataSource)
        {
            InitializeComponent();

            if (DesignMode)
                return;

            dateEdit2.DateTime = DateTime.Now;
            dateEdit1.DateTime = DateTime.Now.AddDays(-7);

            TableName = tableName;
            DataSource = dataSource;
            Fields = tableName.Equals("整车")
                ? fields.Where(f => !string.IsNullOrWhiteSpace(f.VehicleMap)).ToList()
                : fields.Where(f => !string.IsNullOrWhiteSpace(f.CanisterMap)).ToList();

            DateColName = tableName.Equals("整车")
                ? "preStartDate"
                : "testDate";

            ViewName = tableName.Equals("整车")
                ? "VehicleView"
                : "CanisterView";

            InitListView();
            LoadData();
        }

        public void InitListView()
        {
            listView1.BeginUpdate();
            listView1.Columns.Clear();

            var colID = new ColumnHeader()
            {
                Name = "ID",
                Text = "ID",
                Width = 100
            };
            listView1.Columns.Add(colID);
            foreach (var field in Fields)
            {
                var col = new ColumnHeader()
                {
                    Name = field.Eng,
                    Text = field.Chs,
                    Width = 100
                };
                listView1.Columns.Add(col);
            }

            listView1.EndUpdate();
        }

        public void LoadData()
        {
            var strWhere = new List<string>();
            if (dateEdit1.EditValue != null && dateEdit2.EditValue != null)
            {
                strWhere.Add($"(ISDATE({DateColName})=1 " +
                             $"and CONVERT(date,{DateColName})>='{dateEdit1.DateTime:yyyy/MM/dd}' " +
                             $"and CONVERT(date,{DateColName})<='{dateEdit2.DateTime:yyyy/MM/dd}')");
            }

            if (checkBox1.Checked)
            {
                strWhere.Add(
                    $"ID not in (select orvrid  from TaskTable where VehicleOrCanister ='{TableName}')");
            }

            var strsql = $"select * from {ViewName} ";
            for (var i = 0; i < strWhere.Count; i++)
            {
                if (i == 0)
                    strsql += $" where {strWhere[i]}";
                else
                    strsql+= $" and {strWhere[i]}";
            }

            var sql = new DataControl();
            var dt = sql.ExecuteQuery(strsql).Tables[0];

            listView1.BeginUpdate();
            var items = new List<ListViewItem>();
            foreach (DataRow dr in dt.Rows)
            {
                var item = new ListViewItem(dr["ID"].ToString());
                for (var i = 0; i < Fields.Count; i++)
                {
                    item.SubItems.Add(TableName.Equals("整车")
                        ? dr[Fields[i].VehicleMap].ToString()
                        : dr[Fields[i].CanisterMap].ToString());
                }
                items.Add(item);
            }
            listView1.Items.Clear();
            listView1.Items.AddRange(items.ToArray());
            listView1.EndUpdate();

            labelCount.Text = $"数据量:{items.Count}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSource.BeginLoadData();
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                var row = DataSource.NewRow();

                row["department"] = "蒸发组";
                row["userName"] = FormSignIn.CurrentUser.Name;
                row["Finishstate"] = comboBox1.Text.Trim();
                row["VehicleOrCanister"] = TableName;
                row["IsRead"] = "是";

                if (!int.TryParse(item.SubItems[listView1.Columns["ID"].Index].Text, out var id))
                    continue;

                row["ORVRID"] = id;
                for (var i = 0; i < Fields.Count; i++)
                {
                    var value = item.SubItems[listView1.Columns[Fields[i].Eng].Index].Text;
                    try
                    {
                        row[Fields[i].Eng] = value;
                    }
                    catch
                    {
                        row[Fields[i].Eng] = DBNull.Value;
                    }
                        
                }

                DataSource.Rows.Add(row);
            }

            DataSource.EndLoadData();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
