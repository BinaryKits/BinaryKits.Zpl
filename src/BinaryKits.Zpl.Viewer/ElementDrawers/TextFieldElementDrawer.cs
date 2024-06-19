using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;
using SkiaSharp;
using SkiaSharp.HarfBuzz;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class TextFieldElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element.GetType() == typeof(ZplTextField);
        }

        public override bool IsReverseDraw(ZplElementBase element)
        {
            if (element is ZplTextField textField)
            {
                return textField.ReversePrint;
            }

            return false;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element, DrawerOptions options)
        {
            if (element is ZplTextField textField)
            {
                float x = textField.PositionX;
                float y = textField.PositionY;
                var fieldJustification = Label.FieldJustification.None;

                var font = textField.Font;

                float fontSize = font.FontHeight > 0 ? font.FontHeight : font.FontWidth;
                var scaleX = 1.00f;
                if (font.FontWidth != 0 && font.FontWidth != fontSize)
                {
                    scaleX *= (float)font.FontWidth / fontSize;
                }

                var typeface = options.FontLoader(font.FontName);

                var skFont = new SKFont(typeface, fontSize, scaleX);
                using var skPaint = new SKPaint(skFont)
                {
                    IsAntialias = options.Antialias
                };

                string displayText = textField.Text;
                if (textField.UseHexadecimalIndicator)
                {
                    displayText = displayText.ReplaceHexEscapes();
                }

                if (options.ReplaceDashWithEnDash)
                {
                    displayText = displayText.Replace("-", " \u2013 ");
                }

                var textBounds = new SKRect();
                var textBoundBaseline = new SKRect();
                skPaint.MeasureText("X", ref textBoundBaseline);
                skPaint.MeasureText(displayText, ref textBounds);

                using (new SKAutoCanvasRestore(this._skCanvas))
                {
                    SKMatrix matrix = SKMatrix.Empty;

                    if (textField.FieldOrigin != null)
                    {
                        switch (textField.Font.FieldOrientation)
                        {
                            case Label.FieldOrientation.Rotated90:
                                matrix = SKMatrix.CreateRotationDegrees(90, x + fontSize / 2, y + fontSize / 2);
                                break;
                            case Label.FieldOrientation.Rotated180:
                                matrix = SKMatrix.CreateRotationDegrees(180, x + textBounds.Width / 2, y + fontSize / 2);
                                break;
                            case Label.FieldOrientation.Rotated270:
                                matrix = SKMatrix.CreateRotationDegrees(270, x + textBounds.Width / 2, y + textBounds.Width / 2);
                                break;
                            case Label.FieldOrientation.Normal:
                                break;
                        }
                        fieldJustification = textField.FieldOrigin.FieldJustification;
                    }
                    else
                    {
                        switch (textField.Font.FieldOrientation)
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
                        fieldJustification = textField.FieldTypeset.FieldJustification;
                    }

                    if (matrix != SKMatrix.Empty)
                    {
                        var currentMatrix = _skCanvas.TotalMatrix;
                        var concatMatrix = SKMatrix.Concat(currentMatrix, matrix);
                        this._skCanvas.SetMatrix(concatMatrix);
                    }

                    if (textField.FieldTypeset == null)
                    {
                        y += textBoundBaseline.Height;
                    }

                    if (textField.ReversePrint)
                    {
                        skPaint.BlendMode = SKBlendMode.Xor;
                    }

                    if (fieldJustification == Label.FieldJustification.Left)
                    {
                        skPaint.TextAlign = SKTextAlign.Left;
                    }
                    else if (fieldJustification == Label.FieldJustification.Right)
                    {
                        skPaint.TextAlign = SKTextAlign.Right;
                    }
                    else if (fieldJustification == Label.FieldJustification.Auto)
                    {
                        var buffer = new HarfBuzzSharp.Buffer();
                        buffer.AddUtf16(displayText);
                        buffer.GuessSegmentProperties();
                        if (buffer.Direction == HarfBuzzSharp.Direction.RightToLeft)
                        {
                            skPaint.TextAlign = SKTextAlign.Right;
                        }
                    }

                    this._skCanvas.DrawShapedText(displayText, x, y, skPaint);

                }
            }
        }
    }
}
