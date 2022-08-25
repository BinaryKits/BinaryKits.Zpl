using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Field Separator<br/>
    /// The ^FS command denotes the end of the field definition. Alternatively, ^FS command can also be issued
    /// as a single ASCII control code SI(Control-O, hexadecimal 0F).
    /// </summary>
    public class FieldSeparatorCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^FS";

        /// <summary>
        /// Field Separator
        /// </summary>
        public FieldSeparatorCommand()
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
            return new FieldSeparatorCommand();
        }
    }
}
