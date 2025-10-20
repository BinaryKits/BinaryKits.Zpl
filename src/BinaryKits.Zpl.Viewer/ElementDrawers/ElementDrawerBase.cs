using BinaryKits.Zpl.Label;
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
        public virtual bool IsWhiteDraw(ZplElementBase element)
        {
            return false;
        }
        
        ///<inheritdoc/>
        public virtual bool ForceBitmapDraw(ZplElementBase element)
        {
            return false;
        }

        ///<inheritdoc/>
        public virtual void Draw(ZplElementBase element)
        {
            Draw(element, new DrawerOptions());
        }

        ///<inheritdoc/>
        public virtual void Draw(ZplElementBase element, DrawerOptions options = null)
        {
            Draw(element);  // Most element just ignore the context
        }

        ///<inheritdoc/>
        public virtual void Draw(ZplElementBase element, DrawerOptions options, InternationalFont internationalFont = InternationalFont.ZCP850)
        {
            Draw(element, options);
        }

        protected virtual void UpdateNextDefaultPosition(float x, float y, float elementWidth, float elementHeight, bool useFieldOrigin, Label.FieldOrientation fieldOrientation, DrawerOptions options)
        {
            if (useFieldOrigin)
            {
                switch (fieldOrientation)
                {
                    case Label.FieldOrientation.Normal:
                        options.NextDefaultFieldPosition = new SKPoint(x + elementWidth, y + elementHeight);
                        break;
                    case Label.FieldOrientation.Rotated90:
                        options.NextDefaultFieldPosition = new SKPoint(x, y + elementHeight);
                        break;
                    case Label.FieldOrientation.Rotated180:
                        options.NextDefaultFieldPosition = new SKPoint(x - elementWidth, y);
                        break;
                    case Label.FieldOrientation.Rotated270:
                        options.NextDefaultFieldPosition = new SKPoint(x, y - elementHeight);
                        break;
                }
            }
            else
            {
                switch (fieldOrientation)
                {
                    case Label.FieldOrientation.Normal:
                        options.NextDefaultFieldPosition = new SKPoint(x + elementWidth, y);
                        break;
                    case Label.FieldOrientation.Rotated90:
                        options.NextDefaultFieldPosition = new SKPoint(x, y + elementWidth);
                        break;
                    case Label.FieldOrientation.Rotated180:
                        options.NextDefaultFieldPosition = new SKPoint(x - elementWidth, y);
                        break;
                    case Label.FieldOrientation.Rotated270:
                        options.NextDefaultFieldPosition = new SKPoint(x, y - elementWidth);
                        break;
                }
            }
        }
    }
}
