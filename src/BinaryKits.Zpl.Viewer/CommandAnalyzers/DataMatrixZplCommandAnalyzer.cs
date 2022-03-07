using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class DataMatrixZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public DataMatrixZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^BX", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            //var mod43CheckDigit = false;
            var height = this.VirtualPrinter.BarcodeInfo.Height;
            //var printInterpretationLine = true;
            //var printInterpretationLineAboveCode = false;

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            //if (zplDataParts.Length > 1)
            //{
            //    mod43CheckDigit = this.ConvertBoolean(zplDataParts[1]);
            //}
            //if (zplDataParts.Length > 2)
            //{
            //    _ = int.TryParse(zplDataParts[2], out height);
            //}
            //if (zplDataParts.Length > 3)
            //{
            //    printInterpretationLine = !this.ConvertBoolean(zplDataParts[3]);
            //}
            //if (zplDataParts.Length > 4)
            //{
            //    printInterpretationLineAboveCode = this.ConvertBoolean(zplDataParts[4]);
            //}

            //The field data are processing in the FieldDataZplCommandAnalyzer
            this.VirtualPrinter.SetNextElementFieldData(new DataMatrixFieldData
            {
                FieldOrientation = fieldOrientation,
                Height = height,
            });

            return null;
        }
    }
}
