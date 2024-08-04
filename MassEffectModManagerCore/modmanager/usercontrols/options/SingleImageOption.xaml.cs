using System.Windows;
using System.Windows.Controls;
using ME3TweaksModManager.modmanager.localizations;

namespace ME3TweaksModManager.modmanager.usercontrols.options
{
    /// <summary>
    /// Interaction logic for SingleImageOption.xaml
    /// </summary>
    public partial class SingleImageOption : UserControl
    {
        public string ButtonText { get; set; }
        public string ImagePath { get; set; }
        private readonly Action _onClick;

        public SingleImageOption(string imageAssetPath, string buttonText, Action onClick)
        {
            ImagePath = imageAssetPath;
            ButtonText = M3L.GetString(buttonText);
            _onClick = onClick;

            InitializeComponent();
        }


        private void Clicked(object sender, RoutedEventArgs e)
        {
            _onClick();
        }
    }
}
