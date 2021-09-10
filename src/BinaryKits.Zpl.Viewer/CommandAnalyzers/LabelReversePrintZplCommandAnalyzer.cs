using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class LabelReversePrintZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public LabelReversePrintZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^LR", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            if (zplCommand.Length > 3)
            {
                var reverse = char.ToUpperInvariant(zplCommand[3]) == 'Y';
                this.VirtualPrinter.SetLabelReverse(reverse);
            }
            return null;
        }
    }
}
