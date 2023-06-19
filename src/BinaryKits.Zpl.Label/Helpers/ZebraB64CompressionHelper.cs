using System;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Label.Helpers
{
    /// <summary>
    /// B64 Data Compression Scheme for ~DG and ~DB Commands Raw data is encoded using Base64 A CRC is calculated across
    /// the Base64-encoded data. If the CRC-check fails or the download is aborted, the object can be invalidated by the
    /// printer. This scheme is not very efficient and only added for compatibility reasons.
    /// </summary>
    public static class ZebraB64CompressionHelper
    {
        private static Regex _b64Regex = new Regex(":(B64):(\\S+):([0-9a-fA-F]+)", RegexOptions.Compiled);

        public static string Compress(string hexData)
        {
            var cleanedHexData = hexData.Replace("\n", string.Empty).Replace("\r", string.Empty);
            return Compress(cleanedHexData.ToBytesFromHex());
        }

        public static string Compress(byte[] bytes)
        {
            var base64 = Convert.ToBase64String(bytes);
            var crc16 = Crc16.ComputeHex(base64.EncodeBytes());
            return ":B64:" + base64 + ":" + crc16;
        }

        public static byte[] Uncompress(string hexData)
        {
            var match = _b64Regex.Match(hexData);
            if (match.Success)
            {
                var imageBase64 = match.Groups[2].Value;
                return imageBase64.FromBase64();
            }
            else
            {
                throw new FormatException("Hex string not in B64 format");
            }
        }
    }
}
