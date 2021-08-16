using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class CommentZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public CommentZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FX", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var comment = zplCommand.Substring(this.PrinterCommandPrefix.Length);
            this.VirtualPrinter.AddComment(comment);

            return null;
        }
    }
}
