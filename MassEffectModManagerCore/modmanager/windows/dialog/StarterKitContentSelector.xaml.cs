using System.Threading.Tasks;
using System.Windows;
using LegendaryExplorerCore.Helpers;
using LegendaryExplorerCore.Misc;
using ME3TweaksCore.GameFilesystem;
using ME3TweaksCore.ME3Tweaks.M3Merge;
using ME3TweaksCore.ME3Tweaks.StarterKit;
using ME3TweaksModManager.extensions;
using ME3TweaksModManager.modmanager.helpers;
using ME3TweaksModManager.modmanager.localizations;
using ME3TweaksModManager.modmanager.objects;
using ME3TweaksModManager.modmanager.objects.mod;
using ME3TweaksModManager.modmanager.objects.starterkit;
using ME3TweaksModManager.modmanager.usercontrols.moddescinieditor;

namespace ME3TweaksModManager.modmanager.windows.dialog
{
    /// <summary>
    /// Interaction logic for StarterKitContentSelector.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class StarterKitContentSelector : Window
    {
        /// <summary>
        /// The mod we are operating on
        /// </summary>
        public Mod SelectedMod { get; set; }

        public ObservableCollectionExtended<StarterKitAddinFeature> AvailableFeatures { get; } = new();

        /// <summary>
        /// If the selected mod should be reloaded when the window closes
        /// </summary>
        public bool ReloadMod { get; private set; }

        /// <summary>
        /// Bottom left text to display
        /// </summary>
        public string OperationText { get; set; } = M3L.GetString(M3L.string_selectAnOperation);

        public StarterKitContentSelector(Window owner, Mod selectedMod)
        {
            Owner = owner;
            SelectedMod = selectedMod;
            InitializeComponent();
            this.ApplyDarkNetWindowTheme();

            Title += $@" - {selectedMod.ModName}";

            AvailableFeatures.Add(new StarterKitAddinFeature(M3L.GetString(M3L.string_addStartupFile), AddStartupFile, validGames: new[] { MEGame.ME2, MEGame.ME3, MEGame.LE1, MEGame.LE2, MEGame.LE3 }));
            AvailableFeatures.Add(new StarterKitAddinFeature(M3L.GetString(M3L.string_addPlotManagerData), AddPlotManagerData, validGames: new[] { MEGame.ME1, MEGame.ME2, MEGame.ME3, MEGame.LE1, MEGame.LE2, MEGame.LE3 }));

            // LE3 Features
            string[] game3Hench = new[] { @"Ashley", @"EDI", @"Garrus", @"Kaidan", @"Marine", @"Prothean", @"Liara", @"Tali" };
            foreach (var hench in game3Hench)
            {
                AvailableFeatures.Add(new StarterKitAddinFeature(M3L.GetString(M3L.string_interp_addSquadmateOutfitMergeX, StarterKitAddins.GetHumanName(hench)), () => AddSquadmateMergeOutfit(hench), validGames: new[] { MEGame.ME3, MEGame.LE3 }));
            }

            string[] le2Hench = new[] { @"Vixen", @"Leading", @"Professor", @"Garrus", @"Convict", @"Grunt", @"Tali", @"Mystic", @"Assassin", @"Geth", @"Thief", @"Veteran" };
            foreach (var hench in le2Hench)
            {
                AvailableFeatures.Add(new StarterKitAddinFeature(M3L.GetString(M3L.string_interp_addSquadmateOutfitMergeX, StarterKitAddins.GetHumanName(hench)), () => AddSquadmateMergeOutfit(hench), validGames: new[] { MEGame.LE2 }));
            }

            AvailableFeatures.Add(new StarterKitAddinFeature(M3L.GetString(M3L.string_interp_addModSettingsMenuStub), AddModSettingsStub, validGames: new[] { MEGame.LE1, MEGame.LE3 }));
        }

        private void AddModSettingsStub()
        {
            var dlcFolderPath = GetDLCFolderPath();
            if (dlcFolderPath == null) return; // Abort

            OperationText = M3L.GetString(M3L.string_addingModSettingsMenuData);
            Task.Run(() =>
            {
                OperationInProgress = true;
                List<Action<DuplicatingIni>> moddescAddinDelegates = new List<Action<DuplicatingIni>>();
                if (SelectedMod.Game == MEGame.LE1)
                {
                    StarterKitAddins.AddLE1ModSettingsMenu(SelectedMod, SelectedMod.Game, Path.Combine(SelectedMod.ModPath, dlcFolderPath), moddescAddinDelegates);
                }
                else if (SelectedMod.Game == MEGame.LE3)
                {
                    StarterKitAddins.AddLE3ModSettingsMenu(SelectedMod, SelectedMod.Game, Path.Combine(SelectedMod.ModPath, dlcFolderPath), moddescAddinDelegates);
                }

                if (moddescAddinDelegates.Any())
                {
                    var iniData = DuplicatingIni.LoadIni(SelectedMod.ModDescPath);
                    foreach (var del in moddescAddinDelegates)
                    {
                        del(iniData);
                    }

                    iniData.WriteToFile(SelectedMod.ModDescPath);
                    ReloadMod = true;
                }
            }).ContinueWithOnUIThread(x =>
            {
                OperationInProgress = false;

                if (x.Exception == null)
                {
                    OperationText = M3L.GetString(M3L.string_addedModSettingsMenuData);
                }
                else
                {
                    OperationText = M3L.GetString(M3L.string_interp_failedToAddModSettingsMenuDataX, x.Exception.Message);
                }
            });


        }

        /// <summary>
        /// If an operation is currently in progress
        /// </summary>
        public bool OperationInProgress { get; set; }

        private void AddSquadmateMergeOutfit(string hench)
        {
            // Test backup

            if (!BackupService.GetBackupStatus(SelectedMod.Game).BackedUp)
            {
                M3L.ShowDialog(this,
                    M3L.GetString(M3L.string_dialog_squadmateMergeBackupRequired),
                    M3L.GetString(M3L.string_noBackupAvailable), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var dlcFolderPath = GetDLCFolderPath();
            if (dlcFolderPath == null) return; // Abort
            var henchHumanName = StarterKitAddins.GetHumanName(hench);

            OperationText = M3L.GetString(M3L.string_interp_addingSquadmateOutfitMergeFilesForX, henchHumanName);
            Task.Run(() =>
            {
                OperationInProgress = true;
                return StarterKitAddins.GenerateSquadmateMergeFiles(SelectedMod.Game, hench, dlcFolderPath, SQMOutfitMerge.LoadSquadmateMergeInfo(SelectedMod.Game, dlcFolderPath) , ModSourcing.GetGamePatchModFolder);
            }).ContinueWithOnUIThread(x =>
            {
                OperationInProgress = false;
                if (x.Exception == null)
                {
                    if (x.Result != null)
                        OperationText = x.Result;
                    else
                        OperationText = M3L.GetString(M3L.string_interp_addedSquadmateOutfitMergeFilesForX, henchHumanName);
                }
                else
                {
                    OperationText = M3L.GetString(M3L.string_interp_failedToAddSquadmateOutfitMergeFilesForHenchXY, henchHumanName, x.Exception.Message);
                }
            });
        }

        private void AddPlotManagerData()
        {
            var dlcFolderPath = GetDLCFolderPath();
            if (dlcFolderPath == null) return; // Abort

            OperationText = M3L.GetString(M3L.string_interp_addingPlotManagerData);
            Task.Run(() =>
            {
                OperationInProgress = true;
                StarterKitAddins.GeneratePlotData(SelectedMod.Game, Path.Combine(SelectedMod.ModPath, dlcFolderPath));
            }).ContinueWithOnUIThread(x =>
            {
                OperationInProgress = false;
                if (x.Exception == null)
                {
                    OperationText = M3L.GetString(M3L.string_interp_addedPlotManagerData);
                }
                else
                {
                    OperationText = M3L.GetString(M3L.string_interp_failedToAddPlotManagerDataX, x.Exception.Message);
                }
            });
        }

        private void AddStartupFile()
        {
            var dlcFolderPath = GetDLCFolderPath();
            if (dlcFolderPath == null) return; // Abort
            OperationText = M3L.GetString(M3L.string_interp_addingStartupFile);
            Task.Run(() =>
            {
                OperationInProgress = true;
                StarterKitAddins.AddStartupFile(SelectedMod.Game, Path.Combine(SelectedMod.ModPath, dlcFolderPath));
            }).ContinueWithOnUIThread(x =>
            {
                OperationInProgress = false;
                if (x.Exception == null)
                {
                    OperationText = M3L.GetString(M3L.string_interp_addedStartupFile);
                }
                else
                {
                    OperationText = M3L.GetString(M3L.string_interp_failedToAddStartupFileX, x.Exception.Message);
                }
            });
        }

        private string GetDLCFolderPath()
        {
            var dlcJob = SelectedMod.GetJob(ModJob.JobHeader.CUSTOMDLC);
            if (dlcJob == null || dlcJob.CustomDLCFolderMapping.Keys.Count == 0) return null; // Not found

            var sourceDirs = dlcJob.CustomDLCFolderMapping;

            if (sourceDirs.Count > 1)
            {
                // We have to select
                var response = DropdownSelectorDialog.GetSelection<string>(this, M3L.GetString(M3L.string_selectDLCMod), dlcJob.CustomDLCFolderMapping.Keys.ToList(), M3L.GetString(M3L.string_selectADLCFolderToAddAStartupFileTo), @"");
                if (response is string str)
                {
                    return Path.Combine(SelectedMod.ModPath, str);
                }

                return null;
            }

            return Path.Combine(SelectedMod.ModPath, sourceDirs.Keys.FirstOrDefault());
        }
    }
}
