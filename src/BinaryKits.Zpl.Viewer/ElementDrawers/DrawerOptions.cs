using SkiaSharp;
using System;
using System.Linq;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class DrawerOptions
    {
        public Func<string, SKTypeface> FontLoader { get; set; } = DefaultFontLoader;

        public SKEncodedImageFormat RenderFormat { get; set; } = SKEncodedImageFormat.Png;

        public int RenderQuality { get; set; } = 80;

        /// <summary>
        /// Applies label over a white background after rendering all elements
        /// </summary>
        public bool OpaqueBackground { get; set; } = false;

        /// <summary>
        /// Renders the label as pdf
        /// </summary>
        public bool PdfOutput { get; set; } = false;

        public bool ReplaceDashWithEnDash { get; set; } = true;

        public bool Antialias { get; set; } = true;

        private static readonly string[] fontStack0 = new string[] {
            "Swis721 Cn BT",
            "Nimbus Sans",
            "Arial",
            "Helvetica Neue",
            "Roboto Condensed"
        };

        private static readonly string[] fontStackA = new string[] {
            "DejaVu Sans Mono",
            "Cascadia Code",
            "Consolas",
            "SF Mono",
            "Droid Sans Mono"
        };

        private static readonly SKTypeface typeface0;
        private static readonly SKTypeface typefaceA;

        static DrawerOptions()
        {
            var skFontManager = SKFontManager.Default;
            var fontFamilies = skFontManager.FontFamilies;

            typeface0 = SKTypeface.Default;
            typefaceA = SKTypeface.Default;

            foreach (var familyName in fontStack0)
            {
                if (fontFamilies.Contains(familyName))
                {
                    typeface0 = SKTypeface.FromFamilyName(
                        familyName,
                        SKFontStyleWeight.Bold,
                        SKFontStyleWidth.SemiCondensed,
                        SKFontStyleSlant.Upright
                    );
                    break;
                }
            }

            foreach (var familyName in fontStackA)
            {
                if (fontFamilies.Contains(familyName))
                {
                    typefaceA = SKTypeface.FromFamilyName(
                        familyName,
                        SKFontStyleWeight.Normal,
                        SKFontStyleWidth.Normal,
                        SKFontStyleSlant.Upright
                    );
                    break;
                }
            }
        }

        private static Func<string, SKTypeface> DefaultFontLoader = fontName => {
            if (fontName == "0")
            {
                return typeface0;
            }

            return typefaceA;
        };
    }
}
