using System.Windows.Controls;
using ME3TweaksCoreWPF.UI;

namespace ME3TweaksModManager.modmanager.usercontrols.moddescinieditor
{
    /// <summary>
    /// Editor control for a multilist
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class SingleMultilistEditorControl : UserControl
    {
        ///// <summary>
        ///// The list index (as in moddesc.ini) of this multilist
        ///// </summary>
        //public int ListIndex { get; set; }
        public SingleMultilistEditorControl()
        {
            LoadCommands();
            InitializeComponent();
        }

        private void LoadCommands()
        {
            AddFileCommand = new GenericCommand(AddFile);
            DeleteFileCommand = new RelayCommand(DeleteFile);
        }

        private void AddFile()
        {
            if (DataContext is MDMultilist ml)
            {
                if (!ml.Files.Any() || !string.IsNullOrWhiteSpace(ml.Files.Last().Value))
                {
                    ml.Files.Add(new SingleMultilistEditorItem()
                    {
                        ItemIndex = ml.Files.Count + 1 // we use 1 based UI indexing
                    });
                }
            }
        }

        private void DeleteFile(object obj)
        {
            if (DataContext is MDMultilist ml && obj is SingleMultilistEditorItem smlei)
            {
                ml.Files.Remove(smlei);
                ReindexItems(ml);
            }
        }

        private void ReindexItems(MDMultilist ml)
        {
            // 1 based indexing (Item 1, Item 2...)
            for (int i = 0; i < ml.Files.Count; i++)
            {
                ml.Files[i].ItemIndex = i + 1;
            }
        }

        public GenericCommand AddFileCommand { get; set; }
        public RelayCommand DeleteFileCommand { get; set; }

        //public ObservableCollectionExtended<SingleMultilistEditorItem> ml.Files { get; } = new ObservableCollectionExtended<SingleMultilistEditorItem>();
    }

    [AddINotifyPropertyChangedInterface]
    public class SingleMultilistEditorItem
    {
        // The index of the file in the list
        public int ItemIndex { get; set; }
        // The value of the multilist item
        public string Value { get; set; }
    }
}
