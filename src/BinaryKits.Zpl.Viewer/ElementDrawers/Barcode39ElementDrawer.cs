using BinaryKits.Zpl.Label.Elements;
using NetBarcode;
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
                float x = barcode.PositionX + this._padding;
                float y = barcode.PositionY + this._padding;

                if (barcode.FieldTypeset != null)
                {
                    y -= barcode.Height;
                }

                var barcodeElement = new Barcode();
                barcodeElement.Configure(new BarcodeSettings
                {
                    BarcodeHeight = barcode.Height,
                    BarcodeType = BarcodeType.Code39E,
                    BarWidth = barcode.ModuleWidth,
                    BackgroundColor = Color.Transparent
                });

                var barcodeWidth = barcodeElement.GetImage(barcode.Content).Width;
                var barcodeImageData = barcodeElement.GetByteArray(barcode.Content);

                this.DrawBarcode(barcodeImageData, barcode.Height, barcodeWidth, barcode.FieldOrigin != null, x, y, barcode.FieldOrientation);
            }
        }
    }
}
