using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldReversePrintZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldReversePrintZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FR", virtualPrinter)
        { }

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            this.VirtualPrinter.SetFieldReversePrint();

            return null;
        }
    }
}
