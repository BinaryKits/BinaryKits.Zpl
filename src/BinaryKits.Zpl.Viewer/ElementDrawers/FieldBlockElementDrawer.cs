using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;
using SkiaSharp;
using System;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class FieldBlockElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplFieldBlock;
        }

        public override bool IsReverseDraw(ZplElementBase element)
        {
            if (element is ZplFieldBlock fieldBlock)
            {
                return fieldBlock.ReversePrint;
            }

            return false;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplFieldBlock fieldBlock)
            {
                float x = fieldBlock.PositionX;
                float y = fieldBlock.PositionY;

                var font = fieldBlock.Font;

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

                var textLines = fieldBlock.Text.ReplaceSpecialChars().Split(new[] { "\\&" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var textLine in textLines)
                {
                    using var skPaint = new SKPaint();
                    skPaint.Color = SKColors.Black;
                    skPaint.Typeface = typeface;
                    skPaint.TextSize = fontSize;
                    skPaint.TextScaleX = scaleX;
                    //Reset the X point for the next row
                    x = fieldBlock.PositionX;

                    var textBounds = new SKRect();
                    var textBoundBaseline = new SKRect();
                    skPaint.MeasureText(new string('A', fieldBlock.Text.Length), ref textBoundBaseline);
                    skPaint.MeasureText(textLine, ref textBounds);

                    switch (fieldBlock.TextJustification)
                    {
                        case TextJustification.Center:
                            var diff = fieldBlock.Width - textBounds.Width;
                            x += diff / 2;
                            break;
                        case TextJustification.Right:
                            diff = fieldBlock.Width - textBounds.Width;
                            x += diff;
                            break;
                        case TextJustification.Left:
                        case TextJustification.Justified:
                        default:
                            break;
                    }

                    if (fieldBlock.FieldTypeset != null)
                    {
                        y -= textBounds.Height;
                    }

                    using (new SKAutoCanvasRestore(this._skCanvas))
                    {
                        SKMatrix matrix = SKMatrix.Empty;

                        if (fieldBlock.FieldOrigin != null)
                        {
                            switch (fieldBlock.Font.FieldOrientation)
                            {
                                case FieldOrientation.Rotated90:
                                    matrix = SKMatrix.CreateRotationDegrees(90, x, y);
                                    y -= font.FontHeight - textBoundBaseline.Height;
                                    break;
                                case FieldOrientation.Rotated180:
                                    matrix = SKMatrix.CreateRotationDegrees(180, x, y);
                                    x -= textBounds.Width;
                                    y -= font.FontHeight - textBoundBaseline.Height;
                                    break;
                                case FieldOrientation.Rotated270:
                                    matrix = SKMatrix.CreateRotationDegrees(270, x, y);
                                    x -= textBounds.Width;
                                    y += textBoundBaseline.Height;
                                    break;
                                case FieldOrientation.Normal:
                                    y += textBoundBaseline.Height;
                                    break;
                            }
                        }
                        else
                        {
                            switch (fieldBlock.Font.FieldOrientation)
                            {
                                case FieldOrientation.Rotated90:
                                    matrix = SKMatrix.CreateRotationDegrees(90, x, y);
                                    x += textBoundBaseline.Height;
                                    break;
                                case FieldOrientation.Rotated180:
                                    matrix = SKMatrix.CreateRotationDegrees(180, x, y);
                                    y -= textBoundBaseline.Height;
                                    break;
                                case FieldOrientation.Rotated270:
                                    matrix = SKMatrix.CreateRotationDegrees(270, x, y);
                                    x -= textBoundBaseline.Height;
                                    break;
                                case FieldOrientation.Normal:
                                    y += textBoundBaseline.Height;
                                    break;
                            }
                        }

                        if (matrix != SKMatrix.Empty)
                        {
                            this._skCanvas.SetMatrix(matrix);
                        }

                        this._skCanvas.DrawText(textLine, x, y, new SKFont(typeface, fontSize, scaleX, 0), skPaint);
                    }
                }
            }
        }
    }
}
