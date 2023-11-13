using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Controls;

namespace TaskManager
{
    public partial class NewTaskEditDialog : BaseEditDialog
    {
        private readonly bool IsAllocateTask;

        private NewTaskEditDialog():base()
        {
            InitializeComponent();
        }

        public NewTaskEditDialog(bool authorityEdit, GridView theView, int theHand, List<DataField> fields,
            bool isAllocateTask, FormType Type1)
            : base(authorityEdit, theView, theHand, fields, Type1)
        {
            InitializeComponent();
            IsAllocateTask = isAllocateTask;
        }

        protected TitleCombox FinishState;
        protected TitleDate FinishMonth;
        protected override void BaseEditDialog_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            controls = GetAllControls(out Panel);
            var count = InitControls();

            #region 调整窗口大小

            var borderX = Size.Width - Panel.Width;
            var borderY = Size.Height - Panel.Height;

            const int len = 370 + 4;
            const int height = 30 + 4;
            var rowCount = (int)Math.Ceiling(count / 2.0) + 1;

            var w = len * 2 + Panel.Margin.Left + Panel.Margin.Right + borderX;
            var h = rowCount * height + Panel.Margin.Top + Panel.Margin.Bottom + borderY;
            Size = new Size(w, h);

            #endregion

            #region 事件注册

         

           

            #endregion

            flowLayoutPanel1.ResumeLayout(true);
            ResumeLayout(true);
            Log.e("BaseEditDialog_Load");
        }


      

        protected override bool FieldVisible(DataField field)
        {
            if (!IsAllocateTask)
                return field.EditorVisible;
            else
                return field.EditorVisible && field.Group.Equals("分配试验任务");
        }

 


        private void TimeSpanHandler(object sender, EventArgs e)
        {
            if (!(GetControlByFieldName("Testtime") is TitleTextBox StartTime) ||
                !(GetControlByFieldName("TestEndTime") is TitleTextBox EndTime) ||
                !(GetControlByFieldName("TestTimeSpan") is TitleCombox TimeSpan)) return;
            var start = StartTime.Value().Replace("：",":");
            var end = EndTime.Value().Replace("：", ":");

            if (!DateTime.TryParse(start, out var startTime) || !DateTime.TryParse(end, out var endTime)) 
                return;

            var span = endTime - startTime;
            TimeSpan.SetValue(span.TotalHours.ToString());

        }

        private void TaskEditDialog_Shown(object sender, EventArgs e)
        {
            Log.e("TaskEditDialog_Shown");
          
        }

      

        private void NewTaskEditDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
