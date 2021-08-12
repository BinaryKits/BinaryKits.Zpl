using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class GraphicCircleZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public GraphicCircleZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^GC", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var x = 0;
            var y = 0;
            var bottomToTop = false;

            if (this.VirtualPrinter.NextElementPosition != null)
            {
                x = this.VirtualPrinter.NextElementPosition.X;
                y = this.VirtualPrinter.NextElementPosition.Y;
                bottomToTop = this.VirtualPrinter.NextElementPosition.CalculateFromBottom;
            }

            var zplDataParts = this.SplitCommand(zplCommand);

            _ = int.TryParse(zplDataParts[0], out var circleDiameter);
            _ = int.TryParse(zplDataParts[1], out var borderThickness);

            var lineColor = LineColor.Black;
            if (zplDataParts.Length > 2)
            {
                var lineColorTemp = zplDataParts[2];
                lineColor = lineColorTemp == "W" ? LineColor.White : LineColor.Black;
            }

            return new ZplGraphicCircle(x, y, circleDiameter, borderThickness, lineColor, bottomToTop);
        }
    }
}
