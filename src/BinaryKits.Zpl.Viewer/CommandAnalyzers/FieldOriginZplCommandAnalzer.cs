using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldOriginZplCommandAnalzer : ZplCommandAnalyzerBase
    {
        public FieldOriginZplCommandAnalzer(VirtualPrinter virtualPrinter) : base("^FO", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            _ = int.TryParse(zplDataParts[0], out var x);
            _ = int.TryParse(zplDataParts[1], out var y);

            if (this.VirtualPrinter.LabelHomePosition != null)
            {
                x += this.VirtualPrinter.LabelHomePosition.X;
                y += this.VirtualPrinter.LabelHomePosition.Y;
            }

            this.VirtualPrinter.SetNextElementPosition(x, y);

            return null;
        }
    }
}
