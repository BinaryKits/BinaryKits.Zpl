namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Field Separator<br/>
    /// The ^FS command denotes the end of the field definition. Alternatively, ^FS command can also be issued
    /// as a single ASCII control code SI(Control-O, hexadecimal 0F).
    /// </summary>
    public class FieldSeperatorCommand : CommandBase
    {
        /// <summary>
        /// Field Separator
        /// </summary>
        public FieldSeperatorCommand() : base("^FS")
        { }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
        }
    }
}
