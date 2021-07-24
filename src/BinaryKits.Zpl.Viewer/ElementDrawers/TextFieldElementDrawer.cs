using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class TextFieldElementDrawer : ElementDrawerBase
    {
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplTextField;
        }

        public override void Draw(ZplElementBase element)
        {
            if (element is ZplTextField textField)
            {
                this._skPaint.Style = SKPaintStyle.Fill;

                var x = textField.Origin.PositionX + this._padding;
                var y = textField.Origin.PositionY + this._padding;

                var fontName = textField.Font.FontName;
                var fontHeight = textField.Font.FontHeight;
                var fontWidth = textField.Font.FontWidth;

                var fontSize = fontHeight > 0 ? fontHeight : fontWidth;
                var scaleX = 1.0f;
                if (fontWidth != 0 && fontWidth != fontSize)
                {
                    scaleX = (float)fontWidth / fontSize;
                }

                this._skPaint.TextSize = fontSize;
                var textBounds = new SKRect();
                this._skPaint.MeasureText(textField.Text, ref textBounds);

                using (new SKAutoCanvasRestore(this._skCanvas))
                {
                    if (textField.Font.FieldOrientation == Label.FieldOrientation.Rotated90)
                    {
                        var matrix = SKMatrix.CreateRotationDegrees(90, x, y);
                        this._skCanvas.SetMatrix(matrix);
                    }

                    var typeface = SKTypeface.Default;
                    if (fontName != "0")
                    {
                        //typeface = SKTypeface.FromFile(@"swiss-721-black-bt.ttf");
                        typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
                    }

                    this._skCanvas.DrawText(textField.Text, x, y + textBounds.Height, new SKFont(typeface, fontSize, scaleX, 0), this._skPaint);
                }
            }
        }
    }
}
