using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.CommandAnalyzers;
using BinaryKits.Zpl.Viewer.Models;
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
        private readonly string _labelStartCommand = "^XA";
        private readonly string _labelEndCommand = "^XZ";

        public ZplAnalyzer(IPrinterStorage printerStorage)
        {
            this._printerStorage = printerStorage;
            this._virtualPrinter = new VirtualPrinter();
        }

        public AnalyzeInfo Analyze(string zplData)
        {
            var zplCommands = this.SplitZplCommands(zplData);
            var unknownCommands = new List<string>();
            var errors = new List<string>();

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
                new GraphicFieldZplCommandAnalyzer(this._virtualPrinter),
                new DownloadObjectsZplCommandAnaylzer(this._virtualPrinter, this._printerStorage),
                new DownloadGraphicsZplCommandAnalyzer(this._virtualPrinter, this._printerStorage),
                new RecallGraphicZplCommandAnalyzer(this._virtualPrinter),
                new ImageMoveZplCommandAnalyzer(this._virtualPrinter),
                new Code39BarcodeZplCommandAnalyzer(this._virtualPrinter),
                new Code128BarcodeZplCommandAnalyzer(this._virtualPrinter),
                new QrCodeBarcodeZplCommandAnalyzer(this._virtualPrinter),
                new FieldSeparatorZplCommandAnalyzer(this._virtualPrinter)
            };

            var labelInfos = new List<LabelInfo>();

            var elements = new List<ZplElementBase>();
            for (var i = 0; i < zplCommands.Length; i++)
            {
                var currentCommand = zplCommands[i];

                if (this._labelStartCommand.Equals(currentCommand, StringComparison.OrdinalIgnoreCase))
                {
                    elements.Clear();
                    continue;
                }

                if (this._labelEndCommand.Equals(currentCommand, StringComparison.OrdinalIgnoreCase))
                {
                    labelInfos.Add(new LabelInfo
                    {
                        ZplElements = elements.ToArray()
                    });
                    continue;
                }

                var validAnalyzers = elementAnalyzers.Where(o => o.CanAnalyze(currentCommand));

                if (!validAnalyzers.Any())
                {
                    unknownCommands.Add(currentCommand);
                    continue;
                }

                try
                {
                    elements.AddRange(validAnalyzers.Select(analyzer => analyzer.Analyze(currentCommand)).Where(o => o != null));
                }
                catch (Exception exception)
                {
                    errors.Add($"Cannot analyze command {currentCommand} {exception}");
                }
            }

            var analyzeInfo = new AnalyzeInfo
            {
                LabelInfos = labelInfos.ToArray(),
                UnknownCommands = unknownCommands.ToArray(),
                Errors = errors.ToArray()
            };

            return analyzeInfo;
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
