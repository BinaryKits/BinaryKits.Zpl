using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// EAN-13 Bar Code <br/>
    /// The ^BE command is similar to the UPC-A bar code. It is widely used
    /// throughout Europe and Japan in the retail marketplace.
    /// 
    /// The EAN-13 bar code has 12 data characters, one more data character than the UPC-A code.
    /// An EAN-13 symbol contains the same number of bars as the UPC-A, but encodes a 13th digit
    /// into a parity pattern of the left-hand six digits. This 13th digit, in combination with the 12th
    /// digit, represents a country code.
    /// </summary>
    public class Ean13BarCodeCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^BE";

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
        /// Code 39 Bar Code
        /// </summary>
        public Ean13BarCodeCommand()
        { }

        /// <summary>
        /// EAN-13 Bar Code
        /// </summary>
        /// <param name="orientation">Orientation</param>
        /// <param name="barCodeHeight">Bar code height (1 to 32000)</param>
        /// <param name="printInterpretationLine">Print interpretation line</param>
        /// <param name="printInterpretationLineAboveCode">Print interpretation line above code</param>
        public Ean13BarCodeCommand(
            Orientation orientation = Orientation.Normal,
            int? barCodeHeight = null,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false
            )
        {
            this.Orientation = orientation;

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
            return $"{CommandPrefix}{RenderOrientation(this.Orientation)},{this.BarCodeHeight},{RenderBoolean(this.PrintInterpretationLine)},{RenderBoolean(this.PrintInterpretationLineAboveCode)}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new Ean13BarCodeCommand();
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

            return command;
        }

    }
}
