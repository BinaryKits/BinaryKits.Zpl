using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class QrCodeElementDrawer : ElementDrawerBase
    {
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplQrCode;
        }

        public override void Draw(ZplElementBase element)
        {
            if (element is ZplQrCode barcode)
            {
                float x = barcode.PositionX + this._padding;
                float y = barcode.PositionY + this._padding;

                var writer = new ZXing.SkiaSharp.BarcodeWriter
                {
                    Format = ZXing.BarcodeFormat.QR_CODE
                };

                using var bitmap = writer.Write(barcode.Content);
                this._skCanvas.DrawBitmap(bitmap, x, y);
            }
        }
    }
}
