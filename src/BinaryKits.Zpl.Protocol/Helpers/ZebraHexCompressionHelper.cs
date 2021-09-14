using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryKits.Zpl.Protocol.Helpers
{
    /// <summary>
    /// Alternative Data Compression Scheme for ~DG and ~DB Commands
    /// There is an alternative data compression scheme recognized by the Zebra printer. This scheme further
    /// reduces the actual number of data bytes and the amount of time required to download graphic images and
    /// bitmapped fonts with the ~DG and ~DB commands
    /// </summary>
    public static class ZebraHexCompressionHelper
    {
        /// <summary>
        /// MinCompressionBlockCount (CompressionCountMapping -> g)
        /// </summary>
        const int MinCompressionBlockCount = 20;

        /// <summary>
        /// CompressionCountMapping (CompressionCountMapping -> z)
        /// </summary>
        const int MaxCompressionRepeatCount = 400;

        /// <summary>
        /// The mapping table used for compression.
        /// Each character count (the key) is represented by a certain char (the value).
        /// </summary>
        private static readonly Dictionary<int, char> CompressionCountMapping = new Dictionary<int, char>()
        {
            {1, 'G'}, {2, 'H'}, {3, 'I'}, {4, 'J'}, {5, 'K'}, {6, 'L'}, {7, 'M'}, {8, 'N'}, {9, 'O' }, {10, 'P'},
            {11, 'Q'}, {12, 'R'}, {13, 'S'}, {14, 'T'}, {15, 'U'}, {16, 'V'}, {17, 'W'}, {18, 'X'}, {19, 'Y'},
            {20, 'g'}, {40, 'h'}, {60, 'i'}, {80, 'j' }, {100, 'k'}, {120, 'l'}, {140, 'm'}, {160, 'n'}, {180, 'o'}, {200, 'p'},
            {220, 'q'}, {240, 'r'}, {260, 's'}, {280, 't'}, {300, 'u'}, {320, 'v'}, {340, 'w'}, {360, 'x'}, {380, 'y'}, {400, 'z' }
        };

        private static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize).Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        /// <summary>
        /// Compress hex
        /// </summary>
        /// <param name="hexData">Clean hex data 000000\nFFFFFF</param>
        /// <param name="bytesPerRow"></param>
        /// <returns></returns>
        public static string Compress(string hexData, int bytesPerRow)
        {
            var chunkSize = bytesPerRow * 2;
            var cleanedHexData = hexData.Replace("\n", string.Empty).Replace("\r", string.Empty);

            var compressedLines = new StringBuilder(hexData.Length);
            var compressedCurrentLine = new StringBuilder(chunkSize);
            var compressedPreviousLine = string.Empty;

            var dataLines = Split(cleanedHexData, chunkSize);
            foreach (var dataLine in dataLines)
            {
                var lastChar = dataLine[0];
                var charRepeatCount = 1;

                for (var i = 1; i < dataLine.Length; i++)
                {
                    if (dataLine[i] == lastChar)
                    {
                        charRepeatCount++;
                        continue;
                    }

                    //char changed within the line
                    compressedCurrentLine.Append(GetZebraCharCount(charRepeatCount));
                    compressedCurrentLine.Append(lastChar);

                    lastChar = dataLine[i];
                    charRepeatCount = 1;
                }

                //process last char in dataLine
                if (lastChar.Equals('0'))
                {
                    //fills the line, to the right, with zeros
                    compressedCurrentLine.Append(',');
                }
                else if (lastChar.Equals('1'))
                {
                    //fills the line, to the right, with ones
                    compressedCurrentLine.Append('!');
                }
                else
                {
                    compressedCurrentLine.Append(GetZebraCharCount(charRepeatCount));
                    compressedCurrentLine.Append(lastChar);
                }

                if (compressedCurrentLine.Equals(compressedPreviousLine))
                {
                    //previous line is repeated
                    compressedLines.Append(':');
                }
                else
                {
                    compressedLines.Append(compressedCurrentLine);
                }

                compressedPreviousLine = compressedCurrentLine.ToString();
                compressedCurrentLine.Clear();
            }

            return compressedLines.ToString();
        }

        /// <summary>
        /// Uncompress data
        /// </summary>
        /// <param name="compressedHexData"></param>
        /// <param name="bytesPerRow"></param>
        /// <returns></returns>
        public static string Uncompress(string compressedHexData, int bytesPerRow)
        {
            var hexData = new StringBuilder();
            var chunkSize = bytesPerRow * 2;
            var lineIndex = 0;
            var totalCount = 0;

            var reverseMapping = CompressionCountMapping.ToDictionary(o => o.Value, o => o.Key);

            foreach (var c in compressedHexData)
            {
                if (c.Equals(':'))
                {
                    var appendRepeat = hexData.ToString().Substring(hexData.Length - (chunkSize + 1));
                    hexData.Append(appendRepeat);
                    continue;
                }

                if (c.Equals(','))
                {
                    var appendZero = new string('0', chunkSize - lineIndex);
                    hexData.Append(appendZero);
                    hexData.Append("\n");
                    lineIndex = 0;
                    continue;
                }

                if (c.Equals('!'))
                {
                    var appendOne = new string('1', chunkSize - lineIndex);
                    hexData.Append(appendOne);
                    hexData.Append("\n");
                    lineIndex = 0;
                    continue;
                }

                if (reverseMapping.TryGetValue(c, out var count))
                {
                    totalCount += count;
                    continue;
                }

                if (totalCount == 0)
                {
                    totalCount = 1;
                }

                var append = new string(c, totalCount);
                hexData.Append(append);
                lineIndex += append.Length;
                totalCount = 0;

                if (lineIndex == chunkSize)
                {
                    hexData.Append("\n");
                    lineIndex = 0;
                }
            }

            return hexData.ToString();
        }

        /// <summary>
        /// Get Zebra char repeat count
        /// </summary>
        /// <param name="charRepeatCount"></param>
        /// <returns></returns>
        public static string GetZebraCharCount(int charRepeatCount)
        {
            if (CompressionCountMapping.TryGetValue(charRepeatCount, out var compressionKey))
            {
                return $"{compressionKey}";
            }

            var compressionKeys = new StringBuilder(5);

            var multi20 = charRepeatCount / MinCompressionBlockCount * MinCompressionBlockCount;
            var remainder = charRepeatCount % multi20;

            while (remainder > 0 || multi20 > 0)
            {
                if (multi20 > MaxCompressionRepeatCount)
                {
                    remainder += multi20 - MaxCompressionRepeatCount;
                    multi20 = MaxCompressionRepeatCount;
                }

                if (!CompressionCountMapping.TryGetValue(multi20, out compressionKey))
                {
                    throw new Exception("Compression failure");
                }

                compressionKeys.Append(compressionKey);

                if (remainder == 0)
                {
                    break;
                }

                if (remainder <= 20)
                {
                    CompressionCountMapping.TryGetValue(remainder, out compressionKey);
                    compressionKeys.Append(compressionKey);
                    break;
                }

                multi20 = remainder / MinCompressionBlockCount * MinCompressionBlockCount;
                remainder %= multi20;
            }

            return compressionKeys.ToString();
        }
    }
}
