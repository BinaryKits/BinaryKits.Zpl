using BinaryKits.Zpl.Label.Elements;
using NetBarcode;
using SkiaSharp;
using System.Drawing;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class Barcode39ElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcode39;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplBarcode39 barcode)
            {
                float x = barcode.PositionX + this._padding;
                float y = barcode.PositionY + this._padding;

                if (barcode.FieldTypeset != null)
                {
                    y -= barcode.Height;
                }

                var barcodeElement = new Barcode();
                barcodeElement.Configure(new BarcodeSettings
                {
                    BarcodeHeight = barcode.Height,
                    BarcodeType = BarcodeType.Code39E,
                    BarWidth = barcode.ModuleWidth,
                    BackgroundColor = Color.Transparent
                });

                var barcodeWidth = barcodeElement.GetImage(barcode.Content).Width;
                var barcodeImageData = barcodeElement.GetByteArray(barcode.Content);

                using (new SKAutoCanvasRestore(this._skCanvas))
                {
                    SKMatrix matrix = SKMatrix.Empty;

                    switch (barcode.FieldOrientation)
                    {
                        case Label.FieldOrientation.Rotated90:
                            matrix = SKMatrix.CreateRotationDegrees(90, x, y);
                            x += barcode.Height;
                            break;
                        case Label.FieldOrientation.Rotated180:
                            matrix = SKMatrix.CreateRotationDegrees(180, x, y);
                            break;
                        case Label.FieldOrientation.Rotated270:
                            matrix = SKMatrix.CreateRotationDegrees(270, x, y);
                            y -= barcode.Height;
                            x -= barcodeWidth;
                            break;
                        case Label.FieldOrientation.Normal:
                            break;
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
}
