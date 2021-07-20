using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class ImageMoveElementDrawer : ElementDrawerBase
    {
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplImageMove;
        }

        public override void Draw(ZplElementBase element)
        {
            if (element is ZplImageMove imageMove)
            {
                var imageData = this._printerStorage.GetFile(imageMove.StorageDevice, imageMove.ObjectName);

                if (imageData.Length == 0)
                {
                    return;
                }

                var x = imageMove.Origin.PositionX + this._padding;
                var y = imageMove.Origin.PositionY + this._padding;

                this._skCanvas.DrawBitmap(SKBitmap.Decode(imageData), x, y);
            }
        }
    }
}
