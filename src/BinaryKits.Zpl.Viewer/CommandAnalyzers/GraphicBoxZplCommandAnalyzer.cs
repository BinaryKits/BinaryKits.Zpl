using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class GraphicBoxZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public GraphicBoxZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^GB", virtualPrinter)
        { }

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            var x = 0;
            var y = 0;

            if (this.VirtualPrinter.NextElementPosition != null)
            {
                x = this.VirtualPrinter.NextElementPosition.X;
                y = this.VirtualPrinter.NextElementPosition.Y;
            }

            var zplCommandData = zplCommandStructure.CurrentCommand.Substring(this.PrinterCommandPrefix.Length);

            var zplDataParts = zplCommandData.Split(',');

            _ = int.TryParse(zplDataParts[0], out var widht);
            _ = int.TryParse(zplDataParts[1], out var height);
            _ = int.TryParse(zplDataParts[2], out var borderThickness);

            var lineColor = LineColor.Black;
            if (zplDataParts.Length > 3)
            {
                var lineColorTemp = zplDataParts[3];
                lineColor = lineColorTemp == "B" ? LineColor.Black : LineColor.White;
            }

            var cornerRounding = 0;
            if (zplDataParts.Length > 4)
            {
                _ = int.TryParse(zplDataParts[4], out cornerRounding);
            }

            return new ZplGraphicBox(x, y, widht, height, borderThickness, lineColor, cornerRounding);
        }
    }
}
