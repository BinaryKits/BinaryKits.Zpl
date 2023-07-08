using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class RecallFormatCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public RecallFormatCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^XF", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            string formatName = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            return new ZplRecallFormat(formatName);
        }
    }
}
