using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class CommentZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public CommentZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FX", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            string comment = zplCommand.Substring(this.PrinterCommandPrefix.Length);
            this.VirtualPrinter.AddComment(comment);

            return null;
        }
    }
}
