using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;

using SkiaSharp;
using SkiaSharp.HarfBuzz;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for Text Field elements
    /// </summary>
    public class TextFieldElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element.GetType() == typeof(ZplTextField);
        }

        ///<inheritdoc/>
        public override bool IsReverseDraw(ZplElementBase element)
        {
            if (element is ZplTextField textField)
            {
                return textField.ReversePrint;
            }

            return false;
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont, int printDensityDpmm)
        {
            if (element is ZplTextField textField)
            {
                float x = textField.PositionX;
                float y = textField.PositionY;
                FieldJustification fieldJustification = FieldJustification.None;

                if (textField.UseDefaultPosition)
                {
                    x = currentPosition.X;
                    y = currentPosition.Y;
                }

                ZplFont font = textField.Font;

                (float fontSize, float scaleX) = FontScale.GetFontScaling(font.FontName, font.FontHeight, font.FontWidth, printDensityDpmm);

                SKTypeface typeface = options.FontLoader(font.FontName);

                SKFont skFont = new(typeface, fontSize, scaleX);
                using SKPaint skPaint = new()
                {
                    IsAntialias = options.Antialias
                };

                string displayText = textField.Text;
                if (textField.HexadecimalIndicator is char hexIndicator)
                {
                    displayText = displayText.ReplaceHexEscapes(hexIndicator, internationalFont);
                }

                if (options.ReplaceDashWithEnDash)
                {
                    displayText = displayText.Replace("-", " \u2013 ");
                }

                if (options.ReplaceUnderscoreWithEnSpace)
                {
                    displayText = displayText.Replace('_', '\u2002');
                }

                skFont.MeasureText("X", out SKRect textBoundBaseline);
                float totalWidth = skFont.MeasureText(displayText, out SKRect textBounds);

                using (new SKAutoCanvasRestore(this.skCanvas))
                {
                    SKMatrix matrix = SKMatrix.Empty;

                    if (textField.FieldOrigin != null)
                    {
                        switch (textField.Font.FieldOrientation)
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

                        fieldJustification = textField.FieldOrigin.FieldJustification;
                    }
                    else
                    {
                        switch (textField.Font.FieldOrientation)
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

                        fieldJustification = textField.FieldTypeset.FieldJustification;
                    }

                    if (matrix != SKMatrix.Empty)
                    {
                        this.skCanvas.Concat(matrix);
                    }

                    if (textField.FieldTypeset == null)
                    {
                        y += textBoundBaseline.Height;
                    }

                    if (textField.ReversePrint)
                    {
                        skPaint.BlendMode = SKBlendMode.Xor;
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
                    else if (fieldJustification == FieldJustification.Auto)
                    {
                        HarfBuzzSharp.Buffer buffer = new();
                        buffer.AddUtf16(displayText);
                        buffer.GuessSegmentProperties();
                        if (buffer.Direction == HarfBuzzSharp.Direction.RightToLeft)
                        {
                            textAlign = SKTextAlign.Right;
                        }
                    }

                    this.skCanvas.DrawShapedText(displayText, x, y, textAlign, skFont, skPaint);

                    // Update the next default field position after rendering
                    return this.CalculateNextDefaultPosition(x, y, totalWidth, textBounds.Height, false, textField.Font.FieldOrientation, currentPosition);
                }
            }

            return currentPosition;
        }
    }
}
