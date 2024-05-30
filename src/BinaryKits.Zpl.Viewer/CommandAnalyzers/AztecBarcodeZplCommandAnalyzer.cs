using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class AztecBarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public AztecBarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^BO", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            int magnificationFactor = 2;
            bool extendedChannel = false;
            int errorControl = 0;
            bool menuSymbol = false;
            int symbolCount = 1;
            string idField = null;

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                magnificationFactor = tmpint;
            }

            if (zplDataParts.Length > 2)
            {
                extendedChannel = ConvertBoolean(zplDataParts[2]);
            }

            if (zplDataParts.Length > 3 && int.TryParse(zplDataParts[3], out tmpint))
            {
                errorControl = tmpint;
            }

            if (zplDataParts.Length > 4)
            {
                menuSymbol = ConvertBoolean(zplDataParts[4]);
            }

            if (zplDataParts.Length > 5 && int.TryParse(zplDataParts[5], out tmpint))
            {
                symbolCount = tmpint;
            }

            if (zplDataParts.Length > 6)
            {
                idField = zplDataParts[6];
            }

            this.VirtualPrinter.SetNextElementFieldData(new AztecBarcodeFieldData
            {
                FieldOrientation = fieldOrientation,
                MagnificationFactor = magnificationFactor,
                ExtendedChannel = extendedChannel,
                ErrorControl = errorControl,
                MenuSymbol = menuSymbol,
                SymbolCount = symbolCount,
                IdField = idField
            });

            return null;
        }

    }
}
