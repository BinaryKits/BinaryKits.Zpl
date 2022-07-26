using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public abstract class ZplCommandAnalyzerBase : IZplCommandAnalyzer
    {
        public string PrinterCommandPrefix { get; private set; }
        public VirtualPrinter VirtualPrinter { get; private set; }

        public ZplCommandAnalyzerBase(string prefix, VirtualPrinter virtualPrinter)
        {
            this.PrinterCommandPrefix = prefix;
            this.VirtualPrinter = virtualPrinter;
        }

        public bool CanAnalyze(string zplLine)
        {
            return zplLine.StartsWith(this.PrinterCommandPrefix);
        }

        public abstract ZplElementBase Analyze(string zplCommand);

        protected string[] SplitCommand(string zplCommand, int dataStartIndex = 0)
        {
            var zplCommandData = zplCommand.Substring(this.PrinterCommandPrefix.Length + dataStartIndex);
            return zplCommandData.Split(',');
        }

        protected FieldOrientation ConvertFieldOrientation(string fieldOrientation)
        {
            switch (fieldOrientation)
            {
                case "N":
                    return FieldOrientation.Normal;
                case "R":
                    return FieldOrientation.Rotated90;
                case "I":
                    return FieldOrientation.Rotated180;
                case "B":
                    return FieldOrientation.Rotated270;
            }

            return FieldOrientation.Normal;
        }

        protected bool ConvertBoolean(string yesOrNo, string defaultValue = "N")
        {
            return (!string.IsNullOrEmpty(yesOrNo) ? yesOrNo : defaultValue) == "Y";
        }

        protected int IndexOfNthCharacter(string input, int occurranceToFind, char charToFind)
        {
            var index = -1;
            for (var i = 0; i < occurranceToFind; i++)
            {
                index = input.IndexOf(charToFind, index + 1);

                if (index == -1)
                {
                    break;
                }
            }

            return index;
        }
    }
}
