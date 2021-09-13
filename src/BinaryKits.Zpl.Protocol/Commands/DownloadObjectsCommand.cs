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
        public DownloadObjectsCommand() : base("~DY")
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
            : this()
        {
            this.StorageDevice = storageDevice;
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
            return $"{this.CommandPrefix}{this.StorageDevice}{this.FileName},{this.FormatDownloadedInDataField},{this.ExtensionOfStoredFile},{this.TotalNumberOfBytesInFile},{this.TotalNumberOfBytesPerRow},{this.Data}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            if (zplCommand.Length <= 4)
            {
                return;
            }

            this.StorageDevice = zplCommand.Substring(this.CommandPrefix.Length, 2);

            var zplDataParts = this.SplitCommand(zplCommand, 2);

            if (zplDataParts.Length > 0)
            {
                var fileName = zplDataParts[0];

                if (!string.IsNullOrEmpty(fileName))
                {
                    this.FileName = zplDataParts[0];
                }
            }

            if (zplDataParts.Length > 1)
            {
                this.FormatDownloadedInDataField = zplDataParts[1][0];
            }

            if (zplDataParts.Length > 2)
            {
                this.ExtensionOfStoredFile = zplDataParts[2];
            }

            if (zplDataParts.Length > 3)
            {
                if (int.TryParse(zplDataParts[3], out var totalNumberOfBytesInFile))
                {
                    this.TotalNumberOfBytesInFile = totalNumberOfBytesInFile;
                }
            }

            if (zplDataParts.Length > 4)
            {
                if (int.TryParse(zplDataParts[4], out var totalNumberOfBytesPerRow))
                {
                    this.TotalNumberOfBytesPerRow = totalNumberOfBytesPerRow;
                }
            }

            if (zplDataParts.Length > 5)
            {
                this.Data = zplDataParts[5];
            }
        }
    }
}
