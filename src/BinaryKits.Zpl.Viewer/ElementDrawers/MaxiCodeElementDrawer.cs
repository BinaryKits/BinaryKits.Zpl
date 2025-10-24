using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;
using BinaryKits.Zpl.Viewer.Symologies;

using SkiaSharp;

using System;
using System.Collections;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for MaxiCode Barcode Elements.
    /// </summary>
    public class MaxiCodeElementDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplMaxiCode;
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont, int printDensityDpmm)
        {
            if (element is ZplMaxiCode maxiCode)
            {
                float x = maxiCode.PositionX;
                float y = maxiCode.PositionY;

                if (maxiCode.UseDefaultPosition)
                {
                    x = currentPosition.X;
                    y = currentPosition.Y;
                }

                string content = maxiCode.Content;
                if (maxiCode.HexadecimalIndicator is char hexIndicator)
                {
                    content = content.ReplaceHexEscapes(hexIndicator, internationalFont);
                }

                bool[] data = MaxiCodeSymbology.Encode(content, maxiCode.Mode);

                SKBitmap image = DrawMaxiCode(data, printDensityDpmm);
                byte[] png = image.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, x, y, image.Width, image.Height, maxiCode.FieldOrigin != null, maxiCode.FieldOrientation);
                return this.CalculateNextDefaultPosition(x, y, image.Width, image.Height, maxiCode.FieldOrigin != null, maxiCode.FieldOrientation, currentPosition);
            }

            return currentPosition;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Match documentation.", Scope = "member")]
        private static SKBitmap DrawMaxiCode(bool[] data, int dpmm)
        {
            // ISO/IEC 16023:2000 pp. 16, 38-40
            // fundamental dimensions
            float L, H, W, V, X, Y;

            // gutters
            float gX, gV;

            // dark hex pattern
            SKPoint[] pattern;
            float xoff, yoff;

            if (dpmm == 8)
            {
                W = 7;
                V = 8;
                X = W;
                Y = 6;

                gX = 1;
                gV = 1;

                xoff = 4f;
                yoff = 2f;
                pattern = [
                    new SKPoint(0f, 3f),
                    new SKPoint(2f, 2f),
                    new SKPoint(1.5f, 0f),
                    new SKPoint(2f, -2f),
                    new SKPoint(0f, -3f),
                    new SKPoint(-2f, -2f),
                    new SKPoint(-1.5f, 0f)
                ];

                L = 29 * W;
                H = 32 * Y;
            }
            else if (dpmm == 12)
            {
                W = 10;
                V = 12;
                X = W;
                Y = 9;

                gX = 2;
                gV = 2;

                xoff = 5f;
                yoff = 3f;
                pattern = [
                    new SKPoint(0f, 4f),
                    new SKPoint(3f, 3f),
                    new SKPoint(1.5f, 0f),
                    new SKPoint(3f, -3f),
                    new SKPoint(0f, -4f),
                    new SKPoint(-3f, -3f),
                    new SKPoint(-1.5f, 0f)
                ];

                L = 29 * W;
                H = 32 * Y;
            }
            else
            {
                L = 25.50f * dpmm;

                W = L / 29;
                V = 1.1547f * W; // (2/Math.Sqrt(3)) * W
                X = W;
                Y = 0.866f * W; // (Math.Sqrt(3)/2) * W

                H = 32 * Y;

                gX = dpmm / 6f;
                gV = 1.1547f * gX;

                xoff = W / 2;
                yoff = (V - gV) / 4;

                // drawn hexagon dimensions
                float hexW = (X - gX) / 2; // half width
                float hexH = (V - gV) / 4; // quarter height

                pattern = [
                    new SKPoint(0f, hexH * 2),
                    new SKPoint(hexW, hexH),
                    new SKPoint(hexW, -hexH),
                    new SKPoint(0f, -hexH * 2),
                    new SKPoint(-hexW, -hexH)
                ];
            }

            // finder radii
            float R1 = 0.51f * dpmm;
            float R2 = 1.18f * dpmm;
            float R3 = 1.86f * dpmm;
            float R4 = 2.53f * dpmm;
            float R5 = 3.20f * dpmm;
            float R6 = 3.87f * dpmm;

            using SKBitmap image = new((int)Math.Ceiling(L + X - gX), (int)Math.Ceiling(H + V - gV));
            using SKCanvas skCanvas = new(image);
            using SKPaint skPaint = new()
            {
                IsAntialias = false,
                Color = SKColors.Black,
                Style = SKPaintStyle.Fill,
            };

            SKPath path = new();
            IEnumerator dataEnum = data.GetEnumerator();
            for (int j = 0; j < 33; j++)
            {
                for (int i = 0; i < 30 - j % 2; i++)
                {
                    dataEnum.MoveNext();
                    if ((bool)dataEnum.Current)
                    {
                        path.MoveTo(i * W + j % 2 * xoff, j * Y + yoff);
                        foreach (SKPoint point in pattern)
                        {
                            path.RLineTo(point);
                        }

                        path.Close();
                    }
                }
            }

            float finderX = 14 * W + (X - gX) / 2;
            float finderY = 16 * Y + (V - gV) / 2;

            path.AddCircle(finderX, finderY, R1, SKPathDirection.CounterClockwise);
            path.AddCircle(finderX, finderY, R2, SKPathDirection.Clockwise);
            path.Close();
            path.AddCircle(finderX, finderY, R3, SKPathDirection.CounterClockwise);
            path.AddCircle(finderX, finderY, R4, SKPathDirection.Clockwise);
            path.Close();
            path.AddCircle(finderX, finderY, R5, SKPathDirection.CounterClockwise);
            path.AddCircle(finderX, finderY, R6, SKPathDirection.Clockwise);
            path.Close();

            skCanvas.DrawPath(path, skPaint);

            // labelary
            //return image.Resize(new SKSizeI(200, 193), SKFilterQuality.High); //  8dpmm
            //return image.Resize(new SKSizeI(300, 289), SKFilterQuality.High); // 12dpmm
            //return image.Resize(new SKSizeI(600, 579), SKFilterQuality.High); // 24dpmm

            // ISO
            return image.Copy();
        }

    }
}
