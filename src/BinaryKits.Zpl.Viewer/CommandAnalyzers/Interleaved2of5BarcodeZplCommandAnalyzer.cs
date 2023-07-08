using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class Interleaved2of5BarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public Interleaved2of5BarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^B2", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);

            int tmpint;
            int height = this.VirtualPrinter.BarcodeInfo.Height;
            bool printInterpretationLine = true;
            bool printInterpretationLineAboveCode = false;
            bool calculateAndPrintMod10CheckDigit = false;

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
                printInterpretationLineAboveCode = this.ConvertBoolean(zplDataParts[3]);
            }

            if (zplDataParts.Length > 4)
            {
                calculateAndPrintMod10CheckDigit = this.ConvertBoolean(zplDataParts[4]);
            }

            //The field data are processing in the FieldDataZplCommandAnalyzer
            this.VirtualPrinter.SetNextElementFieldData(new Interleaved2of5BarcodeFieldData
            {
                FieldOrientation = fieldOrientation,
                Height = height,
                PrintInterpretationLine = printInterpretationLine,
                PrintInterpretationLineAboveCode = printInterpretationLineAboveCode,
                CalculateAndPrintMod10CheckDigit = calculateAndPrintMod10CheckDigit
            });

            return null;
        }
    }
}
