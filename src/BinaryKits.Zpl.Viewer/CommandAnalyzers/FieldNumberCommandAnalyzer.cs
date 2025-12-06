using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldNumberCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldNumberCommandAnalyzer() : base("^FN") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            int fieldNumber = 0;

            if (zplDataParts.Length > 0 && int.TryParse(zplDataParts[0], out tmpint))
            {
                fieldNumber = tmpint;
            }

            virtualPrinter.SetNextFieldNumber(fieldNumber);
            return null;
        }
    }
}
