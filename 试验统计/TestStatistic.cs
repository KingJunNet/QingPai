using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using LabSystem.DAL;
using TaskManager.common.utils;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
using TaskManager.domain.valueobject;
using TaskManager.infrastructure.db;
using Xfrog.Net;

namespace TaskManager
{
    public partial class TestStatistic : BaseForm
    {
        private CreateTaskForm createTaskForm;

        private IEquipmentUsageRecordRepository equipmentUsageRecordRepository;

        private List<int> savedIds = new List<int>();

        private List<int> removedIds = new List<int>();

        private static readonly List<string> READ_ONLY_COLUMNS = new List<string> { "Carvin", "ItemBrief", "Taskcode", "Equipments",
                "CarType","SampleModel","Producer","YNDirect","PowerType","TransmissionType",
                "EngineModel","EngineProduct","Drivertype","FuelType","FuelLabel",
              "RegistrationDate","question", "MoneySure"};

        public TestStatistic()
        {
            InitializeComponent();
        }

        public TestStatistic(FormType formType, string selectedDept) : base(formType, selectedDept)
        {
            InitializeComponent();
            this.equipmentUsageRecordRepository = new EquipmentUsageRecordRepository();
            this._control.cellValueChangedEvent = new TableControl.CellValueChangedEvent(afterCellValueChanged);
            this._control.saveDataSourceEvent = new TableControl.SaveDataSourceEvent(handleBeforeSaveDataSource);
            this.beforeRemovedHandle = new BeforeRemovedHandle(beforeRemovedHandler);
        }

        private void afterCellValueChanged(DataRow changedRow) {
            int id=int.Parse(changedRow["ID"].ToString());
            if (this.savedIds.Contains(id)) {
                this.savedIds.Remove(id);
            }
        }

        private DataTable handleBeforeSaveDataSource(DataTable updateTable) {
            DataRowCollection rows = updateTable.Rows;

            List<int> deletedRowIndexs = new List<int>();
            List<EquipmentUsageRecordTestPart> updatedTestParts = new List<EquipmentUsageRecordTestPart>();
            for (int index = 0; index < rows.Count; index++)
            {
                DataRow row = rows[index];
                if (row.RowState == DataRowState.Deleted) {
                    continue;
                }

                int id = int.Parse(row["ID"].ToString().Trim());
                if (this.savedIds.Contains(id))
                {
                    deletedRowIndexs.Add(index);
                }
                else {
                    EquipmentUsageRecordTestPart testPart = this.dataRow2EquipmentUsageRecordTestPart(row);
                    updatedTestParts.Add(testPart);
                }
            }

            //移除已经更新的数据行
            deletedRowIndexs.ForEach(index => rows.RemoveAt(index));

            //更新设备记录
            if (!Collections.isEmpty(updatedTestParts)) {
                updatedTestParts.ForEach (item => {
                    this.equipmentUsageRecordRepository.updateTestTaskProperty(item);
                 }) ;
            }

            //删除设备使用记录
            if (!Collections.isEmpty(this.removedIds))
            {
                this.removedIds.ForEach(id =>
                {
                    this.equipmentUsageRecordRepository.removeByTestTaskId(id);
                });
                this.removedIds.Clear();
            }

                return updateTable;
        }

        private void beforeRemovedHandler()
        {
            DialogResult result = MessageBox.Show("是否需要一并删除该试验关联的设备使用记录", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            for (int i = 0; i < _control._view.SelectedRowsCount; i++)
            {
                string id = _control._view.GetRowCellValue(_control._view.GetSelectedRows()[i], "ID")?.ToString().Trim();
                this.removedIds.Add(int.Parse(id));
            }
        }

            private EquipmentUsageRecordTestPart dataRow2EquipmentUsageRecordTestPart(DataRow row)
        {
            EquipmentUsageRecordTestPart result = new EquipmentUsageRecordTestPart();

            result.TestTaskId = int.Parse(row["ID"].ToString().Trim());
            result.Department = this.dataColumn2String(row["department"]);
            result.LocationNumber = this.dataColumn2String(row["LocationNumber"]);
            result.Registrant = this.dataColumn2String(row["Registrant"]);
            result.ItemBrief = this.dataColumn2String(row["ItemBrief"]);
            if (!(row["TestStartDate"] is DBNull)) {
                result.TestStartDate = (DateTime)row["TestStartDate"];
            }
            if (!(row["TestEndDate"] is DBNull))
            {
                result.TestEndDate = (DateTime)row["TestEndDate"];
            }

            result.SampleModel = this.dataColumn2String(row["SampleModel"]);
            result.Producer = this.dataColumn2String(row["Producer"]);
            result.CarVin = this.dataColumn2String(row["Carvin"]);
            result.TestState = this.dataColumn2String(row["Finishstate"]);
            result.SecurityLevel = this.dataColumn2String(row["Confidentiality"]);
            result.buildPurpose();

            return result;
        }

        private string dataColumn2String(object dataValue) {
            if (dataValue is DBNull) {
                return null;
            }
            return dataValue.ToString().Trim();
        }

        protected override void InitUi()
        {
            var year = DateTime.Now.Year.ToString();

            textYear.Visibility = BarItemVisibility.Always;
            comboxState.Visibility = BarItemVisibility.Never;
            startdate.Visibility = BarItemVisibility.Never;
            enddate.Visibility = BarItemVisibility.Never;

            //隐藏复制粘贴
            btnCopyPaste.Visibility= BarItemVisibility.Never;
            this.hideCopyMenuItem();

            //隐藏实验再分配
            this.barButtonItem11.Visibility = BarItemVisibility.Never;

            textYear.EditValue = year;
            comboxState.EditValue = "所有";       
            comGroup.Visibility = BarItemVisibility.Always;

            //barButtonItem1.Visibility = BarItemVisibility.Never;
            //蒸发组.Visible = !string.IsNullOrWhiteSpace(Department) && Department.Equals("蒸发组");
            //barButtonItem2.Enabled = FormTable.Add;
            //barButtonItem3.Enabled = FormTable.Add;

            _control._view.RowStyle += ViewOnRowStyle;
            _control._view.RowCellStyle += ViewRowCellStyle;
            _control._view.CellValueChanged += CellValueChanged;

            _control._view.FocusedRowChanged += SelectChanged;

            _control._view.InitNewRow += InitNewRow;

            //设置字段只读
            //this.initFormColumnsReadOnly();
        }

        protected override void initFormColumnsReadOnly() {
            READ_ONLY_COLUMNS.ForEach(item => this.setColumnReadOnly(item));

            //体系组特殊处理
            if (FormSignIn.CurrentUser.Department == "体系组")
            {
                btnNew.Visibility = BarItemVisibility.Never;
                //新建ToolStripMenuItem.Visibility = BarItemVisibility.Never;
                this.setFormReadOnly();
                _control._view.Columns["MoneySure"].OptionsColumn.ReadOnly = false;
            }
        }

        public string state0;
        private void SelectChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0 && Filter.state == "1")
            {
                Filter.filterText = _control._view.GetRowCellValue(e.FocusedRowHandle, "ItemBrief")?.ToString().Trim();

            }

        }

        /// <summary>
        /// 初始新行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitNewRow(object sender, InitNewRowEventArgs e)
        {
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["department"], FormSignIn.CurrentUser.Department);
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["Registrant"], FormSignIn.CurrentUser.Name);

            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["Taskcode2"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["Testtime"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["Oil"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["PriceDetail"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["Remark"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["Contacts"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["phoneNum"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["Email"], "—");

            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["EndMileage"], "0");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["TotalMileage"], "0");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["ProjectTotal"], 0);

            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["LogisticsInformation"], "—");

            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["ProjectPrice"], 0);
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["NationalFive"], "—");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["NationalSix"], "—");

            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["TestStartDate"], $"{DateTime.Now:yyyy/MM/dd HH:00}");
            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["TestEndDate"], $"{DateTime.Now:yyyy/MM/dd HH:00}");

            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns["RegistrationDate"], $"{DateTime.Now:yyyy/MM/dd HH:mm}");

        }

        /// <summary>
        /// 改变行颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewOnRowStyle(object sender, RowStyleEventArgs e)
        {


            if (e.RowHandle >= 0)
            {
                for (int j = 1; j < _control._view.Columns.Count; j++)
                {
                    if (_control._view.Columns[j].ToString() == "费用总计" && _control._view.GetRowCellValue(e.RowHandle, _control._view.Columns[j])?.ToString().Trim() == "0")
                    {
                        if (_control._view.GetRowCellValue(e.RowHandle, "ProjectPrice").ToString() != "0" && _control._view.GetRowCellValue(e.RowHandle, "ProjectPrice").ToString() != "")
                        {
                            _control._view.SetRowCellValue(e.RowHandle, _control._view.Columns[j], _control._view.GetRowCellValue(e.RowHandle, "ProjectPrice"));
                        }
                    }

                }

                e.Appearance.BackColor = Color.FromArgb(193, 255, 193);

                for (int j = 1; j < _control._view.Columns.Count; j++)
                {
                    if (_control._view.GetRowCellValue(e.RowHandle, _control._view.Columns[j])?.ToString().Trim() == "")
                    {
                        e.Appearance.BackColor = Color.FromArgb(237, 237, 237);
                        if (_control._view.GetRowCellValue(e.RowHandle, "Finishstate")?.ToString().Trim() == "已完成")
                        {
                            //e.Appearance.BackColor = Color.FromArgb(155, 205, 155);
                            e.Appearance.BackColor = Color.FromArgb(199, 237, 204);
                        }
                    }
                }


                if (_control._view.GetRowCellValue(e.RowHandle, "Finishstate")?.ToString().Trim() == "未完成")
                {
                    e.Appearance.BackColor = Color.Orange;
                }

                if (_control._view.GetRowCellValue(e.RowHandle, "MoneySure")?.ToString().Trim() == "是")
                {
                    e.Appearance.BackColor = Color.White;
                }

            }


            //if (e.RowHandle >= 0)
            //{
            //    //校验项目报价
            //    var ItemBrief = _control._view.GetRowCellValue(e.RowHandle, "ItemBrief")?.ToString().Trim();
            //    var SampleType = _control._view.GetRowCellValue(e.RowHandle, "SampleType")?.ToString().Trim();
            //    var StandardStage = _control._view.GetRowCellValue(e.RowHandle, "StandardStage")?.ToString().Trim();
            //    var ProjectPrice = _control._view.GetRowCellValue(e.RowHandle, "ProjectPrice")?.ToString().Trim();



            //    //校验样品信息
            //    var VIN = _control._view.GetRowCellValue(e.RowHandle, "Carvin")?.ToString().Trim();
            //    var VehicleModel = _control._view.GetRowCellValue(e.RowHandle, "SampleModel")?.ToString().Trim();
            //    var SampleProducter = _control._view.GetRowCellValue(e.RowHandle, "Producer")?.ToString().Trim();
            //    var PowerType = _control._view.GetRowCellValue(e.RowHandle, "PowerType")?.ToString().Trim();
            //    var EngineModel = _control._view.GetRowCellValue(e.RowHandle, "EngineModel")?.ToString().Trim();
            //    var EngineProducter = _control._view.GetRowCellValue(e.RowHandle, "EngineProduct")?.ToString().Trim();
            //    var DriveFormTask = _control._view.GetRowCellValue(e.RowHandle, "Drivertype")?.ToString().Trim();
            //    var DirectInjection = _control._view.GetRowCellValue(e.RowHandle, "YNDirect")?.ToString().Trim();

            //    string sql = $"select *  from ProjectQuotation where ItemBrief = '{ItemBrief}'";
            //    DataTable da = SqlHelper.GetList(sql);

            //    string sql2 = $"select *  from SampleTable where VIN = '{VIN}'";
            //    DataTable da2 = SqlHelper.GetList(sql2);

            //    if (da.Rows.Count > 0)
            //    {
            //        if (SampleType != da.Rows[0]["SampleType"].ToString() || StandardStage != da.Rows[0]["StandardStage"].ToString() || ProjectPrice != da.Rows[0]["Price"].ToString())
            //        {
            //            e.Appearance.BackColor = Color.LightGreen;
            //        }
            //    }
            //    if(da2.Rows.Count > 0)
            //    {
            //         if (VehicleModel != da2.Rows[0]["VehicleModel"].ToString() || SampleProducter != da2.Rows[0]["SampleProducter"].ToString() || PowerType != da2.Rows[0]["PowerType"].ToString() ||
            //         EngineModel != da2.Rows[0]["EngineModel"].ToString() || EngineProducter != da2.Rows[0]["EngineProducter"].ToString() ||
            //         DriveFormTask != da2.Rows[0]["DriveFormTask"].ToString() ||
            //    DirectInjection != da2.Rows[0]["DirectInjection"].ToString())
            //         {
            //                e.Appearance.BackColor = Color.LightGreen;
            //         }
            //    }



            //}


        }

        /// <summary>
        /// 判断是否为日期格式
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public bool IsDate(string strDate)
        {
            try
            {
                // strDate格式有要求，必须是yyyy-MM-dd hh:mm:ss
                DateTime.Parse(strDate);  //不是字符串时会出现异常
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 改变单元格颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewRowCellStyle(object sender, RowCellStyleEventArgs e)
        {

            if (e.RowHandle >= 0 && (e.Column.Caption == "创建日期"))
            {
                var TestStartDate = _control._view.GetRowCellValue(e.RowHandle, "TestStartDate")?.ToString().Trim();
                var RegistrationDate = _control._view.GetRowCellValue(e.RowHandle, "RegistrationDate")?.ToString().Trim();
                var question = _control._view.GetRowCellValue(e.RowHandle, "question")?.ToString().Trim();
                if (IsDate(TestStartDate) && IsDate(RegistrationDate))
                {
                    var ss = (Convert.ToDateTime(RegistrationDate) - Convert.ToDateTime(TestStartDate)).Days;
                    //_control._view.SetRowCellValue(e.RowHandle, "question", "");
                    if (ss > 3 && question != "超时")
                    {
                        e.Appearance.BackColor = Color.Red;
                        _control._view.SetRowCellValue(e.RowHandle, "question", "超时");
                    }
                    if (ss <= 3 && question == "超时")
                    {
                        _control._view.SetRowCellValue(e.RowHandle, "question", "");
                    }

                }
            }

            ////校验项目报价
            //if (e.RowHandle >= 0 && (e.Column.Caption == "样品类型" || e.Column.Caption == "标准阶段" || e.Column.Caption == "项目单价"))
            //{
            //    var ItemBrief = _control._view.GetRowCellValue(e.RowHandle, "ItemBrief")?.ToString().Trim();
            //    var SampleType = _control._view.GetRowCellValue(e.RowHandle, "SampleType")?.ToString().Trim();
            //    var StandardStage = _control._view.GetRowCellValue(e.RowHandle, "StandardStage")?.ToString().Trim();
            //    var ProjectPrice = _control._view.GetRowCellValue(e.RowHandle, "ProjectPrice")?.ToString().Trim();

            //    string sql = $"select *  from ProjectQuotation where ItemBrief = '{ItemBrief}'";
            //    DataTable da = SqlHelper.GetList(sql);
            //    if (da.Rows.Count > 0)
            //    {
            //        if (e.Column.Caption == "样品类型" && SampleType != da.Rows[0]["SampleType"].ToString())
            //        {
            //            e.Appearance.BackColor = Color.BurlyWood;
            //        }
            //        if (e.Column.Caption == "标准阶段" && StandardStage != da.Rows[0]["StandardStage"].ToString())
            //        {
            //            e.Appearance.BackColor = Color.BurlyWood;
            //        }
            //        if (e.Column.Caption == "项目单价" && ProjectPrice != da.Rows[0]["Price"].ToString())
            //        {
            //            e.Appearance.BackColor = Color.BurlyWood;
            //        }

            //    }
            //}


            ////校验样品信息
            //if (e.RowHandle >= 0 && (e.Column.Caption == "样品型号" || e.Column.Caption == "生产厂家" || e.Column.Caption == "动力类型" || e.Column.Caption == "发动机型号" || e.Column.Caption == "发动机生产厂" || e.Column.Caption == "驱动形式" || e.Column.Caption == "是否直喷"))
            //{
            //    var VIN = _control._view.GetRowCellValue(e.RowHandle, "Carvin")?.ToString().Trim();
            //    var VehicleModel = _control._view.GetRowCellValue(e.RowHandle, "SampleModel")?.ToString().Trim();
            //    var SampleProducter = _control._view.GetRowCellValue(e.RowHandle, "Producer")?.ToString().Trim();
            //    var PowerType = _control._view.GetRowCellValue(e.RowHandle, "PowerType")?.ToString().Trim();
            //    var EngineModel = _control._view.GetRowCellValue(e.RowHandle, "EngineModel")?.ToString().Trim();
            //    var EngineProducter = _control._view.GetRowCellValue(e.RowHandle, "EngineProduct")?.ToString().Trim();
            //    var DriveFormTask = _control._view.GetRowCellValue(e.RowHandle, "Drivertype")?.ToString().Trim();
            //    var DirectInjection = _control._view.GetRowCellValue(e.RowHandle, "YNDirect")?.ToString().Trim();

            //    string sql2 = $"select *  from SampleTable where VIN = '{VIN}'";
            //    DataTable da2 = SqlHelper.GetList(sql2);

            //    if (da2.Rows.Count > 0)
            //    {
            //        if (e.Column.Caption == "样品型号" && VehicleModel != da2.Rows[0]["VehicleModel"].ToString())
            //        {
            //            e.Appearance.BackColor = Color.BurlyWood;
            //        }
            //        if (e.Column.Caption == "生产厂家" && SampleProducter != da2.Rows[0]["SampleProducter"].ToString())
            //        {
            //            e.Appearance.BackColor = Color.BurlyWood;
            //        }
            //        if (e.Column.Caption == "动力类型" && PowerType != da2.Rows[0]["PowerType"].ToString())
            //        {
            //            e.Appearance.BackColor = Color.BurlyWood;
            //        }
            //        if (e.Column.Caption == "发动机型号" && EngineModel != da2.Rows[0]["EngineModel"].ToString())
            //        {
            //            e.Appearance.BackColor = Color.BurlyWood;
            //        }
            //        if (e.Column.Caption == "发动机生产厂" && EngineProducter != da2.Rows[0]["EngineProducter"].ToString())
            //        {
            //            e.Appearance.BackColor = Color.BurlyWood;
            //        }
            //        if (e.Column.Caption == "驱动形式" && DriveFormTask != da2.Rows[0]["DriveFormTask"].ToString())
            //        {
            //            e.Appearance.BackColor = Color.BurlyWood;
            //        }
            //        if (e.Column.Caption == "是否直喷" && DirectInjection != da2.Rows[0]["DirectInjection"].ToString())
            //        {
            //            e.Appearance.BackColor = Color.BurlyWood;
            //        }

            //    }
            //}

        }

        /// <summary>
        /// 打开编辑器
        /// </summary>
        /// <param name="view"></param>
        /// <param name="hand"></param>
        protected override void OpenAddFormClick(GridView view, int hand)
        {
            //Form1.ShowWaitForm();
            try
            {
                this.createTaskForm = new CreateTaskForm(CreateTestTaskFrom.TEST_STATISTIC_LIST_FORM);
                TestStatisticEntity curTestStatistic = this.extractTestStatisticEntityByRowHand(view, hand);
                this.createTaskForm.setDialogParam(curTestStatistic, this.isPopRedirectToEditDialog());
                DialogResult result = createTaskForm.ShowDialog();
                if (result == DialogResult.OK) {
                    this.reloadData();
                    bool isNeedRedirectToEditForm = this.createTaskForm.isNeedRedirectToEditForm;
                    int curTestStatisticId = this.createTaskForm.testStatisticId;
                    int rowHandle = this.findRowHandleOfId(curTestStatisticId);
                    view.MoveBy(rowHandle);
                    if (isNeedRedirectToEditForm) {
                        this.OpenEditFormClick(view, rowHandle);
                    }
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
        }

        private bool isPopRedirectToEditDialog()
        {
            if (FormSignIn.isEditAdmin())
            {
                return true;
            }

            return StringUtils.isEquals(FormSignIn.CurrentUser.Department, comGroup.EditValue.ToString());
        }

        protected override DialogResult OpenEditForm(GridView view, int hand, List<DataField> fields)
        {
            Form1.ShowWaitForm();
            DialogResult result= DialogResult.Cancel;
            try
            {
                Log.e("OpenEditForm");
                var isAllocateTask = false;
                var dialog = new TestEditDialog(FormTable.Edit, view, hand, fields, FormType.Test, isAllocateTask);
                result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _control.SetSaveStatus(true);
                    if (!this.savedIds.Contains(dialog.getId()))
                    {
                        this.savedIds.Add(dialog.getId());
                    }
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

            return result;
        }

        private int findRowHandleOfId(int id) {
            DataRowCollection rows= this._control.DataSource.Rows;
            for (int index = 0; index < rows.Count; index++) {
                DataRow row = rows[index];
                if (row["ID"].ToString().Trim().Equals(id.ToString())) {
                    return index;
                }
            }

            return 0;
        }

       

        private TestStatisticEntity extractTestStatisticEntityByRowHand(GridView view, int hand) {
            var id = view.GetRowCellValue(hand, "ID");
            var vin = view.GetRowCellValue(hand, "Carvin");
            var itemType = view.GetRowCellValue(hand, "ItemType");
            var itemBrief = view.GetRowCellValue(hand, "ItemBrief");
            TestStatisticEntity curTestStatistic = new TestStatisticEntity().lite(int.Parse(id.ToString()), vin.ToString(), itemType.ToString(), itemBrief.ToString());

            return curTestStatistic;
        }

        protected override DialogResult OpenReplaceForm(GridView view, int hand, List<DataField> fields)
        {
            var dialog = new Dialogs.ReplaceSelectRows(FormTable.Edit, view, hand, fields, FormTable.Type);
            return dialog.ShowDialog();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dialog = new ImportData("整车", _control.Fields, _control.DataSource);
            if (dialog.ShowDialog() == DialogResult.OK)
                _control.SetSaveStatus(false);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dialog = new ImportData("炭罐", _control.Fields, _control.DataSource);
            if (dialog.ShowDialog() == DialogResult.OK)
                _control.SetSaveStatus(false);
        }

        private void _control_Load(object sender, EventArgs e)
        {

        }
        private Thread childThread;

        private List<DataField> Fields = new List<DataField>();

        private DataControl sql = new DataControl();
        private FormTable FormTable2;
        private void TaskForm_Load(object sender, EventArgs e)
        {

            InitColLayout();
            LoadSource();// 加载样品信息



            //ThreadStart childref = new ThreadStart(CallToChildThread);

            //childThread = new Thread(childref);
            //childThread.Start();
        }

        /// <summary>
        /// 滚动条至最左
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (_control._view.FocusedColumn == _control._view.Columns["department"])
            //{
            //    _control._view.FocusedColumn = _control._view.Columns[0];
            //}



            if (Templatecolumn.column == null || Templatecolumn.name == "默认模板")
            {

                _control._view.FocusedColumn = _control._view.Columns["ExperimentalSite"];
            }
            else
            {
                _control._view.FocusedColumn = _control._view.Columns[Templatecolumn.column[0]];
            }

        }
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {


        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (Templatecolumn.column == null || Templatecolumn.name == "默认模板")
            {

                _control._view.FocusedColumn = _control._view.Columns["MoneySure"];
            }
            else
            {
                _control._view.FocusedColumn = _control._view.Columns[Templatecolumn.column[Templatecolumn.column.Length - 1]];
            }


        }


        private ProjectInfo project;
        /// <summary>
        /// 详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {

            //if (row<0)
            //{
            //    return;
            //}
            if (_control._view.SelectedRowsCount == 1)
            {

                if (project == null || project.IsDisposed)
                {
                    int row = Convert.ToInt32(_control._view.GetSelectedRows()[0]);
                    string taskcode = _control._view.GetRowCellValue(row, "Taskcode").ToString();
                    project = new ProjectInfo(taskcode);
                    project.TopLevel = false;
                    project.FormBorderStyle = FormBorderStyle.None;
                    project.Dock = DockStyle.Fill;
                    //this.panel1.Controls.Clear();
                    //this.panel1.Controls.Add(project);
                    project.Show();
                }
                else
                {

                    project.Show();
                    project.Activate();

                }
            }
            else
            {
                MessageBox.Show("请选择某一行");
            }


        }



        /// <summary>
        /// 子线程，获取Lims系统数据
        /// </summary>
        public void CallToChildThread()
        {
            while (true)
            {

                Thread.Sleep(2000000);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// lims接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private SelectTemplate selectTemplate;
        /// <summary>
        ///选择模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (selectTemplate == null || selectTemplate.IsDisposed)
            {
                selectTemplate = new SelectTemplate("试验统计");
                selectTemplate.freshForm += SelectTemplate_freshForm;
                selectTemplate.Show();
            }
            else
            {
                selectTemplate.Show();
                selectTemplate.Activate();
            }

        }

        private void SelectTemplate_freshForm()
        {
            for (int j = 1; j < _control._view.Columns.Count; j++)
            {
                _control._view.Columns[j].Visible = false;
            }
            for (int i = Templatecolumn.column.Length - 1; i >= 0; i--)
            {
                _control._view.Columns[Templatecolumn.column[i]].Visible = true;
                // _control._view.Columns[Templatecolumn.column[i]].VisibleIndex = i;
            }
        }
        /// <summary>
        /// 默认模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            for (int j = 1; j < _control._view.Columns.Count; j++)
            {
                _control._view.Columns[j].Visible = false;
            }
            for (int j = 1; j < _control._view.Columns.Count; j++)
            {
                _control._view.Columns[j].Visible = true;
                _control._view.Columns[j].VisibleIndex = j;
            }
        }

        private void CellValueChanged(object sender, CellValueChangedEventArgs e)
        {



        }

        private AlertTemplate alertTemplate;
        /// <summary>
        /// 自定义模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (alertTemplate == null || alertTemplate.IsDisposed)
            {
                alertTemplate = new AlertTemplate("试验统计");
                alertTemplate.Show();
            }
            else
            {
                alertTemplate.Show();
                alertTemplate.Activate();
            }
        }


        #region 样品信息
        public void LoadSource()
        {
            string sql = "select * from sampletable";
            DataTable da = SqlHelper.GetList(sql);
            gridControl1.DataSource = da;
            gridView1.OptionsView.BestFitMaxRowCount = 30;
            gridView1.BestFitColumns();
            gridView1.OptionsView.ColumnAutoWidth = false;//700毫秒
        }

        private void InitColLayout()
        {
            FormTable2 = new FormTable(FormType.Sample, null);
            Fields = sql.InitDataFields(FormTable2);// 样品信息字段
            gridControl1.BeginUpdate();
            gridView1.BeginUpdate();

            gridView1.Columns.Clear();

            var cols = new List<GridColumn>();

            var fixColsName = new[] { "ID" };
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

                col.OptionsColumn.AllowEdit = Fields[i].AllowEdit;

                cols.Add(col);
            }

            gridView1.Columns.AddRange(cols.ToArray());


            gridView1.EndUpdate();
            gridControl1.EndUpdate();
        }


        #endregion


        /// <summary>
        /// 试验再分配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick_1(object sender, ItemClickEventArgs e)
        {

            //_control.RefreshClick(_year, _startdate, _enddate, _finishState, _group);
            if (_control.SAVE_EDIT != true)
            {
                MessageBox.Show("试验分配之前请保存更改");
                return;
            }

            var hand = _control.FocusedRowHandle;
            if (hand < 0)
            {
                MessageBox.Show("请选择一行数据!");
                return;
            }
            var dialog = new AllocateTest(_control._view, hand);
            dialog.ShowDialog();
            _control._view.FocusedRowHandle = hand + 1;
            _control._view.FocusedRowHandle = hand; //防止出错
            //_control.SetSaveStatus(false);

            _control.RefreshSource();



        }

        private void TestStatistic_Shown(object sender, EventArgs e)
        {

        }

        private void TestStatistic_Activated(object sender, EventArgs e)
        {
            //_control.RefreshSource();
            //if (Filter.Moudle=="任务管理"||Filter.Moudle == "样品信息")
            //{
            //    _control._view.FindFilterText = Filter.filterText;
            //}
            //else
            //{
            //    _control._view.FindFilterText = "";
            //}

            Filter.Moudle = "试验统计";
        }

        private void TestStatistic_Enter(object sender, EventArgs e)
        {
            Filter.state = "1";
        }

        private void TestStatistic_Leave(object sender, EventArgs e)
        {
            Filter.state = "0";
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 问题筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            //gridView1_CustomRowFilter(null, null);
            //_control._view.ActiveFilterString = "[Cartype]=M1";


            //_control._view.CustomRowFilter += _view_CustomRowFilter;


            _control._view.ActiveFilterString = "[question]='超时'";


            //if (FormSignIn.CurrentUser.Role!="超级管理员")
            //{
            //    MessageBox.Show("普通用户无法同步旧系统数据");
            //    return;
            //}
            //if (MessageBox.Show($"是否确认同步旧系统数据？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    string sql0 = "delete from NewTaskManager.dbo.TestStatistic where ExperimentalSite = '旧数据'";

            //    string sql1 = "insert into NewTaskManager.dbo.TestStatistic(ExperimentalSite, department, LocationNumber, Registrant, Taskcode, Taskcode2, CarType, ItemType, ItemBrief, TestStartDate,TestEndDate, Testtime, SampleModel, Producer, Carvin, Confidentiality, YNDirect, PowerType, TransmissionType, EngineModel, EngineProduct, Drivertype,StartMileage, EndMileage, TotalMileage,Oil, FuelLabel, StandardStage, YNcountry, ProjectTotal, PriceDetail, Finishstate, QualificationStatus, Remark, Contacts, phoneNum, MoneySure)select '旧数据', TaskTable.department,TaskTable.Drum,TaskTable.userName,TaskTable.Taskcode,TaskTable.Testtime,TaskTable.Cartype2,TaskTable.Type1,TaskTable.Testsample,TaskTable.TestStartDate,TaskTable.TestEndDate,TaskTable.TestTimeSpan,TaskTable.Cartype,TaskTable.Producer,TaskTable.Carvin,TaskTable.CarBrand,TaskTable.Fueltype,TaskTable.EngineType,TaskTable.Transmissiontype,TaskTable.Engineform,TaskTable.Enginproducer,TaskTable.Drivertype,TaskTable.StartMileage,TaskTable.EndMileage,TaskTable.TotalMileage,TaskTable.Oil,TaskTable.FuelLabel,TaskTable.Checkunit,TaskTable.YNcountry,TaskTable.Price,TaskTable.PriceDetail,TaskTable.Finishstate,TaskTable.Datastate,TaskTable.Remark,TaskTable.contact,TaskTable.phoneNum,TaskTable.MoneySure from TaskManager.dbo.TaskTable where(Convert(varchar(20), TaskManager.dbo.TaskTable.TestEndDate) <> '0001-01-01' or TaskManager.dbo.TaskTable.TestEndDate IS NULL) and TestStartDate is NOT NULL";
            //    SqlHelper.ExecuteNonquery(sql0, CommandType.Text);
            //    SqlHelper.ExecuteNonquery(sql1, CommandType.Text);
            //    MessageBox.Show("获取旧系统数据成功");

            //}


        }

        private void _view_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            //var TestStartDate = _control._view.GetRowCellValue(e.ListSourceRow, "TestStartDate")?.ToString().Trim();
            //var RegistrationDate = _control._view.GetRowCellValue(e.ListSourceRow, "RegistrationDate")?.ToString().Trim();
            //if (IsDate(TestStartDate) && IsDate(RegistrationDate))
            //{
            //    var ss = (Convert.ToDateTime(RegistrationDate) - Convert.ToDateTime(TestStartDate)).Days;
            //    if (ss > 3)
            //    {
            //        e.Visible = false;
            //        e.Handled = false;
            //    }
            //}
            //e.Visible = false;
            //        e.Handled = false;

        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {

        }





        private void repositoryItemTextEdit6_EditValueChanged(object sender, EventArgs e)
        {
            //repositoryItemTextEdit6_Leave(null, null);

            //string sql = $"select count(*) from TestStatistic where taskcode='{}' and MoneySure ='{}'"


        }




        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            //MessageBox.Show(barEditItem3.EditValue?.ToString().Trim());
        }

        private void repositoryItemTextEdit6_Leave(object sender, EventArgs e)
        {
            //MessageBox.Show(barEditItem2.EditValue?.ToString().Trim());
        }

        private void barButtonItem8_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            if (barEditItem2.EditValue?.ToString().Trim() == "")
            {
                return;
            }

            string sql = $"select count(*) from TestStatistic where taskcode='{barEditItem2.EditValue?.ToString().Trim()}' and MoneySure ='是'";
            if (SqlHelper.GetList(sql).Rows[0][0].ToString().ToString() != "0")
            {
                MessageBox.Show("该任务单号费用已确认", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("该任务单号费用未确认");
            }
        }
    }
}
