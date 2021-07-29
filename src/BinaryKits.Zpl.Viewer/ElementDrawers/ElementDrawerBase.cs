using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public abstract class ElementDrawerBase : IElementDrawer
    {
        internal IPrinterStorage _printerStorage;
        internal SKPaint _skPaint;
        internal SKCanvas _skCanvas;
        internal int _padding;

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

        ///<inheritdoc/>
        public abstract bool CanDraw(ZplElementBase element);

        ///<inheritdoc/>
        public abstract void Draw(ZplElementBase element);
    }
}
