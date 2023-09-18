using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.ElementDrawers;
using SkiaSharp;
using System;
using System.Linq;

namespace BinaryKits.Zpl.Viewer
{
    public class ZplElementDrawer
    {
        private readonly DrawerOptions _drawerOptions;
        private readonly IPrinterStorage _printerStorage;
        private readonly IElementDrawer[] _elementDrawers;

        public ZplElementDrawer(IPrinterStorage printerStorage, DrawerOptions drawerOptions = null)
        {
            if (drawerOptions == null)
            {
                drawerOptions = new DrawerOptions();
            }
            this._drawerOptions = drawerOptions;
            this._printerStorage = printerStorage;
            this._elementDrawers = new IElementDrawer[]
            {
                new Barcode128ElementDrawer(),
                new Barcode39ElementDrawer(),
                new BarcodeEAN13ElementDrawer(),
                new DataMatrixElementDrawer(),
                new FieldBlockElementDrawer(),
                new GraphicBoxElementDrawer(),
                new GraphicCircleElementDrawer(),
                new GraphicFieldElementDrawer(),
                new Interleaved2of5BarcodeDrawer(),
                new ImageMoveElementDrawer(),
                new QrCodeElementDrawer(),
                new Pdf417ElementDrawer(),
                new RecallGraphicElementDrawer(),
                new TextFieldElementDrawer()
            };
        }

        /// <summary>
        /// Draw the label
        /// </summary>
        /// <param name="elements">Zpl elements</param>
        /// <param name="labelWidth">Label width in millimeter</param>
        /// <param name="labelHeight">Label height in millimeter</param>
        /// <param name="printDensityDpmm">Dots per millimeter</param>
        /// <returns></returns>
        public byte[] Draw(
            ZplElementBase[] elements,
            double labelWidth = 101.6,
            double labelHeight = 152.4,
            int printDensityDpmm = 8)
        {
            var labelImageWidth = Convert.ToInt32(labelWidth * printDensityDpmm);
            var labelImageHeight = Convert.ToInt32(labelHeight * printDensityDpmm);

            using var skBitmap = new SKBitmap(labelImageWidth, labelImageHeight);
            using var skCanvas = new SKCanvas(skBitmap);
            skCanvas.Clear(SKColors.Transparent);

            foreach (var element in elements)
            {
                var drawer = this._elementDrawers.SingleOrDefault(o => o.CanDraw(element));
                if (drawer == null)
                {
                    continue;
                }

                try
                {
                    if (drawer.IsReverseDraw(element))
                    {
                        using var skBitmapInvert = new SKBitmap(skBitmap.Width, skBitmap.Height);
                        using var skCanvasInvert = new SKCanvas(skBitmapInvert);

                        drawer.Prepare(this._printerStorage, skCanvasInvert);

                        drawer.Draw(element, _drawerOptions);

                        this.InvertDraw(skBitmap, skBitmapInvert);
                        continue;
                    }

                    drawer.Prepare(this._printerStorage, skCanvas);
                    drawer.Draw(element, _drawerOptions);

                    continue;
                }
                catch (Exception ex)
                {
                    if (element is ZplBarcode barcodeElement)
                        throw new Exception($"Error on zpl element \"{barcodeElement.Content}\": {ex.Message}", ex);
                    else if (element is ZplDataMatrix dataMatrixElement)
                        throw new Exception($"Error on zpl element \"{dataMatrixElement.Content}\": {ex.Message}", ex);
                    else
                    {
                        throw;
                    }
                }
            }

            SKBitmap finalBitmap;
            if (this._drawerOptions.OpaqueBackground == true)
            {
                finalBitmap = new SKBitmap(labelImageWidth, labelImageHeight);
                using (SKCanvas canvas = new SKCanvas(finalBitmap))
                {
                    SKPaint paint = new SKPaint
                    {
                        IsAntialias = true,
                        FilterQuality = SKFilterQuality.High
                    };
                    canvas.Clear(SKColors.White);
                    canvas.DrawBitmap(skBitmap, 0, 0, paint);
                }
            }
            else {
                finalBitmap = skBitmap;
            }

            using var data = finalBitmap.Encode(_drawerOptions.RenderFormat, _drawerOptions.RenderQuality);
            return data.ToArray();
        }

        private void InvertDraw(SKBitmap skBitmap, SKBitmap skBitmapInvert)
        {
            // Fast local copy
            var originalBytes = skBitmap.GetPixelSpan();
            var invertBytes = skBitmapInvert.GetPixelSpan();

            int total = originalBytes.Length / 4;
            for (int i = 0; i < total; i++)
            {
                // RGBA8888
                int rLoc = (i << 2);
                int gLoc = (i << 2) + 1;
                int bLoc = (i << 2) + 2;
                int aLoc = (i << 2) + 3;
                if (invertBytes[aLoc] == 0)
                {
                    continue;
                }

                // Set color
                byte rByte = (byte)(originalBytes[rLoc] ^ invertBytes[rLoc]);
                byte gByte = (byte)(originalBytes[gLoc] ^ invertBytes[gLoc]);
                byte bByte = (byte)(originalBytes[bLoc] ^ invertBytes[bLoc]);
                byte aByte = (byte)(originalBytes[aLoc] ^ invertBytes[aLoc]);

                var targetColor = new SKColor(rByte, gByte, bByte, aByte);

                int x, y;
                y = Math.DivRem(i, skBitmapInvert.Width, out x);

                skBitmap.SetPixel(x, y, targetColor);
            }
        }
    }
}
