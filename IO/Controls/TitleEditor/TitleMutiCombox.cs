using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExpertLib.Controls
{
    public partial class TitleMutiCombox : TitleControl
    {
        /// <summary>
        /// 视图数据
        /// </summary>
        public List<Object> ViewModels { get; set; }

        protected Color notContentBackColor= Color.LightYellow;

        public TitleMutiCombox()
        {
            this.init();
        }

        public TitleMutiCombox(Color color)
        {
            this.SetNotContentBackColor(color);
            this.init();
        }

        protected void init() {
            InitializeComponent();
            if (multiComboBox1.ComboBox.Text == "")
            {
                multiComboBox1.ComboBox.BackColor = notContentBackColor;
            }
            else
            {
                multiComboBox1.ComboBox.BackColor = Color.White;
            }

            this.multiComboBox1.ComboBox.MouseWheel += new MouseEventHandler(ComboBox_MouseWheel);
            this.multiComboBox1.ComboBox.TextChanged += new EventHandler(comboBox1_TextChanged);
            //this.multiComboBox1.SelectedItemsText = "init";
        }

        protected void ComboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs e1 = e as HandledMouseEventArgs;//需要强转，否则无法设置handeld
            e1.Handled = true;
        }

        public void SetTextChange(EventHandler handler)
        {
            multiComboBox1.ComboBox.TextChanged += handler;
        }

        public void SetSelectedChanged(EventHandler handler)
        {
            multiComboBox1.ComboBox.SelectedIndexChanged += handler;
        }

        public void SetTextUpdate(EventHandler handler)
        {
            this.multiComboBox1.ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            multiComboBox1.ComboBox.TextUpdate += handler;
        }

        public void SetNotContentBackColor(Color color) {
            this.notContentBackColor = color;
        }

        public override string Text => multiComboBox1.ComboBox.Text;

        public override string Value()
        {
            return multiComboBox1.ComboBox.Text.Trim();
        }

        public override void SetReadOnly(bool only)
        {
            multiComboBox1.ComboBox.Enabled = !only;
            
        }

        public override void SetValue(string only)
        {
            multiComboBox1.ComboBox.Text = only.Trim();
        }

        public override void SetItems(List<string> only)
        {
            multiComboBox1.ComboBox.BeginUpdate();
            multiComboBox1.ComboBox.Items.Clear();

            multiComboBox1.ComboBox.Items.AddRange(only.ToArray());
            multiComboBox1.ComboBox.EndUpdate();
        }

        public void SetCheckedItems(List<CheckItem> checkItems)
        {
            this.multiComboBox1.CheckItems = checkItems;
            this.multiComboBox1.LoadCheckedItms();
        }

        public string GetSelectedItemsText()
        {
            return this.multiComboBox1.SelectedItemsText;

        }

        public void SetViewmModels(List<Object> vms)
        {
            this.ViewModels = vms;
            List<string> values = vms.Select(item => item.ToString()).ToList();
            this.SetItems(values);
        }



        public void SetNotAllowInput()
        {
            multiComboBox1.ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }

        protected void comboBox1_TextChanged(object sender, EventArgs e)
        {
            string content = multiComboBox1.ComboBox.Text.Trim();
            if (!string.Equals(content, multiComboBox1.SelectedItemsText))
            {
                multiComboBox1.ComboBox.Text = multiComboBox1.SelectedItemsText;
            }

            if (content == "")
            {
                multiComboBox1.ComboBox.BackColor = notContentBackColor;
            }
            else
            {
                multiComboBox1.ComboBox.BackColor = Color.White;
            }
        }
    }
}
