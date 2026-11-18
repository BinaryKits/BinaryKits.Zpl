using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

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

            var zplData = File.ReadAllText("DownloadGraphicsZ64.txt");
            CollectionAssert.AreEqual(DecompressZ64(zplData), DecompressZ64(output));
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
        public void DownloadObjects()
        {
            var imageData = File.ReadAllBytes("Zpl.png");
            var options = new ZplRenderOptions
            {
                AddEmptyLineBeforeElementStart = true,
                TargetPrintDpi = 300,
                SourcePrintDpi = 200
            };

            var elements = new List<ZplElementBase>
            {
                new ZplGraphicBox(0, 0, 100, 100, 4),
                new ZplDownloadObjects('R', "SAMPLE.PNG", imageData),
                new ZplImageMove(100, 100, 'R', "SAMPLE", "PNG")
            };

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(options);

            Debug.WriteLine(output);
            Assert.IsNotNull(output);

            var match = Regex.Match(output, @"~DYR:SAMPLE\.PNG,P,P,(\d+),,([0-9A-F]+)");
            Assert.IsTrue(match.Success);

            var objectData = Convert.FromHexString(match.Groups[2].Value);
            Assert.AreEqual(int.Parse(match.Groups[1].Value), objectData.Length);
            using var sourceImage = SKBitmap.Decode(imageData);
            using var downloadedImage = SKBitmap.Decode(objectData);
            Assert.IsNotNull(sourceImage);
            Assert.IsNotNull(downloadedImage);
            Assert.AreEqual((int)Math.Round(sourceImage.Width * options.ScaleFactor), downloadedImage.Width);
            Assert.AreEqual((int)Math.Round(sourceImage.Height * options.ScaleFactor), downloadedImage.Height);
        }

        private static byte[] DecompressZ64(string zplData)
        {
            var match = Regex.Match(zplData, @":Z64:([^:\r\n]+):[0-9A-Fa-f]+");
            Assert.IsTrue(match.Success);

            using var compressedData = new MemoryStream(Convert.FromBase64String(match.Groups[1].Value));
            using var decompressor = new ZLibStream(compressedData, CompressionMode.Decompress);
            using var decompressedData = new MemoryStream();
            decompressor.CopyTo(decompressedData);
            return decompressedData.ToArray();
        }
    }
}
