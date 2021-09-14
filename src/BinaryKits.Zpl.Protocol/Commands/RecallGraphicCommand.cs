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
        public RecallGraphicCommand() : base("^XG")
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

            if (this.ValidateIntParameter(nameof(magnificationFactorX), magnificationFactorX, 1, 10))
            {
                this.MagnificationFactorX = magnificationFactorX;
            }

            if (this.ValidateIntParameter(nameof(magnificationFactorY), magnificationFactorY, 1, 10))
            {
                this.MagnificationFactorY = magnificationFactorY;
            }
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.StorageDevice}{this.ImageName},{this.MagnificationFactorX},{this.MagnificationFactorY}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            var zplData = zplCommand.Substring(this.CommandPrefix.Length);

            if (zplData.Length >= 2)
            {
                this.StorageDevice = zplData.Substring(0, 2);
            }

            var zplDataParts = this.SplitCommand(zplCommand, 2);

            if (zplDataParts.Length > 0)
            {
                var imageName = zplDataParts[0];
                if (!string.IsNullOrEmpty(imageName))
                {
                    this.ImageName = imageName;
                }
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var magnificationFactorY))
                {
                    this.MagnificationFactorX = magnificationFactorY;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var magnificationFactorX))
                {
                    this.MagnificationFactorY = magnificationFactorX;
                }
            }
        }
    }
}
