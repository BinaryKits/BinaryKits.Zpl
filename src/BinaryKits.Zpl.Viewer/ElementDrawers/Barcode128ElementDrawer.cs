using BinaryKits.Zpl.Label.Elements;
using NetBarcode;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class Barcode128ElementDrawer : ElementDrawerBase
    {
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcode128;
        }

        public override void Draw(ZplElementBase element)
        {
            if (element is ZplBarcode128 barcode)
            {
                float x = barcode.PositionX + this._padding;
                float y = barcode.PositionY + this._padding;

                if (barcode.FieldTypeset != null)
                {
                    y -= barcode.Height;
                }

                #region NetBarcode
               
                var barcodeElement = new Barcode(barcode.Content, Type.Code128B, false, 0, barcode.Height, null);
                var barcodeWidth = barcodeElement.GetImage().Width;

                //TODO Optimize logic
                if (barcode.ModuleWidth != 3)
                {
                    barcodeElement = new Barcode(barcode.Content, Type.Code128B, false, (int)(barcodeWidth * (barcode.ModuleWidth / 2.0f)), barcode.Height, null);
                    barcodeWidth = barcodeElement.GetImage().Width;
                }

                var barcodeImageData = barcodeElement.GetByteArray();

                #endregion

                #region XZing

                //var writer = new ZXing.SkiaSharp.BarcodeWriter
                //{
                //    Format = ZXing.BarcodeFormat.CODE_128
                //};

                //writer.Options.Height = barcode.Height;
                //writer.Options.PureBarcode = !barcode.PrintInterpretationLine;
                //writer.Options.Width = barcode.Content.Length * 80;

                //TODO:narrow bar width
                //^BY command (narrow bar width)
                //https://github.com/micjahn/ZXing.Net/issues/60
                //https://github.com/zxing/zxing/issues/322

                //http://www.keepautomation.com/online_barcode_generator/code_128/
                //Require Code 128B
                //https://github.com/micjahn/ZXing.Net/issues/351

                //using var bitmap = writer.Write(barcode.Content);

                #endregion

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

                    //this._skCanvas.DrawBitmap(bitmap, x, y);
                    this._skCanvas.DrawBitmap(SKBitmap.Decode(barcodeImageData), x, y);
                }
            }
        }
    }
}
