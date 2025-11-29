using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class GraphicSymbolZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public GraphicSymbolZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^GS", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            FieldOrientation fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);

            int tmpint;
            int height = this.VirtualPrinter.FontHeight;
            int width = this.VirtualPrinter.FontWidth;

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                height = tmpint;
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                width = tmpint;
            }

            //The field data are processing in the FieldDataZplCommandAnalyzer
            this.VirtualPrinter.SetNextElementFieldData(new GraphicSymbolFieldData
            {
                FieldOrientation = fieldOrientation,
                Height = height,
                Width = width
            });

            return null;
        }
    }
}
