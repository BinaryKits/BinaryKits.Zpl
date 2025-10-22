using BinaryKits.Zpl.Label.Helpers;
using System;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.Helpers
{
    internal static class ImageHelper
    {
        private static readonly Regex hexDataRegex = new("^[0-9A-Fa-f]+$", RegexOptions.Compiled);
        private static readonly Regex z64DataRegex = new(":(Z64):(\\S+):([0-9a-fA-F]+)", RegexOptions.Compiled);
        private static readonly Regex b64DataRegex = new(":(B64):(\\S+):([0-9a-fA-F]+)", RegexOptions.Compiled);

        public static byte[] GetImageBytes(string dataHex, int bytesPerRow)
        {
            if (z64DataRegex.IsMatch(dataHex))
            {
                return ZebraZ64CompressionHelper.Uncompress(dataHex);
            }

            if (b64DataRegex.IsMatch(dataHex))
            {
                return ZebraB64CompressionHelper.Uncompress(dataHex);
            }

            if (hexDataRegex.IsMatch(dataHex))
            {
                return dataHex.ToBytesFromHex();
            }

            return ZebraACSCompressionHelper.Uncompress(dataHex, bytesPerRow).ToBytesFromHex();
        }
    }
}
