using System;
using System.Globalization;
using System.Reflection;
using static System.FormattableString;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Data Matrix Bar Code<br/>
    /// The ^BX command creates a two-dimensional matrix symbology made up of
    /// square modules arranged within a perimeter finder pattern.
    /// </summary>
    public class DataMatrixBarCodeCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^BX";

        /// <summary>
        /// Orientation
        /// </summary>
        public Orientation Orientation { get; private set; } = Orientation.Normal;

        /// <summary>
        /// Element height. Accepted Values: 1 to the width of the label
        /// </summary>
        public int? ElementHeight { get; private set; }

        /// <summary>
        /// Quality level. Accepted Values: 0, 50, 80, 100, 140, 200
        /// </summary>
        public int QualityLevel { get; private set; } = 0;

        /// <summary>
        /// Columns to encode. Accepted Values: 9 to 49
        /// </summary>
        public int? ColumnsToEncode { get; private set; }

        /// <summary>
        /// Rows to encode. Accepted Values: 0, 9 to 49
        /// </summary>
        public int? RowsToEncode { get; private set; }

        /// <summary>
        /// Format ID. Accepted Values: 1 to 6. Not used with QualityLevel 200
        /// </summary>
        public int? FormatId { get; private set; } = 6;

        /// <summary>
        /// Escape sequence control character
        /// </summary>
        public char EscapeSequenceControlChar { get; private set; } = '~';

        /// <summary>
        /// Aspect ratio. Accepted Values: 1, 2
        /// </summary>
        public int AspectRatio { get; private set; } = 1;

        /// <summary>
        /// Data Matrix Bar Code
        /// </summary>
        public DataMatrixBarCodeCommand()
        { }

        /// <summary>
        /// Data Matrix Bar Code
        /// </summary>
        /// <param name="orientation">Orientation</param>
        /// <param name="elementHeight">Element height (1 to width of the label)</param>
        /// <param name="qualityLevel">Quality level</param>
        /// <param name="columnsToEncode">Columns to encode</param>
        /// <param name="rowsToEncode">Rows to encode</param>
        /// <param name="formatId">Format ID</param>
        /// <param name="escapeSequenceControlChar">Escape sequence control character</param>
        /// <param name="aspectRatio">Aspect ratio</param>
        public DataMatrixBarCodeCommand(
            Orientation orientation = Orientation.Normal,
            int? elementHeight = null,
            int qualityLevel = 0,
            int? columnsToEncode = null,
            int? rowsToEncode = null,
            int? formatId = 6,
            char escapeSequenceControlChar = '~',
            int aspectRatio = 1
            )
        {
            this.Orientation = orientation;
            this.ElementHeight = elementHeight;
            this.QualityLevel = qualityLevel;
            this.ColumnsToEncode = columnsToEncode;
            this.RowsToEncode = rowsToEncode;
            this.FormatId = formatId;
            this.EscapeSequenceControlChar = escapeSequenceControlChar;
            this.AspectRatio = aspectRatio;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return Invariant($"{CommandPrefix}{RenderOrientation(this.Orientation)},{this.ElementHeight},{this.QualityLevel},{this.ColumnsToEncode},{this.RowsToEncode},{this.FormatId},{this.EscapeSequenceControlChar},{this.AspectRatio}");
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new DataMatrixBarCodeCommand();
            var zplDataParts = zplCommand.Substring(CommandPrefix.Length).Split(',');

            if (zplDataParts.Length > 0)
            {
                command.Orientation = ConvertOrientation(zplDataParts[0]);
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var elementHeight))
                {
                    command.ElementHeight = elementHeight;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var qualityLevel))
                {
                    command.QualityLevel = qualityLevel;
                }
            }

            if (zplDataParts.Length > 3)
            {
                if (int.TryParse(zplDataParts[3], out var columnsToEncode))
                {
                    command.ColumnsToEncode = columnsToEncode;
                }
            }

            if (zplDataParts.Length > 4)
            {
                if (int.TryParse(zplDataParts[4], out var rowsToEncode))
                {
                    command.RowsToEncode = rowsToEncode;
                }
            }

            if(zplDataParts.Length > 5)
            {
                if (int.TryParse(zplDataParts[5], out var formatId))
                {
                    command.FormatId = formatId;
                }
            }

            if(zplDataParts.Length > 6)
            {
                if (zplDataParts[6].Length > 0)
                {
                    command.EscapeSequenceControlChar = zplDataParts[6][0];
                }
            }

            if(zplDataParts.Length > 7)
            {
                if(int.TryParse(zplDataParts[7], out var aspectRatio))
                {
                    command.AspectRatio = aspectRatio;
                }
            }

            return command;
        }

    }
}
