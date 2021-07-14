using System.Collections.Generic;
using System.Drawing;

namespace BinaryKits.Zpl.TestConsole
{
    public static class DrawImageHelper
    {
        private static void DrawLines(Graphics canvas, List<PointF> points)
        {
            for (var i = 0; i < points.Count; i++)
            {
                var startPoint = points[i];
                PointF endPoint;

                if (i == points.Count - 1)
                {
                    endPoint = points[0];
                }
                else
                {
                    endPoint = points[i + 1];
                }

                canvas.DrawLine(Pens.Black, startPoint, endPoint);
            }
        }

        public static Bitmap GetTestBitmap()
        {
            #region Points for Z

            var pointsForZ = new List<PointF>
            {
                new PointF(10, 10),
                new PointF(10, 20),
                new PointF(25, 20),
                new PointF(10, 50),
                new PointF(10, 60),
                new PointF(40, 60),
                new PointF(40, 50),
                new PointF(20, 50),
                new PointF(40, 10),
            };

            #endregion

            #region Points for P

            var pointsForP = new List<PointF>
            {
                new PointF(50, 10),
                new PointF(50, 60),
                new PointF(60, 60),
                new PointF(60, 40),
                new PointF(80, 40),
                new PointF(80, 10),
                new PointF(50, 10),
                new PointF(60, 20),
                new PointF(70, 20),
                new PointF(70, 30),
                new PointF(60, 30),
                new PointF(60, 20),
            };

            #endregion

            #region Points for L

            var pointsForL = new List<PointF>
            {
                new PointF(90, 10),
                new PointF(90, 60),
                new PointF(120, 60),
                new PointF(120, 50),
                new PointF(100, 50),
                new PointF(100, 10)
            };

            #endregion

            var bitmap = new Bitmap(130, 70);

            using var canvas = Graphics.FromImage(bitmap);
            canvas.Clear(Color.White);

            DrawLines(canvas, pointsForZ);
            DrawLines(canvas, pointsForP);
            DrawLines(canvas, pointsForL);

            return bitmap;
        }
    }
}
