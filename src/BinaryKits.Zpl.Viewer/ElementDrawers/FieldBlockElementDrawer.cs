using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;

using SkiaSharp;
using SkiaSharp.HarfBuzz;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for Field Block elements
    /// </summary>
    public class FieldBlockElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplFieldBlock;
        }

        ///<inheritdoc/>
        public override bool IsReverseDraw(ZplElementBase element)
        {
            if (element is ZplFieldBlock fieldBlock)
            {
                return fieldBlock.ReversePrint;
            }

            return false;
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont, int printDensityDpmm)
        {
            if (element is ZplFieldBlock fieldBlock)
            {
                ZplFont font = fieldBlock.Font;

                (float fontSize, float scaleX) = FontScale.GetFontScaling(font.FontName, font.FontHeight, font.FontWidth, printDensityDpmm);

                SKTypeface typeface = options.FontLoader(font.FontName);
                string text = fieldBlock.Text;
                if (fieldBlock.HexadecimalIndicator is char hexIndicator)
                {
                    text = text.ReplaceHexEscapes(hexIndicator, internationalFont);
                }

                if (options.ReplaceDashWithEnDash)
                {
                    text = text.Replace("-", " \u2013 ");
                }

                if (options.ReplaceUnderscoreWithEnSpace)
                {
                    text = text.Replace('_', '\u2002');
                }

                SKFont skFont = new(typeface, fontSize, scaleX);
                using SKPaint skPaint = new()
                {
                    IsAntialias = options.Antialias
                };

                skFont.MeasureText("X", out SKRect textBoundBaseline);

                float x = fieldBlock.PositionX;
                float y = fieldBlock.PositionY + textBoundBaseline.Height;

                if (fieldBlock.UseDefaultPosition)
                {
                    x = currentPosition.X;
                    y = currentPosition.Y + textBoundBaseline.Height;
                }

                IEnumerable<string> textLines = WordWrap(text, skFont, fieldBlock.Width);
                int hangingIndent = 0;
                float lineHeight = fontSize + fieldBlock.LineSpace;

                // actual ZPL printer does not include trailing line spacing in total height
                float totalHeight = lineHeight * fieldBlock.MaxLineCount - fieldBlock.LineSpace;
                // labelary
                //var totalHeight = lineHeight * fieldBlock.MaxLineCount;

                if (fieldBlock.FieldTypeset != null)
                {
                    totalHeight = lineHeight * (fieldBlock.MaxLineCount - 1) + textBoundBaseline.Height;
                    y -= totalHeight;
                }

                using (new SKAutoCanvasRestore(this.skCanvas))
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
                        SKMatrix currentMatrix = this.skCanvas.TotalMatrix;
                        SKMatrix concatMatrix = SKMatrix.Concat(currentMatrix, matrix);
                        this.skCanvas.SetMatrix(concatMatrix);
                    }

                    foreach (string textLine in textLines)
                    {
                        x = fieldBlock.PositionX + hangingIndent;

                        skFont.MeasureText(textLine, out SKRect textBounds);
                        float diff = fieldBlock.Width - textBounds.Width;

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

                        if (fieldBlock.ReversePrint)
                        {
                            skPaint.BlendMode = SKBlendMode.Xor;
                        }

                        this.skCanvas.DrawShapedText(textLine, x, y, skFont, skPaint);
                        y += lineHeight;
                    }

                    return this.CalculateNextDefaultPosition(fieldBlock.PositionX, fieldBlock.PositionY, fieldBlock.Width, totalHeight, fieldBlock.FieldOrigin != null, fieldBlock.Font.FieldOrientation, currentPosition);
                }
            }

            return currentPosition;
        }

        private static List<string> WordWrap(string text, SKFont font, int maxWidth)
        {
            float spaceWidth = font.MeasureText(" ");
            List<string> lines = [];

            Stack<string> words = new(text.Split([' '], StringSplitOptions.None).AsEnumerable().Reverse());
            StringBuilder line = new();
            float width = 0;
            while (words.Count != 0)
            {
                string word = words.Pop();
                if (word.Contains(@"\&"))
                {
                    string[] subwords = word.Split([@"\&"], 2, StringSplitOptions.None);
                    word = subwords[0];
                    words.Push(subwords[1]);
                    float wordWidth = font.MeasureText(word);
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
                    float wordWidth = font.MeasureText(word);
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
