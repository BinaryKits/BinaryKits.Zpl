using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

using System;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ChangeInternationalFontCommandAnalyzer : ZplCommandAnalyzerBase
    {

        public ChangeInternationalFontCommandAnalyzer() : base("^CI") { }

        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string charSet = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            InternationalFont encoding;
            if (!Enum.TryParse(charSet, out encoding))
            {
                encoding = InternationalFont.ZCP850;
            }

            return new ZplChangeInternationalFont(encoding);
        }

    }
}
