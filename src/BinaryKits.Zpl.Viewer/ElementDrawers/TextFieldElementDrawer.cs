using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class TextFieldElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplTextField;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplTextField textField)
            {
                this._skPaint.Style = SKPaintStyle.Fill;

                float x = textField.PositionX + this._padding;
                float y = textField.PositionY + this._padding;

                var font = textField.Font;

                float fontSize = font.FontHeight > 0 ? font.FontHeight : font.FontWidth;
                var scaleX = 1.0f;
                if (font.FontWidth != 0 && font.FontWidth != fontSize)
                {
                    scaleX = (float)font.FontWidth / fontSize;
                }

                fontSize *= 0.95f;

                var typeface = SKTypeface.Default;
                if (font.FontName == "0")
                {
                    //typeface = SKTypeface.FromFile(@"swiss-721-black-bt.ttf");
                    typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
                }

                this._skPaint.Typeface = typeface;
                this._skPaint.TextSize = fontSize;
                this._skPaint.TextScaleX = scaleX;

                var textBounds = new SKRect();
                this._skPaint.MeasureText(textField.Text, ref textBounds);

                if (textField.FieldTypeset != null)
                {
                    y -= textBounds.Height;
                }

                using (new SKAutoCanvasRestore(this._skCanvas))
                {
                    SKMatrix matrix = SKMatrix.Empty;

                    if (textField.FieldOrigin != null)
                    {
                        switch (textField.Font.FieldOrientation)
                        {
                            case Label.FieldOrientation.Rotated90:
                                matrix = SKMatrix.CreateRotationDegrees(90, x, y);
                                y -= font.FontHeight - textBounds.Height;
                                break;
                            case Label.FieldOrientation.Rotated180:
                                matrix = SKMatrix.CreateRotationDegrees(180, x, y);
                                x -= textBounds.Width;
                                y -= font.FontHeight - textBounds.Height;
                                break;
                            case Label.FieldOrientation.Rotated270:
                                matrix = SKMatrix.CreateRotationDegrees(270, x, y);
                                x -= textBounds.Width;
                                y += textBounds.Height;
                                break;
                            case Label.FieldOrientation.Normal:
                                y += textBounds.Height;
                                break;
                        }
                    }
                    else
                    {
                        switch (textField.Font.FieldOrientation)
                        {
                            case Label.FieldOrientation.Rotated90:
                                matrix = SKMatrix.CreateRotationDegrees(90, x, y);
                                x += textBounds.Height;
                                break;
                            case Label.FieldOrientation.Rotated180:
                                matrix = SKMatrix.CreateRotationDegrees(180, x, y);
                                y -= textBounds.Height;
                                break;
                            case Label.FieldOrientation.Rotated270:
                                matrix = SKMatrix.CreateRotationDegrees(270, x, y);
                                x -= textBounds.Height;
                                break;
                            case Label.FieldOrientation.Normal:
                                y += textBounds.Height;
                                break;
                        }
                    }

                    if (matrix != SKMatrix.Empty)
                    {
                        this._skCanvas.SetMatrix(matrix);
                    }

                    if (textField.ReversePrint)
                    {
                        this.ReversePrint();
                    }

                    this._skCanvas.DrawText(textField.Text, x, y, new SKFont(typeface, fontSize, scaleX, 0), this._skPaint);
                }
            }
        }
    }
}
