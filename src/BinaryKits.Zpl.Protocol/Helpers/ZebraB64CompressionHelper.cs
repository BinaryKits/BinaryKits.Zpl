using System;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Protocol.Helpers
{
    public static class ZebraB64CompressionHelper
    {
        private static Regex _B64Regex;
        static ZebraB64CompressionHelper()
        {
            _B64Regex = new Regex(":(B64):(\\S+):([0-9a-fA-F]+)", RegexOptions.Compiled);
        }

        public static string Compress(string hexData)
        {
            var cleanedHexData = hexData.Replace("\n", string.Empty).Replace("\r", string.Empty);
            return Compress(cleanedHexData.ToBytes());
        }
        public static string Compress(byte[] bytes)
        {
            var base64 = Convert.ToBase64String(bytes);
            return ":B64:" + base64 + ":" + Crc16.ComputeChecksum(base64);

        }
        public static byte[] Uncompress(string hexData)
        {
            var match = _B64Regex.Match(hexData);
            if (match.Success)
            {
                var imageBase64 = match.Groups[2].Value;
                return Convert.FromBase64String(imageBase64);
            }
            else
            {
                throw new FormatException("Hex string not in B64 format");
            }
        }

    }
}
