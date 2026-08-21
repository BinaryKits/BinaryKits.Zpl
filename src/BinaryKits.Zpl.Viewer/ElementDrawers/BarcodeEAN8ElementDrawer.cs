using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;

using SkiaSharp;

using System;

using ZXing.OneD;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for EAN-8 Barcode elements
    /// </summary>
    public class BarcodeEAN8ElementDrawer : BarcodeDrawerBase
    {
        // EAN-8 module layout (67 modules total):
        // 3 (start guard) + 4*7 (left digits) + 5 (middle guard) + 4*7 (right digits) + 3 (end guard)
        // Guard bars (long bars below the barcode) at: start 0,2 | middle 32,34 | end 64,66
        private static readonly bool[] guards = new bool[67];

        static BarcodeEAN8ElementDrawer()
        {
            foreach (int idx in new int[] { 0, 2, 32, 34, 64, 66 })
            {
                guards[idx] = true;
            }
        }

        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcodeEan8;
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont, int printDensityDpmm)
        {
            if (element is ZplBarcodeEan8 barcode)
            {
                float x = barcode.PositionX;
                float y = barcode.PositionY;

                if (barcode.UseDefaultPosition)
                {
                    x = currentPosition.X;
                    y = currentPosition.Y;
                }

                string content = barcode.Content;
                if (barcode.HexadecimalIndicator is char hexIndicator)
                {
                    content = content.ReplaceHexEscapes(hexIndicator, internationalFont);
                }

                content = content.PadLeft(7, '0').Substring(0, 7);
                string interpretation = content;

                // EAN-8 check digit: 3*odd-position digits + even-position digits, then (10 - sum%10) % 10.
                int checksum = 0;
                for (int i = 0; i < 7; i++)
                {
                    checksum += (content[i] - 48) * (i % 2 == 0 ? 3 : 1);
                }
                int checkDigit = (10 - checksum % 10) % 10;
                interpretation = string.Format("{0}{1}", interpretation, checkDigit);

                EAN8Writer writer = new();
                bool[] result = writer.encode(content);
                using SKBitmap resizedImage = BoolArrayToSKBitmap(result, barcode.Height, barcode.ModuleWidth);
                byte[] png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation);

                if (barcode.PrintInterpretationLine)
                {
                    float labelFontSize = FontScale.GetBitmappedFontSize("A", Math.Min(barcode.ModuleWidth, 10), printDensityDpmm).Value;
                    SKTypeface labelTypeFace = options.FontManager.FontLoader("A");
                    SKFont labelFont = new(labelTypeFace, labelFontSize);
                    if (barcode.PrintInterpretationLineAboveCode)
                    {
                        this.DrawInterpretationLine(interpretation, labelFont, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation, true, options);
                    }
                    else
                    {
                        this.DrawEAN8InterpretationLine(interpretation, labelFont, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation, barcode.ModuleWidth, options);
                    }
                }

                return this.CalculateNextDefaultPosition(x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation, currentPosition);
            }

            return currentPosition;
        }

        private void DrawEAN8InterpretationLine(
            string interpretation,
            SKFont skFont,
            float x,
            float y,
            int barcodeWidth,
            int barcodeHeight,
            bool useFieldOrigin,
            FieldOrientation fieldOrientation,
            int moduleWidth,
            DrawerOptions options)
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
                    SKMatrix currentMatrix = this.skCanvas.TotalMatrix;
                    SKMatrix concatMatrix = SKMatrix.Concat(currentMatrix, matrix);
                    this.skCanvas.SetMatrix(concatMatrix);
                }

                skFont.MeasureText(interpretation, out SKRect textBounds);

                if (!useFieldOrigin)
                {
                    y -= barcodeHeight;
                    if (y < 0)
                    {
                        y = 0;
                    }
                }

                float margin = Math.Max((skFont.Spacing - textBounds.Height) / 2, MIN_LABEL_MARGIN);
                int spacing = moduleWidth * 7;

                using SKBitmap guardImage = BoolArrayToSKBitmap(guards, (int)(margin + textBounds.Height / 2), moduleWidth);
                byte[] guardPng = guardImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.skCanvas.DrawBitmap(SKBitmap.Decode(guardPng), x, y + barcodeHeight);

                // Position cursor at the center of the first left-half digit:
                // start guard (3 modules) + half of first digit (3.5 modules) = 6.5 modules from barcode origin.
                x += moduleWidth * 6.5f;
                for (int i = 0; i < interpretation.Length; i++)
                {
                    string digit = interpretation[i].ToString();
                    skFont.MeasureText(digit, out SKRect digitBounds);
                    this.skCanvas.DrawText(digit, x - digitBounds.Width / 2, y + barcodeHeight + textBounds.Height + margin, skFont, skPaint);
                    x += spacing;
                    if (i == 3)
                    {
                        // After the 4 left-half digits, skip the 5-module middle guard.
                        x += moduleWidth * 5;
                    }
                }
            }
        }

    }
}
