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
                if (text[i] == ReplaceChar && doSubstringExists(text, i, 1, 2) && doSubstringExists(text, i, 4, 2) && text.Substring(i + 3, 1) == "_" ) 
                {
                    var buffer = new List<byte>();
                    string temp1 = text.Substring(i + 1, 2);
                    string temp2 = text.Substring(i + 4, 2);

                    buffer.Add(byte.Parse(temp1, System.Globalization.NumberStyles.HexNumber));
                    buffer.Add(byte.Parse(temp2, System.Globalization.NumberStyles.HexNumber));
                
                    sb.Append(Encoding.UTF8.GetString(buffer.ToArray()));
                    i = i + 5;
                }
                else if (text[i] == ReplaceChar && doSubstringExists(text, i, 1, 2) )
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
        
        private static bool doSubstringExists(string text, int iteration, int firstIndex, int secondIndex)
        {

            try {
                text.Substring(iteration + firstIndex, secondIndex);
            } catch (Exception) {
                return false;
            }

            return true;
            
        }
    }
}
