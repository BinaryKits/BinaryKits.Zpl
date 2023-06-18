using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
    }
}
