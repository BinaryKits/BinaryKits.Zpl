namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Image Move<br/>
    /// The ^IM command performs a direct move of an image from storage area into the bitmap. The command is
    /// identical to the ^XG command(Recall Graphic), except there are no sizing parameters.
    /// </summary>
    public class ImageMoveCommand : CommandBase
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
        /// Image Move
        /// </summary>
        public ImageMoveCommand() : base("^IM")
        { }

        /// <summary>
        /// Image Move
        /// </summary>
        /// <param name="storageDevice">Storage device</param>
        /// <param name="imageName">Image name</param>
        public ImageMoveCommand(
            string storageDevice,
            string imageName)
            : this()
        {
            this.StorageDevice = storageDevice;
            this.ImageName = imageName;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.StorageDevice}{this.ImageName}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            var zplData = zplCommand.Substring(this.CommandPrefix.Length);

            if (zplData.Length >= 2)
            {
                this.StorageDevice = zplData.Substring(0, 2);
            }

            if (zplData.Length > 2)
            {
                this.ImageName = zplData.Substring(2);
            }
        }
    }
}
