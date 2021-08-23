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
        /// <summary>
        /// Non printing comment
        /// </summary>
        public string NonPrintingComment { get; private set; }

        /// <summary>
        /// Comment
        /// </summary>
        public CommentCommand() : base("^FX")
        { }

        /// <summary>
        /// Comment
        /// </summary>
        /// <param name="nonPrintingComment">non printing comment</param>
        public CommentCommand(string nonPrintingComment = null)
            : this()
        {
            this.NonPrintingComment = nonPrintingComment;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.NonPrintingComment}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            this.NonPrintingComment = zplCommand.Substring(this.CommandPrefix.Length);
        }
    }
}
