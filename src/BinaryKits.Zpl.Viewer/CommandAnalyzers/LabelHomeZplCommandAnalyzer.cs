using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class LabelHomeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public LabelHomeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^LH", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplCommandData = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            var zplDataParts = zplCommandData.Split(',');

            _ = int.TryParse(zplDataParts[0], out var x);
            _ = int.TryParse(zplDataParts[1], out var y);

            this.VirtualPrinter.SetLabelHome(x, y);

            return null;
        }
    }
}
