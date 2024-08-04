using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ME3TweaksModManager.modmanager.usercontrols.options
{
    [AddINotifyPropertyChangedInterface]
    public abstract class M3Setting : UserControl
    {
        public string SettingCategoryHeader { get; set; }
        public string SettingTitle
        {
            get;
            set;
        }
        public string SettingDescription
        {
            get;
            set;
        }
    }
}
