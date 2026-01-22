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
                // TODO: add validation message: between 1 and 10
                if (tmpint < 1)
                {
                    tmpint = 1;
                }
                else if (tmpint > 10)
                {
                    tmpint = 10;
                }

                virtualPrinter.SetBarcodeModuleWidth(tmpint);
            }

            if (zplDataParts.Length > 1 && double.TryParse(zplDataParts[1], out tmpdbl))
            {
                // TODO: add validation message: between 2.0 and 3.0 in 0.1 increments
                if (tmpdbl < 2.0)
                {
                    tmpdbl = 2.0;
                }
                else if (tmpdbl > 3.0)
                {
                    tmpdbl = 3.0;
                }
                else
                {
                    tmpdbl = System.Math.Round(tmpdbl, 1);
                }

                virtualPrinter.SetBarcodeWideBarToNarrowBarWidthRatio(tmpdbl);
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                // TODO: add validation message: greater or equal than 1
                if (tmpint < 1)
                {
                    tmpint = 1;
                }

                virtualPrinter.SetBarcodeHeight(tmpint);
            }

            return null;
        }
    }
}
