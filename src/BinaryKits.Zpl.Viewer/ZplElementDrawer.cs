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
                new Barcode128ElementDrawer(),
                new Barcode39ElementDrawer(),
                new GraphicBoxElementDrawer(),
                new GraphicCircleElementDrawer(),
                new GraphicFieldElementDrawer(),
                new ImageMoveElementDrawer(),
                new QrCodeElementDrawer(),
                new RecallGraphicElementDrawer(),
                new TextFieldElementDrawer()
            };
        }

        public byte[] Draw(ZplElementBase[] elements)
        {
            var padding = 0;

            using var skBitmap = new SKBitmap(900, 2000);
            using var skCanvas = new SKCanvas(skBitmap);
            skCanvas.Clear(SKColors.White);

            foreach (var element in elements)
            {
                var drawer = this._elementDrawers.SingleOrDefault(o => o.CanDraw(element));
                if (drawer != null)
                {
                    if (drawer.IsReverseDraw(element))
                    {
                        using var skBitmapInvert = new SKBitmap(skBitmap.Width, skBitmap.Height);
                        using var skCanvasInvert = new SKCanvas(skBitmapInvert);

                        drawer.Prepare(this._printerStorage, skCanvasInvert, padding);

                        drawer.Draw(element);

                        this.InvertDraw(skBitmap, skBitmapInvert);
                        continue;
                    }

                    drawer.Prepare(this._printerStorage, skCanvas, padding);
                    drawer.Draw(element);

                    continue;
                }
            }

            using var data = skBitmap.Encode(SKEncodedImageFormat.Png, 80);
            return data.ToArray();
        }

        private void InvertDraw(SKBitmap skBitmap, SKBitmap skBitmapInvert)
        {
            for (var row = 0; row < skBitmapInvert.Height; row++)
            {
                for (var column = 0; column < skBitmapInvert.Width; column++)
                {
                    var pixelInvert = skBitmapInvert.GetPixel(column, row);
                    if (pixelInvert.Alpha == 0)
                    {
                        continue;
                    }

                    var pixel = skBitmap.GetPixel(column, row);

                    //Is black in new graphic and white in the origin
                    if (pixelInvert.Blue == 0 && pixel.Blue == 255)
                    {
                        skBitmap.SetPixel(column, row, SKColors.Black);
                    }
                    //Is black in new graphic and black in the origin (invert)
                    else if (pixelInvert.Blue == 0 && pixel.Blue == 0)
                    {
                        skBitmap.SetPixel(column, row, SKColors.White);
                    }
                    //Is white in new graphic and white in the origin (invert)
                    else if (pixelInvert.Blue == 255 && pixel.Blue == 255)
                    {
                        skBitmap.SetPixel(column, row, SKColors.Black);
                    }
                    //Is white in new graphic and black in the origin
                    else if (pixelInvert.Blue == 255 && pixel.Blue == 0)
                    {
                        skBitmap.SetPixel(column, row, SKColors.White);
                    }
                }
            }
        }
    }
}
