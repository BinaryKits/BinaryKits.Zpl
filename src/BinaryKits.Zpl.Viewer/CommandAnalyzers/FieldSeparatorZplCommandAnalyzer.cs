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
            this.VirtualPrinter.ClearNextElementFieldBlock();
            this.VirtualPrinter.ClearNextElementFieldData();
            this.VirtualPrinter.ClearNextElementFieldReverse();
            this.VirtualPrinter.ClearNextFont();
            this.VirtualPrinter.ClearComments();

            return null;
        }
    }
}
