using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Label.ImageConverters;
using System;

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

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            var zplLine = zplCommandStructure.CurrentCommand;

            var zplCommandData = zplLine.Substring(this.PrinterCommandPrefix.Length);

            var storageDevice = zplCommandData[0];

            zplCommandData = zplCommandData.Substring(2);
            var zplDataParts = zplCommandData.Split(',');

            var imageName = zplDataParts[0];
            _ = int.TryParse(zplDataParts[1], out var totalNumberOfBytesInGraphic);
            _ = int.TryParse(zplDataParts[2], out var totalNumberOfBytesPerRow);

            //third comma is the start of the image data
            var indexOfThirdComma = this.IndexOfNthCharacter(zplCommandData, 3, ',');

            var dataHex = zplCommandData.Substring(indexOfThirdComma + 1);
            var grfImageData = this.StringToByteArray(dataHex);

            if (grfImageData.Length != totalNumberOfBytesInGraphic)
            {
                return null;
            }

            var converter = new ImageSharpImageConverter();
            var imageData = converter.ConvertImage(grfImageData, totalNumberOfBytesPerRow);

            this._printerStorage.AddFile(storageDevice, imageName, imageData);

            return null;
        }

        private int IndexOfNthCharacter(string input, int occurranceToFind, char charToFind)
        {
            var index = -1;
            for (var i = 0; i < occurranceToFind; i++)
            {
                index = input.IndexOf(charToFind, index + 1);

                if (index == -1)
                {
                    break;
                }
            }

            return index;
        }

        private byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
            {
                throw new Exception("The binary key cannot have an odd number of digits");
            }

            var array = new byte[hex.Length >> 1];

            for (var i = 0; i < hex.Length >> 1; ++i)
            {
                array[i] = (byte)((this.GetHexVal(hex[i << 1]) << 4) + (this.GetHexVal(hex[(i << 1) + 1])));
            }

            return array;
        }

        private int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
