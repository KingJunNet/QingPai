using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Utils;
using LabSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManager.common.utils;

namespace TaskManager.编辑数据
{
    public partial class StructureDialog : Form
    {
        public StructureDialog()
        {
            InitializeComponent();

        }
        private string type;

        private readonly GridView Grid;

        private readonly int ID;

        private readonly int Hand;

        public StructureDialog(GridView grid, int hand)
        {

            InitializeComponent();



            Hand = hand;
            Grid = grid;
            if (hand < 0)
            {
                button1.Text = "确定";
            }
            else
            {
                button1.Text = "更新";
            }

        }

        /// <summary>
        /// 新增信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            AddStructure();
        }

        private void AddStructure()
        {
            if (Hand < 0)
            {
                AddUser();
                string sql = $"insert into UserStructure(Unit,Section,Office,Experimentsite,Group1,Locationnumber,Username) values('{Unit.Text}','{Section.Text}','{Office.Text}','{Experimentsite.Text}','{Group1.Text}','{Locationnumber.Text}','{Username.Text}')";

                if (SqlHelper.ExecuteNonquery(sql, CommandType.Text) > 0)
                {
                    this.Close();
                    DialogResult = DialogResult.OK;
                }

            }
            else
            {
                Grid.SetRowCellValue(Hand, "Unit", Unit.Text);
                Grid.SetRowCellValue(Hand, "Section", Section.Text);
                Grid.SetRowCellValue(Hand, "Office", Office.Text);
                Grid.SetRowCellValue(Hand, "Experimentsite", Experimentsite.Text);
                Grid.SetRowCellValue(Hand, "Group1", Group1.Text);
                Grid.SetRowCellValue(Hand, "Locationnumber", Locationnumber.Text);
                Grid.SetRowCellValue(Hand, "Username", Username.Text);

                this.Close();
                DialogResult = DialogResult.OK;
            }
        }

        private void AddUser()
        {
            string userName = Username.Text.Trim();
            bool userExists = CheckUsernameExists(userName);
            if (userExists)
            {
                return;
            }

            string firstLetteText = userName.GetFirstPinyin();
            string passwordText = "Catarc@123";
            string roleText = "普通成员";
            string selectSql = $"insert into UserTable(userID, firstLetter, userName, password, company, section, office, department,role) valus ('000','{firstLetteText}','{userName}','{passwordText}','{Unit.Text}','{Section.Text}','{Office.Text}','{Office.Text}','{roleText}')";
            SqlHelper.ExecuteNonquery(selectSql, CommandType.Text);
        }

        private bool CheckUsernameExists(string username)
        {
            string sql = "SELECT COUNT(*)FROM UserTable WHERE Username = @Username";
            SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("Username", DbHelper.ValueOrDBNullIfNull(username))
                };
            int count = (int)SqlHelper.ExcuteScalar(sql, parameters);
            return count > 0;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            DialogResult = DialogResult.No;
        }

        private void StructureDialog_Load(object sender, EventArgs e)
        {


            //用户名下拉框
            string sql = $"select userName  from UserTable";
            DataTable da = SqlHelper.GetList(sql);
            List<string> list = new List<string>();
            foreach (DataRow row in da.Rows)
            {
                list.Add(row[0].ToString());
            }
            Username.SetItems(list);


            //试验地点与定位编号下拉框
            string sql1 = $"select Experimentsite,Locationnumber,Unit,Section,Office from UserStructure";
            DataTable da1 = SqlHelper.GetList(sql1);
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            List<string> list4 = new List<string>();
            List<string> list5 = new List<string>();

            foreach (DataRow row in da1.Rows)
            {
                if (!list1.Contains(row[0].ToString()))
                {
                    list1.Add(row[0].ToString());
                }
                if (!list2.Contains(row[1].ToString()))
                {
                    list2.Add(row[1].ToString());
                }
                if (!list3.Contains(row[2].ToString()))
                {
                    list3.Add(row[2].ToString());
                }
                if (!list4.Contains(row[3].ToString()))
                {
                    list4.Add(row[3].ToString());
                }
                if (!list5.Contains(row[4].ToString()))
                {
                    list5.Add(row[4].ToString());
                }

            }
            Experimentsite.SetItems(list1);
            Locationnumber.SetItems(list2);
            Unit.SetItems(list3);
            Section.SetItems(list4);
            Office.SetItems(list5);
            Group1.SetItems(FormSignIn.UserDic.Keys);

            ///事件注册
            Username.SetTextChange(UsernameHandle);
            UsernameHandle(null, null);
            if (Hand >= 0)
            {
                Username.SetValue(Grid.GetRowCellValue(Hand, "Username").ToString());
                Unit.SetValue(Grid.GetRowCellValue(Hand, "Unit").ToString());
                Section.SetValue(Grid.GetRowCellValue(Hand, "Section").ToString());
                Office.SetValue(Grid.GetRowCellValue(Hand, "Office").ToString());
                Group1.SetValue(Grid.GetRowCellValue(Hand, "Group1").ToString());
                Experimentsite.SetValue(Grid.GetRowCellValue(Hand, "Experimentsite").ToString());
                Locationnumber.SetValue(Grid.GetRowCellValue(Hand, "Locationnumber").ToString());

            }
            // Unit.SetValue("55");

        }

        private void UsernameHandle(object sender, EventArgs e)
        {

            string sql = $"select company,section,office,department  from UserTable where Username = '{Username.Text}'";
            DataTable da = SqlHelper.GetList(sql);
            if (da.Rows.Count > 0)
            {
                Unit.SetValue(da.Rows[0]["company"].ToString());
                Section.SetValue(da.Rows[0]["section"].ToString());
                Office.SetValue(da.Rows[0]["office"].ToString());
                Group1.SetValue(da.Rows[0]["department"].ToString());
            }
        }
    }
}
