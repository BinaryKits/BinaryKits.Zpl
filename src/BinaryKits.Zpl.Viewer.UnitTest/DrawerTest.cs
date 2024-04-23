using BinaryKits.Zpl.Viewer.ElementDrawers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;


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
            DefaultPrint(zplString, "font-assign.png", 300, 300, 8, drawOptions);
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
            DefaultPrint(zplString, "merge-test.png", 100, 100, 8);
        }
        [TestMethod]
        public void InvertColor()
        {
            // Example in ZPL manual
            string test1 = @"
^XA
^FO10,100
^GB70,70,70,,3^FS
^FO110,100
^GB70,70,70,,3^FS
^FO210,100
^GB70,70,70,,3^FS
^FO310,100
^GB70,70,70,,3^FS
^FO17,110
^CF0,70,93
^FR
^FDREVERSE^FS
^XZ
";
            // from https://github.com/BinaryKits/BinaryKits.Zpl/pull/64
            string test2 = @"
^XA
^FR
^FO50,50
^GB100,100,10,W,5^FS
^FR
^FO200,50
^GB100,100,10,W,5^FS
^FO100,120
^GB30,25,10,B,2^FS
^FO250,120
^GB30,25,10,B,2^FS
^FO130,180
^GB90,90,45,B,8^FS
^FR
^FO75,300
^GB30,20,10,W,0^FS
^FR
^FO265,300
^GB30,20,10,W,0^FS
^FR
^FO105,320
^GB160,20,10,W,0^FS
^FR
^FO120,310
^GB10,20,5,B,0^FS
^FR
^FO140,310
^GB10,20,5,B,0^FS
^FR
^FO160,310
^GB10,20,5,B,0^FS
^FR
^FO180,310
^GB10,20,5,B,0^FS
^FR
^FO200,310
^GB10,20,5,B,0^FS
^FR
^FO220,310
^GB10,20,5,B,0^FS
^FR
^FO240,310
^GB10,20,5,B,0^FS
^FO150,330
^GB70,90,45,B,0^FS
^XZ
";
            DefaultPrint(test1, "inverted1.png", 100, 100, 8);
            DefaultPrint(test2, "inverted2.png");
        }

        /// <summary>
        /// Generic printer to test zpl -> png output
        /// </summary>
        /// <param name="zpl"></param>
        /// <param name="outputFilename">PNG filename ex: "file.png"</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="ppmm"></param>
        /// <param name="options"></param>
        private void DefaultPrint(string zpl, string outputFilename, double width=101.6, double height=152.4, int ppmm=8, DrawerOptions options=null) {
            IPrinterStorage printerStorage = new PrinterStorage();
            var drawer = new ZplElementDrawer(printerStorage, options);

            var analyzer = new ZplAnalyzer(printerStorage);
            var analyzeInfo = analyzer.Analyze(zpl);

            foreach (var labelInfo in analyzeInfo.LabelInfos)
            {
                var imageData = drawer.Draw(labelInfo.ZplElements, width, height, ppmm);
                File.WriteAllBytes(outputFilename, imageData);
            }
        }
    }
}
