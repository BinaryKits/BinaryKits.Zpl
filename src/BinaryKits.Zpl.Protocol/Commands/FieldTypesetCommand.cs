using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Field Typeset<br/>
    /// The ^FT command sets the field position, relative to the home position of the label designated by the ^LH
    /// command.The typesetting origin of the field is fixed with respect to the contents of the field and does not
    /// change with rotation.
    /// </summary>
    public class FieldTypesetCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^FT";

        /// <summary>
        /// X-axis location
        /// </summary>
        public int X { get; private set; } = 0;

        /// <summary>
        /// Y-axis location
        /// </summary>
        public int Y { get; private set; } = 0;

        /// <summary>
        /// Field Typeset
        /// </summary>
        public FieldTypesetCommand()
        { }

        /// <summary>
        /// Field Typeset
        /// </summary>
        /// <param name="x">x-axis location (0 to 32000)</param>
        /// <param name="y">y-axis location (0 to 32000)</param>
        public FieldTypesetCommand(
            int? x = null,
            int? y = null)
        {
            if (ValidateIntParameter(nameof(x), x, 0, 32000))
            {
                this.X = x.Value;
            }

            if (ValidateIntParameter(nameof(y), y, 0, 32000))
            {
                this.Y = y.Value;
            }
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{this.X},{this.Y}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new FieldTypesetCommand();
            var zplDataParts = zplCommand.Substring(CommandPrefix.Length).Split(',');

            if (zplDataParts.Length > 0)
            {
                if (int.TryParse(zplDataParts[0], out var x))
                {
                    command.X = x;
                }
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var y))
                {
                    command.Y = y;
                }
            }

            return command;
        }

    }
}
