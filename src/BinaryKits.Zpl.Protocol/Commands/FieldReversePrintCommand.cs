using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Field Reverse Print<br/>
    /// The ^FR command allows a field to appear as white over black or black over white. When printing a field
    /// and the ^FR command has been used, the color of the output is the reverse of its background.
    /// </summary>
    public class FieldReversePrintCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^FR";

        /// <summary>
        /// Field Reverse Print
        /// </summary>
        public FieldReversePrintCommand()
        { }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            return new FieldReversePrintCommand();
        }

    }
}
