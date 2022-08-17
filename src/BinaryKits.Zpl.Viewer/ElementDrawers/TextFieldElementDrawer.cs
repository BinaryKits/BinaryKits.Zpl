using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;
using SkiaSharp;
using System;

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
        public override void Draw(ZplElementBase element)
        {
            Draw(element, new DrawerOptions());
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element, DrawerOptions options)
        {
            if (element is ZplTextField textField)
            {
                float x = textField.PositionX;
                float y = textField.PositionY;

                var font = textField.Font;

                float fontSize = font.FontHeight > 0 ? font.FontHeight : font.FontWidth;
                var scaleX = 1.00f;
                if (font.FontWidth != 0 && font.FontWidth != fontSize)
                {
                    scaleX *= (float)font.FontWidth / fontSize;
                }

                var typeface = options.FontLoader(font.FontName);

                var skFont = new SKFont(typeface, fontSize, scaleX);
                using var skPaint = new SKPaint(skFont);

                string displayText = textField.Text;
                if (textField.UseHexadecimalIndicator)
                {
                    displayText = displayText.ReplaceHexEscapes();
                }
                displayText = displayText.Replace("-", " \u2013 ");
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
                                matrix = SKMatrix.CreateRotationDegrees(90, textField.PositionX + fontSize / 2, textField.PositionY + fontSize / 2);
                                break;
                            case Label.FieldOrientation.Rotated180:
                                matrix = SKMatrix.CreateRotationDegrees(180, textField.PositionX + textBounds.Width / 2, textField.PositionY + fontSize / 2);
                                break;
                            case Label.FieldOrientation.Rotated270:
                                matrix = SKMatrix.CreateRotationDegrees(270, textField.PositionX + textBounds.Width / 2, textField.PositionY + textBounds.Width / 2);
                                break;
                            case Label.FieldOrientation.Normal:
                                break;
                        }
                    }
                    else
                    {
                        switch (textField.Font.FieldOrientation)
                        {
                            case Label.FieldOrientation.Rotated90:
                                matrix = SKMatrix.CreateRotationDegrees(90, textField.PositionX, textField.PositionY);
                                break;
                            case Label.FieldOrientation.Rotated180:
                                matrix = SKMatrix.CreateRotationDegrees(180, textField.PositionX, textField.PositionY);
                                break;
                            case Label.FieldOrientation.Rotated270:
                                matrix = SKMatrix.CreateRotationDegrees(270, textField.PositionX, textField.PositionY);
                                break;
                            case Label.FieldOrientation.Normal:
                                break;
                        }
                    }

                    if (matrix != SKMatrix.Empty)
                    {
                        this._skCanvas.SetMatrix(matrix);
                    }

                    if (textField.FieldTypeset == null)
                    {
                        y += textBoundBaseline.Height;
                    }

                    this._skCanvas.DrawText(displayText, x, y, skFont, skPaint);
                }
            }
        }
    }
}
