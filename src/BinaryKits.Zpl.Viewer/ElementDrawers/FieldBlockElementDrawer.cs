using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Draw(element, new DrawerOptions());
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element, DrawerOptions options)
        {
            if (element is ZplFieldBlock fieldBlock)
            {
                var font = fieldBlock.Font;

                float fontSize = font.FontHeight > 0 ? font.FontHeight : font.FontWidth;
                var scaleX = 1.00f;
                if (font.FontWidth != 0 && font.FontWidth != fontSize)
                {
                    scaleX *= (float)font.FontWidth / fontSize;
                }

                var typeface = options.FontLoader(font.FontName);
                var text = fieldBlock.Text;
                if (fieldBlock.UseHexadecimalIndicator)
                {
                    text = text.ReplaceHexEscapes();
                }
                text = text.Replace("-", " \u2013 ");

                var skFont = new SKFont(typeface, fontSize, scaleX);
                using var skPaint = new SKPaint(skFont);
                var textBoundBaseline = new SKRect();
                skPaint.MeasureText("X", ref textBoundBaseline);

                float x = fieldBlock.PositionX;
                float y = fieldBlock.PositionY + textBoundBaseline.Height;

                var textLines = WordWrap(text, skFont, fieldBlock.Width);
                var hangingIndent = 0;
                var lineHeight = fontSize + fieldBlock.LineSpace;

                // actual ZPL printer does not include trailing line spacing in total height
                var totalHeight = lineHeight * fieldBlock.MaxLineCount - fieldBlock.LineSpace;
                // labelary
                //var totalHeight = lineHeight * fieldBlock.MaxLineCount;

                if (fieldBlock.FieldTypeset != null)
                {
                    totalHeight = lineHeight * (fieldBlock.MaxLineCount-1) + textBoundBaseline.Height;
                    y -= totalHeight;
                }

                using (new SKAutoCanvasRestore(this._skCanvas))
                {
                    SKMatrix matrix = SKMatrix.Empty;

                    if (fieldBlock.FieldOrigin != null)
                    {
                        switch (fieldBlock.Font.FieldOrientation)
                        {
                            case FieldOrientation.Rotated90:
                                matrix = SKMatrix.CreateRotationDegrees(90, fieldBlock.PositionX + totalHeight / 2, fieldBlock.PositionY + totalHeight / 2);
                                break;
                            case FieldOrientation.Rotated180:
                                matrix = SKMatrix.CreateRotationDegrees(180, fieldBlock.PositionX + fieldBlock.Width / 2, fieldBlock.PositionY + totalHeight / 2);
                                break;
                            case FieldOrientation.Rotated270:
                                matrix = SKMatrix.CreateRotationDegrees(270, fieldBlock.PositionX + fieldBlock.Width / 2, fieldBlock.PositionY + fieldBlock.Width / 2);
                                break;
                            case FieldOrientation.Normal:
                                break;
                        }
                    }
                    else
                    {
                        switch (fieldBlock.Font.FieldOrientation)
                        {
                            case FieldOrientation.Rotated90:
                                matrix = SKMatrix.CreateRotationDegrees(90, fieldBlock.PositionX, fieldBlock.PositionY);
                                break;
                            case FieldOrientation.Rotated180:
                                matrix = SKMatrix.CreateRotationDegrees(180, fieldBlock.PositionX, fieldBlock.PositionY);
                                break;
                            case FieldOrientation.Rotated270:
                                matrix = SKMatrix.CreateRotationDegrees(270, fieldBlock.PositionX, fieldBlock.PositionY);
                                break;
                            case FieldOrientation.Normal:
                                break;
                        }
                    }

                    if (matrix != SKMatrix.Empty)
                    {
                        this._skCanvas.SetMatrix(matrix);
                    }

                    foreach (var textLine in textLines)
                    {
                        x = fieldBlock.PositionX + hangingIndent;

                        var textBounds = new SKRect();
                        skPaint.MeasureText(textLine, ref textBounds);
                        var diff = fieldBlock.Width - textBounds.Width;

                        switch (fieldBlock.TextJustification)
                        {
                            case TextJustification.Center:
                                x += diff / 2 - textBounds.Left;
                                break;
                            case TextJustification.Right:
                                x += diff - textBounds.Left * 2;
                                hangingIndent = -fieldBlock.HangingIndent;
                                break;
                            case TextJustification.Left:
                            case TextJustification.Justified:
                            default:
                                hangingIndent = fieldBlock.HangingIndent;
                                break;
                        }

                        this._skCanvas.DrawText(textLine, x, y, skFont, skPaint);
                        y += lineHeight;
                    }
                }
            }
        }

        private IEnumerable<string> WordWrap(string text, SKFont font, int maxWidth)
        {
            using var tmpPaint = new SKPaint(font);
            var spaceWidth = tmpPaint.MeasureText(" ");
            var lines = new List<string>();

            var words = new Stack<string>(text.Split(new[] { ' ' }, StringSplitOptions.None).Reverse());
            var line = new StringBuilder();
            float width = 0;
            while(words.Any())
            {
                var word = words.Pop();
                if (word.Contains(@"\&"))
                {
                    var subwords = word.Split(new[] { @"\&" }, 2, StringSplitOptions.None);
                    word = subwords[0];
                    words.Push(subwords[1]);
                    var wordWidth = tmpPaint.MeasureText(word);
                    if (width + wordWidth <= maxWidth)
                    {
                        line.Append(word);
                        lines.Add(line.ToString());
                        line = new StringBuilder();
                        width = 0;
                    }
                    else
                    {
                        if (line.Length > 0)
                        {
                            lines.Add(line.ToString().Trim());
                        }
                        lines.Add(word.ToString());
                        line = new StringBuilder();
                        width = 0;
                    }
                }
                else
                {
                    var wordWidth = tmpPaint.MeasureText(word);
                    if (width + wordWidth <= maxWidth)
                    {
                        line.Append(word + " ");
                        width += wordWidth + spaceWidth;
                    }
                    else
                    {
                        if (line.Length > 0)
                        {
                            lines.Add(line.ToString().Trim());
                        }
                        line = new StringBuilder(word + " ");
                        width = wordWidth + spaceWidth;
                    }
                }
            }

            lines.Add(line.ToString().Trim());
            return lines;
        }

    }
}
