using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldDataZplCommandAnalyzer : ZplCommandAnalyzerBase
    {

        public FieldDataZplCommandAnalyzer(string prefix = "^FD") : base(prefix) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string text = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            // If field data follows a field number, a ZplRecallFieldNumber element has to be returned
            int? fieldNumber = virtualPrinter.NextFieldNumber;
            if (fieldNumber != null)
            {
                virtualPrinter.ClearNextFieldNumber(); // Prevents consumption by field separator analyzer
                return new ZplRecallFieldNumber(fieldNumber.Value, text);
            }

            virtualPrinter.SetNextElementFieldDataContent(text);

            return null;
        }
    }
}
