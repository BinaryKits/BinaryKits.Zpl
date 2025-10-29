using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Label.Helpers;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class DownloadObjectsZplCommandAnaylzer : ZplCommandAnalyzerBase
    {
        private readonly IPrinterStorage printerStorage;

        public DownloadObjectsZplCommandAnaylzer(VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
            : base("~DY", virtualPrinter)
        {
            this.printerStorage = printerStorage;
        }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            char storageDevice = zplCommand[this.PrinterCommandPrefix.Length];

            string[] zplDataParts = this.SplitCommand(zplCommand, 2);

            string objectName = zplDataParts[0];
            string formatDownloadedInDataField = zplDataParts[1];
            string extensionOfStoredFile = zplDataParts[2];
            _ = int.TryParse(zplDataParts[3], out int objectDataLength);
            _ = int.TryParse(zplDataParts[4], out int totalNumberOfBytesPerRow);

            // TODO: Handle case when .GRF data is downloaded using the ~DY command
            string dataHex = zplDataParts[5];

            this.printerStorage.AddFile(storageDevice, objectName, ByteHelper.HexToBytes(dataHex));

            return null;
        }
    }
}
