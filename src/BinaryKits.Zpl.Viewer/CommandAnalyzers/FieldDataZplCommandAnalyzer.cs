using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldDataZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldDataZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FD", virtualPrinter)
        { }

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            var x = this.VirtualPrinter.NextElementPosition.X;
            var y = this.VirtualPrinter.NextElementPosition.Y;

            this.VirtualPrinter.ClearNextElementPosition();

            var zplCommandData = zplCommandStructure.CurrentCommand.Substring(this.PrinterCommandPrefix.Length);
            var text = zplCommandData;

            if (this.VirtualPrinter.NextFont != null)
            {
                var fontWidth1 = this.VirtualPrinter.NextFont.FontWidth;
                var fontHeight1 = this.VirtualPrinter.NextFont.FontHeight;
                var fontName1 = this.VirtualPrinter.NextFont.FontName;
                var fieldOrientation = this.VirtualPrinter.NextFont.FieldOrientation;

                this.VirtualPrinter.ClearNextFont();

                return new ZplTextField(text, x, y, new ZplFont(fontWidth1, fontHeight1, fontName1, fieldOrientation));
            }

            var fontWidth = this.VirtualPrinter.FontWidth;
            var fontHeight = this.VirtualPrinter.FontHeight;
            var fontName = this.VirtualPrinter.FontName;

            return new ZplTextField(text, x, y, new ZplFont(fontWidth, fontHeight, fontName));
        }
    }
}
