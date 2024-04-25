using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ZXing;
using ZXing.QrCode;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for QR Code Barcode elements
    /// </summary>
    public class QrCodeElementDrawer : BarcodeDrawerBase
    {
        private static readonly Regex gs1Regex = new Regex(@"^>;>8(.+)$", RegexOptions.Compiled);

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
                
                // support hand-rolled GS1
                bool gs1Mode = false;
                var content = qrcode.Content;

                Match gs1Match = gs1Regex.Match(content);
                if (gs1Match.Success)
                {
                    content = gs1Match.Groups[1].Value;
                    gs1Mode = true;
                }

                int verticalQuietZone = 10;

                var writer = new QRCodeWriter();
                // TODO: use QrCodeEncodingOptions in next version of ZXing.NET
                var hints = new Dictionary<EncodeHintType, object> {
                    { EncodeHintType.ERROR_CORRECTION, CovertErrorCorrection(qrcode.ErrorCorrectionLevel) },
                    { EncodeHintType.QR_MASK_PATTERN, qrcode.MaskValue },
                    { EncodeHintType.CHARACTER_SET, "UTF-8" },
                    { EncodeHintType.MARGIN, 0 },
                    { EncodeHintType.GS1_FORMAT, gs1Mode }
                };
                var result = writer.encode(content, BarcodeFormat.QR_CODE, 0, 0, hints);

                using var resizedImage = this.BitMatrixToSKBitmap(result, qrcode.MagnificationFactor);

                var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, x, y + verticalQuietZone, resizedImage.Width, resizedImage.Height + 2 * verticalQuietZone, qrcode.FieldOrigin != null, qrcode.FieldOrientation);
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
