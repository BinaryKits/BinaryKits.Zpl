using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ImageMoveZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public ImageMoveZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^IM", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
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

            string zplCommandData = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            //Get StorageDevice
            char storageDevice = zplCommandData[0];

            string objectName = zplCommandData.Substring(2);

            return new ZplImageMove(x, y, storageDevice, objectName, string.Empty, bottomToTop, useDefaultPosition);
        }
    }
}
