using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryKits.Utility.ZPLUtility;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace ZPLUtility.UnitTest
{
    [TestClass]
    public class DownloadTest
    {
        private Bitmap GetTestBitmap()
        {
            var bitmap = new Bitmap(100, 20);
            using var canvas = Graphics.FromImage(bitmap);
            canvas.DrawLine(Pens.Black, 10, 10, 90, 10);
            return bitmap;
        }

        [TestMethod]
        public void DownloadGraphics()
        {
            using var bitmap = GetTestBitmap();

            var elements = new List<ZPLElementBase>
            {
                new ZPLGraphicBox(0, 0, 100, 100, 4),
                new ZPLDownloadGraphics('R', "SAMPLE", "GRC", bitmap),
                new ZPLRecallGraphic(100, 100, 'R', "SAMPLE", "GRC")
            };

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions { AddEmptyLineBeforeElementStart = true, TargetPrintDPI = 200, SourcePrintDPI = 200 });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^XA\n^LH0,0\n^CI28\n\n^FO0,0\n^GB100,100,4,B,0^FS\n\n~DGR:SAMPLE.GRC,260,13,\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\nFFFFFFFFFFFFFFFFFFFFFFFFF0\n\n^FO100,100\n^XGR:SAMPLE.GRC,1,1,\n^XZ", output);
        }

        [TestMethod]
        public void DownloadObjets()
        {
            using var bitmap = GetTestBitmap();

            var elements = new List<ZPLElementBase>
            {
                new ZPLGraphicBox(0, 0, 100, 100, 4),
                new ZPLDownloadObjects('R', "SAMPLE.PNG", bitmap),
                new ZPLImageMove(100, 100, 'R', "SAMPLE", "PNG")
            };

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions { AddEmptyLineBeforeElementStart = true, TargetPrintDPI = 300, SourcePrintDPI = 200 });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^XA\n^LH0,0\n^CI28\n\n^FO0,0\n^GB150,150,6,B,0^FS\n\n~DYR:SAMPLE.PNG,P,P,178,,89504E470D0A1A0A0000000D49484452000000960000001E0806000000CEE2E757000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000EC300000EC301C76FA8640000004749444154785EEDD4B10D002008454106712DF71F45A563020ABC4B5E4DF113020000000000000060ACF5DA1A5FEEDC2A8F1E8D2F776EE563FD51FBC7020000000000008022E202B511C9FCC8F65B4A0000000049454E44AE426082\n\n^FO150,150\n^IMR:SAMPLE.PNG\n^XZ", output);
        }
    }
}
