using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class DownloadFormatCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public DownloadFormatCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^DF", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var name = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            this.VirtualPrinter.SetNextDownloadFormatName(name);
            return null;
        }
    }
}
