using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Download Objects<br/>
    /// The ~DY command downloads to the printer graphic objects or fonts in any supported format. This
    /// command can be used in place of ~DG for more saving and loading options. ~DY is the preferred command
    /// to download TrueType fonts on printers with firmware later than X.13. It is faster than ~DU. The ~DY
    /// command also supports downloading wireless certificate files.
    /// </summary>
    public class DownloadObjectsCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "~DY";

        /// <summary>
        /// Storage device (file location)
        /// </summary>
        public string StorageDevice { get; private set; } = "R:";

        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; private set; } = "UNKNOWN";

        /// <summary>
        /// Format downloaded in data field
        /// </summary>
        public char FormatDownloadedInDataField { get; private set; }

        /// <summary>
        /// Extension of stored file
        /// </summary>
        public string ExtensionOfStoredFile { get; private set; }

        /// <summary>
        /// Total number of bytes in file
        /// </summary>
        public int TotalNumberOfBytesInFile { get; private set; }

        /// <summary>
        /// Total number of bytes per row
        /// </summary>
        public int TotalNumberOfBytesPerRow { get; private set; }

        /// <summary>
        /// Data
        /// </summary>
        public string Data { get; private set; }

        /// <summary>
        /// Download Objects
        /// </summary>
        public DownloadObjectsCommand()
        { }

        /// <summary>
        /// Download Objects
        /// </summary>
        /// <param name="storageDevice">Storage device</param>
        /// <param name="fileName">File name</param>
        /// <param name="formatDownloadedInDataField">Format downloaded in data field</param>
        /// <param name="extensionOfStoredFile">Extension of stored file</param>
        /// <param name="totalNumberOfBytesInFile">Total number of bytes in file</param>
        /// <param name="totalNumberOfBytesPerRow">Total number of bytes per row</param>
        /// <param name="data">Data</param>
        public DownloadObjectsCommand(
            string storageDevice,
            string fileName,
            char formatDownloadedInDataField,
            string extensionOfStoredFile,
            int totalNumberOfBytesInFile,
            int totalNumberOfBytesPerRow,
            string data)
        {
            if (ValidateStorageDevice(storageDevice))
            {
                this.StorageDevice = storageDevice;
            }

            this.FileName = fileName;
            this.FormatDownloadedInDataField = formatDownloadedInDataField;
            this.ExtensionOfStoredFile = extensionOfStoredFile;
            this.TotalNumberOfBytesInFile = totalNumberOfBytesInFile;
            this.TotalNumberOfBytesPerRow = totalNumberOfBytesPerRow;
            this.Data = data;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{this.StorageDevice}{this.FileName},{this.FormatDownloadedInDataField},{this.ExtensionOfStoredFile},{this.TotalNumberOfBytesInFile},{this.TotalNumberOfBytesPerRow},{this.Data}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new DownloadObjectsCommand();
            var zplDataParts = zplCommand.Substring(CommandPrefix.Length).Split(new char[] { ',' }, 6);

            if (zplDataParts.Length > 0)
            {
                var storageFileNameMatch = StorageFileNameRegex.Match(zplDataParts[0]);
                if (storageFileNameMatch.Success)
                {
                    if (storageFileNameMatch.Groups[1].Success)
                    {
                        command.StorageDevice = storageFileNameMatch.Groups[1].Value;
                    }

                    command.FileName = storageFileNameMatch.Groups[2].Value;
                }
            }

            if (zplDataParts.Length > 1)
            {
                command.FormatDownloadedInDataField = zplDataParts[1][0];
            }

            if (zplDataParts.Length > 2)
            {
                command.ExtensionOfStoredFile = zplDataParts[2];
            }

            if (zplDataParts.Length > 3)
            {
                if (int.TryParse(zplDataParts[3], out var totalNumberOfBytesInFile))
                {
                    command.TotalNumberOfBytesInFile = totalNumberOfBytesInFile;
                }
            }

            if (zplDataParts.Length > 4)
            {
                if (int.TryParse(zplDataParts[4], out var totalNumberOfBytesPerRow))
                {
                    command.TotalNumberOfBytesPerRow = totalNumberOfBytesPerRow;
                }
            }

            if (zplDataParts.Length > 5)
            {
                command.Data = zplDataParts[5];
            }

            return command;
        }

    }
}
