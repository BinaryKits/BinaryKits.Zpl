using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldOrientationZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldOrientationZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FW", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);
            if (zplDataParts.Length > 0)
            {
                FieldOrientation fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
                this.VirtualPrinter.SetFieldOrientation(fieldOrientation);
            }

            if (zplDataParts.Length > 1)
            {
                FieldJustification fieldJustification = this.ConvertFieldJustification(zplDataParts[1]);
                this.VirtualPrinter.SetFieldJustification(fieldJustification);
            }

            return null;
        }
    }
}
