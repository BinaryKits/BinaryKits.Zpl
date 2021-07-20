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
            if (element is ZplBarcode39 barcode39)
            {
                var writer = new ZXing.SkiaSharp.BarcodeWriter
                {
                    Format = ZXing.BarcodeFormat.CODE_39
                };

                writer.Options.Height = barcode39.Height;
                writer.Options.PureBarcode = barcode39.PrintInterpretationLine;

                //TODO
                //^BY command (narrow bar width)
                //https://github.com/micjahn/ZXing.Net/issues/60

                using var bitmap = writer.Write(barcode39.Content);
                this._skCanvas.DrawBitmap(bitmap, barcode39.Origin.PositionX, barcode39.Origin.PositionY);
            }
        }
    }
}
