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
        public LabelHomeCommand() : base("^LH")
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
