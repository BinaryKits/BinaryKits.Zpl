namespace BinaryKits.Zpl.Protocol.ImageConverters
{
    /// <summary>
    /// Zebra Image Result
    /// </summary>
    public class ImageResult
    {
        /// <summary>
        /// Zpl Image Data
        /// </summary>
        public string ZplData { get; set; }
        /// <summary>
        /// Binary byte count
        /// </summary>
        public int BinaryByteCount { get; set; }
        /// <summary>
        /// Bytes per row
        /// </summary>
        public int BytesPerRow { get; set; }
    }
}
