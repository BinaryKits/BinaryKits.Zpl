using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ImageMoveZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public ImageMoveZplCommandAnalyzer() : base("^IM") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
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

            string zplCommandData = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            //Get StorageDevice
            char storageDevice = zplCommandData[0];

            string objectName = zplCommandData.Substring(2);

            return new ZplImageMove(x, y, storageDevice, objectName, string.Empty, bottomToTop, useDefaultPosition);
        }
    }
}
