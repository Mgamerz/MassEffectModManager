using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LegendaryExplorerCore.Helpers;
using ME3TweaksCoreWPF.UI;
using ME3TweaksModManager.modmanager.localizations;
using ME3TweaksModManager.modmanager.objects;
using ME3TweaksModManager.modmanager.objects.alternates;
using ME3TweaksModManager.modmanager.objects.mod.editor;
using PropertyChanged;

namespace ME3TweaksModManager.modmanager.usercontrols.moddescinieditor.alternates
{
    public class AlternateEventArgs : EventArgs
    {
        public AlternateOption Option { get; set; }
    }

    /// <summary>
    /// User control that allows editing a list of AlternateOptions.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class AlternateItemsControl : UserControl
    {
        public AlternateItemsControl()
        {
            LoadCommands();
            InitializeComponent();
        }

        public RelayCommand MoveAlternateDownCommand { get; set; }
        public RelayCommand MoveAlternateUpCommand { get; set; }
        public RelayCommand DeleteAlternateCommand { get; set; }
        public RelayCommand CloneAlternateCommand { get; set; }

        private void LoadCommands()
        {
            DeleteAlternateCommand = new RelayCommand(RemoveAlternate, x => true);
            CloneAlternateCommand = new RelayCommand(CloneAlternate, x => true);
            MoveAlternateUpCommand = new RelayCommand(MoveAlternateUp, CanMoveAlternateUp);
            MoveAlternateDownCommand = new RelayCommand(MoveAlternateDown, CanMoveAlternateDown);
        }

        private void CloneAlternate(object obj)
        {
            if (obj is AlternateOption option && DataContext is AlternateBuilderBaseControl baseControl)
            {
                AlternateOption clonedAlt = null;
                if (option is AlternateDLC adlc)
                    clonedAlt = adlc.CopyForEditor();
                if (option is AlternateFile af)
                    clonedAlt = af.CopyForEditor();

                if (clonedAlt != null)
                {
                    // Find new name we can use

                    int i = 2;
                    string testName = clonedAlt.FriendlyName;
                    if (testName.Length > 1 && int.TryParse(testName[^1].ToString(), out var endDigit) && testName[^2] == ' ')
                    {
                        testName = testName[..^2]; // Cut off the ' X' number suffix
                        i = endDigit;
                    }

                    while (true)
                    {
                        var testNameSuffixed = $@"{testName} {i}";
                        if (!baseControl.Alternates.Any(x => x.FriendlyName.CaseInsensitiveEquals(testNameSuffixed)))
                        {
                            clonedAlt.FriendlyName = testNameSuffixed;
                            break;
                        }

                        i++;
                    }
                    baseControl.Alternates.Insert(baseControl.Alternates.IndexOf(option) + 1, clonedAlt);
                }
            }
        }

        private void MoveAlternateUp(object obj)
        {
            if (obj is AlternateOption option && DataContext is AlternateBuilderBaseControl baseControl)
            {
                var startingIndex = baseControl.Alternates.IndexOf(option);
                baseControl.Alternates.RemoveAt(startingIndex); // e.g. Remove from position 3
                baseControl.Alternates.Insert(startingIndex - 1, option);
            }
        }

        private void MoveAlternateDown(object obj)
        {
            if (obj is AlternateOption option && DataContext is AlternateBuilderBaseControl baseControl)
            {
                var startingIndex = baseControl.Alternates.IndexOf(option);
                baseControl.Alternates.RemoveAt(startingIndex); // e.g. Remove from position 3
                baseControl.Alternates.Insert(startingIndex + 1, option);
            }
        }

        private bool CanMoveAlternateDown(object obj)
        {
            if (obj is AlternateOption option && DataContext is AlternateBuilderBaseControl baseControl)
            {
                return baseControl.Alternates.IndexOf(option) < baseControl.Alternates.Count - 1; // -1 for 0 indexing. Less than covers the next -1.
            }
            return false;
        }

        private bool CanMoveAlternateUp(object obj)
        {
            if (obj is AlternateOption option && DataContext is AlternateBuilderBaseControl baseControl)
            {
                return baseControl.Alternates.IndexOf(option) > 0;
            }
            return false;
        }

        private void RemoveAlternate(object obj)
        {
            if (obj is AlternateOption option && DataContext is AlternateBuilderBaseControl baseControl)
            {
                var deleteAlternate = M3L.ShowDialog(Window.GetWindow(this), M3L.GetString(M3L.string_mde_deleteAlternateNamed, option.FriendlyName), M3L.GetString(M3L.string_confirmDeletion), MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (deleteAlternate == MessageBoxResult.Yes)
                {
                    baseControl.Alternates.Remove(option);
                }
            }
        }


        private void HandleMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            // This forces scrolling to bubble up
            // cause expander eats it
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = (((Control)sender).TemplatedParent ?? ((Control)sender).Parent) as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        private void DescriptorPropertyChanged(object sender, EventArgs e)
        {
            if (IsLoaded)
            {
                if (sender is TextBox tb && tb.DataContext is MDParameter mdp && tb.Tag is AlternateOption o)
                {
                    if (mdp.Key == AlternateKeys.ALTSHARED_KEY_FRIENDLYNAME)
                    {
                        o.FriendlyName = tb.Text;
                    }
                    else if (mdp.Key == AlternateKeys.ALTSHARED_KEY_OPTIONGROUP)
                    {
                        o.GroupName = tb.Text;
                    }
                }
            }
        }
    }
}
