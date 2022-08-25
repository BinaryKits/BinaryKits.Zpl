using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Start Format<br/>
    /// The ^XA command is used at the beginning of ZPL II code. It is the opening
    /// bracket and indicates the start of a new label format.
    /// </summary>
    public class StartFormatCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^XA";

        /// <summary>
        /// Start Format
        /// </summary>
        public StartFormatCommand()
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
            return new StartFormatCommand();
        }
    }
}
