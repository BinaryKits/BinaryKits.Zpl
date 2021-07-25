using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldSeparatorZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldSeparatorZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FS", virtualPrinter)
        { }

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            this.VirtualPrinter.ClearNextElementPosition();
            this.VirtualPrinter.ClearNextFont();
            this.VirtualPrinter.ClearNextFieldDataElement();

            return null;
        }
    }
}
