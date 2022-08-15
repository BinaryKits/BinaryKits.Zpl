using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class CodeEAN13BarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public CodeEAN13BarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^BE", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            var height = this.VirtualPrinter.BarcodeInfo.Height;
            var printInterpretationLine = true;
            var printInterpretationLineAboveCode = false;

            if (zplDataParts.Length > 1)
            {
                if (!string.IsNullOrEmpty(zplDataParts[1]))
                {
                    _ = int.TryParse(zplDataParts[1], out height);
                }
            }
            if (zplDataParts.Length > 2)
            {
                printInterpretationLine = this.ConvertBoolean(zplDataParts[2], "Y");
            }
            if (zplDataParts.Length > 3)
            {
                printInterpretationLineAboveCode = this.ConvertBoolean(zplDataParts[3]);
            }
          

            //The field data are processing in the FieldDataZplCommandAnalyzer
            this.VirtualPrinter.SetNextElementFieldData(new CodeEAN13BarcodeFieldData
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
