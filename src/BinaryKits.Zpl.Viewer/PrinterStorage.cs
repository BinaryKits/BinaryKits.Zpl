using System.Collections.Concurrent;

namespace BinaryKits.Zpl.Viewer
{
    public class PrinterStorage : IPrinterStorage
    {
        private readonly ConcurrentDictionary<char, ConcurrentDictionary<string, byte[]>> cache;

        public PrinterStorage()
        {
            this.cache = new ConcurrentDictionary<char, ConcurrentDictionary<string, byte[]>>();
        }

        public void AddFile(char storageDevice, string fileName, byte[] data)
        {
            if (!this.cache.ContainsKey(storageDevice))
            {
                this.cache.TryAdd(storageDevice, new ConcurrentDictionary<string, byte[]>());
            }

            if (this.cache.TryGetValue(storageDevice, out ConcurrentDictionary<string, byte[]> files))
            {
                files.TryAdd(fileName, data);
            }
        }

        public byte[] GetFile(char storageDevice, string fileName)
        {
            if (!this.cache.ContainsKey(storageDevice))
            {
                return [];
            }

            if (this.cache.TryGetValue(storageDevice, out ConcurrentDictionary<string, byte[]> files))
            {
                files.TryGetValue(fileName, out byte[] data);
                return data ?? [];
            }

            return [];
        }
    }
}
