using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldBlockZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldBlockZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FB", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            var widthOfTextBlockLine = 0;
            var maximumNumberOfLinesInTextBlock = 1;
            var addOrDeleteSpaceBetweenLines = 0;
            var textJustification = TextJustification.Left;
            var hangingIndentOfTheSecondAndRemainingLines = 0;

            if (zplDataParts.Length > 0)
            {
                _ = int.TryParse(zplDataParts[0], out widthOfTextBlockLine);
            }
            if (zplDataParts.Length > 1)
            {
                _ = int.TryParse(zplDataParts[1], out maximumNumberOfLinesInTextBlock);
            }
            if (zplDataParts.Length > 2)
            {
                _ = int.TryParse(zplDataParts[2], out addOrDeleteSpaceBetweenLines);
            }
            if (zplDataParts.Length > 3)
            {
                switch (zplDataParts[3])
                {
                    case "C":
                        textJustification = TextJustification.Center;
                        break;
                    case "R":
                        textJustification = TextJustification.Right;
                        break;
                    case "J":
                        textJustification = TextJustification.Justified;
                        break;
                }
            }
            if (zplDataParts.Length > 4)
            {
                _ = int.TryParse(zplDataParts[4], out hangingIndentOfTheSecondAndRemainingLines);
            }

            var fieldBlock = new FieldBlock
            {
                WidthOfTextBlockLine = widthOfTextBlockLine,
                MaximumNumberOfLinesInTextBlock = maximumNumberOfLinesInTextBlock,
                AddOrDeleteSpaceBetweenLines = addOrDeleteSpaceBetweenLines,
                TextJustification = textJustification,
                HangingIndentOfTheSecondAndRemainingLines = hangingIndentOfTheSecondAndRemainingLines
            };

            this.VirtualPrinter.SetNextElementFieldBlock(fieldBlock);

            return null;
        }
    }
}
