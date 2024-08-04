using LegendaryExplorerCore.Misc;
using ME3TweaksModManager.modmanager.objects.mod.merge.v1;
using Newtonsoft.Json;

namespace ME3TweaksModManager.modmanager.objects.mod.merge
{
    public interface IMergeMod
    {
        /// <summary>
        /// Name of the merge mod file, relative to the root of the MergeMods folder in the mod directory
        /// </summary>
        public string MergeModFilename { get; set; }

        /// <summary>
        /// Game this merge mod is for
        /// </summary>
        public MEGame Game { get; set; }

        /// <summary>
        /// List of asset files that are part of this merge mod
        /// </summary>
        public CaseInsensitiveDictionary<MergeAsset> Assets { get; set; }

        /// <summary>
        /// The version of this merge mod
        /// </summary>
        public int MergeModVersion { get; set; }

        /// <summary>
        /// Applies the merge mod to the target
        /// </summary>
        /// <returns></returns>
        public bool ApplyMergeMod(MergeModPackage mmp, Action<int> mergeWeightDelegate);
        /// <summary>
        /// Get the number of total merge operations this mod can apply
        /// </summary>
        /// <returns></returns>
        public int GetMergeCount();
        /// <summary>
        /// Get the weight of all merges for this merge mod for accurate progress tracking.
        /// </summary>
        /// <returns></returns>
        public int GetMergeWeight();

        /// <summary>
        /// Extracts this m3m file to the specified folder
        /// </summary>
        /// <param name="outputfolder"></param>
        public void ExtractToFolder(string outputfolder);

        /// <summary>
        /// Returns a list of strings of files that will be modified by this merge file.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetMergeFileTargetFiles();

        /// <summary>
        /// Releases memory for loaded assets that were loaded on-disk from m3ms. Does nothing for memory m3ms
        /// </summary>
        public void ReleaseAssets();

        /// <summary>
        /// Gets the list of target exports of this merge mod
        /// </summary>
        /// <returns></returns>
        CaseInsensitiveDictionary<List<string>> GetMergeModTargetExports();
    }

    public interface IMergeModCommentable
    {
        /// <summary>
        /// The comment on this field. Optional.
        /// </summary>
        [JsonProperty(@"comment")]
        public string Comment { get; set; }
    }

    /// <summary>
    /// Interface for all merge types
    /// </summary>
    public abstract class MergeModUpdateBase : IMergeModCommentable
    {
        /// <summary>
        /// Validation method for update types. Throw an exception if validation fails.
        /// </summary>
        public virtual void Validate() { }

        /// <summary>
        /// The file info that this merge is taking place in
        /// </summary>
        [JsonIgnore] 
        public MergeFileChange1 Parent;

        /// <summary>
        /// The encompassing merge mod object
        /// </summary>
        [JsonIgnore] 
        public MergeMod1 OwningMM => Parent.OwningMM;

        /// <summary>
        /// The comment on this field. Optional.
        /// </summary>
        [JsonProperty(@"comment")]
        public string Comment { get; set; }
    }

}
