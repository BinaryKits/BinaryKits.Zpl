using BinaryKits.ZPLUtility.Elements;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BinaryKits.ZPLUtility.TestConsole.cs
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sampleText = "[_~^][LineBreak\n][The quick fox jumps over the lazy dog.]";
            var font = new ZPLFont(fontWidth: 50, fontHeight: 50);

            var labelElements = new List<ZPLElementBase>
            {
                new ZPLRaw("^POI"), //Invert
                new ZPLTextField(sampleText, 50, 100, font),
                new ZPLGraphicBox(400, 700, 100, 100, 5),
                new ZPLGraphicBox(450, 750, 100, 100, 50, ZPLConstants.LineColor.White),
                new ZPLGraphicCircle(400, 700, 100, 5),
                new ZPLGraphicDiagonalLine(400, 700, 100, 50, 5),
                new ZPLGraphicDiagonalLine(400, 700, 50, 100, 5),
                new ZPLGraphicSymbol(GraphicSymbolCharacter.Copyright, 600, 600, 50, 50),

                //Add raw ZPL code
                new ZPLRaw("^FO200, 200^GB300, 200, 10 ^FS")
            };

            var renderEngine = new ZPLEngine(labelElements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions
            {
                //AddEmptyLineBeforeElementStart = true,
                SourcePrintDPI = 203,
                TargetPrintDPI = 203
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
