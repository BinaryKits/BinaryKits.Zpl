using BarcodeLib;
using BinaryKits.Zpl.Label.Elements;
using System.Drawing;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class Barcode39ElementDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcode39;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplBarcode39 barcode)
            {
                float x = barcode.PositionX;
                float y = barcode.PositionY;

                if (barcode.FieldTypeset != null)
                {
                    y -= barcode.Height;
                }

                var barcodeElement = new Barcode
                {
                    BarWidth = barcode.ModuleWidth,
                    BackColor = Color.Transparent,
                    Height = barcode.Height
                };

                using var image = barcodeElement.Encode(TYPE.CODE39Extended, barcode.Content);
                this.DrawBarcode(this.GetImageData(image), barcode.Height, barcodeElement.Width, barcode.FieldOrigin != null, x, y, barcode.FieldOrientation);
            }
        }
    }
}
