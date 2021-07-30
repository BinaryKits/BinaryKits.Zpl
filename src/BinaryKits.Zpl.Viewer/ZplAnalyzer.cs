using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.CommandAnalyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer
{
    public class ZplAnalyzer
    {
        private readonly VirtualPrinter _virtualPrinter;
        private readonly IPrinterStorage _printerStorage;

        public ZplAnalyzer(IPrinterStorage printerStorage)
        {
            this._printerStorage = printerStorage;
            this._virtualPrinter = new VirtualPrinter();
        }

        public ZplElementBase[] Analyze(string zplData)
        {
            var zplCommands = this.SplitZplCommands(zplData);
            var unknownCommands = new List<string>();

            var elementAnalyzers = new List<IZplCommandAnalyzer>
            {
                new LabelHomeZplCommandAnalyzer(this._virtualPrinter),
                new ChangeAlphanumericDefaultFontZplCommandAnalyzer(this._virtualPrinter),
                new BarCodeFieldDefaultZplCommandAnalyzer(this._virtualPrinter),
                new ScalableBitmappedFontZplCommandAnalyzer(this._virtualPrinter),
                new FieldOriginZplCommandAnalzer(this._virtualPrinter),
                new FieldTypesetZplCommandAnalyzer(this._virtualPrinter),
                new FieldReversePrintZplCommandAnalyzer(this._virtualPrinter),
                new FieldDataZplCommandAnalyzer(this._virtualPrinter),
                new GraphicBoxZplCommandAnalyzer(this._virtualPrinter),
                new GraphicCircleZplCommandAnalyzer(this._virtualPrinter),
                new DownloadObjectsZplCommandAnaylzer(this._virtualPrinter, this._printerStorage),
                new DownloadGraphicsZplCommandAnalyzer(this._virtualPrinter, this._printerStorage),
                new RecallGraphicZplCommandAnalyzer(this._virtualPrinter),
                new ImageMoveZplCommandAnalyzer(this._virtualPrinter),
                new Code39BarcodeZplCommandAnalyzer(this._virtualPrinter),
                new Code128BarcodeZplCommandAnalyzer(this._virtualPrinter),
                new QrCodeBarcodeZplCommandAnalyzer(this._virtualPrinter),
                new FieldSeparatorZplCommandAnalyzer(this._virtualPrinter)
            };

            var elements = new List<ZplElementBase>();
            for (var i = 0; i < zplCommands.Length; i++)
            {
                var currentCommand = zplCommands[i];
                var validAnalyzers = elementAnalyzers.Where(o => o.CanAnalyze(currentCommand));

                if (!validAnalyzers.Any())
                {
                    unknownCommands.Add(currentCommand);
                    continue;
                }

                elements.AddRange(validAnalyzers.Select(analyzer => analyzer.Analyze(currentCommand)).Where(o => o != null));
            }

            return elements.ToArray();
        }

        private string[] SplitZplCommands(string zplData)
        {
            if (string.IsNullOrEmpty(zplData))
            {
                return Array.Empty<string>();
            }

            var replacementString = string.Empty;
            var cleanZpl = Regex.Replace(zplData, @"\r\n?|\n", replacementString);
            return Regex.Split(cleanZpl, "(?=\\^)|(?=\\~)").Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }
    }
}
