using LegendaryExplorerCore.Compression;
using LegendaryExplorerCore.Misc;
using Newtonsoft.Json;

namespace ME3TweaksModManager.modmanager.objects.mod.merge
{
    /// <summary>
    /// Contains methods to determine if a class is a vanilla class in a specified game.
    /// </summary>
    internal class VanillaClasses
    {
        private static CaseInsensitiveDictionary<List<MEGame>> VanillaClassMap;
        public static bool IsVanillaClass(string className, MEGame game)
        {
            if (VanillaClassMap == null)
            {
                var vanillaTextLZ = M3Utilities.ExtractInternalFileToStream(@"ME3TweaksModManager.modmanager.objects.mod.merge.vanillaclasses.json.lzma");

                MemoryStream ms = new MemoryStream();
                LZMA.DecompressLZMAStream(vanillaTextLZ, ms);
                ms.Position = 0;

                var vanillaText = new StreamReader(ms).ReadToEnd();
                VanillaClassMap = JsonConvert.DeserializeObject<CaseInsensitiveDictionary<List<MEGame>>>(vanillaText);
            }

            if (VanillaClassMap.TryGetValue(className, out var games) && games.Contains(game))
                return true;

            return false;
        }
    }
}
