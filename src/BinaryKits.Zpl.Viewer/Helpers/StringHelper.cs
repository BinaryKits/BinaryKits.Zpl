using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.Helpers
{
    public static class StringHelper
    {
        private static readonly Regex hexDigits = new Regex(@"^[0-9A-Fa-f]{2}$", RegexOptions.Compiled);

        /// <summary>
        /// The hexadecimal indicator from the ^FH
        /// </summary>
        public static char ReplaceChar { get; set; } = '_';

        /// <summary>
        /// Replaces hex escapes within a text.
        /// </summary>
        /// <param name="text">Topic variable</param>
        /// <returns>Text with hex escapes replaced with their char equivalents.</returns>
        public static string ReplaceHexEscapes(this string text)
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == ReplaceChar && i + 2 < text.Length)
                {
                    string digits = text.Substring(i + 1, 2);
                    if (hexDigits.IsMatch(digits))
                    {
                        bytes.Add(byte.Parse(digits, NumberStyles.HexNumber));
                        i += 2;
                        continue;
                    }
                }

                bytes.Add((byte)c);
            }

            return Encoding.UTF8.GetString(bytes.ToArray());
        }
    }
}
