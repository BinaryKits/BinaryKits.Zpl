using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;
using System.Collections.Generic;
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
                float x = dataMatrix.PositionX;
                float y = dataMatrix.PositionY;

                var writer = new DataMatrixWriter();
                var hints = new Dictionary<EncodeHintType, object> {
                    { EncodeHintType.DATA_MATRIX_SHAPE, SymbolShapeHint.FORCE_SQUARE }
                };
                var result = writer.encode(dataMatrix.Content, BarcodeFormat.DATA_MATRIX, 0, 0, hints);

                int size = dataMatrix.Height;
                using var image = new SKBitmap(result.Width, result.Height);

                for (int row = 0; row < result.Height; row++)
                {
                    for (int col = 0; col < result.Width; col++)
                    {
                        var color = result[col, row] ? SKColors.Black : SKColors.White;
                        image.SetPixel(col, row, color);
                    }
                }

                using var resizedImage = image.Resize(new SKSizeI(image.Width * size, image.Height * size), SKFilterQuality.None);

                var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, resizedImage.Height, resizedImage.Width, dataMatrix.FieldOrigin != null, x, y, 0, dataMatrix.FieldOrientation);
            }
        }
    }
}
