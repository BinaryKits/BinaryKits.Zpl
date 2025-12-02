using SkiaSharp;

using System;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class DrawerOptions
    {
        [Obsolete("Use FontManager.FontLoader instead.")]
        public Func<string, SKTypeface> FontLoader { get; set; }

        /// <summary>
        /// Gets or sets the image format used when rendering output.
        /// </summary>
        public SKEncodedImageFormat RenderFormat { get; set; } = SKEncodedImageFormat.Png;

        /// <summary>
        /// Gets or sets the quality level used when rendering images in formats that support lossy compression.
        /// </summary>
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

        public FontManager FontManager { get; private set; }

        public DrawerOptions() : this(new FontManager()) { }

        public DrawerOptions(FontManager fontManager)
        {
            this.FontManager = fontManager;
            this.FontLoader = fontManager.FontLoader;
        }
    }
}
