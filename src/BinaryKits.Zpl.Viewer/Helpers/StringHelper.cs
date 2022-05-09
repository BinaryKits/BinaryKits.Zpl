using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BinaryKits.Zpl.Viewer.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// The hexadecimal indicator from the ^FH
        /// </summary>
        public static char ReplaceChar { get; set; } = '_';

        /// <summary>
        /// Search for the Hexadecimal indicator and replaces it with the HEX char
        /// </summary>
        /// <param name="text">String to search in</param>
        /// <returns>Output string</returns>
        public static string ReplaceSpecialChars(this string text)
        {
            if (!text.Contains(ReplaceChar))
                return text;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ReplaceChar)
                {
                    string temp = text.Substring(i + 1, 2);
                    sb.Append((char)Int16.Parse(temp, NumberStyles.AllowHexSpecifier));
                    i = i + 2;
                }
                else
                {
                    sb.Append(text[i]);
                }

            }
            return sb.ToString();
        }
    }
}
