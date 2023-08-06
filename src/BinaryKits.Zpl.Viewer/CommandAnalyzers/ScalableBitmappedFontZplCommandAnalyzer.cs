using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ScalableBitmappedFontZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public ScalableBitmappedFontZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^A", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            string fontName = zplCommand[this.PrinterCommandPrefix.Length].ToString();

            var zplDataParts = this.SplitCommand(zplCommand, 1);

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);

            int tmpint;
            int? parsedHeight = null;
            int? parsedWidth = null;

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                parsedHeight = tmpint;
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                parsedWidth = tmpint;
            }

            int fontHeight = parsedHeight ?? (parsedWidth ?? this.VirtualPrinter.FontHeight);
            int fontWidth = parsedWidth ?? (parsedHeight ?? this.VirtualPrinter.FontWidth);

            this.VirtualPrinter.SetNextFont(fontName, fieldOrientation, fontWidth, fontHeight);

            return null;
        }
    }
}
