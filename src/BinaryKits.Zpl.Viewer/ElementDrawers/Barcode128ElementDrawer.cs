using BinaryKits.Zpl.Label.Elements;

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
                var writer = new ZXing.SkiaSharp.BarcodeWriter
                {
                    Format = ZXing.BarcodeFormat.CODE_128
                };

                writer.Options.Height = barcode.Height;
                writer.Options.PureBarcode = !barcode.PrintInterpretationLine;

                //TODO
                //^BY command (narrow bar width)
                //https://github.com/micjahn/ZXing.Net/issues/60
                //https://github.com/zxing/zxing/issues/322

                using var bitmap = writer.Write(barcode.Content);
                this._skCanvas.DrawBitmap(bitmap, barcode.Origin.PositionX, barcode.Origin.PositionY);
            }
        }
    }
}
