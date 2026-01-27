using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ChangeAlphanumericDefaultFontZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public ChangeAlphanumericDefaultFontZplCommandAnalyzer() : base("^CF") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            int fontHeight = 9;
            int fontWidth = 0;

            if (zplDataParts.Length > 0)
            {
                string newFont = zplDataParts[0];
                if (!string.IsNullOrEmpty(newFont))
                {
                    virtualPrinter.SetFontName(newFont);
                }
            }

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                fontHeight = tmpint;
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                fontWidth = tmpint;
            }

            virtualPrinter.SetFontHeight(fontHeight);
            virtualPrinter.SetFontWidth(fontWidth);

            return null;
        }
    }
}
