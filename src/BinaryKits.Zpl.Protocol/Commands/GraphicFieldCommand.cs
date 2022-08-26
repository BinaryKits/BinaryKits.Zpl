using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Graphic Field<br/>
    /// The ^GF command allows you to download graphic field data directly into the printer’s bitmap storage area.
    /// This command follows the conventions for any other field, meaning a field orientation is included.The
    /// graphic field data can be placed at any location within the bitmap space.
    /// </summary>
    public class GraphicFieldCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^GF";

        /// <summary>
        /// Compression type
        /// </summary>
        public char CompressionType { get; private set; } = 'A';

        /// <summary>
        /// Binary byte count
        /// </summary>
        public int BinaryByteCount { get; private set; }

        /// <summary>
        /// Graphic field count
        /// </summary>
        public int GraphicFieldCount { get; private set; }

        /// <summary>
        /// Bytes per row
        /// </summary>
        public int BytesPerRow { get; private set; }

        /// <summary>
        /// Data
        /// </summary>
        public string Data { get; private set; }

        /// <summary>
        /// Graphic Field
        /// </summary>
        public GraphicFieldCommand()
        { }

        /// <summary>
        /// Graphic Field
        /// </summary>
        /// <param name="compressionType">Compression type</param>
        /// <param name="binaryByteCount">Binary byte count</param>
        /// <param name="graphicFieldCount">Graphic field count</param>
        /// <param name="bytesPerRow">Bytes per row</param>
        /// <param name="data">Data</param>
        public GraphicFieldCommand(
            char compressionType = 'A',
            int binaryByteCount = 1,
            int graphicFieldCount = 1,
            int bytesPerRow = 1,
            string data = null)
        {
            this.CompressionType = compressionType;

            if (ValidateIntParameter(nameof(binaryByteCount), binaryByteCount, 1, 99999))
            {
                this.BinaryByteCount = binaryByteCount;
            }

            if (ValidateIntParameter(nameof(graphicFieldCount), graphicFieldCount, 1, 99999))
            {
                this.GraphicFieldCount = graphicFieldCount;
            }

            if (ValidateIntParameter(nameof(bytesPerRow), bytesPerRow, 1, 99999))
            {
                this.BytesPerRow = bytesPerRow;
            }

            this.Data = data;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{this.CompressionType},{this.BinaryByteCount},{this.GraphicFieldCount},{this.BytesPerRow},{this.Data}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new GraphicFieldCommand();
            var zplDataParts = zplCommand.Substring(CommandPrefix.Length).Split(new char[] { ',' }, 5);

            if (zplDataParts.Length > 0)
            {
                command.CompressionType = zplDataParts[0][0];
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var binaryByteCount))
                {
                    command.BinaryByteCount = binaryByteCount;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var graphicFieldCount))
                {
                    command.GraphicFieldCount = graphicFieldCount;
                }
            }

            if (zplDataParts.Length > 3)
            {
                if (int.TryParse(zplDataParts[3], out var bytesPerRow))
                {
                    command.BytesPerRow = bytesPerRow;
                }
            }

            if (zplDataParts.Length > 4)
            {
                command.Data = zplDataParts[4];
            }

            return command;
        }
    }
}
