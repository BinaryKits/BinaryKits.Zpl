using BinaryKits.Zpl.Label;
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
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, InternationalFont internationalFont, SKPoint currentPosition)
        {
            if (element is ZplImageMove imageMove)
            {
                var imageData = this._printerStorage.GetFile(imageMove.StorageDevice, imageMove.ObjectName);

                if (imageData.Length == 0)
                {
                    return currentPosition;
                }

                float x = imageMove.PositionX;
                float y = imageMove.PositionY;

                if (imageMove.UseDefaultPosition)
                {
                    x = currentPosition.X;
                    y = currentPosition.Y;
                }

                var bitmap = SKBitmap.Decode(imageData);
                this._skCanvas.DrawBitmap(bitmap, x, y);

                return this.CalculateNextDefaultPosition(x, y, bitmap.Width, bitmap.Height, imageMove.FieldOrigin != null, Label.FieldOrientation.Normal, currentPosition);
            }
            
            return currentPosition;
        }
    }
}
