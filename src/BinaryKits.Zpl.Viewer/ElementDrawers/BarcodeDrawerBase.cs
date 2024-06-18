using SkiaSharp;
using SkiaSharp.HarfBuzz;
using System;
using System.Collections.Generic;
using System.Linq;
using ZXing.Common;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Base clase for Barcode element drawers
    /// </summary>
    public abstract class BarcodeDrawerBase : ElementDrawerBase
    {
        /// <summary>
        /// Minimum acceptable magin between a barcode and its interpretation line, in pixels
        /// </summary>
        protected const float MIN_LABEL_MARGIN = 5f;

        protected void DrawBarcode(
            byte[] barcodeImageData,
            float x,
            float y,
            int barcodeWidth,
            int barcodeHeight,
            bool useFieldOrigin,
            Label.FieldOrientation fieldOrientation)
        {
            using (new SKAutoCanvasRestore(this._skCanvas))
            {
                SKMatrix matrix = this.GetRotationMatrix(x, y, barcodeWidth, barcodeHeight, useFieldOrigin, fieldOrientation);

                if (!useFieldOrigin)
                {
                    y -= barcodeHeight;
                }

                if (matrix != SKMatrix.Empty)
                {
                    var currentMatrix = _skCanvas.TotalMatrix;
                    var concatMatrix = SKMatrix.Concat(currentMatrix, matrix);
                    this._skCanvas.SetMatrix(concatMatrix);
                }

                this._skCanvas.DrawBitmap(SKBitmap.Decode(barcodeImageData), x, y);
            }
        }

        protected void DrawInterpretationLine(
            string interpretation,
            SKFont skFont,
            float x,
            float y,
            int barcodeWidth,
            int barcodeHeight,
            bool useFieldOrigin,
            Label.FieldOrientation fieldOrientation,
            bool printInterpretationLineAboveCode,
            DrawerOptions options)
        {
            using (new SKAutoCanvasRestore(this._skCanvas))
            {
                using var skPaint = new SKPaint(skFont);
                skPaint.IsAntialias = options.Antialias;

                SKMatrix matrix = this.GetRotationMatrix(x, y, barcodeWidth, barcodeHeight, useFieldOrigin, fieldOrientation);

                if (matrix != SKMatrix.Empty)
                {
                    var currentMatrix = _skCanvas.TotalMatrix;
                    var concatMatrix = SKMatrix.Concat(currentMatrix, matrix);
                    this._skCanvas.SetMatrix(concatMatrix);
                }

                var textBounds = new SKRect();
                skPaint.MeasureText(interpretation, ref textBounds);

                x += (barcodeWidth - textBounds.Width) / 2;
                if (!useFieldOrigin)
                {
                    y -= barcodeHeight;
                }

                float margin = Math.Max((skFont.Spacing - textBounds.Height) / 2, MIN_LABEL_MARGIN);

                if (printInterpretationLineAboveCode)
                {
                    this._skCanvas.DrawShapedText(interpretation, x, y - margin, skPaint);
                }
                else
                {
                    this._skCanvas.DrawShapedText(interpretation, x, y + barcodeHeight + textBounds.Height + margin, skPaint);
                }
            }
        }

        protected SKMatrix GetRotationMatrix(float x, float y, int width, int height, bool useFieldOrigin, Label.FieldOrientation fieldOrientation)
        {
            SKMatrix matrix = SKMatrix.Empty;

            if (useFieldOrigin)
            {
                switch (fieldOrientation)
                {
                    case Label.FieldOrientation.Rotated90:
                        matrix = SKMatrix.CreateRotationDegrees(90, x + height / 2, y + height / 2);
                        break;
                    case Label.FieldOrientation.Rotated180:
                        matrix = SKMatrix.CreateRotationDegrees(180, x + width / 2, y + height / 2);
                        break;
                    case Label.FieldOrientation.Rotated270:
                        matrix = SKMatrix.CreateRotationDegrees(270, x + width / 2, y + width / 2);
                        break;
                    case Label.FieldOrientation.Normal:
                        break;
                }
            }
            else
            {
                switch (fieldOrientation)
                {
                    case Label.FieldOrientation.Rotated90:
                        matrix = SKMatrix.CreateRotationDegrees(90, x, y);
                        break;
                    case Label.FieldOrientation.Rotated180:
                        matrix = SKMatrix.CreateRotationDegrees(180, x, y);
                        break;
                    case Label.FieldOrientation.Rotated270:
                        matrix = SKMatrix.CreateRotationDegrees(270, x, y);
                        break;
                    case Label.FieldOrientation.Normal:
                        break;
                }
            }

            return matrix;
        }

        protected SKBitmap BoolArrayToSKBitmap(bool[] array, int height, int moduleWidth = 1)
        {
            using var image = new SKBitmap(array.Length, 1);

            for (int col = 0; col < array.Length; col++)
            {
                var color = array[col] ? SKColors.Black : SKColors.Transparent;
                image.SetPixel(col, 0, color);
            }

            return image.Resize(new SKSizeI(image.Width * moduleWidth, height), SKFilterQuality.None);
        }

        protected SKBitmap BitMatrixToSKBitmap(BitMatrix matrix, int pixelScale)
        {
            using var image = new SKBitmap(matrix.Width, matrix.Height);

            for (int row = 0; row < matrix.Height; row++)
            {
                for (int col = 0; col < matrix.Width; col++)
                {
                    var color = matrix[col, row] ? SKColors.Black : SKColors.Transparent;
                    image.SetPixel(col, row, color);
                }
            }

            return image.Resize(new SKSizeI(image.Width * pixelScale, image.Height * pixelScale), SKFilterQuality.None);
        }

        protected bool[] AdjustWidths(bool[] array, int wide, int narrow)
        {
            List<bool> result = new List<bool>();
            var last = true;
            var count = 0;
            foreach (var current in array)
            {
                if (current != last)
                {
                    result.AddRange(Enumerable.Repeat<bool>(last, count == 1 ? narrow : wide));
                    last = current;
                    count = 0;
                }

                count += 1;
            }

            result.AddRange(Enumerable.Repeat<bool>(last, narrow));

            return result.ToArray();
        }
    }
}
