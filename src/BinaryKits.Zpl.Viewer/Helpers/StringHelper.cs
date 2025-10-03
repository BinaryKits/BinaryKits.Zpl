using System;
using System.Collections.Generic;
using System.Globalization;
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

        private static readonly Regex hexDigits = new Regex(@"^[0-9A-Fa-f]{2}$", RegexOptions.Compiled);

        /// <summary>
        /// Replaces hex escapes within a text.
        /// </summary>
        /// <param name="text">Topic variable</param>
        /// <param name="hexIndicator">Character which indicates a hex escape</param>
        /// <returns>Text with hex escapes replaced with their char equivalents.</returns>
        public static string ReplaceHexEscapes(this string text, char hexIndicator)
        {
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

                bytes.Add((byte)c);
            }

            return Charset.GetString(bytes.ToArray());
        }
    }
}
