using BinaryKits.Zpl.Label.Elements;

using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class DownloadFormatCommandAnalyzer : ZplCommandAnalyzerBase
    {
        private static readonly Regex commandRegex = new(@"^\^DF(\w:)?(.*?)?(\..+?)?$", RegexOptions.Compiled);

        public DownloadFormatCommandAnalyzer() : base("^DF") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            Match commandMatch = commandRegex.Match(zplCommand);
            if (commandMatch.Success)
            {
                char storageDevice = commandMatch.Groups[1].Success ? commandMatch.Groups[1].Value[0] : 'R';
                string formatName = commandMatch.Groups[2].Value;

                virtualPrinter.SetNextDownloadFormatName(formatName);
            }

            return null;
        }
    }
}
