using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExpertLib.Controls
{
    public partial class TitleComboxPlus : TitleControl
    {
        /// <summary>
        /// 视图数据
        /// </summary>
        public List<Object> ViewModels { get; set; }

        public TitleComboxPlus()
        {
            InitializeComponent();
            if (comboBox1.Text == "")
            {
                comboBox1.BackColor = Color.LightYellow;
            }
            else
            {
                comboBox1.BackColor = Color.White;
            }
        }

        public void SetTextChange(EventHandler handler)
        {
            comboBox1.TextChanged += handler;
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
        }

        public void SetItems(IEnumerable<string> only)
        {
          
        }

        public void SetViewmModels(List<Object> vms)
        {
           
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                comboBox1.BackColor = Color.LightYellow;
            }
            else
            {
                comboBox1.BackColor = Color.White;
            }
        }

        private void TitleComboxPlus_Load(object sender, EventArgs e)
        {

        }
    }
}
