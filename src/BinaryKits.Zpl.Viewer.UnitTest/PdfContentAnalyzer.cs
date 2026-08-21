using System.Linq;
using System.Text;

using UglyToad.PdfPig;

namespace BinaryKits.Zpl.Viewer.UnitTest
{
    /// <summary>
    /// Lightweight helpers to inspect generated PDF bytes for the reverse-print tests.
    /// </summary>
    internal static class PdfContentAnalyzer
    {
        /// <summary>
        /// Number of raster image XObjects embedded across all pages. Zero means the content is
        /// fully vector (text/shape drawing operations only).
        /// </summary>
        public static int CountImageXObjects(byte[] pdf)
        {
            using var document = PdfDocument.Open(pdf);
            return document.GetPages().Sum(page => page.GetImages().Count());
        }

        /// <summary>
        /// True if the PDF references the given blend mode (e.g. "Difference"). SkiaSharp emits the
        /// ExtGState dictionary as an uncompressed direct object, so a raw token scan is reliable.
        /// </summary>
        public static bool ContainsBlendMode(byte[] pdf, string blendMode)
        {
            string raw = Encoding.Latin1.GetString(pdf);
            return raw.Contains("/" + blendMode);
        }
    }
}
