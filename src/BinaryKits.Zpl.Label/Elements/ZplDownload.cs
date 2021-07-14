namespace BinaryKits.Zpl.Label.Elements
{
    public abstract class ZplDownload : ZplElementBase
    {
        /// <summary>
        /// DRAM, Memory Card, EPROM, Flash
        /// R, E, B, and A
        /// </summary>
        public char StorageDevice { get; private set; }

        public ZplDownload(char storageDevice)
        {
            StorageDevice = storageDevice;
        }
    }
}
