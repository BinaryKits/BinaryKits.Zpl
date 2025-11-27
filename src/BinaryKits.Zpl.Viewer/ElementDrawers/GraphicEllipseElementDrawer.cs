using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

using SkiaSharp;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class GraphicEllipseElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplGraphicEllipse;
        }

        public override bool IsReverseDraw(ZplElementBase element)
        {
            if (element is ZplGraphicCircle graphicEllipse)
            {
                return graphicEllipse.ReversePrint;
            }

            return false;
        }

        public override bool IsWhiteDraw(ZplElementBase element)
        {
            if (element is ZplGraphicCircle graphicEllipse)
            {
                return graphicEllipse.LineColor == LineColor.White;
            }

            return false;
        }

        ///<inheritdoc/>
        public override SKPoint Draw(ZplElementBase element, DrawerOptions options, SKPoint currentPosition, InternationalFont internationalFont)
        {
            if (element is ZplGraphicEllipse graphicEllipse)
            {
                float border = graphicEllipse.BorderThickness;
                float width = graphicEllipse.Width;
                float height = graphicEllipse.Height;

                // if either width or height is less than half border, ellipse will be completely filled
                if (width < border * 2)
                {
                    border = width / 2;
                }

                if (height < border * 2)
                {
                    border = height / 2;
                }

                float baseX = graphicEllipse.PositionX;
                float baseY = graphicEllipse.PositionY;

                float x = baseX;
                float y = baseY;

                if (graphicEllipse.UseDefaultPosition)
                {
                    x = currentPosition.X;
                    y = currentPosition.Y;
                }

                if (graphicEllipse.FieldTypeset != null)
                {
                    y -= height;
                    if (y < 0)
                    {
                        y = 0;
                    }
                }

                float halfBorderThickness = border / 2.0f;

                x += halfBorderThickness;
                y += halfBorderThickness;

                SKRect bounds = new(x, y, x + width - border, y + height - border);

                using SKPaint skPaint = new()
                {
                    IsAntialias = options.Antialias,
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.Black,
                    StrokeWidth = border
                };

                this.skCanvas.DrawOval(bounds, skPaint);
                return this.CalculateNextDefaultPosition(x, y, width, height, graphicEllipse.FieldOrigin != null, FieldOrientation.Normal, currentPosition);
            }

            return currentPosition;
        }
    }
}
