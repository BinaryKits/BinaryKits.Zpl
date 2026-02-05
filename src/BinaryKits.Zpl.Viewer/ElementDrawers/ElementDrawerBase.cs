using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public abstract class ElementDrawerBase : IElementDrawer
    {
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
        public virtual SKPoint Draw(ZplElementBase element, SKCanvas canvas, IPrinterStorage printerStorage, DrawerOptions options, SKPoint currentPosition)
        {
            return currentPosition;
        }

        ///<inheritdoc/>
        public virtual SKPoint Draw(ZplElementBase element, SKCanvas canvas, IPrinterStorage printerStorage, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont)
        {
            return this.Draw(element, canvas, printerStorage, options, currentPosition);
        }

        ///<inheritdoc/>
        public virtual SKPoint Draw(ZplElementBase element, SKCanvas canvas, IPrinterStorage printerStorage, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont, int printDensityDpmm)
        {
            return this.Draw(element, canvas, printerStorage, options, currentPosition, internationalFont);
        }

        protected virtual SKPoint CalculateNextDefaultPosition(float x, float y, float elementWidth, float elementHeight, bool useFieldOrigin, Label.FieldOrientation fieldOrientation, SKPoint currentPosition)
        {
            if (useFieldOrigin)
            {
                switch (fieldOrientation)
                {
                    case Label.FieldOrientation.Normal:
                        return new SKPoint(x + elementWidth, y + elementHeight);
                    case Label.FieldOrientation.Rotated90:
                        return new SKPoint(x, y + elementHeight);
                    case Label.FieldOrientation.Rotated180:
                        return new SKPoint(x - elementWidth, y);
                    case Label.FieldOrientation.Rotated270:
                        return new SKPoint(x, y - elementHeight);
                }
            }
            else
            {
                switch (fieldOrientation)
                {
                    case Label.FieldOrientation.Normal:
                        return new SKPoint(x + elementWidth, y);
                    case Label.FieldOrientation.Rotated90:
                        return new SKPoint(x, y + elementWidth);
                    case Label.FieldOrientation.Rotated180:
                        return new SKPoint(x - elementWidth, y);
                    case Label.FieldOrientation.Rotated270:
                        return new SKPoint(x, y - elementWidth);
                }
            }

            return currentPosition;
        }

    }
}
