using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldOriginZplCommandAnalzer : ZplCommandAnalyzerBase
    {
        public FieldOriginZplCommandAnalzer(VirtualPrinter virtualPrinter) : base("^FO", virtualPrinter)
        { }

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            var zplCommandData = zplCommandStructure.CurrentCommand.Substring(this.PrinterCommandPrefix.Length);
            var zplDataParts = zplCommandData.Split(',');

            _ = int.TryParse(zplDataParts[0], out var x);
            _ = int.TryParse(zplDataParts[1], out var y);

            this.VirtualPrinter.SetNextElementPosition(x, y);

            return null;
        }
    }
}
