using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldOriginZplCommandAnalzer : ZplCommandAnalyzerBase
    {
        public FieldOriginZplCommandAnalzer(VirtualPrinter virtualPrinter) : base("^FO", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            int x = 0;
            int y = 0;
            decimal tempdec;

            if (zplDataParts.Length > 0 && 
                decimal.TryParse(zplDataParts[0], out tempdec) && 
                int.MinValue <= tempdec && 
                tempdec <= int.MaxValue
                )
              {
                x = Decimal.ToInt32(tempdec);
              }
            

            if (zplDataParts.Length > 1 && 
                decimal.TryParse(zplDataParts[1], out tempdec) &&
                int.MinValue <= tempdec && 
                tempdec <= int.MaxValue
               )
            {
                y = Decimal.ToInt32(tempdec);
            }

            if (zplDataParts.Length > 2)
            {
                FieldJustification fieldJustification = this.ConvertFieldJustification(zplDataParts[2]);
                this.VirtualPrinter.SetNextElementFieldJustification(fieldJustification);
            }

            if (this.VirtualPrinter.LabelHomePosition != null)
            {
                x += this.VirtualPrinter.LabelHomePosition.X;
                y += this.VirtualPrinter.LabelHomePosition.Y;
            }

            this.VirtualPrinter.SetNextElementPosition(x, y);

            return null;
        }
    }
}
