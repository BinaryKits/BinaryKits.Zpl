using BinaryKits.Zpl.Viewer.ElementDrawers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Viewer.UnitTest
{
    [TestClass]
    public class ReversePrintPdfTest
    {
        private static byte[] RenderPdf(string zplFixture, bool opaqueBackground)
        {
            string zpl = Common.LoadZPL(zplFixture);

            var options = new DrawerOptions
            {
                PdfOutput = true,
                OpaqueBackground = opaqueBackground
            };

            IPrinterStorage printerStorage = new PrinterStorage();
            var drawer = new ZplElementDrawer(printerStorage, options);
            var analyzer = new ZplAnalyzer(printerStorage);
            var analyzeInfo = analyzer.Analyze(zpl);

            var labelInfo = analyzeInfo.LabelInfos[0];
            return drawer.DrawPdf(labelInfo.ZplElements, 100, 50, 8);
        }

        [TestMethod]
        public void Pdf_ReversePrint_Opaque_IsVectorWithDifferenceBlend()
        {
            byte[] pdf = RenderPdf("reverse_print_demo", opaqueBackground: true);

            int imageCount = PdfContentAnalyzer.CountImageXObjects(pdf);
            bool hasDifference = PdfContentAnalyzer.ContainsBlendMode(pdf, "Difference");

            Assert.AreEqual(0, imageCount, "Opaque reverse-print PDF must contain no raster image XObjects.");
            Assert.IsTrue(hasDifference, "Opaque reverse-print PDF must use the Difference blend mode.");
        }

        [TestMethod]
        public void Pdf_ReversePrint_Transparent_StillUsesLegacyRaster()
        {
            byte[] pdf = RenderPdf("reverse_print_demo", opaqueBackground: false);

            int imageCount = PdfContentAnalyzer.CountImageXObjects(pdf);

            Assert.IsTrue(imageCount > 0, "Transparent (legacy) path is expected to embed a raster image - this guards that the default behavior is unchanged.");
        }
    }
}
