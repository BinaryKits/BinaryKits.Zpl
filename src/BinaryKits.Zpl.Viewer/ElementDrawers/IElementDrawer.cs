using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public interface IElementDrawer
    {
        void Prepare(
            IPrinterStorage printerStorage,
            SKCanvas skCanvas,
            int padding);

        /// <summary>
        /// Check the drawer can draw this element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool CanDraw(ZplElementBase element);

        /// <summary>
        /// Element require reverse draw
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool IsReverseDraw(ZplElementBase element);

        /// <summary>
        /// Draw the element
        /// </summary>
        /// <param name="element"></param>
        void Draw(ZplElementBase element);
    }
}
