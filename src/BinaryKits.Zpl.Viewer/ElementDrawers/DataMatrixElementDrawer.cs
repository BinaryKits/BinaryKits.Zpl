using BarcodeLib;
using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;
using System.Drawing;
using ZXing;
using ZXing.Datamatrix;

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

                if (dataMatrix.FieldTypeset != null)
                {
                    y -= dataMatrix.Height;
                }

                var writer = new DataMatrixWriter();
                var result = writer.encode(dataMatrix.Content, BarcodeFormat.DATA_MATRIX, 0, 0);

                int size = dataMatrix.Height;
                using var image = new SKBitmap(result.Width + size - 1, result.Height + size - 1);

                for (int row = 0; row < result.Height; row++)
                {
                    for (int col = 0; col < result.Width; col++)
                    {
                        var color = result[row, col] ? SKColors.Black : SKColors.White;
                        image.SetPixel(row, col, color);
                    }
                }

                using var resizedImage = image.Resize(new SKSizeI(image.Width * size, image.Height * size), SKFilterQuality.None);

                var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, dataMatrix.Height, dataMatrix.Height, dataMatrix.FieldOrigin != null, x, y, dataMatrix.FieldOrientation);
            }
        }
    }
}
