using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Labelary;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BinaryKits.Zpl.TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var renderActions = new Func<string>[]
            {
                RenderLabel1,
                RenderLabel2,
                RenderLabel3,
                RenderLabel4,
                RenderLabel5,
                RenderLabel6,
            };

            foreach (var renderAction in renderActions)
            {
                Console.WriteLine(renderAction.Method.Name);
                var zplData = renderAction.Invoke();
                await RenderPreviewAsync(zplData);
            }
        }

        static string RenderLabel1()
        {
            var sampleText = "[_~^][LineBreak\n][The quick fox jumps over the lazy dog.]";
            var font = new ZplFont(fontWidth: 50, fontHeight: 50);

            var elements = new ZplElementBase[]
            {
                new ZplTextField(sampleText, 50, 100, font),
                new ZplGraphicBox(400, 700, 100, 100, 5),
                new ZplGraphicBox(200, 700, 100, 200, 3, cornerRounding: 8),
                new ZplGraphicBox(450, 750, 100, 100, 50, LineColor.White),
                new ZplGraphicCircle(400, 700, 100, 5),
                new ZplGraphicDiagonalLine(400, 700, 100, 50, 5),
                new ZplGraphicDiagonalLine(400, 700, 50, 100, 5),
                new ZplGraphicSymbol(GraphicSymbolCharacter.Copyright, 600, 600, 50, 50),

                //Add raw zpl code
                new ZplRaw("^FO200, 200^GB300, 200, 10 ^FS")
            };

            var renderEngine = new ZplEngine(elements);
            return renderEngine.ToZplString(new ZplRenderOptions
            {
                //AddEmptyLineBeforeElementStart = true,
                SourcePrintDpi = 203,
                TargetPrintDpi = 203
            });
        }

        static string RenderLabel2()
        {
            var elements = new ZplElementBase[]
            {
                new ZplReferenceGrid(),
            };

            var renderEngine = new ZplEngine(elements);
            return renderEngine.ToZplString(new ZplRenderOptions
            {
                //AddEmptyLineBeforeElementStart = true,
                SourcePrintDpi = 203,
                TargetPrintDpi = 203
            });
        }

        static string RenderLabel3()
        {
            var elements = new ZplElementBase[]
            {
                new ZplBarcode128("Barcode128", 10, 0),
                new ZplBarcode39("Barcode39", 10, 150),
                new ZplBarcodeAnsiCodabar("123456", 'a', 'd', 10, 300, 100),
                new ZplBarcodeEan13("123456789", 10, 450),
                new ZplBarcodeInterleaved2of5("123456789", 10, 600),
                new ZplQrCode("BinaryKits ZplUtility BinaryKits ZplUtility BinaryKits ZplUtility", 10, 800, magnificationFactor: 6)
            };

            var renderEngine = new ZplEngine(elements);
            return renderEngine.ToZplString(new ZplRenderOptions
            {
                //AddEmptyLineBeforeElementStart = true,
                SourcePrintDpi = 203,
                TargetPrintDpi = 203
            });
        }

        static string RenderLabel4()
        {
            var elements = new ZplElementBase[]
            {
                new ZplDownloadGraphics('R', "TEST", File.ReadAllBytes("logo_sw.png")),
                new ZplRecallGraphic(0, 0, 'R', "TEST")
            };

            var renderEngine = new ZplEngine(elements);
            return renderEngine.ToZplString(new ZplRenderOptions
            {
                //AddEmptyLineBeforeElementStart = true,
                SourcePrintDpi = 203,
                TargetPrintDpi = 203
            });
        }

        static string RenderLabel5()
        {
            var font1 = new ZplFont(fontWidth: 0, fontHeight: 50, fontName: "0");
            var font2 = new ZplFont(fontWidth: 0, fontHeight: 50, fontName: "1");
            var font3 = new ZplFont(fontWidth: 0, fontHeight: 80, fontName: "A");
            var font4 = new ZplFont(fontWidth: 0, fontHeight: 50, fontName: "B");
            var font5 = new ZplFont(fontWidth: 0, fontHeight: 20, fontName: "C");
            var font6 = new ZplFont(fontWidth: 20, fontHeight: 0, fontName: "D");
            var font7 = new ZplFont(fontWidth: 20, fontHeight: 20, fontName: "D");
            var font8 = new ZplFont(fontWidth: 20, fontHeight: 0, fontName: "D", fieldOrientation: FieldOrientation.Rotated90);

            var elements = new ZplElementBase[]
            {
                new ZplTextField("Font1 Demo Text", 10, 0, font1),
                new ZplTextField("Font2 Demo Text", 10, 100, font2),
                new ZplTextField("Font3 Demo Text", 10, 200, font3),
                new ZplTextField("Font4 Demo Text", 10, 300, font4),
                new ZplTextField("Font5 Demo Text", 10, 400, font5),
                new ZplTextField("Font6 Demo Text", 10, 500, font6),
                new ZplTextField("Font7 Demo Text", 10, 600, font7),
                new ZplTextField("Font8 Demo Text", 900, 10, font8),
            };

            var renderEngine = new ZplEngine(elements);
            return renderEngine.ToZplString(new ZplRenderOptions
            {
                //AddEmptyLineBeforeElementStart = true,
                SourcePrintDpi = 203,
                TargetPrintDpi = 203
            });
        }

        static string RenderLabel6()
        {
            var text = "Lorem Ipsum is simply dummy text\nof the printing and typesetting industry.\nLorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book";
            var font1 = new ZplFont(fontWidth: 0, fontHeight: 20, fontName: "0");
            var font2 = new ZplFont(fontWidth: 0, fontHeight: 13, fontName: "0");

            var elements = new ZplElementBase[]
            {
                new ZplTextBlock(text, 10, 0, 400, 100, font1, NewLineConversionMethod.ToSpace),
                new ZplTextBlock(text, 10, 120, 400, 100, font1, NewLineConversionMethod.ToSpace),

                new ZplTextBlock(text, 10, 240, 400, 100, font2, NewLineConversionMethod.ToEmpty),
                
                new ZplFieldBlock(text, 10, 360, 400, font1, 4)
            };

            var renderEngine = new ZplEngine(elements);
            return renderEngine.ToZplString(new ZplRenderOptions
            {
                //AddEmptyLineBeforeElementStart = true,
                SourcePrintDpi = 203,
                TargetPrintDpi = 203
            });
        }

        static async Task RenderPreviewAsync(string zplData)
        {
            var client = new LabelaryClient();
            var previewData = await client.GetPreviewAsync(zplData, PrintDensity.PD8dpmm, new LabelSize(6, 8, Measure.Inch));
            if (previewData.Length == 0)
            {
                return;
            }

            var fileName = $"preview-{Guid.NewGuid()}.png";
            await File.WriteAllBytesAsync(fileName, previewData);

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
