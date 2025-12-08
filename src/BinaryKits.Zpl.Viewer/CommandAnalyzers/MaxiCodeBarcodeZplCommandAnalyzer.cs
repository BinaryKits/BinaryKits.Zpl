using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class MaxiCodeBarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public MaxiCodeBarcodeZplCommandAnalyzer() : base("^BD") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            int mode = 2;
            int position = 1;
            int total = 1;

            if (zplDataParts[0] != "")
            {
                mode = int.Parse(zplDataParts[0]);
            }

            int tmpint;
            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                position = tmpint;
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                total = tmpint;
            }

            virtualPrinter.SetNextElementFieldData(new MaxiCodeBarcodeFieldData
            {
                Mode = mode,
                Position = position,
                Total = total,
            });

            return null;
        }
    }
}
