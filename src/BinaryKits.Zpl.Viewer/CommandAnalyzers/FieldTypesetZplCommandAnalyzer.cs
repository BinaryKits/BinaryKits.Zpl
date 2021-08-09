using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldTypesetZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldTypesetZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FT", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            var x = 0;
            var y = 0;
            var z = 0;

            _ = int.TryParse(zplDataParts[0], out x);
            if (zplDataParts.Length > 1)
            {
                _ = int.TryParse(zplDataParts[1], out y);
            }
            if (zplDataParts.Length > 2)
            {
                _ = int.TryParse(zplDataParts[2], out z);
            }

            if (this.VirtualPrinter.LabelHomePosition != null)
            {
                x += this.VirtualPrinter.LabelHomePosition.X;
                y += this.VirtualPrinter.LabelHomePosition.Y;
            }

            this.VirtualPrinter.SetNextElementPosition(x, y, calculateFromBottom: true);

            return null;
        }
    }
}
