using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Label.Helpers;
using BinaryKits.Zpl.Label.ImageConverters;
using BinaryKits.Zpl.Viewer.Helpers;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class GraphicFieldZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public GraphicFieldZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^GF", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var x = 0;
            var y = 0;
            var bottomToTop = false;

            if (this.VirtualPrinter.NextElementPosition != null)
            {
                x = this.VirtualPrinter.NextElementPosition.X;
                y = this.VirtualPrinter.NextElementPosition.Y;

                bottomToTop = this.VirtualPrinter.NextElementPosition.CalculateFromBottom;
            }

            var zplDataParts = this.SplitCommand(zplCommand);

            var compressionType = zplDataParts[0][0];
            var binaryByteCount = 0;
            var graphicFieldCount = 0;
            var bytesPerRow = 0;

            if (zplDataParts.Length > 1)
            {
                _ = int.TryParse(zplDataParts[1], out binaryByteCount);
            }
            if (zplDataParts.Length > 2)
            {
                _ = int.TryParse(zplDataParts[2], out graphicFieldCount);
            }
            if (zplDataParts.Length > 3)
            {
                _ = int.TryParse(zplDataParts[3], out bytesPerRow);
            }

            //fourth comma is the start of the image data
            var indexOfFourthComma = this.IndexOfNthCharacter(zplCommand, 4, ',');
            var dataHex = zplCommand.Substring(indexOfFourthComma + 1);

            if (dataHex.Length != binaryByteCount * 2)
            {
                dataHex = ZebraHexCompressionHelper.Uncompress(dataHex, bytesPerRow);
            }

            var grfImageData = ByteHelper.HexToBytes(dataHex);
            var converter = new ImageSharpImageConverter();
            var imageData = converter.ConvertImage(grfImageData, bytesPerRow);
            dataHex = ByteHelper.BytesToHex(imageData);

            return new ZplGraphicField(x, y, binaryByteCount, graphicFieldCount, bytesPerRow, dataHex, bottomToTop: bottomToTop, compressionType);
        }
    }
}
