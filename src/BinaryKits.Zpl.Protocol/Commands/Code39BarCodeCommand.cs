namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Code 39 Bar Code<br/>
    /// The Code 39 bar code is the standard for many industries, including the U.S. Department of Defense. It is
    /// one of three symbologies identified in the American National Standards Institute(ANSI) standard
    /// MH10.8M-1983. Code 39 is also known as USD-3 Code and 3 of 9 Code.
    /// </summary>
    public class Code39BarCodeCommand : CommandBase
    {
        /// <summary>
        /// Orientation
        /// </summary>
        public Orientation Orientation { get; private set; } = Orientation.Normal;

        /// <summary>
        /// Mod-43 check digit
        /// </summary>
        public bool Mod43CheckDigit { get; private set; } = false;

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
        /// Code 39 Bar Code
        /// </summary>
        public Code39BarCodeCommand() : base("^B3")
        { }

        /// <summary>
        /// Code 39 Bar Code
        /// </summary>
        /// <param name="orientation">Orientation</param>
        /// <param name="mod43CheckDigit">Mod-43 check digit</param>
        /// <param name="barCodeHeight">Bar code height (1 to 32000)</param>
        /// <param name="printInterpretationLine">Print interpretation line</param>
        /// <param name="printInterpretationLineAboveCode">Print interpretation line above code</param>
        public Code39BarCodeCommand(
            Orientation orientation = Orientation.Normal,
            bool mod43CheckDigit = false,
            int? barCodeHeight = null,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false
            )
            : this()
        {
            this.Orientation = orientation;

            this.Mod43CheckDigit = mod43CheckDigit;

            if (this.ValidateIntParameter(nameof(barCodeHeight), barCodeHeight, 1, 32000))
            {
                this.BarCodeHeight = barCodeHeight.Value;
            }

            this.PrintInterpretationLine = printInterpretationLine;
            this.PrintInterpretationLineAboveCode = printInterpretationLineAboveCode;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.RenderOrientation(this.Orientation)},{this.RenderBoolean(this.Mod43CheckDigit)},{this.BarCodeHeight},{this.RenderBoolean(this.PrintInterpretationLine)},{this.RenderBoolean(this.PrintInterpretationLineAboveCode)}";
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
                this.Mod43CheckDigit = this.ConvertBoolean(zplDataParts[1]);
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var barCodeHeight))
                {
                    this.BarCodeHeight = barCodeHeight;
                }
            }

            if (zplDataParts.Length > 3)
            {
                this.PrintInterpretationLine = this.ConvertBoolean(zplDataParts[3]);
            }

            if (zplDataParts.Length > 4)
            {
                this.PrintInterpretationLineAboveCode = this.ConvertBoolean(zplDataParts[4]);
            }
        }
    }
}
