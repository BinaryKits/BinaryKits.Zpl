using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public interface IElementDrawer
    {
        void Prepare(
            IPrinterStorage printerStorage,
            SKPaint skPaint,
            SKCanvas skCanvas,
            int padding);

        /// <summary>
        /// Check the drawer can draw this element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool CanDraw(ZplElementBase element);

        /// <summary>
        /// Draw the element
        /// </summary>
        /// <param name="element"></param>
        void Draw(ZplElementBase element);
    }
}
