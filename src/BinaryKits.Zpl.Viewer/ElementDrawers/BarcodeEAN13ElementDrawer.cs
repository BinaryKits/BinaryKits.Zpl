using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;
using System;
using ZXing.OneD;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for EAN-13 Barcode elements
    /// </summary>
    public class BarcodeEAN13ElementDrawer : BarcodeDrawerBase
    {
        private static readonly bool[] guards = new bool[95];

        static BarcodeEAN13ElementDrawer()
        {
            foreach (int idx in new int[] { 0, 2, 46, 48, 92, 94 })
            {
                guards[idx] = true;
            }
        }

        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcodeEan13;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element, DrawerOptions options)
        {
            if (element is ZplBarcodeEan13 barcode)
            {
                float x = barcode.PositionX;
                float y = barcode.PositionY;

                var content = barcode.Content;
                content = content.PadLeft(12, '0').Substring(0, 12);
                var interpretation = content;

                int checksum = 0;
                for (int i = 0; i < 12; i++)
                {
                    checksum += (content[i] - 48) * (9 - i % 2 * 2);
                }
                interpretation = string.Format("{0}{1}", interpretation, checksum % 10);

                var writer = new EAN13Writer();
                var result = writer.encode(content);
                using var resizedImage = this.BoolArrayToSKBitmap(result, barcode.Height, barcode.ModuleWidth);
                var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation);

                if (barcode.PrintInterpretationLine)
                {
                    float labelFontSize = Math.Min(barcode.ModuleWidth * 10f, 100f);
                    var labelTypeFace = options.FontLoader("A");
                    var labelFont = new SKFont(labelTypeFace, labelFontSize);
                    if (barcode.PrintInterpretationLineAboveCode)
                    {
                        this.DrawInterpretationLine(interpretation, labelFont, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation, true, options);
                    }
                    else
                    {
                        this.DrawEAN13InterpretationLine(interpretation, labelFont, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation, barcode.ModuleWidth, options);
                    }
                }

            }
        }

        private void DrawEAN13InterpretationLine(
            string interpretation,
            SKFont skFont,
            float x,
            float y,
            int barcodeWidth,
            int barcodeHeight,
            bool useFieldOrigin,
            Label.FieldOrientation fieldOrientation,
            int moduleWidth,
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

                if (!useFieldOrigin)
                {
                    y -= barcodeHeight;
                }

                float margin = Math.Max((skFont.Spacing - textBounds.Height) / 2, MIN_LABEL_MARGIN);
                int spacing = moduleWidth * 7;

                using var guardImage = this.BoolArrayToSKBitmap(guards, (int)(margin + textBounds.Height / 2), moduleWidth);
                var guardPng = guardImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this._skCanvas.DrawBitmap(SKBitmap.Decode(guardPng), x, y + barcodeHeight);

                for (int i = 0; i < interpretation.Length; i++)
                {
                    string digit = interpretation[i].ToString();
                    var digitBounds = new SKRect();
                    skPaint.MeasureText(digit, ref digitBounds);
                    this._skCanvas.DrawText(digit, x - (spacing + digitBounds.Width) / 2 - moduleWidth, y + barcodeHeight + textBounds.Height + margin, skPaint);
                    x += spacing;
                    if (i == 0 || i == 6)
                    {
                        x += moduleWidth * 4;
                    }
                }
            }
        }

    }
}
