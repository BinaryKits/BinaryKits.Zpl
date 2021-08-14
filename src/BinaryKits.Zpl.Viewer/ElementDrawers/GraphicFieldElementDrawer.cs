using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class GraphicFieldElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplGraphicField;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplGraphicField graphicField)
            {
                var imageData = ByteHelper.HexToBytes(graphicField.Data);
                var image = SKBitmap.Decode(imageData);

                var x = graphicField.PositionX;
                var y = graphicField.PositionY;

                var useFieldTypeset = graphicField.FieldTypeset != null;
                if (useFieldTypeset)
                {
                    y -= image.Height;
                }

                this._skCanvas.DrawBitmap(image, x, y);
            }
        }
    }
}
