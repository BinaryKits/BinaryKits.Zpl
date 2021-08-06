using BinaryKits.Zpl.Label.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class GraphicFieldZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public GraphicFieldZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^GF", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplCommandData = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            var zplDataParts = zplCommandData.Split(',');



            return null;
        }
    }
}
