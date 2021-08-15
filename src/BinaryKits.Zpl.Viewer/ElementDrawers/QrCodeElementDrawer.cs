using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using QRCoder;
using SkiaSharp;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class QrCodeElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplQrCode;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplQrCode qrcode)
            {
                float x = qrcode.PositionX;
                float y = qrcode.PositionY;

                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(qrcode.Content, this.Convert(qrcode.ErrorCorrectionLevel));
                using var qrCode = new QRCode(qrCodeData);

                using Bitmap qrCodeImage = qrCode.GetGraphic(qrcode.MagnificationFactor, Color.Black, Color.Transparent, drawQuietZones: false);
                using (var ms = new MemoryStream())
                {
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    var imageData = ms.ToArray();
                    this._skCanvas.DrawBitmap(SKBitmap.Decode(imageData), x, y);
                }
            }
        }

        private QRCodeGenerator.ECCLevel Convert(ErrorCorrectionLevel errorCorrectionLevel)
        {
            switch (errorCorrectionLevel)
            {
                case ErrorCorrectionLevel.UltraHighReliability:
                    return QRCodeGenerator.ECCLevel.H;
                case ErrorCorrectionLevel.HighReliability:
                    return QRCodeGenerator.ECCLevel.Q;
                case ErrorCorrectionLevel.Standard:
                    return QRCodeGenerator.ECCLevel.M;
                case ErrorCorrectionLevel.HighDensity:
                    return QRCodeGenerator.ECCLevel.Q;
            }

            return QRCodeGenerator.ECCLevel.Q;
        }
    }
}
