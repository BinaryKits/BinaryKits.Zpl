using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldOrientationZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldOrientationZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FW", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);
            if (zplDataParts.Length > 0)
            {
                var fieldOrientation = ConvertFieldOrientation(zplDataParts[0]);
                this.VirtualPrinter.SetFieldOrientation(fieldOrientation);
            }

            return null;
        }
    }
}
