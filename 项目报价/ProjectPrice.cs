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
using Xfrog.Net;

namespace TaskManager
{
    public partial class ProjectPrice : BaseForm
    {
        public ProjectPrice()
        {
            InitializeComponent();
        }

        public ProjectPrice(FormType formType, string selectedDept) : base(formType, selectedDept)
        {
            InitializeComponent();
        }



        protected override void InitUi()
        {
            var year = DateTime.Now.Year.ToString();

            textYear.Visibility = BarItemVisibility.Never;
            comboxState.Visibility = BarItemVisibility.Never;
            startdate.Visibility = BarItemVisibility.Never;
            enddate.Visibility = BarItemVisibility.Never;
            textYear.EditValue = year;
            comboxState.EditValue = "所有";
            if (FormSignIn.CurrentUser.Role.ToString() != "超级管理员")
            {
                //btnDetail.Enabled = false;
                //_control._view.OptionsBehavior.Editable = false;
                //新建ToolStripMenuItem.Enabled = false;
                //打开ToolStripMenuItem.Enabled = false;
                //FormTable.Edit = false;
                //FormTable.Delete = false;

            }
      
            _control._view.RowStyle += ViewOnRowStyle;
            _control._view.RowCellStyle += ViewRowCellStyle;
            _control._view.CellValueChanged += CellValueChanged;
        }

        /// <summary>
        /// 改变行颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewOnRowStyle(object sender, RowStyleEventArgs e)
        {
        
        }

        /// <summary>
        /// 改变单元格颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewRowCellStyle(object sender, RowCellStyleEventArgs e)
        {
          
           
        }

        protected override DialogResult OpenEditForm(GridView view, int hand, List<DataField> fields)
        {
            Log.e("OpenEditForm");
            var isAllocateTask = false;
            var dialog = new ProjectEditDialog(FormTable.Edit, view, hand, fields,FormType.Project, isAllocateTask);
            return dialog.ShowDialog();
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
           
        }

        /// <summary>
        /// 滚动条至最左
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void barButtonItem11_ItemClick_1(object sender, ItemClickEventArgs e)
        {
       
           
            if (Templatecolumn.column == null || Templatecolumn.name == "默认模板")
            {

                _control._view.FocusedColumn = _control._view.Columns["SampleType"];
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

                _control._view.FocusedColumn = _control._view.Columns["Remark"];
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
            else {
                MessageBox.Show("请选择某一行");
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
            if(selectTemplate == null || selectTemplate.IsDisposed)
            {
                selectTemplate = new SelectTemplate("项目报价");
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
            for(int j = 1; j < _control._view.Columns.Count; j++)
            {
                _control._view.Columns[j].Visible = false;
            }
            for (int i = Templatecolumn.column.Length-1; i >=0; i--)
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
            //if (e.Column.FieldName == "State")
            //{
            //    if (e.Value.ToString() == "已完成")
            //    {
            //        _control._view.SetRowCellValue(e.RowHandle, "FinishDate",DateTime.Now.ToString("yyyy-MM-dd"));
            //    }
            //    else
            //    {
            //        _control._view.SetRowCellValue(e.RowHandle, "FinishDate", "");
            //    }
            //}
            if(e.Column.FieldName == "TypeBrief")
            {
                if (e.Value.ToString() == "QA")
                {
                    _control._view.SetRowCellValue(e.RowHandle, "Type1", "公告");
                }
                else if (e.Value.ToString() == "HB")
                {
                    _control._view.SetRowCellValue(e.RowHandle, "Type1", "环保");
                }
                else if (e.Value.ToString() == "WT")
                {
                    _control._view.SetRowCellValue(e.RowHandle, "Type1", "委托");
                }
                else if (e.Value.ToString() == "XG")
                {
                    _control._view.SetRowCellValue(e.RowHandle, "Type1", "修改");
                }
                else if (e.Value.ToString() == "JY")
                {
                    _control._view.SetRowCellValue(e.RowHandle, "Type1", "技研");
                }
                else if (e.Value.ToString() == "YF")
                {
                    _control._view.SetRowCellValue(e.RowHandle, "Type1", "研发");
                }
                else if (e.Value.ToString() == "CK")
                {
                    _control._view.SetRowCellValue(e.RowHandle, "Type1", "出口");
                }
                else if (e.Value.ToString() == "SZ")
                {
                    _control._view.SetRowCellValue(e.RowHandle, "Type1", "商检");
                }
                else if (e.Value.ToString() == "CA")
                {
                    _control._view.SetRowCellValue(e.RowHandle, "Type1", "测评");
                }
              
            }
        }

        private AlertTemplate alertTemplate;
        /// <summary>
        /// 自定义模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (alertTemplate == null || alertTemplate.IsDisposed)
            //{
            //    alertTemplate = new AlertTemplate("项目报价");
            //    alertTemplate.Show();
            //}
            //else
            //{
            //    alertTemplate.Show();
            //    alertTemplate.Activate();
            //}
        }


        #region 样品信息
        //public void LoadSource()
        //{
        //    string sql = "select * from sampletable";
        //    DataTable da = SqlHelper.GetList(sql);
        //    gridControl1.DataSource = da;
        //    gridView1.OptionsView.BestFitMaxRowCount = 30;
        //    gridView1.BestFitColumns();
        //    gridView1.OptionsView.ColumnAutoWidth = false;//700毫秒
        //}

        //private void InitColLayout()
        //{
        //    FormTable2 = new FormTable(FormType.Sample, null);
        //    Fields = sql.InitDataFields(FormTable2);// 样品信息字段
        //    gridControl1.BeginUpdate();
        //    gridView1.BeginUpdate();

        //    gridView1.Columns.Clear();

        //    var cols = new List<GridColumn>();

        //    var fixColsName = new[] { "ID" };
        //    foreach (var col in fixColsName.Select(colName => new GridColumn
        //    {
        //        Name = colName,
        //        FieldName = colName
        //    }))
        //    {
        //        col.OptionsColumn.AllowEdit = false;
        //        col.Visible = false;
        //        col.OptionsColumn.AllowMove = false;
        //        col.OptionsColumn.ShowInCustomizationForm = false;
        //        cols.Add(col);
        //    }

        //    for (var i = 0; i < Fields.Count; i++)
        //    {
        //        var col = new GridColumn
        //        {
        //            Tag = Fields[i],
        //            Name = Fields[i].Eng,
        //            FieldName = Fields[i].Eng,
        //            Caption = Fields[i].Chs,
        //            VisibleIndex = i,
        //            Visible = Fields[i].ColumnVisible,
        //            Fixed = Fields[i].DisplayLevel == 0 ? FixedStyle.Left : FixedStyle.None
        //        };
        //        col.OptionsFilter.AllowFilter = true;
        //        col.OptionsFilter.AllowAutoFilter = true;

        //        col.OptionsColumn.AllowEdit = Fields[i].AllowEdit;

        //        cols.Add(col);
        //    }

        //    gridView1.Columns.AddRange(cols.ToArray());


        //    gridView1.EndUpdate();
        //    gridControl1.EndUpdate();
        //}


        #endregion

        private void ProjectPrice_Activated(object sender, EventArgs e)
        {
            
            //if (Filter.Moudle == "试验统计")
            //{
            //    _control._view.FindFilterText = Filter.filterText;
            //}
            //else
            //{
            //    _control._view.FindFilterText = "";
            //}
            Filter.Moudle = "项目报价";
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

    }
}
