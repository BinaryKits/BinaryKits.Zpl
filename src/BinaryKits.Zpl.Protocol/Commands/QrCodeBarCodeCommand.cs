namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// QR Code Bar Code<br/>
    /// The ^BQ command produces a matrix symbology consisting of an array of nominally square modules
    /// arranged in an overall square pattern.A unique pattern at three of the symbol’s four corners assists in
    /// determining bar code size, position, and inclination.
    /// A wide range of symbol sizes is possible, along with four levels of error correction.User-specified module
    /// dimensions provide a wide variety of symbol production techniques.
    /// QR Code Model 1 is the original specification, while QR Code Model 2 is an enhanced form of the
    /// symbology. Model 2 provides additional features and can be automatically differentiated from Model 1.
    /// Model 2 is the recommended model and should normally be used.
    /// </summary>
    public class QrCodeBarCodeCommand : CommandBase
    {
        /// <summary>
        /// Orientation
        /// </summary>
        public Orientation Orientation { get; private set; } = Orientation.Normal;

        /// <summary>
        /// Model
        /// </summary>
        public int Model { get; private set; } = 2;

        /// <summary>
        /// Magnification factor
        /// </summary>
        public int MagnificationFactor { get; private set; } = 1;

        /// <summary>
        /// Error correction
        /// </summary>
        public ErrorCorrectionLevel ErrorCorrection { get; private set; } = ErrorCorrectionLevel.HighReliability;

        /// <summary>
        /// Mask value<br/>
        /// Before QR code is finally generated, the data bits must be XOR-ed with mask pattern.
        /// This process have a purpose of making QR code more readable by QR scanner.
        /// </summary>
        public int MaskValue { get; private set; } = 7;

        /// <summary>
        /// QR Code Bar Code
        /// </summary>
        public QrCodeBarCodeCommand() : base("^BQ")
        { }

        /// <summary>
        /// QR Code Bar Code
        /// </summary>
        /// <param name="orientation">Orientation</param>
        /// <param name="model">Model 1 (original) and 2 (enhanced – recommended)</param>
        /// <param name="magnificationFactor">Magnification factor</param>
        /// <param name="errorCorrection">Error correction</param>
        /// <param name="maskValue">Mask value</param>
        public QrCodeBarCodeCommand(
            Orientation orientation = Orientation.Normal,
            int model = 2,
            int magnificationFactor = 1,
            ErrorCorrectionLevel errorCorrection = ErrorCorrectionLevel.HighReliability,
            int maskValue = 7)
            : this()
        {
            this.Orientation = orientation;

            if (this.ValidateIntParameter(nameof(model), model, 1, 2))
            {
                this.Model = model;
            }

            if (this.ValidateIntParameter(nameof(magnificationFactor), magnificationFactor, 1, 10))
            {
                this.MagnificationFactor = magnificationFactor;
            }

            this.ErrorCorrection = errorCorrection;

            if (this.ValidateIntParameter(nameof(maskValue), maskValue, 0, 7))
            {
                this.MaskValue = maskValue;
            }
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.RenderOrientation(this.Orientation)},{this.Model},{this.MagnificationFactor},{this.RenderErrorCorrectionLevel(this.ErrorCorrection)},{this.MaskValue}";
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
                if (int.TryParse(zplDataParts[1], out var model))
                {
                    this.Model = model;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var magnificationFactor))
                {
                    this.MagnificationFactor = magnificationFactor;
                }
            }

            if (zplDataParts.Length > 3)
            {
                this.ErrorCorrection = this.ConvertErrorCorrectionLevel(zplDataParts[3]);
            }

            if (zplDataParts.Length > 4)
            {
                if (int.TryParse(zplDataParts[4], out var maskValue))
                {
                    this.MaskValue = maskValue;
                }
            }
        }
    }
}
