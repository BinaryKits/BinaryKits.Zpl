using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public interface IElementDrawer
    {
        /// <summary>
        /// Prepare the drawer
        /// </summary>
        /// <param name="printerStorage"></param>
        /// <param name="skCanvas"></param>
        void Prepare(
            IPrinterStorage printerStorage,
            SKCanvas skCanvas);

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

        /// <summary>
        /// Draw the element with extra context information
        /// </summary>
        /// <param name="element"></param>
        /// <param name="options"></param>
        void Draw(ZplElementBase element, DrawerOptions options);
    }
}
