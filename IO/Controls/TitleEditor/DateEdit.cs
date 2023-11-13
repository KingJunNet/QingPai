using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ExpertLib.Controls.TitleEditor
{
    public partial class DateEdit : ExpertLib.Controls.TitleControl
    {
        public DateEdit()
        {
            InitializeComponent();
        }
        public DateTime Date => dateEdit1.DateTime;

        public void SetValueChange(EventHandler handler)
        {
            //dateTimePicker1.ValueChanged += handler;
            dateEdit1.EditValueChanged += handler;
        }

        public override string Value()
        {
            //return dateTimePicker1.Value.Date.ToString("yyyy/MM/dd");
            if (dateEdit1.EditValue != null)
                return dateEdit1.EditValue.ToString();
            else
                return null;
        }



        public override void SetReadOnly(bool only)
        {
            //dateTimePicker1.Enabled = !only;
            dateEdit1.ReadOnly = only;
        }

        public override void SetValue(string only)
        {
            if (DateTime.TryParse(only, out var date))
                dateEdit1.EditValue = date;
            else
                dateEdit1.EditValue = null;
        }

        public void SetValue(DateTime only)
        {
            dateEdit1.EditValue = only;
        }

        public override void SetItems(List<string> only)
        {
            ;
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
