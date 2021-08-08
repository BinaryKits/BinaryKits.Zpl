using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ImageMoveZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public ImageMoveZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^IM", virtualPrinter)
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

            var zplCommandData = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            //Get StorageDevice
            var storageDevice = zplCommandData[0];

            var objectName = zplCommandData.Substring(2);

            return new ZplImageMove(x, y, storageDevice, objectName, string.Empty);
        }
    }
}
