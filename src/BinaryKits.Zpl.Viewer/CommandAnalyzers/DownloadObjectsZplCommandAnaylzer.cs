using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class DownloadObjectsZplCommandAnaylzer : ZplCommandAnalyzerBase
    {
        private readonly IPrinterStorage _printerStorage;

        public DownloadObjectsZplCommandAnaylzer(
            VirtualPrinter virtualPrinter,
            IPrinterStorage printerStorage)
            : base("~DY", virtualPrinter)
        {
            this._printerStorage = printerStorage;
        }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var storageDevice = zplCommand[this.PrinterCommandPrefix.Length];

            var zplDataParts = this.SplitCommand(zplCommand, 2);

            var objectName = zplDataParts[0];
            var formatDownloadedInDataField = zplDataParts[1];
            var extensionOfStoredFile = zplDataParts[2];
            _ = int.TryParse(zplDataParts[3], out var objectDataLength);
            _ = int.TryParse(zplDataParts[4], out var totalNumberOfBytesPerRow);

            var dataHex = zplDataParts[5];

            this._printerStorage.AddFile(storageDevice, objectName, ByteHelper.HexToBytes(dataHex));

            return null;
        }
    }
}
