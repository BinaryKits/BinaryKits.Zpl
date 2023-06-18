using System;
using System.IO.Compression;
using System.IO;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Protocol.Helpers
{
    /// <summary>
    /// Z64 Data Compression Scheme for ~DG and ~DB Commands
    /// First compresses the data using the LZ77 algorithm to reduce its size, then compressed data is then encoded using Base64
    /// A CRC is calculated across the Base64-encoded data. If the CRC-check fails or the download is aborted, the object can be invalidated by the printer.
    /// reduces the actual number of data bytes and the amount of time required to download graphic images and bitmapped fonts with the ~DG and ~DB commands
    /// </summary>
    public static class ZebraZ64CompressionHelper
    {

        private static Regex _Z64Regex;
        static ZebraZ64CompressionHelper()
        {
            _Z64Regex = new Regex(":(Z64):(\\S+):([0-9a-fA-F]+)", RegexOptions.Compiled);
        }

        public static string Compress(string hexData)
        {
            var cleanedHexData = hexData.Replace("\n", string.Empty).Replace("\r", string.Empty);
            return Compress(cleanedHexData.ToBytes());
        }

        public static string Compress(byte[] bytes)
        {
            var data = Deflate(bytes);
            var base64 = Convert.ToBase64String(data);
            return ":Z64:" + base64 + ":" + Crc16.ComputeChecksum(base64);

        }

        public static byte[] Uncompress(string hexData)
        {
            var match = _Z64Regex.Match(hexData);
            if (match.Success)
            {
                var imageBase64 = match.Groups[2].Value;
                var bytes = Convert.FromBase64String(imageBase64);
                return Inflate(bytes);
            }
            else
            {
                throw new FormatException("Hex string not in Z64 format");
            }
        }

        //public static byte[] Uncompress(byte[] bytes)
        //{
        //    return Inflate(bytes);
        //}

        /// <summary>
        /// Decompress graphics data with ZLib headers, could use ZLibStream in .NET 6.0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] Inflate(byte[] data)
        {
            using (var outputStream = new MemoryStream())
            {
                using (var inputStream = new MemoryStream())
                {
                    //skip first 2 bytes of headers and last 4 bytes of checksum.
                    inputStream.Write(data, 2, data.Length - 6);
                    inputStream.Position = 0;
                    using (var decompressor = new DeflateStream(inputStream, CompressionMode.Decompress, true))
                    {
                        decompressor.CopyTo(outputStream);
                        return outputStream.ToArray();

                    }
                }

            }
        }
        /// <summary>
        /// Compress graphics data with ZLib headers, could use ZLibStream in .NET 6.0
        /// https://yal.cc/cs-deflatestream-zlib/
        /// </summary>
        /// <param name="data"></param>
        /// <param name="level"></param>
        /// <returns></returns>

        private static byte[] Deflate(byte[] data)
        {
            byte[] newData;

            using (var memStream = new MemoryStream())
            {
                // write header:
                memStream.WriteByte(0x78);
                // default compression level
                memStream.WriteByte(0x9C);
                // write compressed data (with Deflate headers):
                var dflStream = new DeflateStream(memStream, CompressionMode.Compress);
                dflStream.Write(data, 0, data.Length);
                dflStream.Close();
                newData = memStream.ToArray();
            }
            // compute Adler-32:
            uint a1 = 1, a2 = 0;
            foreach (byte b in data)
            {
                a1 = (a1 + b) % 65521;
                a2 = (a2 + a1) % 65521;
            }
            // append the checksum-trailer:
            var adlerPos = newData.Length;
            Array.Resize(ref newData, adlerPos + 4);
            newData[adlerPos] = (byte)(a2 >> 8);
            newData[adlerPos + 1] = (byte)a2;
            newData[adlerPos + 2] = (byte)(a1 >> 8);
            newData[adlerPos + 3] = (byte)a1;
            return newData;
        }



    }
}
