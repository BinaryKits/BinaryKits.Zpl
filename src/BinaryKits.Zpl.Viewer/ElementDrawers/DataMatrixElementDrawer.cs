using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ZXing;
using ZXing.Datamatrix;
using ZXing.Datamatrix.Encoder;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class DataMatrixElementDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplDataMatrix;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplDataMatrix dataMatrix)
            {
                if (dataMatrix.Height == 0)
                    throw new System.Exception("Matrix Height is set to zero.");

                if (string.IsNullOrWhiteSpace(dataMatrix.Content))
                    throw new System.Exception("Matrix Content is empty.");

                float x = dataMatrix.PositionX;
                float y = dataMatrix.PositionY;

                // support hand-rolled GS1
                bool gs1Mode = false;
                var content = dataMatrix.Content;
                if (Regex.Match(content, @"(^_1)", RegexOptions.None).Success)
                {
                    content = Regex.Replace(content, @"(^_1)", "");
                    gs1Mode = true;
                }

                var writer = new DataMatrixWriter();
                var hints = new Dictionary<EncodeHintType, object> {
                    { EncodeHintType.DATA_MATRIX_SHAPE, SymbolShapeHint.FORCE_SQUARE },
                    { EncodeHintType.DATA_MATRIX_COMPACT, gs1Mode },
                    { EncodeHintType.GS1_FORMAT, gs1Mode }
                };
                var result = writer.encode(content, BarcodeFormat.DATA_MATRIX, 0, 0, hints);

                using var resizedImage = this.BitMatrixToSKBitmap(result, dataMatrix.Height);
                {
                    var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                    this.DrawBarcode(png, resizedImage.Height, resizedImage.Width, dataMatrix.FieldOrigin != null, x, y, 0, dataMatrix.FieldOrientation);
                }
            }
        }
    }
}
