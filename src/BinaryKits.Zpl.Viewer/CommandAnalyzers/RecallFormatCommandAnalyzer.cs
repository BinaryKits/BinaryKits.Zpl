using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class RecallFormatCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public RecallFormatCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^XF", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var formatName = zplCommand.Substring(this.PrinterCommandPrefix.Length);
            return new ZplRecallFormat(formatName);
        }
    }
}
