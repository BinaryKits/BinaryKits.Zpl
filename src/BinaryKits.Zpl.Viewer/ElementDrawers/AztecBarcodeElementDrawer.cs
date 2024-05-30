using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;

using SkiaSharp;

using ZXing.Aztec;

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
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplAztecBarcode aztecBarcode)
            {
                float x = aztecBarcode.PositionX;
                float y = aztecBarcode.PositionY;

                var content = aztecBarcode.Content;

                if (aztecBarcode.UseHexadecimalIndicator)
                {
                    content = content.ReplaceHexEscapes();
                }

                var writer = new AztecWriter();
                var options = new AztecEncodingOptions();
                if (aztecBarcode.ErrorControl >= 1 && aztecBarcode.ErrorControl <= 99)
                {
                    options.ErrorCorrection = aztecBarcode.ErrorControl;
                }
                else if (aztecBarcode.ErrorControl >= 101 && aztecBarcode.ErrorControl <= 104)
                {
                    options.Layers = 100 - aztecBarcode.ErrorControl;
                }
                else if (aztecBarcode.ErrorControl >= 201 && aztecBarcode.ErrorControl <= 232)
                {
                    options.Layers = aztecBarcode.ErrorControl - 200;
                }
                else if (aztecBarcode.ErrorControl == 300)
                {
                    options.PureBarcode = true;
                }
                else
                {
                    // default options
                }

                var result = writer.encode(content, ZXing.BarcodeFormat.AZTEC, 0, 0, options.Hints);

                using var resizedImage = this.BitMatrixToSKBitmap(result, aztecBarcode.MagnificationFactor);
                var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, x, y, resizedImage.Width, resizedImage.Height, aztecBarcode.FieldOrigin != null, aztecBarcode.FieldOrientation);
            }
        }
    }
}
