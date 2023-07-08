﻿using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class BarCodeFieldDefaultZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public BarCodeFieldDefaultZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^BY", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            double tmpdbl;
            int moduleWidth = 2;
            double wideBarToNarrowBarWidthRatio = 3.0;
            int barcodeHeight = 10;

            if (zplDataParts.Length > 0 && int.TryParse(zplDataParts[0], out tmpint))
            {
                moduleWidth = tmpint;
            }

            if (zplDataParts.Length > 1 && double.TryParse(zplDataParts[1], out tmpdbl))
            {
                wideBarToNarrowBarWidthRatio = tmpdbl;
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                barcodeHeight = tmpint;
            }

            this.VirtualPrinter.SetBarcodeModuleWidth(moduleWidth);
            this.VirtualPrinter.SetBarcodeWideBarToNarrowBarWidthRatio(wideBarToNarrowBarWidthRatio);
            this.VirtualPrinter.SetBarcodeHeight(barcodeHeight);

            return null;
        }
    }
}
