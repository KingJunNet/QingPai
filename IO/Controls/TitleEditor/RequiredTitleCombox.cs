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
    }
}
