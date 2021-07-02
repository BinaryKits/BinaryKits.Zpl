// This file is part of PDFtoZPL which is released under MIT License.
// See https://github.com/sungaila/PDFtoZPL/blob/master/LICENSE for full license details.
// Source: https://github.com/sungaila/PDFtoZPL/blob/master/PDFtoZPL/Conversion.cs

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryKits.ZplUtility.Helper
{
    public static class ImageHelper

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

        public static string ConvertBitmap(byte[] imageData, out int binaryByteCount, out int bytesPerRow)
        {
            var zplBuilder = new StringBuilder();

            using (Image<Rgba32> image = Image.Load(imageData))
            {
                bytesPerRow = image.Width % 8 > 0
                    ? image.Width / 8 + 1
                    : image.Width / 8;

                binaryByteCount = image.Height * bytesPerRow;

                var colorBits = 0;
                var j = 0;

                for (var y = 0; y < image.Height; y++)
                {
                    var row = image.GetPixelRowSpan(y);

                    for (var x = 0; x < image.Width; x++)
                    {
                        var pixel = row[x];

                        var isBlackPixel = ((pixel.R + pixel.G + pixel.B) / 3) < 128;
                        if (isBlackPixel)
                        {
                            colorBits |= 1 << (7 - j);
                        }

                        j++;

                        if (j == 8 || x == (image.Width - 1))
                        {
                            zplBuilder.Append(colorBits.ToString("X2"));
                            colorBits = 0;
                            j = 0;
                        }
                    }
                    zplBuilder.Append('\n');
                }                
            }

            return zplBuilder.ToString();
        }

        public static string CompressHex(string code, int widthBytes)
        {
            int maxlinea = widthBytes * 2;
            var sbCode = new StringBuilder();
            var sbLinea = new StringBuilder();
            string previousLine = null;
            int counter = 0;
            char aux = code[0];
            bool firstChar = false;

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
