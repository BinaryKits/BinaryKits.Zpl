using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;

using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for Text Field elements
    /// </summary>
    public class GraphicSymbolElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element.GetType() == typeof(ZplGraphicSymbol);
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont, int printDensityDpmm)
        {
            if (element is ZplGraphicSymbol graphicSymbol)
            {
                float x = graphicSymbol.PositionX;
                float y = graphicSymbol.PositionY;
                FieldJustification fieldJustification = FieldJustification.None;

                if (graphicSymbol.UseDefaultPosition)
                {
                    x = currentPosition.X;
                    y = currentPosition.Y;
                }

               (float fontSize, float scaleX) = FontScale.GetFontScaling("GS", graphicSymbol.Height, graphicSymbol.Width, printDensityDpmm);

                // remove incorrect scaling
                fontSize /= 1.1f;

                SKTypeface typeface = DrawerOptions.TypefaceGS;

                SKFont skFont = new(typeface, fontSize * 1.25f, scaleX);
                using SKPaint skPaint = new()
                {
                    IsAntialias = options.Antialias
                };

                string displayText = $"{(char)graphicSymbol.Character}";
                float totalWidth = skFont.MeasureText(displayText, out SKRect textBounds);

                using (new SKAutoCanvasRestore(this.skCanvas))
                {
                    SKMatrix matrix = SKMatrix.Empty;

                    if (graphicSymbol.FieldOrigin != null)
                    {
                        switch (graphicSymbol.FieldOrientation)
                        {
                            case FieldOrientation.Rotated90:
                                matrix = SKMatrix.CreateRotationDegrees(90, x + fontSize / 2, y + fontSize / 2);
                                break;
                            case FieldOrientation.Rotated180:
                                matrix = SKMatrix.CreateRotationDegrees(180, x + textBounds.Width / 2, y + fontSize / 2);
                                break;
                            case FieldOrientation.Rotated270:
                                matrix = SKMatrix.CreateRotationDegrees(270, x + textBounds.Width / 2, y + textBounds.Width / 2);
                                break;
                            case FieldOrientation.Normal:
                                break;
                        }

                        fieldJustification = graphicSymbol.FieldOrigin.FieldJustification;
                    }
                    else
                    {
                        switch (graphicSymbol.FieldOrientation)
                        {
                            case FieldOrientation.Rotated90:
                                matrix = SKMatrix.CreateRotationDegrees(90, x, y);
                                break;
                            case FieldOrientation.Rotated180:
                                matrix = SKMatrix.CreateRotationDegrees(180, x, y);
                                break;
                            case FieldOrientation.Rotated270:
                                matrix = SKMatrix.CreateRotationDegrees(270, x, y);
                                break;
                            case FieldOrientation.Normal:
                                break;
                        }

                        fieldJustification = graphicSymbol.FieldTypeset.FieldJustification;
                    }

                    if (matrix != SKMatrix.Empty)
                    {
                        this.skCanvas.Concat(matrix);
                    }

                    if (graphicSymbol.FieldTypeset == null)
                    {
                        y += fontSize;
                    }

                    SKTextAlign textAlign = SKTextAlign.Left;
                    if (fieldJustification == FieldJustification.Left)
                    {
                        textAlign = SKTextAlign.Left;
                    }
                    else if (fieldJustification == FieldJustification.Right)
                    {
                        textAlign = SKTextAlign.Right;
                    }

                    this.skCanvas.DrawText(displayText, x, y, textAlign, skFont, skPaint);

                    // Update the next default field position after rendering
                    return this.CalculateNextDefaultPosition(x, y, totalWidth, textBounds.Height, false, graphicSymbol.FieldOrientation, currentPosition);
                }
            }

            return currentPosition;
        }

    }
}
