using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Label Reverse Print<br/>
    /// The ^LR command reverses the printing of all fields in the label format. It
    /// allows a field to appear as white over black or black over white.
    /// Using the ^LR is identical to placing an ^FR command in all current and subsequent fields.
    /// </summary>
    public class LabelReversePrintCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^LR";

        /// <summary>
        /// Reverse print all fields
        /// </summary>
        public bool ReversePrintAllFields { get; private set; } = false;

        /// <summary>
        /// Label Reverse Print
        /// </summary>
        public LabelReversePrintCommand()
        { }

        /// <summary>
        /// Label Reverse Print
        /// </summary>
        /// <param name="reversePrintAllFields">Reverse print all fields</param>
        public LabelReversePrintCommand(bool reversePrintAllFields)
        {
            this.ReversePrintAllFields = reversePrintAllFields;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{RenderBoolean(this.ReversePrintAllFields)}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new LabelReversePrintCommand();
            var zplCommandData = zplCommand.Substring(CommandPrefix.Length);

            command.ReversePrintAllFields = ConvertBoolean(zplCommandData);

            return command;
        }

    }
}
