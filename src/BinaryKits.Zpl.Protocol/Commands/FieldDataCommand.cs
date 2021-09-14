namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Field Data<br/>
    /// The ^FD command defines the data string for a field. The field data can be any printable character except
    /// those used as command prefixes(^ and ~).
    /// </summary>
    public class FieldDataCommand : CommandBase
    {
        /// <summary>
        /// Data to be printed
        /// </summary>
        public string Data { get; private set; }

        /// <summary>
        /// Field Data
        /// </summary>
        public FieldDataCommand() : base("^FD")
        { }

        /// <summary>
        /// Field Data
        /// </summary>
        /// <param name="data">Data to be printed</param>
        public FieldDataCommand(string data = null)
            : this()
        {
            this.Data = data;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.Data}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            this.Data = zplCommand.Substring(this.CommandPrefix.Length);
        }
    }
}
