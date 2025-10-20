using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;

using SkiaSharp;
using System.Text.RegularExpressions;
using ZXing;
using ZXing.Datamatrix;
using ZXing.Datamatrix.Encoder;
using ZXing.QrCode.Internal;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for Data Matrix Barcode elements
    /// </summary>
    public class DataMatrixElementDrawer : BarcodeDrawerBase
    {
        private static readonly Regex gs1Regex = new Regex(@"^_1(.+)$", RegexOptions.Compiled);

        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplDataMatrix;
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, InternationalFont internationalFont, SKPoint currentPosition)
        {
            if (element is ZplDataMatrix dataMatrix)
            {
                if (dataMatrix.Height == 0)
                    throw new System.Exception("Matrix Height is set to zero.");

                if (string.IsNullOrWhiteSpace(dataMatrix.Content))
                    throw new System.Exception("Matrix Content is empty.");

                float x = dataMatrix.PositionX;
                float y = dataMatrix.PositionY;

                if (dataMatrix.UseDefaultPosition)
                {
                    x = currentPosition.X;
                    y = currentPosition.Y;
                }

                var content = dataMatrix.Content;
                if(dataMatrix.HexadecimalIndicator is char hexIndicator)
                {
                    content = content.ReplaceHexEscapes(hexIndicator, internationalFont);
                }

                // support hand-rolled GS1
                bool gs1Mode = false;
                Match gs1Match = gs1Regex.Match(content);
                if (gs1Match.Success)
                {
                    content = gs1Match.Groups[1].Value;
                    gs1Mode = true;
                }

                var writer = new DataMatrixWriter();
                var encodingOptions = new DatamatrixEncodingOptions()
                {
                    SymbolShape = SymbolShapeHint.FORCE_SQUARE,
                    CompactEncoding = gs1Mode,
                    GS1Format = gs1Mode
                };
                var result = writer.encode(content, BarcodeFormat.DATA_MATRIX, 0, 0, encodingOptions.Hints);

                using var resizedImage = this.BitMatrixToSKBitmap(result, dataMatrix.Height);
                {
                    var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                    this.DrawBarcode(png, x, y, resizedImage.Width, resizedImage.Height, dataMatrix.FieldOrigin != null, dataMatrix.FieldOrientation);
                }

                return this.CalculateNextDefaultPosition(x, y, resizedImage.Width, resizedImage.Height, dataMatrix.FieldOrigin != null, dataMatrix.FieldOrientation, currentPosition);
            }
            
            return currentPosition;
        }
    }
}
