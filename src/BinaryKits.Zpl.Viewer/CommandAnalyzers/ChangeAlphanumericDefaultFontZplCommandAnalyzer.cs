using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ChangeAlphanumericDefaultFontZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public ChangeAlphanumericDefaultFontZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^CF", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            this.VirtualPrinter.SetFontName(zplDataParts[0]);

            var fontWidth = 0;

            _ = int.TryParse(zplDataParts[1], out var fontHeight);
            if (zplDataParts.Length > 2)
            {
                _ = int.TryParse(zplDataParts[2], out fontWidth);
            }

            this.VirtualPrinter.SetFontHeight(fontHeight);
            this.VirtualPrinter.SetFontWidth(fontWidth);

            return null;
        }
    }
}
