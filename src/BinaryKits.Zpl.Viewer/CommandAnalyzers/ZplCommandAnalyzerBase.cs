using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public abstract class ZplCommandAnalyzerBase : IZplCommandAnalyzer
    {
        public string PrinterCommandPrefix { get; private set; }

        public ZplCommandAnalyzerBase(string prefix)
        {
            this.PrinterCommandPrefix = prefix;
        }

        ///<inheritdoc/>
        public bool CanAnalyze(string zplLine)
        {
            return zplLine.StartsWith(this.PrinterCommandPrefix);
        }

        ///<inheritdoc/>
        public abstract ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage);

        protected string[] SplitCommand(string zplCommand, int dataStartIndex = 0)
        {
            string zplCommandData = zplCommand.Substring(this.PrinterCommandPrefix.Length + dataStartIndex);
            return zplCommandData.Trim().Split(',');
        }

        protected FieldOrientation ConvertFieldOrientation(string fieldOrientation, VirtualPrinter virtualPrinter)
        {
            return fieldOrientation switch
            {
                "N" => FieldOrientation.Normal,
                "R" => FieldOrientation.Rotated90,
                "I" => FieldOrientation.Rotated180,
                "B" => FieldOrientation.Rotated270,
                _ => virtualPrinter.FieldOrientation,
            };
        }

        protected QualityLevel ConvertQualityLevel(string qualityLevel)
        {
            return qualityLevel switch
            {
                "0" => QualityLevel.ECC0,
                "50" => QualityLevel.ECC50,
                "80" => QualityLevel.ECC80,
                "100" => QualityLevel.ECC100,
                "140" => QualityLevel.ECC140,
                "200" => QualityLevel.ECC200,
                _ => QualityLevel.ECC0
            };
        }

        protected FieldJustification ConvertFieldJustification(string fieldJustification,VirtualPrinter virtualPrinter)
        {
            return fieldJustification switch
            {
                "0" => FieldJustification.Left,
                "1" => FieldJustification.Right,
                "2" => FieldJustification.Auto,
                _ => virtualPrinter.FieldJustification,
            };
        }

        protected ErrorCorrectionLevel ConvertErrorCorrectionLevel(string errorCorrection)
        {
            return errorCorrection switch
            {
                "H" => ErrorCorrectionLevel.UltraHighReliability,
                "Q" => ErrorCorrectionLevel.HighReliability,
                "M" => ErrorCorrectionLevel.Standard,
                "L" => ErrorCorrectionLevel.HighDensity,
                _ => ErrorCorrectionLevel.Standard,
            };
        }

        protected bool ConvertBoolean(string yesOrNo, string defaultValue = "N")
        {
            return (!string.IsNullOrEmpty(yesOrNo) ? yesOrNo : defaultValue) == "Y";
        }

        protected int IndexOfNthCharacter(string input, int occurranceToFind, char charToFind)
        {
            int index = -1;
            for (int i = 0; i < occurranceToFind; i++)
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
