using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ScalableBitmappedFontZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public ScalableBitmappedFontZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^A", virtualPrinter)
        { }

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            var zplCommandData = zplCommandStructure.CurrentCommand.Substring(this.PrinterCommandPrefix.Length);

            var fontName = zplCommandData[0].ToString();

            var zplDataParts = zplCommandData.Substring(1).Split(',');

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            _ = int.TryParse(zplDataParts[1], out var fontHeight);
            _ = int.TryParse(zplDataParts[2], out var fontWidht);

            this.VirtualPrinter.SetNextFont(fontName, fieldOrientation, fontWidht, fontHeight);

            return null;
        }
    }
}
