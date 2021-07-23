using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class Code128BarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public Code128BarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^BC", virtualPrinter)
        { }

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            var zplCommandData = zplCommandStructure.CurrentCommand.Substring(this.PrinterCommandPrefix.Length);

            var zplDataParts = zplCommandData.Split(',');

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            var height = this.VirtualPrinter.BarcodeInfo.Height;
            var printInterpretationLine = true;
            var printInterpretationLineAboveCode = false;
            var uccCheckDigit = false;

            if (zplDataParts.Length > 1)
            {
                _ = int.TryParse(zplDataParts[1], out height);
            }
            if (zplDataParts.Length > 2)
            {
                printInterpretationLine = !this.ConvertBoolean(zplDataParts[2]);
            }
            if (zplDataParts.Length > 3)
            {
                printInterpretationLineAboveCode = this.ConvertBoolean(zplDataParts[3]);
            }
            if (zplDataParts.Length > 4)
            {
                uccCheckDigit = this.ConvertBoolean(zplDataParts[4]);
            }

            this.VirtualPrinter.SetNextFieldDataElement(new Code128BarcodeFieldData
            {
                FieldOrientation = fieldOrientation,
                Height = height,
                PrintInterpretationLine = printInterpretationLine,
                PrintInterpretationLineAboveCode = printInterpretationLineAboveCode,
                UccCheckDigit = uccCheckDigit
            });

            return null;
        }
    }
}
