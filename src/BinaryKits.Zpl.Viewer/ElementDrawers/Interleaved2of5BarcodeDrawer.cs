using BinaryKits.Zpl.Label.Elements;
using NetBarcode;
using System.Drawing;
using System.IO;

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

                var barcodeElement = new BarcodeLib.Barcode();
                barcodeElement.BarWidth = barcode.ModuleWidth;
                barcodeElement.BackColor = Color.Transparent;
                barcodeElement.Height = barcode.Height;
                //barcodeElement.

                byte[] barcodeImageData;
                using var image = barcodeElement.Encode(BarcodeLib.TYPE.Interleaved2of5, barcode.Content);
                using (var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    barcodeImageData = memoryStream.ToArray();
                }

                //TODO: Change to an other project that supports Interleaved2of5 Barcode
                //var barcodeElement = new Barcode();
                //barcodeElement.Configure(new BarcodeSettings
                //{
                //    BarcodeHeight = barcode.Height,
                //    BarcodeType = BarcodeType.Code128,
                //    BarWidth = barcode.ModuleWidth,
                //    BackgroundColor = Color.Transparent,
                //    //ShowLabel = false
                //});

                //var barcodeWidth = barcodeElement.GetImage(barcode.Content).Width;
                //var barcodeImageData = barcodeElement.GetByteArray(barcode.Content);

                this.DrawBarcode(barcodeImageData, barcode.Height, image.Width, barcode.FieldOrigin != null, x, y, barcode.FieldOrientation);
            }
        }
    }
}
