using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;
using System.Collections.Generic;
using ZXing;
using ZXing.QrCode;

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
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplQrCode qrcode)
            {
                float x = qrcode.PositionX;
                float y = qrcode.PositionY;

                int verticalQuietZone = 10;

                var writer = new QRCodeWriter();
                var hints = new Dictionary<EncodeHintType, object> {
                    { EncodeHintType.ERROR_CORRECTION, CovertErrorCorrection(qrcode.ErrorCorrectionLevel) },
                    { EncodeHintType.QR_MASK_PATTERN, qrcode.MaskValue },
                    { EncodeHintType.CHARACTER_SET, "UTF-8" },
                    { EncodeHintType.MARGIN, 0 }
                };
                var result = writer.encode(qrcode.Content, BarcodeFormat.QR_CODE, 0, 0, hints);

                using var resizedImage = this.BitMatrixToSKBitmap(result, qrcode.MagnificationFactor);

                var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, resizedImage.Height + 2 * verticalQuietZone, resizedImage.Width, qrcode.FieldOrigin != null, x, y + verticalQuietZone, 0, qrcode.FieldOrientation);
            }
        }

        private ZXing.QrCode.Internal.ErrorCorrectionLevel CovertErrorCorrection(ErrorCorrectionLevel errorCorrectionLevel)
        {
            return errorCorrectionLevel switch
            {
                ErrorCorrectionLevel.UltraHighReliability => ZXing.QrCode.Internal.ErrorCorrectionLevel.H,
                ErrorCorrectionLevel.HighReliability => ZXing.QrCode.Internal.ErrorCorrectionLevel.Q,
                ErrorCorrectionLevel.Standard => ZXing.QrCode.Internal.ErrorCorrectionLevel.M,
                ErrorCorrectionLevel.HighDensity => ZXing.QrCode.Internal.ErrorCorrectionLevel.L,
                _ => ZXing.QrCode.Internal.ErrorCorrectionLevel.M
            };
        }
    }
}
