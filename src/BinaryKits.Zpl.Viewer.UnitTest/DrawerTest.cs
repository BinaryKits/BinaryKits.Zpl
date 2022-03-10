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
    }
}
