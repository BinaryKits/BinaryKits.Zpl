using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldOrientationZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldOrientationZplCommandAnalyzer() : base("^FW") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);
            if (zplDataParts.Length > 0)
            {
                FieldOrientation fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0], virtualPrinter);
                virtualPrinter.SetFieldOrientation(fieldOrientation);
            }

            if (zplDataParts.Length > 1)
            {
                FieldJustification fieldJustification = this.ConvertFieldJustification(zplDataParts[1], virtualPrinter);
                virtualPrinter.SetFieldJustification(fieldJustification);
            }

            return null;
        }
    }
}
