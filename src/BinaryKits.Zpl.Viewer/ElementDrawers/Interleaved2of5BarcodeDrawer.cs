using BinaryKits.Zpl.Label.Elements;
using NetBarcode;
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
                float x = barcode.PositionX + this._padding;
                float y = barcode.PositionY + this._padding;

                if (barcode.FieldTypeset != null)
                {
                    y -= barcode.Height;
                }

                //TODO: Change to an other project that supports Interleaved2of5 Barcode
                var barcodeElement = new Barcode();
                barcodeElement.Configure(new BarcodeSettings
                {
                    BarcodeHeight = barcode.Height,
                    BarcodeType = BarcodeType.Code128,
                    BarWidth = barcode.ModuleWidth,
                    BackgroundColor = Color.Transparent,
                    //ShowLabel = false
                });

                var barcodeWidth = barcodeElement.GetImage(barcode.Content).Width;
                var barcodeImageData = barcodeElement.GetByteArray(barcode.Content);

                this.DrawBarcode(barcodeImageData, barcode.Height, barcodeWidth, barcode.FieldOrigin != null, x, y, barcode.FieldOrientation);
            }
        }
    }
}
