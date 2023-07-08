﻿using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class QrCodeBarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public QrCodeBarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^BQ", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);

            int tmpint;
            int model = 2;
            int magnificationFactor = 3;
            var errorCorrection = ErrorCorrectionLevel.HighReliability;
            int maskValue = 7;

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                model = tmpint;
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                magnificationFactor = tmpint;

                if (magnificationFactor > 10)
                {
                    //TODO: Add validation message max value is 10
                    magnificationFactor = 10;
                }
            }

            if (zplDataParts.Length > 3)
            {
                errorCorrection = this.ConvertErrorCorrectionLevel(zplDataParts[3]);
            }

            if (zplDataParts.Length > 4 && int.TryParse(zplDataParts[4], out tmpint))
            {
                maskValue = tmpint;
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
