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
            List<LabelInfo> mergedLabelInfos = [];
            // Format label infos indexed by download format name
            Dictionary<string, LabelInfo> templateFormats = [];

            foreach (LabelInfo rawLabelInfo in rawLabelInfos)
            {
                List<ZplElementBase> elements = GetMergedElements(rawLabelInfo, templateFormats);
                LabelInfo labelInfo = new() { ZplElements = elements.ToArray() };

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
            List<ZplElementBase> elements = [];

            foreach (ZplElementBase zplElement in rawLabelInfo.ZplElements)
            {
                if (zplElement is ZplRecallFormat recallFormat)
                {
                    LabelInfo formatLabelInfo = GetFormatLabelInfo(recallFormat, templateFormats);
                    elements.AddRange(GetMergedElements(rawLabelInfo, formatLabelInfo));
                }
                else if (zplElement is ZplRecallFieldNumber recallFieldNumber)
                {
                    for(int i = 0; i < elements.Count; i++)
                    {
                        if (elements[i] is ZplFieldNumber fieldNumber && fieldNumber.Number == recallFieldNumber.Number)
                        {
                            ((IFormatElement)fieldNumber.FormatElement).SetTemplateContent(recallFieldNumber.Text);
                            elements[i] = fieldNumber.FormatElement;
                        }
                    }
                }
                else
                {
                    elements.Add(zplElement);
                }
            }

            return elements;
        }

        private static LabelInfo GetFormatLabelInfo(
            ZplRecallFormat recallFormat,
            Dictionary<string, LabelInfo> templateFormats)
        {
            string formatName = Path.GetFileNameWithoutExtension(recallFormat.FormatName);
            if (!templateFormats.TryGetValue(formatName, out LabelInfo value))
            {
                throw new InvalidOperationException($"Could not find format {recallFormat.FormatName}");
            }

            return value;
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
            foreach (ZplElementBase formatElement in formatLabelInfo.ZplElements)
            {
                if (formatElement is ZplFieldNumber fieldNumber)
                {
                    if (fieldNumber.FormatElement != null)
                    {
                        ZplRecallFieldNumber recallFieldNumber = (ZplRecallFieldNumber)valuesLabelInfo.ZplElements.FirstOrDefault(e =>
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
