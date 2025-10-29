using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class UpcExtensionBarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public UpcExtensionBarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^BS", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            int height = this.VirtualPrinter.BarcodeInfo.Height;
            bool printInterpretationLine = true;
            bool printInterpretationLineAboveCode = true;

            FieldOrientation fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                height = tmpint;
            }

            if (zplDataParts.Length > 2)
            {
                printInterpretationLine = this.ConvertBoolean(zplDataParts[2], "Y");
            }

            if (zplDataParts.Length > 3)
            {
                printInterpretationLineAboveCode = this.ConvertBoolean(zplDataParts[3], "Y");
            }

            this.VirtualPrinter.SetNextElementFieldData(new UpcExtensionBarcodeFieldData
            {
                FieldOrientation = fieldOrientation,
                Height = height,
                PrintInterpretationLine = printInterpretationLine,
                PrintInterpretationLineAboveCode = printInterpretationLineAboveCode
            });

            return null;
        }
    }
}
