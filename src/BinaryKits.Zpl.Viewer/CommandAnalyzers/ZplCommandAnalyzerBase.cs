using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public abstract class ZplCommandAnalyzerBase : IZplCommandAnalyzer
    {
        public string PrinterCommandPrefix { get; private set; }
        public VirtualPrinter VirtualPrinter { get; private set; }

        public ZplCommandAnalyzerBase(string prefix, VirtualPrinter virtualPrinter)
        {
            this.PrinterCommandPrefix = prefix;
            this.VirtualPrinter = virtualPrinter;
        }

        public bool CanAnalyze(string zplLine)
        {
            return zplLine.StartsWith(this.PrinterCommandPrefix);
        }

        public abstract ZplElementBase Analyze(ZplCommandStructure zplCommandStructure);
    }
}
