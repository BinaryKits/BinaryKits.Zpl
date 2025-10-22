using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class LabelReversePrintZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public LabelReversePrintZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^LR", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            bool reverse = false;

            if (zplDataParts.Length > 0)
            {
                reverse = this.ConvertBoolean(zplDataParts[0]);
            }

            this.VirtualPrinter.SetLabelReverse(reverse);
            return null;
        }
    }
}
