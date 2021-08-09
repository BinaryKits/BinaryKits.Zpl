using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class RecallGraphicZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public RecallGraphicZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^XG", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var x = 0;
            var y = 0;

            if (this.VirtualPrinter.NextElementPosition != null)
            {
                x = this.VirtualPrinter.NextElementPosition.X;
                y = this.VirtualPrinter.NextElementPosition.Y;
            }

            var storageDevice = zplCommand[this.PrinterCommandPrefix.Length];

            var zplDataParts = this.SplitCommand(zplCommand, 2);

            var imageName = zplDataParts[0];
            var magnificationFactorX = 1;
            var magnificationFactorY = 1;
            if (zplDataParts.Length > 1)
            {
                _ = int.TryParse(zplDataParts[1], out magnificationFactorX);
            }
            if (zplDataParts.Length > 2)
            {
                _ = int.TryParse(zplDataParts[2], out magnificationFactorY);
            }

            return new ZplRecallGraphic(x, y, storageDevice, imageName, magnificationFactorX, magnificationFactorY);
        }
    }
}
