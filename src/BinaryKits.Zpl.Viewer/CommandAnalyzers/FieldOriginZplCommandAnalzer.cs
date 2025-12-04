using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldOriginZplCommandAnalzer : ZplCommandAnalyzerBase
    {
        public FieldOriginZplCommandAnalzer() : base("^FO") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            decimal tmpdec;
            int x = 0;
            int y = 0;

            if (zplDataParts.Length > 0 &&
                decimal.TryParse(zplDataParts[0], out tmpdec) &&
                int.MinValue <= tmpdec && tmpdec <= int.MaxValue)
            {
                x = decimal.ToInt32(tmpdec);
            }

            if (zplDataParts.Length > 1 &&
                decimal.TryParse(zplDataParts[1], out tmpdec) &&
                int.MinValue <= tmpdec && tmpdec <= int.MaxValue)
            {
                y = decimal.ToInt32(tmpdec);
            }

            if (zplDataParts.Length > 2)
            {
                FieldJustification fieldJustification = this.ConvertFieldJustification(zplDataParts[2], virtualPrinter);
                virtualPrinter.SetNextElementFieldJustification(fieldJustification);
            }

            if (virtualPrinter.LabelHomePosition != null)
            {
                x += virtualPrinter.LabelHomePosition.X;
                y += virtualPrinter.LabelHomePosition.Y;
            }

            virtualPrinter.SetNextElementPosition(x, y);

            return null;
        }
    }
}
