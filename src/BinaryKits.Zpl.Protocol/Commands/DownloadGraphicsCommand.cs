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
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "~DG";

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
        public DownloadGraphicsCommand()
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
        {
            if (ValidateStorageDevice(storageDevice))
            {
                this.StorageDevice = storageDevice;
            }

            this.ImageName = imageName;
            this.TotalNumberOfBytesInGraphic = totalNumberOfBytesInGraphic;
            this.NumberOfBytesPerRow = numberOfBytesPerRow;
            this.Data = data;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{this.StorageDevice}{this.ImageName},{this.TotalNumberOfBytesInGraphic},{this.NumberOfBytesPerRow},{this.Data}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new DownloadGraphicsCommand();
            var zplDataParts = zplCommand.Substring(CommandPrefix.Length).Split(new char[] { ',' }, 4);

            if (zplDataParts.Length > 0)
            {
                var StorageFileNameMatch = StorageFileNameRegex.Match(zplDataParts[0]);
                if (StorageFileNameMatch.Success)
                {
                    if (StorageFileNameMatch.Groups[1].Success)
                    {
                        command.StorageDevice = StorageFileNameMatch.Groups[1].Value;
                    }
                    command.ImageName = StorageFileNameMatch.Groups[2].Value;
                }
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var totalNumberOfBytesInGraphic))
                {
                    command.TotalNumberOfBytesInGraphic = totalNumberOfBytesInGraphic;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var numberOfBytesPerRow))
                {
                    command.NumberOfBytesPerRow = numberOfBytesPerRow;
                }
            }

            if (zplDataParts.Length > 3)
            {
                command.Data = zplDataParts[3];
            }

            return command;
        }

    }
}
