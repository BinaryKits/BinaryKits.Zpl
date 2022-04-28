using BinaryKits.Zpl.Viewer.ElementDrawers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using ZXing;
using ZXing.Datamatrix;

namespace BinaryKits.Zpl.Viewer.UnitTest
{
    [TestClass]
    public class DrawerTest
    {
        [TestMethod]
        public void FontAssignment()
        {
            string zplString = @"
^XA
^FO20, 20
^A1N,40, 30 ^FD西瓜^FS
^FO20, 50
^A0N,40, 30 ^FDABCDEFG^FS
^XZ";

            var drawOptions = new DrawerOptions()
            {
                FontLoader = fontName =>
                {
                    if (fontName == "0")
                    {
                        //typeface = SKTypeface.FromFile(@"swiss-721-black-bt.ttf");
                        return SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
                    }
                    else if (fontName == "1")
                    {
                        return SKTypeface.FromFamilyName("SIMSUN", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
                    }

                    return SKTypeface.Default;
                }
            };
            IPrinterStorage printerStorage = new PrinterStorage();
            var drawer = new ZplElementDrawer(printerStorage, drawOptions);

            var analyzer = new ZplAnalyzer(printerStorage);
            var analyzeInfo = analyzer.Analyze(zplString);

            foreach (var labelInfo in analyzeInfo.LabelInfos)
            {
                var imageData = drawer.Draw(labelInfo.ZplElements, 300, 300, 8);
                File.WriteAllBytes("test.png", imageData);
            }
        }

        [TestMethod]
        public void FormatHandling()
        {
            string zplString = @"
^XA
^DFETIQUE-1^FS
^PRC
^LH0,0^FS
^LL408
^MD0
^MNY
^LH0,0^FS
^FO120,141^A0N,27,23^CI13^FR^FN999^FS
^BY2,3.0^FO213,7^BCN,80,N,Y,N^FR^FN997^FS
^FO313,95^A0N,35,23^CI13^FR^FB105,2,0,L^FN997^FS
^FO40,141^A0N,27,33^CI13^FR^FDP/N :^FS
^XZ


^XA
^XFETIQUE-1.ZPL
^FN999^FDC19755BA01:F9111^FS
^FN997^FD3758292^FS
^PQ1,0,1,N
^XZ
^FX";
            IPrinterStorage printerStorage = new PrinterStorage();
            var drawer = new ZplElementDrawer(printerStorage);

            var analyzer = new ZplAnalyzer(printerStorage);
            var analyzeInfo = analyzer.Analyze(zplString);

            foreach (var labelInfo in analyzeInfo.LabelInfos)
            {
                var imageData = drawer.Draw(labelInfo.ZplElements, 300, 300, 8);
                File.WriteAllBytes("merge-test.png", imageData);
            }
        }
    }
}
