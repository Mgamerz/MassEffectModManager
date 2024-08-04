using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using ME3TweaksModManager.modmanager.objects.alternates;

namespace ME3TweaksModManager.modmanager.usercontrols.modinstaller
{
    /// <summary>
    /// Interaction logic for SingleAlternateOptionPanel.xaml
    /// </summary>
    public partial class SingleAlternateOptionPanel : UserControl
    {
        /// <summary>
        /// Option that is being displayed by this UI object
        /// </summary>
        public AlternateOption Option
        {
            get => (AlternateOption)GetValue(OptionProperty);
            set => SetValue(OptionProperty, value);
        }

        public static readonly DependencyProperty OptionProperty = DependencyProperty.Register(@"Option", typeof(AlternateOption), typeof(SingleAlternateOptionPanel));


        public SingleAlternateOptionPanel()
        {
            InitializeComponent();
        }

        private void ToolTipIsOpening(object sender, ToolTipEventArgs e)
        {
            if (sender is SingleAlternateOptionPanel saop && saop.Tag is AlternateOptionSelector aos && aos.SuppressingTooltip && saop.DataContext is AlternateOption ao)
            {
                e.Handled = true; // Do not open.
                Debug.WriteLine($@"Tooltip was suppressed. Source: {ao.FriendlyName}");
            }
        }
    }
}
