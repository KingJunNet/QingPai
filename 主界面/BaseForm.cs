using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using TaskManager.domain.valueobject;

namespace TaskManager
{
    public partial class BaseForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        #region 属性

        protected string _year => textYear.EditValue?.ToString().Trim();
        protected string _state => comboxState.EditValue?.ToString().Trim();
        public string _startdate => !string.IsNullOrWhiteSpace(startdate.EditValue?.ToString().Trim()) ? Convert.ToDateTime(startdate.EditValue?.ToString().Trim()).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) : startdate.EditValue?.ToString().Trim();

        public string _enddate => !string.IsNullOrWhiteSpace(enddate.EditValue?.ToString().Trim()) ? Convert.ToDateTime(enddate.EditValue?.ToString().Trim()).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) : enddate.EditValue?.ToString().Trim();


        protected string _group => comGroup.EditValue?.ToString().Trim();
        protected string _user => comboxState.EditValue?.ToString().Trim();
        protected int _finishState => _state != null && _state.Equals("已完成") ? 1 :
            (_state != null && _state.Equals("未完成") ? -1 : 0);

        public FormType Type;
        public FormTable FormTable;
        public string Department;
        public string user;

        #endregion

        #region 构造函数

        protected BaseForm()
        {
            InitializeComponent();
        }

        public BaseForm(FormType formType, string selectedDept)
        {
            InitializeComponent();
            if (DesignMode)
                return;


            //int year = DateTime.Now.Year;//当前年 
            //int mouth = DateTime.Now.Month;//当前月 

            //int beforeYear = 0;
            //int beforeMouth = 0;
            //if (mouth <= 1)//如果当前月是一月，那么年份就要减1 
            //{
            //    beforeYear = year - 1;
            //    beforeMouth = 12;//上个月 
            //}
            //else
            //{
            //    beforeYear = year;
            //    beforeMouth = mouth - 1;//上个月 
            //}
            //string beforeMouthOneDay = beforeYear + "/" + beforeMouth + "/01";//上个月第一天 
            //string beforeMouthLastDay = beforeYear + "/" + beforeMouth + "/" + DateTime.DaysInMonth(year, beforeMouth);//上个月最后一天

            //startdate.EditValue = Convert.ToDateTime(beforeMouthOneDay).ToString("yyyy/MM/dd");
            //enddate.EditValue = Convert.ToDateTime(beforeMouthLastDay).ToString("yyyy/MM/dd");

            //startdate.EditValue = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1).ToString("yyyy/MM/dd");
            //enddate.EditValue = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek).ToString("yyyy/MM/dd");

            this.setTimeSpanCondition();

            Type = formType;
            FormTable = new FormTable(formType, selectedDept);
            Department = selectedDept;
            user= FormSignIn.CurrentUser.Name;


            _control.FormTable = FormTable;
            _control.Year = DateTime.Now.Year.ToString();
            _control.FinishState = 0;
            _control.Department = FormSignIn.CurrentUser.Department.ToString();
            _control.user = FormSignIn.CurrentUser.Name;
            _control.startdate = Convert.ToDateTime(startdate.EditValue).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            _control.enddate = Convert.ToDateTime(enddate.EditValue).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);


            _control.NewItemClick += 新建ToolStripMenuItem_Click;
            _control.OpenClick += 打开ToolStripMenuItem_Click;
        }

        protected virtual void setTimeSpanCondition() {
            startdate.EditValue = DateTime.Now.AddYears(-1).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            enddate.EditValue = DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        protected void FormLoad(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

          
            baseInitUi();
            InitUi();
            InitUiByAuthority();

            //设置群组条件
            this.InitComGrouopView();

            //必须在Init之前先定义事件
            //_control.AfterSetSaveStatus += TableControl1_AfterSetSaveStatus;


            _control.LoadSource();
            //_control.RefreshClick(_year, _startdate, _enddate, _finishState, _group);

            TableControl1_AfterSetSaveStatus(null, true);

            AfterFormLoad();
            this.Text = FormTable.FormTitle;
        }

        private void InitComGrouopView()
        {
            repositoryItemComboBox6.Items.Add("所有组别");
            repositoryItemComboBox6.Items.AddRange(FormSignIn.UserDic.Keys.ToList());
            comGroup.EditValue = FormSignIn.CurrentUser.Department.ToString();
            if (FormSignIn.CurrentUser.Department.ToString() == "体系组")
            {
                comGroup.EditValue = "所有组别";
                _control.Department = "所有组别";
            }
        }

        protected void InitComUserView()
        {
            repositoryItemComboBox5.Items.Clear();
            repositoryItemComboBox5.Items.Add("所有人");
            repositoryItemComboBox5.Items.AddRange(FormSignIn.Users);
            comboxState.EditValue = FormSignIn.CurrentUser.Name;
            comboxState.Caption = "使用人";
        }

        protected void InitComItemView()
        {
            repositoryItemComboBox55.Items.Clear();
            repositoryItemComboBox55.Items.Add("所有项目");
            repositoryItemComboBox55.Items.AddRange(Form1.ComboxDictionary["项目简称"]);
            textYear.Edit = this.repositoryItemComboBox55;
            textYear.EditValue = "所有项目";
            textYear.Caption = "项   目";
        }

        private void BaseForm_Shown(object sender, EventArgs e)
        {
            Log.w("BaseForm_Shown");
        }

        protected virtual void InitUi()
        {
            throw new Exception("BaseForm.InitUi is Empty");
        }

        protected void baseInitUi() {
            //导出word默认隐藏
            btnExportWord.Visibility = BarItemVisibility.Never;

            //导入，
            string userRole = FormSignIn.CurrentUser.Role;
            if (!(userRole.Equals(Role.超级管理员.ToString())|| userRole.Equals(Role.管理员.ToString())))
            {
                btnBatchReplace.Visibility = BarItemVisibility.Never;
                btnEditCfgItems.Visibility = BarItemVisibility.Never;
                btnImport.Visibility = BarItemVisibility.Never;
                btnExport.Visibility = BarItemVisibility.Never;
                btnExportEmptyTpl.Visibility = BarItemVisibility.Never;
            }
        }

        /// <summary>
        /// 根据权限设置UI
        /// </summary>
        protected void InitUiByAuthority()
        {
            if (!FormTable.Edit)
            {
                btnEdit.Caption = "详细信息";
                btnDetail.Caption = "详细信息";
                打开ToolStripMenuItem.Text = "查看详细信息";
            }
            else
            {
                btnEdit.Caption = "编辑数据";
                btnDetail.Caption = "编辑数据";
                打开ToolStripMenuItem.Text = "编辑";
            }

            //右键菜单
            新建ToolStripMenuItem.Enabled = FormTable.Add;
            btnCopyPaste.Enabled = FormTable.Add;
            删除toolStripMenuItem1.Enabled = FormTable.Delete;

            //Ribbon和View的状态
            btnImport.Enabled = FormTable.Add;
            btnSave.Enabled = FormTable.Save;
            btnNew.Enabled = FormTable.Add;
            //if((int)FormTable.Type<100||(int)FormTable.Type==301||(int)FormTable.Type==401)
            //    barButtonItem3.Enabled = false;
            //if (FormTable.Type == FormType.RegReport)
            //    barButtonItem3.Enabled = true;
        }

        protected virtual void AfterFormLoad()
        {

        }

        #endregion

        #region 保存状态

        protected virtual void TableControl1_AfterSetSaveStatus(object sender, bool IsSaved)
        {
            //if (IsSaved)
            //   this.Text = _control.FormTable.FormTitle;
            //else
            //    this.Text = "未保存-" + _control.FormTable.FormTitle;
        }

        #endregion

        #region 右键菜单 | 打开编辑器

        private void ContextMenuStrip1Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //打开ToolStripMenuItem.Enabled = _control.FocusedRowHandle >= 0;
        }

        /// <summary>
        /// 以下的快捷键都在TableControl中定义, BaseForm中的快捷键定义其实是无效的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _control._view.SelectAll();
        }

        public void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenEditFormClick(_control._view, _control.FocusedRowHandle);
        }

        public void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!FormTable.Add)
                return;

            OpenAddFormClick(_control._view, _control.FocusedRowHandle);
        }

        private void 删除toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!FormTable.Delete)
                return;
            //只允许删自己创建的信息
            if (FormTable.Module == "样品信息")
            {
                for (int i = 0; i < _control._view.SelectedRowsCount; i++)
                {
                    if (FormSignIn.CurrentUser.Department == "系统维护" || _control._view.GetRowCellValue(_control._view.GetSelectedRows()[i], "CreatePeople").ToString() == FormSignIn.CurrentUser.Name)
                    {

                    }
                    else
                    {
                        MessageBox.Show("无权限进行删除");
                        return;
                    }
                }

            }
            if (FormTable.Module == "试验统计")
            {
                if (FormSignIn.CurrentUser.Department == "体系组")
                {
                    MessageBox.Show("无权限进行删除");
                    return;
                }


            }
            if (FormTable.Module == "任务管理")
            {
                if (FormSignIn.CurrentUser.Department == "体系组" || FormSignIn.CurrentUser.Department == "系统维护")
                {

                }
                else
                {
                    MessageBox.Show("无权限进行删除");
                    return;
                }
                //for (int i = 0; i < _control._view.SelectedRowsCount; i++)
                //{
                //    if (_control._view.GetRowCellValue(_control._view.GetSelectedRows()[i], "CreatePeople").ToString() != FormSignIn.CurrentUser.Name)
                //    {
                //        MessageBox.Show("无权限进行删除");
                //        return;
                //    }
                //}

            }
            _control.DeleteSelectedRows();
        }

        protected void hideCopyMenuItem() {
            this.复制ToolStripMenuItem.Visible = false;
            this.粘贴ToolStripMenuItem.Visible = false;
        }

        protected void hideAddEtitCopyItem()
        {
            this.新建ToolStripMenuItem.Visible = false;
            this.打开ToolStripMenuItem.Visible = false;
            this.hideCopyMenuItem();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _control.CopySelectedRows();
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _control.PasteSelectedRows();
        }

        /// <summary>
        /// 批量替换
        /// </summary>
        /// <param name="view"></param>
        /// <param name="hand"></param>
        private void OpenReplaceFormClick(GridView view, int hand)
        {
            try
            {
                Form1.ShowWaitForm();
                var oldSaveStatus = _control.SAVE_EDIT;
                var result = OpenReplaceForm(view, view.FocusedRowHandle, _control.Fields);
                if (result == DialogResult.Cancel && oldSaveStatus)
                    _control.SetSaveStatus(true);
            }
            catch (Exception e)
            {
                Log.e($"ReplacementSelectedRows {e}");
            }
            finally
            {
                Form1.CloseWaitForm();
            }

        }

        /// <summary>
        /// 打开编辑器
        /// </summary>
        /// <param name="view"></param>
        /// <param name="hand"></param>
        protected virtual void OpenEditFormClick(GridView view, int hand)
        {
            try
            {
                //Form1.ShowWaitForm();
                //if (FormTable.Category == "试验统计" && hand < 0)
                //{
                //    MessageBox.Show("请在样品板块新增试验统计");
                //    return;
                //}


                var oldSaveStatus = _control.SAVE_EDIT;

                var newRow = hand < 0;
                if (newRow)//新数据
                    view.AddNewRow();

                var result = OpenEditForm(view, hand, _control.Fields);
                if (newRow)
                {
                    if (result != DialogResult.OK)
                        view.DeleteRow(view.FocusedRowHandle);
                    else
                        view.MoveLast();
                }

                if (result == DialogResult.Cancel && oldSaveStatus)
                    _control.SetSaveStatus(true);
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
            finally
            {
                //Form1.CloseWaitForm();
            }
        }

        /// <summary>
        /// 打开新建表单
        /// </summary>
        /// <param name="view"></param>
        /// <param name="hand"></param>
        protected virtual void OpenAddFormClick(GridView view, int hand)
        {
            try
            {
                hand = -1;
                var oldSaveStatus = _control.SAVE_EDIT;

                var newRow = hand < 0;
                if (newRow)//新数据
                    view.AddNewRow();

                var result = OpenEditForm(view, view.FocusedRowHandle, _control.Fields);
                if (newRow)
                {
                    if (result != DialogResult.OK)
                        view.DeleteRow(view.FocusedRowHandle);
                    else
                        view.MoveLast();
                }

                if (result == DialogResult.Cancel && oldSaveStatus)
                    _control.SetSaveStatus(true);
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
            finally
            {
                //Form1.CloseWaitForm();
            }
        }

        /// <summary>
        /// 批量替换
        /// </summary>
        /// <param name="view"></param>
        /// <param name="hand"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        protected virtual DialogResult OpenReplaceForm(GridView view, int hand, List<DataField> fields)
        {
            MessageBox.Show("没有重载OpenReplaceForm");
            return DialogResult.Cancel;
        }

        /// <summary>
        /// 打开具体的编辑器
        /// </summary>
        /// <param name="view"></param>
        /// <param name="hand"></param>
        /// <param name="fields"></param>
        protected virtual DialogResult OpenEditForm(GridView view, int hand, List<DataField> fields)
        {
            MessageBox.Show("没有重载OpenEditForm");
            return DialogResult.Cancel;
        }

        #endregion

        #region 菜单栏按键

        private void btnDetail_ItemClick(object sender, ItemClickEventArgs e)
        {
            打开ToolStripMenuItem_Click(sender, e);
        }

        private void btnNew_ItemClick(object sender, ItemClickEventArgs e)
        {

            新建ToolStripMenuItem_Click(sender, e);
        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.reloadData();
        }

        protected void reloadData() {
            string user = null;
            if (comboxState.Caption == "使用人")
            {
                user = _user;
            }
            _control.RefreshClick(_year, _startdate, _enddate, _finishState, _group, user);
        }

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            _control.SaveClick();
        }

        private void btnExport_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.exportEvent();
        }

        private void btnExportWord_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.exportWordEvent();
        }

        protected virtual void exportEvent() {
            _control.Export();
        }

        protected virtual void exportWordEvent()
        {
            
        }

        private void barReplace_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenReplaceFormClick(_control._view, -1);

        }
        private void 导入Btn_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!_control.SAVE_EDIT)
            {
                MessageBox.Show("导入前请先保存当前更改");
                return;
            }
            else if (!FormTable.Add)
            {
                MessageBox.Show("没有新增数据的权限!");
                return;
            }

            MessageBox.Show("导入须知:\n" +
                            "数字字段必须为数字\n" +
                            "是否字段必须为是或者否", "提示");

            _control.Import();
        }

        /// <summary>
        /// 导出空白模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmpty_ItemClick(object sender, ItemClickEventArgs e)
        {
            _control.ExportTemplate();
        }

        /// <summary>
        /// 复制粘贴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            _control.CopySelectedRows();
            _control.PasteSelectedRows();
        }

        /// <summary>
        /// 修正日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCorrectDate_ItemClick(object sender, ItemClickEventArgs e)
        {
            _control.CorrectDate();
        }

        /// <summary>
        /// 编辑备选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dialog = new ComboxForm(FormTable.Module, _control);
            dialog.ShowDialog();
        }

        #endregion

        /// <summary>
        /// 点击关闭窗口按钮;判定是否已经保存;用隐藏取代关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDataStudio_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall) //不问情况直接关闭
            {
                e.Cancel = false;
            }
            else if (e.CloseReason == CloseReason.MdiFormClosing)//父窗体关闭，会询问，然后关闭
            {
                e.Cancel = !_control.Want2Close();
            }
            else//不关闭，只是隐藏
            {
                bool close = _control.Want2Close();
                if (close)
                    Hide();
                e.Cancel = true;
            }
        }

        private void _control_Load(object sender, EventArgs e)
        {

        }

        private void comGroup_EditValueChanged(object sender, EventArgs e)
        {
            if (FormTable.Category == "试验统计")
            {
                if (comGroup.EditValue.ToString() != FormSignIn.CurrentUser.Department.ToString())
                {
                    //btnSave.Enabled = false;
                    FormTable.Add = false;
                    FormTable.Edit = false;
                    FormTable.Delete = false;
                }
                else
                {
                    FormTable.Add = true;
                    FormTable.Edit = true;
                    FormTable.Delete = true;
                }

                if (FormSignIn.CurrentUser.Department.ToString() == "系统维护" || FormSignIn.CurrentUser.Department.ToString() == "体系组")
                {
                    FormTable.Add = true;
                    FormTable.Edit = true;
                    FormTable.Delete = true;
                }
            }

            InitUiByAuthority();

        }

        private void comGroup_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            打开ToolStripMenuItem_Click(sender, e);
        }

        private void startdate_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
