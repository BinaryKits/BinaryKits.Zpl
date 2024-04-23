using SkiaSharp;
using System;
using System.Diagnostics;
using System.Linq;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class DrawerOptions
    {
        public Func<string, SKTypeface> FontLoader { get; set; } = DefaultFontLoader;

        public SKEncodedImageFormat RenderFormat { get; set; } = SKEncodedImageFormat.Png;

        /// <summary>
        /// Applies label over a white background after rendering all elements
        /// </summary>
        public bool OpaqueBackground { get; set; } = false;
        
        /// <summary>
        /// Renders the label as pdf
        /// </summary>
        public bool PdfOutput { get; set; } = false;

        public int RenderQuality { get; set; } = 80;

        public bool ReplaceDashWithEnDash { get; set; } = true;

        public static Func<string, SKTypeface> DefaultFontLoader = fontName => {
            var typeface = SKTypeface.Default;
            var skFontManager = SKFontManager.Default;
            var fontFamilies = skFontManager.FontFamilies;
            
            if (fontName == "0")
            {
                if (fontFamilies.Contains("Helvetica"))
                {
                    typeface = SKTypeface.FromFamilyName(
                        "Helvetica",
                        SKFontStyleWeight.Bold,
                        SKFontStyleWidth.SemiCondensed,
                        SKFontStyleSlant.Upright
                    );
                }
                else if (fontFamilies.Contains("Roboto Condensed"))
                {
                    typeface = SKTypeface.FromFamilyName(
                        "Roboto Condensed",
                        SKFontStyleWeight.Bold,
                        SKFontStyleWidth.Normal,
                        SKFontStyleSlant.Upright
                    );
                }
                else if (fontFamilies.Contains("Swis721 BT"))
                {
                    //Note: Zebra Swis721 BT (tt0003m_.ttf) is not as bold and condensed as Labelary
                    //Note: swiss-721-bt-bold.ttf is not as condensed as Labelary
                    //typeface = SKTypeface.FromFile(@"swiss-721-black-bt.ttf");
                    typeface = SKTypeface.FromFamilyName(
                        "Swis721 BT"
                    );
                }
                else if (fontFamilies.Contains("Arial"))
                {
                    typeface = SKTypeface.FromFamilyName(
                        "Arial",
                        SKFontStyleWeight.Bold,
                        SKFontStyleWidth.Condensed,
                        SKFontStyleSlant.Upright
                    );
                }
                else
                {
                    //let the system provide a fallback for Helvetica
                    typeface = SKTypeface.FromFamilyName(
                        "Helvetica",
                        SKFontStyleWeight.Bold,
                        SKFontStyleWidth.SemiCondensed,
                        SKFontStyleSlant.Upright
                    );
                }
            }
            else
            {
                if (fontFamilies.Contains("DejaVu Sans Mono"))
                {
                    typeface = SKTypeface.FromFamilyName(
                        "DejaVu Sans Mono",
                        SKFontStyleWeight.Normal,
                        SKFontStyleWidth.Normal,
                        SKFontStyleSlant.Upright
                    );
                }
                else if (fontFamilies.Contains("Courier New"))
                {
                    typeface = SKTypeface.FromFamilyName(
                        "Courier New",
                        SKFontStyleWeight.Medium,
                        SKFontStyleWidth.Normal,
                        SKFontStyleSlant.Upright
                    );
                }
                else if (fontFamilies.Contains("Courier"))
                {
                    typeface = SKTypeface.FromFamilyName(
                        "Courier",
                        SKFontStyleWeight.Medium,
                        SKFontStyleWidth.Normal,
                        SKFontStyleSlant.Upright
                    );
                }
                else
                {
                    //let the system provide a fallback for DejaVu
                    typeface = SKTypeface.FromFamilyName(
                        "DejaVu Sans Mono",
                        SKFontStyleWeight.Normal,
                        SKFontStyleWidth.Normal,
                        SKFontStyleSlant.Upright
                    );
                }
            }

            return typeface;
        };
    }
}
