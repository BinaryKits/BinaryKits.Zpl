using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Code 39 Bar Code<br/>
    /// The Code 39 bar code is the standard for many industries, including the U.S. Department of Defense. It is
    /// one of three symbologies identified in the American National Standards Institute(ANSI) standard
    /// MH10.8M-1983. Code 39 is also known as USD-3 Code and 3 of 9 Code.
    /// </summary>
    public class Code39BarCodeCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^B3";

        /// <summary>
        /// Orientation
        /// </summary>
        public Orientation Orientation { get; private set; } = Orientation.Normal;

        /// <summary>
        /// Mod-43 check digit
        /// </summary>
        public bool Mod43CheckDigit { get; private set; } = false;

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
        /// Code 39 Bar Code
        /// </summary>
        public Code39BarCodeCommand()
        { }

        /// <summary>
        /// Code 39 Bar Code
        /// </summary>
        /// <param name="orientation">Orientation</param>
        /// <param name="mod43CheckDigit">Mod-43 check digit</param>
        /// <param name="barCodeHeight">Bar code height (1 to 32000)</param>
        /// <param name="printInterpretationLine">Print interpretation line</param>
        /// <param name="printInterpretationLineAboveCode">Print interpretation line above code</param>
        public Code39BarCodeCommand(
            Orientation orientation = Orientation.Normal,
            bool mod43CheckDigit = false,
            int? barCodeHeight = null,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false
            )
        {
            this.Orientation = orientation;

            this.Mod43CheckDigit = mod43CheckDigit;

            if (ValidateIntParameter(nameof(barCodeHeight), barCodeHeight, 1, 32000))
            {
                this.BarCodeHeight = barCodeHeight.Value;
            }

            this.PrintInterpretationLine = printInterpretationLine;
            this.PrintInterpretationLineAboveCode = printInterpretationLineAboveCode;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{RenderOrientation(this.Orientation)},{RenderBoolean(this.Mod43CheckDigit)},{this.BarCodeHeight},{RenderBoolean(this.PrintInterpretationLine)},{RenderBoolean(this.PrintInterpretationLineAboveCode)}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new Code39BarCodeCommand();
            var zplDataParts = zplCommand.Substring(CommandPrefix.Length).Split(',');

            if (zplDataParts.Length > 0)
            {
                command.Orientation = ConvertOrientation(zplDataParts[0]);
            }

            if (zplDataParts.Length > 1)
            {
                command.Mod43CheckDigit = ConvertBoolean(zplDataParts[1]);
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var barCodeHeight))
                {
                    command.BarCodeHeight = barCodeHeight;
                }
            }

            if (zplDataParts.Length > 3)
            {
                command.PrintInterpretationLine = ConvertBoolean(zplDataParts[3]);
            }

            if (zplDataParts.Length > 4)
            {
                command.PrintInterpretationLineAboveCode = ConvertBoolean(zplDataParts[4]);
            }

            return command;
        }

    }
}
