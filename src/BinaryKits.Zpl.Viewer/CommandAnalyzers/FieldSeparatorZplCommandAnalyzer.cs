using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldSeparatorZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldSeparatorZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FS", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            this.VirtualPrinter.ClearNextElementPosition();
            this.VirtualPrinter.ClearNextFont();
            this.VirtualPrinter.ClearNextFieldDataElement();
            this.VirtualPrinter.ClearFieldReversePrint();
            this.VirtualPrinter.ClearComments();

            return null;
        }
    }
}
