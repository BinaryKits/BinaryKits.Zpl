using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class GraphicBoxZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public GraphicBoxZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^GB", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            int tmpint;
            int width = 1;
            int height = 1;
            int borderThickness = 1;
            LineColor lineColor = LineColor.Black;
            int cornerRounding = 0;

            int x = 0;
            int y = 0;
            bool bottomToTop = false;
            bool useDefaultPosition = false;

            if (this.VirtualPrinter.NextElementPosition != null)
            {
                x = this.VirtualPrinter.NextElementPosition.X;
                y = this.VirtualPrinter.NextElementPosition.Y;

                bottomToTop = this.VirtualPrinter.NextElementPosition.CalculateFromBottom;
                useDefaultPosition = this.VirtualPrinter.NextElementPosition.UseDefaultPosition;
            }

            string[] zplDataParts = this.SplitCommand(zplCommand);

            if (zplDataParts.Length > 0 && int.TryParse(zplDataParts[0], out tmpint)) {
                width= tmpint;
            }

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint)) {
                height = tmpint;
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                borderThickness = tmpint;
            }

            if (zplDataParts.Length > 3)
            {
                string lineColorTemp = zplDataParts[3];
                lineColor = lineColorTemp == "W" ? LineColor.White : LineColor.Black;
            }

            if (zplDataParts.Length > 4 && int.TryParse(zplDataParts[4], out tmpint))
            {
                cornerRounding = tmpint;
            }

            bool reversePrint = this.VirtualPrinter.NextElementFieldReverse || this.VirtualPrinter.LabelReverse;

            return new ZplGraphicBox(x, y, width, height, borderThickness, lineColor, cornerRounding, reversePrint, bottomToTop, useDefaultPosition);
        }
    }
}
