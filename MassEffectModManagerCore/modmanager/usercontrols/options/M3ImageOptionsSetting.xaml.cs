using LegendaryExplorerCore.Misc;
using ME3TweaksModManager.modmanager.localizations;

namespace ME3TweaksModManager.modmanager.usercontrols.options
{
    /// <summary>
    /// Interaction logic for M3ImageOptionsSetting.xaml
    /// </summary>
    public partial class M3ImageOptionsSetting : M3Setting
    {
        public ObservableCollectionExtended<SingleImageOption> Options { get; } = new();

        public M3ImageOptionsSetting(string titleText, params SingleImageOption[] buttons)
        {
            SettingTitle = M3L.GetString(titleText);
            Options.AddRange(buttons);

            InitializeComponent();
        }
    }
}
