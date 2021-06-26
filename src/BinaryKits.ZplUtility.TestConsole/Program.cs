using BinaryKits.ZplUtility.Elements;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BinaryKits.ZplUtility.TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sampleText = "[_~^][LineBreak\n][The quick fox jumps over the lazy dog.]";
            var font = new ZplFont(fontWidth: 50, fontHeight: 50);

            var labelElements = new List<ZplElementBase>
            {
                //new ZplRaw("^POI"), //Invert
                new ZplTextField(sampleText, 50, 100, font),
                new ZplGraphicBox(400, 700, 100, 100, 5),
                new ZplGraphicBox(450, 750, 100, 100, 50, ZplConstants.LineColor.White),
                new ZplGraphicCircle(400, 700, 100, 5),
                new ZplGraphicDiagonalLine(400, 700, 100, 50, 5),
                new ZplGraphicDiagonalLine(400, 700, 50, 100, 5),
                new ZplGraphicSymbol(GraphicSymbolCharacter.Copyright, 600, 600, 50, 50),

                //Add raw zpl code
                new ZplRaw("^FO200, 200^GB300, 200, 10 ^FS")
            };

            var renderEngine = new ZplEngine(labelElements);
            var output = renderEngine.ToZplString(new ZplRenderOptions
            {
                //AddEmptyLineBeforeElementStart = true,
                SourcePrintDpi = 203,
                TargetPrintDpi = 203
            });

            var client = new LabelaryApiClient();
            var previewData = await client.GetPreviewAsync(output);

            var fileName = "preview.png";
            File.WriteAllBytes(fileName, previewData);

            var processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                UseShellExecute = true,
                CreateNoWindow = true,
                Verb = string.Empty
            };

            Process.Start(processStartInfo);
        }
    }
}
