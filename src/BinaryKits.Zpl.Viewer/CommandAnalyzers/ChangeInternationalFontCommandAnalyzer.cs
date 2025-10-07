using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using System;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class ChangeInternationalFontCommandAnalyzer : ZplCommandAnalyzerBase
    {

        public ChangeInternationalFontCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^CI", virtualPrinter) { }

        public override ZplElementBase Analyze(string zplCommand)
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
