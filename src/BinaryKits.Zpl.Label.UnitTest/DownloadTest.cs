using BinaryKits.Zpl.Label.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BinaryKits.Zpl.Label.UnitTest
{
    [TestClass]
    public class DownloadTest
    {
        [TestMethod]
        [DeploymentItem(@"ZplData/Zpl.png")]
        [DeploymentItem(@"ZplData/DownloadGraphicsACS.txt")]
        public void DownloadGraphicsACS()
        {
            var imageData = File.ReadAllBytes("Zpl.png");

            var elements = new List<ZplElementBase>
            {
                new ZplGraphicBox(0, 0, 100, 100, 4),
                new ZplDownloadGraphics('R', "SAMPLE", imageData,ZplCompressionScheme.ACS),
                new ZplRecallGraphic(100, 100, 'R', "SAMPLE")
            };

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions
            {
                AddEmptyLineBeforeElementStart = true,
                TargetPrintDpi = 200,
                SourcePrintDpi = 200
            });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);

            var zplData = File.ReadAllText("DownloadGraphicsACS.txt");
            Assert.AreEqual(zplData, output);
        }

        [TestMethod]
        [DeploymentItem(@"ZplData/Zpl.png")]
        [DeploymentItem(@"ZplData/DownloadGraphicsZ64.txt")]
        [DeploymentItem(@"ZplData/DownloadGraphicsZ64_net472.txt")]
        public void DownloadGraphicsZ64()
        {
            var imageData = File.ReadAllBytes("Zpl.png");

            var elements = new List<ZplElementBase>
            {
                new ZplGraphicBox(0, 0, 100, 100, 4),
                new ZplDownloadGraphics('R', "SAMPLE", imageData,ZplCompressionScheme.Z64),
                new ZplRecallGraphic(100, 100, 'R', "SAMPLE")
            };

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions
            {
                AddEmptyLineBeforeElementStart = true,
                TargetPrintDpi = 200,
                SourcePrintDpi = 200
            });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);

#if NET5_0_OR_GREATER
            var zplData = File.ReadAllText("DownloadGraphicsZ64.txt");
#else
            var zplData = File.ReadAllText("DownloadGraphicsZ64_net472.txt");
#endif
            Assert.AreEqual(zplData, output);
        }

        [TestMethod]
        [DeploymentItem(@"ZplData/Zpl.png")]
        [DeploymentItem(@"ZplData/DownloadGraphicsB64.txt")]
        public void DownloadGraphicsB64()
        {
            var imageData = File.ReadAllBytes("Zpl.png");

            var elements = new List<ZplElementBase>
            {
                new ZplGraphicBox(0, 0, 100, 100, 4),
                new ZplDownloadGraphics('R', "SAMPLE", imageData,ZplCompressionScheme.B64),
                new ZplRecallGraphic(100, 100, 'R', "SAMPLE")
            };

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions
            {
                AddEmptyLineBeforeElementStart = true,
                TargetPrintDpi = 200,
                SourcePrintDpi = 200
            });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);

            var zplData = File.ReadAllText("DownloadGraphicsB64.txt");
            Assert.AreEqual(zplData, output);
        }

        [TestMethod]
        [DeploymentItem(@"ZplData/Zpl.png")]
        [DeploymentItem(@"ZplData/DownloadObject.txt")]
        public void DownloadObjects()
        {
            var imageData = File.ReadAllBytes("Zpl.png");

            var elements = new List<ZplElementBase>
            {
                new ZplGraphicBox(0, 0, 100, 100, 4),
                new ZplDownloadObjects('R', "SAMPLE.PNG", imageData),
                new ZplImageMove(100, 100, 'R', "SAMPLE", "PNG")
            };

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions
            {
                AddEmptyLineBeforeElementStart = true,
                TargetPrintDpi = 300,
                SourcePrintDpi = 200
            });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);

            var zplData = File.ReadAllText("DownloadObject.txt");
            Assert.AreEqual(zplData, output);
        }

        [TestMethod]
        [DataRow(200, 200, 1)]
        [DataRow(150, 300, 2)]
        [DataRow(300, 150, 0.5)]
        public void RenderWithScaleFactorCalculatesCorrectDimensions(int sourceDpi, int targetDpi, double expectedMultiplier)
        {
            int initialWidth = 8;
            var dummyImageData = CreatePng(initialWidth, 8);
            var element = new ZplDownloadGraphics('R', "TESTIMG", dummyImageData, ZplCompressionScheme.None);

            var renderOptions = new ZplRenderOptions
            {
                SourcePrintDpi = sourceDpi,
                TargetPrintDpi = targetDpi
            };

            var zplLines = element.Render(renderOptions).ToList();
            var headerLine = zplLines.FirstOrDefault();

            Assert.IsNotNull(headerLine);
            Assert.StartsWith("~DGR:TESTIMG.GRF,", headerLine);

            var pieces = headerLine.Split(',');
            Assert.IsGreaterThanOrEqualTo(3, pieces.Length, "ZPL header string is missing fields.");

            int actualTotalBytes = int.Parse(pieces[1]);
            int actualBytesPerRow = int.Parse(pieces[2]);

            int expectedWidth = (int)System.Math.Round(initialWidth * expectedMultiplier);
            int calculatedBytesPerRow = expectedWidth % 8 > 0 ? expectedWidth / 8 + 1 : expectedWidth / 8;

            Assert.AreEqual(calculatedBytesPerRow, actualBytesPerRow, $"Bytes per row mismatch for multiplier {expectedMultiplier}");
            Assert.IsGreaterThan(0, actualTotalBytes, "Total byte count must be a positive integer value.");
        }

        private static byte[] CreatePng(int width, int height)
        {
            using (var image = new Image<Rgba32>(width, height))
            {
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, new PngEncoder());
                    return ms.ToArray();
                }
            }
        }
    }
}
