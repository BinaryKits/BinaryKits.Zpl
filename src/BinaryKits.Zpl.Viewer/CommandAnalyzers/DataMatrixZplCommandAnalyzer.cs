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

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            var height = this.VirtualPrinter.BarcodeInfo.Height;

            if (zplDataParts.Length > 1)
            {
                if (!string.IsNullOrEmpty(zplDataParts[1]))
                {
                    _ = int.TryParse(zplDataParts[1], out height);
                }
            }

            //The field data are processing in the FieldDataZplCommandAnalyzer
            this.VirtualPrinter.SetNextElementFieldData(new DataMatrixFieldData
            {
                FieldOrientation = fieldOrientation,
                Height = height
            });

            return null;
        }
    }
}
