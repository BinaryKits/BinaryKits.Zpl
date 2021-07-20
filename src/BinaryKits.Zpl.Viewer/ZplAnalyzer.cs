using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.CommandAnalyzers;
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

            var elementAnalyzers = new List<IZplCommandAnalyzer>
            {
                new ChangeAlphanumericDefaultFontZplCommandAnalyzer(this._virtualPrinter),
                new ScalableBitmappedFontZplCommandAnalyzer(this._virtualPrinter),
                new FieldOriginZplCommandAnalzer(this._virtualPrinter),
                new FieldDataZplCommandAnalyzer(this._virtualPrinter),
                new GraphicBoxZplCommandAnalyzer(this._virtualPrinter),
                new GraphicCircleZplCommandAnalyzer(this._virtualPrinter),
                new DownloadObjectsZplCommandAnaylzer(this._virtualPrinter, this._printerStorage),
                new ImageMoveZplCommandAnalyzer(this._virtualPrinter),
                new Code39BarcodeZplCommandAnalyzer(this._virtualPrinter)
            };

            var elements = new List<ZplElementBase>();
            for (var i = 0; i < zplCommands.Length; i++)
            {
                var currentCommand = zplCommands[i];
                var validAnalyzers = elementAnalyzers.Where(o => o.CanAnalyze(currentCommand));

                var previousIndex = i - 1;
                var nextIndex = i + 1;

                var nextCommand = string.Empty;
                var previousCommand = string.Empty;

                if (previousIndex > 0)
                {
                    previousCommand = zplCommands[previousIndex];
                }

                if (zplCommands.Length > nextIndex)
                {
                    nextCommand = zplCommands[nextIndex];
                }

                var structure = new ZplCommandStructure
                {
                    CurrentCommand = currentCommand,
                    PreviousCommand = previousCommand,
                    NextCommand = nextCommand
                };

                elements.AddRange(validAnalyzers.Select(analyzer => analyzer.Analyze(structure)).Where(o => o != null));
            }

            return elements.ToArray();
        }

        private string[] SplitZplCommands(string zplData)
        {
            var replacementString = string.Empty;
            var cleanZpl = Regex.Replace(zplData, @"\r\n?|\n", replacementString);
            return Regex.Split(cleanZpl, "(?=\\^)|(?=\\~)").ToArray();
        }
    }
}
