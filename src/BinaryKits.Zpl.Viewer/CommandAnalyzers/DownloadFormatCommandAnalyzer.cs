using BinaryKits.Zpl.Label.Elements;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class DownloadFormatCommandAnalyzer : ZplCommandAnalyzerBase
    {
        private static readonly Regex commandRegex = new(@"^\^DF(\w:)?(.*?)?(\..+?)?$", RegexOptions.Compiled);

        public DownloadFormatCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^DF", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            Match commandMatch = commandRegex.Match(zplCommand);
            if (commandMatch.Success)
            {
                char storageDevice = commandMatch.Groups[1].Success ? commandMatch.Groups[1].Value[0] : 'R';
                string formatName = commandMatch.Groups[2].Value;

                this.VirtualPrinter.SetNextDownloadFormatName(formatName);
            }

            return null;
        }
    }
}
