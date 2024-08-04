using ME3TweaksCoreWPF.Targets;
using ME3TweaksModManager.modmanager;
using static ME3TweaksCore.Helpers.MExtendedClassGenerators;

namespace ME3TweaksModManager.me3tweakscoreextended
{
    public static class M3ModifiedFileObject
    {
        /// <summary>
        /// Allows restoring texture packages in dev mode.
        /// </summary>
        public static GenerateModifiedFileObjectDelegate GenerateModifiedFileObject { get; set; } = InternalGenerateModifiedFileObject;

        /// <summary>
        /// Allows restoring texture packages in dev mode.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="target"></param>
        /// <param name="restoreBasegamefileConfirmationCallback"></param>
        /// <param name="notifyRestoringFileCallback"></param>
        /// <param name="notifyRestoredCallback"></param>
        /// <returns></returns>
        private static ModifiedFileObject InternalGenerateModifiedFileObject(string filePath, GameTarget target, Func<string, bool> restoreBasegamefileConfirmationCallback, Action notifyRestoringFileCallback, Action<object> notifyRestoredCallback, string md5 = null)
        {
            // By default we cannot restore texture modded files like this.
            return new ModifiedFileObjectWPF(filePath, target, Settings.DeveloperMode, restoreBasegamefileConfirmationCallback, notifyRestoringFileCallback, notifyRestoredCallback, md5);
        }
    }
}
