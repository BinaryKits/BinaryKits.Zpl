using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldHexadecimalZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldHexadecimalZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FH", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            this.VirtualPrinter.SetNextElementFieldUseHexadecimalIndicator();
            var zplDataParts = this.SplitCommand(zplCommand);

            char Indicator = '_';

            if ((zplDataParts.Length > 0) && (zplDataParts[0].Length > 0))
            {
                Indicator = zplDataParts[0][0];
            }

            StringHelper.ReplaceChar = Indicator;

            return null;
        }
    }
}
