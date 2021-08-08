using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ScalableBitmappedFontZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public ScalableBitmappedFontZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^A", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var fontName = zplCommand.Substring(this.PrinterCommandPrefix.Length)[0].ToString();

            var zplDataParts = this.SplitCommand(zplCommand, 1);

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            _ = int.TryParse(zplDataParts[1], out var fontHeight);
            _ = int.TryParse(zplDataParts[2], out var fontWidht);

            this.VirtualPrinter.SetNextFont(fontName, fieldOrientation, fontWidht, fontHeight);

            return null;
        }
    }
}
