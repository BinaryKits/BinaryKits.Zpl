using BinaryKits.Zpl.Label.Elements;
using System;

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

                this._skPaint.StrokeWidth = border1;

                var offsetX = border1 / 2.0f;
                var offsetY = border1 / 2.0f;

                var x = graphicBox.Origin.PositionX + this._padding + offsetX;
                var y = graphicBox.Origin.PositionY + this._padding + offsetY;
                var width = width1 - border1;
                var height = height1 - border1;

                var cornerRadius = (graphicBox.CornerRounding / 8.0f) * (Math.Min(width1, height1) / 2.0f);
                if (cornerRadius == 0)
                {
                    this._skCanvas.DrawRect(x, y, width, height, this._skPaint);
                    return;
                }

                //using var test = new SkiaSharp.SKRoundRect(new SkiaSharp.SKRect(x, y, width + x, height + y), cornerRadius);
                //test.Deflate(5,5);
                //test.Inflate(10, 10);
                //test.Offset(offsetX, offsetY);
                //test.SetNinePatch(new SkiaSharp.SKRect(x + 1, y, width + x, height + y), 40, 3,4, 5);
                //test.SetOval(new SkiaSharp.SKRect(x - 50, y-10, width + x -10, height + y - 10));
                //this._skCanvas.DrawRoundRect(test, this._skPaint);

                //TODO:Some corner radius is too much
                //^XA^FO50,50^GB100,100,120,B,1^FS^XZ

                this._skCanvas.DrawRoundRect(x, y, width, height, cornerRadius, cornerRadius, this._skPaint);
            }
        }
    }
}
