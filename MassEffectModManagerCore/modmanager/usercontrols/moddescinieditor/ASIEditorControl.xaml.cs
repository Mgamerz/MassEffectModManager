using System.Windows;
using IniParser.Model;
using LegendaryExplorerCore.Misc;
using ME3TweaksCore.Helpers;
using ME3TweaksCore.NativeMods;
using ME3TweaksCoreWPF.UI;
using ME3TweaksModManager.modmanager.objects.mod;
using ME3TweaksModManager.modmanager.objects.mod.moddesc;

namespace ME3TweaksModManager.modmanager.usercontrols.moddescinieditor
{
    /// <summary>
    /// Interaction logic for ASIEditorControl.xaml
    /// </summary>
    public partial class ASIEditorControl : ModdescEditorControlBase
    {
        public ObservableCollectionExtended<ASIModVersionEditor2> ASIMods { get; } = new();
        public ASIEditorControl()
        {
            InitializeComponent();
            LoadCommands();
        }

        private void LoadCommands()
        {
            AddASICommand = new GenericCommand(AddASI);
            RemoveASICommand = new RelayCommand(RemoveASIMod);
        }
        public RelayCommand RemoveASICommand { get; set; }
        public GenericCommand AddASICommand { get; set; }

        private void AddASI()
        {
            ASIMods.Add(new ASIModVersionEditor2());
        }


        public override void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!HasLoaded)
            {
                foreach (var m in EditingMod.ASIModsToInstall)
                {
                    ASIMods.Add(new ASIModVersionEditor2()
                    {
                        ASIModID = m.ASIGroupID,
                        ASIModVersion = m.Version,
                        UseLatestVersion = m.Version == null
                    });
                }

                HasLoaded = true;
            }
        }

        public override void Serialize(IniData ini)
        {
            var mods = ASIMods.Where(x => x.ASIModID > 0).Distinct().ToList();
            if (mods.Any())
            {
                List<string> structs = new List<string>();
                foreach (var asiMod in mods)
                {
                    structs.Add(asiMod.GenerateStruct());
                }

                ini[Mod.MODDESC_HEADERKEY_ASIMODS][Mod.MODDESC_DESCRIPTOR_ASI_ASIMODSTOINSTALL] = $@"({string.Join(',', structs)})";
            }
        }

        public void RemoveASIMod(object obj)
        {
            if (obj is ASIModVersionEditor2 aed)
            {
                ASIMods.Remove(aed);
            }
        }

        public class ASIModVersionEditor : ASIModVersion
        {
            // public M3ASIVersion M3Base { get; set; }
            public ASIModVersion ManifestMod { get; set; }

            /// <summary>
            /// The version set by the user
            /// </summary>
            public int? Version { get; set; }

            public static ASIModVersionEditor Create(MEGame game, M3ASIVersion baseObj)
            {
                ASIModVersionEditor v = new ASIModVersionEditor();

                var asiModsForGame = ASIManager.GetASIModsByGame(game);
                var group = asiModsForGame.FirstOrDefault(x => x.UpdateGroupId == baseObj.ASIGroupID);
                if (group == null)
                {
                    M3Log.Error($@"Unable to find ASI group {baseObj.ASIGroupID}");
                    return null; // Not found!
                }
                if (baseObj.Version != null)
                {
                    v.ManifestMod = group.Versions.FirstOrDefault(x => x.Version == baseObj.Version);
                }
                else
                {
                    v.ManifestMod = group.LatestVersion;
                }

                if (v.ManifestMod == null)
                {
                    M3Log.Error($@"Unable to find version {baseObj.Version?.ToString() ?? @"(Latest)"} in ASI group {baseObj.ASIGroupID} {group.Versions.First().Name}"); // do not localize
                    return null; // Specific version was not found!!
                }

                return v;
            }

            public string GenerateStruct()
            {
                var data = new Dictionary<string, string>();
                data[M3ASIVersion.GROUP_KEY_NAME] = ManifestMod.OwningMod.UpdateGroupId.ToString();
                if (Version != null)
                {
                    data[M3ASIVersion.VERSION_KEY_NAME] = ManifestMod.Version.ToString();
                }
                return $@"({StringStructParser.BuildCommaSeparatedSplitValueList(data)})";
            }
        }
    }
}
