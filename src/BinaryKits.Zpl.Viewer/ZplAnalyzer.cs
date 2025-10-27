using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.CommandAnalyzers;
using BinaryKits.Zpl.Viewer.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer
{
    public class ZplAnalyzer : IZplAnalyzer
    {
        private static readonly Regex verticalWhitespaceRegex = new(@"[\n\v\f\r]", RegexOptions.Compiled);

        private readonly VirtualPrinter virtualPrinter;
        private readonly IPrinterStorage printerStorage;
        private readonly IFormatMerger formatMerger;
        private readonly string labelStartCommand = "^XA";
        private readonly string labelEndCommand = "^XZ";

        public ZplAnalyzer(IPrinterStorage printerStorage, IFormatMerger formatMerger = null)
        {
            this.printerStorage = printerStorage;
            this.formatMerger = formatMerger ?? new FormatMerger();
            this.virtualPrinter = new VirtualPrinter();
        }

        public AnalyzeInfo Analyze(string zplData)
        {
            string[] zplCommands = SplitZplCommands(zplData);
            List<string> unknownCommands = [];
            List<string> errors = [];

            FieldDataZplCommandAnalyzer fieldDataAnalyzer = new(this.virtualPrinter);
            List<IZplCommandAnalyzer> elementAnalyzers = [
                fieldDataAnalyzer,
                new AztecBarcodeZplCommandAnalyzer(this.virtualPrinter),
                new BarCodeFieldDefaultZplCommandAnalyzer(this.virtualPrinter),
                new ChangeAlphanumericDefaultFontZplCommandAnalyzer(this.virtualPrinter),
                new ChangeInternationalFontCommandAnalyzer(this.virtualPrinter),
                new Code39BarcodeZplCommandAnalyzer(this.virtualPrinter),
                new Code93BarcodeZplCommandAnalyzer(this.virtualPrinter),
                new Code128BarcodeZplCommandAnalyzer(this.virtualPrinter),
                new CodeEAN13BarcodeZplCommandAnalyzer(this.virtualPrinter),
                new CommentZplCommandAnalyzer(this.virtualPrinter),
                new DataMatrixZplCommandAnalyzer(this.virtualPrinter),
                new DownloadFormatCommandAnalyzer(this.virtualPrinter),
                new DownloadGraphicsZplCommandAnalyzer(this.virtualPrinter, this.printerStorage),
                new DownloadObjectsZplCommandAnaylzer(this.virtualPrinter, this.printerStorage),
                new FieldBlockZplCommandAnalyzer(this.virtualPrinter),
                new FieldHexadecimalZplCommandAnalyzer(this.virtualPrinter),
                new FieldOrientationZplCommandAnalyzer(this.virtualPrinter),
                new FieldNumberCommandAnalyzer(this.virtualPrinter),
                new FieldVariableZplCommandAnalyzer(this.virtualPrinter),
                new FieldReversePrintZplCommandAnalyzer(this.virtualPrinter),
                new LabelReversePrintZplCommandAnalyzer(this.virtualPrinter),
                new FieldSeparatorZplCommandAnalyzer(this.virtualPrinter, fieldDataAnalyzer),
                new FieldTypesetZplCommandAnalyzer(this.virtualPrinter),
                new FieldOriginZplCommandAnalzer(this.virtualPrinter),
                new GraphicBoxZplCommandAnalyzer(this.virtualPrinter),
                new GraphicCircleZplCommandAnalyzer(this.virtualPrinter),
                new GraphicFieldZplCommandAnalyzer(this.virtualPrinter),
                new Interleaved2of5BarcodeZplCommandAnalyzer(this.virtualPrinter),
                new ImageMoveZplCommandAnalyzer(this.virtualPrinter),
                new LabelHomeZplCommandAnalyzer(this.virtualPrinter),
                new MaxiCodeBarcodeZplCommandAnalyzer(this.virtualPrinter),
                new QrCodeBarcodeZplCommandAnalyzer(this.virtualPrinter),
                new UpcABarcodeZplCommandAnalyzer(this.virtualPrinter),
                new UpcEBarcodeZplCommandAnalyzer(this.virtualPrinter),
                new PDF417ZplCommandAnalyzer(this.virtualPrinter),
                new RecallFormatCommandAnalyzer(this.virtualPrinter),
                new RecallGraphicZplCommandAnalyzer(this.virtualPrinter),
                new ScalableBitmappedFontZplCommandAnalyzer(this.virtualPrinter),
                new AnsiCodabarBarcodeZplCommandAnalyzer(this.virtualPrinter),
            ];

            List<LabelInfo> labelInfos = [];

            List<ZplElementBase> elements = [];
            for (int i = 0; i < zplCommands.Length; i++)
            {
                string currentCommand = zplCommands[i];

                if (this.labelStartCommand.Equals(currentCommand.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    elements.Clear();
                    this.virtualPrinter.ClearNextDownloadFormatName();
                    continue;
                }

                if (this.labelEndCommand.Equals(currentCommand.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    labelInfos.Add(new LabelInfo
                    {
                        DownloadFormatName = this.virtualPrinter.NextDownloadFormatName,
                        ZplElements = elements.ToArray()
                    });
                    continue;
                }

                IEnumerable<IZplCommandAnalyzer> validAnalyzers = elementAnalyzers.Where(o => o.CanAnalyze(currentCommand));

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

            labelInfos = this.formatMerger.MergeFormats(labelInfos);

            AnalyzeInfo analyzeInfo = new()
            {
                LabelInfos = labelInfos.ToArray(),
                UnknownCommands = unknownCommands.ToArray(),
                Errors = errors.ToArray()
            };

            return analyzeInfo;
        }

        // When adding new commands: 1 per line, always upper case, comment why if possible
        private static readonly HashSet<string> ignoredCommands = new(StringComparer.OrdinalIgnoreCase)
        {
        };

        private static string[] SplitZplCommands(string zplData)
        {
            if (string.IsNullOrWhiteSpace(zplData))
            {
                return [];
            }

            string cleanZpl = verticalWhitespaceRegex.Replace(zplData, string.Empty);
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

                    // all commands have at least 3 chars, even ^A because of required font parameter
                    if (command.Length > 2)
                    {
                        PatchCommand(ref command, caret, tilde);

                        string commandLetters = command.Substring(1, 2).ToUpper();

                        if (commandLetters == "CT")
                        {
                            tilde = command.Length > 3 ? command[3] : c;
                        }
                        else if (commandLetters == "CC")
                        {
                            caret = command.Length > 3 ? command[3] : c;
                        }
                        else if (!ignoredCommands.Contains(commandLetters))
                        {
                            results.Add(command);
                        }
                    }
                    // likely invalid command
                    else if (command.Trim().Length > 0)
                    {
                        results.Add(command.Trim());
                    }
                    // no else case, multiple ^ or ~ in a row should not be valid commands to be processed
                }

                buffer.Append(c);
            }

            string lastCommand = buffer.ToString();
            if (lastCommand.Length > 0)
            {
                PatchCommand(ref lastCommand, caret, tilde);
                results.Add(lastCommand);
            }

            return results.ToArray();
        }

        private static void PatchCommand(ref string command, char caret, char tilde)
        {
            if (caret != '^' && command[0] == caret)
            {
                command = '^' + command.Substring(1);
            }

            if (tilde != '~' && command[0] == tilde)
            {
                command = '~' + command.Substring(1);
            }
        }
    }
}
