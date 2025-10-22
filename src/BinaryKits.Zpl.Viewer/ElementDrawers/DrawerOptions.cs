using SkiaSharp;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class DrawerOptions
    {
        public Func<string, SKTypeface> FontLoader { get; set; } = defaultFontLoader;

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

        /// <summary>
        /// Gets or sets a value indicating whether dashes should be replaced with en dash.
        /// </summary>
        public bool ReplaceDashWithEnDash { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether underscores in text should be replaced with en space.
        /// </summary>
        public bool ReplaceUnderscoreWithEnSpace { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether antialiasing is enabled.
    /// </summary>
    public bool Antialias { get; set; } = true;

        private static readonly string[] fontStack0 = [
            "Swis721 Cn BT",
            "Nimbus Sans",
            "Arial",
            "Helvetica Neue",
            "Roboto Condensed"
        ];

        private static readonly string[] fontStackA = [
            "DejaVu Sans Mono",
            "Cascadia Code",
            "Consolas",
            "SF Mono",
            "Droid Sans Mono"
        ];

        private static readonly SKTypeface typeface0;
        private static readonly SKTypeface typefaceA;

        static DrawerOptions()
        {
            SKFontManager skFontManager = SKFontManager.Default;
            IEnumerable<string> fontFamilies = skFontManager.FontFamilies;

            typeface0 = SKTypeface.Default;
            typefaceA = SKTypeface.Default;

            foreach (string familyName in fontStack0)
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

            foreach (string familyName in fontStackA)
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

        private static readonly Func<string, SKTypeface> defaultFontLoader = fontName => {
            if (fontName == "0")
            {
                return typeface0;
            }

            return typefaceA;
        };
    }
}
