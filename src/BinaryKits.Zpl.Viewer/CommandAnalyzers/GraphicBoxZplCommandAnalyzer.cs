using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class GraphicBoxZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public GraphicBoxZplCommandAnalyzer() : base("^GB") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
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

            if (virtualPrinter.NextElementPosition != null)
            {
                x = virtualPrinter.NextElementPosition.X;
                y = virtualPrinter.NextElementPosition.Y;

                bottomToTop = virtualPrinter.NextElementPosition.CalculateFromBottom;
                useDefaultPosition = virtualPrinter.NextElementPosition.UseDefaultPosition;
            }

            string[] zplDataParts = this.SplitCommand(zplCommand);

            // thickness is the default for width and height, parse it first
            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                borderThickness = tmpint;
                width = borderThickness;
                height = borderThickness;
            }

            if (zplDataParts.Length > 0 && int.TryParse(zplDataParts[0], out tmpint))
            {
                width = tmpint;
            }

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                height = tmpint;
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

            bool reversePrint = virtualPrinter.NextElementFieldReverse || virtualPrinter.LabelReverse;

            return new ZplGraphicBox(x, y, width, height, borderThickness, lineColor, cornerRounding, reversePrint, bottomToTop, useDefaultPosition);
        }
    }
}
