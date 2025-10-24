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

        protected void DrawBarcode(byte[] barcodeImageData, float x, float y, int barcodeWidth, int barcodeHeight, bool useFieldOrigin, Label.FieldOrientation fieldOrientation)
        {
            using (new SKAutoCanvasRestore(this.skCanvas))
            {
                SKMatrix matrix = GetRotationMatrix(x, y, barcodeWidth, barcodeHeight, useFieldOrigin, fieldOrientation);
                if (!useFieldOrigin)
                {
                    y -= barcodeHeight;
                    if (y < 0)
                    {
                        y = 0;
                    }
                }

                if (matrix != SKMatrix.Empty)
                {
                    this.skCanvas.Concat(matrix);
                }

                this.skCanvas.DrawBitmap(SKBitmap.Decode(barcodeImageData), x, y);
            }
        }

        protected void DrawInterpretationLine(string interpretation, SKFont skFont, float x, float y, int barcodeWidth, int barcodeHeight, bool useFieldOrigin, Label.FieldOrientation fieldOrientation, bool printInterpretationLineAboveCode, DrawerOptions options)
        {
            using (new SKAutoCanvasRestore(this.skCanvas))
            {
                using SKPaint skPaint = new()
                {
                    IsAntialias = options.Antialias
                };

                SKMatrix matrix = GetRotationMatrix(x, y, barcodeWidth, barcodeHeight, useFieldOrigin, fieldOrientation);
                if (matrix != SKMatrix.Empty)
                {
                    this.skCanvas.Concat(matrix);
                }

                skFont.MeasureText(interpretation, out SKRect textBounds);
                x += (barcodeWidth - textBounds.Width) / 2;
                if (!useFieldOrigin)
                {
                    y -= barcodeHeight;
                    if (y < 0)
                    {
                        y = 0;
                    }
                }

                float margin = Math.Max((skFont.Spacing - textBounds.Height) / 2, MIN_LABEL_MARGIN);
                if (printInterpretationLineAboveCode)
                {
                    this.skCanvas.DrawShapedText(interpretation, x, y - margin, skFont, skPaint);
                }
                else
                {
                    this.skCanvas
                        .DrawShapedText(interpretation, x, y + barcodeHeight + textBounds.Height + margin, skFont, skPaint);
                }
            }
        }

        protected static SKMatrix GetRotationMatrix(float x, float y, int width, int height, bool useFieldOrigin, Label.FieldOrientation fieldOrientation)
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

        protected static SKBitmap BoolArrayToSKBitmap(bool[] array, int height, int moduleWidth = 1)
        {
            using SKBitmap image = new(array.Length, 1);
            for (int col = 0; col < array.Length; col++)
            {
                SKColor color = array[col] ? SKColors.Black : SKColors.Transparent;
                image.SetPixel(col, 0, color);
            }

            SKSamplingOptions sampling = new(SKFilterMode.Nearest);
            return image.Resize(new SKSizeI(image.Width * moduleWidth, height), sampling);
        }

        protected static SKBitmap BitMatrixToSKBitmap(BitMatrix matrix, int pixelScale)
        {
            using SKBitmap image = new(matrix.Width, matrix.Height);
            for (int row = 0; row < matrix.Height; row++)
            {
                for (int col = 0; col < matrix.Width; col++)
                {
                    SKColor color = matrix[col, row] ? SKColors.Black : SKColors.Transparent;
                    image.SetPixel(col, row, color);
                }
            }

            SKSamplingOptions sampling = new(SKFilterMode.Nearest);
            return image.Resize(new SKSizeI(image.Width * pixelScale, image.Height * pixelScale), sampling);
        }

        protected static bool[] AdjustWidths(bool[] array, int wide, int narrow)
        {
            List<bool> result = [];
            bool last = true;
            int count = 0;
            foreach (bool current in array)
            {
                if (current != last)
                {
                    result.AddRange(Enumerable.Repeat(last, count == 1 ? narrow : wide));
                    last = current;
                    count = 0;
                }

                count += 1;
            }

            result.AddRange(Enumerable.Repeat(last, narrow));
            return result.ToArray();
        }
    }
}
