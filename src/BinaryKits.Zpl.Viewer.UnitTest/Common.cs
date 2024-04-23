using System.IO;
using BinaryKits.Zpl.Viewer.ElementDrawers;

namespace BinaryKits.Zpl.Viewer.UnitTest
{
    internal class Common
    {
        /// <summary>
        /// Load zpl strings from files in the Data/Zpl directory
        /// </summary>
        /// <param name="name">name of of the file, .zpl2 extension optional</param>
        /// <returns></returns>
        public static string LoadZPL(string name) {
            if (!name.Contains(".zpl2"))
            {
                name += ".zpl2"; ;
            }
            var path = Path.Combine("Data", "Zpl", name);
            return System.IO.File.ReadAllText(path);
        }

        /// <summary>
        /// Generic printer to test zpl -> png output
        /// </summary>
        /// <param name="zpl"></param>
        /// <param name="outputFilename">PNG filename ex: "file.png"</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="ppmm"></param>
        /// <param name="options"></param>
        public static void DefaultPrint(
            string zpl,
            string outputFilename,
            double width = 101.6,
            double height = 152.4,
            int ppmm = 8,
            DrawerOptions options = null
            )
        {
            IPrinterStorage printerStorage = new PrinterStorage();
            var drawer = new ZplElementDrawer(printerStorage, options);

            var analyzer = new ZplAnalyzer(printerStorage);
            var analyzeInfo = analyzer.Analyze(zpl);

            foreach (var labelInfo in analyzeInfo.LabelInfos)
            {
                var imageData = drawer.Draw(labelInfo.ZplElements, width, height, ppmm);
                File.WriteAllBytes(outputFilename, imageData);
            }
        }
    }
}
