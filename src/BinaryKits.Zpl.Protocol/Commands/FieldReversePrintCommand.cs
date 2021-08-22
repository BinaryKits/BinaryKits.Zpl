namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Field Reverse Print<br/>
    /// The ^FR command allows a field to appear as white over black or black over white. When printing a field
    /// and the ^FR command has been used, the color of the output is the reverse of its background.
    /// </summary>
    public class FieldReversePrintCommand : CommandBase
    {
        /// <summary>
        /// Field Reverse Print
        /// </summary>
        public FieldReversePrintCommand() : base("^FR")
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
