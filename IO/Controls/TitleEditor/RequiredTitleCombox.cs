using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertLib.Controls
{
    public class RequiredTitleCombox: TitleCombox
    {
     
        public RequiredTitleCombox() {
            this.SetNotContentBackColor(Color.FromArgb(238, 232, 170));
            this.init();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Size = new System.Drawing.Size(329, 25);
            // 
            // RequiredTitleCombox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.Name = "RequiredTitleCombox";
            this.ResumeLayout(false);

        }
    }
}
