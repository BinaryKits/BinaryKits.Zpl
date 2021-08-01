using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public abstract class BarcodeDrawerBase : ElementDrawerBase
    {
        public void DrawBarcode(
            byte[] barcodeImageData,
            int barcodeHeight,
            int barcodeWidth,
            bool useFieldOrigin,
            float x,
            float y,
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
                            matrix = SKMatrix.CreateRotationDegrees(90, x, y);
                            y -= barcodeHeight;
                            break;
                        case Label.FieldOrientation.Rotated180:
                            matrix = SKMatrix.CreateRotationDegrees(180, x, y);
                            x -= barcodeWidth;
                            y -= barcodeHeight;
                            break;
                        case Label.FieldOrientation.Rotated270:
                            matrix = SKMatrix.CreateRotationDegrees(270, x, y);
                            x -= barcodeWidth;
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
                            y -= barcodeHeight;
                            x += barcodeHeight;
                            break;
                        case Label.FieldOrientation.Rotated180:
                            matrix = SKMatrix.CreateRotationDegrees(180, x, y);
                            y -= barcodeHeight * 2;
                            break;
                        case Label.FieldOrientation.Rotated270:
                            matrix = SKMatrix.CreateRotationDegrees(270, x, y);
                            y -= barcodeHeight;
                            x -= barcodeHeight;
                            break;
                        case Label.FieldOrientation.Normal:
                            break;
                    }
                }

                if (matrix != SKMatrix.Empty)
                {
                    this._skCanvas.SetMatrix(matrix);
                }

                this._skCanvas.DrawBitmap(SKBitmap.Decode(barcodeImageData), x, y);
            }
        }
    }
}
