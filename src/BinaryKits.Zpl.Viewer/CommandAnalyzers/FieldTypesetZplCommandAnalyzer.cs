using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldTypesetZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldTypesetZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FT", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            int x = 0;
            int y = 0;
            // TODO: Field Justification
            //int z = 0;

            if (zplDataParts.Length > 0 && int.TryParse(zplDataParts[0], out tmpint))
            {
                x = tmpint;
            }

            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                y = tmpint;
            }

            if (zplDataParts.Length > 2)
            {
                var fieldJustification = ConvertFieldJustification(zplDataParts[2]);
                this.VirtualPrinter.SetNextElementFieldJustification(fieldJustification);
            }

            if (this.VirtualPrinter.LabelHomePosition != null)
            {
                x += this.VirtualPrinter.LabelHomePosition.X;
                y += this.VirtualPrinter.LabelHomePosition.Y;
            }

            this.VirtualPrinter.SetNextElementPosition(x, y, calculateFromBottom: true);

            return null;
        }
    }
}
