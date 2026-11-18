using System;

namespace BinaryKits.Zpl.Protocol.ImageConverters
{
    /// <summary>
    /// Compatibility wrapper for the former ImageSharp-based converter.
    /// </summary>
    [Obsolete("Use SkiaSharpImageConverter instead.")]
    public class ImageSharpImageConverter : SkiaSharpImageConverter
    {
    }
}
