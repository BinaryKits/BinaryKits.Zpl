using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Public interface for element drawers
    /// </summary>
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
        /// Check if the drawer can draw this element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool CanDraw(ZplElementBase element);

        /// <summary>
        /// Element requires reverse draw
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool IsReverseDraw(ZplElementBase element);
        
        /// <summary>
        /// Element is white
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool IsWhiteDraw(ZplElementBase element);
        
        /// <summary>
        /// Element needs to be drawn in bitmap mode
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool ForceBitmapDraw(ZplElementBase element);

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
