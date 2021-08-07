using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Label.Helpers;
using BinaryKits.Zpl.Label.ImageConverters;
using BinaryKits.Zpl.Viewer.Helpers;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class DownloadGraphicsZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        private readonly IPrinterStorage _printerStorage;

        public DownloadGraphicsZplCommandAnalyzer(
            VirtualPrinter virtualPrinter,
            IPrinterStorage printerStorage)
            : base("~DG", virtualPrinter)
        {
            this._printerStorage = printerStorage;
        }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplCommandData = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            var storageDevice = zplCommandData[0];

            zplCommandData = zplCommandData.Substring(2);
            var zplDataParts = zplCommandData.Split(',');

            var imageName = zplDataParts[0];
            _ = int.TryParse(zplDataParts[1], out var totalNumberOfBytesInGraphic);
            _ = int.TryParse(zplDataParts[2], out var numberOfBytesPerRow);

            //third comma is the start of the image data
            var indexOfThirdComma = this.IndexOfNthCharacter(zplCommandData, 3, ',');

            var dataHex = zplCommandData.Substring(indexOfThirdComma + 1);
            var grfImageData = ByteHelper.HexToBytes(dataHex);

            if (grfImageData.Length != totalNumberOfBytesInGraphic)
            {
                dataHex = ZebraHexCompressionHelper.Uncompress(dataHex, numberOfBytesPerRow);
                grfImageData = ByteHelper.HexToBytes(dataHex);
            }

            var converter = new ImageSharpImageConverter();
            var imageData = converter.ConvertImage(grfImageData, numberOfBytesPerRow);

            this._printerStorage.AddFile(storageDevice, imageName, imageData);

            return null;
        }
    }
}
