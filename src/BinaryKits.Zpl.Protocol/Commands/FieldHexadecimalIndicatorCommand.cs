using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Field Hexadecimal Indicator<br/>
    /// The ^FH command allows you to enter the hexadecimal value for any
    /// character directly into the ^FD statement. The ^FH command must precede each ^FD
    /// command that uses hexadecimals in its field.
    /// </summary>
    public class FieldHexadecimalIndicatorCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^FH";

        /// <summary>
        /// Hexadecimal indicator
        /// </summary>
        public char HexidecimalIndicator { get; private set; } = '_';

        /// <summary>
        /// Field Hexadecimal Indicator
        /// </summary>
        public FieldHexadecimalIndicatorCommand()
        { }

        /// <summary>
        /// Field Hexadecimal Indicator
        /// </summary>
        /// <param name="hexadecimalIndicator">Hexadecimal indicator</param>
        public FieldHexadecimalIndicatorCommand(char hexadecimalIndicator)
        {
            this.HexidecimalIndicator = hexadecimalIndicator;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{this.HexidecimalIndicator}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new FieldHexadecimalIndicatorCommand();
            var zplCommandData = zplCommand.Substring(CommandPrefix.Length);

            if (zplCommandData.Length > 0)
            {
                command.HexidecimalIndicator = zplCommandData[0];
            }

            return command;
        }

    }
}
