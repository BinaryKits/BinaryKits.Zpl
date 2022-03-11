using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public abstract class ElementDrawerBase : IElementDrawer
    {
        internal IPrinterStorage _printerStorage;
        internal SKCanvas _skCanvas;

        ///<inheritdoc/>
        public void Prepare(
            IPrinterStorage printerStorage,
            SKCanvas skCanvas)
        {
            this._printerStorage = printerStorage;
            this._skCanvas = skCanvas;
        }

        ///<inheritdoc/>
        public abstract bool CanDraw(ZplElementBase element);

        ///<inheritdoc/>
        public virtual bool IsReverseDraw(ZplElementBase element)
        {
            return false;
        }

        ///<inheritdoc/>
        public abstract void Draw(ZplElementBase element);

        ///<inheritdoc/>
        public virtual void Draw(ZplElementBase element, DrawerOptions options = null)
        {
            Draw(element);  // Most element just ignore the context
        }
    }
}
