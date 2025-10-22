using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;
using System;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class GraphicBoxElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplGraphicBox;
        }

        public override bool IsReverseDraw(ZplElementBase element)
        {
            if (element is ZplGraphicBox graphicBox)
            {
                return graphicBox.ReversePrint;
            }

            return false;
        }

        public override bool IsWhiteDraw(ZplElementBase element)
        {
            if (element is ZplGraphicBox graphicBox)
            {
                return graphicBox.LineColor == LineColor.White;
            }

            return false;
        }

        public override bool ForceBitmapDraw(ZplElementBase element)
        {
            if (element is ZplGraphicBox graphicBox && graphicBox.CornerRounding > 0)
            {
                return true;
            }

            return false;
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont)
        {
            if (element is ZplGraphicBox graphicBox)
            {
                int border1 = graphicBox.BorderThickness;
                int width1 = graphicBox.Width;
                int height1 = graphicBox.Height;

                if (border1 > width1)
                {
                    width1 = border1;
                }

                if (border1 > height1)
                {
                    height1 = border1;
                }

                //border cant be bigger or equal to width or height
                if (border1 > width1 / 2 && width1 <= height1)
                {
                    border1 = (int)Math.Ceiling((float)width1 / 2);
                }
                
                if (border1 > height1 / 2 && height1 <= width1)
                {
                    border1 = (int)Math.Ceiling((float)height1 / 2);
                }

                //make sure border is never smaller than 1
                if (border1 < 1)
                {
                    border1 = 1;
                }

                float baseX = graphicBox.PositionX;
                float baseY = graphicBox.PositionY;

                if (graphicBox.UseDefaultPosition)
                {
                    baseX = currentPosition.X;
                    baseY = currentPosition.Y;
                }

                //if the border is thick, the rounding is off, so we need to build that for each increment
                int lastPrintedBorder = border1;
                for (int border2 = border1; border2 >= 1; border2--)
                {
                    //skip the parts that have overlap from the previous draw
                    if (border2 != 1 && border2 != lastPrintedBorder && border2 > lastPrintedBorder / 2)
                    {
                        continue;
                    }
                    
                    lastPrintedBorder = border2;

                    float offsetX = border2 / 2.0f;
                    float offsetY = border2 / 2.0f;

                    float x = baseX + offsetX;
                    float y = baseY + offsetY;

                    if (graphicBox.FieldTypeset != null)
                    {
                        y -= height1;

                        //Fallback
                        if (y < offsetY)
                        {
                            y = offsetY;
                        }
                    }

                    int width = width1 - border2;
                    int height = height1 - border2;

                    using SKPaint skPaint = new()
                    {
                        IsAntialias = options.Antialias,
                        Style = SKPaintStyle.Stroke,
                        StrokeCap = SKStrokeCap.Square,
                        Color = SKColors.Black,
                        StrokeWidth = border2
                    };

                    if (graphicBox.LineColor == LineColor.White)
                    {
                        skPaint.Color = SKColors.White;
                    }

                    float cornerRadius = (graphicBox.CornerRounding / 8.0f) * (Math.Min(width1, height1) / 2.0f);

                    if (cornerRadius == 0)
                    {
                        if (graphicBox.ReversePrint)
                        {
                            skPaint.BlendMode = SKBlendMode.Xor;
                        }
                        
                        this.skCanvas.DrawRect(x, y, width, height, skPaint);
                        // Calculate next position based on box dimensions
                        return this.CalculateNextDefaultPosition(baseX, baseY, width1, height1, graphicBox.FieldOrigin != null, FieldOrientation.Normal, currentPosition);
                    }

                    this.skCanvas.DrawRoundRect(x, y, width, height, cornerRadius, cornerRadius, skPaint);
                }

                // Calculate next position based on box dimensions
                return this.CalculateNextDefaultPosition(baseX, baseY, width1, height1, graphicBox.FieldOrigin != null, FieldOrientation.Normal, currentPosition);
            }
            
            return currentPosition;
        }
    }
}
