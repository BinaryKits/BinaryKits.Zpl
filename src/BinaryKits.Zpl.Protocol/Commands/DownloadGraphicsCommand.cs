using System;
using System.Linq;

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
        /// Storage device
        /// </summary>
        public string StorageDevice { get; private set; } = "R:";

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
        /// <param name="storageDevice">Storage device</param>
        /// <param name="imageName">Image name</param>
        /// <param name="totalNumberOfBytesInGraphic">Total number of bytes in graphic</param>
        /// <param name="numberOfBytesPerRow">Number of bytes per row</param>
        /// <param name="data">ASCII hexadecimal string defining image</param>
        public DownloadGraphicsCommand(
            string storageDevice,
            string imageName,
            int totalNumberOfBytesInGraphic,
            int numberOfBytesPerRow,
            string data)
            : this()
        {
            this.ValidateDeviceToStoreImage(storageDevice);

            this.StorageDevice = storageDevice;
            this.ImageName = imageName;
            this.TotalNumberOfBytesInGraphic = totalNumberOfBytesInGraphic;
            this.NumberOfBytesPerRow = numberOfBytesPerRow;
            this.Data = data;
        }

        private void ValidateDeviceToStoreImage(string deviceToStoreImage)
        {
            if (deviceToStoreImage.Length != 2)
            {
                throw new ArgumentException($"Invalid format requires 2 characters", deviceToStoreImage);
            }

            var allowedDevices = new char[] { 'R', 'E', 'B', 'A' };

            if (!allowedDevices.Contains(deviceToStoreImage[0]))
            {
                throw new ArgumentException($"Invalid device letter", deviceToStoreImage);
            }

            if (deviceToStoreImage[1] != ':')
            {
                throw new ArgumentException($"The second character must be a colon", deviceToStoreImage);
            }
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.StorageDevice}{this.ImageName},{this.TotalNumberOfBytesInGraphic},{this.NumberOfBytesPerRow},{this.Data}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            if (zplDataParts.Length > 0)
            {
                this.StorageDevice = zplDataParts[0].Substring(0, 2);
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
