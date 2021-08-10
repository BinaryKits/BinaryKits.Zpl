using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class GraphicCircleElementDrawer : ElementDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplGraphicCircle;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplGraphicCircle graphicCircle)
            {
                var radius = graphicCircle.Diameter / 2.0f;
                var border = (float)graphicCircle.BorderThickness;

                if (border > radius)
                {
                    border = radius;
                }

                this._skPaint.StrokeWidth = border;

                var halfBorderThickness = border / 2.0f;

                var radiusMinusBorder = radius - halfBorderThickness;
                var offset = halfBorderThickness + radiusMinusBorder;

                var x = graphicCircle.PositionX + offset + this._padding;
                var y = graphicCircle.PositionY + offset + this._padding;

                if (graphicCircle.FieldTypeset != null)
                {
                    y -= graphicCircle.Diameter;
                    if (y < radius)
                    {
                        y = radius;
                    }
                }

                this._skCanvas.DrawCircle(x, y, radiusMinusBorder, this._skPaint);
            }
        }
    }
}
