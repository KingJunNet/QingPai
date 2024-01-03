using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpertLib.Controls.TitleEditor
{
    public partial class ComboBoxMax: ComboBox
    {
        protected override void WndProc(ref Message m)
        {
            int WM_MOUSEWHEEL = 0x020A;
            if (m.Msg == WM_MOUSEWHEEL)
            {
                // 解决“多选下拉框值根据鼠标滚轮滚动而变化显示错误”问题
            }
            else
            {

                base.WndProc(ref m);
            }
        }
    }
}
