using System.Windows;
using IniParser.Model;
using LegendaryExplorerCore.Misc;
using ME3TweaksCoreWPF.UI;
using ME3TweaksModManager.modmanager.localizations;
using ME3TweaksModManager.modmanager.objects;

namespace ME3TweaksModManager.modmanager.usercontrols.moddescinieditor
{
    /// <summary>
    /// Interaction logic for MultilistEditorControl.xaml
    /// </summary>
    public partial class MultilistEditorControl : ModdescEditorControlBase
    {
        /// <summary>
        /// Associated task header
        /// </summary>
        public ModJob.JobHeader? Header
        {
            get => (ModJob.JobHeader?)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(@"Header", typeof(ModJob.JobHeader?), typeof(MultilistEditorControl));
        /// <summary>
        /// The job for this multilist editor control
        /// </summary>
        public ModJob AttachedJob { get; set; }

        public MultilistEditorControl()
        {
            LoadCommands();
            InitializeComponent();
        }

        private void LoadCommands()
        {
            AddNewListCommand = new GenericCommand(AddNewList, CanAddNewList);
            DeleteListCommand = new RelayCommand(DeleteMultilist);
        }

        public RelayCommand DeleteListCommand { get; set; }

        private void DeleteMultilist(object obj)
        {
            if (obj is MDMultilist md)
            {
                var result = M3L.ShowDialog(Window.GetWindow(this),
                                    M3L.GetString(M3L.string_dialog_deleteMultilistConfirm, md.MultilistId),
                                    M3L.GetString(M3L.string_confirmDeletion), MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    Multilists.Remove(md);
                    ReindexLists();
                }
            }
        }

        private void ReindexLists()
        {
            for (int i = 0; i < Multilists.Count; i++)
            {
                Multilists[i].MultilistId = i + 1;
            }
        }

        private bool CanAddNewList()
        {
            if (AttachedJob == null && Header != null)
            {
                AttachedJob = EditingMod.GetJob(Header.Value);
            }

            return AttachedJob != null;
        }

        private void AddNewList()
        {
            Multilists.Add(new MDMultilist()
            {
                MultilistId = Multilists.Count + 1 // mulitlist id indexing begins at 1. Don't @ me
            });
        }

        public GenericCommand AddNewListCommand { get; set; }

        // Kind of a hack. This is a list of multilist indexes. They're passed through the data context
        public ObservableCollectionExtended<MDMultilist> Multilists { get; } = new ObservableCollectionExtended<MDMultilist>();

        public override void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!HasLoaded)
            {
                AttachedJob = Header != null ? EditingMod?.GetJob(Header.Value) : null;

                if (AttachedJob != null)
                {
                    Multilists.ReplaceAll(AttachedJob.MultiLists.Select(x =>
                    {
                        var ml = new MDMultilist();
                        int i = 0;
                        ml.Files.ReplaceAll(x.Value.Select(y => new SingleMultilistEditorItem()
                        {
                            ItemIndex = ++i,
                            Value = y
                        }));
                        ml.MultilistId = x.Key;
                        return ml;
                    }));
                }
                else
                {
                    Multilists.ClearEx();
                }

                HasLoaded = true;
            }
        }

        public override void Serialize(IniData ini)
        {
            foreach (var ml in Multilists)
            {
                var values = ml.Files.Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(x => x.Value);
                ini[Header.ToString()][$@"multilist{ml.MultilistId}"] = string.Join(';', values);
            }
        }
    }

    public class MDMultilist
    {
        public ObservableCollectionExtended<SingleMultilistEditorItem> Files { get; } = new ObservableCollectionExtended<SingleMultilistEditorItem>();
        public int MultilistId { get; set; }
    }
}
