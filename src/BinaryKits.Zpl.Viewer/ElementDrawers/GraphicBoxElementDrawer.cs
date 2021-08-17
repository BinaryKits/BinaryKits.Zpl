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

                var offsetX = border1 / 2.0f;
                var offsetY = border1 / 2.0f;

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

                var width = width1 - border1;
                var height = height1 - border1;

                using var skPaint = new SKPaint();
                skPaint.Style = SKPaintStyle.Stroke;
                skPaint.StrokeCap = SKStrokeCap.Square;
                skPaint.Color = SKColors.Black;
                skPaint.StrokeWidth = border1;

                if (graphicBox.LineColor == Label.LineColor.White)
                {
                    skPaint.Color = SKColors.White;
                }

                var cornerRadius = (graphicBox.CornerRounding / 8.0f) * (Math.Min(width1, height1) / 2.0f);
                if (cornerRadius == 0)
                {
                    this._skCanvas.DrawRect(x, y, width, height, skPaint);
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

                this._skCanvas.DrawRoundRect(x, y, width, height, cornerRadius, cornerRadius, skPaint);
            }
        }
    }
}
