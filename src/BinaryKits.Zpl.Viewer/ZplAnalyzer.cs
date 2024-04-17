using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.CommandAnalyzers;
using BinaryKits.Zpl.Viewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Application.UseCase.ZplToPdf;

namespace BinaryKits.Zpl.Viewer
{
    public class ZplAnalyzer : IZplAnalyzer
    {
        private readonly VirtualPrinter _virtualPrinter;
        private readonly IPrinterStorage _printerStorage;
        private readonly IFormatMerger _formatMerger;
        private readonly string _labelStartCommand = "^XA";
        private readonly string _labelEndCommand = "^XZ";

        public ZplAnalyzer(IPrinterStorage printerStorage, IFormatMerger formatMerger = null)
        {
            this._printerStorage = printerStorage;
            this._formatMerger = formatMerger ?? new FormatMerger();
            this._virtualPrinter = new VirtualPrinter();
        }

        public AnalyzeInfo Analyze(string zplData)
        {
            var zplCommands = this.SplitZplCommands(zplData);
            var unknownCommands = new List<string>();
            var errors = new List<string>();

            var fieldDataAnalyzer = new FieldDataZplCommandAnalyzer(this._virtualPrinter);
            var elementAnalyzers = new List<IZplCommandAnalyzer>
            {
                fieldDataAnalyzer,
                new BarCodeFieldDefaultZplCommandAnalyzer(this._virtualPrinter),
                new ChangeAlphanumericDefaultFontZplCommandAnalyzer(this._virtualPrinter),
                new Code39BarcodeZplCommandAnalyzer(this._virtualPrinter),
                new Code128BarcodeZplCommandAnalyzer(this._virtualPrinter),
                new CodeEAN13BarcodeZplCommandAnalyzer(this._virtualPrinter),
                new CommentZplCommandAnalyzer(this._virtualPrinter),
                new DataMatrixZplCommandAnalyzer(this._virtualPrinter),
                new DownloadFormatCommandAnalyzer(this._virtualPrinter),
                new DownloadGraphicsZplCommandAnalyzer(this._virtualPrinter, this._printerStorage),
                new DownloadObjectsZplCommandAnaylzer(this._virtualPrinter, this._printerStorage),
                new FieldBlockZplCommandAnalyzer(this._virtualPrinter),
                new FieldHexadecimalZplCommandAnalyzer(this._virtualPrinter),
                new FieldNumberCommandAnalyzer(this._virtualPrinter),
                new FieldVariableZplCommandAnalyzer(this._virtualPrinter),
                new FieldReversePrintZplCommandAnalyzer(this._virtualPrinter),
                new LabelReversePrintZplCommandAnalyzer(this._virtualPrinter),
                new FieldSeparatorZplCommandAnalyzer(this._virtualPrinter, fieldDataAnalyzer),
                new FieldTypesetZplCommandAnalyzer(this._virtualPrinter),
                new FieldOriginZplCommandAnalzer(this._virtualPrinter),
                new GraphicBoxZplCommandAnalyzer(this._virtualPrinter),
                new GraphicCircleZplCommandAnalyzer(this._virtualPrinter),
                new GraphicFieldZplCommandAnalyzer(this._virtualPrinter),
                new Interleaved2of5BarcodeZplCommandAnalyzer(this._virtualPrinter),
                new ImageMoveZplCommandAnalyzer(this._virtualPrinter),
                new LabelHomeZplCommandAnalyzer(this._virtualPrinter),
                new MaxiCodeBarcodeZplCommandAnalyzer(this._virtualPrinter),
                new QrCodeBarcodeZplCommandAnalyzer(this._virtualPrinter),
                new PDF417ZplCommandAnalyzer(this._virtualPrinter),
                new RecallFormatCommandAnalyzer(this._virtualPrinter),
                new RecallGraphicZplCommandAnalyzer(this._virtualPrinter),
                new ScalableBitmappedFontZplCommandAnalyzer(this._virtualPrinter),

            };

            var labelInfos = new List<LabelInfo>();

            var elements = new List<ZplElementBase>();
            for (var i = 0; i < zplCommands.Length; i++)
            {
                var currentCommand = zplCommands[i];

                if (this._labelStartCommand.Equals(currentCommand.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    elements.Clear();
                    _virtualPrinter.ClearNextDownloadFormatName();
                    continue;
                }

                if (this._labelEndCommand.Equals(currentCommand.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    labelInfos.Add(new LabelInfo
                    {
                        DownloadFormatName = _virtualPrinter.NextDownloadFormatName,
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

            labelInfos = _formatMerger.MergeFormats(labelInfos);

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
            if (string.IsNullOrWhiteSpace(zplData))
            {
                return Array.Empty<string>();
            }

            var cleanZpl = Regex.Replace(zplData, @"\r|\n", string.Empty);
            char caret = '^';
            char tilde = '~';
            List<string> results = new(200);
            StringBuilder buffer = new(2000);
            for (int i = 0; i < cleanZpl.Length; i++)
            {
                char c = cleanZpl[i];
                if (c == caret || c == tilde)
                {
                    string command = buffer.ToString();
                    buffer.Clear();
                    if (command.Length > 2)
                    {
                        PatchCommand(ref command, ref caret, ref tilde);
                        results.Add(command);
                        if (command.Substring(1, 2) == "CT")
                        {
                            tilde = command[3];
                            results.RemoveAt(results.Count - 1);
                        }
                        else if (command.Substring(1, 2) == "CC")
                        {
                            caret = command[3];
                            results.RemoveAt(results.Count - 1);
                        }
                    }
                    // likely invalid command
                    else if (command.Trim().Length > 0) {
                        results.Add(command.Trim());
                    }
                }
                buffer.Append(c);
            }
            string lastCommand = buffer.ToString();
            if (lastCommand.Length > 0)
            {
                PatchCommand(ref lastCommand, ref caret, ref tilde);
                results.Add(lastCommand);
            }
            return results.ToArray();
        }

        private void PatchCommand(ref string command, ref char caret, ref char tilde)
        {
            if (caret != '^' && command[0] == caret)
            {
                command = '^' + command.Remove(0, 1);
            }
            if (tilde != '~' && command[0] == tilde)
            {
                command = '~' + command.Remove(0, 1);
            }
        }
    }
}
