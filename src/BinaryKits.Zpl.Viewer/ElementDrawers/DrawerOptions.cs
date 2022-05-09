using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class DrawerOptions
    {
        public Func<string, SKTypeface> FontLoader { get; set; } = DefaultFontLoader;

        public static Func<string, SKTypeface> DefaultFontLoader = fontName => {
            var typeface = SKTypeface.Default;
            if (fontName == "0")
            {
                //typeface = SKTypeface.FromFile(@"swiss-721-black-bt.ttf");
                typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
            }

            return typeface;
        };
    }
}
