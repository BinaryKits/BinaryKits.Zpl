using System;

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
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^BQ";

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
        public QrCodeBarCodeCommand()
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
        {
            this.Orientation = orientation;

            if (ValidateIntParameter(nameof(model), model, 1, 2))
            {
                this.Model = model;
            }

            if (ValidateIntParameter(nameof(magnificationFactor), magnificationFactor, 1, 10))
            {
                this.MagnificationFactor = magnificationFactor;
            }

            this.ErrorCorrection = errorCorrection;

            if (ValidateIntParameter(nameof(maskValue), maskValue, 0, 7))
            {
                this.MaskValue = maskValue;
            }
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{RenderOrientation(this.Orientation)},{this.Model},{this.MagnificationFactor},{RenderErrorCorrectionLevel(this.ErrorCorrection)},{this.MaskValue}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new QrCodeBarCodeCommand();
            var zplDataParts = zplCommand.Substring(CommandPrefix.Length).Split(',');

            if (zplDataParts.Length > 0)
            {
                command.Orientation = ConvertOrientation(zplDataParts[0]);
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var model))
                {
                    command.Model = model;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var magnificationFactor))
                {
                    command.MagnificationFactor = magnificationFactor;
                }
            }

            if (zplDataParts.Length > 3)
            {
                command.ErrorCorrection = ConvertErrorCorrectionLevel(zplDataParts[3]);
            }

            if (zplDataParts.Length > 4)
            {
                if (int.TryParse(zplDataParts[4], out var maskValue))
                {
                    command.MaskValue = maskValue;
                }
            }

            return command;
        }

    }
}
