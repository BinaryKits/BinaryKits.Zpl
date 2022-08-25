using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Label Home<br/>
    /// The ^LH command sets the label home position.
    /// 
    /// The default home position of a label is the upper-left corner (position 0,0 along the x and y axis). This is the
    /// axis reference point for labels. Any area below and to the right of this point is available for printing. The ^LH
    /// command changes this reference point. For instance, when working with preprinted labels, use this
    /// command to move the reference point below the preprinted area.
    /// 
    /// This command affects only fields that come after it. It is recommended to use ^LH as one of the first
    /// commands in the label format.
    /// </summary>
    public class LabelHomeCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^LH";

        /// <summary>
        /// X-axis position
        /// </summary>
        public int X { get; private set; } = 0;

        /// <summary>
        /// Y-axis position
        /// </summary>
        public int Y { get; private set; } = 0;

        /// <summary>
        /// Label Home
        /// </summary>
        public LabelHomeCommand()
        { }

        /// <summary>
        /// Label Home
        /// </summary>
        /// <param name="x">x-axis position (0 to 32000)</param>
        /// <param name="y">y-axis position (0 to 32000)</param>
        public LabelHomeCommand(
            int? x = null,
            int? y = null)
            : this()
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
            var command = new LabelHomeCommand();
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
