using ME3TweaksModManager.modmanager.localizations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ME3TweaksCoreWPF.UI;

namespace ME3TweaksModManager.modmanager.usercontrols.options
{
    /// <summary>
    /// Interaction logic for M3DirectorySetting.xaml
    /// </summary>
    public partial class M3DirectorySetting : M3Setting
    {
        private readonly Type _type;
        private readonly string _settingPropertyName;
        private Func<string> _getWatermark;

        public ICommand ButtonClickedCommand { get; init; }

        public M3DirectorySetting(Type classType, string settingPropertyName,string titleKey, string descriptionKey, Func<string> getDirectoryWatermarkValue, string buttonKey, Action<object> buttonClickedCallback)
        {
#if DEBUG
            // In debug builds we support using non localized strings. 
            var locType = typeof(M3L);

            var locTitleField = locType.GetField(titleKey);
            if (locTitleField == null)
            {
                SettingTitle = titleKey;
            }
            else
            {
                SettingTitle = M3L.GetString(titleKey);
            }

            var locDescriptionField = locType.GetField(descriptionKey);
            if (locDescriptionField == null)
            {
                SettingDescription = descriptionKey;
            }
            else
            {
                SettingDescription = M3L.GetString(descriptionKey);
            }

            var locButtonField = locType.GetField(buttonKey);
            if (locButtonField == null)
            {
                ButtonString = buttonKey;
            }
            else
            {
                ButtonString = M3L.GetString(buttonKey);
            }

#else
            SettingTitle = M3L.GetString(titleKey);
            SettingDescription = M3L.GetString(descriptionKey);
            ButtonString = M3L.GetString(buttonKey);
#endif
            _getWatermark = getDirectoryWatermarkValue;

            UpdateWaterMark();

            _type = classType;
            _settingPropertyName = settingPropertyName;
            ButtonClickedCommand = new RelayCommand(buttonClickedCallback);
            InitializeComponent();
        }

        public string DirectoryWatermark { get; set; }
        public string ButtonString { get; set; }

        public void UpdateWaterMark()
        {
            DirectoryWatermark = _getWatermark();
        }

        private void M3DirectorySetting_OnLoaded(object sender, RoutedEventArgs e)
        {
            //var binding = new Binding()
            //{
            //    Path = new PropertyPath(_type.GetProperty(_settingPropertyName))
            //};

            // SettingWMTB.SetBinding(TextBox.TextProperty, binding);
        }
    }
}
