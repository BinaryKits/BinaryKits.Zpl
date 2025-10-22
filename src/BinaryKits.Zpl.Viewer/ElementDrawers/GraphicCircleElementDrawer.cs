using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class GraphicCircleElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplGraphicCircle;
        }
        
        public override bool IsReverseDraw(ZplElementBase element)
        {
            if (element is ZplGraphicCircle graphicCircle)
            {
                return graphicCircle.ReversePrint;
            }

            return false;
        }
        
        public override bool IsWhiteDraw(ZplElementBase element)
        {
            if (element is ZplGraphicCircle graphicCircle)
            {
                return graphicCircle.LineColor == LineColor.White;
            }

            return false;
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont)
        {
            if (element is ZplGraphicCircle graphicCircle)
            {
                float radius = graphicCircle.Diameter / 2.0f;
                float border = graphicCircle.BorderThickness;

                if (border > radius)
                {
                    border = radius;
                }

                using SKPaint skPaint = new()
                {
                    IsAntialias = options.Antialias,
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.Black,
                    StrokeWidth = border
                };
                if (graphicCircle.LineColor == LineColor.White)
                {
                    skPaint.Color = SKColors.White;
                }

                float halfBorderThickness = border / 2.0f;

                float radiusMinusBorder = radius - halfBorderThickness;
                float offset = halfBorderThickness + radiusMinusBorder;

                float baseX = graphicCircle.PositionX;
                float baseY = graphicCircle.PositionY;

                if (graphicCircle.UseDefaultPosition)
                {
                    baseX = currentPosition.X;
                    baseY = currentPosition.Y;
                }

                float x = baseX + offset;
                float y = baseY + offset;

                if (graphicCircle.FieldTypeset != null)
                {
                    y -= graphicCircle.Diameter;

                    //Fallback
                    if (y < radius)
                    {
                        y = radius;
                    }
                }
                
                if (graphicCircle.ReversePrint)
                {
                    skPaint.BlendMode = SKBlendMode.Xor;
                }

                this.skCanvas.DrawCircle(x, y, radiusMinusBorder, skPaint);
                return this.CalculateNextDefaultPosition(baseX, baseY, graphicCircle.Diameter, graphicCircle.Diameter, graphicCircle.FieldOrigin != null, FieldOrientation.Normal, currentPosition);
            }
            
            return currentPosition;
        }
    }
}
