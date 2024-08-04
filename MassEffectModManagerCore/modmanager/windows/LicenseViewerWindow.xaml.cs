using System.Windows;
using ME3TweaksCoreWPF.UI;
using ME3TweaksModManager.extensions;

namespace ME3TweaksModManager.modmanager.windows
{
    /// <summary>
    /// Interaction logic for LicenseViewerWindow.xaml
    /// </summary>
    public partial class LicenseViewerWindow : Window, IClosableWindow
    {
        public LicenseViewerWindow(string licenseText)
        {
            LicenseText = licenseText;
            LoadCommands();
            InitializeComponent();
            this.ApplyDarkNetWindowTheme();
        }

        public string LicenseText { get; set; }

        private void LoadCommands()
        {
            CloseCommand = new GenericCommand(Close);
        }

        public GenericCommand CloseCommand { get; set; }
        public bool AskToClose()
        {
            Close();
            return true;
        }
    }
}
