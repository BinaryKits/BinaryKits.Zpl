using SkiaSharp;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing.Common;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public abstract class BarcodeDrawerBase : ElementDrawerBase
    {
        public byte[] GetImageData(Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }

        public void DrawBarcode(
            byte[] barcodeImageData,
            int barcodeHeight,
            int barcodeWidth,
            bool useFieldOrigin,
            float x,
            float y,
            int labelHeightOffset,
            Label.FieldOrientation fieldOrientation)
        {
            using (new SKAutoCanvasRestore(this._skCanvas))
            {
                SKMatrix matrix = SKMatrix.Empty;

                if (useFieldOrigin)
                {
                    switch (fieldOrientation)
                    {
                        case Label.FieldOrientation.Rotated90:
                            matrix = SKMatrix.CreateRotationDegrees(90, x + barcodeHeight / 2, y + barcodeHeight / 2);
                            break;
                        case Label.FieldOrientation.Rotated180:
                            matrix = SKMatrix.CreateRotationDegrees(180, x + barcodeWidth / 2, y + barcodeHeight / 2);
                            break;
                        case Label.FieldOrientation.Rotated270:
                            matrix = SKMatrix.CreateRotationDegrees(270, x + barcodeWidth / 2, y + barcodeWidth / 2);
                            break;
                        case Label.FieldOrientation.Normal:
                            break;
                    }
                }
                else
                {
                    switch (fieldOrientation)
                    {
                        case Label.FieldOrientation.Rotated90:
                            matrix = SKMatrix.CreateRotationDegrees(90, x, y);
                            break;
                        case Label.FieldOrientation.Rotated180:
                            matrix = SKMatrix.CreateRotationDegrees(180, x, y);
                            break;
                        case Label.FieldOrientation.Rotated270:
                            matrix = SKMatrix.CreateRotationDegrees(270, x, y);
                            break;
                        case Label.FieldOrientation.Normal:
                            break;
                    }
                    y -= barcodeHeight;
                }

                y -= labelHeightOffset;

                if (matrix != SKMatrix.Empty)
                {
                    this._skCanvas.SetMatrix(matrix);
                }

                this._skCanvas.DrawBitmap(SKBitmap.Decode(barcodeImageData), x, y);
            }
        }

        protected SKBitmap BitMatrixToSKBitmap(BitMatrix matrix, int pixelScale)
        {
            using var image = new SKBitmap(matrix.Width, matrix.Height);

            for (int row = 0; row < matrix.Height; row++)
            {
                for (int col = 0; col < matrix.Width; col++)
                {
                    var color = matrix[col, row] ? SKColors.Black : SKColors.Transparent;
                    image.SetPixel(col, row, color);
                }
            }

            return image.Resize(new SKSizeI(image.Width * pixelScale, image.Height * pixelScale), SKFilterQuality.None);
        }
    }
}
