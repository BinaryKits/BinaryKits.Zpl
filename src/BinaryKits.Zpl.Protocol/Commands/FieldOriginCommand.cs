namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Field Origin<br/>
    /// The ^FO command sets a field origin, relative to the label home (^LH) position. ^FO sets the upper-left 
    /// corner of the field area by defining points along the x-axis and y-axis independent of the rotation.
    /// </summary>
    public class FieldOriginCommand : CommandBase
    {
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
        public FieldOriginCommand() : base("^FO")
        { }

        /// <summary>
        /// Field Origin
        /// </summary>
        /// <param name="x">x-axis location (0 to 32000)</param>
        /// <param name="y">y-axis location (0 to 32000)</param>
        public FieldOriginCommand(
            int? x = null,
            int? y = null)
            : this()
        {
            if (this.ValidateIntParameter(nameof(x), x, 0, 32000))
            {
                this.X = x.Value;
            }

            if (this.ValidateIntParameter(nameof(y), y, 0, 32000))
            {
                this.Y = y.Value;
            }
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.X},{this.Y}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            if (zplDataParts.Length > 0)
            {
                if (int.TryParse(zplDataParts[0], out var x))
                {
                    this.X = x;
                }
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var y))
                {
                    this.Y = y;
                }
            }
        }
    }
}
