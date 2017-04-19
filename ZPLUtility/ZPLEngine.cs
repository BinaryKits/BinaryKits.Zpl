using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryKits.Utility.ZPLUtility
{
    public class ZPLEngine : List<ZPLElementBase>
    {
        /// <summary>
        /// Start an empty engine
        /// </summary>
        public ZPLEngine() { }

        /// <summary>
        /// Start an engine with given elements
        /// </summary>
        /// <param name="elements">ZPL elements to be added</param>
        public ZPLEngine(IEnumerable<ZPLElementBase> elements) : base(elements) { }

        /// <summary>
        /// Output the ZPL string using given context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Render(ZPLContext context)
        {
            List<string> result = new List<string>();
            result.Add("^XA");
            result.Add(context.ChangeInternationalFontEncoding);
            foreach (var e in this.Where(x => x.IsEnabled))
            {
                //Empty line
                if (context.AddEmptyLineBeforeElementStart)
                {
                    result.Add("");
                }

                //Comments
                if (context.DisplayComments)
                {
                    result.Add("^FX");
                    if (e.Comments.Any())
                    {
                        e.Comments.ForEach(x => result.Add("//" + x.Replace("^", "[caret]").Replace("~", "[tilde]")));
                    }
                }

                //Actual element
                result.AddRange(e.Render(context));
            }
            result.Add("^XZ");

            return string.Join("\n", result);
        }

        /// <summary>
        /// Add raw ZPL fragment
        /// </summary>
        /// <param name="rawZPLCode"></param>
        public void AddRawZPLCode(string rawZPLCode)
        {
            this.Add(new ZPLRaw(rawZPLCode));
        }

        /// <summary>
        /// Convert a char to be Hex value, 2 letters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public string MapToHexadecimalValue(char input)
        {
            return Convert.ToByte(input).ToString("X2");
        }
    }
}
