using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for Recall Graphic elements
    /// </summary>
    public class RecallGraphicElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplRecallGraphic;
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont)
        {
            if (element is ZplRecallGraphic recallGraphic)
            {
                byte[] imageData = this.printerStorage.GetFile(recallGraphic.StorageDevice, recallGraphic.ImageName);

                if (imageData.Length == 0)
                {
                    return currentPosition;
                }

                float x = recallGraphic.PositionX;
                float y = recallGraphic.PositionY;

                if (recallGraphic.UseDefaultPosition)
                {
                    x = currentPosition.X;
                    y = currentPosition.Y;
                }

                SKBitmap bitmap = SKBitmap.Decode(imageData);
                if (recallGraphic.FieldTypeset != null)
                {
                    y -= bitmap.Height;
                    if (y < 0)
                    {
                        y = 0;
                    }
                }

                this.skCanvas.DrawBitmap(bitmap, x, y);

                float width = bitmap.Width;
                float height = bitmap.Height;
                return this.CalculateNextDefaultPosition(x, y, width, height, recallGraphic.FieldOrigin != null, FieldOrientation.Normal, currentPosition);
            }
            
            return currentPosition;
        }
    }
}
