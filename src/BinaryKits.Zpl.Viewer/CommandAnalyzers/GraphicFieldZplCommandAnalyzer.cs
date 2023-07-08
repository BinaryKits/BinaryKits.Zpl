﻿using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Label.Helpers;
using BinaryKits.Zpl.Label.ImageConverters;
using BinaryKits.Zpl.Viewer.Helpers;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class GraphicFieldZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public GraphicFieldZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^GF", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            int tmpint;
            int binaryByteCount = 0;
            int graphicFieldCount = 0;
            int bytesPerRow = 0;

            int x = 0;
            int y = 0;
            bool bottomToTop = false;

            if (this.VirtualPrinter.NextElementPosition != null)
            {
                x = this.VirtualPrinter.NextElementPosition.X;
                y = this.VirtualPrinter.NextElementPosition.Y;

                bottomToTop = this.VirtualPrinter.NextElementPosition.CalculateFromBottom;
            }

            var zplDataParts = this.SplitCommand(zplCommand);

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
            string dataHex = zplCommand.Substring(indexOfFourthComma + 1);

            byte[] grfImageData = ImageHelper.GetImageBytes(dataHex, bytesPerRow);

            var converter = new ImageSharpImageConverter();
            var imageData = converter.ConvertImage(grfImageData, bytesPerRow);
            dataHex = ByteHelper.BytesToHex(imageData);

            return new ZplGraphicField(x, y, binaryByteCount, graphicFieldCount, bytesPerRow, dataHex, bottomToTop: bottomToTop, compressionType);
        }
    }
}
