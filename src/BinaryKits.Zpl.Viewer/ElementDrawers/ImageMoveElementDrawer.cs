using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for Image Move elements
    /// </summary>
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

                var bitmap = SKBitmap.Decode(imageData);
                this._skCanvas.DrawBitmap(bitmap, x, y);
                this.UpdateNextDefaultPosition(x, y, bitmap.Width, bitmap.Height, imageMove.FieldOrigin != null, Label.FieldOrientation.Normal, new DrawerOptions());
            }
        }
    }
}
