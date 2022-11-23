using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class QrCodeElementDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplQrCode;
        }

        ///<inheritdoc/>
        ///<todo
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplQrCode qrcode)
            {
                float x = qrcode.PositionX;
                float y = qrcode.PositionY;

                // Enulate ZPL printer: quiet zone is rendered vertically, is not rendered horizontally
                int xOffset = 4 * qrcode.MagnificationFactor;

                // TODO: qrcode.MaskValue is unused. The QRCoder library chooses the "best" mask by heuristic. see: QRCoder.QRCodeGenerator.ModulePlacer.Score
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(qrcode.Content, this.CovertErrorCorrection(qrcode.ErrorCorrectionLevel));
                using var qrCode = new QRCode(qrCodeData);

                using Bitmap qrCodeImage = qrCode.GetGraphic(qrcode.MagnificationFactor, Color.Black, Color.Transparent, drawQuietZones: true);
                using var ms = new MemoryStream();
                qrCodeImage.Save(ms, ImageFormat.Png);

                this.DrawBarcode(ms.ToArray(), qrCodeImage.Height, qrCodeImage.Width - 2 * xOffset, qrcode.FieldOrigin != null, x - xOffset, y, 0, qrcode.FieldOrientation);
            }
        }

        private QRCodeGenerator.ECCLevel CovertErrorCorrection(ErrorCorrectionLevel errorCorrectionLevel)
        {
            return errorCorrectionLevel switch
            {
                ErrorCorrectionLevel.UltraHighReliability => QRCodeGenerator.ECCLevel.H,
                ErrorCorrectionLevel.HighReliability => QRCodeGenerator.ECCLevel.Q,
                ErrorCorrectionLevel.Standard => QRCodeGenerator.ECCLevel.M,
                ErrorCorrectionLevel.HighDensity => QRCodeGenerator.ECCLevel.L,
                _ => QRCodeGenerator.ECCLevel.M,
            };
        }
    }
}
