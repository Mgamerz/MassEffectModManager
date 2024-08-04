using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ME3TweaksModManager.modmanager.usercontrols.moddescinieditor
{
    /// <summary>
    /// Interaction logic for HeadmorphsItemControl.xaml
    /// </summary>
    public partial class HeadmorphsItemControl : UserControl
    {
        public HeadmorphsItemControl()
        {
            InitializeComponent();
        }

        private void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // This forces scrolling to bubble up
            // cause expander eats it
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = MouseWheelEvent;
                eventArg.Source = sender;
                var parent = (((Control)sender).TemplatedParent ?? ((Control)sender).Parent) as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }
    }
}
