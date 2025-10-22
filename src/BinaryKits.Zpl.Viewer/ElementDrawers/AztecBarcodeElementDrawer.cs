using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;

using SkiaSharp;

using ZXing.Aztec;
using ZXing.Common;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class AztecBarcodeElementDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplAztecBarcode;
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont)
        {
            if (element is ZplAztecBarcode aztecBarcode)
            {
                float x = aztecBarcode.PositionX;
                float y = aztecBarcode.PositionY;

                if (aztecBarcode.UseDefaultPosition)
                {
                    x = currentPosition.X;
                    y = currentPosition.Y;
                }

                string content = aztecBarcode.Content;

                if (aztecBarcode.HexadecimalIndicator is char hexIndicator)
                {
                    content = content.ReplaceHexEscapes(hexIndicator, internationalFont);
                }

                AztecWriter writer = new();
                AztecEncodingOptions encodingOptions = new();
                if (aztecBarcode.ErrorControl >= 1 && aztecBarcode.ErrorControl <= 99)
                {
                    encodingOptions.ErrorCorrection = aztecBarcode.ErrorControl;
                }
                else if (aztecBarcode.ErrorControl >= 101 && aztecBarcode.ErrorControl <= 104)
                {
                    encodingOptions.Layers = 100 - aztecBarcode.ErrorControl;
                }
                else if (aztecBarcode.ErrorControl >= 201 && aztecBarcode.ErrorControl <= 232)
                {
                    encodingOptions.Layers = aztecBarcode.ErrorControl - 200;
                }
                else if (aztecBarcode.ErrorControl == 300)
                {
                    encodingOptions.PureBarcode = true;
                }
                else
                {
                    // default options
                }

                BitMatrix result = writer.encode(content, ZXing.BarcodeFormat.AZTEC, 0, 0, encodingOptions.Hints);

                using SKBitmap resizedImage = BitMatrixToSKBitmap(result, aztecBarcode.MagnificationFactor);
                byte[] png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, x, y, resizedImage.Width, resizedImage.Height, aztecBarcode.FieldOrigin != null, aztecBarcode.FieldOrientation);
                return this.CalculateNextDefaultPosition(x, y, resizedImage.Width, resizedImage.Height, aztecBarcode.FieldOrigin != null, aztecBarcode.FieldOrientation, currentPosition);
            }
            
            return currentPosition;
        }
    }
}
