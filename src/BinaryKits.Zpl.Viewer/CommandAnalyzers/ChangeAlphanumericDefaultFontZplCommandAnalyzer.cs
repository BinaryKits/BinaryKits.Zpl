using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ChangeAlphanumericDefaultFontZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public ChangeAlphanumericDefaultFontZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^CF", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            this.VirtualPrinter.SetFontName(zplDataParts[0]);

            int tmpint;
            int fontHeight = 9;
            int fontWidth = 0;

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                fontHeight = tmpint;
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                fontWidth = tmpint;
            }

            this.VirtualPrinter.SetFontHeight(fontHeight);
            this.VirtualPrinter.SetFontWidth(fontWidth);

            return null;
        }
    }
}
