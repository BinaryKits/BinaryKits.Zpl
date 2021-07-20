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

        bool CanDraw(ZplElementBase element);

        void Draw(ZplElementBase element);
    }
}
