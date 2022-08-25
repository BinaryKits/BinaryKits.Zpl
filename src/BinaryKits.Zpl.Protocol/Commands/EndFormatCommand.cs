using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// End Format<br/>
    /// The ^XZ command is the ending(closing) bracket. It indicates the end of a
    /// label format. When this command is received, a label prints.
    /// </summary>
    public class EndFormatCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^XZ";

        /// <summary>
        /// End Format
        /// </summary>
        public EndFormatCommand()
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
            return new EndFormatCommand();
        }
    }
}
