using System.ComponentModel;
using System.Windows;
using ME3TweaksCore.NativeMods;

namespace ME3TweaksModManager.modmanager.helpers
{
    public static class SettingsChangeListener
    {
        public static void OnSettingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(Settings.DeveloperMode))
            {
                ASIManager.Options.DevMode = Settings.DeveloperMode;
            }
            else if (e.PropertyName is nameof(Settings.GenerationSettingLE) or nameof(Settings.GenerationSettingOT))
            {
                if (MainWindow.Instance != null)
                {
                    // Must run on UI thread.
                    Application.Current.Dispatcher.Invoke(()=> MainWindow.Instance.UpdateMenuTargets());
                }
            }
        }
    }
}
