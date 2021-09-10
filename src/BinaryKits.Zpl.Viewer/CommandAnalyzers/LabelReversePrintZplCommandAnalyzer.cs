using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class LabelReversePrintZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public LabelReversePrintZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^LR", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            this.VirtualPrinter.SetLabelReverse(zplCommand[3] == 'Y' || zplCommand[3] == 'y');
            return null;
        }
    }
}
