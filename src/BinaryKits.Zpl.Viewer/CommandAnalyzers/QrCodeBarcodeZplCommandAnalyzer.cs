using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class QrCodeBarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public QrCodeBarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^BQ", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplCommandData = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            var zplDataParts = zplCommandData.Split(',');

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            var magnificationFactor = 3;
            var errorCorrection = "Q";
            var maskValue = 7;

            if (zplDataParts.Length > 1)
            {
                _ = int.TryParse(zplDataParts[1], out magnificationFactor);
            }
            if (zplDataParts.Length > 2)
            {
                errorCorrection = zplDataParts[2];
            }
            if (zplDataParts.Length > 3)
            {
                _ = int.TryParse(zplDataParts[3], out maskValue);
            }

            this.VirtualPrinter.SetNextFieldDataElement(new QrCodeBarcodeFieldData
            {
                FieldOrientation = fieldOrientation,
                MagnificationFactor = magnificationFactor,
                ErrorCorrection = errorCorrection,
                MaskValue = maskValue
            });

            return null;
        }
    }
}
