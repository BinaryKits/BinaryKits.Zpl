using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class QrCodeBarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public QrCodeBarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^BQ", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            var model = 2;
            var magnificationFactor = 3;
            var errorCorrection = ErrorCorrectionLevel.HighReliability;
            var maskValue = 7;

            if (zplDataParts.Length > 1)
            {
                _ = int.TryParse(zplDataParts[1], out model);
            }
            if (zplDataParts.Length > 2)
            {
                _ = int.TryParse(zplDataParts[2], out magnificationFactor);

                if (magnificationFactor > 10)
                {
                    //TODO: Add validation message max value is 10
                    magnificationFactor = 10;
                }
            }
            if (zplDataParts.Length > 3)
            {
                switch (zplDataParts[3])
                {
                    case "H":
                        errorCorrection = ErrorCorrectionLevel.UltraHighReliability;
                        break;
                    case "Q":
                        errorCorrection = ErrorCorrectionLevel.HighReliability;
                        break;
                    case "M":
                        errorCorrection = ErrorCorrectionLevel.Standard;
                        break;
                    case "L":
                        errorCorrection = ErrorCorrectionLevel.HighDensity;
                        break;
                }
            }
            if (zplDataParts.Length > 4)
            {
                _ = int.TryParse(zplDataParts[4], out maskValue);
            }

            this.VirtualPrinter.SetNextElementFieldData(new QrCodeBarcodeFieldData
            {
                Model = model,
                FieldOrientation = fieldOrientation,
                MagnificationFactor = magnificationFactor,
                ErrorCorrection = errorCorrection,
                MaskValue = maskValue
            });

            return null;
        }
    }
}
