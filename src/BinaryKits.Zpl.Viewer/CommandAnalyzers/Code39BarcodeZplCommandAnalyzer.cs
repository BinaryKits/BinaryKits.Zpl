using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class Code39BarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public Code39BarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^B3", virtualPrinter)
        { }

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            var x = this.VirtualPrinter.NextElementPosition.X;
            var y = this.VirtualPrinter.NextElementPosition.Y;

            this.VirtualPrinter.ClearNextElementPosition();

            var zplCommandData = zplCommandStructure.CurrentCommand.Substring(this.PrinterCommandPrefix.Length);

            var zplDataParts = zplCommandData.Split(',');

            var orientation = zplDataParts[0];
            var mod43CheckDigit = zplDataParts[1] == "Y" ? true : false;
            _ = int.TryParse(zplDataParts[2], out var height);
            var printInterpretationLine = zplDataParts[3] == "Y" ? false : true;
            var printInterpretationLineAboveCode = zplDataParts[4] == "Y" ? true : false;

            return new ZplBarcode39("123456", x, y, height, Label.FieldOrientation.Normal, printInterpretationLine, printInterpretationLineAboveCode, mod43CheckDigit);
        }
    }
}
