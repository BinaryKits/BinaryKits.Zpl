using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Label.Helpers;
using BinaryKits.Zpl.Label.ImageConverters;
using BinaryKits.Zpl.Viewer.Helpers;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class DownloadGraphicsZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        private static readonly Regex commandRegex = new Regex(@"^~DG(\w:)?(.*?\..+?),(\d+),(\d+),(.+)$", RegexOptions.Compiled);
        private static readonly Regex hexDataRegex = new Regex("^[0-9A-Fa-f]+$", RegexOptions.Compiled);

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
            var commandMatch = commandRegex.Match(zplCommand);
            if (commandMatch.Success)
            {
                var storageDevice = commandMatch.Groups[1].Success ? commandMatch.Groups[1].Value[0] : 'R';
                var imageName = commandMatch.Groups[2].Value;
                _ = int.TryParse(commandMatch.Groups[3].Value, out var totalNumberOfBytesInGraphic);
                _ = int.TryParse(commandMatch.Groups[4].Value, out var numberOfBytesPerRow);
                var dataHex = commandMatch.Groups[5].Value;


                if (!hexDataRegex.IsMatch(dataHex))
                {
                    dataHex = ZebraHexCompressionHelper.Uncompress(dataHex, numberOfBytesPerRow);
                }
                var grfImageData = ByteHelper.HexToBytes(dataHex);

                // assert grfImageData.Length == totalNumberOfBytesInGraphic

                var converter = new ImageSharpImageConverter();
                var imageData = converter.ConvertImage(grfImageData, numberOfBytesPerRow);

                this._printerStorage.AddFile(storageDevice, imageName, imageData);
            }

            return null;
        }
    }
}
