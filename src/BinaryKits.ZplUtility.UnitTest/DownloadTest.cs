using BinaryKits.ZplUtility.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace BinaryKits.ZplUtility.UnitTest
{
    [TestClass]
    public class DownloadTest
    {
        private void DrawLines(Graphics canvas, List<PointF> points)
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

        private Bitmap GetTestBitmap()
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

        [TestMethod]
        [DeploymentItem(@"ZplData/DownloadGraphics.txt")]
        public void DownloadGraphics()
        {
            using var bitmap = GetTestBitmap();

            var elements = new List<ZplElementBase>
            {
                new ZplGraphicBox(0, 0, 100, 100, 4),
                new ZplDownloadGraphics('R', "SAMPLE", "GRC", bitmap),
                new ZplRecallGraphic(100, 100, 'R', "SAMPLE", "GRC")
            };

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions
            {
                AddEmptyLineBeforeElementStart = true,
                TargetPrintDpi = 200,
                SourcePrintDpi = 200
            });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);

            var zplData = File.ReadAllText("DownloadGraphics.txt");
            Assert.AreEqual(zplData, output);
        }

        [TestMethod]
        [DeploymentItem(@"ZplData/DownloadObject.txt")]
        public void DownloadObjets()
        {
            using var bitmap = GetTestBitmap();

            var elements = new List<ZplElementBase>
            {
                new ZplGraphicBox(0, 0, 100, 100, 4),
                new ZplDownloadObjects('R', "SAMPLE.PNG", bitmap),
                new ZplImageMove(100, 100, 'R', "SAMPLE", "PNG")
            };

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions { AddEmptyLineBeforeElementStart = true, TargetPrintDpi = 300, SourcePrintDpi = 200 });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);

            var zplData = File.ReadAllText("DownloadObject.txt");
            Assert.AreEqual(zplData, output);
        }
    }
}
