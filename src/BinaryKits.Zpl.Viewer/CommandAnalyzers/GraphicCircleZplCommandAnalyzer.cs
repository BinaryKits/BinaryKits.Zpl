using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class GraphicCircleZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public GraphicCircleZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^GC", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            int tmpint;
            int circleDiameter = 3;
            int borderThickness = 1;
            var lineColor = LineColor.Black;

            int x = 0;
            int y = 0;
            bool bottomToTop = false;

            if (this.VirtualPrinter.NextElementPosition != null)
            {
                x = this.VirtualPrinter.NextElementPosition.X;
                y = this.VirtualPrinter.NextElementPosition.Y;
                bottomToTop = this.VirtualPrinter.NextElementPosition.CalculateFromBottom;
            }

            var zplDataParts = this.SplitCommand(zplCommand);

            if (zplDataParts.Length > 0 && int.TryParse(zplDataParts[0], out tmpint))
            {
                circleDiameter = tmpint;
            }

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                borderThickness = tmpint;
            }

            if (zplDataParts.Length > 2)
            {
                string lineColorTemp = zplDataParts[2];
                lineColor = lineColorTemp == "W" ? LineColor.White : LineColor.Black;
            }

            bool reversePrint = this.VirtualPrinter.NextElementFieldReverse || this.VirtualPrinter.LabelReverse;
            
            return new ZplGraphicCircle(x, y, circleDiameter, borderThickness, lineColor, reversePrint, bottomToTop);
        }
    }
}
