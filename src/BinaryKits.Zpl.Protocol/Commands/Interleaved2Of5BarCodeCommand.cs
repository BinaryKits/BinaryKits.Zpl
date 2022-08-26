using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Interleaved 2 of 5 Bar Code<br/>
    /// The ^B2 command produces the Interleaved 2 of 5 bar code, a high-density, self-checking, continuous,
    /// numeric symbology.
    /// Each data character for the Interleaved 2 of 5 bar code is composed of five elements: five bars or five
    /// spaces.Of the five elements, two are wide and three are narrow.The bar code is formed by interleaving
    /// characters formed with all spaces into characters formed with all bars. 
    /// </summary>
    public class Interleaved2Of5BarCodeCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^B2";

        /// <summary>
        /// Orientation
        /// </summary>
        public Orientation Orientation { get; private set; } = Orientation.Normal;

        /// <summary>
        /// Bar code height
        /// </summary>
        public int? BarCodeHeight { get; private set; }

        /// <summary>
        /// Print interpretation line
        /// </summary>
        public bool PrintInterpretationLine { get; private set; } = true;

        /// <summary>
        /// Print interpretation line above code
        /// </summary>
        public bool PrintInterpretationLineAboveCode { get; private set; } = false;

        /// <summary>
        /// Calculate and print Mod 10 check digit
        /// </summary>
        public bool CalculateAndPrintMod10CheckDigit { get; private set; } = false;

        /// <summary>
        /// Interleaved 2 of 5 Bar Code
        /// </summary>
        public Interleaved2Of5BarCodeCommand()
        { }

        /// <summary>
        /// Interleaved 2 of 5 Bar Code
        /// </summary>
        /// <param name="orientation">Orientation</param>
        /// <param name="barCodeHeight">Bar code height (1 to 32000)</param>
        /// <param name="printInterpretationLine">Print interpretation line</param>
        /// <param name="printInterpretationLineAboveCode">Print interpretation line above code</param>
        /// <param name="calculateAndPrintMod10CheckDigit">Calculate and print Mod 10 check digit</param>
        public Interleaved2Of5BarCodeCommand(
            Orientation orientation = Orientation.Normal,
            int? barCodeHeight = null,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false,
            bool calculateAndPrintMod10CheckDigit = false)
        {
            this.Orientation = orientation;

            if (ValidateIntParameter(nameof(barCodeHeight), barCodeHeight, 1, 32000))
            {
                this.BarCodeHeight = barCodeHeight.Value;
            }

            this.PrintInterpretationLine = printInterpretationLine;
            this.PrintInterpretationLineAboveCode = printInterpretationLineAboveCode;
            this.CalculateAndPrintMod10CheckDigit = calculateAndPrintMod10CheckDigit;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{RenderOrientation(this.Orientation)},{this.BarCodeHeight},{RenderBoolean(this.PrintInterpretationLine)},{RenderBoolean(this.PrintInterpretationLineAboveCode)},{RenderBoolean(this.CalculateAndPrintMod10CheckDigit)}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new Interleaved2Of5BarCodeCommand();
            var zplDataParts = zplCommand.Substring(CommandPrefix.Length).Split(',');

            if (zplDataParts.Length > 0)
            {
                command.Orientation = ConvertOrientation(zplDataParts[0]);
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var barCodeHeight))
                {
                    command.BarCodeHeight = barCodeHeight;
                }
            }

            if (zplDataParts.Length > 2)
            {
                command.PrintInterpretationLine = ConvertBoolean(zplDataParts[2]);
            }

            if (zplDataParts.Length > 3)
            {
                command.PrintInterpretationLineAboveCode = ConvertBoolean(zplDataParts[3]);
            }

            if (zplDataParts.Length > 4)
            {
                command.CalculateAndPrintMod10CheckDigit = ConvertBoolean(zplDataParts[4]);
            }

            return command;
        }

    }
}
