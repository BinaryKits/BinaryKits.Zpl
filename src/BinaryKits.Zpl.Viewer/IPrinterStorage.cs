namespace BinaryKits.Zpl.Viewer
{
    public interface IPrinterStorage
    {
        void AddFile(char storageDevice, string fileName, byte[] data);
        byte[] GetFile(char storageDevice, string fileName);
    }
}
