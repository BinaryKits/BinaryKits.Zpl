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

            //The field data are processing in the FieldDataZplCommandAnalyzer
            this.VirtualPrinter.SetNextElementFieldData(new DataMatrixFieldData
            {
                FieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]),
                Height = this.VirtualPrinter.BarcodeInfo.Height,
            });

            return null;
        }
    }
}
