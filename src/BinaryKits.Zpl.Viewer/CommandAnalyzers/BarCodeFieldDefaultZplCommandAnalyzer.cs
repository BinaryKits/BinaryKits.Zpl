using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class BarCodeFieldDefaultZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public BarCodeFieldDefaultZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^BY", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            if (zplDataParts.Length > 0)
            {
                _ = int.TryParse(zplDataParts[0], out var moduleWidth);
                this.VirtualPrinter.SetBarcodeModuleWidth(moduleWidth);
            }

            if (zplDataParts.Length > 1)
            {
                _ = double.TryParse(zplDataParts[1], out var wideBarToNarrowBarWidthRatio);
                this.VirtualPrinter.SetBarcodeWideBarToNarrowBarWidthRatio(wideBarToNarrowBarWidthRatio);
            }

            if (zplDataParts.Length > 2)
            {
                _ = int.TryParse(zplDataParts[2], out var barcodeHeight);
                this.VirtualPrinter.SetBarcodeHeight(barcodeHeight);
            }

            return null;
        }
    }
}
