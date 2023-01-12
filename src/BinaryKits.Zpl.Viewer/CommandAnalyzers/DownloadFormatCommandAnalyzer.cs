using BinaryKits.Zpl.Label.Elements;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class DownloadFormatCommandAnalyzer : ZplCommandAnalyzerBase
    {
        private static readonly Regex commandRegex = new Regex(@"^\^DF(\w:)?(.*?)?(\..+?)?$", RegexOptions.Compiled);

        public DownloadFormatCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^DF", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var commandMatch = commandRegex.Match(zplCommand);
            if (commandMatch.Success)
            {
                var storageDevice = commandMatch.Groups[1].Success ? commandMatch.Groups[1].Value[0] : 'R';
                var formatName = commandMatch.Groups[2].Value;

                this.VirtualPrinter.SetNextDownloadFormatName(formatName);
            }

            return null;
        }
    }
}
