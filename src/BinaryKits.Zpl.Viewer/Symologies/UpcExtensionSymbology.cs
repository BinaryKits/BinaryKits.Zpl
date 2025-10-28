using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryKits.Zpl.Viewer.Symologies
{
    public class UpcExtensionSymbology
    {

        private static readonly Dictionary<int, int[]> patternMap;
        private static readonly Dictionary<int, int> parity5Map;

        static UpcExtensionSymbology()
        {
            var codeSetTable = new[] {
                new { Value = 0, Pattern = new int[] { 0b0001101, 0b0100111 }, Parity5 = 0b11000 },
                new { Value = 1, Pattern = new int[] { 0b0011001, 0b0110011 }, Parity5 = 0b10100 },
                new { Value = 2, Pattern = new int[] { 0b0010011, 0b0011011 }, Parity5 = 0b10010 },
                new { Value = 3, Pattern = new int[] { 0b0111101, 0b0100001 }, Parity5 = 0b10001 },
                new { Value = 4, Pattern = new int[] { 0b0100011, 0b0011101 }, Parity5 = 0b01100 },
                new { Value = 5, Pattern = new int[] { 0b0110001, 0b0111001 }, Parity5 = 0b00110 },
                new { Value = 6, Pattern = new int[] { 0b0101111, 0b0000101 }, Parity5 = 0b00011 },
                new { Value = 7, Pattern = new int[] { 0b0111011, 0b0010001 }, Parity5 = 0b01010 },
                new { Value = 8, Pattern = new int[] { 0b0110111, 0b0001001 }, Parity5 = 0b01001 },
                new { Value = 9, Pattern = new int[] { 0b0001011, 0b0010111 }, Parity5 = 0b00101 }
            };

            patternMap = codeSetTable.ToDictionary(item => item.Value, item => item.Pattern);
            parity5Map = codeSetTable.ToDictionary(item => item.Value, item => item.Parity5);
        }

        public static bool[] Encode(string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            else if (content.Length != 2 && content.Length != 5)
            {
                throw new ArgumentException("Barcode content length must be 2 or 5 for UPC Extension.");
            }

            if (!int.TryParse(content, out int value))
            {
                throw new ArgumentException("Barcode content must be numeric for UPC Extension.");
            }

            List<bool> result = [];
            int type = content.Length;
            int[] digits = IntToDigits(value, type);
            bool[] parities = [];

            if (type == 2)
            {
                parities = IntToBitArray(value % 4, type);
            }
            else
            {
                int parity = 0;
                int coeff = 3;
                foreach(int digit in digits)
                {
                    parity += digit * coeff;
                    coeff ^= 10;
                }

                parities = IntToBitArray(parity5Map[parity % 10], type);
            }

            result.AddRange([true, false, true, true]);

            for (int i = 0; i < type; i++) {
                int pattern = patternMap[digits[i]][parities[i] ? 1 : 0];
                result.AddRange(IntToBitArray(pattern, 7));

                if (i < type - 1)
                {
                    result.AddRange([false, true]);
                }
            }

            return result.ToArray();
        }

        private static bool[] IntToBitArray(int value, int length)
        {
            Stack<bool> stack = [];
            for(int i = 0; i < length; i++)
            {
                stack.Push((value & 1) == 1);
                value >>= 1;
            }

            return stack.ToArray();
        }

        private static int[] IntToDigits(int value, int length)
        {
            Stack<int> stack = [];
            for (int i = 0; i < length; i++)
            {
                stack.Push(value % 10);
                value /= 10;
            }

            return stack.ToArray();
        }

    }
}
