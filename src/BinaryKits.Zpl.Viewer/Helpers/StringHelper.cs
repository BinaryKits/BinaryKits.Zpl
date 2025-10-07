using BinaryKits.Zpl.Label;

using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.Helpers
{
    public static class StringHelper
    {
        private static readonly Regex hexDigits = new Regex(@"^[0-9A-Fa-f]{2}$", RegexOptions.Compiled);

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
            List<byte> bytes = new List<byte>();
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
            switch (internationalFont)
            {
                case InternationalFont.UTF8:
                    return Encoding.UTF8;
                case InternationalFont.UTF16_BE:
                    return Encoding.BigEndianUnicode;
                case InternationalFont.UTF16_LE:
                    return Encoding.Unicode;
                case InternationalFont.ZCP1250:
                    return Encoding.GetEncoding(1250);
                case InternationalFont.ZCP1251:
                    return Encoding.GetEncoding(1251);
                case InternationalFont.ZCP1252:
                    return Encoding.GetEncoding(1252);
                case InternationalFont.ZCP1253:
                    return Encoding.GetEncoding(1253);
                case InternationalFont.ZCP1254:
                    return Encoding.GetEncoding(1254);
                case InternationalFont.ZCP1255:
                    return Encoding.GetEncoding(1255);
                default:
                    return Encoding.GetEncoding(850);
            }
        }
    }
}
