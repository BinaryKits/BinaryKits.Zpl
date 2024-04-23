using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.ElementDrawers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
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
                new MaxiCodeElementDrawer(),
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
            return this.DrawMulti(elements, labelWidth, labelHeight, printDensityDpmm)[0];
        }
        
        /// <summary>
        /// Draw the label as PDF
        /// </summary>
        /// <param name="elements">Zpl elements</param>
        /// <param name="labelWidth">Label width in millimeter</param>
        /// <param name="labelHeight">Label height in millimeter</param>
        /// <param name="printDensityDpmm">Dots per millimeter</param>
        /// <returns></returns>
        public byte[] DrawPdf(
            ZplElementBase[] elements,
            double labelWidth = 101.6,
            double labelHeight = 152.4,
            int printDensityDpmm = 8)
        {
            return this.DrawMulti(elements, labelWidth, labelHeight, printDensityDpmm)[1];
        }

        /// <summary>
        /// Draw the label on multiple canvases
        /// </summary>
        /// <param name="elements">Zpl elements</param>
        /// <param name="labelWidth">Label width in millimeter</param>
        /// <param name="labelHeight">Label height in millimeter</param>
        /// <param name="printDensityDpmm">Dots per millimeter</param>
        /// <returns></returns>
        public List<byte[]> DrawMulti(
            ZplElementBase[] elements,
            double labelWidth = 101.6,
            double labelHeight = 152.4,
            int printDensityDpmm = 8)
        {
            var result = new List<byte[]>();
            var imageHistory = new List<SKImage>();
            var labelImageWidth = Convert.ToInt32(labelWidth * printDensityDpmm);
            var labelImageHeight = Convert.ToInt32(labelHeight * printDensityDpmm);
            
            //use SKNWayCanvas to be able to draw on multiple canvases
            using var skCanvas = new SKNWayCanvas(labelImageWidth, labelImageHeight);

            //add Bitmap canvas
            var info = new SKImageInfo(labelImageWidth, labelImageHeight);
            var surface = SKSurface.Create(info);
            using var skImageCanvas = surface.Canvas;
            skCanvas.AddCanvas(skImageCanvas);
            
            //add PDF canvas
            // - When drawing PDF we need the Bitmap as well to fix inverted coloring
            Stream pdfStream = new MemoryStream();
            using var document = SKDocument.CreatePdf(pdfStream);
            using var pdfCanvas = document.BeginPage(labelImageWidth, labelImageHeight);
            if (this._drawerOptions.PdfOutput == true)
            {
                skCanvas.AddCanvas(pdfCanvas);
            }
            
            //make sure to have a transparent canvas for SKBlendMode.Xor to work properly
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
                    //The inverse drawing is moved to the element drawer, so only collect imageHistory for the PDF 
                    if ((element is ZplFieldBlock
                         || element is ZplTextField
                         || element is ZplGraphicCircle
                         || element is ZplGraphicBox
                        )
                        && drawer.IsReverseDraw(element)
                        && !drawer.IsWhiteDraw(element)
                        && !drawer.ForceBitmapDraw(element))
                    {
                        //save state before inverted draw
                        if (this._drawerOptions.PdfOutput == true)
                        {
                            imageHistory.Add(surface.Snapshot());
                        }
                    }
                    else if (drawer.IsReverseDraw(element))
                    {
                        //basically only ZplGraphicBox/Circle depending on requirements
                        using var skBitmapInvert = new SKBitmap(labelImageWidth, labelImageHeight);
                        using var skCanvasInvert = new SKCanvas(skBitmapInvert);
                        skCanvasInvert.Clear(SKColors.Transparent);
                    
                        drawer.Prepare(this._printerStorage, skCanvasInvert);
                        drawer.Draw(element, _drawerOptions);
                        
                        //save state before inverted draw
                        if (this._drawerOptions.PdfOutput == true)
                        {
                            imageHistory.Add(surface.Snapshot());
                        }
                        
                        //use color inversion on an reverse draw white element
                        if (drawer.IsWhiteDraw(element))
                        {
                            this.InvertDrawWhite(skCanvas, skBitmapInvert);
                        }
                        else
                        {
                            this.InvertDraw(skCanvas, skBitmapInvert);
                        }
                        
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

            //check if we need to set a white background
            var image = surface.Snapshot();
            if (this._drawerOptions.OpaqueBackground == true)
            {
                using var surfaceWhiteBg = SKSurface.Create(info);
                using var skImageCanvasWhiteBg = surfaceWhiteBg.Canvas;
                skImageCanvasWhiteBg.Clear(SKColors.White);
                
                var surfaceImage = surface.Snapshot();
                var paint = new SKPaint();
                paint.BlendMode = SKBlendMode.SrcOver;
                skImageCanvasWhiteBg.DrawImage(surfaceImage, 0f, 0f, paint);
                
                image = surfaceWhiteBg.Snapshot();
            }

            var imageData = image.Encode(SKEncodedImageFormat.Png, 80);
            result.Add(imageData.ToArray());

            //only return image
            if (this._drawerOptions.PdfOutput == false)
            {
                result.Add(null);
                return result;
            }

            //Fix the PDF blend
            this.FixPdfInvertDraw(info, imageHistory, surface, skCanvas);

            //close the PDF document
            document.EndPage();
            document.Close();

            //try to export the PDF stream to a byte array
            if (pdfStream is MemoryStream memStream)
            {
                result.Add(memStream.ToArray());
            }
            else
            {
                result.Add(null);
            }

            return result;
        }

        /**
         * PDF transparency and SKBlendMode are not very good friends, SKBlendMode.Xor behaves as SKBlendMode.SrcOver.
         * 
         * This function extracts all the pixels that are removed in the draw process
         * Then that is used to make a white image as overlay in the PDF to get the same effect as SKBlendMode.Xor
         */
        private void FixPdfInvertDraw(SKImageInfo info, List<SKImage> imageHistory, SKSurface surface, SKCanvas skCanvas)
        {
            //fix inverted colors
            using var surfacePdfInvertColorFix = SKSurface.Create(info);
            using var skImageCanvasPdfInvertColorFix = surfacePdfInvertColorFix.Canvas;
            skImageCanvasPdfInvertColorFix.Clear(SKColors.Transparent);
            
            //make an image of everything that was once colored
            foreach (var imageHistoryState in imageHistory)
            {
                var pdfPaint = new SKPaint();
                pdfPaint.BlendMode = SKBlendMode.SrcOver;
                skImageCanvasPdfInvertColorFix.DrawImage(imageHistoryState, 0f, 0f, pdfPaint);
            }
            
            //subtract the parts that are transparent in the final image
            var finalSurfaceImage = surface.Snapshot();
            var pdfFinalPaint = new SKPaint();
            pdfFinalPaint.BlendMode = SKBlendMode.DstOut;
            skImageCanvasPdfInvertColorFix.DrawImage(finalSurfaceImage, 0f, 0f, pdfFinalPaint);
            
            //now invert the colors of the pixels that should be white place it on the canvas
            var pdfTransparentPartsImage = surfacePdfInvertColorFix.Snapshot();
            var pdfTransparentPartsBitmap = SKBitmap.FromImage(pdfTransparentPartsImage);
            var pdfFinalPaintInverted = new SKPaint();
            var inverter = new float[20] {
                -1f,  0f,  0f, 0f, 1f,
                0f, -1f,  0f, 0f, 1f,
                0f,  0f, -1f, 0f, 1f,
                0f,  0f,  0f, 1f, 0f
            };
            pdfFinalPaintInverted.ColorFilter = SKColorFilter.CreateColorMatrix(inverter);
            pdfFinalPaintInverted.BlendMode = SKBlendMode.SrcOver;
            skCanvas.DrawBitmap(pdfTransparentPartsBitmap, 0, 0, pdfFinalPaintInverted);
        }

        private void InvertDraw(SKCanvas baseCanvas, SKBitmap bmToInvert)
        {
            using (SKPaint paint = new SKPaint())
            {
                paint.BlendMode = SKBlendMode.Xor;
                baseCanvas.DrawBitmap(bmToInvert, 0, 0, paint);
            }
        }
        
        private void InvertDrawWhite(SKCanvas baseCanvas, SKBitmap bmToInvert)
        {
            using (SKPaint paint = new SKPaint())
            {
                var inverter = new float[20] {
                    -1f,  0f,  0f, 0f, 1f,
                    0f, -1f,  0f, 0f, 1f,
                    0f,  0f, -1f, 0f, 1f,
                    0f,  0f,  0f, 1f, 0f
                };
                paint.ColorFilter = SKColorFilter.CreateColorMatrix(inverter);
                paint.BlendMode = SKBlendMode.Xor;
                baseCanvas.DrawBitmap(bmToInvert, 0, 0, paint);
            }
        }
    }
}
