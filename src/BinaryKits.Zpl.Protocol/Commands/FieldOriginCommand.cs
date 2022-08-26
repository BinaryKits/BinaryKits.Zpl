using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Field Origin<br/>
    /// The ^FO command sets a field origin, relative to the label home (^LH) position. ^FO sets the upper-left 
    /// corner of the field area by defining points along the x-axis and y-axis independent of the rotation.
    /// </summary>
    public class FieldOriginCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^FO";

        /// <summary>
        /// X-axis location
        /// </summary>
        public int X { get; private set; } = 0;

        /// <summary>
        /// Y-axis location
        /// </summary>
        public int Y { get; private set; } = 0;

        /// <summary>
        /// Field Origin
        /// </summary>
        public FieldOriginCommand()
        { }

        /// <summary>
        /// Field Origin
        /// </summary>
        /// <param name="x">x-axis location (0 to 32000)</param>
        /// <param name="y">y-axis location (0 to 32000)</param>
        public FieldOriginCommand(
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
            var command = new FieldOriginCommand();
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
