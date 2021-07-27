using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class Barcode39ElementDrawer : ElementDrawerBase
    {
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcode39;
        }

        public override void Draw(ZplElementBase element)
        {
            if (element is ZplBarcode39 barcode)
            {
                var writer = new ZXing.SkiaSharp.BarcodeWriter
                {
                    Format = ZXing.BarcodeFormat.CODE_39
                };

                writer.Options.Height = barcode.Height;
                writer.Options.PureBarcode = barcode.PrintInterpretationLine;

                //TODO:narrow bar width
                //^BY command (narrow bar width)
                //https://github.com/micjahn/ZXing.Net/issues/60
                //https://github.com/zxing/zxing/issues/322

                using var bitmap = writer.Write(barcode.Content);
                this._skCanvas.DrawBitmap(bitmap, barcode.Origin.PositionX, barcode.Origin.PositionY);
            }
        }
    }
}
