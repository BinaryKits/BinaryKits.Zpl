using System;
using System.Linq;
using System.Text;

namespace BinaryKits.Zpl.Protocol.Helpers
{
    public static class ByteHelper
    {
        public static string ToBase64(this byte[] bytes) { return Convert.ToBase64String(bytes); }

        public static byte[] FromBase64(this string base64) { return Convert.FromBase64String(base64); }

        public static string ToHexFromBytes(this byte[] bytes) { return BytesToHex(bytes); }

        public static byte[] ToBytesFromHex(this string hex) { return HexToBytes(hex); }

        public static byte[] EncodeBytes(this string hex) { return Encoding.ASCII.GetBytes(hex); }

        public static byte[] HexToBytes(string hex)
        {
            if (hex.IndexOfAny(new[] { '\r', '\n' }) != -1)
            {
                hex = hex.Replace("\n", string.Empty);
                hex = hex.Replace("\r", string.Empty);
            }
            //Not Available on .NET standard and has much greater performance due to Vectorization. 
#if NET5_0_OR_GREATER
           return Convert.FromHexString(hex);
#else
            if (hex.Length % 2 == 1)
            {
                throw new Exception("The binary key cannot have an odd number of digits");
            }

            var array = new byte[hex.Length >> 1];
            for (var i = 0; i < hex.Length >> 1; ++i)
            {
                array[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + GetHexVal(hex[(i << 1) + 1]));
            }

            return array;
#endif
        }

        public static string BytesToHex(byte[] bytes)
        {
            //Not Available on .NET standard and has much greater performance due to Vectorization. 
#if NET5_0_OR_GREATER
            return Convert.ToHexString(bytes);
#else
            var c = new char[bytes.Length * 2];
            int b;
            for (var i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (b - 10 >> 31 & -7));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (b - 10 >> 31 & -7));
            }

            return new string(c);
#endif
        }

        private static int GetHexVal(char hex)
        {
            int val = hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : val < 97 ? 55 : 87);
        }
    }
}
