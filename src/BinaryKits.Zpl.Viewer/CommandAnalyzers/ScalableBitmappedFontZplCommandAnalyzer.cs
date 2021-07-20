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

            var fieldOrientation = FieldOrientation.Normal;
            switch (zplDataParts[0])
            {
                case "N":
                    fieldOrientation = FieldOrientation.Normal;
                    break;
                case "R":
                    fieldOrientation = FieldOrientation.Rotated90;
                    break;
                case "I":
                    fieldOrientation = FieldOrientation.Rotated180;
                    break;
                case "B":
                    fieldOrientation = FieldOrientation.Rotated270;
                    break;
            }

            _ = int.TryParse(zplDataParts[1], out var fontHeight);
            _ = int.TryParse(zplDataParts[2], out var fontWidht);

            this.VirtualPrinter.SetNextFont(fontName, fieldOrientation, fontWidht, fontHeight);

            return null;
        }
    }
}
