using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Label.Helpers;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for Graphic Field elements
    /// </summary>
    public class GraphicFieldElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplGraphicField;
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont)
        {
            if (element is ZplGraphicField graphicField)
            {
                var imageData = ByteHelper.HexToBytes(graphicField.Data);
                var image = SKBitmap.Decode(imageData);

                float x = graphicField.PositionX;
                float y = graphicField.PositionY;

                if (graphicField.UseDefaultPosition)
                {
                    x = currentPosition.X;
                    y = currentPosition.Y;
                }

                var useFieldTypeset = graphicField.FieldTypeset != null;
                if (useFieldTypeset)
                {
                    y -= image.Height;
                    if (y < 0)
                    {
                        y = 0;
                    }
                }

                this._skCanvas.DrawBitmap(image, x, y);
                return this.CalculateNextDefaultPosition(x, y, image.Width, image.Height, graphicField.FieldOrigin != null, Label.FieldOrientation.Normal, currentPosition);
            }
            
            return currentPosition;
        }
    }
}
