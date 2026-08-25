using BinaryKits.Zpl.Viewer.Properties;

using SkiaSharp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinaryKits.Zpl.Viewer
{
    public class FontManager
    {
        private readonly Dictionary<string, IList<SKTypeface>> registeredTypefaces = new(StringComparer.OrdinalIgnoreCase);

        private static readonly SKFontManager systemFontManager = SKFontManager.Default;

        /// <summary>
        /// Gets or sets the list of font family names used as the primary font stack for rendering text.
        /// </summary>
        /// <remarks>The font stack is ordered by preference; the first available font in the list will be
        /// used. Some fonts, such as 'Arial' or 'Helvetica', require that a Narrow or Condensed typeface
        /// is installed on the system for proper rendering.</remarks>
        public List<string> FontStack0 { get; set; } = [
            "Swis721 Cn BT",
            "TeX Gyre Heros Cn",
            "Nimbus Sans Narrow",
            "Roboto Condensed",
            "Helvetica",
            "Helvetica Neue",
            "Arial",
        ];

        /// <summary>
        /// Gets or sets the list of font family names used as the primary monospace font stack.
        /// </summary>
        /// <remarks>The order of font names determines the fallback sequence when rendering text. Modify
        /// this list to customize the preferred monospace fonts for your application.</remarks>
        public List<string> FontStackA { get; set; } = [
            "DejaVu Sans Mono",
            "Lucida Console",
            "Andale Mono",
            "Droid Sans Mono",
        ];

        /// <summary>
        /// Gets or sets the delegate used to load a font by name and return an <see cref="SKTypeface"/>.
        /// </summary>
        /// <remarks>
        /// The delegate should accept a font name as a string and return the corresponding
        /// <see cref="SKTypeface"/>, or <see langword="null"/> if the font is not found. This property allows customization
        /// of font loading behavior, such as loading fonts from embedded resources or external files.
        /// </remarks>
        public Func<string, SKTypeface> FontLoader {
            [Obsolete("Use FontManager.GetFont instead")]
            get => this.fontLoader; 
            set => this.fontLoader = value ?? throw new ArgumentNullException(nameof(value)); 
        }

        private Func<string, SKTypeface> fontLoader;

        private static readonly SKFontStyle fontStyle0 = new(
            SKFontStyleWeight.Bold,
            SKFontStyleWidth.SemiCondensed,
            SKFontStyleSlant.Upright
        );

        private static readonly SKFontStyle fontStyleA = new(
            SKFontStyleWeight.Normal,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright
        );

        private SKTypeface typeface0;
        internal SKTypeface Typeface0 {
            get {
                this.typeface0 ??= this.GetDefaultTypeface("0");
                return this.typeface0;
            }
        }

        private SKTypeface typefaceA;
        internal SKTypeface TypefaceA {
            get {
                this.typefaceA ??= this.GetDefaultTypeface("A");
                return this.typefaceA;
            }
        }

        internal SKTypeface TypefaceGS { get; } = SKTypeface.FromStream(new MemoryStream(Resources.ZplGS));

        public FontManager()
        {
            this.fontLoader = (fontName) => {
                if (fontName == "0")
                {
                    return this.Typeface0;
                }

                return this.TypefaceA;
            };
        }

        /// <summary>
        /// Registers the specified typeface for use within the font manager, making it available for font selection by
        /// family name.
        /// </summary>
        /// <param name="typeface">The typeface to register. Cannot be null. The typeface's family name is used as the
        /// key for registration.</param>
        public void RegisterTypeface(SKTypeface typeface)
        {
            if (this.registeredTypefaces.TryGetValue(typeface.FamilyName, out IList<SKTypeface> typefaces))
            {
                typefaces.Add(typeface);
            }
            else
            {
                this.registeredTypefaces[typeface.FamilyName] = [typeface];
            }
        }

        private SKTypeface GetDefaultTypeface(string fontType)
        {
            List<string> fontStack = fontType switch
            {
                "0" => this.FontStack0,
                "A" => this.FontStackA,
                _ => this.FontStackA,
            };

            SKFontStyle fontStyle = fontType switch
            {
                "0" => fontStyle0,
                "A" => fontStyleA,
                _ => fontStyleA,
            };

            foreach (string fontFamily in fontStack)
            {
                SKTypeface typeface = this.GetTypeFaceByFamily(fontFamily, fontStyle);
                if (typeface != null)
                {
                    return typeface;
                }
            }

            return systemFontManager.MatchFamily(SKTypeface.Default.FamilyName, fontStyle) ?? SKTypeface.Default;
        }

        private SKTypeface GetTypeFaceByFamily(string familyName, SKFontStyle fontStyle)
        {
            if (this.registeredTypefaces.TryGetValue(familyName, out IList<SKTypeface> typefaces))
            {
                return typefaces
                    // group italic and oblique together
                    .OrderBy(t => Math.Abs((t.FontStyle.Slant == SKFontStyleSlant.Upright ? 0 : 1) - (fontStyle.Slant == SKFontStyleSlant.Upright ? 0 : 1)))
                    .ThenBy(t => Math.Abs(t.FontStyle.Width - fontStyle.Width))
                    .ThenBy(t => Math.Abs(t.FontStyle.Weight - fontStyle.Weight))
                    .First();
            }

            SKTypeface typefaceFromSystem = systemFontManager.MatchFamily(familyName, fontStyle);
            if (typefaceFromSystem != null)
            {
                return typefaceFromSystem;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the typeface for <paramref name="fontName"/>.
        /// </summary>
        /// <returns>
        /// The requested <see cref="SKTypeface"/>.
        /// </returns>
        /// <exception cref="FontNotFoundException">
        /// If <paramref name="fontName"/> is not found.
        /// </exception>
        /// <inheritdoc cref="GetFont(string, string, out bool)"/>
        public SKTypeface GetFont(string fontName)
        {
            return this.GetFont(fontName, null, out bool _);
        }

        /// <inheritdoc cref="GetFont(string, string, out bool)"/>
        public SKTypeface GetFont(string fontName, string fallbackFont)
        {
            return this.GetFont(fontName, fallbackFont, out bool _);
        }

        /// <summary>
        /// Tries to retrieve the typeface for <paramref name="fontName"/>, falling back to <paramref name="fallbackFont"/>
        /// if not <see langword="null"/>.
        /// </summary>
        /// <param name="fontName">The name of the font to find</param>
        /// <param name="fallbackFont">The fallback font to use if <paramref name="fontName"/> is not found</param>
        /// <param name="found">
        /// <see langword="true"/> if <paramref name="fontName"/> is found, <see langword="false"/> otherwise.
        /// </param>
        /// <returns>
        /// The requested <see cref="SKTypeface"/> or the fallback one.
        /// </returns>
        /// <exception cref="FontNotFoundException">
        /// If <paramref name="fontName"/> is not found and <paramref name="fallbackFont"/> is <see langword="null"/>
        /// or if it is not <see langword="null"/> but not found.
        /// </exception>
        public SKTypeface GetFont(string fontName, string fallbackFont, out bool found)
        {
            SKTypeface font = this.fontLoader(fontName);
            if (font == null)
            {
                found = false;
                if (fallbackFont == null)
                {
                    throw new FontNotFoundException(fontName);
                }

                font = this.fontLoader(fallbackFont);
                if (font == null)
                {
                    throw new FontNotFoundException(fontName, fallbackFont);
                }
            }
            else
            {
                found = true;
            }

            return font;
        }
    }

    public class FontNotFoundException : Exception
    {
        internal FontNotFoundException(string fontName) : base($"Font '{fontName}' not found") { }

        internal FontNotFoundException(string fontName, string fallbackFont) : base($"Font '{fontName}' and fallback font '{fallbackFont}' not found") { }
    }
}
