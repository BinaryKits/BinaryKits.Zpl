namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Download Graphics<br/>
    /// The ~DG command downloads an ASCII Hex representation of a graphic image. If .GRF is not the specified
    /// file extension, .GRF is automatically appended.
    /// </summary>
    public class DownloadGraphicsCommand : CommandBase
    {
        /// <summary>
        /// Device to store image
        /// </summary>
        public string DeviceToStoreImage { get; private set; } = "R:";

        /// <summary>
        /// Image name
        /// </summary>
        public string ImageName { get; private set; } = "UNKNOWN.GRF";

        /// <summary>
        /// Total number of bytes in graphic
        /// </summary>
        public int TotalNumberOfBytesInGraphic { get; private set; }

        /// <summary>
        /// Number of bytes per row
        /// </summary>
        public int NumberOfBytesPerRow { get; private set; }

        /// <summary>
        /// ASCII hexadecimal string defining image
        /// </summary>
        public string Data { get; private set; }

        /// <summary>
        /// Download Graphics
        /// </summary>
        public DownloadGraphicsCommand() : base("~DG")
        { }

        /// <summary>
        /// Download Graphics
        /// </summary>
        /// <param name="deviceToStoreImage">Device to store image</param>
        /// <param name="imageName">Image name</param>
        /// <param name="totalNumberOfBytesInGraphic">Total number of bytes in graphic</param>
        /// <param name="numberOfBytesPerRow">Number of bytes per row</param>
        /// <param name="data">ASCII hexadecimal string defining image</param>
        public DownloadGraphicsCommand(
            string deviceToStoreImage,
            string imageName,
            int totalNumberOfBytesInGraphic,
            int numberOfBytesPerRow,
            string data)
            : this()
        {
            this.DeviceToStoreImage = deviceToStoreImage;
            this.ImageName = imageName;
            this.TotalNumberOfBytesInGraphic = totalNumberOfBytesInGraphic;
            this.NumberOfBytesPerRow = numberOfBytesPerRow;
            this.Data = data;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.DeviceToStoreImage}{this.ImageName},{this.TotalNumberOfBytesInGraphic},{this.NumberOfBytesPerRow},{this.Data}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            if (zplDataParts.Length > 0)
            {
                this.DeviceToStoreImage = zplDataParts[0].Substring(0, 2);
                var imageName = zplDataParts[0].Substring(2);

                if (!string.IsNullOrEmpty(imageName))
                {
                    this.ImageName = imageName;
                }
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var totalNumberOfBytesInGraphic))
                {
                    this.TotalNumberOfBytesInGraphic = totalNumberOfBytesInGraphic;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var numberOfBytesPerRow))
                {
                    this.NumberOfBytesPerRow = numberOfBytesPerRow;
                }
            }

            //Third comma is the start of the image data
            var indexOfThirdComma = this.IndexOfNthCharacter(zplCommand, 3, ',');
            if (indexOfThirdComma != -1)
            {
                this.Data = zplCommand.Substring(indexOfThirdComma + 1);
            }
        }
    }
}
