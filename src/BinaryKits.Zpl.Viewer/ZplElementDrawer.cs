using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.ElementDrawers;
using SkiaSharp;
using System;
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
                new FieldBlockElementDrawer(),
                new GraphicBoxElementDrawer(),
                new GraphicCircleElementDrawer(),
                new GraphicFieldElementDrawer(),
                new Interleaved2of5BarcodeDrawer(),
                new ImageMoveElementDrawer(),
                new QrCodeElementDrawer(),
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
            double labelWidth = 102,
            double labelHeight = 152,
            int printDensityDpmm = 8)
        {
            SKBitmap skBitmap = DrawBitmap(elements,labelWidth,labelHeight,printDensityDpmm);
            using var data = skBitmap.Encode(SKEncodedImageFormat.Png, 80);
            return data.ToArray();
        }

        /// <summary>
        /// Draw the label to bitmap
        /// </summary>
        /// <param name="elements">Zpl elements</param>
        /// <param name="labelWidth">Label width in millimeter</param>
        /// <param name="labelHeight">Label height in millimeter</param>
        /// <param name="printDensityDpmm">Dots per millimeter</param>
        /// <returns></returns>
        public SKBitmap DrawBitmap(
            ZplElementBase[] elements,
            double labelWidth = 102,
            double labelHeight = 152,
            int printDensityDpmm = 8)
        {
            var labelImageWidth = Convert.ToInt32(labelWidth * printDensityDpmm);
            var labelImageHeight = Convert.ToInt32(labelHeight * printDensityDpmm);

            var skBitmap = new SKBitmap(labelImageWidth, labelImageHeight);
            using var skCanvas = new SKCanvas(skBitmap);
            skCanvas.Clear(SKColors.White);

            bool drawInverted = false;
            foreach (var element in elements)
            {
                var drawer = this._elementDrawers.SingleOrDefault(o => o.CanDraw(element));
                if (drawer != null)
                {
                    if (drawer.IsReverseDraw(element))
                    {
                        drawInverted = true;
                    } else {
                        drawer.Prepare(this._printerStorage, skCanvas);
                        drawer.Draw(element);
                    }
                }
            }

            if (drawInverted)
            {
                using var skBitmapInvert = new SKBitmap(skBitmap.Width, skBitmap.Height);
                using var skCanvasInvert = new SKCanvas(skBitmapInvert);

                foreach (var element in elements)
                {
                    var drawer = this._elementDrawers.SingleOrDefault(o => o.CanDraw(element));
                    if (drawer != null)
                    {
                        if (drawer.IsReverseDraw(element))
                        {
                            drawer.Prepare(this._printerStorage, skCanvasInvert);
                            drawer.Draw(element);
                        }
                    }
                }
                this.InvertDraw(skBitmap, skBitmapInvert);
            }

            return skBitmap;
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
