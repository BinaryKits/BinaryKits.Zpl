using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Download Format<br/>
    /// The ^DF command saves ZPL II format commands as text strings to be later
    /// merged using ^XF with variable data.The format to be stored might contain field number
    /// (^FN) commands to be referenced when recalled.
    /// </summary>
    public class DownloadFormatCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^DF";

        /// <summary>
        /// Storage device
        /// </summary>
        public string StorageDevice { get; private set; } = "R:";

        /// <summary>
        /// Image name
        /// </summary>
        public string ImageName { get; private set; } = "UNKNOWN.ZPL";

        /// <summary>
        /// Download Format
        /// </summary>
        public DownloadFormatCommand()
        { }

        /// <summary>
        /// Download Format
        /// </summary>
        /// <param name="storageDevice">Storage device</param>
        /// <param name="imageName">Image name</param>
        public DownloadFormatCommand(
            string storageDevice,
            string imageName)
        {
            if (ValidateStorageDevice(storageDevice))
            {
                this.StorageDevice = storageDevice;
            }

            this.ImageName = imageName;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{this.StorageDevice}{this.ImageName}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new DownloadFormatCommand();
            var zplCommandData = zplCommand.Substring(CommandPrefix.Length);

            var storageFileNameMatch = StorageFileNameRegex.Match(zplCommandData);
            if (storageFileNameMatch.Success)
            {
                if (storageFileNameMatch.Groups[1].Success)
                {
                    command.StorageDevice = storageFileNameMatch.Groups[1].Value;
                }
                command.ImageName = storageFileNameMatch.Groups[2].Value;
            }

            return command;
        }

    }
}
