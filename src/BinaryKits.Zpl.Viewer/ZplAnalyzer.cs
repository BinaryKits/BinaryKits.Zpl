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

        /// <summary>
        /// The array of <see cref="IZplCommandAnalyzer"/> to analyze ZPL text
        /// </summary>
        public static IZplCommandAnalyzer[] Analyzers { get; } = [
            new FieldDataZplCommandAnalyzer(),
            new AztecBarcodeZplCommandAnalyzer(),
            new BarCodeFieldDefaultZplCommandAnalyzer(),
            new ChangeAlphanumericDefaultFontZplCommandAnalyzer(),
            new ChangeInternationalFontCommandAnalyzer(),
            new Code39BarcodeZplCommandAnalyzer(),
            new Code93BarcodeZplCommandAnalyzer(),
            new Code128BarcodeZplCommandAnalyzer(),
            new CodeEAN13BarcodeZplCommandAnalyzer(),
            new CommentZplCommandAnalyzer(),
            new DataMatrixZplCommandAnalyzer(),
            new DownloadFormatCommandAnalyzer(),
            new DownloadGraphicsZplCommandAnalyzer(),
            new DownloadObjectsZplCommandAnaylzer(),
            new FieldBlockZplCommandAnalyzer(),
            new FieldHexadecimalZplCommandAnalyzer(),
            new FieldOrientationZplCommandAnalyzer(),
            new FieldNumberCommandAnalyzer(),
            new FieldVariableZplCommandAnalyzer(),
            new FieldReversePrintZplCommandAnalyzer(),
            new LabelReversePrintZplCommandAnalyzer(),
            new FieldSeparatorZplCommandAnalyzer(),
            new FieldTypesetZplCommandAnalyzer(),
            new FieldOriginZplCommandAnalzer(),
            new GraphicBoxZplCommandAnalyzer(),
            new GraphicCircleZplCommandAnalyzer(),
            new GraphicDiagonalLineZplCommandAnalyzer(),
            new GraphicEllipseZplCommandAnalyzer(),
            new GraphicFieldZplCommandAnalyzer(),
            new GraphicSymbolZplCommandAnalyzer(),
            new Interleaved2of5BarcodeZplCommandAnalyzer(),
            new ImageMoveZplCommandAnalyzer(),
            new LabelHomeZplCommandAnalyzer(),
            new MaxiCodeBarcodeZplCommandAnalyzer(),
            new QrCodeBarcodeZplCommandAnalyzer(),
            new UpcABarcodeZplCommandAnalyzer(),
            new UpcEBarcodeZplCommandAnalyzer(),
            new UpcExtensionBarcodeZplCommandAnalyzer(),
            new PDF417ZplCommandAnalyzer(),
            new RecallFormatCommandAnalyzer(),
            new RecallGraphicZplCommandAnalyzer(),
            new ScalableBitmappedFontZplCommandAnalyzer(),
            new AnsiCodabarBarcodeZplCommandAnalyzer(),
        ];

        private readonly VirtualPrinter virtualPrinter;
        private readonly IPrinterStorage printerStorage;
        private readonly IFormatMerger formatMerger;
        private const string labelStartCommand = "^XA";
        private const string labelEndCommand = "^XZ";

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

            List<LabelInfo> labelInfos = [];

            List<ZplElementBase> elements = [];
            for (int i = 0; i < zplCommands.Length; i++)
            {
                string currentCommand = zplCommands[i];

                if (labelStartCommand.Equals(currentCommand.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    elements.Clear();
                    this.virtualPrinter.ClearNextDownloadFormatName();
                    continue;
                }

                if (labelEndCommand.Equals(currentCommand.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    labelInfos.Add(new LabelInfo
                    {
                        DownloadFormatName = this.virtualPrinter.NextDownloadFormatName,
                        ZplElements = elements.ToArray()
                    });
                    continue;
                }

                IEnumerable<IZplCommandAnalyzer> validAnalyzers = Analyzers.Where(o => o.CanAnalyze(currentCommand));

                if (!validAnalyzers.Any())
                {
                    unknownCommands.Add(currentCommand);
                    continue;
                }

                try
                {
                    elements.AddRange(validAnalyzers.Select(analyzer => analyzer.Analyze(currentCommand, this.virtualPrinter, this.printerStorage)).Where(o => o != null));
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
