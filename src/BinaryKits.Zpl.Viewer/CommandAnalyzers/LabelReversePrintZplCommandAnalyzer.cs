using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class LabelReversePrintZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public LabelReversePrintZplCommandAnalyzer() : base("^LR") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            bool reverse = false;

            if (zplDataParts.Length > 0)
            {
                reverse = this.ConvertBoolean(zplDataParts[0]);
            }

            virtualPrinter.SetLabelReverse(reverse);
            return null;
        }
    }
}
