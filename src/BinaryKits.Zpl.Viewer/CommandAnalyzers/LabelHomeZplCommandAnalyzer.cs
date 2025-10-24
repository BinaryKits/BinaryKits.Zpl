using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class LabelHomeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public LabelHomeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^LH", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            int x = 0;
            int y = 0;

            if (zplDataParts.Length > 0 && int.TryParse(zplDataParts[0], out tmpint))
            {
                x = tmpint;
            }

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                y = tmpint;
            }

            this.VirtualPrinter.SetLabelHome(x, y);

            return null;
        }
    }
}
