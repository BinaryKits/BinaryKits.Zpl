using BarcodeLib;
using BinaryKits.Zpl.Label.Elements;
using System.Drawing;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class Interleaved2of5BarcodeDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcodeInterleaved2of5;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplBarcodeInterleaved2of5 barcode)
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

                using var image = barcodeElement.Encode(TYPE.Interleaved2of5, barcode.Content);
                this.DrawBarcode(this.GetImageData(image), barcode.Height, image.Width, barcode.FieldOrigin != null, x, y, barcode.FieldOrientation);
            }
        }
    }
}
