using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertLib.Controls
{
    public class CheckItem
    {
        public CheckItem()
        {

        }

        public CheckItem(string value, bool isChecked)
        {
            Value = value;
            Checked = isChecked;
        }

        public string Value { get;  set; }

        public bool Checked { get; set; }


    }
}
