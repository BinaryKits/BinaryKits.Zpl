using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class Code39BarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public Code39BarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^B3", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            bool mod43CheckDigit = false;
            int height = this.VirtualPrinter.BarcodeInfo.Height;
            bool printInterpretationLine = true;
            bool printInterpretationLineAboveCode = false;

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            if (zplDataParts.Length > 1)
            {
                mod43CheckDigit = this.ConvertBoolean(zplDataParts[1]);
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                height = tmpint;
            }

            if (zplDataParts.Length > 3)
            {
                printInterpretationLine = this.ConvertBoolean(zplDataParts[3], "Y");
            }

            if (zplDataParts.Length > 4)
            {
                printInterpretationLineAboveCode = this.ConvertBoolean(zplDataParts[4]);
            }

            //The field data are processing in the FieldDataZplCommandAnalyzer
            this.VirtualPrinter.SetNextElementFieldData(new Code39BarcodeFieldData
            {
                FieldOrientation = fieldOrientation,
                Height = height,
                PrintInterpretationLine = printInterpretationLine,
                PrintInterpretationLineAboveCode = printInterpretationLineAboveCode,
                Mod43CheckDigit = mod43CheckDigit
            });

            return null;
        }
    }
}
