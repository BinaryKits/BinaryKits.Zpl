using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.Helpers
{
    public static class StringHelper
    {
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
            Regex hexEscapeRegex = new Regex(Regex.Escape(ReplaceChar.ToString()) + @"([0-9A-Fa-f]{2})");
            return hexEscapeRegex.Replace(text, match => Convert.ToChar(int.Parse(match.Groups[1].Value, NumberStyles.HexNumber)).ToString());
        }
    }
}
