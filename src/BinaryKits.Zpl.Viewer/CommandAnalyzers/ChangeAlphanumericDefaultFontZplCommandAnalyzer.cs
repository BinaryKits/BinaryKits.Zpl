using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ChangeAlphanumericDefaultFontZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public ChangeAlphanumericDefaultFontZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^CF", virtualPrinter)
        { }

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            var zplCommandData = zplCommandStructure.CurrentCommand.Substring(this.PrinterCommandPrefix.Length);

            var zplDataParts = zplCommandData.Split(',');

            this.VirtualPrinter.SetFontName(zplDataParts[0]);

            var fontWidht = 0;

            _ = int.TryParse(zplDataParts[1], out var fontHeight);
            if (zplDataParts.Length > 2)
            {
                _ = int.TryParse(zplDataParts[2], out fontWidht);
            }

            this.VirtualPrinter.SetFontHeight(fontHeight);
            this.VirtualPrinter.SetFontWidth(fontWidht);

            return null;
        }
    }
}
