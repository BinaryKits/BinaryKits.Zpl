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
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont)
        {
            if (element is ZplImageMove imageMove)
            {
                byte[] imageData = this.printerStorage.GetFile(imageMove.StorageDevice, imageMove.ObjectName);
                SKBitmap image = SKBitmap.Decode(imageData);

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

                bool useFieldTypeset = imageMove.FieldTypeset != null;
                if (useFieldTypeset)
                {
                    y -= image.Height;
                    if (y < 0)
                    {
                        y = 0;
                    }
                }

                this.skCanvas.DrawBitmap(image, x, y);

                return this.CalculateNextDefaultPosition(x, y, image.Width, image.Height, imageMove.FieldOrigin != null, Label.FieldOrientation.Normal, currentPosition);
            }
            
            return currentPosition;
        }
    }
}
