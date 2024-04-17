using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;
using System;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class MaxiCodeBarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public MaxiCodeBarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^BD", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            int mode = 2;
            int position = 1;
            int total = 1;
            
            if (zplDataParts[0] != "")
            {
                mode = Int32.Parse(zplDataParts[0]);
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

            this.VirtualPrinter.SetNextElementFieldData(new MaxiCodeBarcodeFieldData {
                Mode = mode, Position = position, Total = total,
            });

            return null;
        }
    }
}
