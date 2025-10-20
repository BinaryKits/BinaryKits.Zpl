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
            bool useDefaultPosition = false;
            // TODO: Field Justification
            //int z = 0;

            // Handle missing coordinates - when coordinates are missing, use default positioning
            if (zplDataParts.Length == 0 || string.IsNullOrEmpty(zplDataParts[0]))
            {
                // No coordinates specified - use default position
                useDefaultPosition = true;
            }
            else
            {
                if (int.TryParse(zplDataParts[0], out tmpint))
                {
                    x = tmpint;
                }
                else
                {
                    // Empty or invalid x coordinate - use default position
                    useDefaultPosition = true;
                }
            }

            if (zplDataParts.Length > 1 && !string.IsNullOrEmpty(zplDataParts[1]))
            {
                if (int.TryParse(zplDataParts[1], out tmpint))
                {
                    y = tmpint;
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
                var fieldJustification = ConvertFieldJustification(zplDataParts[2]);
                this.VirtualPrinter.SetNextElementFieldJustification(fieldJustification);
            }

            if (this.VirtualPrinter.LabelHomePosition != null)
            {
                x += this.VirtualPrinter.LabelHomePosition.X;
                y += this.VirtualPrinter.LabelHomePosition.Y;
            }

            if (useDefaultPosition)
            {
                this.VirtualPrinter.SetNextElementUseDefaultPosition(true);
            }
            else
            {
                this.VirtualPrinter.SetNextElementPosition(x, y, calculateFromBottom: true);
            }

            return null;
        }
    }
}
