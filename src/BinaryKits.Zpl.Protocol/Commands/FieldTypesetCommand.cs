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
        public FieldTypesetCommand() : base("^FT")
        { }

        /// <summary>
        /// Field Typeset
        /// </summary>
        /// <param name="x">x-axis location (0 to 32000)</param>
        /// <param name="y">y-axis location (0 to 32000)</param>
        public FieldTypesetCommand(
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
