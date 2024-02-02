using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManager.common.utils;
using TaskManager.controller;
using TaskManager.domain.entity;
using TaskManager.domain.service;

namespace TaskManager
{
    public partial class EquipmentUsageRecordForm : BaseForm
    {
        public static string EQUIPMENT_USAGE_RECORD_WORD_TPL_PATH = @"D:\template\equipment_usage_record_tpl2.docx";

        MaskLayer maskLayer = new MaskLayer();

        private bool isExportingWord = false;

        public EquipmentUsageRecordForm()
        {
            InitializeComponent();
        }

        public EquipmentUsageRecordForm(FormType formType, string selectedDept) : base(formType, selectedDept)
        {
            InitializeComponent();
            if (DesignMode)
                return;
            this.isNewCopyFromCurItem = true;
            _control._view.RowStyle += ViewOnRowStyle;
            this.Controls.Add(maskLayer);
        }

        protected override void setTimeSpanCondition()
        {
            startdate.EditValue = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            enddate.EditValue = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        protected override void InitUi()
        {
            textYear.Visibility = BarItemVisibility.Never;
            comboxState.Visibility = BarItemVisibility.Never;

            comGroup.Visibility = BarItemVisibility.Always;
            comboxState.Visibility = BarItemVisibility.Always;
            textYear.Visibility = BarItemVisibility.Always;

            //操作区功能可见性
            btnExportWord.Visibility = BarItemVisibility.Always;
            btnImport.Visibility = BarItemVisibility.Never;
            btnExportEmptyTpl.Visibility = BarItemVisibility.Never;
            btnBatchReplace.Visibility = BarItemVisibility.Never;

            //startdate.Visibility = BarItemVisibility.Never;
            //enddate.Visibility = BarItemVisibility.Never;
            this.InitComUserView();
            this.InitComItemView();
            _control.Year = "所有项目";
        }


        private void EquipmentRecordForm_Load(object sender, EventArgs e)
        {
          
        }

        public void reInitForm() {
            //初始化筛选条件
            textYear.EditValue = "所有项目";
            _control.Year = "所有项目";
            comboxState.EditValue = FormSignIn.CurrentUser.Name;
            comGroup.EditValue = FormSignIn.CurrentUser.Department.ToString();
            startdate.EditValue = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            enddate.EditValue = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);

            //丢弃未保存更新
            _control.SetSaveStatus(true);

            //加载数据
            this.reloadData();
        }

        public void refreshForm()
        {
            //丢弃未保存更新
            _control.SetSaveStatus(true);

            //加载数据
            this.reloadData();
        }

            protected  void exportEventBack()
        {
            this.exportEquipmentUasageRecords();

            Report report = new Report();
            report.CreateNewDocument(EQUIPMENT_USAGE_RECORD_WORD_TPL_PATH); //模板路径

            //2,插入一个值
            report.InsertValue("code", "NJ.18.8.1102");
            report.InsertValue("name", "烟度计");
            report.InsertValue("type", "MEXA-600S");

            //添加一行
            report.AddRow(1);

            string[] values = { "2023.10.17",
                "√",
                 "",
                  "奇瑞商用车(安徽)有限公司LA9DB21B4N0ZST038X70P 1.6T+7DCT常温排放",

                   "良好",
                    "√",
                     "",
                      "韩东",
                       "测试"
            };
            report.InsertCell(1, 6, 9, values); //给模板中第一个表格的第二行的5列分别插入数据


            string filePath = @"D:\设备使用记录\3.doc";
            //保存文档
            report.SaveDocument(filePath);
        }

        protected override void exportWordEvent()
        {
            if (this.isExportingWord)
            {
                DialogResult result = MessageBox.Show("确定终止导出么？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    UIHelp.Instance.AbortEquipmentUasageRecordExportWork = true;
                }
            }
            else {
                //this.exportEquipmentUasageRecords();
                Thread exportWordThread = new Thread(exportWord);
                exportWordThread.IsBackground = true;
                exportWordThread.Start();
            }
        }

       private void exportWord()
        {
            try
            {
                this.isExportingWord = true;
                this.btnExportWord.Caption = "终止导出Word\r\n";
                List<EquipmentUsageRecordEntity> equipmentUsageRecords = this.buildEquipmentUasageRecords();
                if (Collections.isEmpty(equipmentUsageRecords))
                {
                    MessageBox.Show("无数据需要导出！", "提示", MessageBoxButtons.OK);
                    this.isExportingWord = false;
                    this.btnExportWord.Caption = "导出Word\r\n";
                    UIHelp.Instance.AbortEquipmentUasageRecordExportWork = false;
                    return;
                }
               
                maskLayer.ShowMask();
                EquipmentUasageRecordExportHanlderPlus hanlder = new EquipmentUasageRecordExportHanlderPlus(equipmentUsageRecords, maskLayer);
                hanlder.work();
                maskLayer.HideMask();
                maskLayer.SetProgressBarValue(0);
                this.isExportingWord = false;
                this.btnExportWord.Caption = "导出Word\r\n";
                UIHelp.Instance.AbortEquipmentUasageRecordExportWork = false;
                if (!hanlder.isAbort)
                {
                    MessageBox.Show($"设备使用记录已导出，请转至目录‘{hanlder.FileBasePath}’下查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else {
                    MessageBox.Show($"导出过程已被终止，部分设备使用记录可能已导出，请转至目录‘{hanlder.FileBasePath}’下查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               
            }
            catch (Exception ex) {
                maskLayer.HideMask();
                maskLayer.SetProgressBarValue(0);
                this.isExportingWord = false;
                this.btnExportWord.Caption = "导出Word\r\n";
                UIHelp.Instance.AbortEquipmentUasageRecordExportWork = false;
                string error = $"导出设备使用记录失败,失败原因：{ex.Message}-{ex.StackTrace}";
                MessageBox.Show(error, "错误信息", MessageBoxButtons.OK);
            }
        }

        private void exportEquipmentUasageRecords() {
            List<EquipmentUsageRecordEntity> equipmentUsageRecords =this. buildEquipmentUasageRecords();
            if (Collections.isEmpty(equipmentUsageRecords)) {
                MessageBox.Show("无数据需要导出！", "提示", MessageBoxButtons.OK);
                return;
            }

            EquipmentUasageRecordExportHanlder hanlder = new EquipmentUasageRecordExportHanlder(equipmentUsageRecords, maskLayer);
            hanlder.work();
           

            MessageBox.Show($"设备使用记录已导出，请转至目录‘{hanlder.FileBasePath}’下查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private List<EquipmentUsageRecordEntity> buildEquipmentUasageRecords()
        {
            List<EquipmentUsageRecordEntity> equipmentUsageRecords = new List<EquipmentUsageRecordEntity>();

            DataRowCollection rows = this._control.DataSource.Rows;
            if (rows.Count == 0) {
                return equipmentUsageRecords;
            }
            for (int index = 0; index < rows.Count; index++)
            {
                DataRow row = rows[index];
                EquipmentUsageRecordEntity result= this.dataRow2EquipmentUsageRecord(row);
                equipmentUsageRecords.Add(result);
            }

            return equipmentUsageRecords;
        }

        private EquipmentUsageRecordEntity dataRow2EquipmentUsageRecord(DataRow row)
        {
            EquipmentUsageRecordEntity result = new EquipmentUsageRecordEntity();
            result.ID= int.Parse(row["ID"].ToString().Trim());
            result.EquipmentCode = DbHelper.dataColumn2String(row["EquipmentCode"]);
            result.EquipmentName = DbHelper.dataColumn2String(row["EquipmentName"]);
            result.EquipmentType = DbHelper.dataColumn2String(row["EquipmentType"]);
            result.UsePerson = DbHelper.dataColumn2String(row["UsePerson"]);
            result.UseTime = DbHelper.dataColumn2DateTime(row["UseTime"]);
            result.Purpose = DbHelper.dataColumn2String(row["Purpose"]);
            result.PreUseState = DbHelper.dataColumn2String(row["PreUseState"]);
            result.UseState = DbHelper.dataColumn2String(row["UseState"]);
            result.PostUseState = DbHelper.dataColumn2String(row["PostUseState"]);
            result.PostUseProblem = DbHelper.dataColumn2String(row["PostUseProblem"]);
            result.Department = DbHelper.dataColumn2String(row["Department"]);
            result.LocationNumber = DbHelper.dataColumn2String(row["LocationNumber"]);
            result.Remark = DbHelper.dataColumn2String(row["Remark"]);

            return result;
        }

        /// <summary>
        /// 行标色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewOnRowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                string vin = _control._view.GetRowCellValue(e.RowHandle, "CarVin")?.ToString().Trim();
                if (string.IsNullOrWhiteSpace(vin))
                {
                    e.Appearance.BackColor = Color.FromArgb(199, 237, 204);
                }
            }

        }

        protected override DialogResult OpenEditForm(GridView view, int hand, List<DataField> fields)
        {
            Log.e("OpenEditForm");
            var isAllocateTask = false;
            var dialog = new EditEquipmentUsageRecordDialog(FormTable.Edit, view, hand, fields, FormType.EquipmentUsageRecord, isAllocateTask);
            return dialog.ShowDialog();
        }

        /// <summary>
        /// 打开具体的编辑器
        /// </summary>
        /// <param name="view"></param>
        /// <param name="hand"></param>
        /// <param name="fields"></param>
        protected override DialogResult OpenAddForm(GridView view, int hand, List<DataField> fields)
        {
            Log.e("OpenEditForm");
            var isAllocateTask = false;
            var dialog = new EditEquipmentUsageRecordDialog(FormTable.Edit, view, hand, fields, FormType.EquipmentUsageRecord, isAllocateTask);
            EquipmentUsageRecordEntity curEquipmentUsageRecord = this.extractEquipmentUsageRecordEntityByRowHand(view, this.selectedRowHand);
            dialog.setEquipmentUsageRecord(curEquipmentUsageRecord);
            return dialog.ShowDialog();
        }

        private EquipmentUsageRecordEntity extractEquipmentUsageRecordEntityByRowHand(GridView view, int hand)
        {
            EquipmentUsageRecordEntity entity = new EquipmentUsageRecordEntity();
            entity.EquipmentCode = view.GetRowCellValue(hand, "EquipmentCode").ToString();
            entity.EquipmentName = view.GetRowCellValue(hand, "EquipmentName").ToString();
            entity.EquipmentType = view.GetRowCellValue(hand, "EquipmentType").ToString();
            entity.Purpose = view.GetRowCellValue(hand, "Purpose").ToString();
            entity.ItemBrief = view.GetRowCellValue(hand, "ItemBrief").ToString();

            return entity;
        }
    }
}
