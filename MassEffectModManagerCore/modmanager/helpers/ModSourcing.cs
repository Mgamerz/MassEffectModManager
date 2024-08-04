using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ME3TweaksCore.GameFilesystem;

namespace ME3TweaksModManager.modmanager.helpers
{
    public static class ModSourcing
    {
        /// <summary>
        /// Attempts to locate a preferable source for sourcing a mod.
        /// </summary>
        /// <returns></returns>
        public static string GetGamePatchModFolder(MEGame game)
        {
            string patchName = game switch
            {
                MEGame.LE1 => @"DLC_MOD_LE1CP",
                MEGame.LE2 => @"DLC_MOD_LE2PATCH",
                MEGame.LE3 => @"DLC_MOD_LE3PATCH",
                _ => null
            };

            if (patchName == null)
                return null;

            var modLibraryFolder = M3LoadedMods.FindDLCModFolderInLibrary(game, patchName);
            if (modLibraryFolder == null) // This probably should be run on the UI thread. But it makes my code ugly, so I'm not going to do that. 06/21/2024
            {
                var target = MainWindow.Instance.GetCurrentTarget(game);
                if (target.TextureModded)
                    return null; // We cannot find a usable source

                var installedDLC = Path.Combine(target.GetDLCPath(), patchName);
                if (Directory.Exists(installedDLC))
                    modLibraryFolder = installedDLC;
            }

            return modLibraryFolder;
        }
    }
}
