using BinaryKits.Zpl.Label.Elements;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class RecallGraphicZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        private static readonly Regex commandRegex = new(@"^\^XG(\w:)?(.*?\..+?)(?:,(\d*))?(?:,(\d*))?$", RegexOptions.Compiled);

        public RecallGraphicZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^XG", virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            int x = 0;
            int y = 0;
            bool bottomToTop = false;
            bool useDefaultPosition = false;

            if (this.VirtualPrinter.NextElementPosition != null)
            {
                x = this.VirtualPrinter.NextElementPosition.X;
                y = this.VirtualPrinter.NextElementPosition.Y;
                bottomToTop = this.VirtualPrinter.NextElementPosition.CalculateFromBottom;
                useDefaultPosition = this.VirtualPrinter.NextElementPosition.UseDefaultPosition;
            }

            Match commandMatch = commandRegex.Match(zplCommand);

            if (commandMatch.Success)
            {
                char storageDevice = commandMatch.Groups[1].Success ? commandMatch.Groups[1].Value[0] : 'R';
                string imageName = commandMatch.Groups[2].Value;
                int magnificationFactorX = commandMatch.Groups[3].Success ? int.Parse(commandMatch.Groups[3].Value) : 1;
                int magnificationFactorY = commandMatch.Groups[4].Success ? int.Parse(commandMatch.Groups[4].Value) : 1;

                return new ZplRecallGraphic(x, y, storageDevice, imageName, magnificationFactorX, magnificationFactorY, bottomToTop, useDefaultPosition);
            }

            return null;
        }
    }
}
