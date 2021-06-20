namespace BinaryKits.Utility.ZPLUtility.Elements
{
    public abstract class ZPLDownload : ZPLElementBase
    {
        /// <summary>
        /// DRAM, Memory Card, EPROM, Flash
        /// R, E, B, and A
        /// </summary>
        public char StorageDevice { get; set; }

        public ZPLDownload(char storageDevice)
        {
            StorageDevice = storageDevice;
        }
    }
}
