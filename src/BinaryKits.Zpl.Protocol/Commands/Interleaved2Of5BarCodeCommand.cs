namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Interleaved 2 of 5 Bar Code<br/>
    /// The ^B2 command produces the Interleaved 2 of 5 bar code, a high-density, self-checking, continuous,
    /// numeric symbology.
    /// Each data character for the Interleaved 2 of 5 bar code is composed of five elements: five bars or five
    /// spaces.Of the five elements, two are wide and three are narrow.The bar code is formed by interleaving
    /// characters formed with all spaces into characters formed with all bars. 
    /// </summary>
    public class Interleaved2Of5BarCodeCommand : CommandBase
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
        /// Calculate and print Mod 10 check digit
        /// </summary>
        public bool CalculateAndPrintMod10CheckDigit { get; private set; } = false;

        /// <summary>
        /// Interleaved 2 of 5 Bar Code
        /// </summary>
        public Interleaved2Of5BarCodeCommand() : base("^B2")
        { }

        /// <summary>
        /// Interleaved 2 of 5 Bar Code
        /// </summary>
        /// <param name="orientation">Orientation</param>
        /// <param name="barCodeHeight">Bar code height (1 to 32000)</param>
        /// <param name="printInterpretationLine">Print interpretation line</param>
        /// <param name="printInterpretationLineAboveCode">Print interpretation line above code</param>
        /// <param name="calculateAndPrintMod10CheckDigit">Calculate and print Mod 10 check digit</param>
        public Interleaved2Of5BarCodeCommand(
            Orientation orientation = Orientation.Normal,
            int? barCodeHeight = null,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false,
            bool calculateAndPrintMod10CheckDigit = false)
            : this()
        {
            this.Orientation = orientation;

            if (this.ValidateIntParameter(nameof(barCodeHeight), barCodeHeight, 1, 32000))
            {
                this.BarCodeHeight = barCodeHeight.Value;
            }

            this.PrintInterpretationLine = printInterpretationLine;
            this.PrintInterpretationLineAboveCode = printInterpretationLineAboveCode;
            this.CalculateAndPrintMod10CheckDigit = calculateAndPrintMod10CheckDigit;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.RenderOrientation(this.Orientation)},{this.BarCodeHeight},{this.RenderBoolean(this.PrintInterpretationLine)},{this.RenderBoolean(this.PrintInterpretationLineAboveCode)},{this.RenderBoolean(this.CalculateAndPrintMod10CheckDigit)}";
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
                this.CalculateAndPrintMod10CheckDigit = this.ConvertBoolean(zplDataParts[4]);
            }
        }
    }
}
