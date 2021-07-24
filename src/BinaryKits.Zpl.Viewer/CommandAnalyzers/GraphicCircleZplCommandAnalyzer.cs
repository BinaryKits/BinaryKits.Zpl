using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class GraphicCircleZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public GraphicCircleZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^GC", virtualPrinter)
        { }

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            var x = this.VirtualPrinter.NextElementPosition.X;
            var y = this.VirtualPrinter.NextElementPosition.Y;

            this.VirtualPrinter.ClearNextElementPosition();

            var zplCommandData = zplCommandStructure.CurrentCommand.Substring(this.PrinterCommandPrefix.Length);

            var zplDataParts = zplCommandData.Split(',');

            _ = int.TryParse(zplDataParts[0], out var circleDiameter);
            _ = int.TryParse(zplDataParts[1], out var borderThickness);

            var lineColor = LineColor.Black;
            if (zplDataParts.Length > 2)
            {
                var lineColorTemp = zplDataParts[2];
                lineColor = lineColorTemp == "B" ? LineColor.Black : LineColor.White;
            }

            return new ZplGraphicCircle(x, y, circleDiameter, borderThickness, lineColor);
        }
    }
}
