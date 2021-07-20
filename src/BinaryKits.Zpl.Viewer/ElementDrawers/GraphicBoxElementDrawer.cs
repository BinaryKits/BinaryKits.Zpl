using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class GraphicBoxElementDrawer : ElementDrawerBase
    {
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplGraphicBox;
        }

        public override void Draw(ZplElementBase element)
        {
            if (element is ZplGraphicBox graphicBox)
            {
                var borderThickness = graphicBox.BorderThickness;
                if (borderThickness > graphicBox.Width / 2)
                {
                    borderThickness = graphicBox.Width / 2;
                }

                this._skPaint.StrokeWidth = borderThickness;

                var offset = borderThickness / 2;

                var x = graphicBox.Origin.PositionX + this._padding + offset;
                var y = graphicBox.Origin.PositionY + this._padding + offset;
                var width = graphicBox.Width - offset;
                var height = graphicBox.Height - offset;

                this._skCanvas.DrawRect(x, y, width, height, this._skPaint);
            }
        }
    }
}
