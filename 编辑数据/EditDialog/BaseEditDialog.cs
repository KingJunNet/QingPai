using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Controls;
using ExpertLib.Controls.TitleEditor;
using LabSystem.DAL;
using TaskManager.domain.valueobject;

namespace TaskManager
{
    public partial class BaseEditDialog : Form
    {
        protected BaseEditDialog()
        {
            InitializeComponent();
        }

        private DataControl sql = new DataControl();
        public readonly string Server;
        private const string RootFolder = "轻排参数表服务器";
        private const string Category = "设备管理";

        private FormType type;

        protected OperationType operation;

        protected int recordId;

        public BaseEditDialog(bool authorityEdit, GridView theView, int theHand, List<DataField> fields, FormType Type1)
        {
            InitializeComponent();
            type = Type1;
            if(DesignMode)
                return;
            btnUpdate.Enabled = authorityEdit;
            view = theView;
            hand = theHand;
            if (hand<0)
            {
                this.operation = OperationType.ADD;
                btnUpdate.Text = "新增";
            }
            else
            {
                this.operation = OperationType.EDIT;
                btnUpdate.Text = "更新";
            }
            this.Text = this.buildTitle();
            Fields = fields;

            var sql = new DataControl();
            Server = sql.ServerIP;
            if (!Server.EndsWith("\\"))
                Server += "\\";
        }

        protected virtual string buildTitle() {
            string title = "";
            if (this.operation.Equals(OperationType.ADD)) {
                title = "新增信息";
            }
            else if (this.operation.Equals(OperationType.EDIT)) {
                title = "编辑信息";
            }

            return title;
        }

        protected virtual void BaseEditDialog_Load(object sender, EventArgs e)
        {
            controls = GetAllControls(out Panel);
            InitControls();
        }

        #region InitDialog

        protected GridView view;

        protected int hand;

        protected Dictionary<int, TitleControl> controls;

        protected Dictionary<string, TitleControl> fieldControlMap;

        protected FlowLayoutPanel Panel;

        protected List<DataField> Fields;

        /// <summary>
        /// 返回可见的控件数量
        /// </summary>
        /// <returns></returns>
        public int InitControls()
        {
            fieldControlMap = new Dictionary<string, TitleControl>();

            if (!controls.Any())
                return 0;

            var count = 0;
            foreach (var field in Fields)
            {
                if (!controls.ContainsKey(field.DisplayIndex))
                    continue;

                var titleControl = controls[field.DisplayIndex];
                SetControlField(titleControl, field);
                fieldControlMap.Add(field.Eng, titleControl);
                if (titleControl.Visible) 
                    count++;
            }

            var idValue = view.GetRowCellValue(hand, "ID");
            this.recordId = int.Parse(idValue.ToString());

            Panel.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
                                                                     | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(Panel, true, null);

            return count;
        }

        protected virtual bool FieldVisible(DataField field)
        {
            return field.EditorVisible;
        }

        #endregion

        protected virtual void BtnUpdateClick(object sender, EventArgs e)
        {
            if (!this.validateFormParam()) {
                return;
            }

            if (type == FormType.Equipment)
            {
                string f1 = "", f2 = "", f3 = "";
                string equipname = "", checkdate = "", checktype = "";

                if (view.GetRowCellValue(hand, "ID") == null) return;
                int.TryParse(view.GetRowCellValue(hand, "ID").ToString(), out var id);
                if (id == -1)
                {
                    equipname = GetControlByFieldName("EquipName").Value().Trim();
                    checkdate = GetControlByFieldName("CheckDate").Value().Trim();
                    checktype = GetControlByFieldName("CheckType").Value().Trim();
                }
                else
                {
                    equipname = view.GetRowCellValue(hand, "EquipName").ToString().Trim();
                    checkdate = Convert.ToDateTime(view.GetRowCellValue(hand, "CheckDate").ToString().Trim()).Date.ToString("yyyy-MM-dd");
                    checktype = view.GetRowCellValue(hand, "CheckType").ToString().Trim();
                }
                DataSet d1 = sql.ExecuteQuery("select f1,f2,f3 from EquipmentFileTable where EquipName='" + equipname + "' and Checkdate='" + checkdate + "' and CheckType = '" + checktype + "'");

                if (d1.Tables[0].Rows.Count != 0)
                {
                    f1 = d1.Tables[0].Rows[0]["f1"].ToString().Trim();
                    f2 = d1.Tables[0].Rows[0]["f2"].ToString().Trim();
                    f3 = d1.Tables[0].Rows[0]["f3"].ToString().Trim();
                }
            }

            foreach (var control in controls)
            {
                var fieldName = control.Value.Tag?.ToString();
                if (string.IsNullOrWhiteSpace(fieldName))
                    continue;

                var value = control.Value.Value();
                view.SetRowCellValue(hand, fieldName, value);
            }            
            view.FocusedRowHandle = hand + 1;
            view.FocusedRowHandle = hand; //防止出错


            //if (controls.Count == 37)
            //{
            //    if (MessageBox.Show("是否将改动的样品信息同步到试验统计对应的信息？", " 提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //    {
            //        string sql = $"update TestStatics set SampleModel ='{view.GetRowCellValue(hand, "SampleModel").ToString()}',Producer='{view.GetRowCellValue(hand, "Producer").ToString()}',Carvin='{view.GetRowCellValue(hand, "Carvin").ToString()}',YNDirect='{view.GetRowCellValue(hand, "YNDirect").ToString()}',PowerType='{view.GetRowCellValue(hand, "PowerType").ToString()}',EngineModel='{view.GetRowCellValue(hand, "EngineModel").ToString()}',TransmissionType='{view.GetRowCellValue(hand, "TransmissionType").ToString()}',EngineProduct='{view.GetRowCellValue(hand, "EngineProduct").ToString()}',FuelLabel='{view.GetRowCellValue(hand, "FuelLabel").ToString()}'";
            //        SqlHelper.ExecuteNonquery(sql, System.Data.CommandType.Text);
            //        MessageBox.Show("同步成功");
            //    }
                  
            //}

            DialogResult = DialogResult.OK;
            Close();

        }

        protected virtual bool validateFormParam() {
            return true;
        }

        protected void BtnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// 获得所有控件
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        protected Dictionary<int, TitleControl> GetAllControls(out FlowLayoutPanel panel)
        {
            var dic = new Dictionary<int, TitleControl>();
            panel = null;
            foreach (Control control in Controls)
            {
                if (control.GetType() != typeof(FlowLayoutPanel)) 
                    continue;

                panel = control as FlowLayoutPanel;

                foreach (Control c in control.Controls)
                {
                    if (c.GetType() != typeof(TitleCombox) &&
                        c.GetType() != typeof(RequiredTitleCombox)&&
                        c.GetType() != typeof(TitleDate) &&
                        c.GetType() !=  typeof(TitleTextBox)&&
                        c.GetType() != typeof(DateEdit))
                        continue;

                    var key = c.TabIndex;
                    if(dic.ContainsKey(key))
                        throw new Exception("BaseEditDialog GetAllControls");

                    dic.Add(key, c as TitleControl);
                }

                break;
            }

            return dic;
        }

        protected TitleControl GetControlByFieldName(string fieldName)
        {
            //foreach (var value in controls.Values)
            //{
            //    if (value.Tag != null && value.Tag.ToString().Equals(fieldName))
            //        return value;
            //}

            if (this.fieldControlMap.ContainsKey(fieldName)) {
                return this.fieldControlMap[fieldName];
            }

            return null;
        }

        /// <summary>
        /// 基于DataField设置编辑控件的属性
        /// </summary>
        /// <param name="titleControl"></param>
        /// <param name="field"></param>
        protected void SetControlField(TitleControl titleControl, DataField field)
        {
            var value = view.GetRowCellValue(hand, field.Eng);

            titleControl.Title = field.Chs;
            titleControl.Tag = field.Eng;
            titleControl.Visible = FieldVisible(field);
            titleControl.SetValue(value.ToString());

            var editable = view.Columns[field.Eng].OptionsColumn.AllowEdit;
            titleControl.SetReadOnly(!editable);
            titleControl.OriginalReadOnly = !editable;

            if (field.Format.Equals("时间"))
            {
                if (titleControl is TitleTextBox time)
                {
                    time.InputType = TextInputType.NotControl;
                    time.PromptText = "请输入时间例如 13:14";
                }
                else
                {
                    ((TitleCombox)titleControl).comboBox1.DropDownStyle = ComboBoxStyle.Simple;
                }
            }
            else if (field.Format.Equals("数字"))
            {
                if ((titleControl is TitleTextBox text))
                {
                    text.PromptText = "请输入数字";
                    text.InputType = TextInputType.Number;
                }
                else
                {
                    ((TitleCombox)titleControl).comboBox1.DropDownStyle = ComboBoxStyle.Simple;
                }
            }
            else if(!field.Format.Equals("日期"))
            {
                if (!(view.Columns[field.Eng].ColumnEdit is RepositoryItemComboBox editor))
                    return;

                var list = new List<string>();
                foreach (var item in editor.Items)
                {
                    list.Add(item.ToString());
                }
                titleControl.SetItems(list);
            }
        }

        protected string getValue(string fieldName) {
            var value = view.GetRowCellValue(hand, fieldName);
            return value.ToString();
        }

        /// <summary>
        /// 对比值是否发生了变化
        /// </summary>
        /// <param name="titleControl"></param>
        /// <returns></returns>
        protected bool CompareControlValueChanged(TitleControl titleControl)
        {
            var tag = titleControl.Tag?.ToString();
            if (string.IsNullOrWhiteSpace(tag))
                return false;

            var oldValue = view.GetRowCellValue(hand, tag).ToString().Trim();
            var newValue = titleControl.Value().Trim();
            return !oldValue.Equals(newValue);
        }
    }
}
