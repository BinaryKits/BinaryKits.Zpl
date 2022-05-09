using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldNumberCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldNumberCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FN", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            _ = int.TryParse(zplDataParts[0], out var number);

            this.VirtualPrinter.SetNextFieldNumber(number);
            return null;
        }
    }
}
