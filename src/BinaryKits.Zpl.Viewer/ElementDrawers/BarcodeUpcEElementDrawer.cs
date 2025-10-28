using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;

using SkiaSharp;

using System;

using ZXing.OneD;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class BarcodeUpcEElementDrawer : BarcodeDrawerBase
    {
        private static readonly bool[] guards = new bool[51];

        static BarcodeUpcEElementDrawer()
        {
            foreach (int idx in new int[] { 0, 2, 46, 48, 50 })
            {
                guards[idx] = true;
            }
        }

        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcodeUpcE;
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont)
        {
            if (element is ZplBarcodeUpcE barcode)
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

                // [S]DDDDDD[C]
                if (content.Length < 7)
                {
                    // number system 0
                    content = content.PadLeft(7, '0');
                }
                else if (content.Length <= 8)
                {
                    // ignore user provided checksum
                    content = content.Substring(0, 7);
                }
                else
                {
                    // UPC-A to UPC-E
                    string numberSystem = "0";
                    content = content.PadRight(10, '0');
                    if (content.Length > 10)
                    {
                        numberSystem = content.Substring(0, 1);
                        content = content.Substring(1, 10);
                    }

                    int manufacturer = int.Parse(content.Substring(0, 5));
                    int product = int.Parse(content.Substring(5, 5));

                    if (manufacturer % 100 == 0)
                    {
                        int trail = manufacturer / 100 % 10;
                        if (trail <= 2)
                        {
                            content = $"{numberSystem}{manufacturer / 1000:D2}{product % 1000:D3}{trail}";
                        }
                        else
                        {
                            content = $"{numberSystem}{manufacturer / 100:D3}{product % 100:D2}{3}";
                        }
                    }
                    else if (manufacturer % 10 == 0)
                    {
                        content = $"{numberSystem}{manufacturer / 10:D4}{product % 10:D1}{4}";
                    }
                    else
                    {
                        content = $"{numberSystem}{manufacturer:D5}{Math.Max(product % 10, 5):D1}";
                    }
                }

                string interpretation = content;

                if (barcode.PrintCheckDigit)
                {
                    string expanded = UPCEReader.convertUPCEtoUPCA(content);
                    int checksum = 0;
                    for (int i = 0; i < 11; i++)
                    {
                        checksum += (expanded[i] - 48) * (i % 2 * 2 + 7);
                    }

                    interpretation = string.Format("{0}{1}", interpretation, checksum % 10);
                }

                UPCEWriter writer = new();
                bool[] result = writer.encode(content);
                using SKBitmap resizedImage = BoolArrayToSKBitmap(result, barcode.Height, barcode.ModuleWidth);
                byte[] png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation);

                if (barcode.PrintInterpretationLine)
                {
                    float labelFontSize = Math.Min(barcode.ModuleWidth * 10f, 100f);
                    SKTypeface labelTypeFace = options.FontLoader("A");
                    SKFont labelFont = new(labelTypeFace, labelFontSize);
                    if (barcode.PrintInterpretationLineAboveCode)
                    {
                        this.DrawInterpretationLine(interpretation, labelFont, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation, true, options);
                    }
                    else
                    {
                        this.DrawUpcEInterpretationLine(result, interpretation, labelFont, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation, barcode.ModuleWidth, options);
                    }
                }

                return this.CalculateNextDefaultPosition(x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation, currentPosition);
            }

            return currentPosition;
        }

        private void DrawUpcEInterpretationLine(
            bool[] data,
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

                for (int i = 0; i < interpretation.Length; i++)
                {
                    string digit = interpretation[i].ToString();
                    skFont.MeasureText(digit, out SKRect digitBounds);
                    this.skCanvas.DrawText(digit, x - (spacing + digitBounds.Width) / 2 - moduleWidth, y + barcodeHeight + textBounds.Height + margin, skFont, skPaint);
                    x += spacing;

                    if (i == 0)
                    {
                        x += moduleWidth * 4;
                    }
                    else if (i == 6)
                    {
                        x += moduleWidth * 6;
                    }
                }
            }
        }

    }
}
