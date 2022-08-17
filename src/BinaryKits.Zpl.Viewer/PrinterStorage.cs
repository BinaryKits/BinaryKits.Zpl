using System;
using System.Collections.Concurrent;

namespace BinaryKits.Zpl.Viewer
{
    public class PrinterStorage : IPrinterStorage
    {
        private readonly ConcurrentDictionary<char, ConcurrentDictionary<string, byte[]>> _cache;

        public PrinterStorage()
        {
            this._cache = new ConcurrentDictionary<char, ConcurrentDictionary<string, byte[]>>();
        }

        public void AddFile(char storageDevice, string fileName, byte[] data)
        {
            if (!this._cache.ContainsKey(storageDevice))
            {
                this._cache.TryAdd(storageDevice, new ConcurrentDictionary<string, byte[]>());
            }

            if (this._cache.TryGetValue(storageDevice, out var files))
            {
                files.TryAdd(fileName, data);
            }
        }

        public byte[] GetFile(char storageDevice, string fileName)
        {
            if (!this._cache.ContainsKey(storageDevice))
            {
                return Array.Empty<byte>();
            }

            if (this._cache.TryGetValue(storageDevice, out var files))
            {
                files.TryGetValue(fileName, out var data);
                return data ?? Array.Empty<byte>();
            }

            return Array.Empty<byte>();
        }
    }
}
