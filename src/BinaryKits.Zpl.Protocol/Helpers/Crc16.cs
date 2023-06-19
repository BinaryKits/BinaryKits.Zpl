using System;
using System.Linq;

namespace BinaryKits.Zpl.Protocol.Helpers
{
    /// <summary>
    /// Calculate Crc16 Checksum.
    /// </summary>
    internal static class Crc16
    {
        private static ushort[] crc16_table = {
           0x0000, 0x1021, 0x2042, 0x3063,
           0x4084, 0x50A5, 0x60C6, 0x70E7,
           0x8108, 0x9129, 0xA14A, 0xB16B,
           0xC18C, 0xD1AD, 0xE1CE, 0xF1EF
        };

        public static ushort Compute(byte[] bytes)
        {
            int crc = 0;
            foreach (byte b in bytes)
            {
                crc ^= (b << 8);
                crc = (crc << 4) ^ crc16_table[(crc >> 12) & 0xF];
                crc = (crc << 4) ^ crc16_table[(crc >> 12) & 0xF];
            }

            return (ushort)(crc & 0xFFFF);
        }

        public static string ComputeHex(byte[] bytes)
        {
            return Compute(bytes).ToString("x4");
        }

    }
}
