using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldBlockZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldBlockZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FB", virtualPrinter)
        { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            int widthOfTextBlockLine = 0;
            int maximumNumberOfLinesInTextBlock = 1;
            int addOrDeleteSpaceBetweenLines = 0;
            var textJustification = TextJustification.Left;
            int hangingIndentOfTheSecondAndRemainingLines = 0;

            if (zplDataParts.Length > 0 && int.TryParse(zplDataParts[0], out tmpint))
            {
                widthOfTextBlockLine = tmpint;
            }

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                maximumNumberOfLinesInTextBlock = tmpint;
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint)) {
                addOrDeleteSpaceBetweenLines = tmpint;
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

            if (zplDataParts.Length > 4 && int.TryParse(zplDataParts[4], out tmpint)) {
                hangingIndentOfTheSecondAndRemainingLines = tmpint;
            }

            this.VirtualPrinter.SetNextElementFieldBlock(new FieldBlock
            {
                WidthOfTextBlockLine = widthOfTextBlockLine,
                MaximumNumberOfLinesInTextBlock = maximumNumberOfLinesInTextBlock,
                AddOrDeleteSpaceBetweenLines = addOrDeleteSpaceBetweenLines,
                TextJustification = textJustification,
                HangingIndentOfTheSecondAndRemainingLines = hangingIndentOfTheSecondAndRemainingLines
            });

            return null;
        }
    }
}
