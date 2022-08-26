using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Comment<br/>
    /// The ^FX command is useful when you want to add non-printing informational comments or statements
    /// within a label format.Any data after the ^FX command up to the next caret (^) or tilde(~) command does
    /// not have any effect on the label format.Therefore, you should avoid using the caret(^) or tilde(~)
    /// commands within the ^FX statement.
    /// </summary>
    public class CommentCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^FX";

        /// <summary>
        /// Non printing comment
        /// </summary>
        public string NonPrintingComment { get; private set; }

        /// <summary>
        /// Comment
        /// </summary>
        public CommentCommand()
        { }

        /// <summary>
        /// Comment
        /// </summary>
        /// <param name="nonPrintingComment">non printing comment</param>
        public CommentCommand(string nonPrintingComment = null)
        {
            this.NonPrintingComment = nonPrintingComment;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{this.NonPrintingComment}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new CommentCommand();
            var zplCommandData = zplCommand.Substring(CommandPrefix.Length);

            command.NonPrintingComment = zplCommandData;

            return command;
        }

    }
}
