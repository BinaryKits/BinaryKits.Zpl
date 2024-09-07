using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.Helpers
{
    public static class StringHelper
    {
        static StringHelper()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // TODO set right code page this is determined by CI command which is now ignored, 850 is default when not set
            Charset = Encoding.GetEncoding(850);
        }

        /// <summary>
        /// Active charset, determined by CI command,
        /// when not present in label the default is assumed ^CI0 - code page 850.
        /// </summary>
        public static Encoding Charset { get; set; }

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
            var hexEscapeRegex = new Regex(Regex.Escape(ReplaceChar.ToString()) + @"([0-9A-Fa-f]{2})");
            return hexEscapeRegex.Replace(text, match => Charset.GetString([Convert.ToByte(match.Groups[1].Value, 16)]).ToString());
        }
    }
}
