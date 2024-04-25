using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Utils;
using LabSystem.DAL;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TaskManager.domain.valueobject;

namespace TaskManager
{
    public partial class TableControl : UserControl
    {
        #region 自定义属性
        
        public bool SAVE_EDIT = true;

        [Description("右键菜单"), Category("自定义属性")]
        public ContextMenuStrip _Menu { get; set; }

        public FormTable FormTable { get; set; }

        public int FocusedRowHandle => _view.FocusedRowHandle;

        public DataRow GetDataRow(int hand)
        {
            return _view.GetDataRow(hand);
        }

        #region 公共参数

        public List<DataField> Fields = new List<DataField>();

        public DataControl sql = new DataControl();

        protected SqlConnection sqlConnection;

        protected SqlDataAdapter sqlAdapter;

        public DataTable DataSource;

        private static readonly int UPDATE_DATA_PER_COUNT = 3;

        //private readonly List<object> ControlClipboard = new List<object>();

        #endregion

        #endregion

        #region 自定义事件

        public delegate void BoolEventsHandle(object sender, bool value);

        public delegate bool BeforeDataSourceAddRowHandler(DataRow targetRow, DataRow sourceRow);

        [Description("在设置保存状态后触发"), Category("自定义事件")]
        public event BoolEventsHandle AfterSetSaveStatus;

        [Description("在设置保存状态后触发"), Category("自定义事件")]
        public BeforeDataSourceAddRowHandler BeforeAddRowOnImportExcel;

        [Description("新建"), Category("自定义事件")]
        public event EventHandler NewItemClick ;

        [Description("打开/编辑"), Category("自定义事件")]
        public event EventHandler OpenClick;

        public delegate void CellValueChangedEvent(DataRow changedRow);
        public delegate DataTable SaveDataSourceEvent(DataTable updateTable );
        public delegate void AfterSavedEvent();
        public delegate void ImportExcelPreHandler();

        public CellValueChangedEvent cellValueChangedEvent;
        public SaveDataSourceEvent saveDataSourceEvent;
        public AfterSavedEvent afterSavedHandle;

        [Description("导入Excel预处理"), Category("自定义事件")]
        public ImportExcelPreHandler importExcelPreHandler;

        #endregion

        #region 初始化和加载数据

        public TableControl()
        {
            InitializeComponent();
        }

        private void TableControl_Load(object sender, EventArgs e)
        {
            if(DesignMode)
                return;
            try
            {
                _control.SuspendLayout();
                
                Fields = sql.InitDataFields(FormTable);

                //设备信息增加序号显示           
                if (FormTable.Category.Equals("设备信息"))
                {
                    DataField orderField = new DataField("Order");
                    Fields.Insert(0,orderField);
                }

                InitColLayout();
                InitRepoCombox();

                _view.CellValueChanged += CellValueChanged;
                _view.RowDeleted += RowDeleted;
                _view.InitNewRow += ViewInitNewRowHandle;
                _view.KeyDown += ViewOnKeyDown;

                

                SetSaveStatus(true);

                #region 根据权限设置UI

                _view.OptionsBehavior.Editable = FormTable.Edit;
                _view.OptionsBehavior.ReadOnly = !FormTable.Edit;

                _view.OptionsBehavior.AllowAddRows = FormTable.Add ? DefaultBoolean.True : DefaultBoolean.False;
                _view.OptionsView.NewItemRowPosition = FormTable.Add ? NewItemRowPosition.Top : NewItemRowPosition.None;

                _view.OptionsBehavior.AllowDeleteRows = FormTable.Delete ? DefaultBoolean.True : DefaultBoolean.False;

                #endregion
                
                _control.ResumeLayout(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "初始化失败");
            }

     
            
        }

      
        /// <summary>
        /// 列的初始化
        /// </summary>
        private void InitColLayout()
        {
            _control.BeginUpdate();
            _view.BeginUpdate();

            _view.Columns.Clear();
         
            var cols = new List<GridColumn>();

            var fixColsName = new[] { "ID"};
            foreach (var col in fixColsName.Select(colName => new GridColumn
            {
                Name = colName,
                FieldName = colName
            }))
            {
                col.OptionsColumn.AllowEdit = false;
                col.Visible = false;
                col.OptionsColumn.AllowMove = false;
                col.OptionsColumn.ShowInCustomizationForm = false;
                
                //OptionsFilter.FilterPopupMode = FilterPopupMode.List;
                cols.Add(col);
            }

            for (var i = 0; i < Fields.Count; i++)
            {
                var col = new GridColumn
                {
                    Tag = Fields[i],
                    Name = Fields[i].Eng,
                    FieldName = Fields[i].Eng,
                    Caption = Fields[i].Chs,
                    VisibleIndex = i,
                    Visible = Fields[i].ColumnVisible,
                    Fixed = Fields[i].DisplayLevel == 0 ? FixedStyle.Left : FixedStyle.None
                };
                col.OptionsFilter.AllowFilter = true;
                col.OptionsFilter.AllowAutoFilter = true;
                col.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
                col.OptionsColumn.AllowEdit = Fields[i].AllowEdit;

                cols.Add(col);
            }
            
            _view.Columns.AddRange(cols.ToArray());

            if (_view.Columns.Contains(_view.Columns["consistent"]))
            {
               
                _view.Columns["consistent"].Visible=false;//是否一致字段隐藏
            }
          

            _view.EndUpdate();
            _control.EndUpdate();
        }

        /// <summary>
        /// 下拉编辑框定义
        /// </summary>
        private void InitRepoCombox()
        {
            repoDepartment.Items.AddRange(FormSignIn.UserDic.Keys);
            repoComboxUsers.Items.AddRange(FormSignIn.Users);

            repoReportType.Items.AddRange(Form1.ComboxDictionary["报告类型"]);
            repoNationalEnvironment.Items.AddRange(Form1.ComboxDictionary["国环视同"]);
            repoRemark.Items.AddRange(Form1.ComboxDictionary["任务备注"]);
            repoTypeBrief.Items.AddRange(Form1.ComboxDictionary["任务类型简称"]);
            repoProjectManager.Items.AddRange(Form1.ComboxDictionary["项目经理"]);
            repoProjectManagerPerson.Items.AddRange(Form1.ComboxDictionary["项目经理：个人备注"]);
            repoProjectManagerState.Items.AddRange(Form1.ComboxDictionary["项目经理：初校状态"]);
            
            
             
            repoProducer.Items.AddRange(Form1.ComboxDictionary["生产厂家"]);
            repoFuelType.Items.AddRange(Form1.ComboxDictionary["供油种类"]);
            repoTrans.Items.AddRange(Form1.ComboxDictionary["变速器形式"]);
            repoDriverType.Items.AddRange(Form1.ComboxDictionary["驱动形式"]);
            repoCheckUnit.Items.AddRange(Form1.ComboxDictionary["标准"]);
            repoDrum.Items.AddRange(Form1.ComboxDictionary["转鼓"]);
            repoState.Items.AddRange(Form1.ComboxDictionary["完成状态"]);
            
            repoDataState.Items.AddRange(Form1.ComboxDictionary["数据状态"]);
            repoEngineProducer.Items.AddRange(Form1.ComboxDictionary["发动机生产厂"]);
            repoTestSample.Items.AddRange(Form1.ComboxDictionary["试验项目"]);
            repoCarType.Items.AddRange(Form1.ComboxDictionary["车辆类型"]);
            repoEngineType.Items.AddRange(Form1.ComboxDictionary["动力类型"]);
            //repoCheckType.Items.AddRange(Form1.ComboxDictionary["检定方式"]);
            //repoEquipState.Items.AddRange(Form1.ComboxDictionary["设备状态"]);
            repoEquipClass.Items.AddRange(Form1.ComboxDictionary["情况等级"]);
            repoEmissionTimeLimit.Items.AddRange(Form1.ComboxDictionary["排放限值档"]);

            repoStandard.Items.AddRange(Form1.ComboxDictionary["标准阶段"]);
            repoNumber.Items.AddRange(Form1.ComboxDictionary["定位编号"]);
            repoVerification.Items.AddRange(Form1.ComboxDictionary["检定方式"]);
            repoInspection.Items.AddRange(Form1.ComboxDictionary["检验依据1"]);
            repoFueltype2.Items.AddRange(Form1.ComboxDictionary["燃料类型"]);
            repoFuellabel.Items.AddRange(Form1.ComboxDictionary["燃油标号"]);
            repoExperimentalLocation.Items.AddRange(Form1.ComboxDictionary["实验地点"]);
            repoYNKing.Items.AddRange(Form1.ComboxDictionary["是否报国环"]);

            repoYNDirect.Items.AddRange(Form1.ComboxDictionary["是否直喷"]);
            repoDataState2.Items.AddRange(Form1.ComboxDictionary["数据合格状态"]);
            repoItemBrief.Items.AddRange(Form1.ComboxDictionary["项目简称"]);
            repoItemType.Items.AddRange(Form1.ComboxDictionary["项目类型"]);
            repoItemName.Items.AddRange(Form1.ComboxDictionary["项目名称"]);
            repoSampleType.Items.AddRange(Form1.ComboxDictionary["样品类型"]);

            repo.Items.AddRange(Form1.ComboxDictionary["项目备注"]);

            repoItemCode.Items.AddRange(Form1.ComboxDictionary["项目代码"]);
            repoSingleTestMileage.Items.AddRange(Form1.ComboxDictionary["试验里程数"]);
            repoSinglePretreatmentMileage.Items.AddRange(Form1.ComboxDictionary["预处理里程数"]);
            repoPriceUnit.Items.AddRange(Form1.ComboxDictionary["价格单位"]);
            repoPrice.Items.AddRange(Form1.ComboxDictionary["单价"]);
            repoTestCycle.Items.AddRange(Form1.ComboxDictionary["测试周期"]);

            repoMoneySure.Items.AddRange(Form1.ComboxDictionary["费用确认"]);

            repoTirepressure.Items.AddRange(Form1.ComboxDictionary["胎压"]);

            repoChecksite.Items.AddRange(Form1.ComboxDictionary["检定地点"]);

            //设备状态
            repoEquipmentState.Items.AddRange(Form1.ComboxDictionary["设备状态"]);
            repoEquipmentUsageState.Items.AddRange(Form1.ComboxDictionary["设备使用状况"]);
            repoEquipmentManageState.Items.AddRange(ConstHolder.EQUIPMENT_STATE_CHN_NAMES);
            repoEquipmentTraceabilityState.Items.AddRange(ConstHolder.EQUIPMENT_TRACEABILITY_STATE_CHN_NAMES);

            //配置项类型
            repoConfigItemName.Items.AddRange(Form1.ComboxDictionary["配置项名称"]);

            foreach (GridColumn col in _view.Columns)
            {
                var field = DataField.GetFieldByEng(Fields, col.FieldName);
                if (field == null)
                    continue;

                if (field.Format.Equals("部门"))
                    col.ColumnEdit = repoDepartment;
                if (field.Format.Equals("用户"))
                    col.ColumnEdit = repoComboxUsers;
                else if (field.Format.Equals("日期"))
                    col.ColumnEdit = repoDate;
                else if (field.Format.Equals("日期时间"))
                    col.ColumnEdit = repoDatetime;
                else if (field.Format.Equals("是否"))
                    col.ColumnEdit = repoYesNo;
                else if (field.Format.Equals("生产厂家"))
                    col.ColumnEdit = repoProducer;
                else if (field.Format.Equals("供油种类"))
                    col.ColumnEdit = repoFuelType;
                else if (field.Format.Equals("变速器形式"))
                    col.ColumnEdit = repoTrans;
                else if (field.Format.Equals("驱动形式"))
                    col.ColumnEdit = repoDriverType;
                else if (field.Format.Equals("标准"))
                    col.ColumnEdit = repoCheckUnit;
                else if (field.Format.Equals("转鼓"))
                    col.ColumnEdit = repoDrum;
                else if (field.Format.Equals("完成状态"))
                    col.ColumnEdit = repoState;
                else if (field.Format.Equals("报告类型"))
                    col.ColumnEdit = repoReportType;
                else if (field.Format.Equals("数据状态"))
                    col.ColumnEdit = repoDataState;
                else if (field.Format.Equals("发动机生产厂"))
                    col.ColumnEdit = repoEngineProducer;
                else if (field.Format.Equals("试验项目"))
                    col.ColumnEdit = repoTestSample;
                else if (field.Format.Equals("动力类型"))
                    col.ColumnEdit = repoEngineType;
                else if (field.Format.Equals("车辆类型"))
                    col.ColumnEdit = repoCarType;
                //else if (field.Format.Equals("检定方式"))
                //    col.ColumnEdit = repoCheckType;
                else if (field.Format.Equals("情况等级"))
                    col.ColumnEdit = repoEquipClass;
                //else if (field.Format.Equals("设备状态"))
                //    col.ColumnEdit = repoEquipState;
                else if (field.Format.Equals("排放限值档"))
                    col.ColumnEdit = repoEmissionTimeLimit;
                else if (field.Format.Equals("标准阶段"))
                    col.ColumnEdit = repoStandard;
                else if (field.Format.Equals("定位编号"))
                    col.ColumnEdit = repoNumber;
                else if (field.Format.Equals("检定方式"))
                    col.ColumnEdit = repoVerification;
                else if (field.Format.Equals("检验依据1"))
                    col.ColumnEdit = repoInspection;
                else if (field.Format.Equals("燃料类型"))
                    col.ColumnEdit = repoFueltype2;
                else if (field.Format.Equals("燃油标号"))
                    col.ColumnEdit = repoFuellabel;
                else if (field.Format.Equals("实验地点"))
                    col.ColumnEdit = repoExperimentalLocation;
                else if (field.Format.Equals("是否报国环"))
                    col.ColumnEdit = repoYNKing;
                else if (field.Format.Equals("是否直喷"))
                    col.ColumnEdit = repoYNDirect;
                else if (field.Format.Equals("数据合格状态"))
                    col.ColumnEdit = repoDataState2;
                else if (field.Format.Equals("项目简称"))
                    col.ColumnEdit = repoItemBrief;
                else if (field.Format.Equals("项目类型"))
                    col.ColumnEdit = repoItemType;
                else if (field.Format.Equals("项目名称"))
                    col.ColumnEdit = repoItemName;
                else if (field.Format.Equals("样品类型"))
                    col.ColumnEdit = repoSampleType;
                else if (field.Format.Equals("国环视同"))
                    col.ColumnEdit = repoNationalEnvironment;
                else if (field.Format.Equals("任务备注"))
                    col.ColumnEdit = repoRemark;
                else if (field.Format.Equals("任务类型简称"))
                    col.ColumnEdit = repoTypeBrief;
                else if (field.Format.Equals("项目经理"))
                    col.ColumnEdit = repoProjectManager;
                else if (field.Format.Equals("项目经理：个人备注"))
                    col.ColumnEdit = repoProjectManagerPerson;
                else if (field.Format.Equals("项目经理：初校状态"))
                    col.ColumnEdit = repoProjectManagerState;
                else if (field.Format.Equals("项目备注"))
                    col.ColumnEdit = repo;

                else if (field.Format.Equals("项目代码"))
                    col.ColumnEdit = repoItemCode;
                else if (field.Format.Equals("试验里程数"))
                    col.ColumnEdit = repoSingleTestMileage;
                else if (field.Format.Equals("预处理里程数"))
                    col.ColumnEdit = repoSinglePretreatmentMileage;
                else if (field.Format.Equals("价格单位"))
                    col.ColumnEdit = repoPriceUnit;
                else if (field.Format.Equals("单价"))
                    col.ColumnEdit = repoPrice;
                else if (field.Format.Equals("测试周期"))
                    col.ColumnEdit = repoTestCycle;
                else if (field.Format.Equals("费用确认"))
                    col.ColumnEdit = repoMoneySure;
                else if (field.Format.Equals("胎压"))
                    col.ColumnEdit = repoTirepressure;
                else if (field.Format.Equals("检定地点"))
                    col.ColumnEdit = repoChecksite;
                else if (field.Format.Equals("设备状态"))
                    col.ColumnEdit = repoEquipmentState;
                else if (field.Format.Equals("设备使用状况"))
                    col.ColumnEdit = repoEquipmentUsageState;
                else if (field.Format.Equals("设备管理状态"))
                    col.ColumnEdit = repoEquipmentManageState;
                else if (field.Format.Equals("设备溯源状态"))
                    col.ColumnEdit = repoEquipmentTraceabilityState;
                else if (field.Format.Equals("配置项名称"))
                    col.ColumnEdit = repoConfigItemName;
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadSource()
        {
            var strsql = FormTable.GetSqlString(Year, Department, user, FinishState,startdate,enddate);

            #region Connection

            sqlConnection = new SqlConnection(sql.strCon);
            //设置select查询命令，SqlCommandBuilder要求至少有select命令 共计150毫秒
            var selectCMD = new SqlCommand(strsql, sqlConnection);
            DataSource = new DataTable();
            sqlAdapter = new SqlDataAdapter(selectCMD);
            sqlAdapter.Fill(DataSource);
            sqlConnection.Close();


            #endregion
            #region 加载数据

            _control.BeginUpdate();
            _view.BeginUpdate();

            _control.DataSource = DataSource;
            
            _view.EndUpdate();
            _control.EndUpdate();//加载数据共计250毫秒

            #endregion

            #region 调整列宽

            _view.OptionsView.BestFitMaxRowCount = 30;
            _view.BestFitColumns();
            _view.OptionsView.ColumnAutoWidth = false;//700毫秒

            if (_view.Columns.Contains(_view.Columns["Remark"]))
            {
                if (_view.Columns["Remark"].Width > 500)
                {
                    _view.Columns["Remark"].Width = 500;
                }
                
            }

            #endregion
        }

        #endregion

        #region 保存

        /// <summary>
        /// 点击保存按钮
        /// </summary>
        public void SaveClick()
        {
            if (!FormTable.Save)
            {
                MessageBox.Show("您没有保存权限");
                SetSaveStatus(true);
                return;
            }
     
            try
            {
                Form1.ShowWaitForm();               
                SaveSource();
                LoadSource();
                SetSaveStatus(true);
                if (afterSavedHandle != null)
                {
                    afterSavedHandle();
                }
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
            finally
            {
                Form1.CloseWaitForm();
            }

            //if (FormTable.Category == "试验统计")
            //{
            //    MessageBox.Show("保存成功！\r\n注意：旧数据的费用确认请在旧系统进行，请勿在新系统操作！");

            //}
        }

    
        #region 保存数据

        /// <summary>
        /// 保存数据
        /// </summary>
        public void SaveSource()
        {
            _view.FocusedRowHandle = -1;
            if (!FormTable.Save) return;

            if (sqlAdapter == null || DataSource == null)
                return;

            if (DataSource.GetChanges() == null)
                return;

            //二次处理
            var updateTable = DataSource.GetChanges();
            if (saveDataSourceEvent != null) {
                updateTable=saveDataSourceEvent(updateTable);
            }
            if (updateTable == null||updateTable.Rows.Count==0)
                return;

            try
            {
                if(sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
                // ReSharper disable once UnusedVariable
                var commandBuilder = new SqlCommandBuilder(sqlAdapter);//必须要有这一句
              
                //执行更新
                sqlAdapter.Update(updateTable ?? throw new InvalidOperationException());
                //使DataTable保存更新
                DataSource.AcceptChanges();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public void SaveSourceNew()
        {
            _view.FocusedRowHandle = -1;
            if (!FormTable.Save) return;

            if (sqlAdapter == null || DataSource == null)
                return;

            if (DataSource.GetChanges() == null)
                return;

            //二次处理
            var updateTable = DataSource.GetChanges();
            if (saveDataSourceEvent != null)
            {
                updateTable = saveDataSourceEvent(updateTable);
            }
            if (updateTable == null || updateTable.Rows.Count == 0)
                return;

            try
            {
                //执行更新
                executeUpdate(updateTable);
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
        }

        private void executeUpdate(DataTable updateTable) {
            int totalCount = updateTable.Rows.Count;
            if (totalCount <= UPDATE_DATA_PER_COUNT)
            {
                exeuteUpdateTable(updateTable);
                return;
            }

            //记录过多就拆分
            int startIndex = 0;
            int number = 1;
            while (startIndex < totalCount)
            {
                int curCount = (totalCount - startIndex) < UPDATE_DATA_PER_COUNT ?
                    (totalCount - startIndex) :
                    UPDATE_DATA_PER_COUNT;

                DataRow[] dataRows = new DataRow[curCount];
                int endIndex = startIndex + curCount;
                int curRowIndex = 0;
                for (int index = startIndex; index < endIndex; index++) {
                    dataRows[curRowIndex] = updateTable.Rows[index];
                    curRowIndex++;
                }
                exeuteUpdateTableRows(dataRows);
                 startIndex = startIndex + curCount;
                number++;
            }
        }

        private void exeuteUpdateTable(DataTable updateTable) {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
                // ReSharper disable once UnusedVariable
                var commandBuilder = new SqlCommandBuilder(sqlAdapter);//必须要有这一句

                //执行更新
                sqlAdapter.Update(updateTable ?? throw new InvalidOperationException());
                //使DataTable保存更新
                //DataSource.AcceptChanges();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
        }

        private void exeuteUpdateTableRows(DataRow[] dataRows)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
                // ReSharper disable once UnusedVariable
                var commandBuilder = new SqlCommandBuilder(sqlAdapter);//必须要有这一句

                //执行更新
                sqlAdapter.Update(dataRows);
                //使DataTable保存更新
                //DataSource.AcceptChanges();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
        }

        #endregion

        #region 提示

        public bool Want2Close()
        {
            string content = "有更改未保存!\n点击\n‘是(Y)’关闭窗口并保存\n‘否(N)’关闭窗口不保存\n‘取消’留在当前窗口";
            string title = FormTable.Category + "数据尚未保存！";

            bool WantToClose = SAVE_EDIT;

            if (SAVE_EDIT != true)
            {
                DialogResult result = MessageBox.Show(content, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    SaveClick();
                    WantToClose = true;
                }
                else if (result == DialogResult.No)
                {
                    WantToClose = true;
                }
                else
                {
                    WantToClose = false;
                }
            }

            return WantToClose;
        }
        
        #endregion

        #region 保存状态事件

        /// <summary>
        /// 设置保存状态,并触发事件
        /// </summary>
        /// <param name="value"></param>
        public void SetSaveStatus(bool value)
        {
            SAVE_EDIT = value;
            AfterSetSaveStatus?.Invoke(this, SAVE_EDIT);
        }

        #endregion

        #region 刷新

        public string Year;

        public string Group;

        public string Department = null;

        public string user;

        public int FinishState;

        public string startdate;
        public string enddate;
        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="year"></param>
        /// <param name="finish"></param>
        public void RefreshClick(string year, string startdate, string enddate, int finish = 1, string group =null,string user=null)
        {
            this.Year = year;
            this.FinishState = finish;
            this.Department = group;
            this.user = user;

            this.startdate = startdate;
            this.enddate = enddate;

            const string content = "有更改未保存!\n点击\n‘是(Y)’保存并刷新\n‘否(N)’不保存已修改的内容\n‘取消’留在当前窗口";
            const string title = "数据尚未保存！";

            if (SAVE_EDIT != true)
            {
                var result = MessageBox.Show(content, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if(result== DialogResult.Cancel)
                {
                    return;
                }
                if (result == DialogResult.Yes)

                {
                    SaveClick();
                    RefreshSource();
                }
                else if (result == DialogResult.No)
                {
                    RefreshSource();
                }
            }
            else
            {
                RefreshSource();
            }
        }

        public void RefreshSource()
        {
            try
            {
                Form1.ShowWaitForm();
                LoadSource();
                SetSaveStatus(true);
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
            finally
            {
                Form1.CloseWaitForm();
                _view.ClearSorting();
            }
        }

        #endregion

        #endregion

        #region UI效果|右键菜单|设为未保存
        
        /// <summary>
        /// 快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers != Keys.Control) return;

            if (e.KeyCode == Keys.Delete)
                DeleteSelectedRows();
            else if (e.KeyCode == Keys.A)
                _view.SelectAll();
            else if (e.KeyCode == Keys.C)
                CopySelectedRows();
            else if (e.KeyCode == Keys.V)
                PasteSelectedRows();
            else if (e.KeyCode == Keys.N)
                NewItemClick?.Invoke(sender, e);
            else if (e.KeyCode == Keys.O)
                OpenClick?.Invoke(sender, e);
        }

        private void ViewMouseDown(object sender, MouseEventArgs e)
        {
            var hitInfo = _view.CalcHitInfo(new Point(e.X, e.Y));
            if (((e.Button & MouseButtons.Right) == 0) || !hitInfo.InRow || _view.IsGroupRow(hitInfo.RowHandle)) return;
            // switching focus
            _view.FocusedRowHandle = hitInfo.RowHandle;
            _view.FocusedColumn = hitInfo.Column;
            // showing the custom context menu�
            if (hitInfo.Column != null)
            {
                _Menu?.Show(_control, hitInfo.HitPoint);
            }
        }

        /// <summary>
        /// 编辑表格完成后设置状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (!FormTable.Edit)
                return;

            SetSaveStatus(false);

            var dr = _view.GetDataRow(e.RowHandle);

            if (dr == null || dr.RowState == DataRowState.Deleted) return;

            if (!(e.Column.Tag is DataField field)) return;

            #region 日期

            if (field.Format == "日期")
            {
                if (DateTime.TryParse(e.Value as string, out var time))
                    dr[field.Eng] = time.ToString("yyyy/MM/dd", Form1.DTFormat);
            }

            //触发更改时间
            if (this.cellValueChangedEvent != null) {
                this.cellValueChangedEvent(dr);
            }

            #endregion
        }

        /// <summary>
        /// 删除选中行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDeleted(object sender, RowDeletedEventArgs e)
        {
            SetSaveStatus(false);
        }

        #endregion

        #region 编辑|修正数据格式

        /// <summary>
        /// 新增行初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewInitNewRowHandle(object sender, InitNewRowEventArgs e)
        {
            FormTable.InitNewRow(_view, e.RowHandle);
        }

        /// <summary>
        /// 删除选中行
        /// </summary>
        public void DeleteSelectedRows()
        {
            if (FormTable.Delete)
                _view.DeleteSelectedRows();
            else
                MessageBox.Show("您没有删除的权限");
        }

        /// <summary>
        /// 修正日期与数字格式
        /// </summary>
        public void CorrectDate()
        {
            if (!FormTable.Edit)
                return;

            try
            {
                Form1.ShowWaitForm();

                foreach (DataRow dr in DataSource.Rows)
                {
                    foreach (var field in Fields)
                    {
                        //if (!field.Format.Equals("日期")) continue;
                        //if (!DataSource.Columns.Contains(field.Eng)) continue;
                        if(field.Format.Equals("日期")&& DataSource.Columns.Contains(field.Eng))
                        {
                            if (dr[field.Eng].ToString() != "")
                            {
                                dr[field.Eng].ToDate(out var date);
                                dr[field.Eng] = date.ToString("yyyy/MM/dd");
                            }
                        }
                        else if (field.Format.Equals("数字") && DataSource.Columns.Contains(field.Eng))
                        {
                            if (dr[field.Eng].ToString().Contains("."))
                            {
                                dr[field.Eng] = Math.Round(double.Parse(dr[field.Eng].ToString()), 2);
                            }
                        }


                    }
                }

                SetSaveStatus(false);
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
            finally
            {
                Form1.CloseWaitForm();
            }
            
        }

        #endregion

        #region 导入导出

        public void Import()
        {
            #region 容错

            if (!FormTable.Add)
                return;

            var fileDialog = new OpenFileDialog
            {
                Multiselect = false, 
                Title = "请选择源文件", 
                Filter = "EXCEL文件(*.xlsx)|*.xlsx"
            };
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            #endregion

            Form1.ShowWaitForm();
            try
            {
                var tempDt = new DataTable();

                #region 读取Excel

                using (var stream = new FileStream(fileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                     var book = new XSSFWorkbook(stream);
                    var sheet = book.GetSheetAt(0);
                    if (sheet == null)
                        return;

                    var hearRow = sheet.GetRow(2);
                    var colCount = GetColCount(hearRow);
                    for (var i = 0; i < colCount; i++)
                    {
                        var col = GetCellValue(hearRow, i);
                        if (string.IsNullOrWhiteSpace(col))
                            continue;
                        tempDt.Columns.Add(col, typeof(string));
                    }

                    var rowCount = sheet.LastRowNum + 1;

                    for (var rowIndex = 5; rowIndex < rowCount; rowIndex++)
                    {
                        var row = sheet.GetRow(rowIndex);
                        var items = new List<object>();

                        for (var colIndex = 0; colIndex < colCount; colIndex++)
                        {
                            items.Add(GetCellValue(row, colIndex));
                        }

                        tempDt.Rows.Add(items.ToArray());
                    }

                }

                #endregion

                importExcelPreHandler?.Invoke();

                #region 增加到DataSource

                DataSource.BeginLoadData();
                foreach (DataRow sDr in tempDt.Rows)
                {
                    var row = DataSource.NewRow();

                    foreach (DataColumn column in tempDt.Columns)
                    {
                        
                        sDr.CopyValue(row, column.ColumnName);
                        if (column.ColumnName == "RegistrationDate")
                        {
                            row["RegistrationDate"] = $"{DateTime.Now:yyyy/MM/dd HH:mm}";
                        }
                    }

                    bool isAdd = true;
                    if (BeforeAddRowOnImportExcel != null) {
                        isAdd= BeforeAddRowOnImportExcel(row, sDr);
                    }
                    if (isAdd) {
                        DataSource.Rows.Add(row);
                    }
                }

                DataSource.EndLoadData();

                #endregion
            }
            catch (Exception ex)
            {
                Log.e($"Import {ex}");
            }
            finally
            {
                Form1.CloseWaitForm();
                SetSaveStatus(false);
                MessageBox.Show("导入成功");
            }
        }

        public int GetColCount(IRow row)
        {
            var value = 0;
            var colCount = row.Cells.Count;
            for (var i = 0; i < colCount; i++)
            {
                var cell = row.GetCell(i);
                if (cell == null)
                    continue;
                if(string.IsNullOrWhiteSpace(cell.ToString()))
                    continue;

                value++;
            }

            return value;
        }

        /// <summary>
        /// 导出当前
        /// </summary>
        public void Export()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "导出Excel",
                Filter = "EXCEL文件(*.xlsx)|*.xlsx",
                FileName = FormTable.FormTitle + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")
            };
            var dialogResult = saveFileDialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK) 
                return;
            var options = new DevExpress.XtraPrinting.XlsxExportOptions
            {
                ExportMode = DevExpress.XtraPrinting.XlsxExportMode.SingleFile
            };
            //_control.ExportToXls(saveFileDialog.FileName);
            _control.ExportToXlsx(saveFileDialog.FileName, options);
            MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 导出空白模板
        /// </summary>
        public void ExportTemplate()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "导出Excel",
                Filter = "EXCEL文件(*.xlsx)|*.xlsx",
                FileName = FormTable.Category + "模板"
            };
            var dialogResult = saveFileDialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
                return;

            var dt = sql
                .ExecuteQuery($"select IncludeDepts,ExcludeDepts,eng,chs,remark from FieldDefinitionTable where category='{FormTable.Category}' order by tableIndex")
                .Tables[0];

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show($"没有{FormTable.Category}的表格定义");
                return;
            }

            Form1.ShowWaitForm();
            try
            {
                IWorkbook book = new XSSFWorkbook();
                var sheet = book.CreateSheet("使用统计");
                var includeRow = sheet.CreateRow(0);
                var excludeRow = sheet.CreateRow(1);
                var engRow = sheet.CreateRow(2);
                var chsRow = sheet.CreateRow(3);
                var remarkRow = sheet.CreateRow(4);

                var col = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    var includes = dr[0].ToString().Trim();
                    var excludes = dr[1].ToString().Trim();
                    var eng = dr[2].ToString().Trim();
                    var chs = dr[3].ToString().Trim();
                    var remark = dr[4].ToString().Trim();

                    var cell0 = includeRow.CreateCell(col);
                    cell0.SetCellValue(includes);

                    var cell01 = excludeRow.CreateCell(col);
                    cell01.SetCellValue(excludes);

                    var cell1 = engRow.CreateCell(col);
                    cell1.SetCellValue(eng);

                    var cell2 = chsRow.CreateCell(col);
                    cell2.SetCellValue(chs);

                    var cell3 = remarkRow.CreateCell(col);
                    cell3.SetCellValue(remark);

                    col++;
                }
                
                using (var fs = File.OpenWrite(saveFileDialog.FileName))
                {
                    book.Write(fs);
                }

                MessageBox.Show("模板导出成功，请从第6行开始录入数据。前5行为字段定义");
            }
            catch (Exception e)
            {
                Log.e(e.ToString());
            }
            finally
            {
                Form1.CloseWaitForm();
            }
            
        }

        public static string GetCellValue(IRow row, int i)
        {
            var cell = row.GetCell(i);

            if (cell != null && row.RowNum>2)
            {
                if (cell.CellType == NPOI.SS.UserModel.CellType.Numeric)
                {
                    if (HSSFDateUtil.IsCellDateFormatted(cell))
                    {
                        if (Convert.ToInt32(cell.DateCellValue.Year)<1902)
                        {
                            return cell.DateCellValue.ToString("HH:mm:ss");
                        }
                        else
                        {
                            return cell.DateCellValue.ToString("yyyy/MM/dd");
                        }
                       
                    }                   
                    else
                    {
                        return cell == null ? "" : cell.NumericCellValue.ToString();
                    }
                }
                else if (cell.CellType == NPOI.SS.UserModel.CellType.Formula)
                {
                    cell.SetCellType(CellType.String);
                    return cell.StringCellValue.ToString();
                }
                else
                {
                    return cell == null ? "" : cell.ToString();
                }          
            }
            else
            {
                return cell == null ? "" : cell.ToString();
            }
            
            #region 老版方法
            //var cell = row.GetCell(i);

            //if (row.LastCellNum > 40 && cell != null && cell.ToString() != "" && row.RowNum > 2)
            //{
            //    if (i == 5 || i == 7)
            //    {
            //        if (row.GetCell(i).ToString().Contains(":"))
            //        {
            //            return cell == null ? "" : cell.ToString();
            //        }
            //        else
            //        {
            //            var time = ToDateTimeValue(row.GetCell(i).ToString());
            //            //string[] date = time.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);           
            //            return time;
            //        }

            //    }
            //    else
            //    {
            //        return cell == null ? "" : cell.ToString();
            //    }

            //}
            //else
            //{
            //    return cell == null ? "" : cell.ToString();
            //}
            #endregion

        }

        /// <summary>
        /// 数字转日期
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        private static string ToDateTimeValue(string strNumber)
        {
            if (!string.IsNullOrWhiteSpace(strNumber))
            {
                Decimal tempValue;
                //先检查 是不是数字;
                if (Decimal.TryParse(strNumber, out tempValue))
                {
                    //天数,取整
                    int day = Convert.ToInt32(Math.Truncate(tempValue));
                    //这里也不知道为什么. 如果是小于32,则减1,否则减2
                    //日期从1900-01-01开始累加 
                    // day = day < 32 ? day - 1 : day - 2;
                    DateTime dt = new DateTime(1900, 1, 1).AddDays(day < 32 ? (day - 1) : (day - 2));

                    //小时:减掉天数,这个数字转换小时:(* 24) 
                    Decimal hourTemp = (tempValue - day) * 24;//获取小时数
                                                              //取整.小时数
                    int hour = Convert.ToInt32(Math.Truncate(hourTemp));
                    //分钟:减掉小时,( * 60)
                    //这里舍入,否则取值会有1分钟误差.
                    Decimal minuteTemp = Math.Round((hourTemp - hour) * 60, 2);//获取分钟数
                    int minute = Convert.ToInt32(Math.Truncate(minuteTemp));
                    //秒:减掉分钟,( * 60)
                    //这里舍入,否则取值会有1秒误差.
                    Decimal secondTemp = Math.Round((minuteTemp - minute) * 60, 2);//获取秒数
                    int second = Convert.ToInt32(Math.Truncate(secondTemp));

                    //时间格式:00:00:00
                    string resultTimes = string.Format("{0}:{1}:{2}",
                            (hour < 10 ? ("0" + hour) : hour.ToString()),
                            (minute < 10 ? ("0" + minute) : minute.ToString()),
                            (second < 10 ? ("0" + second) : second.ToString()));

                    if (day > 0)
                        return string.Format("{0},{1}", dt.ToString("yyyy/MM/dd"), resultTimes);
                    else
                        return resultTimes;
                }
            }
            return string.Empty;
        }
        #endregion

        #region 复制粘贴

        public void CopySelectedRows()
        {
            try
            {
                Copy.ControlClipboard.Clear();
                Clipboard.Clear();
                var handles = _view.GetSelectedRows();
                foreach (var dr in handles.Select(handle => _view.GetDataRow(handle)))
                {
                    Copy.ControlClipboard.Add(dr.ItemArray);
                }
            }
            catch (Exception e)
            {
                Log.e($"CopySelectedRows {e}");
            }
            
        }

        public void PasteSelectedRows()
        {
            Clipboard.Clear();

            if (Copy.ControlClipboard.Count == 0) return;
            
            for (var i = Copy.ControlClipboard.Count - 1; i > -1; i--)
            {
                if (!(Copy.ControlClipboard[i] is object[] objs))
                    continue;

                var dr = DataSource.NewRow();

                if (dr.ItemArray.Length != objs.Length)
                    continue;

                try
                {
                    dr.ItemArray = objs;

                    dr["ID"] = -1;
                    foreach (var col in FormTable.IdCols)
                    {
                        if (string.IsNullOrWhiteSpace(col))
                            continue;
                        dr[col] = "";
                    }

                    var stateFields = DataField.GetFieldByFormat(Fields, "完成状态");
                    foreach (var field in stateFields)
                    {
                        if(field.Eng!= "Records")
                            dr[field.Eng] = "未完成";
                    }

                    foreach(var item in Fields)
                    {
                        if (item.Eng == "VIN")
                        {
                            dr["VIN"] = "";
                        }
                        if(item.Eng== "CreatePeople")
                        {
                            dr["CreatePeople"] = FormSignIn.CurrentUser.Name;
                        }
                        if (item.Eng == "MoneySure")
                        {
                            dr["MoneySure"] = "";
                        }
                        if (item.Eng == "Taskcode")
                        {
                            if (dr["Taskcode"].ToString().Contains('?'))
                            {
                                dr["Taskcode"] = "";
                                //string sqlmoneysure = $"select count(*) from TestStatistic where Taskcode='{dr["Taskcode"].ToString()}' and MoneySure ='是'";
                                //if (SqlHelper.GetList(sqlmoneysure).Rows[0][0].ToString() != "0" && !dr["Taskcode"].ToString().Contains("?"))
                                //{
                                //    GetControlByFieldName("Taskcode").SetValue(taskcode0 + "?");

                                //}

                                //string sqlcartype = $"select count(*) from NewTaskTable where Taskcode='{taskcode0}' and Model ='{SampleModel0}'";
                                //if (SqlHelper.GetList(sqlcartype).Rows[0][0].ToString() == "0" && !taskcode0.Contains("?"))
                                //{
                                //    GetControlByFieldName("Taskcode").SetValue(taskcode0 + "?");
                                //    MessageBox.Show("该任务单号与车型不一致，请重新填写！");
                                //    return;
                                //}
                            }
                            
                        }
                        if (item.Eng == "RegistrationDate")
                        {
                            dr["RegistrationDate"] = $"{DateTime.Now:yyyy/MM/dd HH:mm}";
                        }

                        //var taskcode0 = GetControlByFieldName("Taskcode").Value().Trim();
                        //var SampleModel0 = GetControlByFieldName("SampleModel").Value().Trim();
                        //if (taskcode0 != "")
                        //{
                        //    string sqlmoneysure = $"select count(*) from TestStatistic where Taskcode='{taskcode0}' and MoneySure ='是'";
                        //    if (SqlHelper.GetList(sqlmoneysure).Rows[0][0].ToString() != "0" && !taskcode0.Contains("?"))
                        //    {
                        //        GetControlByFieldName("Taskcode").SetValue(taskcode0 + "?");
                        //       
                        //    }

                        //    string sqlcartype = $"select count(*) from NewTaskTable where Taskcode='{taskcode0}' and Model ='{SampleModel0}'";
                        //    if (SqlHelper.GetList(sqlcartype).Rows[0][0].ToString() == "0" && !taskcode0.Contains("?"))
                        //    {
                        //        GetControlByFieldName("Taskcode").SetValue(taskcode0 + "?");
                        //        MessageBox.Show("该任务单号与车型不一致，请重新填写！");
                        //        return;
                        //    }
                        //}

                    }
                    //if (Fields.Contains())
                    //{

                    //}
                    //var stateFields1 = DataField.GetFieldByChs(Fields, "VIN");
                    //foreach (var field in stateFields1)
                    //{
                    //    if (field.Eng != "Records")
                    //        dr[field.Eng] = "未完成";
                    //}

                    DataSource.Rows.InsertAt(dr, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "粘贴失败");
                }

            }

            SetSaveStatus(false);

            Copy.ControlClipboard.Clear();
            
        }

        #endregion
  

        #region 自定义列显示

        private void _view_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.MenuType != GridMenuType.Column) 
                return;

            if (!(e.Menu is GridViewColumnMenu menu)) 
                return;

            //for (var i = menu.Items.Count - 1; i >= 0; i--)
            //{
            //    if (!menu.Items[i].Tag.Equals(GridStringId.MenuColumnRemoveColumn) &&
            //        !menu.Items[i].Tag.Equals(GridStringId.MenuColumnColumnCustomization) &&
            //        !menu.Items[i].Tag.Equals(GridStringId.MenuColumnBestFit) &&
            //        !menu.Items[i].Tag.Equals(GridStringId.MenuColumnBestFitAllColumns))
            //    {
            //        menu.Items.Remove(menu.Items[i]);
            //    }
            //}

            if (menu.Column == null) return;
            //1-固定
            menu.Items.Add
            (menu.Column.Fixed == FixedStyle.None
                ? CreateCheckItem("固定此列", menu.Column, FixedStyle.Left, null)
                : CreateCheckItem("取消固定", menu.Column, FixedStyle.None, null));
        }
        
        /// <summary>
        /// Create a menu item
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="column"></param>
        /// <param name="style"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        private static DXMenuCheckItem CreateCheckItem(string caption, GridColumn column, FixedStyle style, Image image)
        {
            var item = new DXMenuCheckItem(caption, column.Fixed == style, image, OnFixedClick)
            {
                Tag = new MenuInfo(column, style)
            };
            return item;
        }
                
        /// <summary>
        /// Menu item click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnFixedClick(object sender, EventArgs e)
        {
            if (!(sender is DXMenuItem item)) return;
            if (!(item.Tag is MenuInfo info)) return;
            info.Column.Fixed = info.Style;
        }
        
        /// <summary>
        /// The class that stores menu specific information
        /// </summary>
        private class MenuInfo
        {
            public MenuInfo(GridColumn column, FixedStyle style)
            {
                this.Column = column;
                this.Style = style;
            }
            public readonly FixedStyle Style;
            public readonly GridColumn Column;
        }


        #endregion

        private void _view_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {

        }

        public void reLoadCombox()
        {
            repoDepartment.Items.Clear();
            repoComboxUsers.Items.Clear();

            repoReportType.Items.Clear();
            repoNationalEnvironment.Items.Clear(); ;
            repoRemark.Items.Clear(); ;
            repoTypeBrief.Items.Clear(); ;
            repoProjectManager.Items.Clear(); ;
            repoProjectManagerPerson.Items.Clear(); ;
            repoProjectManagerState.Items.Clear(); ;

            
            repoProducer.Items.Clear();
            repoFuelType.Items.Clear();
            repoTrans.Items.Clear();
            repoDriverType.Items.Clear();
            repoCheckUnit.Items.Clear();
            repoDrum.Items.Clear();
            repoState.Items.Clear();
           
            repoDataState.Items.Clear();
            repoEngineProducer.Items.Clear();
            repoTestSample.Items.Clear();
            repoCarType.Items.Clear();
            repoEngineType.Items.Clear();
            //repoCheckType.Items.AddRange(Form1.ComboxDictionary["检定方式"]);
            //repoEquipState.Items.Clear();
            repoEquipClass.Items.Clear();
            repoEmissionTimeLimit.Items.Clear();

            repoStandard.Items.Clear();
            repoNumber.Items.Clear();
            repoVerification.Items.Clear();
            repoInspection.Items.Clear();
            repoFueltype2.Items.Clear();
            repoFuellabel.Items.Clear();
            repoExperimentalLocation.Items.Clear();
            repoYNKing.Items.Clear();

            repoYNDirect.Items.Clear();
            repoDataState2.Items.Clear();
            repoItemBrief.Items.Clear();
            repoItemType.Items.Clear();
            repoItemName.Items.Clear();
            repoSampleType.Items.Clear();


            repo.Items.Clear();

            repoItemCode.Items.Clear();
            repoSingleTestMileage.Items.Clear();
            repoSinglePretreatmentMileage.Items.Clear();
            repoPriceUnit.Items.Clear();
            repoPrice.Items.Clear();
            repoTestCycle.Items.Clear();
            repoMoneySure.Items.Clear();
            repoTirepressure.Items.Clear();

            repoChecksite.Items.Clear();
            InitRepoCombox();
        }

        private void _control_Enter(object sender, EventArgs e)
        {

          
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private int index =5; 
        private void _view_MouseWheel(object sender, MouseEventArgs e)
        {
         
          
            //if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            //{
            //    _view.MoveBy(_view.FocusedRowHandle);
            //    if (e.Delta < 0)
            //    {

            //        if (index >= _view.Columns.Count - 1)
            //        {
            //            _view.MoveFirst();
            //            return;
            //        }
            //        index++;
            //        _view.FocusedColumn = _view.Columns[index];
            //    }
            //    else if (e.Delta > 0)
            //    {

            //        if (index <= 0)
            //        {

            //            return;
            //        }
            //        index--;
            //        _view.FocusedColumn = _view.Columns[index];
            //    }
            //}



        }
    }
}