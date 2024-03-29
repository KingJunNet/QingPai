using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExpertLib.Controls
{
    public partial class TitleCombox : TitleControl
    {
        /// <summary>
        /// 视图数据
        /// </summary>
        public List<Object> ViewModels { get; set; }

        protected Color notContentBackColor= Color.LightYellow;

        public TitleCombox()
        {
            this.init();
        }

        public TitleCombox(Color color)
        {
            this.SetNotContentBackColor(color);
            this.init();
        }

        protected void init() {
            InitializeComponent();
            if (comboBox1.Text == "")
            {
                comboBox1.BackColor = notContentBackColor;
            }
            else
            {
                comboBox1.BackColor = Color.White;
            }

            this.comboBox1.MouseWheel += new MouseEventHandler(ComboBox_MouseWheel);
        }

        protected void ComboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs e1 = e as HandledMouseEventArgs;//需要强转，否则无法设置handeld
            e1.Handled = true;
        }

        public void SetTextChange(EventHandler handler)
        {
            comboBox1.TextChanged += handler;
        }

        public void SetTextUpdate(EventHandler handler)
        {
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            comboBox1.TextUpdate += handler;
        }

        public void SetNotContentBackColor(Color color) {
            this.notContentBackColor = color;
        }

        public override string Text => comboBox1.Text;

        public override string Value()
        {
            return comboBox1.Text.Trim();
        }

        public override void SetReadOnly(bool only)
        {
            comboBox1.Enabled = !only;
            
        }

        public override void SetValue(string only)
        {
            comboBox1.Text = only.Trim();
        }

        public override void SetItems(List<string> only)
        {
            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();

            comboBox1.Items.AddRange(only.ToArray());
            comboBox1.EndUpdate();
        }

        public void SetItems(IEnumerable<string> only)
        {
            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();

            comboBox1.Items.AddRange(only.ToArray());
            comboBox1.EndUpdate();
        }

        public void SetViewmModels(List<Object> vms)
        {
            this.ViewModels = vms;
            List<string> values = vms.Select(item => item.ToString()).ToList();
            this.SetItems(values);
        }

        protected void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                comboBox1.BackColor = notContentBackColor;
            }
            else
            {
                comboBox1.BackColor = Color.White;
            }
        }
    }
}
