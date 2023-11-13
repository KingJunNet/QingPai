using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExpertLib.Controls
{
    public partial class TitleCombox : TitleControl
    {
        public TitleCombox()
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
    }
}
