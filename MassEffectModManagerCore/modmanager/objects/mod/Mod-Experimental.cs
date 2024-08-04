using LegendaryExplorerCore.GameFilesystem;
using ME3TweaksModManager.modmanager.objects.gametarget;

namespace ME3TweaksModManager.modmanager.objects.mod
{
    // Experimental, potentially unreliable features
    public partial class Mod
    {

        /// <summary>
        /// Mount Priority value cached from the first call to GetModMountPriority. If a mount priority could not be determined, the value will be 0.
        /// </summary>
        public int UIMountPriority { get; set; }

        /// <summary>
        /// Attempts to determine a mod's mount priority. This will return 0 on mods without DLC folders and will not be reliable for multi-DLC mods.
        /// </summary>
        /// <returns></returns>
        public int EXP_GetModMountPriority(bool isForUI = true)
        {
            if (isForUI && UIMountPriority != 0)
                return UIMountPriority;

            if (IsInArchive)
                throw new Exception(@"Cannot get a mod's mount priority while it is in an archive! This is a bug.");

            var custDLC = GetJob(ModJob.JobHeader.CUSTOMDLC);
            if (custDLC == null || custDLC.CustomDLCFolderMapping.Count == 0)
            {
                // No DLC folder
                return 0;
            }

            // We will take the mod's LOWEST mount priority
            // A multi-dlc mod may have additional dlc folders as patches or extra content. It likely will have a higher
            // mount. So we will take the lowest one
            // If mod has special flag set it will use highest. this is for multi-dlc mods that ship their own patche set

            int mountValuetoUse = isForUI && BatchInstallUseReverseMountSort ? int.MinValue : int.MaxValue;
            foreach (var fm in custDLC.CustomDLCFolderMapping)
            {
                var dlcPath = Path.Combine(ModPath, fm.Key);

                // Should be in a try catch
                try
                {
                    var mount = MELoadedDLC.GetMountPriority(dlcPath, Game);
                    if (isForUI && BatchInstallUseReverseMountSort)
                    {
                        mountValuetoUse = Math.Max(mount, mountValuetoUse);
                    }
                    else
                    {
                        mountValuetoUse = Math.Min(mount, mountValuetoUse);
                    }
                }
                catch (Exception ex)
                {
                    M3Log.Error($@"Unable to get mount priority value for mod {ModName}'s DLC folder {dlcPath}: {ex.Message}. We will skip this mount.");
                }

            }

            // If no mount value was set, return the default value of 0
            if (mountValuetoUse == (isForUI && BatchInstallUseReverseMountSort ? int.MinValue : int.MaxValue))
                mountValuetoUse = 0;

            if (isForUI)
            {
                UIMountPriority = mountValuetoUse;
            }

            return mountValuetoUse;
        }

        public bool IsInstalledToTarget { get; set; }

        /// <summary>
        /// Attempts to determine if a mod is installed to the given target. This is not reliable.
        /// </summary>
        /// <param name="target"></param>
        public void DetermineIfInstalled(GameState state)
        {
            if (!Settings.ShowInstalledModsInLibrary)
                return;

            foreach (var metaCMM in state.DLCMetaCMMs)
            {
                if (metaCMM.Value == null)
                    continue;
                if (!string.IsNullOrWhiteSpace(metaCMM.Value.ModdescSourceHash) && metaCMM.Value.ModdescSourceHash == ModDescHash)
                {
                    IsInstalledToTarget = true;
                    return;
                }
            }

            foreach (var file in state.BasegameHashes)
            {
                if (file.Value.moddeschashes.Any(x => x == ModDescHash))
                {
                    // At least one merge file was composed of this moddesc hash.
                    IsInstalledToTarget = true;
                    return;
                }
            }

            IsInstalledToTarget = false;
        }
    }
}
