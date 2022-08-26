using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Field Data<br/>
    /// The ^FD command defines the data string for a field. The field data can be any printable character except
    /// those used as command prefixes(^ and ~).
    /// </summary>
    public class FieldDataCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^FD";

        /// <summary>
        /// Data to be printed
        /// </summary>
        public string Data { get; private set; }

        /// <summary>
        /// Field Data
        /// </summary>
        public FieldDataCommand()
        { }

        /// <summary>
        /// Field Data
        /// </summary>
        /// <param name="data">Data to be printed</param>
        public FieldDataCommand(string data = null)
        {
            this.Data = data;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{this.Data}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new FieldDataCommand();
            var zplCommandData = zplCommand.Substring(CommandPrefix.Length);

            command.Data = zplCommandData;

            return command;
        }

    }
}
