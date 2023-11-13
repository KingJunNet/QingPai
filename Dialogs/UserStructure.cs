using System;
using System.Data;
//using ExpertLib.Utils;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Utils;
using TaskManager.编辑数据;

namespace TaskManager
{
    public partial class UserStructure : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private UserGridControl1 userContrl;
        private string title = "组织结构";
        public bool IsSaved;


        public UserStructure()
        {
            InitializeComponent();
            IsSaved = true;
            userContrl = new UserGridControl1(gridControl1, gridView1);
            
            var sql = new DataControl();
            gridView1.OptionsBehavior.Editable = false;
            var authority = sql.AuthorityCheck2("系统维护", "用户管理", FormSignIn.CurrentUser.Name);
            if (FormSignIn.CurrentUser.Role.ToString() != "超级管理员")
            {
                barButtonItem3.Enabled = false;
                barButtonItem5.Enabled = false;
                //gridView1.OptionsBehavior.Editable = false;
            }
            else
            {
                //gridView1.OptionsBehavior.Editable = true;
            }
        }

        private void FormUser_Shown(object sender, EventArgs e)
        {
            userContrl.ShowData();
            IsSaved = true;
            Text = title;
            
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            userContrl.ShowData();
            IsSaved = true;
            Text = title;
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            userContrl.Save();
            userContrl.ShowData();
            IsSaved = true;
            Text = title;
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridView1.DeleteSelectedRows();
            IsSaved = false;
            Text = "未保存-" + title;
        }

        private void ribbon_Click(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// 新增信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {

            StructureDialog structureDialog = new StructureDialog(gridView1, -1);
            structureDialog.ShowDialog();
            if (structureDialog.DialogResult == DialogResult.OK)
            {
                
                userContrl.ShowData();
                IsSaved = true;
                Text = title;
            }
            
               
        }

        private void UserStructure_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView1.SelectedRowsCount == 1)
            {
                int row = Convert.ToInt32(gridView1.GetSelectedRows()[0]);
                StructureDialog structureDialog = new StructureDialog(gridView1, row);
                structureDialog.ShowDialog();
                if (structureDialog.DialogResult == DialogResult.OK)
                {
                    gridView1.FocusedRowHandle = -1;
                    userContrl.Save();
                    userContrl.ShowData();
                    IsSaved = true;
                    Text = title;
                }

            }
        }
    }

    public class UserGridControl1 : IDisposable
    {
        private DataControl sql = new DataControl();
        private GridControl _control;
        private GridView _view;
        private DataTable DataSource;
        private SqlConnection sqlConnection;
        private SqlDataAdapter sqlAdapter;

        public UserGridControl1(GridControl gridControl, GridView gridView)
        {
            _control = gridControl;
            _view = gridView;
            _view.InitNewRow += InitNewRow;
            _view.ValidateRow += ValidateRow;
            _view.InvalidRowException += InvalidRowException;

            _view.CellValueChanged += CellValueChanged;
            _view.CellMerge += CellMerge;

            _view.OptionsBehavior.Editable = false;

        }

        private void CellMerge(object sender, CellMergeEventArgs e)
        {
            if(e.Column.FieldName== "Username" ||e.Column.FieldName == "Group1" || e.Column.FieldName == "Locationnumber")
            {
                e.Merge = false;
                
                e.Handled = true;
            }
        }

        public void Dispose()
        {
            DataSource?.Dispose();
            if(sqlConnection!=null)
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            sqlAdapter?.Dispose();
        }

        public void ShowData()
        {
            const string strsql = "select * from UserStructure order by Unit,Section,Office,Experimentsite,Group1,Username";
            sqlConnection = new SqlConnection(sql.strCon);

            var selectCMD = new SqlCommand(strsql, sqlConnection);
            DataSource = new DataTable();
            sqlAdapter = new SqlDataAdapter(selectCMD);
            sqlAdapter.Fill(DataSource);

            DataSource = sql.ExecuteQuery(strsql).Tables[0];
            _control.DataSource = DataSource;

            sqlConnection.Close();
        }

        public void Save()
        {
            if (sqlAdapter == null || DataSource?.GetChanges() == null)
                return;
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();

                var commandBuilder = new SqlCommandBuilder(sqlAdapter);
                var table = DataSource.GetChanges();
                //执行更新
                //sqlAdapter.UpdateCommand = 
                sqlAdapter.Update(table ?? throw new InvalidOperationException());
                //使DataTable保存更新
                DataSource.AcceptChanges();
            }
            catch (Exception ex)
            {
                Log.e($"Form User Save {ex}");
            }
        }

        private void CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            //if (!e.Column.FieldName.Equals("userName", StringComparison.CurrentCultureIgnoreCase))
            //    return;
            //var value = e.Value.ToString().Trim();
            //if (value == "") return;
            //var pinyin = value.GetFirstPinyin();
            //_view.SetRowCellValue(e.RowHandle, _view.Columns["firstLetter"], pinyin);
        }

        private static void InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            
        }

        private void ValidateRow(object sender, ValidateRowEventArgs e)
        {
            try
            {
                //if (e.RowHandle < -1)
                //    return;

                if (e.Row == null)
                    return;

                #region 检查账号

                var colUserID = _view.Columns["userID"];
                var userID = _view.GetRowCellValue(e.RowHandle, colUserID).ToString();
                if (userID.Trim() == "")
                {
                    e.Valid = false;
                    _view.SetColumnError(colUserID, "账号不能为空！");
                }
                else
                {
                    for (var i = 0; i < _view.RowCount; i++)
                    {
                        if (i == e.RowHandle) continue;
                        else if (_view.GetRowCellValue(i, colUserID) == null) continue;
                        var value = _view.GetRowCellValue(i, colUserID).ToString();
                        if (!value.Equals(userID)) 
                            continue;
                        e.Valid = false;
                        _view.SetColumnError(colUserID, "账号重复！");
                    }
                }

                #endregion

                #region 检查用户名

                var colUserName = _view.Columns["userName"];
                var userName = _view.GetRowCellValue(e.RowHandle, colUserName).ToString();
                if (userName.Trim() == "")
                {
                    e.Valid = false;
                    _view.SetColumnError(colUserName, "用户名不能为空！");
                }
                //else if (!userName.IsAllChinese())
                //{
                //    e.Valid = false;
                //    _view.SetColumnError(colUserName, "用户名必须为汉字！");
                //}
                else
                {
                    for (var i = 0; i < _view.RowCount; i++)
                    {
                        if (i == e.RowHandle) continue;
                        else if (_view.GetRowCellValue(i, colUserName) == null) continue;
                        var value = _view.GetRowCellValue(i, colUserName).ToString();
                        if (!value.Equals(userName))
                            continue;
                        e.Valid = false;
                        _view.SetColumnError(colUserName, "用户名重复！");
                    }
                }

                #endregion
            
                #region 检查密码

                var colPwd = _view.Columns["password"];
                var password = _view.GetRowCellValue(e.RowHandle, colPwd).ToString();
                if (password.Trim() == "")
                {
                    e.Valid = false;
                    _view.SetColumnError(colPwd, "密码不能为空！");
                }

                #endregion
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
        }

        private void InitNewRow(object sender, InitNewRowEventArgs e)
        {
            //_view.SetRowCellValue(e.RowHandle, _view.Columns["Unit"], "中汽研汽车检验中心");
            //_view.SetRowCellValue(e.RowHandle, _view.Columns["Section"], "轻型排放节能试验研究部");
            //_view.SetRowCellValue(e.RowHandle, _view.Columns["Office"], "轻型排放室");
            //_view.SetRowCellValue(e.RowHandle, _view.Columns["Experimentsite"], "天津19号试验楼");
            //_view.SetRowCellValue(e.RowHandle, _view.Columns["Group1"], "耐久组");
            //_view.SetRowCellValue(e.RowHandle, _view.Columns["Username"], "雷立刚");
        }
                
    }
}