using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer
{
    /// <summary>
    /// Helper class responsible of merging several LabelInfo based on format data (templating).
    /// </summary>
    public class FormatMerger : IFormatMerger
    {
        /// <inheritdoc />
        public List<LabelInfo> MergeFormats(List<LabelInfo> rawLabelInfos)
        {
            var mergedLabelInfos = new List<LabelInfo>();
            // Format label infos indexed by download format name
            var templateFormats = new Dictionary<string, LabelInfo>();

            foreach (var rawLabelInfo in rawLabelInfos)
            {
                var elements = GetMergedElements(rawLabelInfo, templateFormats);
                var labelInfo = new LabelInfo { ZplElements = elements.ToArray() };

                if (rawLabelInfo.DownloadFormatName != null)
                {
                    templateFormats.Add(rawLabelInfo.DownloadFormatName, labelInfo);
                }
                else
                {
                    mergedLabelInfos.Add(labelInfo);
                }
            }

            return mergedLabelInfos;
        }

        private static List<ZplElementBase> GetMergedElements(
            LabelInfo rawLabelInfo,
            Dictionary<string, LabelInfo> templateFormats)
        {
            var elements = new List<ZplElementBase>();

            foreach (ZplElementBase element in rawLabelInfo.ZplElements)
            {
                if (element is ZplRecallFormat recallFormat)
                {
                    LabelInfo formatLabelInfo = GetFormatLabelInfo(recallFormat, templateFormats);
                    elements.AddRange(GetMergedElements(rawLabelInfo, formatLabelInfo));
                }
                else if (element is not ZplRecallFieldNumber)
                {
                    elements.Add(element);
                }
            }

            return elements;
        }

        private static LabelInfo GetFormatLabelInfo(
            ZplRecallFormat recallFormat,
            Dictionary<string, LabelInfo> templateFormats)
        {
            string formatName = Path.GetFileNameWithoutExtension(recallFormat.FormatName);
            if (!templateFormats.ContainsKey(formatName))
            {
                throw new InvalidOperationException($"Could not find format {recallFormat.FormatName}");
            }

            return templateFormats[formatName];
        }

        /// <summary>
        /// Merge two label infos into a single list of elements.
        /// </summary>
        /// <param name="valuesLabelInfo">Label info providing values</param>
        /// <param name="formatLabelInfo">Label info providing format</param>
        private static IEnumerable<ZplElementBase> GetMergedElements(
            LabelInfo valuesLabelInfo,
            LabelInfo formatLabelInfo)
        {
            foreach (var formatElement in formatLabelInfo.ZplElements)
            {
                if (formatElement is ZplFieldNumber fieldNumber)
                {
                    if (fieldNumber.FormatElement != null)
                    {
                        var recallFieldNumber = (ZplRecallFieldNumber)valuesLabelInfo.ZplElements.FirstOrDefault(e =>
                            (e as ZplRecallFieldNumber)?.Number == fieldNumber.Number);
                        if (recallFieldNumber != null)
                        {
                            ((IFormatElement)fieldNumber.FormatElement).SetTemplateContent(recallFieldNumber.Text);
                            yield return fieldNumber.FormatElement;
                        }
                    }
                }
                else
                {
                    yield return formatElement;
                }
            }
        }
    }
}