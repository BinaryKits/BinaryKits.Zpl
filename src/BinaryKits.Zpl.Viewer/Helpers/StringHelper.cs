using BinaryKits.Zpl.Label;

using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.Helpers
{
    public static class StringHelper
    {
        private static readonly Regex hexDigits = new(@"^[0-9A-Fa-f]{2}$", RegexOptions.Compiled);

        static StringHelper()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// Replaces hex escapes within a text.
        /// </summary>
        /// <param name="text">Topic variable</param>
        /// <param name="hexIndicator">Character which indicates a hex escape</param>
        /// <param name="internationalFont">Byte encoding for escaped hex</param>
        /// <returns>Text with hex escapes replaced with their char equivalents.</returns>
        public static string ReplaceHexEscapes(this string text, char hexIndicator, InternationalFont internationalFont)
        {
            Encoding charset = GetCharset(internationalFont);
            List<byte> bytes = [];
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == hexIndicator && i + 2 < text.Length)
                {
                    string digits = text.Substring(i + 1, 2);
                    if (hexDigits.IsMatch(digits))
                    {
                        bytes.Add(byte.Parse(digits, NumberStyles.HexNumber));
                        i += 2;
                        continue;
                    }
                }

                bytes.AddRange(charset.GetBytes(c.ToString()));
            }

            return charset.GetString(bytes.ToArray());
        }

        private static Encoding GetCharset(InternationalFont internationalFont)
        {
            return internationalFont switch
            {
                InternationalFont.UTF8 => Encoding.UTF8,
                InternationalFont.UTF16_BE => Encoding.BigEndianUnicode,
                InternationalFont.UTF16_LE => Encoding.Unicode,
                InternationalFont.ZCP1250 => Encoding.GetEncoding(1250),
                InternationalFont.ZCP1251 => Encoding.GetEncoding(1251),
                InternationalFont.ZCP1252 => Encoding.GetEncoding(1252),
                InternationalFont.ZCP1253 => Encoding.GetEncoding(1253),
                InternationalFont.ZCP1254 => Encoding.GetEncoding(1254),
                InternationalFont.ZCP1255 => Encoding.GetEncoding(1255),
                _ => Encoding.GetEncoding(850),
            };
        }
    }
}
