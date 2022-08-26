using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Recall Format<br/>
    /// The ^XF command recalls a stored format to be merged with variable data.
    /// There can be multiple ^XF commands in one format, and they can be located anywhere within
    /// the code.
    /// </summary>
    public class RecallFormatCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^XF";

        /// <summary>
        /// Storage device
        /// </summary>
        public string StorageDevice { get; private set; } = "R:";

        /// <summary>
        /// Image name
        /// </summary>
        public string ImageName { get; private set; } = "UNKNOWN.ZPL";

        /// <summary>
        /// Recall Format
        /// </summary>
        public RecallFormatCommand()
        { }

        /// <summary>
        /// Recall Format
        /// </summary>
        /// <param name="storageDevice">Storage device</param>
        /// <param name="imageName">Image name</param>
        public RecallFormatCommand(
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
            var command = new RecallFormatCommand();
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
