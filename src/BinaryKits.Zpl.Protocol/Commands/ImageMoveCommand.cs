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
        /// Location of stored object
        /// </summary>
        public string LocationOfStoredObject { get; private set; }

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
        /// <param name="locationOfStoredObject">Location of stored object</param>
        /// <param name="imageName">Image name</param>
        public ImageMoveCommand(
            string locationOfStoredObject,
            string imageName)
            : this()
        {
            this.LocationOfStoredObject = locationOfStoredObject;
            this.ImageName = imageName;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.LocationOfStoredObject}{this.ImageName}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            var zplData = zplCommand.Substring(this.CommandPrefix.Length);

            if (zplData.Length >= 2)
            {
                this.LocationOfStoredObject = zplData.Substring(0, 2);
            }

            if (zplData.Length > 2)
            {
                this.ImageName = zplData.Substring(2);
            }
        }
    }
}
