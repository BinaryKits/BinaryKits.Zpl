using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Recall Graphic<br/>
    /// The ^XG command is used to recall one or more graphic images for printing. This command is
    /// used in a label format to merge graphics, such as company logos and piece parts, with text data to form a
    /// complete label.
    /// </summary>
    public class RecallGraphicCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^XG";

        /// <summary>
        /// Storage device
        /// </summary>
        public string StorageDevice { get; private set; } = "R:";

        /// <summary>
        /// Image name
        /// </summary>
        public string ImageName { get; private set; } = "UNKNOWN.GRF";

        /// <summary>
        /// Magnification factor on the x-axis
        /// </summary>
        public int MagnificationFactorX { get; private set; } = 1;

        /// <summary>
        /// Magnification factor on the y-axis
        /// </summary>
        public int MagnificationFactorY { get; private set; } = 1;

        /// <summary>
        /// Recall Graphic
        /// </summary>
        public RecallGraphicCommand()
        { }

        /// <summary>
        /// Recall Graphic
        /// </summary>
        /// <param name="storageDevice">Storage device</param>
        /// <param name="imageName">Image name</param>
        /// <param name="magnificationFactorX">Magnification factor on the x-axis (1 to 10)</param>
        /// <param name="magnificationFactorY">Magnification factor on the y-axis (1 to 10)</param>
        public RecallGraphicCommand(
            string storageDevice,
            string imageName,
            int magnificationFactorX = 1,
            int magnificationFactorY = 1)
            : this()
        {
            this.StorageDevice = storageDevice;
            this.ImageName = imageName;

            if (ValidateIntParameter(nameof(magnificationFactorX), magnificationFactorX, 1, 10))
            {
                this.MagnificationFactorX = magnificationFactorX;
            }

            if (ValidateIntParameter(nameof(magnificationFactorY), magnificationFactorY, 1, 10))
            {
                this.MagnificationFactorY = magnificationFactorY;
            }
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{this.StorageDevice}{this.ImageName},{this.MagnificationFactorX},{this.MagnificationFactorY}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new RecallGraphicCommand();
            var zplDataParts = zplCommand.Substring(CommandPrefix.Length).Split(',');

            if (zplDataParts.Length > 0)
            {
                var storageFileNameMatch = StorageFileNameRegex.Match(zplDataParts[0]);
                if (storageFileNameMatch.Success)
                {
                    if (storageFileNameMatch.Groups[1].Success)
                    {
                        command.StorageDevice = storageFileNameMatch.Groups[1].Value;
                    }
                    command.ImageName = storageFileNameMatch.Groups[2].Value;
                }
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var magnificationFactorY))
                {
                    command.MagnificationFactorX = magnificationFactorY;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var magnificationFactorX))
                {
                    command.MagnificationFactorY = magnificationFactorX;
                }
            }

            return command;
        }
    }
}
