using LegendaryExplorerCore.Misc;
using ME3TweaksCore.GameFilesystem;
using ME3TweaksCore.Services.Shared.BasegameFileIdentification;

namespace ME3TweaksModManager.modmanager.objects.gametarget
{
    /// <summary>
    /// Container object that contains information about a target that can be used to determine if a mod is installed or not.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// DLC mod information
        /// </summary>
        public CaseInsensitiveDictionary<MetaCMM> DLCMetaCMMs { get; init; }
        /// <summary>
        /// Hashes of mergemod target files
        /// </summary>
        public CaseInsensitiveDictionary<BasegameFileRecord> BasegameHashes { get; init; }
        /// <summary>
        /// Game target this is for
        /// </summary>
        public GameTarget Target { get; set; }

        public static GameState Default => new() { DLCMetaCMMs = [], BasegameHashes = [] };
    }
}
