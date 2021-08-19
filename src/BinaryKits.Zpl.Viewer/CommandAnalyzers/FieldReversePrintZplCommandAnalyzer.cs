using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldReversePrintZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldReversePrintZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FR", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            this.VirtualPrinter.SetNextElementFieldReverse();

            return null;
        }
    }
}
