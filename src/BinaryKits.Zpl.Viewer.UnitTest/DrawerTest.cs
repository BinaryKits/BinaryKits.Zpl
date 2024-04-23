using BinaryKits.Zpl.Viewer.ElementDrawers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkiaSharp;


namespace BinaryKits.Zpl.Viewer.UnitTest
{
    [TestClass]
    public class DrawerTest
    {
        [TestMethod]
        public void FontAssignment()
        {
            string zplString = Common.LoadZPL("font-assign");

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
            Common.DefaultPrint(zplString, "font-assign.png", 300, 300, 8, drawOptions);
        }

        [TestMethod]
        public void FormatHandling()
        {
            string zplString = Common.LoadZPL("merge");
            Common.DefaultPrint(zplString, "merge.png", 100, 100, 8);
        }

        [TestMethod]
        public void InvertColor()
        {
            // Example in ZPL manual
            string test1 = Common.LoadZPL("invert1");
            // from https://github.com/BinaryKits/BinaryKits.Zpl/pull/64
            string test2 = Common.LoadZPL("invert2");

            Common.DefaultPrint(test1, "inverted1.png", 100, 100, 8);
            Common.DefaultPrint(test2, "inverted2.png");
        }
    }
}