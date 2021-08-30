using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ScalableBitmappedFontZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public ScalableBitmappedFontZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^A", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var fontName = zplCommand[this.PrinterCommandPrefix.Length].ToString();

            var zplDataParts = this.SplitCommand(zplCommand, 1);

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            var fontHeight = 0;
            var fontWidth = 0;
            if (zplDataParts.Length > 1)
            {
                _ = int.TryParse(zplDataParts[1], out fontHeight);
            }
            if (zplDataParts.Length > 2)
            {
                _ = int.TryParse(zplDataParts[2], out fontWidth);
            }

            this.VirtualPrinter.SetNextFont(fontName, fieldOrientation, fontWidth, fontHeight);

            return null;
        }
    }
}
