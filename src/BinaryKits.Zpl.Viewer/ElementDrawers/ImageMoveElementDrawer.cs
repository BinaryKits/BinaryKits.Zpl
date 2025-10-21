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
                var image = SKBitmap.Decode(imageData);

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

                var useFieldTypeset = imageMove.FieldTypeset != null;
                if (useFieldTypeset)
                {
                    y -= image.Height;
                    if (y < 0)
                    {
                        y = 0;
                    }
                }

                this._skCanvas.DrawBitmap(image, x, y);

                return this.CalculateNextDefaultPosition(x, y, image.Width, image.Height, imageMove.FieldOrigin != null, Label.FieldOrientation.Normal, currentPosition);
            }
            
            return currentPosition;
        }
    }
}
