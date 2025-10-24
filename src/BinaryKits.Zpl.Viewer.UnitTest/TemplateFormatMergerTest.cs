using System;
using System.Collections.Generic;
using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Viewer.UnitTest
{
    [TestClass]
    public class TemplateFormatMergerTest
    {
        [TestMethod]
        public void FormatMerging()
        {
            // Given
            const int FieldNumber = 999;
            ZplFont defaultFont = ZplConstants.Font.Default;

            LabelInfo templateFormat = new LabelInfo()
            {
                DownloadFormatName = "format",
                ZplElements = new[]
                {
                    new ZplFieldNumber(
                        FieldNumber, new ZplTextField(null, 50, 200, defaultFont))
                }
            };
            LabelInfo dataLabel = new LabelInfo()
            {
                ZplElements = new ZplElementBase[]
                {
                    new ZplTextField("one", 50, 100, defaultFont),
                    new ZplRecallFormat("format.zpl"),
                    new ZplRecallFieldNumber(FieldNumber, "two"),
                    new ZplTextField("three", 50, 300, defaultFont),
                }
            };
            List<LabelInfo> rawLabels = new List<LabelInfo> { templateFormat, dataLabel };

            FormatMerger formatMerger = new FormatMerger();

            // When
            List<LabelInfo> merged = formatMerger.MergeFormats(rawLabels);

            // Then
            Assert.HasCount(1, merged);
            ZplElementBase[] zplElements = merged[0].ZplElements;

            Assert.HasCount(3, zplElements);
            Assert.AreEqual("one", ((ZplTextField)zplElements[0]).Text);
            Assert.AreEqual("two", ((ZplTextField)zplElements[1]).Text);
            Assert.AreEqual("three", ((ZplTextField)zplElements[2]).Text);
        }

        [TestMethod]
        public void FormatNotFound()
        {
            LabelInfo dataLabel = new LabelInfo()
            {
                ZplElements = new ZplElementBase[]
                {
                    new ZplRecallFormat("format.zpl"),
                }
            };
            List<LabelInfo> rawLabels = new List<LabelInfo> { dataLabel };

            FormatMerger formatMerger = new FormatMerger();

            Assert.Throws<InvalidOperationException>(() => formatMerger.MergeFormats(rawLabels));
        }
    }
}
