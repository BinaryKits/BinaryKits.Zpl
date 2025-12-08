using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldTypesetZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldTypesetZplCommandAnalyzer() : base("^FT") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            decimal tmpdec;
            int x = 0;
            int y = 0;
            bool useDefaultPosition = false;

            // Handle missing coordinates - when coordinates are missing, use default positioning
            if (zplDataParts.Length == 0 || string.IsNullOrEmpty(zplDataParts[0]))
            {
                // No coordinates specified - use default position
                useDefaultPosition = true;
            }
            else
            {
                if (decimal.TryParse(zplDataParts[0], out tmpdec) &&
                    int.MinValue <= tmpdec && tmpdec <= int.MaxValue)
                {
                    x = decimal.ToInt32(tmpdec);
                }
                else
                {
                    // Empty or invalid x coordinate - use default position
                    useDefaultPosition = true;
                }
            }

            if (zplDataParts.Length > 1 && !string.IsNullOrEmpty(zplDataParts[1]))
            {
                if (decimal.TryParse(zplDataParts[1], out tmpdec) &&
                    int.MinValue <= tmpdec && tmpdec <= int.MaxValue)
                {
                    y = decimal.ToInt32(tmpdec);
                }
                else if (!useDefaultPosition)
                {
                    // Invalid y coordinate but x was valid - use default position
                    useDefaultPosition = true;
                }
            }
            else if (zplDataParts.Length > 1)
            {
                // Empty y coordinate - use default position
                useDefaultPosition = true;
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

            virtualPrinter.SetNextElementPosition(x, y, true, useDefaultPosition);

            return null;
        }
    }
}
