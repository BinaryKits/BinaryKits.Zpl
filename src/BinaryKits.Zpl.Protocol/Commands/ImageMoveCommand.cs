using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Image Move<br/>
    /// The ^IM command performs a direct move of an image from storage area into the bitmap. The command is
    /// identical to the ^XG command(Recall Graphic), except there are no sizing parameters.
    /// </summary>
    public class ImageMoveCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^IM";

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
        public ImageMoveCommand()
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
            var command = new ImageMoveCommand();
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
