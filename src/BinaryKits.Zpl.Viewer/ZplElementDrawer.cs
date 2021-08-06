using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.ElementDrawers;
using SkiaSharp;
using System.Linq;

namespace BinaryKits.Zpl.Viewer
{
    public class ZplElementDrawer
    {
        private readonly IPrinterStorage _printerStorage;
        private readonly IElementDrawer[] _elementDrawers;

        public ZplElementDrawer(IPrinterStorage printerStorage)
        {
            this._printerStorage = printerStorage;
            this._elementDrawers = new IElementDrawer[]
            {
                new GraphicBoxElementDrawer(),
                new GraphicCircleElementDrawer(),
                new ImageMoveElementDrawer(),
                new RecallGraphicElementDrawer(),
                new TextFieldElementDrawer(),
                new Barcode39ElementDrawer(),
                new Barcode128ElementDrawer(),
                new QrCodeElementDrawer()
            };
        }

        public byte[] Draw(ZplElementBase[] elements)
        {
            var padding = 0;

            using var skBitmap = new SKBitmap(900, 2000);
            using var skCanvas = new SKCanvas(skBitmap);
            skCanvas.Clear(SKColors.White);

            using var skPaint = new SKPaint();
            skPaint.Style = SKPaintStyle.Stroke;
            skPaint.Color = SKColors.Black;
            skPaint.StrokeCap = SKStrokeCap.Square;

            foreach (var element in elements)
            {
                skPaint.Style = SKPaintStyle.Stroke;

                var drawer = this._elementDrawers.SingleOrDefault(o => o.CanDraw(element));
                if (drawer != null)
                {
                    drawer.Prepare(this._printerStorage, skPaint, skCanvas, padding);
                    drawer.Draw(element);
                    continue;
                }
            }

            using var data = skBitmap.Encode(SKEncodedImageFormat.Png, 80);
            return data.ToArray();
        }
    }
}
