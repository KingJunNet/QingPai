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
    public partial class ConfigItemForm  : BaseForm
    {
        public ConfigItemForm ()
        {
            InitializeComponent();
        }

        public ConfigItemForm (FormType formType, string selectedDept) : base(formType, selectedDept)
        {
            InitializeComponent();
            if (DesignMode)
                return;
            this.isNewCopyFromCurItem = true;         
        }

        
        protected override void setTimeSpanCondition()
        {
            startdate.EditValue = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            enddate.EditValue = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        protected override void InitUi()
        {
            textYear.Visibility = BarItemVisibility.Never;
            comGroup.Visibility = BarItemVisibility.Always;
            comboxState.Visibility = BarItemVisibility.Always;
           

            //操作区功能可见性
            btnBatchReplace.Visibility = BarItemVisibility.Never;

            startdate.Visibility = BarItemVisibility.Never;
            enddate.Visibility = BarItemVisibility.Never;
            this.InitComUserView();
            _control.user = "所有类别";
        }

        override
        protected void InitComUserView()
        {
            repositoryItemComboBox5.Items.Clear();            repositoryItemComboBox5.Items.Add("所有类别");            repositoryItemComboBox5.Items.AddRange(Form1.ComboxDictionary["配置项名称"]);            comboxState.EditValue = "所有类别";            comboxState.Caption = "类   别";
        }


        private void EquipmentRecordForm_Load(object sender, EventArgs e)
        {
          
        }

        protected override DialogResult OpenEditForm(GridView view, int hand, List<DataField> fields)
        {
            Log.e("OpenEditForm");
            var dialog = new EditConfigItemDialog(FormTable.Edit, view, hand, fields, FormType.ConfigItem);
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
            Log.e("OpenAddForm");
            var dialog = new EditConfigItemDialog(FormTable.Edit, view, hand, fields, FormType.ConfigItem);
            ConfigItemEntity curConfigItemEntity = this.extractConfigItemEntityByRowHand(view, this.selectedRowHand);
            dialog.setBaseConfigItem(curConfigItemEntity);
            return dialog.ShowDialog();
        }

        override
        protected bool validateRemoveAuthority()
        {
            if (RoleManager.isAdmin(FormSignIn.CurrentUser.Role))
            {
                return true;
            }

            string userName = FormSignIn.CurrentUser.Name;
            for (int i = 0; i < _control._view.SelectedRowsCount; i++)
            {
                if (_control._view.GetRowCellValue(_control._view.GetSelectedRows()[i], "Registrant").ToString() != userName)
                {
                    MessageBox.Show("您无权限进行删除");
                    return false;
                }
            }

            return true;
        }

        private ConfigItemEntity extractConfigItemEntityByRowHand(GridView view, int hand)
        {
            ConfigItemEntity entity = new ConfigItemEntity();

            try
            {
                entity.Name = view.GetRowCellValue(hand, "Name").ToString();
                entity.Value = view.GetRowCellValue(hand, "Value").ToString();
                entity.GroupName = view.GetRowCellValue(hand, "GroupName").ToString();
                entity.Moudle = view.GetRowCellValue(hand, "Moudle").ToString();
                entity.Registrant = view.GetRowCellValue(hand, "Registrant").ToString();
            }
            catch (Exception ex) {
                return null;
            }

            return entity;
        }
    }
}
