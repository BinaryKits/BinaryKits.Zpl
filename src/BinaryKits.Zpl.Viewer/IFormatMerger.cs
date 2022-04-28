using System.Collections.Generic;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer
{
    /// <summary>
    /// Helper service responsible of merging <see cref="LabelInfo"/>s
    /// based on "format" data (templating).
    /// </summary>
    public interface IFormatMerger
    {
        /// <summary>
        /// Merge format label infos (with ^DF command)
        /// with values label infos (with ^XF command)
        /// based on field numbers (^FN command).
        /// </summary>
        /// <param name="rawLabelInfos">Raw label infos as read by ZplAnalyzer</param>
        /// <returns>Merged label info list</returns>
        public List<LabelInfo> MergeFormats(List<LabelInfo> rawLabelInfos);
    }
}