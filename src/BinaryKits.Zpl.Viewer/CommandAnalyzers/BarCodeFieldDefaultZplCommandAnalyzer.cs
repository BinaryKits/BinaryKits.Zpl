using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class BarCodeFieldDefaultZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public BarCodeFieldDefaultZplCommandAnalyzer() : base("^BY") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            double tmpdbl;

            if (zplDataParts.Length > 0 && int.TryParse(zplDataParts[0], out tmpint))
            {
                virtualPrinter.SetBarcodeModuleWidth(tmpint);
            }

            if (zplDataParts.Length > 1 && double.TryParse(zplDataParts[1], out tmpdbl))
            {
                virtualPrinter.SetBarcodeWideBarToNarrowBarWidthRatio(tmpdbl);
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                virtualPrinter.SetBarcodeHeight(tmpint);
            }

            return null;
        }
    }
}
