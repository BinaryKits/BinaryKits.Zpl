// This file is part of PDFtoZPL which is released under MIT License.
// See https://github.com/sungaila/PDFtoZPL/blob/master/LICENSE for full license details.
// Source: https://github.com/sungaila/PDFtoZPL/blob/master/PDFtoZPL/Conversion.cs

using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryKits.Zpl.Label.Helpers
{
    /// <summary>
    /// Alternative Data Compression Scheme for ~DG and ~DB Commands
    /// There is an alternative data compression scheme recognized by the Zebra printer. This scheme further
    /// reduces the actual number of data bytes and the amount of time required to download graphic images and
    /// bitmapped fonts with the ~DG and ~DB commands
    /// </summary>
    public static class CompressHelper
    {
        /// <summary>
        /// The mapping table used for compression.
        /// Each character count (the key) is represented by a certain char (the value).
        /// </summary>
        private static readonly Dictionary<int, string> CompressionCountMapping = new Dictionary<int, string>()
        {
            {1, "G"}, {2, "H"}, {3, "I"}, {4, "J"}, {5, "K"}, {6, "L"}, {7, "M"}, {8, "N"}, {9, "O" }, {10, "P"},
            {11, "Q"}, {12, "R"}, {13, "S"}, {14, "T"}, {15, "U"}, {16, "V"}, {17, "W"}, {18, "X"}, {19, "Y"},
            {20, "g"}, {40, "h"}, {60, "i"}, {80, "j" }, {100, "k"}, {120, "l"}, {140, "m"}, {160, "n"}, {180, "o"}, {200, "p"},
            {220, "q"}, {240, "r"}, {260, "s"}, {280, "t"}, {300, "u"}, {320, "v"}, {340, "w"}, {360, "x"}, {380, "y"}, {400, "z" }
        };

        public static string CompressHex(string code, int widthBytes)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Invalid data", nameof(code));
            }

            int maxlinea = widthBytes * 2;
            var sbCode = new StringBuilder();
            var sbLinea = new StringBuilder();
            string previousLine = null;
            var counter = 0;
            char aux = code[0];
            var firstChar = false;

            foreach (char item in code)
            {
                if (firstChar)
                {
                    aux = item;
                    firstChar = false;
                    continue;
                }

                if (item == '\n')
                {
                    if (counter >= maxlinea && aux == '0')
                    {
                        sbLinea.Append(',');
                    }
                    else if (counter >= maxlinea && aux == 'F')
                    {
                        sbLinea.Append('!');
                    }
                    else if (counter > 20)
                    {
                        int multi20 = (counter / 20) * 20;
                        sbLinea.Append(CompressionCountMapping[Math.Min(multi20, 400)]);

                        int restover400 = multi20 / 400;
                        if (restover400 > 0)
                        {
                            for (; restover400 > 1; restover400--)
                            {
                                sbLinea.Append(CompressionCountMapping[400]);
                            }

                            int restto400 = (counter % 400) / 20 * 20;

                            if (restto400 > 0)
                            {
                                sbLinea.Append(CompressionCountMapping[restto400]);
                            }
                        }

                        int resto20 = (counter % 20);

                        if (resto20 != 0)
                        {
                            sbLinea.Append(CompressionCountMapping[resto20]).Append(aux);
                        }
                        else
                        {
                            sbLinea.Append(aux);
                        }
                    }
                    else
                    {
                        sbLinea.Append(CompressionCountMapping[counter]).Append(aux);
                    }

                    counter = 1;
                    firstChar = true;

                    if (sbLinea.ToString().Equals(previousLine))
                    {
                        sbCode.Append(':');
                    }
                    else
                    {
                        sbCode.Append(sbLinea);
                    }

                    previousLine = sbLinea.ToString();
                    sbLinea.Length = 0;
                    continue;
                }

                if (aux == item)
                {
                    counter++;
                }
                else
                {
                    if (counter > 20)
                    {
                        int multi20 = (counter / 20) * 20;
                        sbLinea.Append(CompressionCountMapping[Math.Min(multi20, 400)]);

                        int restover400 = multi20 / 400;
                        if (restover400 > 0)
                        {
                            for (; restover400 > 1; restover400--)
                            {
                                sbLinea.Append(CompressionCountMapping[400]);
                            }

                            int restto400 = (counter % 400) / 20 * 20;

                            if (restto400 > 0)
                            {
                                sbLinea.Append(CompressionCountMapping[restto400]);
                            }
                        }

                        int resto20 = (counter % 20);

                        if (resto20 != 0)
                        {
                            sbLinea.Append(CompressionCountMapping[resto20]).Append(aux);
                        }
                        else
                        {
                            sbLinea.Append(aux);
                        }
                    }
                    else
                    {
                        sbLinea.Append(CompressionCountMapping[counter]).Append(aux);
                    }

                    counter = 1;
                    aux = item;
                }
            }

            return sbCode.ToString();
        }
    }
}
