using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Label.Helpers;
using BinaryKits.Zpl.Label.ImageConverters;
using BinaryKits.Zpl.Viewer.Helpers;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class GraphicFieldZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public GraphicFieldZplCommandAnalyzer() : base("^GF") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            int tmpint;
            int binaryByteCount = 0;
            int graphicFieldCount = 0;
            int bytesPerRow = 0;

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

            string[] zplDataParts = this.SplitCommand(zplCommand);

            char compressionType = zplDataParts[0][0];

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                binaryByteCount = tmpint;
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                graphicFieldCount = tmpint;
            }

            if (zplDataParts.Length > 3 && int.TryParse(zplDataParts[3], out tmpint))
            {
                bytesPerRow = tmpint;
            }

            //fourth comma is the start of the image data
            int indexOfFourthComma = this.IndexOfNthCharacter(zplCommand, 4, ',');
            string dataHex = zplCommand.Substring(indexOfFourthComma + 1).TrimStart();

            byte[] grfImageData = ImageHelper.GetImageBytes(dataHex, bytesPerRow);

            ImageSharpImageConverter converter = new();
            byte[] imageData = converter.ConvertImage(grfImageData, bytesPerRow);
            dataHex = ByteHelper.BytesToHex(imageData);

            return new ZplGraphicField(x, y, binaryByteCount, graphicFieldCount, bytesPerRow, dataHex, bottomToTop, useDefaultPosition, compressionType);
        }
    }
}
