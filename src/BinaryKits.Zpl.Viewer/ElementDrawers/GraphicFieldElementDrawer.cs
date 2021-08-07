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
                var imageData = ByteHelper.StringToByteArray(graphicField.Data);

                var x = graphicField.PositionX + this._padding;
                var y = graphicField.PositionY + this._padding;

                this._skCanvas.DrawBitmap(SKBitmap.Decode(imageData), x, y);
            }
        }
    }
}
