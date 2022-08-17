using BinaryKits.Zpl.Label.Elements;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class RecallGraphicZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        private static readonly Regex commandRegex = new Regex(@"^\^XG(\w:)?(.*?\..+?)(?:,(\d*))?(?:,(\d*))?$", RegexOptions.Compiled);

        public RecallGraphicZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^XG", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var x = 0;
            var y = 0;
            var bottomToTop = false;

            if (this.VirtualPrinter.NextElementPosition != null)
            {
                x = this.VirtualPrinter.NextElementPosition.X;
                y = this.VirtualPrinter.NextElementPosition.Y;
                bottomToTop = this.VirtualPrinter.NextElementPosition.CalculateFromBottom;
            }

            var commandMatch = commandRegex.Match(zplCommand);

            if (commandMatch.Success)
            {
                var storageDevice = commandMatch.Groups[1].Success ? commandMatch.Groups[1].Value[0] : 'R';
                var imageName = commandMatch.Groups[2].Value;
                var magnificationFactorX = commandMatch.Groups[3].Success ? int.Parse(commandMatch.Groups[3].Value) : 1;
                var magnificationFactorY = commandMatch.Groups[4].Success ? int.Parse(commandMatch.Groups[4].Value) : 1;

                return new ZplRecallGraphic(x, y, storageDevice, imageName, magnificationFactorX, magnificationFactorY, bottomToTop);
            }

            return null;
        }
    }
}
