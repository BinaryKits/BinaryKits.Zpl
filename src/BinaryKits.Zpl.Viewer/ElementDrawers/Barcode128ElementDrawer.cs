using BinaryKits.Zpl.Label.Elements;
using NetBarcode;
using System.Drawing;
using System.IO;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class Barcode128ElementDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcode128;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplBarcode128 barcode)
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

                byte[] barcodeImageData;
                using var image = barcodeElement.Encode(BarcodeLib.TYPE.CODE128B, barcode.Content);
                using (var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    barcodeImageData = memoryStream.ToArray();
                }

                //var barcodeElement = new Barcode();
                //barcodeElement.Configure(new BarcodeSettings
                //{
                //    BarcodeHeight = barcode.Height,
                //    //TODO: Choose barcodeType over the mode config of zpl barcode config
                //    BarcodeType = BarcodeType.Code128B,
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
