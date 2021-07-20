using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public abstract class ElementDrawerBase : IElementDrawer
    {
        protected IPrinterStorage _printerStorage;
        protected SKPaint _skPaint;
        protected SKCanvas _skCanvas;
        protected int _padding;

        public void Prepare(
            IPrinterStorage printerStorage,
            SKPaint skPaint,
            SKCanvas skCanvas,
            int padding)
        {
            this._printerStorage = printerStorage;
            this._skPaint = skPaint;
            this._skCanvas = skCanvas;
            this._padding = padding;
        }

        public abstract bool CanDraw(ZplElementBase element);

        public abstract void Draw(ZplElementBase element);
    }
}
