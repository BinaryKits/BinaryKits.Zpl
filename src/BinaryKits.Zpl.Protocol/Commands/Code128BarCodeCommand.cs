namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Code 128 Bar Code (Subsets A, B, and C)<br/>
    /// The ^BC command creates the Code 128 bar code, a high-density, variable length, continuous,
    /// alphanumeric symbology.It was designed for complexly encoded product identification.
    /// Code 128 has three subsets of characters.There are 106 encoded printing characters in each set, and
    /// each character can have up to three different meanings, depending on the character subset being used.
    /// Each Code 128 character consists of six elements: three bars and three spaces.
    /// </summary>
    public class Code128BarCodeCommand : CommandBase
    {
        /// <summary>
        /// Orientation
        /// </summary>
        public Orientation Orientation { get; private set; } = Orientation.Normal;

        /// <summary>
        /// Bar code height
        /// </summary>
        public int? BarCodeHeight { get; private set; }

        /// <summary>
        /// Print interpretation line
        /// </summary>
        public bool PrintInterpretationLine { get; private set; } = true;

        /// <summary>
        /// Print interpretation line above code
        /// </summary>
        public bool PrintInterpretationLineAboveCode { get; private set; } = false;

        /// <summary>
        /// UCC check digit
        /// </summary>
        public bool UccCheckDigit { get; private set; } = false;

        /// <summary>
        /// Code 128 Bar Code (Subsets A, B, and C)
        /// </summary>
        public Code128BarCodeCommand() : base("^BC")
        { }

        /// <summary>
        /// Code 128 Bar Code (Subsets A, B, and C)
        /// </summary>
        /// <param name="orientation">Orientation</param>
        /// <param name="barCodeHeight">Bar code height (1 to 32000)</param>
        /// <param name="printInterpretationLine">Print interpretation line</param>
        /// <param name="printInterpretationLineAboveCode">Print interpretation line above code</param>
        /// <param name="uccCheckDigit">UCC check digit</param>
        public Code128BarCodeCommand(
            Orientation orientation = Orientation.Normal,
            int? barCodeHeight = null,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false,
            bool uccCheckDigit = false)
            : this()
        {
            this.Orientation = orientation;

            if (this.ValidateIntParameter(nameof(barCodeHeight), barCodeHeight, 1, 32000))
            {
                this.BarCodeHeight = barCodeHeight.Value;
            }

            this.PrintInterpretationLine = printInterpretationLine;
            this.PrintInterpretationLineAboveCode = printInterpretationLineAboveCode;
            this.UccCheckDigit = uccCheckDigit;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.RenderOrientation(this.Orientation)},{this.BarCodeHeight},{this.RenderBoolean(this.PrintInterpretationLine)},{this.RenderBoolean(this.PrintInterpretationLineAboveCode)},{this.RenderBoolean(this.UccCheckDigit)}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            if (zplDataParts.Length > 0)
            {
                this.Orientation = this.ConvertOrientation(zplDataParts[0]);
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var barCodeHeight))
                {
                    this.BarCodeHeight = barCodeHeight;
                }
            }

            if (zplDataParts.Length > 2)
            {
                this.PrintInterpretationLine = this.ConvertBoolean(zplDataParts[2]);
            }

            if (zplDataParts.Length > 3)
            {
                this.PrintInterpretationLineAboveCode = this.ConvertBoolean(zplDataParts[3]);
            }

            if (zplDataParts.Length > 4)
            {
                this.UccCheckDigit = this.ConvertBoolean(zplDataParts[4]);
            }
        }
    }
}
