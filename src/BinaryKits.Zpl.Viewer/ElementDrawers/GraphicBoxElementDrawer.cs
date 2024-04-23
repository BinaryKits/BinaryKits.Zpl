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
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplGraphicBox graphicBox)
            {
                var border1 = graphicBox.BorderThickness;
                var width1 = graphicBox.Width;
                var height1 = graphicBox.Height;

                if (border1 > width1)
                {
                    width1 = border1;
                }

                if (border1 > height1)
                {
                    height1 = border1;
                }

                //border cant be bigger or equal to width or height
                if (border1 > (width1 / 2) && width1 <= height1)
                {
                    border1 = (int)Math.Ceiling((float)width1 / 2);
                }
                
                if (border1 > (height1 / 2) && height1 <= width1)
                {
                    border1 = (int)Math.Ceiling((float)height1 / 2);
                }

                //make sure border is never smaller than 1
                if (border1 < 1)
                {
                    border1 = 1;
                }

                //if the border is thick, the rounding is off, so we need to build that for each increment
                var lastPrintedBorder = border1;
                for (var border2 = border1; border2 >= 1; border2--)
                {
                    //skip the parts that have overlap from the previous draw
                    if (border2 != 1 && border2 != lastPrintedBorder && border2 > (lastPrintedBorder / 2))
                    {
                        continue;
                    }
                    
                    lastPrintedBorder = border2;

                    var offsetX = border2 / 2.0f;
                    var offsetY = border2 / 2.0f;

                    var x = graphicBox.PositionX + offsetX;
                    var y = graphicBox.PositionY + offsetY;

                    if (graphicBox.FieldTypeset != null)
                    {
                        y -= height1;

                        //Fallback
                        if (y < offsetY)
                        {
                            y = offsetY;
                        }
                    }

                    var width = width1 - border2;
                    var height = height1 - border2;

                    using var skPaint = new SKPaint();
                    skPaint.Style = SKPaintStyle.Stroke;
                    skPaint.StrokeCap = SKStrokeCap.Square;
                    skPaint.Color = SKColors.Black;
                    skPaint.StrokeWidth = border2;

                    if (graphicBox.LineColor == LineColor.White)
                    {
                        skPaint.Color = SKColors.White;
                    }

                    var cornerRadius = (graphicBox.CornerRounding / 8.0f) * (Math.Min(width1, height1) / 2.0f);

                    if (cornerRadius == 0)
                    {
                        if (graphicBox.ReversePrint)
                        {
                            skPaint.BlendMode = SKBlendMode.Xor;
                        }
                        
                        this._skCanvas.DrawRect(x, y, width, height, skPaint);
                        return;
                    }

                    this._skCanvas.DrawRoundRect(x, y, width, height, cornerRadius, cornerRadius, skPaint);
                }
            }
        }
    }
}
