using SkiaSharp;
using System;
using System.Diagnostics;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class DrawerOptions
    {
        public Func<string, SKTypeface> FontLoader { get; set; } = DefaultFontLoader;

        public SKEncodedImageFormat RenderFormat { get; set; } = SKEncodedImageFormat.Png;

        public int RenderQuality { get; set; } = 80;

        public bool ReplaceDashWithEnDash { get; set; } = true;

        public static Func<string, SKTypeface> DefaultFontLoader = fontName => {
            var typeface = SKTypeface.Default;
            if (fontName == "0")
            {
                //typeface = SKTypeface.FromFile(@"swiss-721-black-bt.ttf");
                typeface = SKTypeface.FromFamilyName("Helvetica", SKFontStyleWeight.Bold, SKFontStyleWidth.SemiCondensed, SKFontStyleSlant.Upright);
            }
            else
            {
                typeface = SKTypeface.FromFamilyName("DejaVu Sans Mono", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
            }

            return typeface;
        };
    }
}
