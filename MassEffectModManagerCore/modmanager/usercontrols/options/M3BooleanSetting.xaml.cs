using LegendaryExplorerCore.UnrealScript.Language.Tree;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using ME3TweaksModManager.modmanager.localizations;

namespace ME3TweaksModManager.modmanager.usercontrols.options
{
    /// <summary>
    /// Interaction logic for M3BooleanSetting.xaml
    /// </summary>
    public partial class M3BooleanSetting : M3Setting
    {
        private string _settingPropertyName;
        private Type _type;
        private readonly Func<bool> _changingValueCallback;

        public M3BooleanSetting()
        {
            InitializeComponent();
        }

        public M3BooleanSetting(Type classType, string settingPropertyName, string titleKey, string descriptionKey, Func<bool> changingCallback = null)
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

#else
            SettingTitle = M3L.GetString(titleKey);
            SettingDescription = M3L.GetString(descriptionKey);
#endif
            _type = classType;
            _settingPropertyName = settingPropertyName;
            _changingValueCallback = changingCallback;
            InitializeComponent();
        }

        private void M3BooleanSetting_OnLoaded(object sender, RoutedEventArgs e)
        {
            var binding = new Binding()
            {
                Path = new PropertyPath(_type.GetProperty(_settingPropertyName)),
                NotifyOnSourceUpdated = true
            };

            DataContext = this;
            SettingCB.SetBinding(ToggleButton.IsCheckedProperty, binding);
            if (_changingValueCallback != null)
            {
                SettingCB.SourceUpdated += SettingChanged;
            }
        }

        private void SettingChanged(object sender, DataTransferEventArgs e)
        {
            if (e.Property == ToggleButton.IsCheckedProperty)
            {
                _changingValueCallback();
            }
        }
    }
}
