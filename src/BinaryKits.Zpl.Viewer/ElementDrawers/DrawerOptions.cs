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
        /// Applies the label over a white background after rendering all elements.
        /// When <c>true</c> this also makes the <b>PDF</b> output opaque (white background) and
        /// enables fully-vector reverse-print (^FR / ^LR) rendering in the PDF. When <c>false</c>
        /// the PDF keeps a transparent background and uses the legacy raster compositing for
        /// reverse-print, which embeds an image instead of vector graphics.
        /// </summary>
        public bool OpaqueBackground { get; set; } = false;

        /// <summary>
        /// Transient flag, toggled per draw call by <see cref="ZplElementDrawer"/>, that tells a
        /// reverse-capable drawer to render for the vector PDF canvas (white + Difference) instead
        /// of the bitmap canvas (black + Xor). Not part of the public configuration surface.
        /// </summary>
        internal bool PdfReverseDraw { get; set; } = false;

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
