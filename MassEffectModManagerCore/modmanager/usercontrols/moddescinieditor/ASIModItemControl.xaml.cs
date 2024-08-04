using ME3TweaksCore.Helpers;
using ME3TweaksModManager.modmanager.objects.mod.moddesc;
using System.Windows.Controls;

namespace ME3TweaksModManager.modmanager.usercontrols.moddescinieditor
{
    /// <summary>
    /// Interaction logic for ASIModItemControl.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class ASIModItemControl : UserControl
    {
        public ASIModVersionEditor2 ASIMod { get; } = new();

        public ASIModItemControl()
        {
            InitializeComponent();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class ASIModVersionEditor2
    {
        #region Equality
        protected bool Equals(ASIModVersionEditor2 other)
        {
            return ASIModID == other.ASIModID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ASIModVersionEditor2)obj);
        }

        public override int GetHashCode()
        {
            return ASIModID;
        }
        #endregion

        public int ASIModID { get; set; }

        public int? ASIModVersion { get; set; }
        public bool UseLatestVersion { get; set; } = true;

        public string GenerateStruct()
        {
            var data = new Dictionary<string, string>();
            data[M3ASIVersion.GROUP_KEY_NAME] = ASIModID.ToString();
            if (!UseLatestVersion)
            {
                data[M3ASIVersion.VERSION_KEY_NAME] = ASIModVersion?.ToString() ?? @""; // This @"" will cause validation to fail, which is what we want for the editor.
            }
            return StringStructParser.BuildCommaSeparatedSplitValueList(data);
        }
    }
}
