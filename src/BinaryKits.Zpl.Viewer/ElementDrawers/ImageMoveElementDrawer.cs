using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class ImageMoveElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplImageMove;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplImageMove imageMove)
            {
                var imageData = this._printerStorage.GetFile(imageMove.StorageDevice, imageMove.ObjectName);

                if (imageData.Length == 0)
                {
                    return;
                }

                var x = imageMove.PositionX;
                var y = imageMove.PositionY;

                this._skCanvas.DrawBitmap(SKBitmap.Decode(imageData), x, y);
            }
        }
    }
}
