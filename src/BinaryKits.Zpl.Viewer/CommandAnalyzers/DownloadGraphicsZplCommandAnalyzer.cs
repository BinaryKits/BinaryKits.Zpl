using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Label.ImageConverters;
using BinaryKits.Zpl.Viewer.Helpers;

using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class DownloadGraphicsZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        private static readonly Regex commandRegex = new(@"^~DG(\w:)?(.*?\..+?),(\d+),(\d+),(.+)$", RegexOptions.Compiled);

        private readonly IPrinterStorage printerStorage;

        public DownloadGraphicsZplCommandAnalyzer(VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
            : base("~DG", virtualPrinter)
        {
            this.printerStorage = printerStorage;
        }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            Match commandMatch = commandRegex.Match(zplCommand);
            if (commandMatch.Success)
            {
                char storageDevice = commandMatch.Groups[1].Success ? commandMatch.Groups[1].Value[0] : 'R';
                string imageName = commandMatch.Groups[2].Value;
                _ = int.TryParse(commandMatch.Groups[3].Value, out int totalNumberOfBytesInGraphic);
                _ = int.TryParse(commandMatch.Groups[4].Value, out int numberOfBytesPerRow);
                string dataHex = commandMatch.Groups[5].Value;

                byte[] grfImageData = ImageHelper.GetImageBytes(dataHex, numberOfBytesPerRow);

                // assert grfImageData.Length == totalNumberOfBytesInGraphic

                ImageSharpImageConverter converter = new();
                byte[] imageData = converter.ConvertImage(grfImageData, numberOfBytesPerRow);

                this.printerStorage.AddFile(storageDevice, imageName, imageData);
            }

            return null;
        }
    }
}
