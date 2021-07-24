using BinaryKits.Zpl.Label.Elements;
using System;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class DownloadObjectsZplCommandAnaylzer : ZplCommandAnalyzerBase
    {
        private readonly IPrinterStorage _printerStorage;

        public DownloadObjectsZplCommandAnaylzer(VirtualPrinter virtualPrinter, IPrinterStorage printerStorage) : base("~DY", virtualPrinter)
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

            var objectName = zplDataParts[0];
            var formatDownloadedInDataField = zplDataParts[1];
            var extensionOfStoredFile = zplDataParts[2];
            _ = int.TryParse(zplDataParts[3], out var objectDataLength);
            var totalNumberOfBytesPerRow = zplDataParts[4];
            var dataHex = zplDataParts[5];

            this._printerStorage.AddFile(storageDevice, objectName, this.StringToByteArray(dataHex));

            return null;
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
